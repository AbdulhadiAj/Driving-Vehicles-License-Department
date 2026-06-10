using DVLD_BusinessLoginLayer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLD_Project.People
{
    public partial class frmAddEditPerson : Form
    {

        public delegate void DataBackPersonID(object sender, int PersonID);
        public DataBackPersonID DelegatePersonID;

        private enum enPictureTag { man, woman, other };
        private enum enMode { Add, Update};

        private DataTable _countries;
        private clsPerson _person;
        private enMode _mode;

        public frmAddEditPerson()
        {
            InitializeComponent();
            _countries = clsCountry.GetCountriesNames();
            _person = new clsPerson();
            _mode = enMode.Add;
        }

        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();
            _countries = clsCountry.GetCountriesNames();
            _person = clsPerson.GetPerson(PersonID);
            _mode = enMode.Update;
        }

        private void _FillFields()
        {
            lblPersonID.Text = _person.PersonID.ToString();
            tbNationalNumber.Text = _person.NationalNumber;
            tbFirstName.Text = _person.FirstName;
            tbSecondName.Text = _person.SecondName;
            tbThirdName.Text = _person.ThirdName;
            tbLastName.Text = _person.LastName;
            dtpDateOfBirth.Value = _person.DateOfBirth;
            if (_person.Gender == "Male")
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;
            cbCountry.SelectedItem = _person.Country;
            tbPhone.Text = _person.Phone;
            tbEmail.Text = _person.Email;
            tbAddress.Text = _person.Address;
            if(_person.ImagePath != "")
            {
                pbPersonalImage.ImageLocation = _person.ImagePath;
                pbPersonalImage.Tag = enPictureTag.other;
                btnRemoveImage.Visible = true;
                btnAddImage.Visible = false;
            }
        }

        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            pbPersonalImage.Image = Resources.man;
            pbPersonalImage.Tag = enPictureTag.man.ToString();
            btnRemoveImage.Visible = false;
            _FillCountriesInComboBox();
            cbCountry.SelectedItem = "Syria";
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
            pbPersonalImage.ImageLocation = "";

            if(_mode == enMode.Update)
                _FillFields();
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

        private void _FillCountriesInComboBox()
        {
            if (_countries == null)
                return;
            foreach (DataRow dr in _countries.Rows)
            {
                cbCountry.Items.Add(dr[0].ToString());
            }
        }

        private void _ChangeDefaultImage()
        {
            if (pbPersonalImage.Tag.ToString() == enPictureTag.man.ToString())
            {
                pbPersonalImage.Image = Resources.woman;
                pbPersonalImage.Tag = enPictureTag.woman.ToString();
            }
            else if (pbPersonalImage.Tag.ToString() == enPictureTag.woman.ToString())
            {
                pbPersonalImage.Image = Resources.man;
                pbPersonalImage.Tag = enPictureTag.man.ToString();
            }
        }

        public void _HandlePersonImage()
        {
            if (_person.ImagePath == "" && pbPersonalImage.ImageLocation == "")
            {
                _person.ImagePath = "";
            }
            else if (_person.ImagePath == "" && pbPersonalImage.ImageLocation != "")
            {
                _person.ImagePath = clsGlobal.DVLDPeopleImagesPath + clsUtil.RenameFileUsingGUID(pbPersonalImage.ImageLocation);
                File.Copy(pbPersonalImage.ImageLocation, _person.ImagePath);
            }
            else if (_person.ImagePath != "" && pbPersonalImage.ImageLocation == "")
            {
                File.Delete(_person.ImagePath);
                _person.ImagePath = "";
            }
            else
            {
                if(pbPersonalImage.ImageLocation != _person.ImagePath)
                {
                    File.Delete(_person.ImagePath);
                    _person.ImagePath = clsGlobal.DVLDPeopleImagesPath + clsUtil.RenameFileUsingGUID(pbPersonalImage.ImageLocation);
                    File.Copy(pbPersonalImage.ImageLocation, _person.ImagePath);
                }
            }
        }

        private bool _HasErrors()
        {
            this.ValidateChildren();
            foreach (Control ctrl in this.Controls)
            {
                if (!string.IsNullOrEmpty(errorProvider1.GetError(ctrl)))
                    return true;
            }
            return false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked)
            {
                _ChangeDefaultImage();
            }
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked)
            {
                _ChangeDefaultImage();
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (openFileDialog1.FileName.EndsWith(".jpg") || openFileDialog1.FileName.EndsWith(".png"))
            {
                pbPersonalImage.ImageLocation = openFileDialog1.FileName;
                pbPersonalImage.Tag = enPictureTag.other.ToString();
                btnRemoveImage.Visible = true;
                btnAddImage.Visible = false;
            }
            else
            {
                MessageBox.Show("The file must be an image (end with .jpg or .png)", "Invalid file type", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            pbPersonalImage.Image = rbMale.Checked ? Resources.man : Resources.woman;
            pbPersonalImage.Tag = rbMale.Checked ? enPictureTag.man.ToString() : enPictureTag.woman.ToString();
            pbPersonalImage.ImageLocation = "";
            btnRemoveImage.Visible = false;
            btnAddImage.Visible = true;
        }

        private void OnValidating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                errorProvider1.SetError((TextBox)sender, "This field is required");
            }
            else
            {
                errorProvider1.SetError((TextBox)sender, "");
            }
        }

        private void tbEmail_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbEmail.Text))
            {
                if (!clsValidation.IsEmailValid(tbEmail.Text))
                {
                    errorProvider1.SetError(tbEmail, "Wrong email format");
                }
                else
                {
                    errorProvider1.SetError(tbEmail, "");
                }
            }
            else
            {
                errorProvider1.SetError(tbEmail, "");
            }
        }

        private void tbNationalNumber_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbNationalNumber.Text))
            {
                if (clsPerson.IsPersonExists(tbNationalNumber.Text) && tbNationalNumber.Text != _person.NationalNumber)
                {
                    errorProvider1.SetError(tbNationalNumber, "This person is already exists");
                }
                else
                {
                    errorProvider1.SetError(tbNationalNumber, "");
                }
            }
        }

        private void onTextChange(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                errorProvider1.SetError(textBox, "This field is required");
            }
            else
            {
                errorProvider1.SetError(textBox, "");
            }
        }

        private void btnSavePerson_Click(object sender, EventArgs e)
        {
            if (_HasErrors())
            {
                MessageBox.Show("Some fields is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _person.FirstName = tbFirstName.Text;
            _person.SecondName = tbSecondName.Text;
            _person.ThirdName = tbThirdName.Text;
            _person.LastName = tbLastName.Text;
            _person.NationalNumber = tbNationalNumber.Text;
            _person.DateOfBirth = dtpDateOfBirth.Value;
            if (rbMale.Checked)
                _person.Gender = rbMale.Text;
            else
                _person.Gender = rbFemale.Text;
            _person.Phone = tbPhone.Text;
            _person.Email = tbEmail.Text;
            _person.Country = cbCountry.SelectedItem.ToString();
            _person.Address = tbAddress.Text;
            _HandlePersonImage();

            if (_person.Save())
            {
                lblPersonID.Text = _person.PersonID.ToString();
                _mode = enMode.Update;
                MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DelegatePersonID?.Invoke(this, _person.PersonID);
            }
            else
            {
                MessageBox.Show("Saving Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
