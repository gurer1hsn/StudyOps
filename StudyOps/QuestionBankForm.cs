using System;
using System.Linq;
using System.Windows.Forms;

namespace StudyOps
{
    public partial class QuestionBankForm : Form
    {
        private DataGridView grid;
        private TextBox txtSearch;
        private ComboBox cmbSubject;
        private Button btnAdd, btnEdit, btnDelete, btnRefresh;

        private Panel cardTop;
        private Panel page;

        public QuestionBankForm()
        {
            InitializeComponent();
            Text = "Soru Bankası";

            BuildUI();

            Theme.Apply(this);          
            Theme.StyleGrid(grid);     
            Theme.StylePrimary(btnAdd); 
            Theme.StyleButton(btnEdit); 
            Theme.StyleDanger(btnDelete); 
            Theme.StyleButton(btnRefresh); 

            // ✅ görünüm için en kritik satır:
            Theme.Apply(this);

            LoadData();
        }

        private void BuildUI()
        {
            // --- ÜST KART (filtre + butonlar) ---
            cardTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 78,
                Padding = new Padding(12),
                BackColor = Theme.PanelBg
            };

            var lblSubject = new Label
            {
                Text = "Konu",
                AutoSize = true,
                ForeColor = Theme.Muted,
                Padding = new Padding(0, 8, 0, 0)
            };

            cmbSubject = new ComboBox
            {
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSubject.SelectedIndexChanged += (s, e) => ApplyFilter();

            var lblSearch = new Label
            {
                Text = "Ara",
                AutoSize = true,
                ForeColor = Theme.Muted,
                Padding = new Padding(12, 8, 0, 0)
            };

            txtSearch = new TextBox { Width = 260 };
            txtSearch.TextChanged += (s, e) => ApplyFilter();

            btnAdd = new Button { Text = "Yeni Soru", Width = 130 };
            btnEdit = new Button { Text = "Düzenle", Width = 130 };
            btnDelete = new Button { Text = "Sil", Width = 110 };
            btnRefresh = new Button { Text = "Yenile", Width = 120 };

            btnAdd.Click += (s, e) => AddQuestion();
            btnEdit.Click += (s, e) => EditSelected();
            btnDelete.Click += (s, e) => DeleteSelected();
            btnRefresh.Click += (s, e) => LoadData();

            // buton stilleri
            Theme.StylePrimary(btnAdd);
            Theme.StyleButton(btnEdit);
            Theme.StyleDanger(btnDelete);
            Theme.StyleButton(btnRefresh);

            var topRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Theme.PanelBg
            };
            topRow.Controls.Add(lblSubject);
            topRow.Controls.Add(cmbSubject);
            topRow.Controls.Add(lblSearch);
            topRow.Controls.Add(txtSearch);

            // sağa yaslı butonlar
            var rightButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 520,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Theme.PanelBg
            };
            rightButtons.Controls.Add(btnAdd);
            rightButtons.Controls.Add(btnEdit);
            rightButtons.Controls.Add(btnDelete);
            rightButtons.Controls.Add(btnRefresh);

            cardTop.Controls.Add(rightButtons);
            cardTop.Controls.Add(topRow);

            // --- GRID ---
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = false,
                MultiSelect = false
            };
            grid.CellDoubleClick += (s, e) => EditSelected();

            // ✅ grid görünümü
            Theme.StyleGrid(grid);

            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Id", DataPropertyName = "Id", Visible = false });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Konu", DataPropertyName = "Subject", Width = 160 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Zorluk", DataPropertyName = "Difficulty", Width = 100 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Soru", DataPropertyName = "Text", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            // --- SAYFA PADDING ---
            page = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = Theme.Bg
            };
            page.Controls.Add(grid);
            page.Controls.Add(cardTop);

            Controls.Clear();
            Controls.Add(page);
        }

        private void LoadData()
        {
            var list = Storage.LoadQuestions();

            var subjects = list.Select(q => q.Subject ?? "")
                               .Where(s => !string.IsNullOrWhiteSpace(s))
                               .Distinct()
                               .OrderBy(s => s)
                               .ToList();

            cmbSubject.Items.Clear();
            cmbSubject.Items.Add("Tümü");
            foreach (var s in subjects) cmbSubject.Items.Add(s);
            if (cmbSubject.SelectedIndex < 0) cmbSubject.SelectedIndex = 0;

            grid.DataSource = list;
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var all = Storage.LoadQuestions();

            var subject = cmbSubject.SelectedItem?.ToString() ?? "Tümü";
            var search = (txtSearch.Text ?? "").Trim().ToLowerInvariant();

            var filtered = all.AsEnumerable();

            if (subject != "Tümü")
                filtered = filtered.Where(q => (q.Subject ?? "") == subject);

            if (!string.IsNullOrWhiteSpace(search))
                filtered = filtered.Where(q =>
                    (q.Text ?? "").ToLowerInvariant().Contains(search) ||
                    (q.Subject ?? "").ToLowerInvariant().Contains(search)
                );

            grid.DataSource = filtered.ToList();
        }

        private Question GetSelected()
        {
            if (grid.CurrentRow == null) return null;
            return grid.CurrentRow.DataBoundItem as Question;
        }

        private void AddQuestion()
        {
            using (var frm = new QuestionEditForm(null))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var list = Storage.LoadQuestions();
                    list.Add(frm.ResultQuestion);
                    Storage.SaveQuestions(list);
                    LoadData();
                }
            }
        }

        private void EditSelected()
        {
            var selected = GetSelected();
            if (selected == null)
            {
                MessageBox.Show("Düzenlemek için bir soru seç.");
                return;
            }

            using (var frm = new QuestionEditForm(selected))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var list = Storage.LoadQuestions();
                    var idx = list.FindIndex(x => x.Id == frm.ResultQuestion.Id);
                    if (idx >= 0) list[idx] = frm.ResultQuestion;

                    Storage.SaveQuestions(list);
                    LoadData();
                }
            }
        }

        private void DeleteSelected()
        {
            var selected = GetSelected();
            if (selected == null)
            {
                MessageBox.Show("Silmek için bir soru seç.");
                return;
            }

            var ok = MessageBox.Show("Seçili soruyu silmek istiyor musun?", "Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (ok != DialogResult.Yes) return;

            var list = Storage.LoadQuestions();
            list = list.Where(x => x.Id != selected.Id).ToList();
            Storage.SaveQuestions(list);
            LoadData();
        }
    }
}
