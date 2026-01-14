namespace StudyOps
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem soruBankasiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem denemeOlusturToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sinavModuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem raporlarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cikisToolStripMenuItem;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.soruBankasiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.denemeOlusturToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sinavModuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.raporlarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cikisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.soruBankasiToolStripMenuItem,
                this.denemeOlusturToolStripMenuItem,
                this.sinavModuToolStripMenuItem,
                this.raporlarToolStripMenuItem,
                this.cikisToolStripMenuItem
            });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1000, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // soruBankasiToolStripMenuItem
            // 
            this.soruBankasiToolStripMenuItem.Name = "soruBankasiToolStripMenuItem";
            this.soruBankasiToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.soruBankasiToolStripMenuItem.Text = "Soru Bankası";
            this.soruBankasiToolStripMenuItem.Click += new System.EventHandler(this.soruBankasiToolStripMenuItem_Click);
            // 
            // denemeOlusturToolStripMenuItem
            // 
            this.denemeOlusturToolStripMenuItem.Name = "denemeOlusturToolStripMenuItem";
            this.denemeOlusturToolStripMenuItem.Size = new System.Drawing.Size(105, 20);
            this.denemeOlusturToolStripMenuItem.Text = "Deneme Oluştur";
            this.denemeOlusturToolStripMenuItem.Click += new System.EventHandler(this.denemeOlusturToolStripMenuItem_Click);
            // 
            // sinavModuToolStripMenuItem
            // 
            this.sinavModuToolStripMenuItem.Name = "sinavModuToolStripMenuItem";
            this.sinavModuToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.sinavModuToolStripMenuItem.Text = "Sınav Modu";
            this.sinavModuToolStripMenuItem.Click += new System.EventHandler(this.sinavModuToolStripMenuItem_Click);
            // 
            // raporlarToolStripMenuItem
            // 
            this.raporlarToolStripMenuItem.Name = "raporlarToolStripMenuItem";
            this.raporlarToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.raporlarToolStripMenuItem.Text = "Raporlar";
            this.raporlarToolStripMenuItem.Click += new System.EventHandler(this.raporlarToolStripMenuItem_Click);
            // 
            // cikisToolStripMenuItem
            // 
            this.cikisToolStripMenuItem.Name = "cikisToolStripMenuItem";
            this.cikisToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.cikisToolStripMenuItem.Text = "Çıkış";
            this.cikisToolStripMenuItem.Click += new System.EventHandler(this.cikisToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StudyOps";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
