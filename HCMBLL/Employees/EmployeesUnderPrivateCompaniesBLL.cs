using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EmployeesUnderPrivateCompaniesBLL
    {
        public string EmployeeCodeNo { get; set; }
        public string EmployeeIDNo { get; set; }
        public string EmployeeNameAr { get; set; }
        public string GenderName { get; set; }
        public string OrganizationName { get; set; }
        public string RankCategoryName { get; set; }
        public string RankName { get; set; }
        public string JobName { get; set; }
        public string Department { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TotalSalary { get; set; }
        public string QualificationDegreeName { get; set; }
        public string QualificationName { get; set; }
        public string ExactSpecialization { get; set; }
        public DateTime? HiringDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal RemainingNormalVacationBalance { get; set; }
        public string Nationality { get; set; }
        public double Age
        {
            get
            {
                return CompensationCalculationsForPrivateCompaniesEmployees.GetAgeByGregYears(BirthDate);
            }
        }

        public List<EmployeesUnderPrivateCompaniesBLL> GetEmployeesUnderPrivateCompanies(string EmployeeCodeNo)
        {
            try
            {
                List<EmployeesUnderPrivateCompaniesBLL> EmployeesUnderPrivateCompaniesBLLList = new List<EmployeesUnderPrivateCompaniesBLL>();
                List<EmployeesUnderPrivateCompanies> EmployeesUnderPrivateCompaniesList = new EmployeesUnderPrivateCompaniesDAL().GetEmployeesUnderPrivateCompanies(EmployeeCodeNo);
                foreach (var item in EmployeesUnderPrivateCompaniesList)
                {
                    EmployeesUnderPrivateCompaniesBLLList.Add(new EmployeesUnderPrivateCompaniesBLL() 
                    {
                        EmployeeCodeNo = item.EmployeeCodeNo,
                        EmployeeIDNo = item.EmployeeIDNo,
                        EmployeeNameAr = item.NameAr,
                        GenderName = item.GenderName,
                        OrganizationName = item.OrganizationName,
                        RankCategoryName = Globalization.SaudiContractorText,
                        RankName = item.RankName,
                        BasicSalary = item.BasicSalary,
                        TotalSalary = item.TotalSalary,
                        HiringDate = item.HiringDate,
                        RemainingNormalVacationBalance = item.RemainingNormalVacationBalance,
                        BirthDate = item.BirthDate
                    });
                }
                return EmployeesUnderPrivateCompaniesBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
