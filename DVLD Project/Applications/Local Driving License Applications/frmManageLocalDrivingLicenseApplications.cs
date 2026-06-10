using DVLD_BusinessLoginLayer;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.Local_Licenses;
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

namespace DVLD_Project.Applications.Local_Driving_License_Applications
{
    public partial class frmManageLocalDrivingLicenseApplications : Form
    {
        private DataTable _dtLocalDrivingLicenseApplications;

        public frmManageLocalDrivingLicenseApplications()
        {
            InitializeComponent();
            _dtLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplicationsInfo();
        }

        private void _RefreshData()
        {
            _dtLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplicationsInfo();
            dgvLocalDrivingLicenseApplications.DataSource = _dtLocalDrivingLicenseApplications;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            dgvLocalDrivingLicenseApplications.DataSource = _dtLocalDrivingLicenseApplications;
            lblRecordsNumber.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
            cbFilterType.SelectedIndex = 0;
            tbFilterInput.Visible = false;
            foreach (DataColumn dc in _dtLocalDrivingLicenseApplications.Columns)
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
                _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            }
        }

        private void tbFilterInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_dtLocalDrivingLicenseApplications.Columns[cbFilterType.SelectedItem.ToString()].DataType == typeof(int))
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void tbFilterInput_TextChanged(object sender, EventArgs e)
        {
            _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = $"CONVERT([{cbFilterType.SelectedItem}], 'System.String') Like '{tbFilterInput.Text}%'";
        }

        private void dgvLocalDrivingLicenseApplications_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblRecordsNumber.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
        }

        private void dgvLocalDrivingLicenseApplications_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblRecordsNumber.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
        }

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingLicenseApplication form = new frmAddEditLocalDrivingLicenseApplication();
            form.ShowDialog();
            _RefreshData();
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int localDrivingLicenseApplicationID = Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value);
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplication(localDrivingLicenseApplicationID);

            if (clsApplication.CancelApplication(localDrivingLicenseApplication.ApplicationID))
            {
                MessageBox.Show("Canceled Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshData();
            }
            else
                MessageBox.Show("Failed to cancel", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplication(Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value));
            clsApplication application = clsApplication.GetApplication(localDrivingLicenseApplication.ApplicationID);

            if (clsLocalDrivingLicenseApplication.DeleteLocalDrivingLicenseApplication(localDrivingLicenseApplication.LocalDrivingLicenseApplicationID))
            {
                if (clsApplication.DeleteApplication(application.ApplicationID))
                    MessageBox.Show("Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Local Driving License Application was deleted successfully but the Application is still found in the Applications table", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _RefreshData();
            }
            else
            {
                MessageBox.Show("Failed to delete", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value.ToString());
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplication(id);
            clsApplication app = clsApplication.GetApplication(localDrivingLicenseApplication.ApplicationID);

            int passedTestCount = Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[5].Value.ToString());
            bool isNew = (app.ApplicationStatus == clsApplication.enApplicationStatus.New);
            bool isCompleted = (app.ApplicationStatus == clsApplication.enApplicationStatus.Completed);

            editApplicationToolStripMenuItem.Enabled = isNew && (passedTestCount == 0);
            deleteApplicationToolStripMenuItem.Enabled = isNew && (passedTestCount == 0);
            cancelApplicationToolStripMenuItem.Enabled = isNew;
            scheduleTestsToolStripMenuItem.Enabled = isNew && (passedTestCount < 3);
            visionTestToolStripMenuItem.Enabled = isNew && (passedTestCount == 0);
            writingTestToolStripMenuItem.Enabled = isNew && (passedTestCount == 1);
            streetTestToolStripMenuItem.Enabled = isNew && (passedTestCount == 2);
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = isNew && (passedTestCount == 3);
            showLicenseToolStripMenuItem.Enabled = isCompleted;
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditLocalDrivingLicenseApplication form = new frmAddEditLocalDrivingLicenseApplication(Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
            _RefreshData();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLocalDrivingLicenseApplication form = new frmShowLocalDrivingLicenseApplication(Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
        }

        private void dgvLocalDrivingLicenseApplications_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmShowLocalDrivingLicenseApplication form = new frmShowLocalDrivingLicenseApplication(Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestAppointments form = new frmManageTestAppointments(Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value), clsTestType.enTestTypes.Vision);
            form.ShowDialog();
            _RefreshData();
        }

        private void writingTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestAppointments form = new frmManageTestAppointments(Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value), clsTestType.enTestTypes.Written);
            form.ShowDialog();
            _RefreshData();
        }

        private void streetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestAppointments form = new frmManageTestAppointments(Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value), clsTestType.enTestTypes.Practical);
            form.ShowDialog();
            _RefreshData();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueDriverLicense form = new frmIssueDriverLicense(Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
            _RefreshData();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLAppID = Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value);
            frmShowLicense form = new frmShowLicense(clsLicense.GetLicenseID(clsLocalDrivingLicenseApplication.GetApplicationID(LDLAppID)));
            form.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLAppID = Convert.ToInt32(dgvLocalDrivingLicenseApplications.CurrentCell.OwningRow.Cells[0].Value);
            frmShowPersonLicenseHistory form = new frmShowPersonLicenseHistory(clsApplication.GetApplicantPersonID(clsLocalDrivingLicenseApplication.GetApplicationID(LDLAppID)));
            form.ShowDialog();
        }
    }
}
