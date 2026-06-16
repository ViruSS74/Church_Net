using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace ChurchBudget
{
    public class DatabaseCleanService
    {
        private readonly string _connectionString;
        private readonly string _backupPath;

        public DatabaseCleanService(string connectionString)
        {
            _connectionString = connectionString;

            // Исправлено для старых версий .NET
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _backupPath = Path.Combine(baseDir, "Data\\Archive");

            if (!Directory.Exists(_backupPath))
                Directory.CreateDirectory(_backupPath);
        }

        /// <summary>
        /// Создаёт резервную копию БД
        /// </summary>
        public string CreateBackup()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string sourceFile = Path.Combine(baseDir, "Data\\church.db");

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string backupFile = Path.Combine(_backupPath, "church_backup_" + timestamp + ".db");

                if (File.Exists(sourceFile))
                {
                    File.Copy(sourceFile, backupFile, true);
                    return backupFile;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка создания бэкапа: " + ex.Message);
            }
        }

        /// <summary>
        /// Подсчитывает количество записей в таблицах
        /// </summary>
        public Dictionary<string, int> GetTableCounts()
        {
            var counts = new Dictionary<string, int>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                string[] tables = {
            "income_docs", "income_items",
            "expense_docs", "expense_items",
            "cash_orders",
            "personal", "id_documents",
            "organizations"  // ← Добавьте эту строку
        };

                foreach (string table in tables)
                {
                    string sql = string.Format("SELECT COUNT(*) FROM {0}", table);
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        counts[table] = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }

            return counts;
        }

        /// <summary>
        /// Очищает документы (Доходы/Расходы/ПКО/РКО)
        /// </summary>
        public List<string> CleanDocuments(bool income, bool expense, bool cashOrders)
        {
            var log = new List<string>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (income)
                        {
                            int count = GetCount(conn, "income_docs");
                            ExecuteNonQuery(conn, "DELETE FROM income_items");
                            ExecuteNonQuery(conn, "DELETE FROM income_docs");
                            log.Add($"✓ Удалено документов доходов: {count}");
                        }

                        if (expense)
                        {
                            int count = GetCount(conn, "expense_docs");
                            ExecuteNonQuery(conn, "DELETE FROM expense_items");
                            ExecuteNonQuery(conn, "DELETE FROM expense_docs");
                            log.Add($"✓ Удалено документов расходов: {count}");
                        }

                        if (cashOrders)
                        {
                            int count = GetCount(conn, "cash_orders");
                            ExecuteNonQuery(conn, "DELETE FROM cash_orders");
                            log.Add($"✓ Удалено кассовых ордеров: {count}");
                        }

                        trans.Commit();
                        log.Add("✓ Транзакция успешно завершена");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        log.Add($"✗ Ошибка: {ex.Message}");
                        throw;
                    }
                }
            }

            return log;
        }

        /// <summary>
        /// Очищает справочники (сотрудники, ИД документы)
        /// </summary>
        public List<string> CleanDirectories(bool personal, bool idDocs)
        {
            var log = new List<string>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (idDocs)
                        {
                            int count = GetCount(conn, "id_documents");
                            ExecuteNonQuery(conn, "DELETE FROM id_documents");
                            log.Add($"✓ Удалено ИД документов: {count}");
                        }

                        if (personal)
                        {
                            int count = GetCount(conn, "personal");
                            ExecuteNonQuery(conn, "DELETE FROM personal");
                            log.Add($"✓ Удалено сотрудников: {count}");
                        }

                        trans.Commit();
                        log.Add("✓ Транзакция успешно завершена");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        log.Add($"✗ Ошибка: {ex.Message}");
                        throw;
                    }
                }
            }

            return log;
        }

        /// <summary>
        /// Очищает справочник организаций
        /// </summary>
        public List<string> CleanOrganizations()
        {
            var log = new List<string>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        int count = GetCount(conn, "organizations");
                        ExecuteNonQuery(conn, "DELETE FROM organizations");
                        log.Add(string.Format("✓ Удалено организаций: {0}", count));

                        trans.Commit();
                        log.Add("✓ Справочник организаций очищен");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        log.Add(string.Format("✗ Ошибка: {0}", ex.Message));
                        throw;
                    }
                }
            }

            return log;
        }

        private int GetCount(SQLiteConnection conn, string table)
        {
            string sql = $"SELECT COUNT(*) FROM {table}";
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void ExecuteNonQuery(SQLiteConnection conn, string sql)
        {
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}