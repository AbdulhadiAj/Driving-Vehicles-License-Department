using System;
using System.Windows.Forms;
using DVLD_BusinessLoginLayer;
using DVLD_Project.Properties;

namespace DVLD_Project.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _person;

        public int PersonID
        {
            get
            {
                return _person != null ? _person.PersonID : -1;
            }
            set
            {
                _person = clsPerson.GetPerson(value);
                if (_person != null)
                {
                    lblPersonID.Text = _person.PersonID.ToString();
                    lblNationalNumber.Text = _person.NationalNumber;
                    lblFirstName.Text = _person.FirstName;
                    lblLastName.Text = _person.LastName;
                    lblSecondName.Text = _person.SecondName;
                    lblThirdName.Text = _person.ThirdName;
                    lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();
                    lblGender.Text = _person.Gender;
                    lblCountry.Text = _person.Country;
                    lblPhone.Text = _person.Phone;
                    lblEmail.Text = _person.Email;
                    lblAddress.Text = _person.Address;
                    if (_person.ImagePath != "")
                    {
                        pbPersonalImage.ImageLocation = _person.ImagePath;
                    }
                    else
                    {
                        pbPersonalImage.Image = _person.Gender == "Male" ? Resources.man : Resources.woman;
                    }
                }
            }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
            _person = new clsPerson();
        }
    }
}
