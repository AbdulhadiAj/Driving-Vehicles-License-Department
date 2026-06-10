using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLoginLayer
{
    public class clsTest
    {
        enum enMode { AddNew, Update };
        public enum enTestResult { Fail = 0,  Pass = 1 };

        public int TestID;
        public int TestAppointmentID;
        public enTestResult TestResult;
        public string Notes;
        public int CreatedByUserID;
        private enMode _mode;

        public clsTest()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = enTestResult.Fail;
            Notes = "";
            CreatedByUserID = -1;
            _mode = enMode.AddNew;
        }

        public clsTest(int testID, int testAppointmentID, enTestResult testResult, string notes, int createdByUserID)
        {
            TestID = testID;
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdByUserID;
            _mode = enMode.Update;
        }

        public static int GetTestID(int testAppointmentID)
        {
            return Tests.GetTestID(testAppointmentID);
        }

        public static clsTest GetTest(int TestID)
        {
            clsTest test = new clsTest();
            DataTable dt = Tests.GetTest(TestID);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int testAppointmentID = Convert.ToInt32(dr["TestAppointmentID"]);
                enTestResult testResult = (enTestResult)Convert.ToInt32(dr["TestResult"]);
                string notes = dr["Notes"].ToString();
                int createdByUserID = Convert.ToInt32(dr["CreatedByUserID"]);
                test = new clsTest(TestID, testAppointmentID, testResult, notes, createdByUserID);
            }

            return test;
        }

        public static bool PassedTest(int LocalDrivingLicenseApplication, clsTestType.enTestTypes TestTypeID)
        {
            return Tests.PassedTest(LocalDrivingLicenseApplication, Convert.ToInt16(TestTypeID));
        }

        private bool AddTest()
        {
            TestID = Tests.AddTest(TestAppointmentID, Convert.ToInt32(TestResult), Notes, CreatedByUserID);
            return TestID != -1;
        }

        public bool Save()
        {
            bool isSaved = false;

            if (_mode == enMode.AddNew)
            {
                if (AddTest())
                {
                    isSaved = true;
                    _mode = enMode.Update;
                }
            }

            return isSaved;
        }
    }
}
