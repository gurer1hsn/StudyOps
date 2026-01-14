using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace StudyOps
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // MDI ise:
            IsMdiContainer = true;

            // Tema (Theme.cs)
            Theme.Apply(this);

            // Üst menü modern app bar
            StyleTopMenuAsAppBar();

            // MDI gri alanı modern arkaplan
            SetupMdiBackground();
        }

        // Aynı tip form açık ise öne getir, değilse MDI child aç
        private void OpenChild(Form child)
        {
            if (child == null) return;

            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == child.GetType())
                {
                    f.WindowState = FormWindowState.Maximized;
                    f.Activate();
                    child.Dispose();
                    return;
                }
            }

            child.MdiParent = this;
            child.StartPosition = FormStartPosition.Manual;
            child.WindowState = FormWindowState.Maximized;
            child.Show();
        }

        // Form açma sırasında hata olursa mesaj göster
        private void OpenChildSafe<T>() where T : Form, new()
        {
            try
            {
                OpenChild(new T());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Form açılamadı.\n\n" +
                    "Beklenen form: " + typeof(T).Name + "\n\n" +
                    "Hata: " + ex.Message,
                    "StudyOps",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void soruBankasiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildSafe<QuestionBankForm>();
        }

        private void denemeOlusturToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildSafe<ExamBuilderForm>();
        }

        private void sinavModuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildSafe<ExamForm>();
        }

        private void raporlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildSafe<ReportsForm>();
        }

        private void cikisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        // -------------------- GÖRSEL İYİLEŞTİRME --------------------

        private void StyleTopMenuAsAppBar()
        {
            var ms = this.Controls.OfType<MenuStrip>().FirstOrDefault();
            if (ms == null) return;

            ms.BackColor = Color.FromArgb(17, 24, 39); // koyu lacivert
            ms.ForeColor = Color.White;
            ms.Padding = new Padding(10, 6, 10, 6);
            ms.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            ms.Renderer = new ToolStripProfessionalRenderer(new AppBarColorTable());

            // Sol başa "StudyOps" etiketi ekle (varsa tekrar ekleme)
            bool hasBrand = ms.Items.OfType<ToolStripLabel>().Any(x => x.Name == "lblBrand");
            if (!hasBrand)
            {
                var brand = new ToolStripLabel("StudyOps");
                brand.Name = "lblBrand";
                brand.ForeColor = Color.White;
                brand.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
                brand.Margin = new Padding(6, 2, 14, 2);

                ms.Items.Insert(0, brand);
                ms.Items.Insert(1, new ToolStripSeparator() { Margin = new Padding(0, 0, 10, 0) });
            }

            foreach (ToolStripItem item in ms.Items)
            {
                item.ForeColor = Color.White;
                item.Padding = new Padding(10, 6, 10, 6);
                item.Margin = new Padding(2, 2, 2, 2);
            }

            this.BackColor = Color.White;
        }

        private void SetupMdiBackground()
        {
            var mdi = this.Controls.OfType<MdiClient>().FirstOrDefault();
            if (mdi == null) return;

            mdi.BackColor = Color.White;

            mdi.Paint -= Mdi_Paint;
            mdi.Paint += Mdi_Paint;

            mdi.Resize -= Mdi_Resize;
            mdi.Resize += Mdi_Resize;

            mdi.Invalidate();
        }

        private void Mdi_Resize(object sender, EventArgs e)
        {
            var c = sender as Control;
            if (c != null) c.Invalidate();
        }

        // Arka plan: gradient + hafif pattern + watermark
        private void Mdi_Paint(object sender, PaintEventArgs e)
        {
            var mdi = sender as MdiClient;
            if (mdi == null) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var r = mdi.ClientRectangle;
            if (r.Width <= 0 || r.Height <= 0) return;

            using (var br = new LinearGradientBrush(
                r,
                Color.FromArgb(245, 247, 250),
                Color.White,
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(br, r);
            }

            // Hafif nokta pattern
            using (var dot = new SolidBrush(Color.FromArgb(18, 33, 37, 41)))
            {
                int step = 28;
                for (int y = 16; y < r.Height; y += step)
                {
                    for (int x = 16; x < r.Width; x += step)
                    {
                        g.FillEllipse(dot, x, y, 2, 2);
                    }
                }
            }

            // Watermark
            string text = "StudyOps";
            using (var f = new Font("Segoe UI", 64f, FontStyle.Bold))
            using (var b = new SolidBrush(Color.FromArgb(28, 17, 24, 39)))
            {
                var size = g.MeasureString(text, f);
                float x = (r.Width - size.Width) / 2f;
                float y = (r.Height - size.Height) / 2f;
                g.DrawString(text, f, b, x, y);
            }

            // Alt slogan
            using (var f2 = new Font("Segoe UI", 14f, FontStyle.Regular))
            using (var b2 = new SolidBrush(Color.FromArgb(90, 17, 24, 39)))
            {
                string subtitle = "Soru Bankası • Deneme • Sınav • Raporlar";
                var size2 = g.MeasureString(subtitle, f2);
                g.DrawString(subtitle, f2, b2, (r.Width - size2.Width) / 2f, r.Height * 0.62f);
            }
        }

        // MenuStrip renkleri
        private class AppBarColorTable : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin { get { return Color.FromArgb(17, 24, 39); } }
            public override Color MenuStripGradientEnd { get { return Color.FromArgb(17, 24, 39); } }

            public override Color ToolStripDropDownBackground { get { return Color.FromArgb(24, 33, 50); } }
            public override Color ImageMarginGradientBegin { get { return Color.FromArgb(24, 33, 50); } }
            public override Color ImageMarginGradientMiddle { get { return Color.FromArgb(24, 33, 50); } }
            public override Color ImageMarginGradientEnd { get { return Color.FromArgb(24, 33, 50); } }

            public override Color MenuItemSelected { get { return Color.FromArgb(35, 49, 72); } }
            public override Color MenuItemBorder { get { return Color.FromArgb(60, 80, 120); } }

            public override Color MenuItemSelectedGradientBegin { get { return Color.FromArgb(35, 49, 72); } }
            public override Color MenuItemSelectedGradientEnd { get { return Color.FromArgb(35, 49, 72); } }

            public override Color MenuItemPressedGradientBegin { get { return Color.FromArgb(30, 41, 59); } }
            public override Color MenuItemPressedGradientEnd { get { return Color.FromArgb(30, 41, 59); } }
        }
    }
}
