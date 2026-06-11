using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    public static class Countries
    {

        public static int GetCountryID(string CountryName)
        {
            int CountryID = -1;

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);
            string query = $@"Select CountryID from Countries where CountryName = @CountryName";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int.TryParse(result.ToString(), out CountryID);
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return CountryID;
        }

        public static DataTable GetCountriesInfo()
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);
            string query = @"Select * from Countries";
            SqlCommand cmd = new SqlCommand(query, conn);

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
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public static DataTable GetCountriesNames()
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]);
            string query = @"select CountryName from Countries;";
            SqlCommand cmd = new SqlCommand(query, conn);

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
            finally
            {
                conn.Close();
            }

            return dt;
        }
    }
}
