using DVLD_BusinessLoginLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using DVLD_Project.Global_Classes;

namespace DVLD_Project.Users
{
    public partial class frmAddEditUser : Form
    {
        private bool _isPersonSelected = false;

        private enum enMode { Add, Update };
        private clsUser _user;
        private enMode _mode;

        public frmAddEditUser()
        {
            InitializeComponent();
            _user = new clsUser();
            _mode = enMode.Add;
        }

        public frmAddEditUser(int UserID)
        {
            InitializeComponent();
            _user = clsUser.GetUser(UserID);
            _mode = enMode.Update;
        }

        private void _FillFields()
        {
            ctrlPersonCardWithFilter1.PersonID = _user.Person.PersonID;
            lblUserID.Text = _user.UserId.ToString();
            tbUserName.Text = _user.UserName.ToString();
            tbPassword.Enabled = false;
            tbConfirmPassword.Enabled = false;
            cbIsActive.Checked = _user.IsActive;
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(object sender, EventArgs e)
        {
            _isPersonSelected = true;
            btnNext.Enabled = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!_isPersonSelected)
            {
                MessageBox.Show("Select a person first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if((clsUser.IsUserFound(ctrlPersonCardWithFilter1.PersonID) && _mode == enMode.Add) ||
                (clsUser.IsUserFound(ctrlPersonCardWithFilter1.PersonID) && _mode == enMode.Update && ctrlPersonCardWithFilter1.PersonID != _user.Person.PersonID))
            {
                MessageBox.Show("This person is a user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tabControl1.SelectedIndex = 1;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (!_isPersonSelected)
                {
                    tabControl1.SelectedIndex = 0;
                    MessageBox.Show("Select a person first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if ((clsUser.IsUserFound(ctrlPersonCardWithFilter1.PersonID) && _mode == enMode.Add) ||
                (clsUser.IsUserFound(ctrlPersonCardWithFilter1.PersonID) && _mode == enMode.Update && ctrlPersonCardWithFilter1.PersonID != _user.Person.PersonID))
                {
                    tabControl1.SelectedIndex = 0;
                    MessageBox.Show("This person is a user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            if (_mode == enMode.Update)
            {
                _FillFields();
            }
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

        private void OnValidating(object sender, CancelEventArgs e)
        {
            if (((TextBox)sender).Enabled == true)
            {
                if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
                {
                    errorProvider1.SetError((TextBox)sender, "This field is required");
                }
                else
                {
                    errorProvider1.SetError((TextBox)sender, "");
                }
            }
        }

        private void tbUserName_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbUserName.Text))
            {
                if (clsUser.IsUserFound(tbUserName.Text) && tbUserName.Text != _user.UserName)
                {
                    errorProvider1.SetError(tbUserName, "This user is already exists");
                }
                else
                {
                    errorProvider1.SetError(tbUserName, "");
                }
            }
        }

        private void tbConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbConfirmPassword.Text))
            {
                if (tbConfirmPassword.Text != tbPassword.Text)
                {
                    errorProvider1.SetError(tbConfirmPassword, "The passwords doesn't matches");
                }
                else
                {
                    errorProvider1.SetError(tbConfirmPassword, "");
                }
            }
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                errorProvider1.SetError(textBox, "This field is required");
            }
            else
            {
                errorProvider1.SetError(textBox, "");
            }
        }

        private bool HasErrors()
        {
            this.ValidateChildren();
            foreach (Control ctrl in tpUser.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider1.GetError(ctrl)))
                    return true;
            }
            return false;
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            if (HasErrors())
            {
                MessageBox.Show("Some fields is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _user.UserName = tbUserName.Text;
            _user.Password = clsUtil.ComputeHash(tbPassword.Text);
            _user.IsActive = cbIsActive.Checked;
            _user.Person = clsPerson.GetPerson(ctrlPersonCardWithFilter1.PersonID);

            if(_user.Save())
            {
                lblUserID.Text = _user.UserId.ToString();
                _mode = enMode.Update;
                MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Saving Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
