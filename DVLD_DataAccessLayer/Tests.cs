using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class Tests
    {

        public static int GetTestID(int TestAppointmentID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select TestID from Tests where TestAppointmentID = {TestAppointmentID}";
            SqlCommand cmd = new SqlCommand(query, conn);

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
            finally { conn.Close(); }

            return id;
        }

        public static DataTable GetTest(int TestID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select * from Tests where TestID = {TestID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    dt = new DataTable();
                    dt.Load(reader);
                    if (dt.Rows[0]["Notes"] == DBNull.Value)
                        dt.Rows[0]["Notes"] = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return dt;
        }

        public static int AddTest(int TestAppointmentID, int TestResult, string Notes, int createdByUserID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Insert into Tests values({TestAppointmentID}, {TestResult}, @Notes, {createdByUserID});
                            select SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(query, conn);
            if (Notes != "" && Notes != null)
                cmd.Parameters.AddWithValue("@Notes", Notes);
            else
                cmd.Parameters.AddWithValue("@Notes", DBNull.Value);

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

        public static bool PassedTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool passed = false;

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
                    passed = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return passed;
        }

    }
}
