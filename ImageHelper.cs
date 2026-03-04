using System.Drawing;
using System.Windows.Forms;

namespace ChurchBudget
{
    public static class ImageHelper
    {
        // Ваш существующий метод
        public static Image Resize(Image image, int size)
        {
            if (image == null) return null;
            var newImage = new Bitmap(size, size);
            using (var g = Graphics.FromImage(newImage))
            {
                // Включаем «умное» сглаживание
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                // Рисуем с учетом новых настроек
                g.DrawImage(image, 0, 0, size, size);
            }
            return newImage;
        }

        // Находит все кнопки и настраивает их
        // ImageHelper.ApplyToButtons(this, 24); - вставить это в каждый файл в метод загрузки формы
        public static void ApplyToButtons(Control parent, int iconSize)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Button)
                {
                    Button btn = (Button)ctrl;
                    if (btn.Image != null)
                    {
                        btn.Image = Resize(btn.Image, iconSize);

                        // Кнопка подстраивается под текст
                        btn.AutoSize = true;
                        btn.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                        // Красивые внутренние отступы (вокруг текста и иконки)
                        btn.Padding = new Padding(10, 5, 10, 5);

                        // Внешние отступы (расстояние между кнопками в FlowLayoutPanel)
                        btn.Margin = new Padding(5);

                        btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                        btn.TextAlign = ContentAlignment.MiddleCenter;
                        btn.ImageAlign = ContentAlignment.MiddleLeft;
                    }
                }
                if (ctrl.HasChildren) ApplyToButtons(ctrl, iconSize);
            }
        }

        // Настройка всех таблиц
        // ImageHelper.ApplyToDataGridViews(this); - вставить в каждую форму для настройки таблиц
        public static void ApplyToDataGridViews(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is DataGridView dgv)
                {
                    // Убираем тот самый служебный столбец слева
                    dgv.RowHeadersVisible = false;

                    // Настройки из вашего старого кода:
                    dgv.DefaultCellStyle.Font = new Font("Segoe UI", 11);
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    dgv.RowTemplate.Height = 30; // Высота обычных строк
                    dgv.ColumnHeadersVisible = true;
                    dgv.ColumnHeadersHeight = 35; // Высота шапки
                    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    // Дополнительно для красоты на Win 11:
                    dgv.BackgroundColor = Color.White;
                    dgv.BorderStyle = BorderStyle.None;
                    dgv.EnableHeadersVisualStyles = false; // Чтобы применился ваш цвет/шрифт заголовка
                }

                if (ctrl.HasChildren) ApplyToDataGridViews(ctrl);
            }
        }
    }
}