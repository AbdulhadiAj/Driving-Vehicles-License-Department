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

namespace DVLD_Project.Applications.Replacement_For_Damaged_Or_Lost_License
{
    public partial class frmReplaceDamagedOrLostLicense : Form
    {
        private clsLicense _license;
        private clsLicense _oldLicense;
        private bool _isDamaged;

        public frmReplaceDamagedOrLostLicense()
        {
            InitializeComponent();
            _license = new clsLicense();
            _oldLicense = new clsLicense();
            _isDamaged = true;
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

        private void frmReplaceDamagedOrLostLicense_Load(object sender, EventArgs e)
        {
            btnShowLicenseInfo.Enabled = false;
            btnShowLicensesHistory.Enabled = false;
            btnReplace.Enabled = false;

            lblApplicationDate.Text = DateTime.Today.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            lblApplicationFees.Text = clsApplicationType.GetApplicationTypeFees(Convert.ToInt32(clsApplication.enApplicationType.ReplaceDamagedDrivingLicense)).ToString();
        }

        private void btnShowLicensesHistory_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory form = new frmShowPersonLicenseHistory(clsApplication.GetApplicantPersonID(clsLicense.GetLicense(ctrlLicenseCardWithFilter1.LicenseID).ApplicationID));
            form.ShowDialog();
        }

        private void ctrlLicenseCardWithFilter1_OnLicenseSelected(object sender, EventArgs e)
        {
            _oldLicense = clsLicense.GetLicense(ctrlLicenseCardWithFilter1.LicenseID);
            lblOldLicenseID.Text = _oldLicense.LicenseID.ToString();

            btnShowLicensesHistory.Enabled = true;
            btnReplace.Enabled = true;
        }

        private void btnShowLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowLicense form = new frmShowLicense(_license.LicenseID);
            form.ShowDialog();
        }

        private void replaceFor_CheckedChanged(object sender, EventArgs e)
        {
            if(rbDamaged.Checked)
            {
                _isDamaged = true;
                lblApplicationFees.Text = clsApplicationType.GetApplicationTypeFees(Convert.ToInt32(clsApplication.enApplicationType.ReplaceDamagedDrivingLicense)).ToString();
            }
            else
            {
                _isDamaged = false;
                lblApplicationFees.Text = clsApplicationType.GetApplicationTypeFees(Convert.ToInt32(clsApplication.enApplicationType.ReplaceLostDrivingLicense)).ToString();
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (!clsLicense.IsLicenseActive(_oldLicense.LicenseID))
            {
                MessageBox.Show("This license is not active", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsApplication application = new clsApplication();
            application.ApplicantPersonID = clsLicense.GetPersonID(_oldLicense.LicenseID);
            application.ApplicationDate = DateTime.Today;
            application.ApplicationTypeID = _isDamaged ? Convert.ToInt16(clsApplication.enApplicationType.ReplaceDamagedDrivingLicense) : Convert.ToInt16(clsApplication.enApplicationType.ReplaceLostDrivingLicense);
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
            _license.PaidFees = clsLicenseClass.GetFees(_license.LicenseClassID);
            _license.IsActive = true;
            _license.IssueReason = _isDamaged ? clsLicense.enIssueReason.ReplacementForDamaged : clsLicense.enIssueReason.ReplacementForLost;
            _license.CreatedByUserID = clsGlobal.CurrentUser.UserId;

            if (_license.Save())
            {
                MessageBox.Show($"Replaced Successfully with License ID = {_license.LicenseID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnShowLicenseInfo.Enabled = true;
                lblLRApplicationID.Text = application.ApplicationID.ToString();
                lblReplacedLicenseID.Text = _license.LicenseID.ToString();
            }
            else
            {
                MessageBox.Show("Failed to issue", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
