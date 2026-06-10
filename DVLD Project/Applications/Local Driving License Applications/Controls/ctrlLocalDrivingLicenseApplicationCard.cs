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

namespace DVLD_Project.Applications.Local_Driving_License_Applications.Controls
{
    public partial class ctrlLocalDrivingLicenseApplicationCard : UserControl
    {
        private clsLocalDrivingLicenseApplication _localDrivingLicenseApplication;
        public int LocalDrivingLicenseApplicationID
        {
            get
            {
                return _localDrivingLicenseApplication != null ? _localDrivingLicenseApplication.LocalDrivingLicenseApplicationID : -1;
            }
            set
            {
                _localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplication(value);
                if (_localDrivingLicenseApplication != null)
                {
                    lblLDLApplicationID.Text = _localDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                    lblLicenseClass.Text = clsLicenseClass.GetLicenseClassName(_localDrivingLicenseApplication.LicenseClassID);
                    lblPassedTestCount.Text = _localDrivingLicenseApplication.GetNumberOfPassedTest().ToString();
                    ctrlApplicationCard1.ApplicationID = _localDrivingLicenseApplication.ApplicationID;
                }
            }
        }

        public ctrlLocalDrivingLicenseApplicationCard()
        {
            InitializeComponent();
        }
    }
}
