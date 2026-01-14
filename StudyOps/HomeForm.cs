using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StudyOps
{
    public class HomeForm : Form
    {
        private Label lblQ;
        private Label lblE;
        private Label lblLast;

        public HomeForm()
        {
            Text = "Ana Sayfa";
            BuildUI();
            RefreshStats();
            Theme.Apply(this);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            RefreshStats();
        }

        private void BuildUI()
        {
            BackColor = Theme.Bg;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(18),
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Theme.Bg
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var header = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Bg };
            var title = new Label
            {
                Text = "StudyOps • Dashboard",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Theme.Text,
                Height = 38
            };
            var sub = new Label
            {
                Text = "Soru bankanı yönet, deneme oluştur, sınava gir ve raporları takip et.",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                ForeColor = Theme.Muted,
                Height = 26
            };
            header.Controls.Add(sub);
            header.Controls.Add(title);

            var cards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Theme.Bg
            };
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

            var c1 = MakeCard("Toplam Soru", out lblQ);
            var c2 = MakeCard("Toplam Deneme", out lblE);
            var c3 = MakeCard("Son Puan", out lblLast);

            cards.Controls.Add(c1, 0, 0);
            cards.Controls.Add(c2, 1, 0);
            cards.Controls.Add(c3, 2, 0);

            var hint = new WatermarkPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(18)
            };
            hint.Controls.Add(new Label
            {
                Text = "İpucu: Ctrl+F ile Soru Bankasında arama kutusuna hızlı geçebilirsin.",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 10.5f),
                ForeColor = Theme.Muted,
                Height = 28
            });

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(cards, 0, 1);
            root.Controls.Add(hint, 0, 2);

            Controls.Clear();
            Controls.Add(root);
        }

        private Panel MakeCard(string caption, out Label valueLabel)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 14, 0),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(16)
            };

            var cap = new Label
            {
                Text = caption,
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Theme.Muted
            };

            valueLabel = new Label
            {
                Text = "—",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                ForeColor = Theme.Text
            };

            card.Controls.Add(valueLabel);
            card.Controls.Add(cap);
            return card;
        }

        private void RefreshStats()
        {
            try
            {
                var qCount = Storage.LoadQuestions().Count;
                var eCount = Storage.LoadExams().Count;

                var last = Storage.LoadResults()
                    .OrderByDescending(x => x.TakenAt)
                    .FirstOrDefault();

                lblQ.Text = qCount.ToString();
                lblE.Text = eCount.ToString();
                lblLast.Text = last == null ? "—" : (last.Score + " / 100");
            }
            catch
            {
                // sessiz
            }
        }

        // Basit watermark/pattern
        private class WatermarkPanel : Panel
        {
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (var brush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
                using (var f = new Font("Segoe UI", 48f, FontStyle.Bold))
                {
                    g.DrawString("StudyOps", f, brush, new PointF(18, 60));
                }
            }
        }
    }
}
