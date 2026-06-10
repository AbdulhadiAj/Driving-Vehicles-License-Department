using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLoginLayer
{
    public class clsTestAppointment
    {

        enum enMode { AddNew, Update };

        public int TestAppointmentID;
        public int TestTypeID;
        public int LocalDrivingLicenseApplicationID;
        public DateTime AppointmentDate;
        public double PaidFees;
        public int CreatedByUserID;
        public bool IsLocked;
        public int RetakeTestApplicationID;
        private enMode _mode;

        public clsTestAppointment()
        {
            TestAppointmentID = -1;
            TestTypeID = -1;
            LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = -1;
            IsLocked = false;
            RetakeTestApplicationID = -1;
            _mode = enMode.AddNew;
        }

        public clsTestAppointment(int testAppointmentID, int testTypeID, int localDrivingLicenseApplicationID, DateTime appointmentDate, double paidFees, int createdByUserID, bool isLocked, int retakeTestApplicationID)
        {
            TestAppointmentID = testAppointmentID;
            TestTypeID = testTypeID;
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            IsLocked = isLocked;
            RetakeTestApplicationID = retakeTestApplicationID;
            _mode = enMode.Update;
        }

        private bool _AddTestAppointment()
        {
            TestAppointmentID = TestAppointments.AddTestAppointment(TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            return TestAppointmentID != -1;
        }

        private bool _UpdateTestAppointment()
        {
            return TestAppointments.UpdateTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
        }

        public bool Save()
        {
            bool isSaved = false;

            if (_mode == enMode.AddNew)
            {
                if (_AddTestAppointment())
                {
                    isSaved = true;
                    this._mode = enMode.Update;
                }
            }
            else
            {
                if (_UpdateTestAppointment())
                    isSaved = true;
            }

            return isSaved;
        }

        public static clsTestAppointment GetTestAppointment(int testAppointmentID)
        {
            clsTestAppointment testAppointment = new clsTestAppointment();
            DataTable dt = TestAppointments.GetTestAppointment(testAppointmentID);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int testTypeID = Convert.ToInt32(dr["TestTypeID"]);
                int localDrivingLicenseApplicationID = Convert.ToInt32(dr["LocalDrivingLicenseApplicationID"]);
                DateTime appointmentDate = Convert.ToDateTime(dr["AppointmentDate"]);
                double paidFees = Convert.ToDouble(dr["PaidFees"]);
                int createdByUserID = Convert.ToInt32(dr["CreatedByUserID"]);
                bool isLocked = Convert.ToBoolean(dr["IsLocked"]);
                int retakeTestApplicationID = Convert.ToInt32(dr["RetakeTestApplicationID"]);

                testAppointment = new clsTestAppointment(testAppointmentID, testTypeID, localDrivingLicenseApplicationID, appointmentDate, paidFees, createdByUserID, isLocked, retakeTestApplicationID);
            }
            return testAppointment;
        }

        public static int GetTrialCount(int localDrivingLicenseApplicationID, int testTypeID)
        {
            return TestAppointments.GetTrialCount(localDrivingLicenseApplicationID, testTypeID);
        }

        public static DataTable GetTestAppointmentsOfApplication(int localDrivingLicenseApplicationID, int testTypeID)
        {
            return TestAppointments.GetTestAppointmentsOfApplication(localDrivingLicenseApplicationID, testTypeID);
        }

        public static bool HasActiveTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return TestAppointments.HasActiveTestAppointment(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool HasFailedTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return TestAppointments.HasFailedTest(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool HasPassedTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return TestAppointments.HasPassedTest(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool LockTestAppointment(int TestAppointmentID)
        {
            return TestAppointments.LockTestAppointment(TestAppointmentID);
        }
    }
}
