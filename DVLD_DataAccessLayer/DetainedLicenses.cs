using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class DetainedLicenses
    {
        public static DataTable GetLicensesInfo()
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select * from DetainedLicenses_View";
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return dt;
        }

        public static int AddLicense(int LicenseID, DateTime DetainDate, double FineFees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Insert into DetainedLicenses values ({LicenseID}, '{DetainDate}', {FineFees}, {CreatedByUserID}, {Convert.ToInt16(IsReleased)}, @ReleaseDate, @ReleasedByUserID, @ReleaseApplicationID)
                              Select Scope_Identity()";
            SqlCommand cmd = new SqlCommand(query, conn);
            if(IsReleased)
            {
                cmd.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
                cmd.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
                cmd.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@ReleasedByUserID", DBNull.Value);
                cmd.Parameters.AddWithValue("@ReleaseApplicationID", DBNull.Value);
            }

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

        public static bool UpdateLicense(int DetainID, int LicenseID, DateTime DetainDate, double FineFees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            bool isUpdated = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Update DetainedLicenses set LicenseID = {LicenseID}, DetainDate = '{DetainDate}', FineFees = {FineFees}, CreatedByUserID = {CreatedByUserID}, IsReleased = {Convert.ToInt16(IsReleased)}
, ReleaseDate = @ReleaseDate, ReleasedByUserID = @ReleasedByUserID, ReleaseApplicationID = @ReleaseApplicationID
                              Where DetainID = {DetainID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            if (IsReleased)
            {
                cmd.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
                cmd.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
                cmd.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);
                cmd.Parameters.AddWithValue("@ReleasedByUserID", DBNull.Value);
                cmd.Parameters.AddWithValue("@ReleaseApplicationID", DBNull.Value);
            }

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

        public static DataTable GetLicense(int LicenseID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select * from DetainedLicenses Where LicenseID = {LicenseID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    dt = new DataTable();
                    dt.Load(reader);
                    if (dt.Rows[0]["ReleaseDate"] == DBNull.Value)
                        dt.Rows[0]["ReleaseDate"] = DateTime.MinValue;
                    if (dt.Rows[0]["ReleasedByUserID"] == DBNull.Value)
                        dt.Rows[0]["ReleasedByUserID"] = -1;
                    if (dt.Rows[0]["ReleaseApplicationID"] == DBNull.Value)
                        dt.Rows[0]["ReleaseApplicationID"] = -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return dt;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool isDetained = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select Found = 1 from DetainedLicenses where LicenseID = {LicenseID} and IsReleased = 0;";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    isDetained = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return isDetained;
        }

    }
}
