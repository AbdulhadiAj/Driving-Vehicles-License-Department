using DVLD_BusinessLoginLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.International_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.International_Driving_License_Application
{
    public partial class frmAddInternationalDrivingLicense : Form
    {
        private clsInternationalLicense _internationalLicense;

        public frmAddInternationalDrivingLicense()
        {
            InitializeComponent();
            _internationalLicense = new clsInternationalLicense();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddInternationalDrivingLicense_Load(object sender, EventArgs e)
        {
            btnShowLicenseInfo.Enabled = false;
            btnShowLicensesHistory.Enabled = false;
            btnIssue.Enabled = false;

            lblApplicationDate.Text = DateTime.Today.ToShortDateString();
            lblExpirationDate.Text = DateTime.Today.AddYears(1).ToShortDateString();
            lblIssueDate.Text = DateTime.Today.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            lblFees.Text = clsApplicationType.GetApplicationTypeFees(Convert.ToInt32(clsApplication.enApplicationType.NewInternationalLicense)).ToString();
        }

        private void btnShowLicensesHistory_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory form = new frmShowPersonLicenseHistory(clsApplication.GetApplicantPersonID(clsLicense.GetLicense(ctrlLicenseCardWithFilter1.LicenseID).ApplicationID));
            form.ShowDialog();
        }

        private void ctrlLicenseCardWithFilter1_OnLicenseSelected(object sender, EventArgs e)
        {
            btnShowLicensesHistory.Enabled = true;
            lblLocalLicenseID.Text = ctrlLicenseCardWithFilter1.LicenseID.ToString();
            btnIssue.Enabled = true;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if(clsInternationalLicense.HasLicense(clsLicense.GetPersonID(ctrlLicenseCardWithFilter1.LicenseID)))
            {
                MessageBox.Show("This person already has an international license", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!clsLicense.IsLicenseActiveAndValid(ctrlLicenseCardWithFilter1.LicenseID))
            {
                MessageBox.Show("This license is not active, choose another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(clsLicense.GetLicenseClassID(ctrlLicenseCardWithFilter1.LicenseID) != 3)
            {
                MessageBox.Show("This license is not belong to class 3, choose another one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsApplication application = new clsApplication();
            application.ApplicantPersonID = clsLicense.GetPersonID(ctrlLicenseCardWithFilter1.LicenseID);
            application.ApplicationDate = DateTime.Today;
            application.ApplicationTypeID = Convert.ToInt16(clsApplication.enApplicationType.NewInternationalLicense);
            application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            application.LastStatusDate = DateTime.Today;
            application.PaidFees = clsApplicationType.GetApplicationTypeFees(application.ApplicationTypeID);
            application.CreatedByUserID = clsGlobal.CurrentUser.UserId;
            application.Save();

            clsDriver driver = clsDriver.GetDriver(clsLicense.GetPersonID(ctrlLicenseCardWithFilter1.LicenseID));
            if(driver.DriverID == -1)
            {
                driver.PersonID = clsLicense.GetPersonID(ctrlLicenseCardWithFilter1.LicenseID);
                driver.CreatedByUserID = clsGlobal.CurrentUser.UserId;
                driver.CreatedDate = DateTime.Today;
                driver.Save();
            }

            _internationalLicense.ApplicationID = application.ApplicationID;
            _internationalLicense.DriverID = driver.DriverID;
            _internationalLicense.IssuedUsingLocalLicenseID = ctrlLicenseCardWithFilter1.LicenseID;
            _internationalLicense.IssueDate = DateTime.Today;
            _internationalLicense.ExpirationDate = DateTime.Today.AddYears(1);
            _internationalLicense.IsActive = true;
            _internationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserId;
            
            if(_internationalLicense.Save())
            {
                MessageBox.Show($"Issued Successfully with International License ID = {_internationalLicense.InternationalLicenseID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnShowLicenseInfo.Enabled = true;
                lblILApplicationID.Text = application.ApplicationID.ToString();
                lblILLicenseID.Text = _internationalLicense.InternationalLicenseID.ToString();
            }
            else
            {
                MessageBox.Show("Failed to issue", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnShowLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowInternationalLicense form = new frmShowInternationalLicense(Convert.ToInt16(lblILLicenseID.Text));
            form.ShowDialog();
        }
    }
}
