using DVLD_BusinessLoginLayer;
using System;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace DVLD_Project.People
{
    public partial class frmManagePeople : Form
    {
        private DataTable _dtPeople;

        public frmManagePeople()
        {
            InitializeComponent();
            _dtPeople = clsPerson.GetPeopleInfo();
        }

        private void _RefreshData()
        {
            _dtPeople = clsPerson.GetPeopleInfo();
            dgvPeople.DataSource = _dtPeople;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            dgvPeople.DataSource = _dtPeople;
            lblRecordsNumber.Text = dgvPeople.RowCount.ToString();
            cbFilterType.SelectedIndex = 0;
            tbFilterInput.Visible = false;
            foreach (DataColumn dc in _dtPeople.Columns)
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
                _dtPeople.DefaultView.RowFilter = "";
            }
        }

        private void tbFilterInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_dtPeople.Columns[cbFilterType.SelectedItem.ToString()].DataType == typeof(int))
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void tbFilterInput_TextChanged(object sender, EventArgs e)
        {
            _dtPeople.DefaultView.RowFilter = $"CONVERT([{cbFilterType.SelectedItem}], 'System.String') Like '{tbFilterInput.Text}%'";
        }

        private void dgvPeople_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblRecordsNumber.Text = dgvPeople.RowCount.ToString();
        }

        private void dgvPeople_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblRecordsNumber.Text = dgvPeople.RowCount.ToString();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPerson form = new frmShowPerson(Convert.ToInt32(dgvPeople.SelectedCells[0].OwningRow.Cells[0].Value));
            form.ShowDialog();
        }

        private void dgvPeople_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmShowPerson form = new frmShowPerson(Convert.ToInt32(dgvPeople.SelectedCells[0].OwningRow.Cells[0].Value));
            form.ShowDialog();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson form = new frmAddEditPerson();
            form.ShowDialog();
            _RefreshData();
        }

        private void addNewRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPerson form = new frmAddEditPerson();
            form.ShowDialog();
            _RefreshData();
        }

        private void editInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson(Convert.ToInt32(dgvPeople.SelectedCells[0].OwningRow.Cells[0].Value));
            frm.ShowDialog();
            _RefreshData();
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedID = Convert.ToInt32(dgvPeople.SelectedCells[0].OwningRow.Cells[0].Value);
            DialogResult result = MessageBox.Show($"You want to delete person of ID: {selectedID}? ", "Are you sure", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string imagePath = clsPerson.GetImagePath(selectedID);
                if(clsPerson.DeletePerson(selectedID))
                {
                    if(imagePath != "")
                        File.Delete(imagePath);
                    MessageBox.Show("Deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshData();
                }
                else
                {
                    MessageBox.Show("Cannot delete this person because there is a data in the system related to this person", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
