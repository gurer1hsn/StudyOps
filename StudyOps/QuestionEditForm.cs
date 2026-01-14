using System;
using System.Windows.Forms;

namespace StudyOps
{
    public partial class QuestionEditForm : Form
    {
        public Question ResultQuestion { get; private set; }
        private readonly Question _editing;

        public QuestionEditForm(Question existing = null)
        {
            InitializeComponent();
            _editing = existing;

            // ✅ Layout freeze doğru kullanım
            this.SuspendLayout();

            Theme.Apply(this);

            cmbCorrect.Items.Clear();
            cmbCorrect.Items.AddRange(new object[] { "A", "B", "C", "D" });

            cmbDiff.Items.Clear();
            cmbDiff.Items.AddRange(new object[] { "Kolay", "Orta", "Zor" });

            cmbCorrect.SelectedIndex = 0;
            cmbDiff.SelectedIndex = 1;

            Theme.StylePrimary(btnOk);
            Theme.StyleButton(btnCancel);

            btnOk.Click -= BtnOk_Click;
            btnOk.Click += BtnOk_Click;

            btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            if (_editing != null)
                FillFromExisting(_editing);

            // ✅ Layout restore doğru kullanım
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void FillFromExisting(Question q)
        {
            txtSubject.Text = q.Subject ?? "";
            txtText.Text = q.Text ?? "";
            txtA.Text = q.A ?? "";
            txtB.Text = q.B ?? "";
            txtC.Text = q.C ?? "";
            txtD.Text = q.D ?? "";

            if (!string.IsNullOrWhiteSpace(q.Correct) && cmbCorrect.Items.Contains(q.Correct))
                cmbCorrect.SelectedItem = q.Correct;

            if (!string.IsNullOrWhiteSpace(q.Difficulty) && cmbDiff.Items.Contains(q.Difficulty))
                cmbDiff.SelectedItem = q.Difficulty;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtText.Text) ||
                string.IsNullOrWhiteSpace(txtA.Text) ||
                string.IsNullOrWhiteSpace(txtB.Text) ||
                string.IsNullOrWhiteSpace(txtC.Text) ||
                string.IsNullOrWhiteSpace(txtD.Text))
            {
                MessageBox.Show("Soru ve A/B/C/D seçenekleri boş olamaz.");
                return;
            }

            var q = _editing ?? new Question();

            q.Subject = (txtSubject.Text ?? "").Trim();
            q.Text = (txtText.Text ?? "").Trim();
            q.A = (txtA.Text ?? "").Trim();
            q.B = (txtB.Text ?? "").Trim();
            q.C = (txtC.Text ?? "").Trim();
            q.D = (txtD.Text ?? "").Trim();
            q.Correct = (cmbCorrect.SelectedItem != null ? cmbCorrect.SelectedItem.ToString() : "A");
            q.Difficulty = (cmbDiff.SelectedItem != null ? cmbDiff.SelectedItem.ToString() : "Orta");

            ResultQuestion = q;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
