using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_DataAccessLayer
{
    public static class ApplicationTypes
    {

        public static DataTable GetApplicationTypesInfo()
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = @"Select * from ApplicationTypes";
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

        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, double ApplicationTypeFees)
        {
            bool isUpdated = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Update ApplicationTypes set ApplicationTypeTitle = '{ApplicationTypeTitle}', ApplicationFees = {ApplicationTypeFees}
                              where ApplicationTypeID = {ApplicationTypeID}";
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

        public static DataTable GetApplicationType(int ApplicationTypeID)
        {
            DataTable dtApplicationType = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select ApplicationTypeTitle, ApplicationFees
                            from ApplicationTypes
                            where ApplicationTypeID = {ApplicationTypeID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    dtApplicationType = new DataTable();
                    dtApplicationType.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return dtApplicationType;
        }

        public static double GetApplicationTypeFees(int ApplicationTypeID)
        {
            double fees = 0;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select ApplicationFees from ApplicationTypes where ApplicationTypeID = {ApplicationTypeID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    double.TryParse(result.ToString(), out fees);
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

            return fees;
        }
    }
}
