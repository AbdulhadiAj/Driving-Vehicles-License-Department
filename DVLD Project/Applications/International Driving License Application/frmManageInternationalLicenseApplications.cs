using DVLD_BusinessLoginLayer;
using DVLD_Project.Applications.Local_Driving_License_Applications;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.International_Licenses;
using DVLD_Project.Licenses.Local_Licenses;
using DVLD_Project.People;
using DVLD_Project.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.International_Driving_License_Application
{
    public partial class frmManageInternationalLicenseApplications : Form
    {
        private DataTable _dtInternationalDrivingLicenseApplications;

        public frmManageInternationalLicenseApplications()
        {
            InitializeComponent();
            _dtInternationalDrivingLicenseApplications = clsInternationalLicense.GetLicensesInfo();
        }

        private void _RefreshData()
        {
            _dtInternationalDrivingLicenseApplications = clsInternationalLicense.GetLicensesInfo();
            dgvInternationalDrivingLicenseApplications.DataSource = _dtInternationalDrivingLicenseApplications;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            dgvInternationalDrivingLicenseApplications.DataSource = _dtInternationalDrivingLicenseApplications;
            lblRecordsNumber.Text = dgvInternationalDrivingLicenseApplications.RowCount.ToString();
            cbFilterType.SelectedIndex = 0;
            tbFilterInput.Visible = false;
            foreach (DataColumn dc in _dtInternationalDrivingLicenseApplications.Columns)
            {
                if (dc.DataType == typeof(string) || dc.DataType == typeof(int))
                {
                    cbFilterType.Items.Add(dc.ColumnName);
                }
            }
        }

        private void cbFilterType_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbFilterType.SelectedIndex != 0)
            {
                tbFilterInput.Visible = true;
                tbFilterInput.Text = "";
                tbFilterInput.Focus();
            }
            else
            {
                tbFilterInput.Visible = false;
                _dtInternationalDrivingLicenseApplications.DefaultView.RowFilter = "";
            }
        }

        private void tbFilterInput_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (_dtInternationalDrivingLicenseApplications.Columns[cbFilterType.SelectedItem.ToString()].DataType == typeof(int))
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void tbFilterInput_TextChanged_1(object sender, EventArgs e)
        {
            _dtInternationalDrivingLicenseApplications.DefaultView.RowFilter = $"CONVERT([{cbFilterType.SelectedItem}], 'System.String') Like '{tbFilterInput.Text}%'";
        }

        private void dgvInternationalDrivingLicenseApplications_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblRecordsNumber.Text = dgvInternationalDrivingLicenseApplications.RowCount.ToString();
        }

        private void dgvInternationalDrivingLicenseApplications_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblRecordsNumber.Text = dgvInternationalDrivingLicenseApplications.RowCount.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddInternationalDrivingLicense form = new frmAddInternationalDrivingLicense();
            form.ShowDialog();
            _RefreshData();
        }

        private void showPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int appId = Convert.ToInt32(dgvInternationalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[1].Value);
            frmShowPerson form = new frmShowPerson(clsApplication.GetApplicantPersonID(appId));
            form.ShowDialog();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowInternationalLicense form = new frmShowInternationalLicense(Convert.ToInt32(dgvInternationalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int appId = Convert.ToInt32(dgvInternationalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[1].Value);
            frmShowPersonLicenseHistory form = new frmShowPersonLicenseHistory(clsApplication.GetApplicantPersonID(appId));
            form.ShowDialog();
        }

        private void dgvInternationalDrivingLicenseApplications_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmShowInternationalLicense form = new frmShowInternationalLicense(Convert.ToInt32(dgvInternationalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
        }
    }
}
