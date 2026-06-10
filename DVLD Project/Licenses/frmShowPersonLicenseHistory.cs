using DVLD_BusinessLoginLayer;
using DVLD_Project.Licenses.International_Licenses;
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

namespace DVLD_Project.Licenses
{
    public partial class frmShowPersonLicenseHistory : Form
    {

        private int _personID;

        public frmShowPersonLicenseHistory(int PersonID)
        {
            InitializeComponent();
            _personID = PersonID;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            dgvLocalLicenses.DataSource = clsLicense.GetPersonLicenses(_personID);
            lblLocalLicensesRecordNumber.Text = dgvLocalLicenses.RowCount.ToString();
            dgvInternationalLicenses.DataSource = clsInternationalLicense.GetPersonLicenses(_personID);
            lblInternationalLicensesRecordNumber.Text = dgvInternationalLicenses.RowCount.ToString();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                frmShowLicense form = new frmShowLicense(Convert.ToInt32(dgvLocalLicenses.CurrentCell.OwningRow.Cells[0].Value));
                form.ShowDialog();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                frmShowInternationalLicense form = new frmShowInternationalLicense(Convert.ToInt32(dgvInternationalLicenses.CurrentCell.OwningRow.Cells[0].Value));
                form.ShowDialog();
            }
        }

        private void dgvLocalLicenses_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmShowLicense form = new frmShowLicense(Convert.ToInt32(dgvLocalLicenses.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
        }

        private void dgvInternationalLicenses_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmShowInternationalLicense form = new frmShowInternationalLicense(Convert.ToInt32(dgvInternationalLicenses.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
        }
    }
}
