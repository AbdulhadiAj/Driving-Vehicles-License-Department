using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLoginLayer
{
    public class clsCountry
    {


        public static DataTable GetCountriesInfo()
        {
            return Countries.GetCountriesInfo();
        }

        public static DataTable GetCountriesNames()
        {
            return Countries.GetCountriesNames();
        }

    }
}
