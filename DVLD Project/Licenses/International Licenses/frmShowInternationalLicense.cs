using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.International_Licenses
{
    public partial class frmShowInternationalLicense : Form
    {
        private int _internationaLicenseID;

        public frmShowInternationalLicense(int InternationaLicenseID)
        {
            InitializeComponent();
            _internationaLicenseID = InternationaLicenseID;
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

        private void frmShowInternationalLicense_Load(object sender, EventArgs e)
        {
            ctrlInternationalLicenseCard1.InternationalLicenseID = _internationaLicenseID;
        }
    }
}
