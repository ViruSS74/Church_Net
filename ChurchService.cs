using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ChurchBudget
{
    public class ChurchService
    {
        private string GetConnectionString()
        {
            // Здесь путь правильный: Debug\Data\church.db
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            dbPath = Path.Combine(dbPath, "church.db");
            return string.Format("Data Source={0};Version=3;", dbPath);
        }

        public List<Church> GetAllChurches()
        {
            var list = new List<Church>();

            // Используем уже настроенную строку подключения!
            string connString = GetConnectionString();

            // Выделяем чистый путь для проверки файла
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            dbPath = Path.Combine(dbPath, "church.db");

            if (!File.Exists(dbPath))
            {
                MessageBox.Show("ФАЙЛ НЕ НАЙДЕН ПО ПУТИ: " + dbPath);
                return list;
            }

            try
            {
                using (var conn = new System.Data.SQLite.SQLiteConnection(connString))
                {
                    conn.Open();

                    // Проверка наличия таблицы organizations
                    using (var checkCmd = new System.Data.SQLite.SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name='organizations'", conn))
                    {
                        if (checkCmd.ExecuteScalar() == null)
                        {
                            MessageBox.Show("ОШИБКА: Таблица 'organizations' отсутствует в файле!");
                            return list;
                        }
                    }

                    string sql = "SELECT id, name, location, deanery, diocese FROM organizations";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new Church
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                Name = dr["name"].ToString(),
                                Location = dr["location"].ToString(),
                                Deanery = dr["deanery"].ToString(),
                                Diocese = dr["diocese"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка SQLite: " + ex.Message);
            }
            return list;
        }

        public void SaveChurch(int? id, string name, string location, string deanery, string diocese)
        {
            using (var conn = new System.Data.SQLite.SQLiteConnection(GetConnectionString()))
            {
                conn.Open();
                string sql = id == null
                    ? "INSERT INTO organizations (name, location, deanery, diocese) VALUES (@name, @loc, @dean, @dioc)"
                    : "UPDATE organizations SET name=@name, location=@loc, deanery=@dean, diocese=@dioc WHERE id=@id";

                using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                {
                    if (id != null) cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@loc", location);
                    cmd.Parameters.AddWithValue("@dean", deanery);
                    cmd.Parameters.AddWithValue("@dioc", diocese);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteChurch(int id)
        {
            using (var conn = new System.Data.SQLite.SQLiteConnection(GetConnectionString()))
            {
                conn.Open();
                using (var cmd = new System.Data.SQLite.SQLiteCommand("DELETE FROM organizations WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}