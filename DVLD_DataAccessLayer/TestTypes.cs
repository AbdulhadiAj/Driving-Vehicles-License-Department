using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class TestTypes
    {


        public static DataTable GetTestTypesInfo()
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = @"Select * from TestTypes";
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

        public static bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDesc, double TestTypeFees)
        {
            bool isUpdated = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Update TestTypes set TestTypeTitle = '{TestTypeTitle}', TestTypeDescription = '{TestTypeDesc}', TestTypeFees = {TestTypeFees}
                              where TestTypeID = {TestTypeID}";
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

        public static DataTable GetTestType(int TestTypeID)
        {
            DataTable dt = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select TestTypeTitle, TestTypeDescription, TestTypeFees
                            from TestTypes
                            where TestTypeID = {TestTypeID}";
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

        public static string GetTestTypeTitle(int TestTypeID)
        {
            string title = "";

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select TestTypeTitle from TestTypes where TestTypeID = {TestTypeID}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    title = result.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally { conn.Close(); }

            return title;
        }

        public static double GetTestTypeFees(int TestTypeID)
        {
            double fees = 0;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select TestTypeFees from TestTypes where TestTypeID = {TestTypeID}";
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
            finally { conn.Close(); }

            return fees;
        }
    }
}
