using System;
using System.Data;
using System.Data.SQLite;

namespace ChurchBudget.Forms
{
    public class IDDocsService
    {
        private string _connectionString = "Data Source=Data\\church.db;Version=3;";

        // 1. Получение всех документов для ГЛАВНОЙ ТАБЛИЦЫ
        public DataTable GetAllDocs()
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                // Склеиваем Фамилию И. О. для компактности в таблице
                string sql = @"
                    SELECT 
                        d.id AS Id, 
                        (p.last_name || ' ' || SUBSTR(p.first_name, 1, 1) || '.' || SUBSTR(p.middle_name, 1, 1) || '.') AS EmployeeName, 
                        t.name AS DocTypeName, 
                        d.series AS Series, 
                        d.number AS Number,
                        d.issued_by AS IssuedBy,
                        d.issue_date AS IssueDate,
                        d.employee_id,
                        d.type_id_doc
                    FROM id_documents d
                    JOIN personal p ON d.employee_id = p.id
                    JOIN type_id_document t ON d.type_id_doc = t.id
                    ORDER BY p.last_name";

                new SQLiteDataAdapter(sql, conn).Fill(dt);
            }
            return dt;
        }

        // 2. Список сотрудников для ВЫБОРА (ComboBox)
        public DataTable GetEmployeesForCombo()
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                // Здесь лучше полное ФИО, чтобы не перепутать однофамильцев
                string sql = "SELECT id, (last_name || ' ' || first_name || ' ' || middle_name) as fio FROM personal ORDER BY last_name";
                new SQLiteDataAdapter(sql, conn).Fill(dt);
            }
            return dt;
        }

        // 3. Сохранение данных
        public void SaveDoc(int? id, int empId, int typeId, string series, string number, string issuedBy, DateTime date)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = id == null
                    ? @"INSERT INTO id_documents (employee_id, type_id_doc, series, number, issued_by, issue_date) 
                        VALUES (@empId, @typeId, @ser, @num, @issued, @date)"
                    : @"UPDATE id_documents SET employee_id=@empId, type_id_doc=@typeId, series=@ser, 
                        number=@num, issued_by=@issued, issue_date=@date WHERE id=@id";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (id != null) cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@empId", empId);
                    cmd.Parameters.AddWithValue("@typeId", typeId);
                    cmd.Parameters.AddWithValue("@ser", string.IsNullOrEmpty(series) ? (object)DBNull.Value : series);
                    cmd.Parameters.AddWithValue("@num", number);
                    cmd.Parameters.AddWithValue("@issued", issuedBy);
                    cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 4. Удаление
        public void DeleteDoc(int id)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM id_documents WHERE id = @id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 5. Список типов документов для выбора
        public DataTable GetDocTypesForCombo()
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                string sql = "SELECT id, name FROM type_id_document ORDER BY name";
                new SQLiteDataAdapter(sql, conn).Fill(dt);
            }
            return dt;
        }
    }
}