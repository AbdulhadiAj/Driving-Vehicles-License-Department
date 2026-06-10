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

namespace DVLD_Project.Drivers
{
    public partial class frmManageDrivers : Form
    {

        private DataTable _dtDrivers;

        public frmManageDrivers()
        {
            InitializeComponent();
            _dtDrivers = clsDriver.GetDriversInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageDrivers_Load(object sender, EventArgs e)
        {
            dgvDrivers.DataSource = _dtDrivers;
            lblRecordsNumber.Text = dgvDrivers.RowCount.ToString();
            cbFilterType.SelectedIndex = 0;
            tbFilterInput.Visible = false;
            foreach (DataColumn dc in _dtDrivers.Columns)
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
                _dtDrivers.DefaultView.RowFilter = "";
            }
        }

        private void tbFilterInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_dtDrivers.Columns[cbFilterType.SelectedItem.ToString()].DataType == typeof(int))
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void tbFilterInput_TextChanged(object sender, EventArgs e)
        {
            _dtDrivers.DefaultView.RowFilter = $"CONVERT([{cbFilterType.SelectedItem}], 'System.String') Like '{tbFilterInput.Text}%'";
        }

        private void dgvDrivers_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblRecordsNumber.Text = dgvDrivers.RowCount.ToString();
        }

        private void dgvDrivers_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblRecordsNumber.Text = dgvDrivers.RowCount.ToString();
        }
    }
}
