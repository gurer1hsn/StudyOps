using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudyOps
{
    public static class Theme
    {
        // --- Renk Paleti ---
        public static readonly Color Bg = Color.FromArgb(245, 246, 248);
        public static readonly Color PanelBg = Color.White;

        public static readonly Color Text = Color.FromArgb(25, 28, 33);
        public static readonly Color Muted = Color.FromArgb(110, 117, 124);

        public static readonly Color Border = Color.FromArgb(225, 228, 232);

        public static readonly Color Primary = Color.FromArgb(45, 125, 246);
        public static readonly Color PrimaryDark = Color.FromArgb(30, 105, 220);

        public static readonly Color Success = Color.FromArgb(34, 197, 94);
        public static readonly Color Danger = Color.FromArgb(239, 68, 68);

        // --- Genel Uygula ---
        public static void Apply(Form form)
        {
            if (form == null) return;

            form.BackColor = Bg;
            form.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            ApplyRecursive(form.Controls);
        }

        private static void ApplyRecursive(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c == null) continue;

                // Panel / Container arkaplan
                if (c is Panel || c is TableLayoutPanel || c is FlowLayoutPanel)
                    c.BackColor = Bg;

                // Text renkleri
                if (c is Label)
                    c.ForeColor = Text;

                // TextBox/ComboBox
                if (c is TextBox)
                {
                    var tb = (TextBox)c;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    tb.BackColor = Color.White;
                    tb.ForeColor = Text;
                }

                if (c is ComboBox)
                {
                    var cb = (ComboBox)c;
                    cb.BackColor = Color.White;
                    cb.ForeColor = Text;
                }

                // Grid
                if (c is DataGridView)
                    StyleGrid((DataGridView)c);

                // Menü
                if (c is MenuStrip)
                    StyleMenu((MenuStrip)c);

                if (c.HasChildren)
                    ApplyRecursive(c.Controls);
            }
        }

        // --- Buton stilleri ---
        public static void StyleButton(Button b)
        {
            if (b == null) return;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Border;
            b.BackColor = Color.White;
            b.ForeColor = Text;
            b.Cursor = Cursors.Hand;
        }

        public static void StylePrimary(Button b)
        {
            if (b == null) return;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Primary;
            b.ForeColor = Color.White;
            b.Cursor = Cursors.Hand;

            b.MouseEnter += (s, e) => { b.BackColor = PrimaryDark; };
            b.MouseLeave += (s, e) => { b.BackColor = Primary; };
        }

        public static void StyleSuccess(Button b)
        {
            if (b == null) return;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Success;
            b.ForeColor = Color.White;
            b.Cursor = Cursors.Hand;
        }

        public static void StyleDanger(Button b)
        {
            if (b == null) return;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Danger;
            b.ForeColor = Color.White;
            b.Cursor = Cursors.Hand;
        }

        // --- Grid stili ---
        public static void StyleGrid(DataGridView grid)
        {
            if (grid == null) return;

            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = Border;

            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Text;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(6);
            grid.ColumnHeadersHeight = 36;

            grid.DefaultCellStyle.ForeColor = Text;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 240, 255);
            grid.DefaultCellStyle.SelectionForeColor = Text;
            grid.DefaultCellStyle.Padding = new Padding(4);

            grid.RowTemplate.Height = 34;
        }

        // --- Menü stili ---
        public static void StyleMenu(MenuStrip menu)
        {
            if (menu == null) return;

            menu.BackColor = Color.White;
            menu.ForeColor = Text;
            menu.Renderer = new MenuRenderer();
        }

        private class MenuRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                // alttaki ince çizgi
                using (var p = new Pen(Border))
                {
                    e.Graphics.DrawLine(p, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
                }
            }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                var item = e.Item;
                var r = new Rectangle(Point.Empty, item.Size);

                if (item.Selected)
                {
                    using (var b = new SolidBrush(Color.FromArgb(245, 248, 255)))
                        e.Graphics.FillRectangle(b, r);
                }
                else
                {
                    using (var b = new SolidBrush(Color.White))
                        e.Graphics.FillRectangle(b, r);
                }
            }
        }
    }
}
