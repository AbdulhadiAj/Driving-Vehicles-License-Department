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

namespace DVLD_Project.Tests
{
    public partial class frmTakeTest : Form
    {

        private int _testAppointmentID;
        private clsTest _test;

        public frmTakeTest(int TestAppointmentID)
        {
            InitializeComponent();
            _testAppointmentID = TestAppointmentID;
            _test = new clsTest();
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

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            lblUserMessage.Visible = false;
            ctrlScheduledTestCard1.TestAppointmentID = _testAppointmentID;

            int testID = clsTest.GetTestID(_testAppointmentID);
            if (testID != -1)
            {
                _test = clsTest.GetTest(testID);
                if (_test.TestResult == clsTest.enTestResult.Fail)
                    rbFail.Checked = true;
                else
                    rbPass.Checked = true;
                tbNotes.Text = _test.Notes;

                lblUserMessage.Visible = true;
                rbFail.Enabled = false;
                rbPass.Enabled = false;
                tbNotes.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure? ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _test.TestAppointmentID = ctrlScheduledTestCard1.TestAppointmentID;
                _test.TestResult = rbFail.Checked ? clsTest.enTestResult.Fail : clsTest.enTestResult.Pass;
                _test.Notes = tbNotes.Text;
                _test.CreatedByUserID = clsGlobal.CurrentUser.UserId;

                if (_test.Save())
                {
                    clsTestAppointment.LockTestAppointment(_test.TestAppointmentID);
                    MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Saving Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
                
            }
        }
    }
}
