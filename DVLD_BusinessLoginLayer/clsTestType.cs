using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLoginLayer
{
    public class clsTestType
    {
        public enum enTestTypes { Vision = 1, Written = 2, Practical = 3};

        public int TestTypeID { get; }
        public string TestTypeTitle;
        public string TestTypeDescription;
        public double TestTypeFees;

        clsTestType()
        {
            TestTypeID = -1;
            TestTypeTitle = "";
            TestTypeDescription = "";
            TestTypeFees = 0;
        }

        clsTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, double TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;
        }

        public static DataTable GetTestTypesInfo()
        {
            return TestTypes.GetTestTypesInfo();
        }

        public static clsTestType GetTestType(int TestTypeID)
        {
            clsTestType TestType = new clsTestType();
            DataTable dt = TestTypes.GetTestType(TestTypeID);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                int testID = TestTypeID;
                string testTitle = dr["TestTypeTitle"].ToString();
                string testDesc = dr["TestTypeDescription"].ToString();
                double testFees = Convert.ToDouble(dr["TestTypeFees"]);
                TestType = new clsTestType(testID, testTitle, testDesc, testFees);
            }

            return TestType;
        }

        public bool UpdateTestType()
        {
            return TestTypes.UpdateTestType(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
        }

        public static string GetTestTypeTitle(int TestTypeID)
        {
            return TestTypes.GetTestTypeTitle(TestTypeID);
        }

        public static double GetTestTypeFees(int TestTypeID)
        {
            return TestTypes.GetTestTypeFees(TestTypeID);
        }
    }
}
