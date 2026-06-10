using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLoginLayer
{
    public class clsLicense
    {
        enum enMode { AddNew, Update };
        public enum enIssueReason { FirstTime = 1, Renew = 2, ReplacementForDamaged = 3, ReplacementForLost = 4};

        public int LicenseID;
        public int ApplicationID;
        public int DriverID;
        public int LicenseClassID;
        public DateTime IssueDate;
        public DateTime ExpirationDate;
        public string Notes;
        public double PaidFees;
        public bool IsActive;
        public enIssueReason IssueReason;
        public int CreatedByUserID;
        private enMode _mode;

        public clsLicense()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClassID = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = "";
            PaidFees = 0;
            IsActive = false;
            IssueReason = enIssueReason.FirstTime;
            CreatedByUserID = -1;
            _mode = enMode.AddNew;
        }

        public clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, double PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClassID = LicenseClassID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;
            _mode = enMode.Update;
        }

        private bool _AddLicense()
        {
            LicenseID = Licenses.AddLicense(ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, Convert.ToInt16(IssueReason), CreatedByUserID);
            return LicenseID != -1;
        }

        private bool _UpdateLicense()
        {
            return Licenses.UpdateLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, Convert.ToInt32(IssueReason), CreatedByUserID);
        }

        public bool Save()
        {
            bool isSaved = false;
            if (_mode == enMode.AddNew)
            {
                if (_AddLicense())
                {
                    _mode = enMode.Update;
                    isSaved = true;
                }
            }
            else
            {
                if(_UpdateLicense())
                {
                    isSaved = true;
                }
            }

            return isSaved;
        }

        public static clsLicense GetLicense(int LicenseID)
        {
            clsLicense license = new clsLicense();
            DataTable dt = Licenses.GetLicense(LicenseID);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int ApplicationID = Convert.ToInt32(dr["ApplicationID"]);
                int DriverID = Convert.ToInt32(dr["DriverID"]);
                int LicenseClass = Convert.ToInt32(dr["LicenseClass"]);
                DateTime IssueDate = Convert.ToDateTime(dr["IssueDate"]);
                DateTime ExpirationDate = Convert.ToDateTime(dr["ExpirationDate"]);
                string Notes = dr["Notes"].ToString();
                double PaidFees = Convert.ToDouble(dr["PaidFees"]);
                bool IsActive = Convert.ToBoolean(dr["IsActive"]);
                enIssueReason IssueReason = (enIssueReason)Convert.ToInt16(dr["IssueReason"]);
                int CreatedByUserID = Convert.ToInt32(dr["CreatedByUserID"]);

                license = new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);
            }

            return license;
        }

        public static int GetLicenseID(int ApplicationID)
        {
            return Licenses.GetLicenseID(ApplicationID);
        }

        public static DataTable GetPersonLicenses(int PersonID)
        {
            return Licenses.GetPersonLicenses(PersonID);
        }

        public static bool HasLicense(int PersonID, int LicenseClassID)
        {
            return Licenses.HasLicense(PersonID, LicenseClassID);
        }

        public static bool IsLicenseActiveAndValid(int LicenseID)
        {
            return Licenses.IsLicenseActiveAndValid(LicenseID);
        }

        public static bool IsLicenseActive(int LicenseID)
        {
            return Licenses.IsLicenseActive(LicenseID);
        }

        public static bool IsLicenseExpired(int LicenseID)
        {
            return Licenses.IsLicenseExpired(LicenseID);
        }

        public static bool IsLicenseWillExpiresSoon(int LicenseID)
        {
            return Licenses.IsLicenseWillExpiresSoon(LicenseID);
        }

        public static int GetPersonID(int LicenseID)
        {
            return Licenses.GetPersonID(LicenseID);
        }

        public static int GetLicenseClassID(int LicenseID)
        {
            return Licenses.GetLicenseClassID(LicenseID);
        }
    }
}
