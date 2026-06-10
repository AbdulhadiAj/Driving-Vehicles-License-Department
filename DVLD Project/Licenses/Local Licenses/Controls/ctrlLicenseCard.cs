using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLoginLayer;

namespace DVLD_Project.Licenses.Controls
{
    public partial class ctrlLicenseCard : UserControl
    {

        private clsLicense _license;
        private clsPerson _person;

        public int LicenseID
        {
            get
            {
                return _license != null ? _license.LicenseID : -1;
            }

            set
            {
                _license = clsLicense.GetLicense(value);
                if(_license.LicenseID != -1)
                {
                    _person = clsPerson.GetPerson(clsApplication.GetApplicantPersonID(_license.ApplicationID));
                    lblClass.Text = clsLicenseClass.GetLicenseClassName(_license.LicenseClassID);
                    lblName.Text = _person.FullName;
                    lblLicenseID.Text = _license.LicenseID.ToString();
                    lblNationalNo.Text = _person.NationalNumber;
                    lblGender.Text = _person.Gender;
                    lblIssueDate.Text = _license.IssueDate.ToShortDateString();
                    lblIssueReason.Text = _license.IssueReason == clsLicense.enIssueReason.FirstTime ? "First Time" :
                        (_license.IssueReason == clsLicense.enIssueReason.Renew ? "Renew" :
                        (_license.IssueReason == clsLicense.enIssueReason.ReplacementForDamaged ? "Replacement For Damaged" : "Replacement For Lost"));
                    lblNotes.Text = _license.Notes;
                    lblIsActive.Text = _license.IsActive ? "Yes" : "No";
                    lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();
                    lblDriverID.Text = _license.DriverID.ToString();
                    lblExpirationDate.Text = _license.ExpirationDate.ToShortDateString();
                    lblIsDetained.Text = clsDetainedLicense.IsLicenseDetained(_license.LicenseID) ? "Yes" : "No";
                    pbPersonImage.ImageLocation = _person.ImagePath;
                }
            }
        }

        public ctrlLicenseCard()
        {
            InitializeComponent();
        }
    }
}
