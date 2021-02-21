using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EmployeesPrivateCompaniesBenefitsAfterEndOfServiceBLL
    {
        public EmployeesUnderPrivateCompaniesBLL Employee { get; set; }

        public double RemainingNormalVacationBalanceCompensation { get; set; }

        public string ServicePeriod { get; set; }

        public double EndOfServiceCompensation { get; set; }

        public double RemainingYearsCountInService { get; set; }

        public double AdditionalCompensation { get; set; }

        public List<EmployeesPrivateCompaniesBenefitsAfterEndOfServiceBLL> GetEmployeesBenefits(string EmployeeCodeNo)
        {
            try
            {
                List<EmployeesPrivateCompaniesBenefitsAfterEndOfServiceBLL> EmployeesBenefitsBLLList = new List<EmployeesPrivateCompaniesBenefitsAfterEndOfServiceBLL>();
                List<EmployeesUnderPrivateCompaniesBLL> ActiveEmployeesList = new EmployeesUnderPrivateCompaniesBLL().GetEmployeesUnderPrivateCompanies(EmployeeCodeNo);

                foreach (var item in ActiveEmployeesList)
                {
                    EmployeesPrivateCompaniesBenefitsAfterEndOfServiceBLL EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL = new EmployeesPrivateCompaniesBenefitsAfterEndOfServiceBLL();

                    EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.Employee = item;

                    #region NormalVacationBalanceCompensation

                    EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.RemainingNormalVacationBalanceCompensation = Math.Round(CompensationCalculationsForPrivateCompaniesEmployees.GetNormalVacationCompensation(double.Parse(item.TotalSalary.ToString()), double.Parse(item.RemainingNormalVacationBalance.ToString())), 2);
                    #endregion

                    #region EndOfServiceCompensation

                    EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.ServicePeriod = CompensationCalculationsForPrivateCompaniesEmployees.GetServicePeriodByGregYears(EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.Employee.HiringDate).ToString();
                    EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.EndOfServiceCompensation = CompensationCalculationsForPrivateCompaniesEmployees.GetEndOfServiceCompensation(EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.Employee.HiringDate,
                                                                                                                                                                                    double.Parse(item.TotalSalary.ToString()),
                                                                                                                                                                                    Enums.EndOfServicesCasesEnum.Retirement);
                    #endregion

                    #region AdditionalCompensation
                    EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.RemainingYearsCountInService = CompensationCalculationsForPrivateCompaniesEmployees.GetRemainingYearsInService(item.BirthDate != null ? item.BirthDate : null);

                    EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.AdditionalCompensation = CompensationCalculationsForPrivateCompaniesEmployees.GetAdditionalCompensation(EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.Employee.HiringDate,
                                                                                                                                                     double.Parse(EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.Employee.TotalSalary.ToString()),
                                                                                                                                                    EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.RemainingYearsCountInService,
                                                                                                                                                    double.Parse(EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.ServicePeriod));
                    #endregion                                                                                                                      

                    EmployeesBenefitsBLLList.Add(EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL);
                }
                return EmployeesBenefitsBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
