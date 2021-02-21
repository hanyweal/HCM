using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class EmployeesUnderPrivateCompaniesDAL
    {
        public List<EmployeesUnderPrivateCompanies> GetEmployeesUnderPrivateCompanies(string EmployeeCodeNo)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesUnderPrivateCompanies.Where(x=> x.Nationality == "Saudi" && (string.IsNullOrEmpty(EmployeeCodeNo) ? x.EmployeeCodeNo == x.EmployeeCodeNo : x.EmployeeCodeNo == EmployeeCodeNo)).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
