using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class CleanDatabaseForm : Form
    {
        private DatabaseCleanService _service;
        private Dictionary<string, int> _tableCounts;

        public CleanDatabaseForm(string connectionString)
        {
            InitializeComponent();
            _service = new DatabaseCleanService(connectionString);
            LoadStatistics();
        }

        private void CleanDatabaseForm_Load(object sender, EventArgs e)
        {
            ImageHelper.ApplyToButtons(this, 24);

            // Устанавливаем режим по умолчанию
            rbDocumentsOnly.Checked = true;
            UpdateCheckboxes();
        }

        private void LoadStatistics()
        {
            try
            {
                _tableCounts = _service.GetTableCounts();

                chkIncomeDocs.Text = string.Format("Документы доходов ({0} записей)", _tableCounts["income_docs"]);
                chkExpenseDocs.Text = string.Format("Документы расходов ({0} записей)", _tableCounts["expense_docs"]);
                chkCashOrders.Text = string.Format("Кассовые ордера ({0} записей)", _tableCounts["cash_orders"]);
                chkPersonal.Text = string.Format("Сотрудники ({0} записей)", _tableCounts["personal"]);
                chkIdDocs.Text = string.Format("ИД документы ({0} записей)", _tableCounts["id_documents"]);

                // Добавьте, если есть чекбокс для организаций
                if (_tableCounts.ContainsKey("organizations"))
                {
                    chkOrganizations.Text = string.Format("Организации ({0} записей)", _tableCounts["organizations"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки статистики: " + ex.Message);
            }
        }

        private void rbDocumentsOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDocumentsOnly.Checked)
                UpdateCheckboxes();
        }

        private void rbDirectories_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDirectories.Checked)
                UpdateCheckboxes();
        }

        private void rbFullClean_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFullClean.Checked)
                UpdateCheckboxes();
        }

        private void UpdateCheckboxes()
        {
            if (rbDocumentsOnly.Checked)
            {
                chkIncomeDocs.Checked = true;
                chkExpenseDocs.Checked = true;
                chkCashOrders.Checked = true;
                chkPersonal.Checked = false;
                chkIdDocs.Checked = false;
                chkOrganizations.Checked = false;  // Добавьте эту строку

                chkPersonal.Enabled = false;
                chkIdDocs.Enabled = false;
                chkOrganizations.Enabled = false;  // Добавьте эту строку
            }
            else if (rbDirectories.Checked)
            {
                chkIncomeDocs.Checked = true;
                chkExpenseDocs.Checked = true;
                chkCashOrders.Checked = true;
                chkPersonal.Checked = true;
                chkIdDocs.Checked = true;
                chkOrganizations.Checked = false;  // Организации НЕ очищаем в этом режиме

                chkPersonal.Enabled = true;
                chkIdDocs.Enabled = true;
                chkOrganizations.Enabled = false;  // Заблокируем
            }
            else if (rbFullClean.Checked)
            {
                chkIncomeDocs.Checked = true;
                chkExpenseDocs.Checked = true;
                chkCashOrders.Checked = true;
                chkPersonal.Checked = true;
                chkIdDocs.Checked = true;
                chkOrganizations.Checked = true;  // ✅ Полная очистка включает организации!

                chkPersonal.Enabled = true;
                chkIdDocs.Enabled = true;
                chkOrganizations.Enabled = true;  // Разблокируем
            }
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            // Проверка подтверждения
            if (txtConfirm.Text.ToUpper() != "УДАЛИТЬ")
            {
                MessageBox.Show("Для подтверждения введите слово 'УДАЛИТЬ' (заглавными буквами)",
                    "Подтверждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirm.Focus();
                return;
            }

            // Предупреждение
            DialogResult result = MessageBox.Show(
                "ВНИМАНИЕ!\n\nБудет создана резервная копия перед очисткой.\n" +
                "Это действие НЕЛЬЗЯ отменить!\n\nПродолжить?",
                "Подтверждение очистки",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                rtbLog.Clear();
                rtbLog.AppendText("=== Начало очистки ===\n");
                rtbLog.AppendText($"Время: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n\n");

                // 1. Создаём бэкап
                rtbLog.AppendText("Создание резервной копии...\n");
                string backupPath = _service.CreateBackup();
                if (backupPath != null)
                {
                    rtbLog.AppendText($"✓ Бэкап создан: {backupPath}\n\n");
                }
                else
                {
                    rtbLog.AppendText("✗ Не удалось создать бэкап\n\n");
                }

                // 2. Очищаем документы
                if (chkIncomeDocs.Checked || chkExpenseDocs.Checked || chkCashOrders.Checked)
                {
                    rtbLog.AppendText("Очистка документов...\n");
                    var log = _service.CleanDocuments(
                        chkIncomeDocs.Checked,
                        chkExpenseDocs.Checked,
                        chkCashOrders.Checked
                    );
                    foreach (string line in log)
                        rtbLog.AppendText(line + "\n");
                    rtbLog.AppendText("\n");
                }

                // 3. Очищаем справочники
                if (chkPersonal.Checked || chkIdDocs.Checked)
                {
                    rtbLog.AppendText("Очистка справочников...\n");
                    var log = _service.CleanDirectories(
                        chkPersonal.Checked,
                        chkIdDocs.Checked
                    );
                    foreach (string line in log)
                        rtbLog.AppendText(line + "\n");
                    rtbLog.AppendText("\n");
                }

                // 4. Очищаем организации (только при полной очистке)
                if (chkOrganizations.Checked)
                {
                    rtbLog.AppendText("Очистка справочника организаций...\n");
                    var log = _service.CleanOrganizations();
                    foreach (string line in log)
                        rtbLog.AppendText(line + "\n");
                    rtbLog.AppendText("\n");
                }

                rtbLog.AppendText("=== Очистка завершена ===\n");

                MessageBox.Show("Очистка успешно завершена!\n\n" +
                    $"Резервная копия сохранена:\n{backupPath}",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Обновляем статистику
                LoadStatistics();
                txtConfirm.Clear();
            }
            catch (Exception ex)
            {
                rtbLog.AppendText($"\n✗ КРИТИЧЕСКАЯ ОШИБКА: {ex.Message}\n");
                MessageBox.Show("Ошибка при очистке:\n" + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            LoadStatistics();
            MessageBox.Show("Статистика обновлена", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}