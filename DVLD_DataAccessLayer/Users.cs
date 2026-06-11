using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class Users
    {
        
        public static bool IsUserFound(string username)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select Found = 1 from Users where UserName = @username";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if(result != null && result != DBNull.Value)
                {
                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally
            {
                conn.Close();
            }


            return isFound;
        }

        public static bool IsUserFound(int PersonID)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select Found = 1 from Users where PersonID = @PersonID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally
            {
                conn.Close();
            }


            return isFound;
        }

        public static string GetPassword(string username)
        {
            string password = "";

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select Password from Users where UserName = @username";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    password = result.ToString();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return password;
        }

        public static bool IsUserActive(string username)
        {
            bool isActive = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select IsActive from Users where UserName = @username";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", username);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    isActive = (bool)(result);
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally
            {
                conn.Close();
            }


            return isActive;
        }

        public static DataTable GetUsersInfo()
        {
            DataTable dtUsers = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = @"select * from Users;";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dtUsers = new DataTable();
                    dtUsers.Load(reader);
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

            return dtUsers;
        }

        public static int AddUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Insert into Users values(@PersonID, @UserName, @Password, @IsActive);
                            select SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@IsActive", IsActive ? 1 : 0);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int.TryParse(result.ToString(), out UserID);
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return UserID;
        }

        public static DataTable GetUser(int UserID)
        {
            DataTable dtUser = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select PersonID, UserName, Password, IsActive
                            from Users
                            where UserID = @UserID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dtUser = new DataTable();
                    dtUser.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return dtUser;
        }

        public static DataTable GetUser(string UserName, string Password)
        {
            DataTable dtUser = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select UserID, PersonID, IsActive
                            from Users
                            where UserName = @UserName and Password = @Password";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Password", Password);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dtUser = new DataTable();
                    dtUser.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return dtUser;
        }

        public static string GetUserName(int UserID)
        {
            string username = "";

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select UserName
                            from Users
                            where UserID = @UserID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    username = result.ToString();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return username;
        }

        public static int GetUserID(string UserName)
        {
            int id = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select UserID
                            from Users
                            where UserName = @UserName";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserName", UserName);

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

        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            bool isUpdated = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Update Users set PersonID = @PersonID, UserName = @UserName, Password = @Password, IsActive = @IsActive
                              Where UserID = @UserID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@IsActive", IsActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@UserID", UserID);

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

        public static bool DeleteUser(int UserID)
        {
            bool isDeleted = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Delete from Users
                              Where UserID = @UserID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", UserID);

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

    }
}
