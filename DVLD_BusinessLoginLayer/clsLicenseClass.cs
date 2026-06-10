using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLoginLayer
{
    public class clsLicenseClass
    {

        public int LicenseClassID;
        public string ClassName;
        public string ClassDescription;
        public int MinimumAllowedAge;
        public int DefaultValidityLength;
        public double ClassFees;

        public clsLicenseClass()
        {
            LicenseClassID = -1;
            ClassName = "";
            ClassDescription = "";
            MinimumAllowedAge = 0;
            DefaultValidityLength = 0;
            ClassFees = 0;
        }

        public clsLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, int MinimumAllowedAge, int DefaultValidityLength,  double ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
        }

        public static DataTable GetLicenseClassesNames()
        {
            return LicenseClasses.GetLicenseClassesNames();
        }

        public static int GetLicenseClassID(string ClassName)
        {
            return LicenseClasses.GetLicenseClassID(ClassName);
        }

        public static string GetLicenseClassName(int LicenseClassID)
        {
            return LicenseClasses.GetLicenseClassName(LicenseClassID);
        }

        public static int GetValidityLength(int LicenseClassID)
        {
            return LicenseClasses.GetValidityLength(LicenseClassID);
        }

        public static double GetFees(int LicenseClassID)
        {
            return LicenseClasses.GetFees(LicenseClassID);
        }

    }
}
