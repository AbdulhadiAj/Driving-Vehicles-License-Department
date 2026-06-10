using DVLD_BusinessLoginLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses.Local_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.Detain_License
{
    public partial class frmDetainLicense : Form
    {
        private clsLicense _license;
        private clsDetainedLicense _detainedLicense;

        public frmDetainLicense()
        {
            InitializeComponent();
            _license = new clsLicense();
            _detainedLicense = new clsDetainedLicense();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            btnShowLicenseInfo.Enabled = false;
            btnShowLicensesHistory.Enabled = false;
            btnDetain.Enabled = false;

            lblDetainDate.Text = DateTime.Today.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnShowLicensesHistory_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory form = new frmShowPersonLicenseHistory(clsApplication.GetApplicantPersonID(clsLicense.GetLicense(ctrlLicenseCardWithFilter1.LicenseID).ApplicationID));
            form.ShowDialog();
        }

        private void ctrlLicenseCardWithFilter1_OnLicenseSelected(object sender, EventArgs e)
        {
            _license = clsLicense.GetLicense(ctrlLicenseCardWithFilter1.LicenseID);

            btnShowLicensesHistory.Enabled = true;
            lblLicenseID.Text = _license.LicenseID.ToString();
            btnDetain.Enabled = true;
        }

        private void btnShowLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowLicense form = new frmShowLicense(_license.LicenseID);
            form.ShowDialog();
        }

        private void tbFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFineFees.Text))
            {
                errorProvider1.SetError(tbFineFees, "This field cannot be empty");
            }
            else
            {
                errorProvider1.SetError(tbFineFees, "");
            }
        }

        private void tbFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private bool HasErrors()
        {
            this.ValidateChildren();
            foreach (Control ctrl in groupBox1.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider1.GetError(ctrl)))
                    return true;
            }
            return false;
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(HasErrors())
            {
                MessageBox.Show("Some fields has errors", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }    

            if (!clsLicense.IsLicenseActive(_license.LicenseID))
            {
                MessageBox.Show("This license is not active", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsDetainedLicense.IsLicenseDetained(_license.LicenseID))
            {
                MessageBox.Show("This license is detained, choose another license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _detainedLicense.LicenseID = _license.LicenseID;
            _detainedLicense.DetainDate = DateTime.Now;
            _detainedLicense.FineFees = Convert.ToDouble(tbFineFees.Text);
            _detainedLicense.CreatedByUserID = clsGlobal.CurrentUser.UserId;
            _detainedLicense.IsReleased = false;

            if (_detainedLicense.Save())
            {
                MessageBox.Show($"Detained Successfully with Detain ID = {_detainedLicense.DetainID}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnShowLicenseInfo.Enabled = true;
                lblDetainID.Text = _detainedLicense.DetainID.ToString();
            }
            else
            {
                MessageBox.Show("Failed to issue", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
