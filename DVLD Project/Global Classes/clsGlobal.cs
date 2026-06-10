using DVLD_BusinessLoginLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Project.Global_Classes
{
    public static class clsGlobal
    {

        public static clsUser CurrentUser { get; set; }

        public static string DVLDPeopleImagesPath = @"C:\DVLD People Images\";

        public static string DVLDUserRememberMeFilePath = @"C:\DVLD User Remeber Me\rememberme.txt";
    }
}
