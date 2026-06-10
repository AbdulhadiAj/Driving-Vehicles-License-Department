using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLoginLayer
{
    public class clsApplicationType
    {
        public int ApplicationTypeID { get; }
        public string ApplicationTypeTitle;
        public double ApplicationTypeFees;

        public clsApplicationType()
        {
            ApplicationTypeID = -1;
            ApplicationTypeTitle = "";
            ApplicationTypeFees = 0;
        }

        public clsApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, double ApplicationTypeFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.ApplicationTypeFees = ApplicationTypeFees;
        }

        public static DataTable GetApplicationTypesInfo()
        {
            return ApplicationTypes.GetApplicationTypesInfo();
        }

        public static clsApplicationType GetApplicationType(int ApplicationTypeID)
        {
            clsApplicationType ApplicationType = new clsApplicationType();
            DataTable dtAppType = ApplicationTypes.GetApplicationType(ApplicationTypeID);

            if (dtAppType != null && dtAppType.Rows.Count > 0)
            {
                DataRow drAppType = dtAppType.Rows[0];
                int appID = ApplicationTypeID;
                string appTitle = drAppType["ApplicationTypeTitle"].ToString();
                double appFees = Convert.ToDouble(drAppType["ApplicationFees"]);
                ApplicationType = new clsApplicationType(appID, appTitle, appFees);
            }

            return ApplicationType;
        }

        public static double GetApplicationTypeFees(int ApplicationTypeID)
        {
            return ApplicationTypes.GetApplicationTypeFees(ApplicationTypeID);
        }

        public bool UpdateApplicationType()
        {
            return ApplicationTypes.UpdateApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationTypeFees);
        }
    }
}
