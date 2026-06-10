using DVLD_BusinessLoginLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.International_Licenses;
using DVLD_Project.Licenses.Local_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.Renew_Driving_License
{
    public partial class frmRenewDrivingLicense : Form
    {
        private clsLicense _license;
        private clsLicense _oldLicense;

        public frmRenewDrivingLicense()
        {
            InitializeComponent();
            _license = new clsLicense();
            _oldLicense = new clsLicense();
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

        private void frmRenewDrivingLicense_Load(object sender, EventArgs e)
        {
            btnShowLicenseInfo.Enabled = false;
            btnShowLicensesHistory.Enabled = false;
            btnRenew.Enabled = false;

            lblApplicationDate.Text = DateTime.Today.ToShortDateString();
            lblIssueDate.Text = DateTime.Today.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            lblApplicationFees.Text = clsApplicationType.GetApplicationTypeFees(Convert.ToInt32(clsApplication.enApplicationType.RenewDrivingLicense)).ToString();
        }

        private void btnShowLicensesHistory_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory form = new frmShowPersonLicenseHistory(clsApplication.GetApplicantPersonID(clsLicense.GetLicense(ctrlLicenseCardWithFilter1.LicenseID).ApplicationID));
            form.ShowDialog();
        }

        private void ctrlLicenseCardWithFilter1_OnLicenseSelected(object sender, EventArgs e)
        {
            _oldLicense = clsLicense.GetLicense(ctrlLicenseCardWithFilter1.LicenseID);

            btnShowLicensesHistory.Enabled = true;
            lblOldLicenseID.Text = _oldLicense.LicenseID.ToString();
            lblExpirationDate.Text = DateTime.Today.AddYears(clsLicenseClass.GetValidityLength(_oldLicense.LicenseClassID)).ToShortDateString();
            lblLicenseFees.Text = clsLicenseClass.GetFees(_oldLicense.LicenseClassID).ToString();
            lblTotalFees.Text = (Convert.ToInt32(lblApplicationFees.Text) + Convert.ToInt32(lblLicenseFees.Text)).ToString();
            btnRenew.Enabled = true;
        }

        private void btnShowLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowLicense form = new frmShowLicense(_license.LicenseID);
            form.ShowDialog();
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (!clsLicense.IsLicenseActive(_oldLicense.LicenseID))
            {
                MessageBox.Show("This license is not active", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsLicense.IsLicenseExpired(_oldLicense.LicenseID))
            {
                MessageBox.Show("This license have been expired, make a new license", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!clsLicense.IsLicenseWillExpiresSoon(_oldLicense.LicenseID))
            {
                MessageBox.Show("This license cannot be renewed now, you can renew a license before one month of its expiration date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsApplication application = new clsApplication();
            application.ApplicantPersonID = clsLicense.GetPersonID(_oldLicense.LicenseID);
            application.ApplicationDate = DateTime.Today;
            application.ApplicationTypeID = Convert.ToInt16(clsApplication.enApplicationType.RenewDrivingLicense);
            application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            application.LastStatusDate = DateTime.Today;
            application.PaidFees = clsApplicationType.GetApplicationTypeFees(application.ApplicationTypeID);
            application.CreatedByUserID = clsGlobal.CurrentUser.UserId;
            application.Save();

            _oldLicense.IsActive = false;
            _oldLicense.Save();

            _license.ApplicationID = application.ApplicationID;
            _license.DriverID = _oldLicense.DriverID;
            _license.LicenseClassID = _oldLicense.LicenseClassID;
            _license.IssueDate = DateTime.Today;
            _license.ExpirationDate = DateTime.Today.AddYears(clsLicenseClass.GetValidityLength(_license.LicenseClassID));
            _license.Notes = tbNotes.Text;
            _license.PaidFees = clsLicenseClass.GetFees(_license.LicenseClassID);
            _license.IsActive = true;
            _license.IssueReason = clsLicense.enIssueReason.Renew;
            _license.CreatedByUserID = clsGlobal.CurrentUser.UserId;

            if (_license.Save())
            {
                MessageBox.Show($"Renewd Successfully with License ID = {_license.LicenseID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnShowLicenseInfo.Enabled = true;
                lblRLApplicationID.Text = application.ApplicationID.ToString();
                lblRenewedLicenseID.Text = _license.LicenseID.ToString();
            }
            else
            {
                MessageBox.Show("Failed to issue", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
