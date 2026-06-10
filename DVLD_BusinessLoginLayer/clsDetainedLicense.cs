using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_BusinessLoginLayer.clsLicense;

namespace DVLD_BusinessLoginLayer
{
    public class clsDetainedLicense
    {
        enum enMode { AddNew, Update };

        public int DetainID;
        public int LicenseID;
        public DateTime DetainDate;
        public double FineFees;
        public int CreatedByUserID;
        public bool IsReleased;
        public DateTime ReleaseDate;
        public int ReleasedByUserID;
        public int ReleaseApplicationID;
        private enMode _mode;

        public clsDetainedLicense()
        {
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.MinValue;
            FineFees = 0;
            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = DateTime.MinValue;
            ReleasedByUserID = -1;
            ReleaseApplicationID = -1;
            _mode = enMode.AddNew;
        }

        public clsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, double FineFees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
            _mode = enMode.Update;
        }

        private bool _AddLicense()
        {
            DetainID = DetainedLicenses.AddLicense(LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
            return DetainID != -1;
        }

        private bool _UpdateLicense()
        {
            return DetainedLicenses.UpdateLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
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
                if (_UpdateLicense())
                {
                    isSaved = true;
                }
            }

            return isSaved;
        }

        public static DataTable GetDetainedLicensesInfo()
        {
            return DetainedLicenses.GetLicensesInfo();
        }

        public static clsDetainedLicense GetDetainedLicense(int LicenseID)
        {
            clsDetainedLicense dLicense = new clsDetainedLicense();
            DataTable dt = DetainedLicenses.GetLicense(LicenseID);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int DetainID = Convert.ToInt32(dr["DetainID"]);
                DateTime DetainDate = Convert.ToDateTime(dr["DetainDate"]);
                double FineFees = Convert.ToDouble(dr["FineFees"]);
                int CreatedByUserID = Convert.ToInt32(dr["CreatedByUserID"]);
                bool IsReleased = Convert.ToBoolean(dr["IsReleased"]);
                DateTime ReleaseDate = Convert.ToDateTime(dr["ReleaseDate"]);
                int ReleasedByUserID = Convert.ToInt32(dr["ReleasedByUserID"]);
                int ReleaseApplicationID = Convert.ToInt32(dr["ReleaseApplicationID"]);

                dLicense = new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
            }

            return dLicense;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return DetainedLicenses.IsLicenseDetained(LicenseID);
        }

    }
}
