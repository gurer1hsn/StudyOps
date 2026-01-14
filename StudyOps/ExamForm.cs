using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace StudyOps
{
    public partial class ExamForm : Form
    {
        private ComboBox cmbExam;
        private Button btnLoad, btnPrev, btnNext, btnFinish;

        private Label lblInfo;
        private Label lblQuestion;

        private Panel cardTop;
        private Panel cardInfo;
        private Panel cardQuestion;
        private Panel cardOptions;

        private Panel questionScrollHost;

        private OptionCard optA, optB, optC, optD;

        private List<Question> _questions = new List<Question>();
        private Exam _exam;
        private int _index = 0;

        private readonly Dictionary<string, string> _answers = new Dictionary<string, string>();
        private bool _loadingQuestion = false;

        // Sabit palette (tema bozsa bile okunur)
        private static readonly Color ExamBg = Color.FromArgb(245, 246, 248);
        private static readonly Color ExamText = Color.FromArgb(33, 37, 41);
        private static readonly Color ExamMuted = Color.FromArgb(108, 117, 125);

        public ExamForm()
        {
            InitializeComponent();
            Text = "Sınav Modu";

            BuildUI();
            ApplyExamStyles();

            LoadExams();
            SetExamUIEnabled(false);

            // Global Theme.Apply gibi şeyler yazıları bozarsa geri düzelt
            Shown += (s, e) => ForceExamColors();
            Activated += (s, e) => ForceExamColors();
        }

        private void BuildUI()
        {
            Controls.Clear();

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = ExamBg,
                ColumnCount = 1,
                RowCount = 5
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 46));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 54));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));

            // TOP
            cardTop = CreateCardPanel();
            cardTop.Padding = new Padding(8);

            var lblPick = new Label
            {
                Text = "Deneme Seç:",
                AutoSize = true,
                Padding = new Padding(0, 10, 8, 0),
                ForeColor = ExamText
            };

            cmbExam = new ComboBox
            {
                Width = 560,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnLoad = new Button { Text = "Yükle", Width = 170, Height = 40 };
            btnLoad.Click += (s, e) => LoadSelectedExam();

            var topRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(4),
                Margin = new Padding(0)
            };
            topRow.Controls.Add(lblPick);
            topRow.Controls.Add(cmbExam);
            topRow.Controls.Add(btnLoad);

            cardTop.Controls.Add(topRow);

            // INFO
            cardInfo = CreateCardPanel();
            lblInfo = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Deneme yükleyin...",
                Padding = new Padding(10),
                AutoEllipsis = true,
                ForeColor = ExamMuted
            };
            cardInfo.Controls.Add(lblInfo);

            // QUESTION
            cardQuestion = CreateCardPanel();

            questionScrollHost = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                BackColor = Color.White
            };

            lblQuestion = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(980, 0),
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = ExamText,
                Text = ""
            };

            questionScrollHost.Controls.Add(lblQuestion);
            cardQuestion.Controls.Add(questionScrollHost);

            // OPTIONS
            cardOptions = CreateCardPanel();
            cardOptions.Padding = new Padding(10);

            var optionsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            optionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            optionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            var optTitle = new Label
            {
                Text = "Seçenekler",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(6, 0, 6, 0),
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = ExamText
            };

            var optGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(0, 8, 0, 0),
                Margin = new Padding(0)
            };
            optGrid.RowStyles.Clear();
            for (int i = 0; i < 4; i++)
                optGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));

            optA = CreateOptionCard("A");
            optB = CreateOptionCard("B");
            optC = CreateOptionCard("C");
            optD = CreateOptionCard("D");

            optA.Container.Margin = new Padding(0, 0, 0, 10);
            optB.Container.Margin = new Padding(0, 0, 0, 10);
            optC.Container.Margin = new Padding(0, 0, 0, 10);
            optD.Container.Margin = new Padding(0);

            optGrid.Controls.Add(optA.Container, 0, 0);
            optGrid.Controls.Add(optB.Container, 0, 1);
            optGrid.Controls.Add(optC.Container, 0, 2);
            optGrid.Controls.Add(optD.Container, 0, 3);

            optionsLayout.Controls.Add(optTitle, 0, 0);
            optionsLayout.Controls.Add(optGrid, 0, 1);
            cardOptions.Controls.Add(optionsLayout);

            // BOTTOM
            btnPrev = new Button { Text = "← Geri", Width = 180, Height = 44 };
            btnNext = new Button { Text = "İleri →", Width = 180, Height = 44 };
            btnFinish = new Button { Text = "Bitir", Width = 180, Height = 44 };

            btnPrev.Click += (s, e) => { if (_index > 0) { _index--; ShowQuestion(); } };
            btnNext.Click += (s, e) => { if (_index < _questions.Count - 1) { _index++; ShowQuestion(); } };
            btnFinish.Click += (s, e) => FinishExam();

            var bottom = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0),
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false
            };
            bottom.Controls.Add(btnFinish);
            bottom.Controls.Add(btnNext);
            bottom.Controls.Add(btnPrev);

            root.Controls.Add(cardTop, 0, 0);
            root.Controls.Add(cardInfo, 0, 1);
            root.Controls.Add(cardQuestion, 0, 2);
            root.Controls.Add(cardOptions, 0, 3);
            root.Controls.Add(bottom, 0, 4);

            Controls.Add(root);
        }

        private Panel CreateCardPanel()
        {
            return new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
        }

        // ✅ En stabil: manuel yerleşim (UI kesin görünür)
        private OptionCard CreateOptionCard(string tag)
        {
            var p = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            var rb = new RadioButton
            {
                Tag = tag,
                Text = "",
                AutoSize = true,
                Location = new Point(12, 18)
            };

            var lbl = new Label
            {
                AutoSize = false,
                Location = new Point(42, 10),
                Size = new Size(900, 40),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                ForeColor = ExamText,
                BackColor = Color.Transparent,
                AutoEllipsis = true,
                UseMnemonic = false,
                Text = tag + ") "
            };

            p.Controls.Add(rb);
            p.Controls.Add(lbl);

            // tıklama ile seç
            p.Click += (s, e) => rb.Checked = true;
            lbl.Click += (s, e) => rb.Checked = true;
            rb.Click += (s, e) => rb.Checked = true;

            rb.CheckedChanged += Option_CheckedChanged;

            // hover
            p.MouseEnter += (s, e) => { if (!rb.Checked) p.BackColor = Color.FromArgb(248, 249, 250); };
            p.MouseLeave += (s, e) => { if (!rb.Checked) p.BackColor = Color.White; };

            return new OptionCard { Tag = tag, Container = p, Radio = rb, TextLabel = lbl };
        }

        private void ApplyExamStyles()
        {
            BackColor = ExamBg;

            lblInfo.ForeColor = ExamMuted;
            lblQuestion.ForeColor = ExamText;

            // Butonlar Theme ile boyanabilir (yazı sorunu çözülmüş durumda)
            Theme.StylePrimary(btnLoad);
            Theme.StyleButton(btnPrev);
            Theme.StylePrimary(btnNext);
            Theme.StyleSuccess(btnFinish);

            UpdateOptionVisuals();
        }

        // ✅ Dış tema yazıları bozarsa geri düzelt
        private void ForceExamColors()
        {
            BackColor = ExamBg;

            if (lblInfo != null) lblInfo.ForeColor = ExamMuted;
            if (lblQuestion != null) lblQuestion.ForeColor = ExamText;

            ForceColorsRecursive(this);

            if (optA?.TextLabel != null) optA.TextLabel.ForeColor = ExamText;
            if (optB?.TextLabel != null) optB.TextLabel.ForeColor = ExamText;
            if (optC?.TextLabel != null) optC.TextLabel.ForeColor = ExamText;
            if (optD?.TextLabel != null) optD.TextLabel.ForeColor = ExamText;
        }

        private void ForceColorsRecursive(Control c)
        {
            if (c is Label l)
            {
                if (l == lblInfo) l.ForeColor = ExamMuted;
                else l.ForeColor = ExamText;
            }
            else if (c is RadioButton rb)
            {
                rb.ForeColor = ExamText;
            }

            foreach (Control child in c.Controls)
                ForceColorsRecursive(child);
        }

        private void SetExamUIEnabled(bool enabled)
        {
            cardInfo.Enabled = enabled;
            cardQuestion.Enabled = enabled;
            cardOptions.Enabled = enabled;

            btnPrev.Enabled = enabled;
            btnNext.Enabled = enabled;
            btnFinish.Enabled = enabled;

            optA.Radio.Enabled = enabled;
            optB.Radio.Enabled = enabled;
            optC.Radio.Enabled = enabled;
            optD.Radio.Enabled = enabled;
        }

        private void LoadExams()
        {
            var exams = Storage.LoadExams()
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            cmbExam.DataSource = exams;
            cmbExam.DisplayMember = "Title";
        }

        private void LoadSelectedExam()
        {
            _exam = cmbExam.SelectedItem as Exam;
            if (_exam == null)
            {
                MessageBox.Show("Deneme yok. Önce Deneme Oluştur menüsünden deneme kaydet.");
                SetExamUIEnabled(false);
                return;
            }

            var allQuestions = Storage.LoadQuestions();

            // eski kayıtlarda Id boş olabilir
            bool changed = false;
            foreach (var q in allQuestions)
            {
                if (q != null && string.IsNullOrWhiteSpace(q.Id))
                {
                    q.Id = Guid.NewGuid().ToString("N");
                    changed = true;
                }
            }
            if (changed) Storage.SaveQuestions(allQuestions);

            _questions = allQuestions
                .Where(q => q != null
                            && !string.IsNullOrWhiteSpace(q.Id)
                            && _exam.QuestionIds != null
                            && _exam.QuestionIds.Contains(q.Id))
                .ToList();

            if (_questions.Count == 0)
            {
                MessageBox.Show("Bu denemenin soruları bulunamadı. (Soru silinmiş olabilir.)");
                SetExamUIEnabled(false);
                return;
            }

            _answers.Clear();
            _index = 0;

            SetExamUIEnabled(true);
            ShowQuestion();
            ForceExamColors();
        }

        private void ShowQuestion()
        {
            if (_questions == null || _questions.Count == 0) return;

            _loadingQuestion = true;

            if (_index < 0) _index = 0;
            if (_index > _questions.Count - 1) _index = _questions.Count - 1;

            var q = _questions[_index];
            if (q != null && string.IsNullOrWhiteSpace(q.Id))
                q.Id = Guid.NewGuid().ToString("N");

            lblInfo.Text = "Deneme: " + _exam.Title + "    •    Soru: " + (_index + 1) + "/" + _questions.Count;
            lblQuestion.Text = GetQuestionText(q);

            optA.TextLabel.Text = "A) " + GetOption(q, "A");
            optB.TextLabel.Text = "B) " + GetOption(q, "B");
            optC.TextLabel.Text = "C) " + GetOption(q, "C");
            optD.TextLabel.Text = "D) " + GetOption(q, "D");

            // reset
            optA.Radio.Checked = false;
            optB.Radio.Checked = false;
            optC.Radio.Checked = false;
            optD.Radio.Checked = false;

            // önceki cevap varsa işaretle
            if (q != null && !string.IsNullOrWhiteSpace(q.Id) && _answers.TryGetValue(q.Id, out var ans))
            {
                if (ans == "A") optA.Radio.Checked = true;
                else if (ans == "B") optB.Radio.Checked = true;
                else if (ans == "C") optC.Radio.Checked = true;
                else if (ans == "D") optD.Radio.Checked = true;
            }

            btnPrev.Enabled = _index > 0;
            btnNext.Enabled = _index < _questions.Count - 1;

            UpdateOptionVisuals();

            _loadingQuestion = false;

            ForceExamColors();
        }

        private void Option_CheckedChanged(object sender, EventArgs e)
        {
            if (_loadingQuestion) return;

            var rb = sender as RadioButton;
            if (rb == null) return;

            if (!rb.Checked)
            {
                UpdateOptionVisuals();
                return;
            }

            // ✅ tek seçim (panel farklı olduğu için)
            _loadingQuestion = true;
            if (rb != optA.Radio) optA.Radio.Checked = false;
            if (rb != optB.Radio) optB.Radio.Checked = false;
            if (rb != optC.Radio) optC.Radio.Checked = false;
            if (rb != optD.Radio) optD.Radio.Checked = false;
            _loadingQuestion = false;

            var option = rb.Tag?.ToString();
            if (string.IsNullOrWhiteSpace(option)) return;

            var q = _questions[_index];
            if (q == null) return;

            if (string.IsNullOrWhiteSpace(q.Id))
                q.Id = Guid.NewGuid().ToString("N");

            _answers[q.Id] = option;

            UpdateOptionVisuals();
            ForceExamColors();
        }

        private void UpdateOptionVisuals()
        {
            ApplyOptionVisual(optA);
            ApplyOptionVisual(optB);
            ApplyOptionVisual(optC);
            ApplyOptionVisual(optD);
        }

        private void ApplyOptionVisual(OptionCard c)
        {
            if (c == null) return;

            c.Container.BackColor = c.Radio.Checked
                ? Color.FromArgb(235, 245, 255)
                : Color.White;

            c.TextLabel.ForeColor = ExamText; // her durumda okunur
        }

        private void FinishExam()
        {
            if (_exam == null || _questions == null || _questions.Count == 0)
            {
                MessageBox.Show("Sınav bitirilemedi: Deneme yüklenmedi.");
                return;
            }

            int total = _questions.Count;
            int correct = 0;

            foreach (var q in _questions)
            {
                if (q == null) continue;
                if (string.IsNullOrWhiteSpace(q.Id)) continue;

                if (_answers.TryGetValue(q.Id, out var userAns))
                {
                    var correctAns = GetCorrectAnswer(q);

                    if (!string.IsNullOrWhiteSpace(correctAns) &&
                        string.Equals(userAns, correctAns, StringComparison.OrdinalIgnoreCase))
                    {
                        correct++;
                    }
                }
            }

            int score = total == 0 ? 0 : (int)Math.Round(correct * 100.0 / total);

            try
            {
                var results = Storage.LoadResults();
                results.Add(new ExamResult
                {
                    Id = Guid.NewGuid().ToString("N"),
                    ExamId = _exam.Id,
                    TakenAt = DateTime.Now,
                    Total = total,
                    Correct = correct,
                    Score = score
                });
                Storage.SaveResults(results);
            }
            catch { }

            MessageBox.Show(
                "Sınav Bitti!\nDoğru: " + correct + "/" + total + "\nPuan: " + score,
                "StudyOps",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            SetExamUIEnabled(false);
        }

        // =========================
        //  ✅ MODEL ADI FARKLILIKLARINA DAYANIKLI OKUMA
        // =========================

        private static string GetQuestionText(Question q)
        {
            // çoğu projede q.Text vardır, ama yoksa "QuestionText" vb. de olabilir
            var direct = TryGetStringProp(q, "Text");
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            direct = TryGetStringProp(q, "QuestionText");
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            direct = TryGetStringProp(q, "Soru");
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            return "";
        }

        private static string GetCorrectAnswer(Question q)
        {
            var direct = TryGetStringProp(q, "Correct");
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            direct = TryGetStringProp(q, "CorrectOption");
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            direct = TryGetStringProp(q, "Answer");
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            return "";
        }

        private static string GetOption(Question q, string letter)
        {
            if (q == null) return "";

            // Önce A/B/C/D alanlarını dene
            var direct = TryGetStringProp(q, letter);
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            // OptionA / OptionB / OptionC / OptionD
            direct = TryGetStringProp(q, "Option" + letter);
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            // ChoiceA / ChoiceB ...
            direct = TryGetStringProp(q, "Choice" + letter);
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            // AnswerA / AnswerB ...
            direct = TryGetStringProp(q, "Answer" + letter);
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            // SeçenekA gibi Türkçe isimler
            direct = TryGetStringProp(q, "Secenek" + letter);
            if (!string.IsNullOrWhiteSpace(direct)) return direct;

            // Options list/dizi ise: Options[0] -> A, [1] -> B...
            var optionsObj = TryGetObjProp(q, "Options") ?? TryGetObjProp(q, "Choices");
            if (optionsObj is IEnumerable<string> strEnum)
            {
                var arr = strEnum.ToList();
                int idx = letter == "A" ? 0 : letter == "B" ? 1 : letter == "C" ? 2 : 3;
                if (idx >= 0 && idx < arr.Count) return arr[idx] ?? "";
            }

            return "";
        }

        private static string TryGetStringProp(object obj, string propName)
        {
            if (obj == null) return null;

            var t = obj.GetType();
            var p = t.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p == null) return null;

            var val = p.GetValue(obj);
            return val as string;
        }

        private static object TryGetObjProp(object obj, string propName)
        {
            if (obj == null) return null;

            var t = obj.GetType();
            var p = t.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p == null) return null;

            return p.GetValue(obj);
        }

        // =========================

        private class OptionCard
        {
            public string Tag;
            public Panel Container;
            public RadioButton Radio;
            public Label TextLabel;
        }
    }
}
