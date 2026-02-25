using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace ChurchBudget
{
    // Оставляем только один статический класс
    public static class DatabaseHelper
    {
        // Путь к базе: папка программы \ Data \ church.db
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\church.db");

        // Строка подключения для SQLite
        private static string connectionString = string.Format("Data Source={0};Version=3;", dbPath);

        // Универсальный метод для получения данных (SELECT)
        public static DataTable ExecuteQuery(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при чтении БД: " + ex.Message);
            }
            return dt;
        }

        // Универсальный метод для изменения данных (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string sql)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при записи в БД: " + ex.Message);
                return -1;
            }
        }

        public static string GetDbSetting(string columnName)
        {
            // Запрос к таблице churches, берем первую запись
            string sql = "SELECT " + columnName + " FROM churches LIMIT 1";
            DataTable dt = ExecuteQuery(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][columnName].ToString();
            }
            return "Данные не найдены";
        }
    }
}