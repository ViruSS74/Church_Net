using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ChurchBudget
{
    public class FinancialReportService
    {
        private string _connectionString;

        public FinancialReportService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Получает остаток на начало периода.
        /// Для 01.01.YYYY — берёт из opening_balances.
        /// Для остальных дат — автоматически считает из истории операций.
        /// </summary>
        public decimal GetOpeningBalanceForPeriod(int orgId, DateTime periodStart)
        {
            // Если это 1 января — берём ручной ввод
            if (periodStart.Month == 1 && periodStart.Day == 1)
            {
                decimal? manualBalance = GetManualOpeningBalance(orgId, periodStart);
                if (manualBalance.HasValue)
                    return manualBalance.Value;
            }

            // Для всех остальных дат — автоматический расчёт
            return CalculateOpeningBalanceFromHistory(orgId, periodStart);
        }

        /// <summary>
        /// Ищет ручной ввод остатка на дату <= periodStart
        /// </summary>
        private decimal? GetManualOpeningBalance(int orgId, DateTime periodStart)
        {
            try
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                SELECT amount FROM opening_balances 
                WHERE organization_id = @orgId AND balance_date <= @periodStart 
                ORDER BY balance_date DESC LIMIT 1";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@orgId", orgId);
                        cmd.Parameters.AddWithValue("@periodStart", periodStart.ToString("yyyy-MM-dd"));
                        var result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                            return Convert.ToDecimal(result);
                    }
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Считает остаток как: (остаток на 01.01) + (доходы) - (расходы) до начала периода
        /// </summary>
        private decimal CalculateOpeningBalanceFromHistory(int orgId, DateTime periodStart)
        {
            // 1. Сначала находим остаток на 01.01 года, для которого считаем
            int year = periodStart.Year;
            DateTime startOfYear = new DateTime(year, 1, 1);

            decimal openingBalance = 0;
            decimal? manualStart = GetManualOpeningBalance(orgId, startOfYear);
            if (manualStart.HasValue)
            {
                openingBalance = manualStart.Value;
            }

            decimal incomeBefore = 0;
            decimal expenseBefore = 0;

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                // 2. Считаем все доходы С 01.01 ДО periodStart
                string sqlIncome = @"
            SELECT COALESCE(SUM(ii.amount), 0)
            FROM income_items ii
            INNER JOIN income_docs id ON ii.doc_id = id.id
            WHERE id.date >= @startDate AND id.date < @beforeDate";

                using (var cmd = new SQLiteCommand(sqlIncome, conn))
                {
                    cmd.Parameters.AddWithValue("@startDate", startOfYear.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@beforeDate", periodStart.ToString("yyyy-MM-dd"));
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        incomeBefore = Convert.ToDecimal(result);
                }

                // 3. Считаем все расходы С 01.01 ДО periodStart
                string sqlExpense = @"
            SELECT COALESCE(SUM(ei.amount), 0)
            FROM expense_items ei
            INNER JOIN expense_docs ed ON ei.doc_id = ed.id
            WHERE ed.date >= @startDate AND ed.date < @beforeDate";

                using (var cmd = new SQLiteCommand(sqlExpense, conn))
                {
                    cmd.Parameters.AddWithValue("@startDate", startOfYear.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@beforeDate", periodStart.ToString("yyyy-MM-dd"));
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        expenseBefore = Convert.ToDecimal(result);
                }
            }

            // 4. Формула: остаток_на_01.01 + доходы - расходы
            return openingBalance + incomeBefore - expenseBefore;
        }

        /// <summary>
        /// Сохраняет или обновляет остаток на дату
        /// </summary>
        public void SaveOpeningBalance(int orgId, DateTime date, decimal amount, string comment = "")
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                // 1. Проверяем, есть ли уже запись на эту дату
                string checkSql = "SELECT id FROM opening_balances WHERE organization_id = @orgId AND balance_date = @date";
                object existingId = null;

                using (var cmdCheck = new SQLiteCommand(checkSql, conn))
                {
                    cmdCheck.Parameters.AddWithValue("@orgId", orgId);
                    cmdCheck.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    existingId = cmdCheck.ExecuteScalar();
                }

                // 2. Обновляем или создаём запись
                if (existingId != null && existingId != DBNull.Value)
                {
                    // Обновление
                    string updateSql = "UPDATE opening_balances SET amount = @amount, comment = @comment WHERE id = @id";
                    using (var cmd = new SQLiteCommand(updateSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@amount", (double)amount);
                        cmd.Parameters.AddWithValue("@comment", comment);
                        cmd.Parameters.AddWithValue("@id", existingId);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Создание новой записи
                    string insertSql = "INSERT INTO opening_balances (organization_id, balance_date, amount, comment) VALUES (@orgId, @date, @amount, @comment)";
                    using (var cmd = new SQLiteCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@orgId", orgId);
                        cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@amount", (double)amount);
                        cmd.Parameters.AddWithValue("@comment", comment);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Получает список всех остатков для организации
        /// </summary>
        public List<Dictionary<string, object>> GetOpeningBalancesList(int orgId)
        {
            var list = new List<Dictionary<string, object>>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT id, balance_date, amount, comment, created_at FROM opening_balances WHERE organization_id = @orgId ORDER BY balance_date DESC";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@orgId", orgId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new Dictionary<string, object>
                            {
                                { "id", reader["id"] },
                                { "date", reader["balance_date"] },
                                { "amount", reader["amount"] },
                                { "comment", reader["comment"] },
                                { "created", reader["created_at"] }
                            };
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        public Dictionary<string, decimal> GetIncomeByReportCode(DateTime start, DateTime end)
        {
            var result = new Dictionary<string, decimal>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT ic.report_code, COALESCE(SUM(ii.amount), 0) as total FROM income_items ii INNER JOIN income_docs id ON ii.doc_id = id.id INNER JOIN income_categories ic ON ii.category = ic.name WHERE id.date >= @start AND id.date <= @end AND ic.report_code IS NOT NULL GROUP BY ic.report_code";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string code = reader["report_code"].ToString();
                            decimal amount = Convert.ToDecimal(reader["total"]);
                            result[code] = amount;
                        }
                    }
                }
            }
            return result;
        }

        public Dictionary<string, decimal> GetExpenseByReportCode(DateTime start, DateTime end)
        {
            var result = new Dictionary<string, decimal>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT ec.report_code, COALESCE(SUM(ei.amount), 0) as total FROM expense_items ei INNER JOIN expense_docs ed ON ei.doc_id = ed.id INNER JOIN expense_categories ec ON ei.category = ec.name WHERE ed.date >= @start AND ed.date <= @end AND ec.report_code IS NOT NULL GROUP BY ec.report_code";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string code = reader["report_code"].ToString();
                            decimal amount = Convert.ToDecimal(reader["total"]);
                            result[code] = amount;
                        }
                    }
                }
            }
            return result;
        }

        public Dictionary<string, string> GetOrganizationInfo(int orgId)
        {
            var info = new Dictionary<string, string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT name, location, deanery, diocese FROM organizations WHERE id = @id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", orgId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            info["Name"] = reader["name"].ToString();
                            info["Location"] = reader["location"].ToString();
                            info["Blagochinie"] = reader["deanery"].ToString();
                            info["Diocese"] = reader["diocese"].ToString();
                        }
                    }
                }
            }
            return info;
        }

        public Dictionary<string, string> GetSignatures(int orgId)
        {
            var signatures = new Dictionary<string, string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT role, last_name, substr(first_name, 1, 1) || '.' as name_initial, substr(middle_name, 1, 1) || '.' as patronymic_initial FROM personal";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string position = reader["role"].ToString();
                            string lastName = reader["last_name"].ToString();
                            string initials = reader["name_initial"].ToString() + reader["patronymic_initial"].ToString();
                            signatures[position] = lastName + " " + initials;
                        }
                    }
                }
            }
            return signatures;
        }

        public string GetConstant(string key)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT value FROM constants WHERE key = @key";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@key", key);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return result.ToString();
                }
            }
            return "";
        }
    }
}