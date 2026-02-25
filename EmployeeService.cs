using System;
using System.Data; // Нужен для DataTable
using System.Data.SQLite; // Нужен для работы с SQLite

namespace ChurchBudget
{
    public class EmployeeService
    {
        // Метод для получения строки подключения
        private string GetConnectionString()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            // Указываем путь: папка_запуска\Data\church.db
            string dbPath = System.IO.Path.Combine(baseDir, "Data");
            dbPath = System.IO.Path.Combine(dbPath, "church.db");
            return string.Format("Data Source={0};Version=3;", dbPath);
        }

        // Получение всех сотрудников
        public DataTable GetAllEmployees()
        {
            using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();
                // Присваиваем русские алиасы (псевдонимы) колонкам
                string sql = @"SELECT id, 
                              last_name AS [Фамилия], 
                              first_name AS [Имя], 
                              middle_name AS [Отчество], 
                              role AS [Должность] 
                       FROM personal";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        // Поиск по фамилии
        public DataTable Search(string text)
        {
            if (string.IsNullOrEmpty(text) || text == "Поиск по фамилии...")
                return GetAllEmployees();

            using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();
                string sql = @"SELECT id, 
                              last_name AS [Фамилия], 
                              first_name AS [Имя], 
                              middle_name AS [Отчество], 
                              role AS [Должность] 
                       FROM personal 
                       WHERE last_name LIKE @text || '%'";
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@text", text);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public void SaveEmployee(int? id, string last, string first, string middle, string role)
        {
            using (var conn = new System.Data.SQLite.SQLiteConnection(GetConnectionString()))
            {
                conn.Open();
                string sql = id == null
                    ? "INSERT INTO personal (last_name, first_name, middle_name, role) VALUES (@last, @first, @middle, @role)"
                    : "UPDATE personal SET last_name=@last, first_name=@first, middle_name=@middle, role=@role WHERE id=@id";

                using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                {
                    if (id != null) cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@last", last);
                    cmd.Parameters.AddWithValue("@first", first);
                    cmd.Parameters.AddWithValue("@middle", middle);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteEmployee(int id)
        {
            using (var conn = new System.Data.SQLite.SQLiteConnection(GetConnectionString()))
            {
                conn.Open();
                // Убедитесь, что таблица называется Employees (или замените на ваше имя)
                string sql = "DELETE FROM Employees WHERE id = @id";

                using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}