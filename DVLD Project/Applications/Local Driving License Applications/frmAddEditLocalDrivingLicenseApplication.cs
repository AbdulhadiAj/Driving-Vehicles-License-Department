using DVLD_BusinessLoginLayer;
using DVLD_Project.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.Local_Driving_License_Applications
{
    public partial class frmAddEditLocalDrivingLicenseApplication : Form
    {
        private bool _isPersonSelected = false;

        private enum enMode { Add, Update };
        private clsLocalDrivingLicenseApplication _localDrivingLicenseApplication;
        private clsApplication _application;
        private enMode _mode;

        public frmAddEditLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _localDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
            _application = new clsApplication();
            _mode = enMode.Add;
        }

        public frmAddEditLocalDrivingLicenseApplication(int appID)
        {
            InitializeComponent();
            _localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplication(appID);
            _application = clsApplication.GetApplication(_localDrivingLicenseApplication.ApplicationID);
            _mode = enMode.Update;
        }

        private void _FillFields()
        {
            ctrlPersonCardWithFilter1.PersonID = _application.ApplicantPersonID;
            lblLDLApplicationID.Text = _localDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = _application.ApplicationDate.ToShortDateString();
            cbLicenseClasses.SelectedItem = clsLicenseClass.GetLicenseClassName(_localDrivingLicenseApplication.LicenseClassID);
            lblPaidFees.Text = _application.PaidFees.ToString();
            lblCreatedBy.Text = clsUser.GetUserName(_application.CreatedByUserID);
        }

        private void _FillComboBox()
        {
            DataTable dt = clsLicenseClass.GetLicenseClassesNames();
            foreach (DataRow dr in dt.Rows)
            {
                cbLicenseClasses.Items.Add(dr[0].ToString());
            }
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(object sender, EventArgs e)
        {
            _isPersonSelected = true;
            btnNext.Enabled = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!_isPersonSelected)
            {
                MessageBox.Show("Select a person first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tabControl1.SelectedIndex = 1;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (!_isPersonSelected)
                {
                    tabControl1.SelectedIndex = 0;
                    MessageBox.Show("Select a person first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if(_mode == enMode.Update)
                {
                    tabControl1.SelectedIndex = 1;
                    MessageBox.Show("Cannot Change the selected person", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddEditLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _FillComboBox();
            if (_mode == enMode.Update)
            {
                _FillFields();
                tabControl1.SelectedIndex = 1;
            }
            else
            {
                lblApplicationDate.Text = DateTime.Today.ToShortDateString();
                cbLicenseClasses.SelectedIndex = 2;
                lblPaidFees.Text = clsApplicationType.GetApplicationTypeFees(Convert.ToInt32(clsApplication.enApplicationType.NewDrivingLicense)).ToString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int borderSize = 4;
            Color borderColor = Color.Black;

            ControlPaint.DrawBorder(
                e.Graphics,
                this.ClientRectangle,
                borderColor,
                borderSize,
                ButtonBorderStyle.Solid,
                borderColor,
                borderSize,
                ButtonBorderStyle.Solid,
                borderColor,
                borderSize,
                ButtonBorderStyle.Solid,
                borderColor,
                borderSize,
                ButtonBorderStyle.Solid
            );
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsPerson person = clsPerson.GetPerson(ctrlPersonCardWithFilter1.PersonID);
            if (clsLocalDrivingLicenseApplication.HasActiveLicenseApplicationOfClass(person.NationalNumber, cbLicenseClasses.SelectedItem.ToString()))
            {
                MessageBox.Show("This person already has an active application of this license class", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsLicense.HasLicense(person.PersonID, clsLicenseClass.GetLicenseClassID(cbLicenseClasses.SelectedItem.ToString())))
            {
                MessageBox.Show("This person already has a license of this class", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _application.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _application.ApplicationDate = Convert.ToDateTime(lblApplicationDate.Text);
            _application.ApplicationTypeID = Convert.ToInt32(clsApplication.enApplicationType.NewDrivingLicense);
            _application.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _application.LastStatusDate = Convert.ToDateTime(lblApplicationDate.Text);
            _application.PaidFees = Convert.ToDouble(lblPaidFees.Text);
            _application.CreatedByUserID = clsUser.GetUserID(lblCreatedBy.Text);

            if (_application.Save())
            {
                _localDrivingLicenseApplication.LicenseClassID = clsLicenseClass.GetLicenseClassID(cbLicenseClasses.SelectedItem.ToString());
                _localDrivingLicenseApplication.ApplicationID = _application.ApplicationID;
                if (_localDrivingLicenseApplication.Save())
                {
                    lblLDLApplicationID.Text = _localDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                    _mode = enMode.Update;
                    MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Saving Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Failed to add application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            
        }
    }
}
