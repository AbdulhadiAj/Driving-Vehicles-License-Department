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

namespace DVLD_Project.Applications.Controls
{
    public partial class ctrlApplicationCard : UserControl
    {

        private clsApplication _application;

        public int ApplicationID
        {
            get
            {
                return _application != null ? _application.ApplicationID : -1;
            }
            set
            {
                _application = clsApplication.GetApplication(value);
                if(_application != null)
                {
                    lblApplicationID.Text = _application.ApplicationID.ToString();
                    lblApplicantPersonID.Text = _application.ApplicantPersonID.ToString();
                    lblApplicationDate.Text = _application.ApplicationDate.ToShortDateString();
                    lblApplicationType.Text = clsApplicationType.GetApplicationType(_application.ApplicationTypeID).ApplicationTypeTitle;
                    lblApplicationStatus.Text = _application.ApplicationStatus.ToString();
                    lblLastStatusDate.Text = _application.LastStatusDate.ToShortDateString();
                    lblPaidFees.Text = _application.PaidFees.ToString();
                    lblCreatedBy.Text = clsUser.GetUserName(_application.CreatedByUserID);
                }
            }
        }

        public ctrlApplicationCard()
        {
            InitializeComponent();
        }
    }
}
