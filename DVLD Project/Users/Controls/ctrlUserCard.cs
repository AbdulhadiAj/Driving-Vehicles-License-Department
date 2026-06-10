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

namespace DVLD_Project.Users.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        private clsUser _user;

        public int UserId
        {
            get
            {
                return _user != null ? _user.UserId : -1;
            }
            set
            {  
                _user = clsUser.GetUser(value);
                if (_user != null)
                {
                    ctrlPersonCard1.PersonID = _user.Person.PersonID;
                    lblUserID.Text = _user.UserId.ToString();
                    lblUserName.Text = _user.UserName.ToString();
                    lblIsActive.Text = (_user.IsActive) ? "yes" : "no";
                }
            }
        }
        public ctrlUserCard()
        {
            InitializeComponent();
        }
    }
}
