using DVLD_BusinessLoginLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.International_Licenses.Controls
{
    public partial class ctrlInternationalLicenseCard : UserControl
    {
        private clsInternationalLicense _internationalLicense;
        private clsPerson _person;

        public int InternationalLicenseID
        {
            get
            {
                return _internationalLicense != null ? _internationalLicense.InternationalLicenseID : -1;
            }

            set
            {
                _internationalLicense = clsInternationalLicense.GetInternationalLicense(value);
                if (_internationalLicense.InternationalLicenseID != -1)
                {
                    _person = clsPerson.GetPerson(clsApplication.GetApplicantPersonID(_internationalLicense.ApplicationID));
                    lblName.Text = _person.FullName;
                    lblIntLicenseID.Text = _internationalLicense.InternationalLicenseID.ToString();
                    lblLocalLicenseID.Text = _internationalLicense.IssuedUsingLocalLicenseID.ToString();
                    lblNationalNo.Text = _person.NationalNumber;
                    lblGender.Text = _person.Gender;
                    lblIssueDate.Text = _internationalLicense.IssueDate.ToShortDateString();
                    lblApplicationID.Text = _internationalLicense.ApplicationID.ToString();
                    lblIsActive.Text = _internationalLicense.IsActive ? "Yes" : "No";
                    lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();
                    lblDriverID.Text = _internationalLicense.DriverID.ToString();
                    lblExpirationDate.Text = _internationalLicense.ExpirationDate.ToShortDateString();
                    pbPersonImage.ImageLocation = _person.ImagePath;
                }
            }
        }

        public ctrlInternationalLicenseCard()
        {
            InitializeComponent();
        }
    }
}
