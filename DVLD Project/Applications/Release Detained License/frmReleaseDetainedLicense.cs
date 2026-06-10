using DVLD_BusinessLoginLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
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

namespace DVLD_Project.Applications.Release_Detained_License
{
    public partial class frmReleaseDetainedLicense : Form
    {

        private clsLicense _license;
        private clsDetainedLicense _detainedLicense;

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
            _license = new clsLicense();
            _detainedLicense = new clsDetainedLicense();
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

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            btnShowLicenseInfo.Enabled = false;
            btnShowLicensesHistory.Enabled = false;
            btnRelease.Enabled = false;

            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            lblApplicationFees.Text = clsApplicationType.GetApplicationTypeFees(Convert.ToInt32(clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense)).ToString();
            lblTotalFees.Text = lblApplicationFees.Text;
        }

        private void btnShowLicensesHistory_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory form = new frmShowPersonLicenseHistory(clsApplication.GetApplicantPersonID(clsLicense.GetLicense(ctrlLicenseCardWithFilter1.LicenseID).ApplicationID));
            form.ShowDialog();
        }

        private void ctrlLicenseCardWithFilter1_OnLicenseSelected(object sender, EventArgs e)
        {
            _license = clsLicense.GetLicense(ctrlLicenseCardWithFilter1.LicenseID);
            _detainedLicense = clsDetainedLicense.GetDetainedLicense(_license.LicenseID);

            btnShowLicensesHistory.Enabled = true;
            lblDetainID.Text = _detainedLicense.DetainID.ToString();
            lblDetainDate.Text = _detainedLicense.DetainDate.ToShortDateString();
            lblLicenseID.Text = _license.LicenseID.ToString();
            lblFineFees.Text = _detainedLicense.FineFees.ToString();
            lblTotalFees.Text = (Convert.ToInt32(lblApplicationFees.Text) + Convert.ToDouble(lblFineFees.Text)).ToString();
            btnRelease.Enabled = true;
        }

        private void btnShowLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowLicense form = new frmShowLicense(_license.LicenseID);
            form.ShowDialog();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (!clsLicense.IsLicenseActive(_license.LicenseID))
            {
                MessageBox.Show("This license is not active", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!clsDetainedLicense.IsLicenseDetained(_license.LicenseID))
            {
                MessageBox.Show("This license is not detained, choose another license", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsApplication application = new clsApplication();
            application.ApplicantPersonID = clsLicense.GetPersonID(_license.LicenseID);
            application.ApplicationDate = DateTime.Today;
            application.ApplicationTypeID = Convert.ToInt16(clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense);
            application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            application.LastStatusDate = DateTime.Today;
            application.PaidFees = clsApplicationType.GetApplicationTypeFees(application.ApplicationTypeID);
            application.CreatedByUserID = clsGlobal.CurrentUser.UserId;
            application.Save();

            _detainedLicense.IsReleased = true;
            _detainedLicense.ReleaseDate = DateTime.Today;
            _detainedLicense.ReleasedByUserID = clsGlobal.CurrentUser.UserId;
            _detainedLicense.ReleaseApplicationID = application.ApplicationID;

            if (_detainedLicense.Save())
            {
                MessageBox.Show($"Released Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnShowLicenseInfo.Enabled = true;
                lblApplicationID.Text = application.ApplicationID.ToString();
            }
            else
            {
                MessageBox.Show("Failed to issue", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
