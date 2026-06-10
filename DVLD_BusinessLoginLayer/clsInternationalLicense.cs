using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_BusinessLoginLayer.clsLicense;

namespace DVLD_BusinessLoginLayer
{
    public class clsInternationalLicense
    {
        enum enMode { AddNew, Update};

        public int InternationalLicenseID;
        public int ApplicationID;
        public int DriverID;
        public int IssuedUsingLocalLicenseID;
        public DateTime IssueDate;
        public DateTime ExpirationDate;
        public bool IsActive;
        public int CreatedByUserID;
        private enMode _mode;


        public clsInternationalLicense()
        {
            InternationalLicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            IssuedUsingLocalLicenseID = -1;
            IssueDate = DateTime.Today;
            ExpirationDate = DateTime.Today;
            IsActive = false;
            CreatedByUserID = -1;
            _mode = enMode.AddNew;
        }

        public clsInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;
            _mode= enMode.Update;
        }

        private bool _AddInternationalLicense()
        {
            InternationalLicenseID = InternationalLicenses.AddInterntaionalLicense(ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            return InternationalLicenseID != -1;
        }

        public bool Save()
        {
            bool isSaved = false;

            if(_mode == enMode.AddNew)
            {
                if(_AddInternationalLicense())
                {
                    isSaved = true;
                    _mode = enMode.Update;
                }
            }

            return isSaved;
        }

        public static DataTable GetPersonLicenses(int PersonID)
        {
            return InternationalLicenses.GetPersonLicenses(PersonID);
        }

        public static DataTable GetLicensesInfo()
        {
            return InternationalLicenses.GetLicensesInfo();
        }

        public static bool HasLicense(int PersonID)
        {
            return InternationalLicenses.HasLicense(PersonID);
        }

        public static clsInternationalLicense GetInternationalLicense(int InternationalLicenseID)
        {
            clsInternationalLicense license = new clsInternationalLicense();
            DataTable dt = InternationalLicenses.GetInternationalLicense(InternationalLicenseID);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int ApplicationID = Convert.ToInt32(dr["ApplicationID"]);
                int DriverID = Convert.ToInt32(dr["DriverID"]);
                int IssuedUsingLocalLicenseID = Convert.ToInt32(dr["IssuedUsingLocalLicenseID"]);
                DateTime IssueDate = Convert.ToDateTime(dr["IssueDate"]);
                DateTime ExpirationDate = Convert.ToDateTime(dr["ExpirationDate"]);
                bool IsActive = Convert.ToBoolean(dr["IsActive"]);
                int CreatedByUserID = Convert.ToInt32(dr["CreatedByUserID"]);

                license = new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            }

            return license;
        }

    }
}
