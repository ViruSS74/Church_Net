using ChurchBudget.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace ChurchBudget
{
    public class ListOfDocsService
    {
        private readonly string _connectionString;
        public string ConnectionString { get { return _connectionString; } }

        public ListOfDocsService(string dbPath)
        {
            if (dbPath.Contains("Data Source"))
                _connectionString = dbPath;
            else
                _connectionString = string.Format("Data Source={0};Version=3;", dbPath);
        }

        private bool IsIncome(string type)
        {
            return type == "Доходы" || type == "Income" || type == "ПКО" || type == "Доход";
        }

        // 1. ПРОВЕРКА СУЩЕСТВОВАНИЯ НОМЕРА
        public bool IsNumberExists(string docNumber, string tableName, SQLiteConnection conn, SQLiteTransaction trans = null)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} WHERE doc_number = @n", tableName);
            using (var cmd = new SQLiteCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@n", docNumber);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // Автономный вариант: использует внутреннюю строку подключения
        public bool IsNumberExists(string docNumber, string tableName)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                return IsNumberExists(docNumber, tableName, conn);
            }
        }

        // 2. ГЕНЕРАЦИЯ НОМЕРА
        public string GetNextDocNumber(DateTime selectedDate, string prefixLetter)
        {
            string datePart = selectedDate.ToString("ddMMyyyy");
            string prefix = string.Format("{0}-{1}-", prefixLetter, datePart);
            string tableName = (prefixLetter == "Д" || prefixLetter == "П") ? "income_docs" : "expense_docs";
            try
            {
                using (var conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sql = string.Format("SELECT doc_number FROM {0} WHERE doc_number LIKE @pref || '%' ORDER BY doc_number DESC LIMIT 1", tableName);
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@pref", prefix);
                        var result = cmd.ExecuteScalar();
                        int nextId = 1;
                        if (result != null)
                        {
                            string lastNum = result.ToString();
                            string[] parts = lastNum.Split('-');
                            if (parts.Length == 3 && int.TryParse(parts[2], out nextId))
                                nextId++;
                        }
                        return string.Format("{0}{1:D3}", prefix, nextId);
                    }
                }
            }
            catch { return prefix + "001"; }
        }

        // 3. ПОЛУЧЕНИЕ ПУНКТОВ ДОКУМЕНТА
        public DataTable GetDocumentItems(string type, int docId)
        {
            DataTable dt = new DataTable();
            bool isInc = IsIncome(type);
            string mainTable = isInc ? "income_docs" : "expense_docs";
            string sql = string.Format("SELECT * FROM {0} WHERE id = @id", mainTable);
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", docId);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public string GetNextCashOrderNumber(string type, string connectionString)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                // Берем текущий год
                string currentYear = DateTime.Now.Year.ToString();
                // Считаем документы только за этот год
                string sql = @"SELECT COUNT(*) FROM cash_orders 
                       WHERE order_type = @t 
                       AND date LIKE @year || '%'";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@t", type);
                    cmd.Parameters.AddWithValue("@year", currentYear);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    // Результат: ПКО-2026/0001 (так солиднее и понятнее для архива)
                    return string.Format("{0}-{1}/{2:D4}", type, currentYear, count + 1);
                }
            }
        }

        public DataTable GetDocumentsByPeriod(string type, DateTime start, DateTime end)
        {
            DataTable dt = new DataTable();
            bool isInc = IsIncome(type);
            string tableName = isInc ? "income_docs" : "expense_docs";
            string docType = isInc ? "Income" : "Expense";
            string sql = string.Format(@"
                SELECT id, doc_number, date, total, '{0}' as doc_type 
                FROM {1} 
                WHERE date(date) BETWEEN date(@s) AND date(@e) 
                ORDER BY date(date) ASC", docType, tableName);
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@s", start.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@e", end.ToString("yyyy-MM-dd"));
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        public DataTable GetAllDocuments(DateTime start, DateTime end, string filterType)
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                // Исправлено: Заменяем ПКО/РКО на Income/Expense для внутренней логики
                string dbFilter = filterType;
                if (filterType == "Доходы" || filterType == "ПКО") dbFilter = "Income";
                if (filterType == "Расходы" || filterType == "РКО") dbFilter = "Expense";
                string sql = @"
    SELECT * FROM (
        SELECT id, date, doc_number, 'Income' as doc_type FROM income_docs 
        UNION ALL
        SELECT id, date, doc_number, 'Expense' as doc_type FROM expense_docs
        UNION ALL
        -- Добавляем ПКО и РКО из кассовой таблицы
        SELECT id, date, order_number as doc_number, order_type as doc_type FROM cash_orders
    ) AS combined
    WHERE date(date) BETWEEN date(@s) AND date(@e)";

                if (dbFilter != "Все")
                {
                    sql += " AND doc_type = @t";
                }
                sql += " ORDER BY date DESC, id DESC";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@s", start.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@e", end.ToString("yyyy-MM-dd"));
                    if (dbFilter != "Все") cmd.Parameters.AddWithValue("@t", dbFilter);
                    new SQLiteDataAdapter(cmd).Fill(dt);
                }
            }
            return dt;
        }

        public DataTable GetFullRaportichka(string type, int docId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("display_name", typeof(string));
            dt.Columns.Add("amount", typeof(decimal));
            bool isInc = IsIncome(type);
            string catTable = isInc ? "income_categories" : "expense_categories";
            string itemsTable = isInc ? "income_items" : "expense_items";
            string sql = string.Format(@"
                SELECT 
                    c.name, 
                    c.parent_id, 
                    (SELECT SUM(amount) FROM {1} WHERE category = c.name AND doc_id = @docId) as total_amount
                FROM {0} c
                WHERE c.is_active = 1
                ORDER BY c.id ASC", catTable, itemsTable);
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@docId", docId);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["name"].ToString();
                            decimal amount = 0;
                            if (reader["total_amount"] != DBNull.Value)
                                amount = Convert.ToDecimal(reader["total_amount"]);
                            if (reader["parent_id"] != DBNull.Value && reader["parent_id"].ToString() != "0")
                                name = "      " + name;
                            dt.Rows.Add(name, amount);
                        }
                    }
                }
            }
            return dt;
        }

        public DataRow GetOrganizationData()
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                string sql = "SELECT name, location, deanery FROM organizations LIMIT 1";
                new SQLiteDataAdapter(sql, conn).Fill(dt);
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public DataTable GetPersonalList()
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                string sql = "SELECT last_name, first_name, middle_name, role FROM personal";
                new SQLiteDataAdapter(sql, conn).Fill(dt);
            }
            return dt;
        }

        public DataTable GetPersonalListForCmb()
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                // Склеиваем ФИО прямо в SQLite, чтобы ComboBox выглядел красиво
                string sql = @"SELECT id, 
                       (last_name || ' ' || first_name || ' ' || middle_name) AS FullName 
                       FROM personal 
                       ORDER BY last_name ASC";
                using (var adapter = new SQLiteDataAdapter(sql, conn))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public void DeleteDocument(long docId, string docType)
        {
            bool isInc = IsIncome(docType);
            string headerTable = isInc ? "income_docs" : "expense_docs";
            string itemsTable = isInc ? "income_items" : "expense_items";
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlItems = string.Format("DELETE FROM {0} WHERE doc_id = @id", itemsTable);
                        string sqlHeader = string.Format("DELETE FROM {0} WHERE id = @id", headerTable);
                        using (SQLiteCommand cmd = new SQLiteCommand(sqlItems, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id", docId);
                            cmd.ExecuteNonQuery();
                        }
                        using (SQLiteCommand cmd = new SQLiteCommand(sqlHeader, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id", docId);
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
        }

        public DataRow GetCashOrderData(int orderId)
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                // Добавили p.last_name, p.first_name, p.middle_name
                string sql = @"
            SELECT co.*, 
                   p.last_name, p.first_name, p.middle_name,
                   (COALESCE(d.series, '') || ' ' || COALESCE(d.number, '') || ', выдан ' || COALESCE(d.issued_by, '') || ' ' || IFNULL(STRFTIME('%d.%m.%Y', d.issue_date), '')) AS full_passport
            FROM cash_orders co
            LEFT JOIN personal p ON co.person_id = p.id
            LEFT JOIN id_documents d ON p.id = d.employee_id
            WHERE co.id = @id";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@id", orderId);
                adapter.Fill(dt);
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public DataTable GetAllCashOrders()
        {
            string sql = "SELECT * FROM cash_orders ORDER BY date DESC";
            return ExecuteDataTable(sql);
        }

        private DataTable ExecuteDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при выполнении запроса: " + ex.Message);
            }
            return dt;
        }

        public DataTable GetRecipients()
        {
            // Объединяем пустую строку с реальными данными из таблицы personal
            string sql = @"
        SELECT -1 AS id, 'Не указан' AS full_name, '' AS role
        UNION ALL
        SELECT id, (last_name || ' ' || first_name || ' ' || middle_name), role
        FROM personal 
        ORDER BY id ASC"; // -1 всегда будет первым

            return ExecuteDataTable(sql);
        }
        public DataTable GetPkoList()
        {
            // Используем ваши имена колонок: order_number, date, amount, person, base
            // Фильтруем по order_type = 'ПКО' (или как вы записываете тип в БД)
            string sql = @"SELECT 
                        id, 
                        order_number AS [№ Ордера], 
                        date AS [Дата], 
                        amount AS [Сумма], 
                        person AS [Принято от], 
                        base AS [Основание] 
                   FROM cash_orders 
                   WHERE order_type = 'ПКО' 
                   ORDER BY date DESC, order_number DESC";
            return ExecuteDataTable(sql);
        }
        public DataTable GetPkoItems(int pkoId)
        {
            // Связываем cash_orders с income_items через doc_ref_id
            string sql = string.Format(@"
        SELECT 
            i.id,
            i.category AS [Категория],
            i.description AS [Описание],
            i.amount AS [Сумма]
        FROM income_items i
        JOIN cash_orders co ON i.doc_id = co.doc_ref_id
        WHERE co.id = {0}", pkoId);
            return ExecuteDataTable(sql);
        }
        public DataTable GetPkoRegistryRow(int pkoId)
        {
            // Собираем данные: 
            // 1 и 1а - из personal (через связь в cash_orders)
            // 2 и 2а - константы
            // 3 - сгруппированные категории из income_items
            string sql = string.Format(@"
        SELECT 
            p.last_name AS [1], 
            (p.first_name || ' ' || p.middle_name) AS [1а],
            'BYR' AS [2],
            'Белорусский рубль' AS [2а],
            (SELECT GROUP_CONCAT(category, CHAR(10)) FROM income_items WHERE doc_id = co.doc_ref_id) AS [3]
        FROM cash_orders co
        LEFT JOIN personal p ON co.person = p.id
        WHERE co.id = {0}", pkoId);
            return ExecuteDataTable(sql);
        }
        public string GetIncomeBaseDescription(int incomeDocId)
        {
            // Собираем все категории дохода в одну строку через запятую
            string sql = string.Format(@"
        SELECT GROUP_CONCAT(c.name, ', ') 
        FROM income_items i
        JOIN income_categories c ON i.category_id = c.id
        WHERE i.doc_id = {0}", incomeDocId);
            // Выполняем запрос
            object result = ExecuteScalar(sql);
            return result != null ? result.ToString() : "Приход средств";
        }
        private object ExecuteScalar(string sql)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка ExecuteScalar: " + ex.Message);
            }
        }

        // Для команд INSERT, UPDATE, DELETE
        public void ExecuteNonQuery(string sql)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка ExecuteNonQuery: " + ex.Message);
            }
        }

        public DataTable GetPkoReportData(int pkoId)
        {
            // Запрос собирает: 
            // 1 - Фамилия, 1а - Имя Отчество
            // 2 и 2а - Константы (BYN, Белорусский рубль)
            // 3 - Объединенные категории из income_items
            string sql = string.Format(@"
        SELECT 
            p.last_name AS [1], 
            (p.first_name || ' ' || p.middle_name) AS [1а],
            'BYN' AS [2],
            'Белорусский рубль' AS [2а],
            (SELECT GROUP_CONCAT(category, char(10)) 
             FROM income_items 
             WHERE doc_id = co.doc_ref_id) AS [3]
        FROM cash_orders co
        LEFT JOIN personal p ON co.person = p.id
        WHERE co.id = {0}", pkoId);

            return ExecuteDataTable(sql);
        }

        public DataTable GetPkoRegistryByRole(int pkoId)
        {
            string sql = string.Format(@"
        SELECT 
            p.last_name AS [1], 
            (p.first_name || ' ' || p.middle_name) AS [1а],
            'BYN' AS [2],
            'Белорусский рубль' AS [2а],
            (SELECT GROUP_CONCAT(category, char(10)) 
             FROM income_items 
             WHERE doc_id = co.doc_ref_id) AS [3]
        FROM cash_orders co
        JOIN personal p ON p.role LIKE 'Казначей'
        WHERE co.id = {0}

        UNION ALL

        SELECT 
            p.last_name AS [1], 
            (p.first_name || ' ' || p.middle_name) AS [1а],
            '' AS [2],
            '' AS [2а],
            (SELECT GROUP_CONCAT(category, char(10)) 
             FROM income_items 
             WHERE doc_id = co.doc_ref_id) AS [3]
        FROM cash_orders co
        JOIN personal p ON p.role LIKE 'Настоятель храма'
        WHERE co.id = {0}", pkoId);

            return ExecuteDataTable(sql);
        }

        public void SaveCashOrder(string type, string number, DateTime date, decimal amount, string basis, int? personId, string manualName, int refDocId)
        {
            string sql = @"INSERT INTO cash_orders 
                  (order_type, order_number, date, amount, base, person_id, person_name_manual, doc_ref_id) 
                  VALUES (@type, @num, @date, @amt, @base, @pId, @pManual, @refId)";

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@num", number);
                    cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@amt", (double)amount);
                    cmd.Parameters.AddWithValue("@base", basis);
                    cmd.Parameters.AddWithValue("@pId", (object)personId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pManual", (object)manualName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@refId", refDocId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePkoRecord(int id, string from, string basis, string app, double sum)
        {
            // Используем точные названия колонок со скриншота
            string sql = @"UPDATE cash_orders 
                   SET person_name_manual = @f, 
                       base = @b, 
                       appendix = @a, 
                       amount = @s 
                   WHERE id = @id";

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    // Параметры защищают от ошибок синтаксиса (точки, кавычки)
                    cmd.Parameters.AddWithValue("@f", (object)from ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@b", (object)basis ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@a", (object)app ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@s", sum);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool UpdateCashOrder(int orderId, int? personId, string basis, string appendix)
        {
            string sql = @"UPDATE cash_orders 
                   SET person_id = @pId, 
                       base = @basis, 
                       appendix = @app 
                   WHERE id = @id";

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@pId", (object)personId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@basis", basis);
                    cmd.Parameters.AddWithValue("@app", appendix);
                    cmd.Parameters.AddWithValue("@id", orderId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Методы склонения Фамилии Имени Отчества
        public string GetPersonDative(string last, string first, string middle)
        {
            if (string.IsNullOrEmpty(last)) return "";

            // 1. Определяем пол по отчеству (наиболее надежный способ для документов)
            bool isMale = true;
            if (!string.IsNullOrEmpty(middle) && (middle.ToLower().EndsWith("на") || middle.ToLower().EndsWith("а")))
                isMale = false;

            string dLast = DeclineLastName(last, isMale);
            string dFirst = DeclineFirstName(first, isMale);
            string dMiddle = DeclineMiddleName(middle, isMale);

            return string.Format("{0} {1} {2}", dLast, dFirst, dMiddle).Trim();
        }

        private string DeclineLastName(string name, bool isMale)
        {
            string low = name.ToLower();
            // Несклоняемые (на -о, -их, -ых, -ко, -е, -и, -у)
            if (low.EndsWith("о") || low.EndsWith("их") || low.EndsWith("ых") ||
                low.EndsWith("ко") || low.EndsWith("е") || low.EndsWith("и") || low.EndsWith("у"))
                return name;

            if (isMale)
            {
                if (low.EndsWith("ов") || low.EndsWith("ев") || low.EndsWith("ин") || low.EndsWith("ын")) return name + "у";
                if (low.EndsWith("ий") || low.EndsWith("ый")) return name.Substring(0, name.Length - 2) + "ому";
                if ("бвгджзклмнпрстфхцчшщ".Contains(low.Substring(low.Length - 1))) return name + "у";
                if (low.EndsWith("ь")) return name.Substring(0, name.Length - 1) + "ю";
            }
            else // Женские
            {
                if (low.EndsWith("ова") || low.EndsWith("ева") || low.EndsWith("ина") || low.EndsWith("ына") || low.EndsWith("ая"))
                    return name.Substring(0, name.Length - 1) + "ой";
            }
            return name;
        }

        private string DeclineFirstName(string name, bool isMale)
        {
            if (string.IsNullOrEmpty(name)) return "";
            string low = name.ToLower();
            if (low.EndsWith("а") || low.EndsWith("я")) return name.Substring(0, name.Length - 1) + "е";
            if (isMale)
            {
                if (low.EndsWith("й") || low.EndsWith("ь")) return name.Substring(0, name.Length - 1) + "ю";
                return name + "у";
            }
            return name;
        }

        private string DeclineMiddleName(string name, bool isMale)
        {
            if (string.IsNullOrEmpty(name)) return "";
            if (isMale) return name.EndsWith("ич") ? name + "у" : name;
            return name.EndsWith("на") ? name.Substring(0, name.Length - 1) + "е" : name;
        }

        // Метод для ПКО: Родительный падеж (от кого?)
        public string GetPersonGenitive(string last, string first, string middle)
        {
            if (string.IsNullOrEmpty(last)) return "";

            // Определяем пол (как в вашем методе)
            bool isMale = true;
            string m = middle.ToLower();
            if (!string.IsNullOrEmpty(middle) && (m.EndsWith("на") || m.EndsWith("а") || m.EndsWith("ична")))
                isMale = false;

            string gLast = DeclineLastNameGen(last, isMale);
            string gFirst = DeclineFirstNameGen(first, isMale);
            string gMiddle = DeclineMiddleNameGen(middle, isMale);

            return string.Format("{0} {1} {2}", gLast, gFirst, gMiddle).Trim();
        }

        private string DeclineLastNameGen(string name, bool isMale)
        {
            string low = name.ToLower();
            // Несклоняемые
            if (low.EndsWith("о") || low.EndsWith("их") || low.EndsWith("ых") || low.EndsWith("ко")) return name;

            if (isMale)
            {
                if (low.EndsWith("ов") || low.EndsWith("ев") || low.EndsWith("ин") || low.EndsWith("ын")) return name + "а";
                if (low.EndsWith("ий") || low.EndsWith("ый")) return name.Substring(0, name.Length - 2) + "ого";
                if ("бвгджзклмнпрстфхцчшщ".Contains(low.Substring(low.Length - 1))) return name + "а";
            }
            else // Женские (Иванова -> Ивановой, Боярина -> Бояриной)
            {
                if (low.EndsWith("а") || low.EndsWith("я")) return name.Substring(0, name.Length - 1) + "ой";
            }
            return name;
        }

        private string DeclineFirstNameGen(string name, bool isMale)
        {
            if (string.IsNullOrEmpty(name)) return "";
            string low = name.ToLower();
            if (low.EndsWith("а") || low.EndsWith("я")) return name.Substring(0, name.Length - 1) + "ы"; // Елена -> Елены
            if (isMale) return (low.EndsWith("й") || low.EndsWith("ь")) ? name.Substring(0, name.Length - 1) + "я" : name + "а";
            return name;
        }

        private string DeclineMiddleNameGen(string name, bool isMale)
        {
            if (string.IsNullOrEmpty(name)) return "";
            if (isMale) return name + "а"; // Анатольевич -> Анатольевича
            return name.EndsWith("на") ? name.Substring(0, name.Length - 1) + "ы" : name; // Петровна -> Петровны
        }

        // --- МЕТОДЫ ДЛЯ РКО (КО-2) ---
        public DataTable GetRkoReportData(int rkoId)
        {
            // Используем CASE: если p.id существует, склеиваем ФИО, иначе берем ручной ввод.
            string sql = string.Format(@"
SELECT 
    co.order_number AS [No],
    co.date AS [Date],
    co.amount AS [Sum],
    CASE 
        WHEN p.id IS NOT NULL THEN (p.last_name || ' ' || p.first_name || ' ' || p.middle_name)
        ELSE co.person_name_manual 
    END AS [Recipient],
    (td.name || ' ' || COALESCE(idd.series, '') || ' ' || idd.number || ', выдан ' || idd.issued_by || ' ' || strftime('%d.%m.%Y', idd.issue_date)) AS [Passport], 
    p.role AS [RecipientRole],
    co.base AS [Basis],
    co.appendix AS [Appendix],
    'BYN' AS [CurrencyCode],
    'Белорусский рубль' AS [CurrencyName]
FROM cash_orders co
LEFT JOIN personal p ON co.person_id = p.id
LEFT JOIN id_documents idd ON co.doc_ref_id = idd.id
LEFT JOIN type_id_document td ON idd.type_id_doc = td.id
WHERE co.id = {0}", rkoId);

            return ExecuteDataTable(sql);
        }

        public DataTable GetOrderOutTableStructure(int orderId)
        {
            DataTable dt = new DataTable();

            // Создаем колонки (имена не важны, важен порядок для dgvData)
            dt.Columns.Add("FIO");
            dt.Columns.Add("Passport");
            dt.Columns.Add("Ground");
            dt.Columns.Add("DocName");
            dt.Columns.Add("CurrencyCode");
            dt.Columns.Add("CurrencyName");

            // Строка №2: Нумерация столбцов согласно ТЗ (1, 1а, 2, 3, 4, 4а)
            dt.Rows.Add("1", "1а", "2", "3", "4", "4а");

            // Добиваем до 15 строк (с учетом строки нумерации и строки данных)
            while (dt.Rows.Count < 16)
            {
                dt.Rows.Add(string.Empty, string.Empty, string.Empty, string.Empty, "BYN", "Белорусский рубль");
            }

            return dt;
        }

        public DataTable GetOrderOutTable(int orderId, int personId, string recipientName, string selectedDocType)
        {
            DataTable dt = new DataTable();
            // Названия колонок для внутренней логики (должны совпадать с DataPropertyName в Grid)
            dt.Columns.Add("FIO");
            dt.Columns.Add("Passport");
            dt.Columns.Add("Ground");
            dt.Columns.Add("DocName"); // Везде теперь DocName
            dt.Columns.Add("CurrencyCode");
            dt.Columns.Add("CurrencyName");

            // СТРОКА 1: Заголовки (как на вашем скриншоте)
            dt.Rows.Add("Фамилия, имя, отчество", "Документ, удостоверяющий личность", "Основание выдачи денег", "Наименование документа", "Код", "Валюта");

            // СТРОКА 2: Технические номера
            dt.Rows.Add("1", "1а", "2", "3", "4", "4а");

            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                // 1. Логика имени: если пусто, ищем Настоятеля в БД
                string finalRecipient = recipientName;
                if (string.IsNullOrEmpty(finalRecipient))
                {
                    string rectorSql = "SELECT last_name || ' ' || first_name || ' ' || middle_name FROM personal WHERE role LIKE '%Настоятель%' LIMIT 1";
                    using (SQLiteCommand cmd = new SQLiteCommand(rectorSql, conn))
                    {
                        object res = cmd.ExecuteScalar();
                        finalRecipient = res != null ? res.ToString() : "Настоятель";
                    }
                }

                // 2. Паспортные данные
                string passportStr = "";
                if (personId > 0)
                {
                    string passportSql = @"
                SELECT td.name || ' ' || id.series || ' ' || id.number || ', выдан ' || id.issued_by
                FROM id_documents id
                JOIN type_id_document td ON id.type_id_doc = td.id
                WHERE id.employee_id = @pId LIMIT 1";
                    using (SQLiteCommand cmd = new SQLiteCommand(passportSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@pId", personId);
                        object res = cmd.ExecuteScalar();
                        if (res != null) passportStr = res.ToString();
                    }
                }

                // 3. Получаем список статей расхода и заполняем таблицу построчно
                string itemsSql = "SELECT category FROM expense_items WHERE doc_id = @id ORDER BY id ASC";
                using (SQLiteCommand cmd = new SQLiteCommand(itemsSql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", orderId);
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        bool isFirstDataRow = true;
                        while (rdr.Read())
                        {
                            dt.Rows.Add(
                                isFirstDataRow ? finalRecipient : string.Empty,
                                isFirstDataRow ? passportStr : string.Empty,
                                rdr["category"].ToString(),
                                isFirstDataRow ? selectedDocType : string.Empty,
                                "BYN",
                                "Белорусский рубль"
                            );
                            isFirstDataRow = false;
                        }
                    }
                }

                // Добиваем до 16 строк
                while (dt.Rows.Count < 16) { dt.Rows.Add(string.Empty, string.Empty, string.Empty, string.Empty, "BYN", "Белорусский рубль"); }
            }
            return dt;
        }

        public bool UpdateRkoDetails(int orderId, int personId, string docName)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                // Обновляем связь с человеком и тип документа для этого ордера
                string sql = @"UPDATE OrdersOut 
                       SET person_id = @pId, 
                           document_name = @doc 
                       WHERE id = @orderId";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@pId", personId);
                    cmd.Parameters.AddWithValue("@doc", docName);
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public string GetPassportInfo(int personId)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT t.name as doc_type, d.series, d.number, d.issued_by, d.issue_date 
                       FROM id_documents d
                       LEFT JOIN type_id_document t ON d.type_id_doc = t.id
                       WHERE d.employee_id = @id LIMIT 1";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", personId);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // 1. Получаем номер. Если его нет — сразу возвращаем пустоту
                            string number = dr["number"] != DBNull.Value ? dr["number"].ToString().Trim() : "";
                            if (string.IsNullOrEmpty(number)) return string.Empty;

                            // 2. Получаем остальные поля
                            string docType = dr["doc_type"] != DBNull.Value ? dr["doc_type"].ToString().Trim() : "Паспорт";
                            string series = dr["series"] != DBNull.Value ? dr["series"].ToString().Trim() : "";
                            string issuedBy = dr["issued_by"] != DBNull.Value ? dr["issued_by"].ToString().Trim() : "";
                            string dateStr = dr["issue_date"] != DBNull.Value
                                ? Convert.ToDateTime(dr["issue_date"]).ToShortDateString()
                                : "";

                            // 3. Собираем строку: сначала тип, серия и номер
                            string result = string.Format("{0} {1} {2}", docType, series, number).Trim();

                            // 4. Добавляем "выдан" ТОЛЬКО если есть данные о выдаче
                            if (!string.IsNullOrEmpty(issuedBy) || !string.IsNullOrEmpty(dateStr))
                            {
                                result += ", выдан " + issuedBy + " " + dateStr;
                            }

                            return result.Trim();
                        }
                    }
                }
            }
            // Если записи в базе вообще нет (человек не найден)
            return string.Empty;
        }

        public DataTable GetOrderOutTable(int id, string fio, string passport)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FIO");
            dt.Columns.Add("Passport");
            dt.Columns.Add("Ground");
            dt.Columns.Add("DocName");
            dt.Columns.Add("CurrencyCode");
            dt.Columns.Add("CurrencyName");

            // СТРОКА 1: Текстовые заголовки
            DataRow titleRow = dt.NewRow();
            titleRow["FIO"] = "Фамилия, собственное имя и отчество (если таковое имеется)";
            titleRow["Passport"] = "Документ, удостоверяющий личность";
            titleRow["Ground"] = "Основание выдачи денег";
            titleRow["DocName"] = "Наименование документа";
            titleRow["CurrencyCode"] = "Код валюты";
            titleRow["CurrencyName"] = "Наименование валюты";
            dt.Rows.Add(titleRow);

            // СТРОКА 2: Нумерация (1, 1а, 2...)
            DataRow numRow = dt.NewRow();
            numRow["FIO"] = "1";
            numRow["Passport"] = "1а";
            numRow["Ground"] = "2";
            numRow["DocName"] = "3";
            numRow["CurrencyCode"] = "4";
            numRow["CurrencyName"] = "4а";
            dt.Rows.Add(numRow);

            // Дополняем до нужного количества строк (например, до 15-16 всего)
            while (dt.Rows.Count < 16)
            {
                dt.Rows.Add(dt.NewRow());
            }

            return dt;
        }

        // Попытка запустить таблицу для РКО
        public DataTable GetRkoRegistryData()
        {
            DataTable dt = new DataTable();
            string sql = @"
        SELECT 
            p.last_name, 
            (p.first_name || ' ' || p.middle_name) as first_mid,
            (COALESCE(d.series, '') || ' ' || COALESCE(d.number, '') || ', выдан ' || COALESCE(d.issued_by, '')) as passport_full,
            COALESCE(e.description, '') as expense_reason
        FROM personal p
        LEFT JOIN id_documents d ON p.id = d.employee_id
        LEFT JOIN expense_items e ON p.id = e.doc_id
        WHERE p.role != 'Настоятель храма'
        ORDER BY p.last_name ASC";

            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex) { throw new Exception("Ошибка БД: " + ex.Message); }
            }
            return dt;
        }

        public DataTable GetRkoTableData(int rkoId)
        {
            DataTable dt = new DataTable();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                // Запрос выводит ФИО и Паспорт ТОЛЬКО в первой строке (индекс 0)
                string sql = string.Format(@"
            SELECT 
                CASE WHEN i.id = (SELECT MIN(id) FROM expense_items WHERE doc_id = co.id) 
                     THEN (p.last_name || ' ' || p.first_name || ' ' || p.middle_name)
                     ELSE '' END AS [1],
                CASE WHEN i.id = (SELECT MIN(id) FROM expense_items WHERE doc_id = co.id) AND idd.number IS NOT NULL 
                     THEN (COALESCE(td.name, 'Паспорт') || ' ' || COALESCE(idd.series, '') || ' ' || idd.number || ', выдан ' || COALESCE(idd.issued_by, ''))
                     ELSE '' END AS [1а],
                i.category AS [2],       -- Основание (Электроэнергия...)
                '' AS [3],               -- Наименование документа (для ComboBox)
                'BYN' AS [4],
                'Белорусский рубль' AS [4а],
                i.amount AS [Sum_Hidden]
            FROM expense_items i
            JOIN cash_orders co ON i.doc_id = co.id 
            LEFT JOIN personal p ON co.person_id = p.id
            LEFT JOIN id_documents idd ON p.id = idd.employee_id 
            LEFT JOIN type_id_document td ON idd.type_id_doc = td.id
            WHERE co.id = {0}
            ORDER BY i.id", rkoId);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
                adapter.Fill(dt);
            }
            return dt;
        }

        public List<string> GetOrderExpenseItems(int orderId)
        {
            List<string> items = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                // Связываем ордер с пунктами расхода через doc_ref_id
                string sql = @"SELECT ei.item_name 
                       FROM expense_items ei
                       JOIN cash_orders co ON ei.expense_doc_id = co.doc_ref_id
                       WHERE co.id = @id";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", orderId);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            items.Add(dr["item_name"].ToString());
                        }
                    }
                }
            }
            return items;
        }

        public List<string> GetRkoBasisItems(int orderId)
        {
            List<string> items = new List<string>();

            // Запрос: идем от кассового ордера к пунктам расхода
            // Используем COALESCE, чтобы не было ошибок, если description пустой
            string sql = @"
        SELECT 
            ei.category || ' (' || COALESCE(ei.description, '') || ')' as full_item
        FROM expense_items ei
        INNER JOIN cash_orders co ON ei.doc_id = co.doc_ref_id
        WHERE co.id = @orderId";

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // Убираем пустые скобки, если описания нет
                            string item = dr["full_item"].ToString().Replace(" ()", "").Trim();
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }      
    }
}
