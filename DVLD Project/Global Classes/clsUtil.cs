using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_BusinessLoginLayer;

namespace DVLD_Project.Global_Classes
{
    public static class clsUtil
    {

        public static string RenameFileUsingGUID(string file)
        {
            string guid = Guid.NewGuid().ToString();
            FileInfo fi = new FileInfo(file);
            string newFileName = guid + fi.Extension;
            return newFileName;
        }

    }
}
