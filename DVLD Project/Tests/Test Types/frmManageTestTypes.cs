using DVLD_BusinessLoginLayer;
using DVLD_Project.Applications.Applications_Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Applications.Test_Types
{
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void _RefreshData()
        {
            dgvTestTypes.DataSource = clsTestType.GetTestTypesInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            dgvTestTypes.DataSource = clsTestType.GetTestTypesInfo();
            lblRecordsNumber.Text = dgvTestTypes.Rows.Count.ToString();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditTestType form = new frmEditTestType(Convert.ToInt32(dgvTestTypes.SelectedCells[0].OwningRow.Cells[0].Value));
            form.ShowDialog();
            _RefreshData();
        }

        private void dgvTestTypes_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblRecordsNumber.Text = dgvTestTypes.Rows.Count.ToString();
        }

        private void dgvTestTypes_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblRecordsNumber.Text = dgvTestTypes.Rows.Count.ToString();
        }
    }
}

