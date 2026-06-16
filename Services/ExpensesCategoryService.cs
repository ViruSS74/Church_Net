using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace ChurchBudget
{
    public class ExpenseCategoryService
    {
        // Универсальный метод получения строки подключения
        private string GetConnectionString()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\church.db");
            return string.Format("Data Source={0};Version=3;", dbPath);
        }

        public List<ExpenseCategoryItem> GetAll()
        {
            List<ExpenseCategoryItem> list = new List<ExpenseCategoryItem>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();
                    // Запрос к таблице РАСХОДОВ
                    string sql = "SELECT id, name, parent_id FROM expense_categories ORDER BY name";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ExpenseCategoryItem
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                ParentId = reader["parent_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["parent_id"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка SQLite (expense_categories): " + ex.Message);
            }
            return list;
        }

        public void Save(int? id, string name, int? parentId)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();
                    string sql;
                    if (id == null)
                        sql = "INSERT INTO expense_categories (name, parent_id) VALUES (@name, @pid)";
                    else
                        sql = "UPDATE expense_categories SET name = @name, parent_id = @pid WHERE id = @id";

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
                MessageBox.Show("Ошибка сохранения расходов: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();
                    // Проверка на наличие подкатегорий в расходах
                    string checkSql = "SELECT COUNT(*) FROM expense_categories WHERE parent_id = @id";
                    using (var checkCmd = new SQLiteCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", id);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Нельзя удалить категорию расходов, у которой есть подкатегории!");
                            return;
                        }
                    }

                    string sql = "DELETE FROM expense_categories WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления категории расходов: " + ex.Message);
            }
        }

        public class ExpenseCategoryItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? ParentId { get; set; }
        }
    }
}