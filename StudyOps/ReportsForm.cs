using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StudyOps
{
    public partial class ReportsForm : Form
    {
        private DataGridView grid;

        private ComboBox cmbExam;
        private ComboBox cmbRange;

        private Button btnRefresh;
        private Button btnClear;

        private Label lblSummary;

        private List<Exam> _exams = new List<Exam>();
        private List<ExamResult> _results = new List<ExamResult>();

        public ReportsForm()
        {
            InitializeComponent();
            Text = "Raporlar";

            BuildUI();

            Theme.Apply(this);
            ApplyVisuals();

            LoadAll();
            ApplyFilter();
        }

        private void BuildUI()
        {
            Controls.Clear();

            // ===== TOP CARD (Filter) =====
            var cardTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 92,
                Padding = new Padding(12),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var title = new Label
            {
                Text = "Sınav Sonuçları",
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Theme.Text
            };
            cardTop.Controls.Add(title);

            cmbExam = new ComboBox
            {
                Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbExam.SelectedIndexChanged += (s, e) => ApplyFilter();

            cmbRange = new ComboBox
            {
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRange.Items.AddRange(new object[] { "Tümü", "Son 7 Gün", "Son 30 Gün" });
            cmbRange.SelectedIndex = 0;
            cmbRange.SelectedIndexChanged += (s, e) => ApplyFilter();

            btnRefresh = new Button { Text = "Yenile", Width = 120, Height = 36 };
            btnRefresh.Click += (s, e) => { LoadAll(); ApplyFilter(); };

            btnClear = new Button { Text = "Temizle", Width = 120, Height = 36 };
            btnClear.Click += (s, e) =>
            {
                if (cmbExam.Items.Count > 0) cmbExam.SelectedIndex = 0;
                cmbRange.SelectedIndex = 0;
                ApplyFilter();
            };

            var filters = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.White,
                Padding = new Padding(0, 8, 0, 0)
            };

            filters.Controls.Add(MakeField("Deneme", cmbExam, 300));
            filters.Controls.Add(MakeField("Tarih", cmbRange, 220));

            var actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 260,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = Color.White,
                Padding = new Padding(0, 22, 0, 0)
            };
            actions.Controls.Add(btnRefresh);
            actions.Controls.Add(btnClear);

            cardTop.Controls.Add(actions);
            cardTop.Controls.Add(filters);

            // ===== SUMMARY BAR =====
            lblSummary = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Padding = new Padding(16, 8, 16, 0),
                ForeColor = Theme.Muted,
                Text = "Hazır."
            };

            // ===== GRID CARD =====
            var cardGrid = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = Theme.Bg
            };

            var gridInner = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8)
            };

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            grid.Columns.Clear();
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tarih", DataPropertyName = "Tarih", FillWeight = 18 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Deneme", DataPropertyName = "Deneme", FillWeight = 32 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Toplam", DataPropertyName = "Toplam", FillWeight = 10 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Doğru", DataPropertyName = "Dogru", FillWeight = 10 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Yanlış", DataPropertyName = "Yanlis", FillWeight = 10 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Puan", DataPropertyName = "Puan", FillWeight = 10 });

            gridInner.Controls.Add(grid);
            cardGrid.Controls.Add(gridInner);

            // ===== PAGE =====
            Controls.Add(cardGrid);
            Controls.Add(lblSummary);
            Controls.Add(cardTop);
        }

        private Control MakeField(string labelText, Control input, int width)
        {
            var box = new Panel
            {
                Width = width,
                Height = 56,
                BackColor = Color.White
            };

            var lbl = new Label
            {
                Text = labelText,
                AutoSize = true,
                ForeColor = Theme.Muted
            };
            lbl.Location = new Point(0, 0);

            input.Location = new Point(0, 22);

            box.Controls.Add(lbl);
            box.Controls.Add(input);
            return box;
        }

        private void ApplyVisuals()
        {
            Theme.StyleButton(btnClear);
            Theme.StyleButton(btnRefresh);
            Theme.StyleGrid(grid);
        }

        private void LoadAll()
        {
            _exams = Storage.LoadExams() ?? new List<Exam>();
            _results = Storage.LoadResults() ?? new List<ExamResult>();

            // Deneme dropdown doldur
            cmbExam.Items.Clear();
            cmbExam.Items.Add("Tümü");
            foreach (var ex in _exams.OrderByDescending(x => x.CreatedAt))
                cmbExam.Items.Add(ex.Title);

            cmbExam.SelectedIndex = 0;
        }

        private void ApplyFilter()
        {
            var examsById = _exams.ToDictionary(x => x.Id, x => x);
            var filtered = _results.AsEnumerable();

            // Tarih filtresi
            var range = cmbRange.SelectedItem?.ToString() ?? "Tümü";
            if (range == "Son 7 Gün")
                filtered = filtered.Where(r => r.TakenAt >= DateTime.Now.AddDays(-7));
            else if (range == "Son 30 Gün")
                filtered = filtered.Where(r => r.TakenAt >= DateTime.Now.AddDays(-30));

            // Deneme filtresi (başlığa göre)
            var examTitle = cmbExam.SelectedItem?.ToString() ?? "Tümü";
            if (examTitle != "Tümü")
            {
                var examIds = _exams.Where(e => e.Title == examTitle).Select(e => e.Id).ToHashSet();
                filtered = filtered.Where(r => examIds.Contains(r.ExamId));
            }

            var list = filtered
                .OrderByDescending(x => x.TakenAt)
                .ToList();

            // View model
            var view = list.Select(r =>
            {
                var deneme = examsById.ContainsKey(r.ExamId) ? examsById[r.ExamId].Title : "Bilinmiyor";
                var yanlis = Math.Max(0, r.Total - r.Correct);

                return new
                {
                    Tarih = r.TakenAt.ToString("dd.MM.yyyy HH:mm"),
                    Deneme = deneme,
                    Toplam = r.Total,
                    Dogru = r.Correct,
                    Yanlis = yanlis,
                    Puan = r.Score
                };
            }).ToList();

            grid.DataSource = view;

            // Özet
            if (list.Count == 0)
            {
                lblSummary.Text = "Kayıt bulunamadı.";
                return;
            }

            var totalExams = list.Count;
            var avgScore = (int)Math.Round(list.Average(x => x.Score));
            var sumCorrect = list.Sum(x => x.Correct);
            var sumTotal = list.Sum(x => x.Total);
            var sumWrong = Math.Max(0, sumTotal - sumCorrect);

            lblSummary.Text = $"Toplam Sınav: {totalExams}   |   Ortalama Puan: {avgScore}   |   Doğru: {sumCorrect}   Yanlış: {sumWrong}";
        }
    }
}
