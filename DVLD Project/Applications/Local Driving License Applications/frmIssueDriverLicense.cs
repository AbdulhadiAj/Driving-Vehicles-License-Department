using DVLD_BusinessLoginLayer;
using DVLD_Project.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.Local_Driving_License_Applications
{
    public partial class frmIssueDriverLicense : Form
    {

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public frmIssueDriverLicense(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
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

        private void frmIssueDriverLicense_Load(object sender, EventArgs e)
        {
            ctrlLocalDrivingLicenseApplicationCard1.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            clsDriver driver = clsDriver.GetDriver(clsApplication.GetApplicantPersonID(_LocalDrivingLicenseApplication.ApplicationID));
            if(driver.DriverID == -1)
            {
                driver.PersonID = clsApplication.GetApplicantPersonID(_LocalDrivingLicenseApplication.ApplicationID);
                driver.CreatedByUserID = clsGlobal.CurrentUser.UserId;
                driver.CreatedDate = DateTime.Now;
                driver.Save();
            }
            

            clsLicense license = new clsLicense();
            license.ApplicationID = _LocalDrivingLicenseApplication.ApplicationID;
            license.DriverID = driver.DriverID;
            license.LicenseClassID = _LocalDrivingLicenseApplication.LicenseClassID;
            license.IssueDate = DateTime.Now;
            license.ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.GetValidityLength(_LocalDrivingLicenseApplication.LicenseClassID));
            license.Notes = tbNotes.Text;
            license.PaidFees = clsLicenseClass.GetFees(_LocalDrivingLicenseApplication.LicenseClassID);
            license.IsActive = true;
            license.IssueReason = clsLicense.enIssueReason.FirstTime;
            license.CreatedByUserID = clsGlobal.CurrentUser.UserId;
            license.Save();

            clsApplication.CompleteApplication(_LocalDrivingLicenseApplication.ApplicationID);

            MessageBox.Show($"License Issued Successfully with LicenseID = {license.LicenseID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
