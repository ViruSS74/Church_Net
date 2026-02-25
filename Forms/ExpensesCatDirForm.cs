using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ExpensesCategoryItem = ChurchBudget.ExpenseCategoryService.ExpenseCategoryItem;

namespace ChurchBudget.Forms
{
    public partial class ExpensesCatDirForm : Form
    {
        private ExpenseCategoryService _service;

        public ExpensesCatDirForm()
        {
            InitializeComponent();
            _service = new ExpenseCategoryService();

            // Подписка на событие загрузки (как в вашем примере)
            this.Load += new EventHandler(ExpensesCatDirForm_Load);
        }

        private void ExpensesCatDirForm_Load(object sender, EventArgs e)
        {
            this.Text = "Справочник категорий доходов";

            // 1. Настройка шрифта для TreeView
            // Делаем крупный шрифт для удобства чтения
            tvCategories.Font = new Font("Segoe UI", 12F, FontStyle.Regular);

            // 2. Увеличиваем расстояние между узлами (аналог высоты строки)
            tvCategories.ItemHeight = 30;

            // 3. Загрузка данных
            LoadData();

            // 4. Настройка кнопок (иконки и текст)
            SetupButtons();
        }

        private void SetupButtons()
        {
            // Массив кнопок для быстрой обработки
            Button[] buttons = { btnAdd, btnEdit, btnDelete, btnClose };

            foreach (var btn in buttons)
            {
                if (btn != null)
                {
                    if (btn.Image != null) btn.Image = ImageHelper.Resize(btn.Image, 24);
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.TextAlign = ContentAlignment.MiddleLeft;
                }
            }
        }

        private void LoadData()
        {
            try
            {
                tvCategories.Nodes.Clear();
                var allCategories = _service.GetAll();

                if (allCategories == null || allCategories.Count == 0) return;

                // Берем только корни
                var rootCategories = allCategories
                    .Where(c => c.ParentId == null || c.ParentId == 0)
                    .ToList();

                foreach (var cat in rootCategories)
                {
                    TreeNode parentNode = new TreeNode(cat.Name);
                    parentNode.Tag = cat.Id;
                    AddChildNodes(parentNode, cat.Id, allCategories);
                    tvCategories.Nodes.Add(parentNode);
                }

                tvCategories.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки категорий: " + ex.Message);
            }
        }

        private void AddChildNodes(TreeNode parentNode, int parentId, List<ExpensesCategoryItem> allCats)
        {
            var children = allCats.Where(c => c.ParentId == parentId).ToList();
            foreach (var child in children)
            {
                TreeNode childNode = new TreeNode(child.Name);
                childNode.Tag = child.Id;
                parentNode.Nodes.Add(childNode);
                AddChildNodes(childNode, child.Id, allCats);
            }
        }

        // --- ОБРАБОТКА КНОПОК ---
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 1. Определяем имя и ID родителя из выбранного узла дерева
            string parentName = (tvCategories.SelectedNode != null)
                ? tvCategories.SelectedNode.Text
                : "--- (Корневая)";

            int? parentId = (tvCategories.SelectedNode != null)
                ? (int?)tvCategories.SelectedNode.Tag
                : null;

            // 2. Вызываем форму, ПЕРЕДАВАЯ ей имя родителя (чтобы оно отобразилось)
            var f = new ExpensesCatEditForm(parentName);

            if (f.ShowDialog() == DialogResult.OK)
            {
                // 3. ВЫЗЫВАЕМ СОХРАНЕНИЕ В БАЗУ
                // Передаем: null (т.к. запись новая), имя из формы, ID родителя
                _service.Save(null, f.CategoryName, parentId);

                // 4. Обновляем дерево
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (tvCategories.SelectedNode == null)
            {
                MessageBox.Show("Выберите категорию для редактирования");
                return;
            }

            int id = (int)tvCategories.SelectedNode.Tag;
            string name = tvCategories.SelectedNode.Text;

            // Имя родителя берем из дерева уровнем выше
            string parentName = (tvCategories.SelectedNode.Parent != null)
                ? tvCategories.SelectedNode.Parent.Text
                : "--- (Корневая)";

            // Вызываем конструктор для РЕДАКТИРОВАНИЯ (с 3-мя параметрами)
            var f = new ExpensesCatEditForm(id, name, parentName);

            if (f.ShowDialog() == DialogResult.OK)
            {
                // Вызываем Save, передавая существующий ID
                _service.Save(id, f.CategoryName, (int?)(tvCategories.SelectedNode.Parent?.Tag));
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tvCategories.SelectedNode != null)
            {
                int id = (int)tvCategories.SelectedNode.Tag;
                string name = tvCategories.SelectedNode.Text;

                if (MessageBox.Show($"Удалить категорию '{name}'?", "Удаление",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _service.Delete(id);
                    LoadData();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}