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

namespace DVLD_Project.Tests
{
    public partial class frmManageTestAppointments : Form
    {

        private clsLocalDrivingLicenseApplication _localDrivingLicenseApplication;
        private DataTable _testAppointments;
        private clsTestType.enTestTypes _testType;

        public frmManageTestAppointments(int localDrivingLicenseApplicationID, clsTestType.enTestTypes testType)
        {
            InitializeComponent();
            _localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLocalDrivingLicenseApplication(localDrivingLicenseApplicationID);
            _testType = testType;
            _testAppointments = clsTestAppointment.GetTestAppointmentsOfApplication(_localDrivingLicenseApplication.LocalDrivingLicenseApplicationID, Convert.ToInt32(_testType));
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

        private void _RefreshData()
        {
            _testAppointments = clsTestAppointment.GetTestAppointmentsOfApplication(_localDrivingLicenseApplication.LocalDrivingLicenseApplicationID, Convert.ToInt32(_testType));
            dgvAppointments.DataSource = _testAppointments;
        }

        private void frmManageTestAppointments_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"Manage {_testType.ToString()} Tests";
            ctrlLocalDrivingLicenseApplicationCard1.LocalDrivingLicenseApplicationID = _localDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            dgvAppointments.DataSource = _testAppointments;
            lblRecordsNumber.Text = dgvAppointments.RowCount.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (clsTestAppointment.HasActiveTestAppointment(_localDrivingLicenseApplication.LocalDrivingLicenseApplicationID, Convert.ToInt32(_testType)))
            {
                MessageBox.Show("This person already has an active scheduled test", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(clsTestAppointment.HasPassedTest(_localDrivingLicenseApplication.LocalDrivingLicenseApplicationID, Convert.ToInt32(_testType)))
            {
                MessageBox.Show("This person already passed this test", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest form = new frmScheduleTest(_testType, _localDrivingLicenseApplication.LocalDrivingLicenseApplicationID);
            form.ShowDialog();
            _RefreshData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScheduleTest form = new frmScheduleTest(_testType, _localDrivingLicenseApplication.LocalDrivingLicenseApplicationID, Convert.ToInt32(dgvAppointments.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
            _RefreshData();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTakeTest form = new frmTakeTest(Convert.ToInt32(dgvAppointments.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
            _RefreshData();
        }

        private void dgvAppointments_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblRecordsNumber.Text = dgvAppointments.RowCount.ToString();
        }

        private void dgvAppointments_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblRecordsNumber.Text = dgvAppointments.RowCount.ToString();
        }
    }
}
