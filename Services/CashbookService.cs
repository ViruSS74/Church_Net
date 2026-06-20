using System;
using System.Data;
using System.Data.SQLite;

public class CashbookService
{
    private readonly string _connectionString;

    public CashbookService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Получить начальный остаток на начало года
    public decimal GetOpeningBalanceForYear(int year)
    {
        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string sql = @"SELECT amount FROM opening_balances 
                           WHERE balance_date = @date LIMIT 1";
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@date", $"{year}-01-01");
                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value
                    ? Convert.ToDecimal(result) : 0;
            }
        }
    }

    // Получить все операции (ПКО и РКО) за месяц
    public DataTable GetOperationsForMonth(int year, int month)
    {
        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string sql = @"
                SELECT 
                    co.id,
                    co.order_number,
                    co.date,
                    co.order_type,
                    co.amount,
                    COALESCE(co.person_name_manual, '') as counterparty,
                    COALESCE(co.base, '') as base,
                    COALESCE(co.appendix, '') as appendix
                FROM cash_orders co
                WHERE strftime('%Y', co.date) = @year
                AND strftime('%m', co.date) = @month
                ORDER BY co.date, co.id";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@year", year.ToString());
                cmd.Parameters.AddWithValue("@month", month.ToString("D2"));
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    // Получить операции за конкретный день
    public DataTable GetOperationsForDay(int year, int month, int day)
    {
        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string dateStr = $"{year}-{month:D2}-{day:D2}";
            string sql = @"
                SELECT 
                    co.id,
                    co.order_number,
                    co.date,
                    co.order_type,
                    co.amount,
                    COALESCE(co.person_name_manual, '') as counterparty,
                    COALESCE(co.base, '') as base
                FROM cash_orders co
                WHERE date(co.date) = date(@dateStr)
                ORDER BY co.id";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@dateStr", dateStr);
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    // Расчет остатка на конец дня
    public decimal CalculateClosingBalance(int year, int month, int day)
    {
        decimal openingBalance = GetOpeningBalanceForYear(year);

        // Добавляем все ПКО и вычитаем все РКО до этого дня включительно
        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string endDate = $"{year}-{month:D2}-{day:D2}";

            string sqlPko = @"SELECT COALESCE(SUM(amount), 0) 
                FROM cash_orders 
                WHERE order_type = 'ПКО' 
                AND date(date) <= date(@endDate)";

            string sqlRko = @"SELECT COALESCE(SUM(amount), 0) 
                FROM cash_orders 
                WHERE order_type = 'РКО' 
                AND date(date) <= date(@endDate)";

            using (var cmdPko = new SQLiteCommand(sqlPko, conn))
            using (var cmdRko = new SQLiteCommand(sqlRko, conn))
            {
                cmdPko.Parameters.AddWithValue("@endDate", endDate);
                cmdRko.Parameters.AddWithValue("@endDate", endDate);

                decimal totalPko = Convert.ToDecimal(cmdPko.ExecuteScalar());
                decimal totalRko = Convert.ToDecimal(cmdRko.ExecuteScalar());

                return openingBalance + totalPko - totalRko;
            }
        }
    }

    // Получить информацию об организации
    public DataRow GetOrganizationInfo()
    {
        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string sql = "SELECT name, location FROM organizations LIMIT 1";
            DataTable dt = new DataTable();
            new SQLiteDataAdapter(sql, conn).Fill(dt);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }

    // Получить ФИО кассира и бухгалтера
    public (string Cashier, string Accountant) GetSignatures()
    {
        string cashier = "";
        string accountant = "";

        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string sql = "SELECT last_name, first_name, middle_name, role FROM personal";
            using (var cmd = new SQLiteCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string role = reader["role"].ToString().ToLower();
                    string fullName = $"{reader["last_name"]} {reader["first_name"]} {reader["middle_name"]}";

                    if (role.Contains("казначей"))
                        cashier = fullName;
                    else if (role.Contains("бухгалтер"))
                        accountant = fullName;
                }
            }
        }

        return (cashier, accountant);
    }

    // Получить количество дней в месяце
    public int GetDaysInMonth(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }

    // Проверить, есть ли операции за день
    public bool HasOperationsForDay(int year, int month, int day)
    {
        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string dateStr = $"{year}-{month:D2}-{day:D2}";
            string sql = "SELECT COUNT(*) FROM cash_orders WHERE date(date) = date(@dateStr)";
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@dateStr", dateStr);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }
}