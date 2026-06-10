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

namespace DVLD_Project.Applications.Applications_Types
{
    public partial class frmEditApplicationType : Form
    {
        private clsApplicationType _appType;

        public frmEditApplicationType(int ID)
        {
            InitializeComponent();
            _appType = clsApplicationType.GetApplicationType(ID);
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

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            lblApplicationTypeID.Text = _appType.ApplicationTypeID.ToString();
            tbApplicationTypeTitle.Text = _appType.ApplicationTypeTitle;
            tbApplicationTypeFees.Text = _appType.ApplicationTypeFees.ToString();
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

        private void tbApplicationTypeFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
                return;

            if (e.KeyChar == '.' && !tbApplicationTypeFees.Text.Contains("."))
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

        private void btnSavePerson_Click(object sender, EventArgs e)
        {
            if(HasErrors())
            {
                MessageBox.Show("Some fields is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _appType.ApplicationTypeTitle = tbApplicationTypeTitle.Text;
            _appType.ApplicationTypeFees = Convert.ToDouble(tbApplicationTypeFees.Text);

            if(_appType.UpdateApplicationType())
                MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Saving Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
