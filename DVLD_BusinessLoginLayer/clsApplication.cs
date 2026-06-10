using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_BusinessLoginLayer
{

    public class clsApplication
    {
        enum enMode { AddNew, Update }
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };

        public int ApplicationID;
        public int ApplicantPersonID;
        public DateTime ApplicationDate;
        public int ApplicationTypeID;
        public enApplicationStatus ApplicationStatus;
        public DateTime LastStatusDate;
        public double PaidFees;
        public int CreatedByUserID;
        private enMode _mode;

        public clsApplication()
        {
            ApplicationID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.Today;
            ApplicationTypeID = -1;
            ApplicationStatus = enApplicationStatus.New;
            LastStatusDate = DateTime.Today;
            PaidFees = 0;
            CreatedByUserID = -1;
            _mode = enMode.AddNew;
        }

        public clsApplication(int applicationID, int applicantPersonID, DateTime applicationDate, int applicationTypeID, enApplicationStatus applicationStatus, DateTime lastStatusDate, double paidFees, int createdByUserID)
        {
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            _mode = enMode.Update;
        }

        private bool _AddApplication()
        {
            ApplicationID = Applications.AddApplication(ApplicantPersonID, ApplicationDate, ApplicationTypeID, Convert.ToInt32(ApplicationStatus), LastStatusDate, PaidFees, CreatedByUserID);
            return (ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            return Applications.UpdateApplication(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, Convert.ToInt32(ApplicationStatus), LastStatusDate, PaidFees, CreatedByUserID);
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            return Applications.DeleteApplication(ApplicationID);
        }

        public static clsApplication GetApplication(int ApplicationID)
        {
            clsApplication application = new clsApplication();
            DataTable dt = Applications.GetApplication(ApplicationID);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int applicationID = ApplicationID;
                int applicantPersonID = Convert.ToInt32(dr["ApplicantPersonID"]);
                DateTime applicationDate = Convert.ToDateTime(dr["ApplicationDate"]);
                int applicationTypeID = Convert.ToInt32(dr["ApplicationTypeID"]);
                int applicationStatus = Convert.ToInt32(dr["ApplicationStatus"]);
                DateTime lastStatusDate = Convert.ToDateTime(dr["LastStatusDate"]);
                double paidFees = Convert.ToDouble(dr["PaidFees"]);
                int createdByUserID = Convert.ToInt32(dr["CreatedByUserID"]);

                application = new clsApplication(applicationID, applicantPersonID, applicationDate, applicationTypeID, (enApplicationStatus)applicationStatus, lastStatusDate, paidFees, createdByUserID);
            }

            return application;
        }

        public static bool CancelApplication(int ApplicationID)
        {
            return Applications.CancelApplication(ApplicationID);
        }

        public static bool CompleteApplication(int ApplicationID)
        {
            return Applications.CompleteApplication(ApplicationID);
        }

        public static int GetApplicantPersonID(int ApplicationID)
        {
            return Applications.GetApplicantPersonID(ApplicationID);
        }

        public bool Save()
        {
            bool isSaved = false;

            if (_mode == enMode.AddNew)
            {
                if (_AddApplication())
                {
                    isSaved = true;
                    _mode = enMode.Update;
                }
            }
            else
            {
                if (_UpdateApplication())
                {
                    isSaved = true;
                }
            }

            return isSaved;
        }
    }
}
