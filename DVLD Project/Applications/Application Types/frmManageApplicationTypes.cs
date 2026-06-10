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

namespace DVLD_Project.Applications.Applications_Types
{
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void _RefreshData()
        {
            dgvApplicationTypes.DataSource = clsApplicationType.GetApplicationTypesInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            dgvApplicationTypes.DataSource = clsApplicationType.GetApplicationTypesInfo();
            lblRecordsNumber.Text = dgvApplicationTypes.Rows.Count.ToString();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditApplicationType form = new frmEditApplicationType(Convert.ToInt32(dgvApplicationTypes.SelectedCells[0].OwningRow.Cells[0].Value));
            form.ShowDialog();
            _RefreshData();
        }

        private void dgvApplicationTypes_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblRecordsNumber.Text = dgvApplicationTypes.Rows.Count.ToString();
        }

        private void dgvApplicationTypes_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblRecordsNumber.Text = dgvApplicationTypes.Rows.Count.ToString();
        }
    }
}
