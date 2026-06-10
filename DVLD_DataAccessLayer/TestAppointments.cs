using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class TestAppointments
    {
        public static int AddTestAppointment(int testTypeID, int localDrivingLicenseApplicationID, DateTime appointmentDate, double paidFees, int createdByUserID, bool isLocked, int retakeTestApplicationID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Insert into TestAppointments values({testTypeID}, {localDrivingLicenseApplicationID}, '{appointmentDate}', {paidFees}, {createdByUserID}, {Convert.ToInt32(isLocked)}, @RetakeTestApplicationID);
                            select SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(query, conn);
            if (retakeTestApplicationID != -1)
                cmd.Parameters.AddWithValue("@RetakeTestApplicationID", retakeTestApplicationID);
            else
                cmd.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int.TryParse(result.ToString(), out id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return id;
        }

        public static bool UpdateTestAppointment(int testAppointmentID, int testTypeID, int localDrivingLicenseApplicationID, DateTime appointmentDate, double paidFees, int createdByUserID, bool isLocked, int retakeTestApplicationID)
        {
            bool isUpdated = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Update TestAppointments set TestTypeID = {testTypeID}, LocalDrivingLicenseApplicationID = {localDrivingLicenseApplicationID}, AppointmentDate = '{appointmentDate}', PaidFees = {paidFees}, CreatedByUserID = {createdByUserID},
                                                IsLocked = {Convert.ToInt32(isLocked)}, RetakeTestApplicationID = @RetakeTestApplicationID
                              Where TestAppointmentID = {testAppointmentID}";
            SqlCommand cmd = new SqlCommand(query, conn);
            if (retakeTestApplicationID != -1)
                cmd.Parameters.AddWithValue("@RetakeTestApplicationID", retakeTestApplicationID);
            else
                cmd.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

            try
            {
                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return isUpdated;
        }

        public static DataTable GetTestAppointment(int TestAppointmentID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select * from TestAppointments where TestAppointmentID = {TestAppointmentID} ";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    dt = new DataTable();
                    dt.Load(reader);
                    if (dt.Rows[0]["RetakeTestApplicationID"] == DBNull.Value)
                        dt.Rows[0]["RetakeTestApplicationID"] = -1;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return dt;
        }

        public static int GetTrialCount(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            int count = 0;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select count(*) from TestAppointments where LocalDrivingLicenseApplicationID = {LocalDrivingLicenseApplicationID} and TestTypeID = {TestTypeID};";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int.TryParse(result.ToString(), out count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return count;
        }

        public static DataTable GetTestAppointmentsOfApplication(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select TestAppointmentID, AppointmentDate, PaidFees, IsLocked from TestAppointments where LocalDrivingLicenseApplicationID = {LocalDrivingLicenseApplicationID} 
                              and TestTypeID = {TestTypeID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    dt = new DataTable();
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return dt;
        }

        public static bool HasActiveTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool has = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select Found = 1 from TestAppointments where LocalDrivingLicenseApplicationID = {LocalDrivingLicenseApplicationID} 
                              and TestTypeID = {TestTypeID} and IsLocked = 0";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object reader = cmd.ExecuteScalar();
                if (reader != null)
                {
                    has = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return has;
        }

        public static bool HasFailedTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool has = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select Found = 1 from Tests Join TestAppointments on Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                              where LocalDrivingLicenseApplicationID = {LocalDrivingLicenseApplicationID} and TestTypeID = {TestTypeID} and TestResult = 0";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    has = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return has;
        }

        public static bool HasPassedTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool has = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select Found = 1 from Tests Join TestAppointments on Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                              where LocalDrivingLicenseApplicationID = {LocalDrivingLicenseApplicationID} and TestTypeID = {TestTypeID} and TestResult = 1";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    has = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return has;
        }

        public static bool LockTestAppointment(int TestAppointmentID)
        {
            bool isLocked = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"update TestAppointments set IsLocked = 1 where TestAppointmentID = {TestAppointmentID};";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    isLocked = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return isLocked;
        }

    }
}
