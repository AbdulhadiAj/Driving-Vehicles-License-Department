using DVLD_BusinessLoginLayer;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.Detain_License;
using DVLD_Project.Licenses.Local_Licenses;
using DVLD_Project.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.Release_Detained_License
{
    public partial class frmManageDetainedLisences : Form
    {
        private DataTable _dtDetainedLicenses;

        public frmManageDetainedLisences()
        {
            InitializeComponent();
            _dtDetainedLicenses = clsDetainedLicense.GetDetainedLicensesInfo();
        }

        private void _RefreshData()
        {
            _dtDetainedLicenses = clsDetainedLicense.GetDetainedLicensesInfo();
            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;
            tbFilterInput.Text = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageDetainedLisences_Load(object sender, EventArgs e)
        {
            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;
            lblRecordsNumber.Text = dgvDetainedLicenses.RowCount.ToString();
            cbFilterType.SelectedIndex = 0;
            tbFilterInput.Visible = false;
            foreach (DataColumn dc in _dtDetainedLicenses.Columns)
            {
                if (dc.DataType == typeof(string) || dc.DataType == typeof(int))
                {
                    cbFilterType.Items.Add(dc.ColumnName);
                }
            }
        }

        private void cbFilterType_SelectedIndexChanged(object sender, EventArgs e)
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
                _dtDetainedLicenses.DefaultView.RowFilter = "";
            }
        }

        private void tbFilterInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_dtDetainedLicenses.Columns[cbFilterType.SelectedItem.ToString()].DataType == typeof(int))
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void tbFilterInput_TextChanged(object sender, EventArgs e)
        {
            _dtDetainedLicenses.DefaultView.RowFilter = $"CONVERT([{cbFilterType.SelectedItem}], 'System.String') Like '{tbFilterInput.Text}%'";
        }

        private void dgvDetainedLicenses_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblRecordsNumber.Text = dgvDetainedLicenses.RowCount.ToString();
        }

        private void dgvDetainedLicenses_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblRecordsNumber.Text = dgvDetainedLicenses.RowCount.ToString();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = Convert.ToInt32(dgvDetainedLicenses.CurrentCell.OwningRow.Cells[1].Value);
            frmShowPerson form = new frmShowPerson(clsLicense.GetPersonID(licenseID));
            form.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = Convert.ToInt32(dgvDetainedLicenses.CurrentCell.OwningRow.Cells[1].Value);
            frmShowLicense form = new frmShowLicense(licenseID);
            form.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = Convert.ToInt32(dgvDetainedLicenses.CurrentCell.OwningRow.Cells[1].Value);
            frmShowPersonLicenseHistory form = new frmShowPersonLicenseHistory(clsLicense.GetPersonID(licenseID));
            form.ShowDialog();
        }

        private void dgvDetainedLicenses_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int licenseID = Convert.ToInt32(dgvDetainedLicenses.CurrentCell.OwningRow.Cells[1].Value);
            frmShowPerson form = new frmShowPerson(clsLicense.GetPersonID(licenseID));
            form.ShowDialog();
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
            _RefreshData();
        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
            _RefreshData();
        }
    }
}
