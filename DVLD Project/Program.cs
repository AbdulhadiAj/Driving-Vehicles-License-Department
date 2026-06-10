using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Project.Global_Classes;
using DVLD_Project.Login;

namespace DVLD_Project
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                frmLogin LoginForm = new frmLogin();
                if(LoginForm.ShowDialog() != DialogResult.OK)
                    break;
                frmMain MainForm = new frmMain();
                clsGlobal.CurrentUser = LoginForm.User;
                if (MainForm.ShowDialog() != DialogResult.Retry)
                    break;
            }
        }
    }
}
