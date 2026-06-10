using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class InternationalLicenses
    {
        public static int AddInterntaionalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Insert into InternationalLicenses values({ApplicationID}, {DriverID}, {IssuedUsingLocalLicenseID}, '{IssueDate}', '{ExpirationDate}', {Convert.ToInt16(IsActive)}, {CreatedByUserID});
                            select SCOPE_IDENTITY();";
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

        public static DataTable GetPersonLicenses(int PersonID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select InternationalLicenseID, ApplicationID, IssueDate, ExpirationDate, IsActive
from InternationalLicenses join Drivers on InternationalLicenses.DriverID = Drivers.DriverID
where Drivers.PersonID = {PersonID}";
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

        public static DataTable GetLicensesInfo()
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = @"select InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive from InternationalLicenses";
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

        public static bool HasLicense(int PersonID)
        {
            bool has = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select Found = 1 from InternationalLicenses join Drivers on InternationalLicenses.DriverID = Drivers.DriverID where PersonID = {PersonID}";
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

        public static DataTable GetInternationalLicense(int InternationalLicenseID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select * from InternationalLicenses Where InternationalLicenseID = {InternationalLicenseID}";
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

    }
}
