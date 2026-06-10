using DVLD_BusinessLoginLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Tests.Controls
{
    public partial class ctrlScheduledTestCard : UserControl
    {

        private clsTestAppointment _testAppointment;

        public int TestAppointmentID
        {
            get
            {
                return _testAppointment != null ? _testAppointment.TestAppointmentID : -1;
            }
            set
            {
                _testAppointment = clsTestAppointment.GetTestAppointment(value);
                if (_testAppointment != null)
                {
                    groupBox1.Text = clsTestType.GetTestTypeTitle(_testAppointment.TestTypeID);
                    lblLDLAppID.Text = _testAppointment.LocalDrivingLicenseApplicationID.ToString();
                    lblDLClass.Text = clsLicenseClass.GetLicenseClassName(clsLocalDrivingLicenseApplication.GetLicenseClassID(_testAppointment.LocalDrivingLicenseApplicationID));
                    lblName.Text = clsPerson.GetFullName(clsApplication.GetApplicantPersonID(clsLocalDrivingLicenseApplication.GetApplicationID(_testAppointment.LocalDrivingLicenseApplicationID)));
                    lblTrial.Text = clsTestAppointment.GetTrialCount(_testAppointment.LocalDrivingLicenseApplicationID, _testAppointment.TestTypeID).ToString();
                    lblDate.Text = _testAppointment.AppointmentDate.ToShortDateString();
                    lblFees.Text = _testAppointment.PaidFees.ToString();
                    int testID = clsTest.GetTestID(_testAppointment.TestAppointmentID);
                    lblTestID.Text = testID != -1 ? testID.ToString() : "Not Taken Yet";
                }
            }
        }

        public ctrlScheduledTestCard()
        {
            InitializeComponent();
        }
    }
}
