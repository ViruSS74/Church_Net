using System;
using System.IO;
using System.Windows.Forms;

namespace ChurchBudget.Forms
{
    public partial class DocPreviewForm : Form
    {
        private int _docId;
        private string _type;
        private ListOfDocsService _service;

        public DocPreviewForm(int docId, string type, ListOfDocsService service)
        {
            InitializeComponent();
            _docId = docId;
            _type = type;
            _service = service;
        }

        private void DocPreviewForm_Load(object sender, EventArgs e)
        {
            try
            {
                string html = "";
                string fileName = "";

                // Определяем тип документа и генерируем HTML
                if (_type == "Income" || _type == "Доходы")
                {
                    html = _service.GenerateIncomeReportHtml(_docId);
                    fileName = string.Format("Raportichka_{0}.html", _docId);
                }
                else if (_type == "Expense" || _type == "Расходы")
                {
                    html = _service.GenerateExpenseReportHtml(_docId);
                    fileName = string.Format("Expense_{0}.html", _docId);
                }
                else
                {
                    MessageBox.Show("Неподдерживаемый тип документа: " + _type,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                // Создаём папку для временных файлов
                string tempFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "ChurchBudget_Print");

                if (!Directory.Exists(tempFolder))
                    Directory.CreateDirectory(tempFolder);

                // Сохраняем HTML файл
                string tempFile = Path.Combine(tempFolder, fileName);
                File.WriteAllText(tempFile, html, new System.Text.UTF8Encoding(true));

                // Открываем в браузере по умолчанию
                System.Diagnostics.Process.Start(tempFile);

                // Закрываем эту форму (она выполнила свою задачу)
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при формировании документа:\n" + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}