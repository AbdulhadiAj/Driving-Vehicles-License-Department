using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Policy;

namespace DVLD_DataAccessLayer
{
    public static class People
    {

        public static DataTable GetPeopleInfo()
        {
            DataTable dtPeople = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = @"select People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, Gender = 
                            Case
                            	when People.Gendor = 0 then 'Male'
                            	else 'Female'
                            End
                            , People.DateOfBirth, Nationality = Countries.CountryName, People.Phone, People.Email
                            from People inner join Countries on People.NationalityCountryID = Countries.CountryID;";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dtPeople = new DataTable();
                    dtPeople.Load(reader);
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dtPeople;
        }

        public static int AddPerson(string NationalNumber, string FirstName, string SecondName, string ThirdName , string LastName, DateTime DateOfBirth,
            int Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Insert into People values(@NationalNumber, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gender, @Address,
                            @Phone, @Email, @NationalityCountryID, @ImagePath);
                            select SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NationalNumber", NationalNumber);
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@SecondName", SecondName);
            cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            cmd.Parameters.AddWithValue("@Gender", Gender);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            if (ImagePath == "")
                cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if(result != null && result != DBNull.Value)
                {
                    int.TryParse(result.ToString(), out PersonID);
                }
            }
            catch(Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return PersonID;
        }

        public static bool IsPersonExists(string NationalNo)
        {
            bool isExist = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select found = 1 from People where NationalNo = @NationalNo";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    isExist = true;
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

            return isExist;
        }

        public static bool IsPersonExists(int PersonID)
        {
            bool isExist = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Select found = 1 from People where PersonID = @PersonID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    isExist = true;
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

            return isExist;
        }

        public static DataTable GetPerson(int PersonID)
        {
            DataTable dtPerson = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, Gender = 
                            Case
                            	when People.Gendor = 0 then 'Male'
                            	else 'Female'
                            End
                            , People.DateOfBirth, Nationality = Countries.CountryName, People.Phone, People.Email, People.Address, People.ImagePath
                            from People inner join Countries on People.NationalityCountryID = Countries.CountryID
                            where PersonID = @PersonID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dtPerson = new DataTable();
                    dtPerson.Load(reader);
                    if (dtPerson.Rows[0]["ThirdName"] == DBNull.Value)
                        dtPerson.Rows[0]["ThirdName"] = "";
                    if (dtPerson.Rows[0]["Email"] == DBNull.Value)
                        dtPerson.Rows[0]["Email"] = "";
                    if (dtPerson.Rows[0]["ImagePath"] == DBNull.Value)
                        dtPerson.Rows[0]["ImagePath"] = "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return dtPerson;
        }

        public static DataTable GetPerson(string NationalNo)
        {
            DataTable dtPerson = null;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, Gender = 
                            Case
                            	when People.Gendor = 0 then 'Male'
                            	else 'Female'
                            End
                            , People.DateOfBirth, Nationality = Countries.CountryName, People.Phone, People.Email, People.Address, People.ImagePath
                            from People inner join Countries on People.NationalityCountryID = Countries.CountryID
                            where NationalNo = @NationalNo";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dtPerson = new DataTable();
                    dtPerson.Load(reader);
                    if (dtPerson.Rows[0]["ThirdName"] == DBNull.Value)
                        dtPerson.Rows[0]["ThirdName"] = "";
                    if (dtPerson.Rows[0]["Email"] == DBNull.Value)
                        dtPerson.Rows[0]["Email"] = "";
                    if (dtPerson.Rows[0]["ImagePath"] == DBNull.Value)
                        dtPerson.Rows[0]["ImagePath"] = "";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return dtPerson;
        }

        public static bool UpdatePerson(int PersonID, string NationalNumber, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            int Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            bool isUpdated = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Update People set NationalNo = @NationalNumber, FirstName = @FirstName, SecondName = @SecondName, ThirdName = @ThirdName, LastName = @LastName, DateOfBirth = @DateOfBirth,
                                                Gendor = @Gender, Address = @Address, Phone = @Phone, Email = @Email, NationalityCountryID = @NationalityCountryID, ImagePath = @ImagePath
                              Where PersonID = @PersonID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NationalNumber", NationalNumber);
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@SecondName", SecondName);
            cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            cmd.Parameters.AddWithValue("@Gender", Gender);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            if (ImagePath == "")
                cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool DeletePerson(int PersonID)
        {
            bool isDeleted = false;

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"Delete from People
                              Where PersonID = @PersonID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static string GetImagePath(int PersonID)
        {
            string imagePath = "";

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select ImagePath from People where PersonID = @PersonID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    imagePath = result.ToString();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return imagePath;
        }

        public static string GetFullName(int PersonID)
        {
            string name = "";

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select FullName = FirstName + ' ' + SecondName + ' ' + LastName from People where PersonID = @PersonID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    name = result.ToString();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return name;
        }

        public static string GetNationalNo(int PersonID)
        {
            string n = "";

            SqlConnection conn = new SqlConnection(Settings.ConnectionString);
            string query = $@"select NationalNo from People where PersonID = @PersonID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    n = result.ToString();
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogError(ex.Message);
            }
            finally { conn.Close(); }

            return n;
        }

    }
}
