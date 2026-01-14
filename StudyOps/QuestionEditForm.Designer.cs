namespace StudyOps
{
    partial class QuestionEditForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel table;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.TextBox txtSubject;

        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TextBox txtText;

        private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.TextBox txtA;

        private System.Windows.Forms.Label lblB;
        private System.Windows.Forms.TextBox txtB;

        private System.Windows.Forms.Label lblC;
        private System.Windows.Forms.TextBox txtC;

        private System.Windows.Forms.Label lblD;
        private System.Windows.Forms.TextBox txtD;

        private System.Windows.Forms.Label lblCorrect;
        private System.Windows.Forms.ComboBox cmbCorrect;

        private System.Windows.Forms.Label lblDiff;
        private System.Windows.Forms.ComboBox cmbDiff;

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.lblSubject = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();

            this.lblText = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();

            this.lblA = new System.Windows.Forms.Label();
            this.txtA = new System.Windows.Forms.TextBox();

            this.lblB = new System.Windows.Forms.Label();
            this.txtB = new System.Windows.Forms.TextBox();

            this.lblC = new System.Windows.Forms.Label();
            this.txtC = new System.Windows.Forms.TextBox();

            this.lblD = new System.Windows.Forms.Label();
            this.txtD = new System.Windows.Forms.TextBox();

            this.lblCorrect = new System.Windows.Forms.Label();
            this.cmbCorrect = new System.Windows.Forms.ComboBox();

            this.lblDiff = new System.Windows.Forms.Label();
            this.cmbDiff = new System.Windows.Forms.ComboBox();

            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // Form
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 420);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Soru Ekle / Düzenle";

            // table
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Padding = new System.Windows.Forms.Padding(12);
            this.table.ColumnCount = 2;
            this.table.RowCount = 10;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            for (int i = 0; i < this.table.RowCount; i++)
                this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));

            // Labels common
            this.lblSubject.Text = "Konu:";
            this.lblSubject.AutoSize = true;
            this.lblSubject.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            this.lblText.Text = "Soru:";
            this.lblText.AutoSize = true;
            this.lblText.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            this.lblA.Text = "A:";
            this.lblA.AutoSize = true;
            this.lblA.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            this.lblB.Text = "B:";
            this.lblB.AutoSize = true;
            this.lblB.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            this.lblC.Text = "C:";
            this.lblC.AutoSize = true;
            this.lblC.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            this.lblD.Text = "D:";
            this.lblD.AutoSize = true;
            this.lblD.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            this.lblCorrect.Text = "Doğru:";
            this.lblCorrect.AutoSize = true;
            this.lblCorrect.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            this.lblDiff.Text = "Zorluk:";
            this.lblDiff.AutoSize = true;
            this.lblDiff.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);

            // Inputs
            this.txtSubject.Dock = System.Windows.Forms.DockStyle.Fill;

            this.txtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtText.Multiline = true;
            this.txtText.Height = 60;
            this.table.RowStyles[1] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F);

            this.txtA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtD.Dock = System.Windows.Forms.DockStyle.Fill;

            this.cmbCorrect.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbCorrect.Width = 120;
            this.cmbCorrect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cmbDiff.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbDiff.Width = 160;
            this.cmbDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Buttons
            this.btnOk.Text = "Kaydet";
            this.btnOk.Width = 120;

            this.btnCancel.Text = "İptal";
            this.btnCancel.Width = 120;

            // Place controls
            this.table.Controls.Add(this.lblSubject, 0, 0);
            this.table.Controls.Add(this.txtSubject, 1, 0);

            this.table.Controls.Add(this.lblText, 0, 1);
            this.table.Controls.Add(this.txtText, 1, 1);

            this.table.Controls.Add(this.lblA, 0, 2);
            this.table.Controls.Add(this.txtA, 1, 2);

            this.table.Controls.Add(this.lblB, 0, 3);
            this.table.Controls.Add(this.txtB, 1, 3);

            this.table.Controls.Add(this.lblC, 0, 4);
            this.table.Controls.Add(this.txtC, 1, 4);

            this.table.Controls.Add(this.lblD, 0, 5);
            this.table.Controls.Add(this.txtD, 1, 5);

            this.table.Controls.Add(this.lblCorrect, 0, 6);
            this.table.Controls.Add(this.cmbCorrect, 1, 6);

            this.table.Controls.Add(this.lblDiff, 0, 7);
            this.table.Controls.Add(this.cmbDiff, 1, 7);

            var btnRow = new System.Windows.Forms.FlowLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft,
                Padding = new System.Windows.Forms.Padding(0, 0, 0, 0)
            };
            btnRow.Controls.Add(this.btnOk);
            btnRow.Controls.Add(this.btnCancel);

            this.table.RowStyles[8] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F);
            this.table.RowStyles[9] = new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F);
            this.table.Controls.Add(btnRow, 1, 9);

            this.Controls.Add(this.table);

            this.ResumeLayout(false);
        }
    }
}
