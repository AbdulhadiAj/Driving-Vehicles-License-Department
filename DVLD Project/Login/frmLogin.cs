using System;
using System.IO;
using System.Windows.Forms;
using DVLD_BusinessLoginLayer;
using DVLD_Project.Global_Classes;
using Microsoft.Win32;

namespace DVLD_Project.Login
{
    public partial class frmLogin : Form
    {
        public clsUser User { get; private set; }

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUsername.Text) || string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                MessageBox.Show("Some fields are missing", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!clsUser.IsUserFound(tbUsername.Text))
            {
                MessageBox.Show("Invalid Username", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!clsUser.IsPasswordMatch(tbUsername.Text, tbPassword.Text))
            {
                MessageBox.Show("Incorrect Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!clsUser.IsUserActive(tbUsername.Text))
            {
                MessageBox.Show("Your account is deactivated, please contact your manager", "Not Active", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cbRememberMe.Checked)
            {
                Registry.SetValue(clsGlobal.UserRememberMeRegistryPath, "UserName", tbUsername.Text);
                Registry.SetValue(clsGlobal.UserRememberMeRegistryPath, "Password", tbPassword.Text);
                Registry.SetValue(clsGlobal.UserRememberMeRegistryPath, "IsRemeberMeChecked", true);
            }
            else
            {
                Registry.SetValue(clsGlobal.UserRememberMeRegistryPath, "UserName", "");
                Registry.SetValue(clsGlobal.UserRememberMeRegistryPath, "Password", "");
                Registry.SetValue(clsGlobal.UserRememberMeRegistryPath, "IsRemeberMeChecked", false);
            }

            User = clsUser.GetUser(tbUsername.Text, tbPassword.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            bool? isRememberMeChecked = Convert.ToBoolean(Registry.GetValue(clsGlobal.UserRememberMeRegistryPath, "IsRemeberMeChecked", false));

            if(isRememberMeChecked == true)
            {
                tbUsername.Text = Registry.GetValue(clsGlobal.UserRememberMeRegistryPath, "UserName", null).ToString() ?? "";
                tbPassword.Text = Registry.GetValue(clsGlobal.UserRememberMeRegistryPath, "Password", null).ToString() ?? "";
                cbRememberMe.Checked = true;
            }
        }
    }
}
