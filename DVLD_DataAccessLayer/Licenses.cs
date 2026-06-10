using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccessLayer
{
    public static class Licenses
    {

        public static int AddLicense(int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, double PaidFees, bool IsActive, int IssueReason, int CreatedByUserID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Insert into Licenses values ({ApplicationID}, {DriverID}, {LicenseClassID}, '{IssueDate}', '{ExpirationDate}', @Notes, {PaidFees}, {Convert.ToInt16(IsActive)}, {IssueReason}, {CreatedByUserID})
                              Select Scope_Identity()";
            SqlCommand cmd = new SqlCommand(query, conn);
            if(!string.IsNullOrWhiteSpace(Notes))
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
            finally { conn.Close(); }

            return id;
        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, double PaidFees, bool IsActive, int IssueReason, int CreatedByUserID)
        {
            bool isUpdated = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Update Licenses set ApplicationID = {ApplicationID}, DriverID = {DriverID}, LicenseClass = {LicenseClassID}, IssueDate = '{IssueDate}', ExpirationDate = '{ExpirationDate}', Notes = '{Notes}'
, PaidFees = {PaidFees}, IsActive = {Convert.ToInt16(IsActive)}, IssueReason = {IssueReason}, CreatedByUserID = {CreatedByUserID}
                              Where LicenseID = {LicenseID}";
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

        public static DataTable GetLicense(int LicenseID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select * from Licenses Where LicenseID = {LicenseID}";
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

        public static int GetLicenseID(int ApplicationID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select LicenseID from Licenses Where ApplicationID = {ApplicationID}";
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

        public static int GetLicenseClassID(int LicenseID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select LicenseClass from Licenses Where LicenseID = {LicenseID}";
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

        public static DataTable GetPersonLicenses(int PersonID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select LicenseID, ApplicationID, LicenseClasses.ClassName, IssueDate, ExpirationDate, IsActive
from Licenses join LicenseClasses on Licenses.LicenseClass = LicenseClasses.LicenseClassID
join Drivers on Licenses.DriverID = Drivers.DriverID
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

        public static bool HasLicense(int PersonID, int LicenseClassID)
        {
            bool has = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select Found = 1 from Licenses join Drivers on Licenses.DriverID = Drivers.DriverID where PersonID = {PersonID} and LicenseClass = {LicenseClassID}";
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

        public static bool IsLicenseActiveAndValid(int LicenseID)
        {
            bool isActive = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select Found = 1 from Licenses where LicenseID = {LicenseID} and IsActive = 1 and ExpirationDate > GETDATE()";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object reader = cmd.ExecuteScalar();
                if (reader != null)
                {
                    isActive = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return isActive;
        }

        public static bool IsLicenseActive(int LicenseID)
        {
            bool isRenewed = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select Found = 1 from Licenses where LicenseID = {LicenseID} and IsActive = 1";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object reader = cmd.ExecuteScalar();
                if (reader != null)
                {
                    isRenewed = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return isRenewed;
        }

        public static bool IsLicenseExpired(int LicenseID)
        {
            bool isExpired = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select Found = 1 from Licenses where LicenseID = {LicenseID} and ExpirationDate < GETDATE()";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object reader = cmd.ExecuteScalar();
                if (reader != null)
                {
                    isExpired = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return isExpired;
        }

        public static bool IsLicenseWillExpiresSoon(int LicenseID)
        {
            bool isWillExpiresSoon = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select Found = 1 from Licenses where LicenseID = {LicenseID} and GETDATE() BETWEEN DATEADD(MONTH, -1, ExpirationDate) AND ExpirationDate";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object reader = cmd.ExecuteScalar();
                if (reader != null)
                {
                    isWillExpiresSoon = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return isWillExpiresSoon;
        }

        public static int GetPersonID(int LicenseID)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select Drivers.PersonID from Licenses join Drivers on Licenses.DriverID = Drivers.DriverID Where LicenseID = {LicenseID}";
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
