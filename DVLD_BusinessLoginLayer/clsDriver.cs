using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLoginLayer
{
    public class clsDriver
    {
        enum enMode { AddNew, Update};

        public int DriverID;
        public int PersonID;
        public int CreatedByUserID;
        public DateTime CreatedDate;
        private enMode _mode;

        public clsDriver()
        {
            DriverID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;
            _mode = enMode.AddNew;
        }

        public clsDriver(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            DriverID = driverID;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            CreatedDate = createdDate;
            _mode = enMode.Update;
        }

        private bool _AddDriver()
        {
            DriverID = Drivers.AddDriver(PersonID, CreatedByUserID, CreatedDate);
            return DriverID != -1;
        }

        public bool Save()
        {
            bool isSaved = false;
            if (_mode == enMode.AddNew)
            {
                if (_AddDriver())
                {
                    _mode = enMode.Update;
                    isSaved = true;
                }
            }

            return isSaved;
        }

        public static clsDriver GetDriver(int PersonID)
        {
            clsDriver driver = new clsDriver();
            DataTable dt = Drivers.GetDriver(PersonID);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int DriverID = Convert.ToInt32(dr["DriverID"]);
                int CreatedByUserID = Convert.ToInt32(dr["CreatedByUserID"]);
                DateTime CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                driver = new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            return driver;
        }

        public static DataTable GetDriversInfo()
        {
            return Drivers.GetDriversInfo();
        }
    }
}
