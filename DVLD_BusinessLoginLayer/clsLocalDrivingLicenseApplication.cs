using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLoginLayer
{

    public class clsLocalDrivingLicenseApplication
    {
        enum enMode { AddNew, Update}

        public int LocalDrivingLicenseApplicationID;
        public int ApplicationID;
        public int LicenseClassID;
        private enMode _mode;

        public clsLocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;
            _mode = enMode.AddNew;
        }

        public clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.LicenseClassID = LicenseClassID;
            _mode = enMode.Update;
        }

        private bool _AddLocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.AddLicenseApplication(ApplicationID, LicenseClassID);
            return (LocalDrivingLicenseApplicationID != -1);
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return LocalDrivingLicenseApplications.UpdateLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID);
        }

        public static DataTable GetLocalDrivingLicenseApplicationsInfo()
        {
            return LocalDrivingLicenseApplications.GetApplicationsInfo();
        }

        public static clsLocalDrivingLicenseApplication GetLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
            DataTable dt = LocalDrivingLicenseApplications.GetLicenseApplication(LocalDrivingLicenseApplicationID);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int localDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
                int applicationID = Convert.ToInt32(dr["ApplicationID"]);
                int licenseClassID = Convert.ToInt32(dr["LicenseClassID"]);

                LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication(localDrivingLicenseApplicationID, applicationID, licenseClassID);
            }

            return LocalDrivingLicenseApplication;
        }

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            return LocalDrivingLicenseApplications.DeleteLicenseApplication(LocalDrivingLicenseApplicationID);
        }

        public static bool HasActiveLicenseApplicationOfClass(string NationalNo, string ClassName)
        {
            return LocalDrivingLicenseApplications.HasActiveLicenseApplicationOfClass(NationalNo, ClassName);
        }

        public int GetNumberOfPassedTest()
        {
            return LocalDrivingLicenseApplications.GetNumberOfPassedTest(LocalDrivingLicenseApplicationID);
        }

        public static int GetLicenseClassID(int LocalDrivingLicenseApplicationID)
        {
            return LocalDrivingLicenseApplications.GetLicenseClassID(LocalDrivingLicenseApplicationID);
        }

        public static int GetApplicationID(int LocalDrivingLicenseApplicationID)
        {
            return LocalDrivingLicenseApplications.GetApplicationID(LocalDrivingLicenseApplicationID);
        }

        public bool Save()
        {
            bool isSaved = false;

            if (_mode == enMode.AddNew)
            {
                if (_AddLocalDrivingLicenseApplication())
                {
                    isSaved = true;
                    _mode = enMode.Update;
                }
            }
            else
            {
                if (_UpdateLocalDrivingLicenseApplication())
                {
                    isSaved = true;
                }
            }

            return isSaved;
        }
    }
}
