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

namespace DVLD_Project.Tests
{
    public partial class frmScheduleTest : Form
    {
        private enum enMode { Add, Update };
        private clsTestType.enTestTypes _testType;
        private clsLocalDrivingLicenseApplication _localDrivingLicenseApplication;
        private clsTestAppointment _testAppointment;
        private bool _isRetake;
        private clsApplication _retakeApplication;
        private enMode _mode;

        public frmScheduleTest(clsTestType.enTestTypes TestType, int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _testType = TestType;
            _localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
            _testAppointment = new clsTestAppointment();
            _isRetake = clsTestAppointment.HasFailedTest(_localDrivingLicenseApplication.LocalDrivingLicenseApplicationID, Convert.ToInt32(_testType));
            _retakeApplication = new clsApplication();
            if (_isRetake)
            {
                _retakeApplication.ApplicantPersonID = clsApplication.GetApplicantPersonID(_localDrivingLicenseApplication.ApplicationID);
                _retakeApplication.ApplicationDate = DateTime.Today;
                _retakeApplication.ApplicationTypeID = Convert.ToInt32(clsApplication.enApplicationType.RetakeTest);
                _retakeApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
                _retakeApplication.LastStatusDate = DateTime.Today;
                _retakeApplication.PaidFees = clsApplicationType.GetApplicationTypeFees(_retakeApplication.ApplicationTypeID);
                _retakeApplication.CreatedByUserID = clsGlobal.CurrentUser.UserId;
                _retakeApplication.Save();
            }
            _mode = enMode.Add;
        }

        public frmScheduleTest(clsTestType.enTestTypes TestType, int LocalDrivingLicenseApplicationID, int TestAppointmentID)
        {
            InitializeComponent();
            _testType = TestType;
            _localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
            _testAppointment = clsTestAppointment.GetTestAppointment(TestAppointmentID);
            _isRetake = _testAppointment.RetakeTestApplicationID != -1;
            _retakeApplication = new clsApplication();
            if(_isRetake)
                _retakeApplication = clsApplication.GetApplication(_testAppointment.RetakeTestApplicationID);
            _mode = enMode.Update;
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

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            groupBox1.Text = clsTestType.GetTestTypeTitle(Convert.ToInt16(_testType));
            dtpDate.MinDate = DateTime.Today;
            lblUserMessage.Visible = false;

            lblTitle.Text = $"Schedule {_testType} Test";
            lblLDLAppID.Text = _localDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDLClass.Text = clsLicenseClass.GetLicenseClassName(_localDrivingLicenseApplication.LicenseClassID);
            lblName.Text = clsPerson.GetFullName(clsApplication.GetApplicantPersonID(_localDrivingLicenseApplication.ApplicationID));
            int trial = clsTestAppointment.GetTrialCount(_localDrivingLicenseApplication.LocalDrivingLicenseApplicationID, Convert.ToInt32(_testType));
            lblTrial.Text = _mode == enMode.Add ? (trial + 1).ToString() : trial.ToString();
            lblFees.Text = clsTestType.GetTestTypeFees(Convert.ToInt32(_testType)).ToString();

            if(_isRetake)
            {
                gbRetakeTest.Enabled = true;
                lblRetakeAppID.Text = _retakeApplication.ApplicationID.ToString();
                lblRetakeTestFees.Text = _retakeApplication.PaidFees.ToString();
                lblTotalFees.Text = (Convert.ToDouble(lblFees.Text) + Convert.ToDouble(lblRetakeTestFees.Text)).ToString();
            }
            else
            {
                gbRetakeTest.Enabled = false;
                lblRetakeAppID.Text = "N/A";
                lblRetakeTestFees.Text = "0";
                lblTotalFees.Text = lblFees.Text;
            }

            if (_mode == enMode.Update)
            {
                dtpDate.Value = _testAppointment.AppointmentDate;
                lblTestID.Text = _testAppointment.TestAppointmentID.ToString();
                if(_testAppointment.IsLocked)
                {
                    lblUserMessage.Visible = true;
                    dtpDate.Enabled = false;
                    btnSchedule.Enabled = false;
                }
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            _testAppointment.AppointmentDate = dtpDate.Value;

            if (_mode == enMode.Add)
            { 
                _testAppointment.TestTypeID = Convert.ToInt32(_testType);
                _testAppointment.LocalDrivingLicenseApplicationID = Convert.ToInt32(lblLDLAppID.Text);
                _testAppointment.PaidFees = _isRetake ? Convert.ToDouble(lblTotalFees.Text) : Convert.ToDouble(lblFees.Text);
                _testAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserId;
                _testAppointment.IsLocked = false;
                if(_isRetake) 
                    _testAppointment.RetakeTestApplicationID = _retakeApplication.ApplicationID;
            }

            if(_testAppointment.Save())
            {
                _mode = enMode.Update;
                lblTestID.Text = _testAppointment.TestAppointmentID.ToString();
                MessageBox.Show("Scheduled Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Saving Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
