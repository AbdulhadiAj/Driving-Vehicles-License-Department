using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class Applications
    {

        public static int AddApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, int ApplicationStatus, DateTime LastStatusDate, double PaidFees,
            int CreatedByUserID)
        {
            int id = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                {
                    conn.Open();
                    string query = $@"Insert into Applications values({ApplicantPersonID}, '{ApplicationDate}', {ApplicationTypeID}, {ApplicationStatus}, '{LastStatusDate}', {PaidFees}, {CreatedByUserID});
                            select SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int.TryParse(result.ToString(), out id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return id;
        }

        public static DataTable GetApplication(int ApplicationID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select *
                            from Applications 
                            where ApplicationID = {ApplicationID}";
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

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, int ApplicationStatus, DateTime LastStatusDate, double PaidFees,
            int CreatedByUserID)
        {
            bool isUpdated = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Update Applications set ApplicantPersonID = {ApplicantPersonID}, ApplicationDate = '{ApplicationDate}', ApplicationTypeID = {ApplicationTypeID},  
                              ApplicationStatus = {ApplicationStatus}, LastStatusDate = '{LastStatusDate}', PaidFees = {PaidFees}, CreatedByUserID = {CreatedByUserID}
                              Where ApplicationID = {ApplicationID}";
            SqlCommand cmd = new SqlCommand(query, conn);

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

        public static bool DeleteApplication(int ApplicationID)
        {
            bool isDeleted = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Delete from Applications
                              Where ApplicationID = {ApplicationID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return isDeleted;
        }

        public static bool CancelApplication(int ApplicationID)
        {
            bool isCanceled = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Update Applications
                              Set ApplicationStatus = 2, LastStatusDate = GETDATE()
                              Where ApplicationID = {ApplicationID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    isCanceled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return isCanceled;
        }

        public static bool CompleteApplication(int ApplicationID)
        {
            bool isCompleted = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Update Applications
                              Set ApplicationStatus = 3, LastStatusDate = GETDATE()
                              Where ApplicationID = {ApplicationID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    isCompleted = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return isCompleted;
        }

        public static int GetApplicantPersonID(int ApplicationID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select ApplicantPersonID from Applications where ApplicationID = {ApplicationID}";
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
    }
}
