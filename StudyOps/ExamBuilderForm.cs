using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StudyOps
{
    public partial class ExamBuilderForm : Form
    {
        private TextBox txtTitle;
        private ComboBox cmbSubject;
        private NumericUpDown numCount;
        private Button btnCreate;

        private Label lblStats;
        private DataGridView gridPreview;

        // Son önizleme seçimi (Kaydet’e basınca aynı soruları kaydetmek için)
        private List<Question> _lastPicked = new List<Question>();

        public ExamBuilderForm()
        {
            InitializeComponent();
            Text = "Deneme Oluştur";

            BuildUI();

            // Tema uygulandıktan SONRA stilleri tekrar garanti altına alıyoruz
            Theme.Apply(this);
            ApplyBuilderStyles();

            LoadSubjects();
            RefreshPreview();
        }

        private void BuildUI()
        {
            Controls.Clear();

            var page = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = Theme.Bg,
                ColumnCount = 1,
                RowCount = 2
            };
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 160));
            page.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Üst kart
            var cardTop = CreateCard();
            cardTop.Padding = new Padding(14);

            var form = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 120,
                ColumnCount = 2,
                RowCount = 3
            };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));

            txtTitle = new TextBox { Width = 360, Text = "Deneme 1" };
            cmbSubject = new ComboBox { Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
            numCount = new NumericUpDown { Width = 100, Minimum = 1, Maximum = 200, Value = 10 };

            cmbSubject.SelectedIndexChanged += (s, e) => RefreshPreview();
            numCount.ValueChanged += (s, e) => RefreshPreview();
            txtTitle.TextChanged += (s, e) => { /* sadece başlık */ };

            form.Controls.Add(new Label { Text = "Başlık:", AutoSize = true, Padding = new Padding(0, 10, 0, 0) }, 0, 0);
            form.Controls.Add(txtTitle, 1, 0);

            form.Controls.Add(new Label { Text = "Konu:", AutoSize = true, Padding = new Padding(0, 10, 0, 0) }, 0, 1);
            form.Controls.Add(cmbSubject, 1, 1);

            form.Controls.Add(new Label { Text = "Soru Adedi:", AutoSize = true, Padding = new Padding(0, 10, 0, 0) }, 0, 2);
            form.Controls.Add(numCount, 1, 2);

            btnCreate = new Button { Text = "Deneme Oluştur ve Kaydet", Width = 260, Height = 40 };
            btnCreate.Click += (s, e) => CreateAndSave();

            lblStats = new Label
            {
                AutoSize = true,
                Padding = new Padding(10, 12, 0, 0),
                Text = ""
            };

            var actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            actions.Controls.Add(btnCreate);
            actions.Controls.Add(lblStats);

            cardTop.Controls.Add(actions);
            cardTop.Controls.Add(form);

            // Alt kart (Önizleme)
            var cardBottom = CreateCard();
            cardBottom.Padding = new Padding(12);

            var lbl = new Label
            {
                Text = "Önizleme (Seçilecek Sorular)",
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold)
            };
            cardBottom.Controls.Add(lbl);

            gridPreview = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Konu", DataPropertyName = "Subject", Width = 160 });
            gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Zorluk", DataPropertyName = "Difficulty", Width = 100 });
            gridPreview.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Soru", DataPropertyName = "Text", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            cardBottom.Controls.Add(gridPreview);

            page.Controls.Add(cardTop, 0, 0);
            page.Controls.Add(cardBottom, 0, 1);

            Controls.Add(page);
        }

        private Panel CreateCard()
        {
            return new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0, 0, 0, 12)
            };
        }

        private void ApplyBuilderStyles()
        {
            BackColor = Theme.Bg;

            // Buton görünürlüğü için GARANTİ
            btnCreate.FlatStyle = FlatStyle.Flat;
            btnCreate.UseVisualStyleBackColor = false;
            btnCreate.BackColor = Theme.Primary;
            btnCreate.ForeColor = Color.White;
            btnCreate.FlatAppearance.BorderSize = 0;

            Theme.StyleGrid(gridPreview);

            lblStats.ForeColor = Theme.Muted;
        }

        private void LoadSubjects()
        {
            var questions = Storage.LoadQuestions();

            var subjects = questions
                .Select(q => q.Subject ?? "")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            cmbSubject.Items.Clear();
            cmbSubject.Items.Add("Tümü");
            foreach (var s in subjects) cmbSubject.Items.Add(s);

            if (cmbSubject.Items.Count > 0) cmbSubject.SelectedIndex = 0;
        }

        private void RefreshPreview()
        {
            var subject = cmbSubject.SelectedItem != null ? cmbSubject.SelectedItem.ToString() : "Tümü";
            var count = (int)numCount.Value;

            var questions = Storage.LoadQuestions();
            var pool = questions.AsEnumerable();

            if (subject != "Tümü")
                pool = pool.Where(q => (q.Subject ?? "") == subject);

            var poolList = pool.ToList();
            if (poolList.Count == 0)
            {
                _lastPicked = new List<Question>();
                gridPreview.DataSource = _lastPicked;
                lblStats.Text = "Seçilen konuda soru yok.";
                return;
            }

            if (count > poolList.Count) count = poolList.Count;

            var rnd = new Random();
            _lastPicked = poolList.OrderBy(_ => rnd.Next()).Take(count).ToList();

            gridPreview.DataSource = _lastPicked;

            lblStats.Text = $"Soru Bankası: {questions.Count} | Havuz: {poolList.Count} | Seçilecek: {_lastPicked.Count}";
        }

        private void CreateAndSave()
        {
            var title = (txtTitle.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Başlık boş olamaz.");
                return;
            }

            // Önizleme boşsa yeniden üret
            if (_lastPicked == null || _lastPicked.Count == 0)
            {
                RefreshPreview();
                if (_lastPicked == null || _lastPicked.Count == 0)
                {
                    MessageBox.Show("Deneme oluşturulamadı: Havuz boş.");
                    return;
                }
            }

            var subject = cmbSubject.SelectedItem != null ? cmbSubject.SelectedItem.ToString() : "Tümü";

            var exam = new Exam
            {
                Id = Guid.NewGuid().ToString("N"),
                Title = title,
                Subject = subject,
                QuestionIds = _lastPicked.Select(q => q.Id).Where(id => !string.IsNullOrWhiteSpace(id)).ToList(),
                CreatedAt = DateTime.Now
            };

            var exams = Storage.LoadExams();
            exams.Add(exam);
            Storage.SaveExams(exams);

            MessageBox.Show("Deneme kaydedildi.");

            // Yeni deneme oluştururken önizlemeyi tazele
            RefreshPreview();
        }
    }
}
