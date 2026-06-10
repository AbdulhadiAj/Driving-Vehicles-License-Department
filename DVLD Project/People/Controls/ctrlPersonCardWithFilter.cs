using DVLD_BusinessLoginLayer;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public event EventHandler OnPersonSelected;

        private clsPerson _person;
        private bool _isIDSelected = true;

        public int PersonID
        {
            get
            {
                return _person != null ? _person.PersonID : -1;
            }
            set
            {
                _person = clsPerson.GetPerson(value);
                if (_person != null)
                {
                    ctrlPersonCard1.PersonID = _person.PersonID;
                    OnPersonSelected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public string NationalNo
        {
            get
            {
                return _person != null ? _person.NationalNumber : "";
            }
            set
            {
                _person = clsPerson.GetPerson(value);
                if (_person != null)
                {
                    ctrlPersonCard1.PersonID = _person.PersonID;
                    OnPersonSelected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
            _person = new clsPerson();
        }

        private void frmNewPerson_DataBack(object sender, int PersonID)
        {
            this.PersonID = PersonID;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (tbFilterInput.Text == "")
            {
                MessageBox.Show("Fill the input first", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(_isIDSelected)
            {
                int id = Convert.ToInt32(tbFilterInput.Text);
                if (clsPerson.IsPersonExists(id))
                {
                    PersonID = id;
                }
                else
                {
                    MessageBox.Show("This ID is not found", "Wrong Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string n = tbFilterInput.Text;
                if (clsPerson.IsPersonExists(n))
                {
                    NationalNo = n;
                }
                else
                {
                    MessageBox.Show("This National Number is not found", "Wrong Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void btnNewPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson form = new frmAddEditPerson();
            form.DelegatePersonID += frmNewPerson_DataBack;
            form.ShowDialog();
        }

        private void cbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterType.SelectedIndex == 0) 
                _isIDSelected = true;
            else
                _isIDSelected = false;
        }

        private void tbFilterInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_isIDSelected)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterType.SelectedIndex = 0;
        }
    }
}
