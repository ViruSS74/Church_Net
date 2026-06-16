using ChurchBudget.Forms;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ChurchBudget
{
    public class IncomeCategoryService
    {
        // Путь к базе в папке Data
        private string connectionString = string.Format("Data Source={0};Version=3;", Program.DbPath);

        public List<IncomeCategoryItem> GetAll()
        {
            List<IncomeCategoryItem> list = new List<IncomeCategoryItem>();
            try
            {
                // Используем путь относительно исполняемого файла
                string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\church.db");
                string connString = string.Format("Data Source={0};Version=3;", dbPath);

                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    // Запрос ко всем записям таблицы
                    string sql = "SELECT id, name, parent_id FROM income_categories ORDER BY name";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new IncomeCategoryItem
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                // Обработка NULL для корректной работы дерева
                                ParentId = reader["parent_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["parent_id"])
                            });
                        }
                    }
                }

                // ТЕСТОВЫЙ ВЫВОД: Если это окно покажет число больше 0, значит данные считаны!
                // MessageBox.Show("Записей загружено из БД: " + list.Count.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка SQLite (income_categories): " + ex.Message);
            }
            return list;
        }

        public void Save(int? id, string name, int? parentId)
        {
            try
            {
                string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\church.db");
                string connString = string.Format("Data Source={0};Version=3;", dbPath);

                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    string sql;
                    if (id == null) // Новая запись
                        sql = "INSERT INTO income_categories (name, parent_id) VALUES (@name, @pid)";
                    else // Редактирование
                        sql = "UPDATE income_categories SET name = @name, parent_id = @pid WHERE id = @id";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@pid", (object)parentId ?? DBNull.Value);
                        if (id != null) cmd.Parameters.AddWithValue("@id", id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения в БД: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\church.db");
                string connString = string.Format("Data Source={0};Version=3;", dbPath);

                using (var conn = new System.Data.SQLite.SQLiteConnection(connString))
                {
                    conn.Open();
                    // Сначала проверим, нет ли у категории "детей" (подкатегорий)
                    string checkSql = "SELECT COUNT(*) FROM income_categories WHERE parent_id = @id";
                    using (var checkCmd = new System.Data.SQLite.SQLiteCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", id);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Нельзя удалить категорию, у которой есть подкатегории!");
                            return;
                        }
                    }

                    // Если детей нет — удаляем
                    string sql = "DELETE FROM income_categories WHERE id = @id";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления: " + ex.Message);
            }
        }

        // Класс-модель для таблицы
        public class IncomeCategoryItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? ParentId { get; set; } // Чья это подкатегория
        }
    }
}