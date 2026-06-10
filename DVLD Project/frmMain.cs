using DVLD_BusinessLoginLayer;
using DVLD_Project.Applications.Applications_Types;
using DVLD_Project.Applications.International_Driving_License_Application;
using DVLD_Project.Applications.Local_Driving_License_Applications;
using DVLD_Project.Applications.Release_Detained_License;
using DVLD_Project.Applications.Renew_Driving_License;
using DVLD_Project.Applications.Replacement_For_Damaged_Or_Lost_License;
using DVLD_Project.Applications.Test_Types;
using DVLD_Project.Drivers;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses.Detain_License;
using DVLD_Project.People;
using DVLD_Project.Users;
using System;
using System.Windows.Forms;


namespace DVLD_Project
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManagePeople form = new frmManagePeople();
            form.ShowDialog();
        }

        private void signoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageUsers form = new frmManageUsers();
            form.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword form = new frmChangePassword(clsGlobal.CurrentUser.UserId);
            form.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowUser form = new frmShowUser(clsGlobal.CurrentUser.UserId);
            form.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes form = new frmManageApplicationTypes();
            form.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestTypes form = new frmManageTestTypes();
            form.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLocalDrivingLicenseApplications form = new frmManageLocalDrivingLicenseApplications();
            form.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingLicenseApplication form = new frmAddEditLocalDrivingLicenseApplication();
            form.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDrivers form = new frmManageDrivers();
            form.ShowDialog();
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageInternationalLicenseApplications form = new frmManageInternationalLicenseApplications();
            form.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddInternationalDrivingLicense form = new frmAddInternationalDrivingLicense();
            form.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewDrivingLicense form = new frmRenewDrivingLicense();
            form.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplaceDamagedOrLostLicense form = new frmReplaceDamagedOrLostLicense();
            form.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetainLicense form = new frmDetainLicense();
            form.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense form = new frmReleaseDetainedLicense();
            form.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDetainedLisences form = new frmManageDetainedLisences();
            form.ShowDialog();
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense form = new frmReleaseDetainedLicense();
            form.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLocalDrivingLicenseApplications form = new frmManageLocalDrivingLicenseApplications();
            form.ShowDialog();
        }
    }
}
