using DVLD_BusinessLoginLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.People.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Users
{
    public partial class frmChangePassword : Form
    {
        private clsUser _user;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _user = clsUser.GetUser(UserID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void tbCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbCurrentPassword.Text))
            {
                errorProvider1.SetError(tbCurrentPassword, "This field is required");
                return;
            }

            if (_user.Password != clsUtil.ComputeHash(tbCurrentPassword.Text))
            {
                errorProvider1.SetError(tbCurrentPassword, "Wrong Password");
                return;
            }

            errorProvider1.SetError(tbCurrentPassword, "");
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbConfirmPassword.Text))
            {
                errorProvider1.SetError(tbConfirmPassword, "This field is required");
                return;
            }

            if (tbConfirmPassword.Text != tbNewPassword.Text)
            {
                errorProvider1.SetError(tbConfirmPassword, "Password Not Matching");
                return;
            }

            errorProvider1.SetError(tbConfirmPassword, "");
        }

        private void tbNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                errorProvider1.SetError((TextBox)sender, "This field is required");
                return;
            }
            else
            {
                errorProvider1.SetError((TextBox)sender, "");
            }
        }

        private bool HasErrors()
        {
            bool hasErrors = false;
            this.ValidateChildren();
            foreach (Control ctrl in groupBox1.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider1.GetError(ctrl)))
                    hasErrors = true;
            }
            return hasErrors;
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (HasErrors())
            {
                MessageBox.Show("Some Fields are invalid", "Invalid Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _user.Password = clsUtil.ComputeHash(tbNewPassword.Text);
            if (_user.Save())
                MessageBox.Show("Password Changed Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Password Changing Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            ctrlUserCard1.UserId = _user.UserId;
        }
    }
}
