using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class Drivers
    {

        public static int AddDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Insert into Drivers values ({PersonID}, {CreatedByUserID}, '{CreatedDate}')
                              Select Scope_Identity()";
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

        public static DataTable GetDriver(int PersonID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select * from Drivers Where PersonID = {PersonID}";
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

        public static DataTable GetDriversInfo()
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"SELECT Drivers.DriverID, People.PersonID, People.NationalNo, FullName = People.FirstName + ' ' + People.SecondName + ' ' + People.LastName,
Drivers.CreatedDate, COUNT(*) AS ActiveLicenses FROM Drivers JOIN People ON Drivers.PersonID = People.PersonID
JOIN Licenses ON Licenses.DriverID = Drivers.DriverID WHERE Licenses.IsActive = 1
GROUP BY Drivers.DriverID, People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.LastName, Drivers.CreatedDate;";
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
