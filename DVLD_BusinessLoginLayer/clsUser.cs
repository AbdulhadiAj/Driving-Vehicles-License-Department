using DVLD_DataAccessLayer;
using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace DVLD_BusinessLoginLayer
{
    public class clsUser
    {
        enum enMode { AddNew = 0, Update = 1 }

        public int UserId;
        public string UserName;
        public string Password;
        public bool IsActive;
        public clsPerson Person;
        private enMode _mode;


        public clsUser()
        {
            UserId = -1;
            UserName = "";
            Password = "";
            IsActive = false;
            Person = new clsPerson();
            _mode = enMode.AddNew;
        }

        public clsUser(int UserId, string UserName, string Password, bool IsActive, clsPerson Person)
        {
            this.UserId = UserId;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;
            this.Person= Person;
            _mode = enMode.Update;
        }

        private bool _AddUser()
        {
            UserId = Users.AddUser(Person.PersonID, UserName, Password, IsActive);
            return (UserId != -1);
        }

        private bool _UpdateUser()
        {
            return Users.UpdateUser(UserId, Person.PersonID, UserName, Password, IsActive);
        }

        public static bool IsUserFound(string username)
        {
            return Users.IsUserFound(username);
        }

        public static bool IsUserFound(int PersonID)
        {
            return Users.IsUserFound(PersonID);
        }

        public static bool IsPasswordMatch(string username, string password)
        {
            string realPassword = Users.GetPassword(username);
            return realPassword == password;
        }

        public static bool IsUserActive(string username)
        {
            return Users.IsUserActive(username);
        }

        public static DataTable GetUsersInfo()
        {
            return Users.GetUsersInfo();
        }

        public static clsUser GetUser(int UserID)
        {
            clsUser User = new clsUser();
            DataTable dtUser = Users.GetUser(UserID);

            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                DataRow drUser = dtUser.Rows[0];
                int userID = UserID;
                string userName = drUser["UserName"].ToString();
                string password = drUser["Password"].ToString();
                bool isActive = Convert.ToBoolean(drUser["IsActive"]);
                clsPerson Person = clsPerson.GetPerson(Convert.ToInt32(drUser["PersonID"]));
                User = new clsUser(userID, userName, password, isActive, Person);
            }

            return User;
        }

        public static string GetUserName(int UserID)
        {
            return Users.GetUserName(UserID);
        }

        public static int GetUserID(string UserName)
        {
            return Users.GetUserID(UserName);
        }

        public static clsUser GetUser(string UserName, string Password)
        {
            clsUser User = new clsUser();
            DataTable dtUser = Users.GetUser(UserName, Password);

            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                DataRow drUser = dtUser.Rows[0];
                int userID = Convert.ToInt32(drUser["UserID"]);
                string userName = UserName;
                string password = Password;
                bool isActive = Convert.ToBoolean(drUser["IsActive"]);
                clsPerson Person = clsPerson.GetPerson(Convert.ToInt32(drUser["PersonID"]));
                User = new clsUser(userID, userName, password, isActive, Person);
            }

            return User;
        }

        public static bool DeleteUser(int UserID)
        {
            return Users.DeleteUser(UserID);
        }

        public bool Save()
        {
            bool isSaved = false;

            if(_mode == enMode.AddNew)
            {
                if(_AddUser())
                {
                    isSaved = true;
                    _mode = enMode.Update;
                }
            }
            else
            {
                if(_UpdateUser())
                {
                    isSaved = true;
                }
            }

            return isSaved;
        }

    }
}
