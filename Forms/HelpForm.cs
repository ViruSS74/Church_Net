using mshtml;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;


namespace ChurchBudget.Forms
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();

            ImageHelper.ApplyToButtons(this, 24);
        }
        
        private void HelpForm_Load(object sender, EventArgs e)
        {
            // Это покажет, какой URL браузер пытается открыть на самом деле
            MessageBox.Show(webBrowser1.Url?.ToString() ?? "Пусто");
            this.Text = "Документ: " + (webBrowser1.Document == null ? "NULL" : "OK");
            // Путь к HTML-файлу в папке с программой
            string helpPath = Path.Combine(Application.StartupPath, @"Help\index.html");
            if (File.Exists(helpPath))
            {
                MessageBox.Show(helpPath);
                // Преобразуем строку в абсолютный URI (file:///...)
                // webBrowser1.Navigate(new Uri(helpPath).AbsoluteUri);
                webBrowser1.Navigate(new Uri("Help/test.txt").AbsoluteUri);
            }
            else
            {
                MessageBox.Show("Файл справки не найден по пути: " + helpPath);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e) { webBrowser1.ShowPrintDialog(); }
        private void btnBack_Click(object sender, EventArgs e) { webBrowser1.GoBack(); }
        private void btnGo_Click(object sender, EventArgs e) { webBrowser1.GoForward(); }
        private void btnSearch_Click(object sender, EventArgs e)
        { 
            if (webBrowser1.Document == null) return;
            // Приводим документ к интерфейсу IHTMLDocument2
            IHTMLDocument2 doc = webBrowser1.Document.DomDocument as IHTMLDocument2;
            if (doc == null) return;
            // Создаем текстовый диапазон для поиска
            IHTMLSelectionObject sel = doc.selection;
            IHTMLTxtRange range = sel.createRange() as IHTMLTxtRange;
            if (range.findText(txtSearch.Text, 1000000, 0))
            {
                range.select();
                range.scrollIntoView(true);
                // Сдвигаем точку начала поиска в конец найденного фрагмента для следующего нажатия
                range.collapse(false);
            }
        }
    }
}
