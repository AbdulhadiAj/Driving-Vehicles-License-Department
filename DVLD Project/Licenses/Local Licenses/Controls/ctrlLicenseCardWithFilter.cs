using DVLD_BusinessLoginLayer;
using DVLD_Project.People;
using DVLD_Project.People.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.Local_Licenses.Controls
{
    public partial class ctrlLicenseCardWithFilter : UserControl
    {
        public event EventHandler OnLicenseSelected;

        private clsLicense _license;

        public int LicenseID
        {
            get
            {
                return _license != null ? _license.LicenseID : -1;
            }
            set
            {
                _license = clsLicense.GetLicense(value);
                if (_license != null)
                {
                    ctrlLicenseCard1.LicenseID = _license.LicenseID;
                    OnLicenseSelected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public ctrlLicenseCardWithFilter()
        {
            InitializeComponent();
            _license = new clsLicense();
        }

        private void tbFilterInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            if (tbFilterInput.Text == "")
            {
                MessageBox.Show("Fill the input first", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int id = Convert.ToInt32(tbFilterInput.Text);
            if (clsLicense.GetLicense(id).LicenseID != -1)
            {
                LicenseID = id;
            }
            else
            {
                MessageBox.Show("This ID is not found", "Wrong Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
