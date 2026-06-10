using DVLD_BusinessLoginLayer;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD_Project.Users
{
    public partial class frmManageUsers : Form
    {
        private DataTable _dtUsers;

        public frmManageUsers()
        {
            InitializeComponent();
            _dtUsers = clsUser.GetUsersInfo();

        }

        private void _RefreshData()
        {
            _dtUsers = clsUser.GetUsersInfo();
            dgvUsers.DataSource = _dtUsers;
            dgvUsers.Columns["Password"].Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            dgvUsers.DataSource = _dtUsers;
            dgvUsers.Columns["Password"].Visible = false;
            lblRecordsNumber.Text = dgvUsers.RowCount.ToString();
            cbFilterType.SelectedIndex = 0;
            tbFilterInput.Visible = false;
            foreach (DataColumn dc in _dtUsers.Columns)
            {
                if (dc.ColumnName != "Password")
                {
                    cbFilterType.Items.Add(dc.ColumnName);
                }
            }
        }

        private void cbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dtUsers.DefaultView.RowFilter = "";
            if (cbFilterType.SelectedIndex != 0)
            {
                if(cbFilterType.SelectedItem.ToString() == "IsActive")
                {
                    tbFilterInput.Visible = false;
                    cbActiveType.Visible = true;
                    cbActiveType.SelectedIndex = 0;
                    cbActiveType.Focus();
                }
                else
                {
                    tbFilterInput.Visible = true;
                    cbActiveType.Visible = false;
                    tbFilterInput.Text = "";
                    tbFilterInput.Focus();
                }
            }
            else
            {
                tbFilterInput.Visible = false;
                cbActiveType.Visible = false;
                _dtUsers.DefaultView.RowFilter = "";
            }
        }

        private void tbFilterInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_dtUsers.Columns[cbFilterType.SelectedItem.ToString()].DataType == typeof(int))
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void tbFilterInput_TextChanged(object sender, EventArgs e)
        {
            _dtUsers.DefaultView.RowFilter = $"CONVERT([{cbFilterType.SelectedItem}], 'System.String') Like '{tbFilterInput.Text}%'";
        }

        private void cbActiveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbActiveType.SelectedIndex != 0)
            {
                _dtUsers.DefaultView.RowFilter = $@"IsActive = {(cbActiveType.SelectedIndex == 1 ? '1' : '0')}";
            }
            else
            {
                _dtUsers.DefaultView.RowFilter = "";
            }
        }

        private void dgvUsers_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblRecordsNumber.Text = dgvUsers.RowCount.ToString();
        }

        private void dgvUsers_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblRecordsNumber.Text = dgvUsers.RowCount.ToString();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowUser form = new frmShowUser(Convert.ToInt32(dgvUsers.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
        }

        private void dgvUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmShowUser form = new frmShowUser(Convert.ToInt32(dgvUsers.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddEditUser form = new frmAddEditUser();
            form.ShowDialog();
            _RefreshData();
        }

        private void addNewRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser form = new frmAddEditUser();
            form.ShowDialog();
            _RefreshData();
        }

        private void editInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser form = new frmAddEditUser(Convert.ToInt32(dgvUsers.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
            _RefreshData();
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvUsers.CurrentCell.OwningRow.Cells[0].Value);
            if(MessageBox.Show($"You want to delete User of ID: {id}?", "Are you sure", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (clsUser.DeleteUser(id))
                {
                    MessageBox.Show("Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshData();
                }
                else
                    MessageBox.Show("Cannot delete a user has a related data", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword form = new frmChangePassword(Convert.ToInt32(dgvUsers.CurrentCell.OwningRow.Cells[0].Value));
            form.ShowDialog();
            _RefreshData();
        }
    }
}
