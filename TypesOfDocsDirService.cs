using System.Data;
using System.Data.SQLite;

namespace ChurchBudget.Forms
{
    public class TypesOfDocsDirService
    {
        private string _connectionString = "Data Source=Data\\church.db;Version=3;";

        // Получение всех видов документов
        public DataTable GetAllViews()
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                string sql = "SELECT id as Id, name as Name FROM type_document ORDER BY name";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
                adapter.Fill(dt);
            }
            return dt;
        }

        // Сохранение (Добавление или Обновление)
        public void SaveView(int? id, string name)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = id == null
                    ? "INSERT INTO type_document (name) VALUES (@name)"
                    : "UPDATE type_document SET name = @name WHERE id = @id";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (id != null) cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Удаление
        public void DeleteView(int id)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM type_document WHERE id = @id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}