using System;
using System.IO;
using System.Windows.Forms;
using DVLD_BusinessLoginLayer;
using DVLD_DataAccessLayer;
using DVLD_Project.Global_Classes;
using Microsoft.Win32;
using System.Configuration;

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

            try
            {
                if (cbRememberMe.Checked)
                {
                    Registry.SetValue(ConfigurationManager.AppSettings["UserRememberMeRegistryPath"], "UserName", tbUsername.Text);
                    Registry.SetValue(ConfigurationManager.AppSettings["UserRememberMeRegistryPath"], "Password", tbPassword.Text);
                    Registry.SetValue(ConfigurationManager.AppSettings["UserRememberMeRegistryPath"], "IsRemeberMeChecked", true);
                }
                else
                {
                    Registry.SetValue(ConfigurationManager.AppSettings["UserRememberMeRegistryPath"], "UserName", "");
                    Registry.SetValue(ConfigurationManager.AppSettings["UserRememberMeRegistryPath"], "Password", "");
                    Registry.SetValue(ConfigurationManager.AppSettings["UserRememberMeRegistryPath"], "IsRemeberMeChecked", false);
                }
            }
            catch(Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }    

            User = clsUser.GetUser(tbUsername.Text, tbPassword.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                bool? isRememberMeChecked = Convert.ToBoolean(Registry.GetValue(ConfigurationManager.AppSettings["UserRememberMeRegistryPath"], "IsRemeberMeChecked", false));

                if (isRememberMeChecked == true)
                {
                    tbUsername.Text = Registry.GetValue(ConfigurationManager.AppSettings["UserRememberMeRegistryPath"], "UserName", null).ToString() ?? "";
                    tbPassword.Text = Registry.GetValue(ConfigurationManager.AppSettings["UserRememberMeRegistryPath"], "Password", null).ToString() ?? "";
                    cbRememberMe.Checked = true;
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
        }
    }
}
