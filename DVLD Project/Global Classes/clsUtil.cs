using DVLD_BusinessLoginLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }

    }
}
