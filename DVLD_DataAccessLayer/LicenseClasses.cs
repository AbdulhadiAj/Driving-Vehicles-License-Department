using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class LicenseClasses
    {

        public static DataTable GetLicenseClassesNames()
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = @"select ClassName
                            from LicenseClasses";
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
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public static int GetLicenseClassID(string LicenseClassName)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select LicenseClassID from LicenseClasses where ClassName = '{LicenseClassName}'";
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
            finally
            {
                conn.Close();
            }

            return id;
        }

        public static string GetLicenseClassName(int LicenseClassID)
        {
            string name = "";

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select ClassName from LicenseClasses where LicenseClassID = {LicenseClassID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    name = result.ToString();
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

            return name;
        }

        public static int GetValidityLength(int LicenseClassID)
        {
            int length = 0;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select DefaultValidityLength from LicenseClasses where LicenseClassID = {LicenseClassID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int.TryParse(result.ToString(), out length);
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

            return length;
        }

        public static double GetFees(int LicenseClassID)
        {
            double fees = 0;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select ClassFees from LicenseClasses where LicenseClassID = {LicenseClassID}";
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
