using DVLD_BusinessLoginLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.Test_Types
{
    public partial class frmEditTestType : Form
    {
        private clsTestType _testType;

        public frmEditTestType(int ID)
        {
            InitializeComponent();
            _testType = clsTestType.GetTestType(ID);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int borderSize = 4;
            Color borderColor = Color.Black;

            ControlPaint.DrawBorder(
                e.Graphics,
                this.ClientRectangle,
                borderColor,
                borderSize,
                ButtonBorderStyle.Solid,
                borderColor,
                borderSize,
                ButtonBorderStyle.Solid,
                borderColor,
                borderSize,
                ButtonBorderStyle.Solid,
                borderColor,
                borderSize,
                ButtonBorderStyle.Solid
            );
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            lblTestTypeID.Text = _testType.TestTypeID.ToString();
            tbTestTypeTitle.Text = _testType.TestTypeTitle;
            tbTestTypeDesc.Text = _testType.TestTypeDescription;
            tbTestTypeFees.Text = _testType.TestTypeFees.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnValidating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                errorProvider1.SetError((TextBox)sender, "This field is required");
            }
            else
            {
                errorProvider1.SetError((TextBox)sender, "");
            }
        }

        private void tbTestTypeFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
                return;

            if (e.KeyChar == '.' && !tbTestTypeFees.Text.Contains("."))
                return;

            e.Handled = true;
        }

        private bool HasErrors()
        {
            this.ValidateChildren();
            foreach (Control ctrl in this.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider1.GetError(ctrl)))
                    return true;
            }
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (HasErrors())
            {
                MessageBox.Show("Some fields is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _testType.TestTypeTitle = tbTestTypeTitle.Text;
            _testType.TestTypeDescription = tbTestTypeDesc.Text;
            _testType.TestTypeFees = Convert.ToDouble(tbTestTypeFees.Text);

            if (_testType.UpdateTestType())
                MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Saving Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
