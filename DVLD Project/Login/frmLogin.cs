using System;
using System.IO;
using System.Windows.Forms;
using DVLD_BusinessLoginLayer;
using DVLD_Project.Global_Classes;

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
                File.WriteAllText(clsGlobal.DVLDUserRememberMeFilePath, $@"{tbUsername.Text}|0|{tbPassword.Text}");
            else
                File.WriteAllText(clsGlobal.DVLDUserRememberMeFilePath, "");

            User = clsUser.GetUser(tbUsername.Text, tbPassword.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if(!File.Exists(clsGlobal.DVLDUserRememberMeFilePath))
                File.Create(clsGlobal.DVLDUserRememberMeFilePath).Close();

            string line = File.ReadAllText(clsGlobal.DVLDUserRememberMeFilePath);
            if(line != "")
            {
                string[] parts = line.Split(new string[] { "|0|" }, StringSplitOptions.None);

                string username = parts[0];
                string password = parts[1];
                tbUsername.Text = username;
                tbPassword.Text = password;
                cbRememberMe.Checked = true;
            }
        }
    }
}
