using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLoginLayer
{
    public class clsPerson
    {

        enum enMode { AddNew = 0, Update = 1 }

        //Properties
        public int PersonID;
        public string NationalNumber;
        public string FirstName;
        public string SecondName;
        public string ThirdName;
        public string LastName;
        public DateTime DateOfBirth;
        public string Gender;
        public string Address;
        public string Phone;
        public string Email;
        public string Country;
        public string ImagePath;
        enMode Mode;

        public string FullName;


        //Constructors
        public clsPerson()
        {
            PersonID = -1;
            NationalNumber = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            FullName = "";
            DateOfBirth = DateTime.Now;
            Gender = "";
            Address = "";
            Phone = "";
            Email = "";
            Country = "";
            ImagePath = "";
            Mode = enMode.AddNew;
        }

        public clsPerson(int PersonID, string NationalNumber, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            string Gender, string Address, string Phone, string Email, string Country, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNumber = NationalNumber;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.FullName = $"{FirstName} {SecondName} {LastName}";
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.Country = Country;
            this.ImagePath = ImagePath;
            Mode = enMode.Update;
        }


        //Private methods
        private bool _AddPerson()
        {
            int GenderAsInt = (Gender == "Male") ? 0 : 1;
            int CountryID = Countries.GetCountryID(Country);

            this.PersonID = People.AddPerson(NationalNumber, FirstName, SecondName, ThirdName, LastName, DateOfBirth, GenderAsInt, Address, Phone, Email, CountryID, ImagePath);

            return this.PersonID != -1;
        }

        private bool _UpdatePerson()
        {
            int GenderAsInt = (Gender == "Male") ? 0 : 1;
            int CountryID = Countries.GetCountryID(Country);
            return People.UpdatePerson(PersonID, NationalNumber, FirstName, SecondName, ThirdName, LastName, DateOfBirth, GenderAsInt, Address, Phone, Email, CountryID, ImagePath);
        }


        //Static methods
        public static DataTable GetPeopleInfo()
        {
            return People.GetPeopleInfo();
        }

        public static bool IsPersonExists(string NationalNo)
        {
            return People.IsPersonExists(NationalNo);
        }

        public static bool IsPersonExists(int PersonID)
        {
            return People.IsPersonExists(PersonID);
        }

        public static clsPerson GetPerson(int PersonID)
        {
            clsPerson person = new clsPerson();
            DataTable dtPerson = People.GetPerson(PersonID);

            if (dtPerson != null && dtPerson.Rows.Count > 0)
            {
                DataRow drPerson = dtPerson.Rows[0];
                int personID = PersonID;
                string nationalNo = drPerson["NationalNo"].ToString();
                string firstName = drPerson["FirstName"].ToString();
                string secondName = drPerson["SecondName"].ToString();
                string thirdName = drPerson["ThirdName"].ToString();
                string lastName = drPerson["LastName"].ToString();
                DateTime dateOfBirth = Convert.ToDateTime(drPerson["DateOfBirth"]);
                string gender = drPerson["Gender"].ToString();
                string address = drPerson["Address"].ToString();
                string phone = drPerson["Phone"].ToString();
                string email = drPerson["Email"].ToString();
                string country = drPerson["Nationality"].ToString();
                string imagePath = drPerson["ImagePath"].ToString();
                person = new clsPerson(personID, nationalNo, firstName, secondName, thirdName, lastName, dateOfBirth, gender, address, phone, email, country, imagePath);
            }

            return person;
        }

        public static clsPerson GetPerson(string NationalNo)
        {
            clsPerson person = new clsPerson();
            DataTable dtPerson = People.GetPerson(NationalNo);

            if (dtPerson != null && dtPerson.Rows.Count > 0)
            {
                DataRow drPerson = dtPerson.Rows[0];
                int personID = Convert.ToInt32(drPerson["PersonID"]);
                string nationalNo = NationalNo;
                string firstName = drPerson["FirstName"].ToString();
                string secondName = drPerson["SecondName"].ToString();
                string thirdName = drPerson["ThirdName"].ToString();
                string lastName = drPerson["LastName"].ToString();
                DateTime dateOfBirth = Convert.ToDateTime(drPerson["DateOfBirth"]);
                string gender = drPerson["Gender"].ToString();
                string address = drPerson["Address"].ToString();
                string phone = drPerson["Phone"].ToString();
                string email = drPerson["Email"].ToString();
                string country = drPerson["Nationality"].ToString();
                string imagePath = drPerson["ImagePath"].ToString();
                person = new clsPerson(personID, nationalNo, firstName, secondName, thirdName, lastName, dateOfBirth, gender, address, phone, email, country, imagePath);
            }

            return person;
        }

        public static bool DeletePerson(int PersonID)
        {
            return People.DeletePerson(PersonID);
        }

        public static string GetImagePath(int PersonID)
        {
            return People.GetImagePath(PersonID);
        }

        public static string GetFullName(int PersonID)
        {
            return People.GetFullName(PersonID);
        }

        public static string GetNationalNo(int PersonID)
        {
            return People.GetNationalNo(PersonID);
        }


        //Other methods
        public bool Save()
        {
            bool isSaved = false;

            if(Mode == enMode.AddNew)
            {
                if(_AddPerson())
                {
                    isSaved = true;
                    this.Mode = enMode.Update;
                }
            }
            else
            {
                if(_UpdatePerson())
                    isSaved = true;
            }

            return isSaved;
        }

    }
}
