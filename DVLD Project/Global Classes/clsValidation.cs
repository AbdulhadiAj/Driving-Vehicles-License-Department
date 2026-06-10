using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DVLD_Project.Global_Classes
{
    public static class clsValidation
    {

        public static bool IsEmailValid(string Email)
        {
            bool isValid = false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (Regex.IsMatch(Email, pattern))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }

    }
}
