using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

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
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
                {
                    conn.Open();
                    string query = $@"Insert into Applications values(@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID);
                            select SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                        cmd.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                        cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        cmd.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                        cmd.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                        cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
                        cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            int.TryParse(result.ToString(), out id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }

            return id;
        }

        public static DataTable GetApplication(int ApplicationID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);
            string query = $@"select *
                            from Applications 
                            where ApplicationID = @ApplicationID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dt = new DataTable();
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return dt;
        }

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, int ApplicationStatus, DateTime LastStatusDate, double PaidFees,
            int CreatedByUserID)
        {
            bool isUpdated = false;

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);
            string query = $@"Update Applications set ApplicantPersonID = @ApplicantPersonID, ApplicationDate = @ApplicationDate, ApplicationTypeID = @ApplicationTypeID,  
                              ApplicationStatus = @ApplicationStatus, LastStatusDate = @LastStatusDate, PaidFees = @PaidFees, CreatedByUserID = @CreatedByUserID
                              Where ApplicationID = @ApplicationID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            cmd.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            cmd.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            cmd.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
            cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return isUpdated;
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            bool isDeleted = false;

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);
            string query = $@"Delete from Applications
                              Where ApplicationID = @ApplicationID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return isDeleted;
        }

        public static bool CancelApplication(int ApplicationID)
        {
            bool isCanceled = false;

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);
            string query = $@"Update Applications
                              Set ApplicationStatus = 2, LastStatusDate = GETDATE()
                              Where ApplicationID = @ApplicationID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return isCanceled;
        }

        public static bool CompleteApplication(int ApplicationID)
        {
            bool isCompleted = false;

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);
            string query = $@"Update Applications
                              Set ApplicationStatus = 3, LastStatusDate = GETDATE()
                              Where ApplicationID = @ApplicationID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return isCompleted;
        }

        public static int GetApplicantPersonID(int ApplicationID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);
            string query = $@"select ApplicantPersonID from Applications where ApplicationID = @ApplicationID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int.TryParse(result.ToString(), out id);
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return id;
        }
    }
}
