using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class DbUpdateForm : Form
    {
        private string _dbPath;
        private string connectionString;
        private string _selectedScriptPath = "";

        public DbUpdateForm(string dbPath)
        {
            InitializeComponent();
            _dbPath = dbPath;
            connectionString = $"Data Source={dbPath};Version=3;";

            // Иконки на кнопки
            btnUpdateStructure.Image = Properties.Resources.repare_db;
            btnVacuum.Image = Properties.Resources.save;
            btnCheckIntegrity.Image = Properties.Resources.check;
            btnClose.Image = Properties.Resources.exit;
            btnSelectScript.Image = Properties.Resources.open;
            btnRefreshLog.Image = Properties.Resources.refresh;
            btnClearLog.Image = Properties.Resources.delete;

            ChurchBudget.ImageHelper.ApplyToButtons(this, 24);

            // По умолчанию выбран update.sql
            _selectedScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "update.sql");
            txtScriptPath.Text = File.Exists(_selectedScriptPath) ? _selectedScriptPath : "(файл не найден)";

            LoadLogHistory();
        }

        #region Логирование

        private void LogOperation(string operationType, string description, string result, string errorMessage = "", string details = "")
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"INSERT INTO db_maintenance_log 
                        (operation_type, operation_date, description, result, error_message, user_name, details) 
                        VALUES (@type, @date, @desc, @result, @error, @user, @details)";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@type", operationType);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@desc", description);
                        cmd.Parameters.AddWithValue("@result", result);
                        cmd.Parameters.AddWithValue("@error", errorMessage ?? "");
                        cmd.Parameters.AddWithValue("@user", Environment.UserName);
                        cmd.Parameters.AddWithValue("@details", details ?? "");
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadLogHistory();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка логирования: {ex.Message}");
            }
        }

        private void LoadLogHistory()
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    var tableCheck = new SQLiteCommand(
                        "SELECT name FROM sqlite_master WHERE type='table' AND name='db_maintenance_log'",
                        conn).ExecuteScalar();

                    if (tableCheck == null)
                    {
                        dgvLogHistory.DataSource = null;
                        lblLogInfo.Text = "Таблица логов не создана. Примените скрипт create_log_table.sql";
                        return;
                    }

                    string sql = @"SELECT 
                        operation_date AS [Дата],
                        operation_type AS [Тип],
                        description AS [Описание],
                        result AS [Результат],
                        user_name AS [Пользователь]
                        FROM db_maintenance_log 
                        ORDER BY id DESC 
                        LIMIT 50";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvLogHistory.DataSource = dt;

                            dgvLogHistory.ReadOnly = true;
                            dgvLogHistory.AllowUserToAddRows = false;
                            dgvLogHistory.AllowUserToDeleteRows = false;
                            dgvLogHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            dgvLogHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                            dgvLogHistory.RowHeadersVisible = false;

                            foreach (DataGridViewRow row in dgvLogHistory.Rows)
                            {
                                string result = row.Cells["Результат"].Value?.ToString();
                                if (result == "SUCCESS")
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                                else if (result == "ERROR")
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                            }

                            var countCmd = new SQLiteCommand("SELECT COUNT(*) FROM db_maintenance_log", conn);
                            int total = Convert.ToInt32(countCmd.ExecuteScalar());
                            lblLogInfo.Text = $"Записей в журнале: {total}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Автоматический бэкап

        private string CreateBackupBeforeUpdate()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string archiveFolder = Path.Combine(baseDir, "Data", "Archive");
                if (!Directory.Exists(archiveFolder))
                    Directory.CreateDirectory(archiveFolder);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string backupFileName = $"church_before_update_{timestamp}.db";
                string backupPath = Path.Combine(archiveFolder, backupFileName);

                File.Copy(_dbPath, backupPath, true);

                // Удаляем старые бэкапы (оставляем последние 10)
                var files = new DirectoryInfo(archiveFolder).GetFiles("church_before_update_*.db");
                var filesToDelete = files.AsEnumerable()
                    .OrderByDescending(f => f.CreationTime)
                    .Skip(10)
                    .ToList();

                foreach (var file in filesToDelete) file.Delete();

                return backupPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка бэкапа: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region Операции с БД

        private void btnSelectScript_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "SQL-скрипты (*.sql)|*.sql|Все файлы (*.*)|*.*";
                ofd.Title = "Выберите SQL-скрипт для применения";
                ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedScriptPath = ofd.FileName;
                    txtScriptPath.Text = _selectedScriptPath;
                }
            }
        }

        private void btnUpdateStructure_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedScriptPath) || !File.Exists(_selectedScriptPath))
            {
                MessageBox.Show(
                    $"Файл скрипта не найден:\n{_selectedScriptPath}\n\nВыберите файл через кнопку «Выбрать...»",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Будет применён скрипт:\n{Path.GetFileName(_selectedScriptPath)}\n\n" +
                $"Автоматически будет создана резервная копия БД.\n\nПродолжить?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes) return;

            // ПРИНУДИТЕЛЬНЫЙ БЭКАП ПЕРЕД ОБНОВЛЕНИЕМ
            string backupPath = CreateBackupBeforeUpdate();
            if (backupPath == null)
            {
                MessageBox.Show(
                    "Не удалось создать резервную копию БД!\n\nОперация отменена для безопасности.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogOperation("BACKUP", "Автоматический бэкап перед обновлением", "ERROR", "Не удалось создать копию", "");
                return;
            }

            string scriptContent = "";
            try
            {
                scriptContent = File.ReadAllText(_selectedScriptPath, Encoding.UTF8);

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        using (var cmd = new SQLiteCommand(scriptContent, conn, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }

                MessageBox.Show(
                    $"Скрипт успешно применён!\n\n" +
                    $"Резервная копия создана:\n{Path.GetFileName(backupPath)}",
                    "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogOperation("UPDATE", $"Применён скрипт: {Path.GetFileName(_selectedScriptPath)}",
                    "SUCCESS", "", $"Бэкап: {Path.GetFileName(backupPath)}");
            }
            catch (SQLiteException sqliteEx)
            {
                string errorMsg = sqliteEx.Message;
                if (errorMsg.Contains("duplicate column name"))
                {
                    MessageBox.Show(
                        "Поле уже существует в таблице!\n\nЭто нормально — база уже обновлена.\n\n" +
                        $"Резервная копия сохранена:\n{Path.GetFileName(backupPath)}",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogOperation("UPDATE", $"Применение скрипта: {Path.GetFileName(_selectedScriptPath)}",
                        "SUCCESS", "", "Поля уже существуют");
                }
                else if (errorMsg.Contains("unique constraint failed") || errorMsg.Contains("Запись с таким ключом уже существует"))
                {
                    MessageBox.Show(
                        "Нарушение уникальности!\n\nПроверьте SQL-скрипт — используйте INSERT OR IGNORE вместо INSERT.\n\n" +
                        $"Резервная копия сохранена:\n{Path.GetFileName(backupPath)}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LogOperation("UPDATE", $"Применение скрипта: {Path.GetFileName(_selectedScriptPath)}",
                        "ERROR", errorMsg, "");
                }
                else
                {
                    MessageBox.Show(
                        $"Ошибка SQLite:\n{errorMsg}\n\n" +
                        $"Резервная копия сохранена:\n{Path.GetFileName(backupPath)}\n" +
                        "При необходимости можно восстановить БД из этой копии.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogOperation("UPDATE", $"Применение скрипта: {Path.GetFileName(_selectedScriptPath)}",
                        "ERROR", errorMsg, "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при применении скрипта:\n{ex.Message}\n\n" +
                    $"Резервная копия сохранена:\n{Path.GetFileName(backupPath)}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogOperation("UPDATE", $"Применение скрипта: {Path.GetFileName(_selectedScriptPath)}",
                    "ERROR", ex.Message, "");
            }
        }

        private void btnVacuum_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Оптимизация БД (VACUUM) сжимает базу и ускоряет работу.\n\nЭто может занять несколько секунд.\n\nПродолжить?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                if (!File.Exists(_dbPath))
                {
                    MessageBox.Show($"Файл БД не найден:\n{_dbPath}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogOperation("VACUUM", "Оптимизация БД", "ERROR", "Файл не найден", _dbPath);
                    return;
                }

                long sizeBefore = new FileInfo(_dbPath).Length;

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("VACUUM", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                long sizeAfter = new FileInfo(_dbPath).Length;
                long saved = sizeBefore - sizeAfter;

                string message = saved > 0
                    ? $"Оптимизация завершена!\n\nРазмер до: {sizeBefore / 1024} КБ\nРазмер после: {sizeAfter / 1024} КБ\nОсвобождено: {saved / 1024} КБ"
                    : $"Оптимизация завершена!\n\nРазмер БД: {sizeAfter / 1024} КБ\nДополнительное сжатие не потребовалось.";

                MessageBox.Show(message, "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogOperation("VACUUM", "Оптимизация БД", "SUCCESS", "", $"До: {sizeBefore / 1024} КБ, После: {sizeAfter / 1024} КБ");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оптимизации:\n{ex.Message}\n\nПуть: {_dbPath}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogOperation("VACUUM", "Оптимизация БД", "ERROR", ex.Message, _dbPath);
            }
        }

        private void btnCheckIntegrity_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder report = new StringBuilder();
                report.AppendLine("Отчёт о проверке целостности БД:");
                report.AppendLine("=====================================");
                report.AppendLine();

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    using (var cmd = new SQLiteCommand("PRAGMA integrity_check", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result.ToString() == "ok")
                        {
                            report.AppendLine("✓ Целостность БД: OK");
                        }
                        else
                        {
                            report.AppendLine("✗ Целостность БД: ОШИБКА");
                            report.AppendLine($"  Детали: {result}");
                            LogOperation("CHECK", "Проверка целостности", "ERROR", result.ToString(), "");
                        }
                    }

                    using (var cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            report.AppendLine();
                            report.AppendLine("Таблицы в базе данных:");
                            int tableCount = 0;
                            while (reader.Read())
                            {
                                tableCount++;
                                report.AppendLine($"  • {reader["name"]}");
                            }
                            report.AppendLine();
                            report.AppendLine($"Всего таблиц: {tableCount}");
                        }
                    }

                    if (File.Exists(_dbPath))
                    {
                        long dbSize = new FileInfo(_dbPath).Length;
                        report.AppendLine();
                        report.AppendLine($"Размер БД: {dbSize / 1024} КБ ({dbSize / (1024 * 1024)} МБ)");
                    }
                }

                MessageBox.Show(report.ToString(), "Результат проверки",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogOperation("CHECK", "Проверка целостности", "SUCCESS", "", report.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogOperation("CHECK", "Проверка целостности", "ERROR", ex.Message, "");
            }
        }

        #endregion

        #region Управление историей

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Очистить журнал операций?\n\nЭто действие нельзя отменить.",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand("DELETE FROM db_maintenance_log", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadLogHistory();
                MessageBox.Show("Журнал очищен.", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка очистки: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefreshLog_Click(object sender, EventArgs e)
        {
            LoadLogHistory();
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}