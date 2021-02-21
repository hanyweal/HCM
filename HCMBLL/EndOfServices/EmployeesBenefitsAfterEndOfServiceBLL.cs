using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EmployeesBenefitsAfterEndOfServiceBLL
    {
        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public EmployeesCareersHistoryBLL EmployeeHiringRecord { get; set; }

        public SalaryDetailsBLL SalaryDetails { get; set; }

        public int TotalRemainingBalance { get; set; }

        public double RemainingNormalVacationBalanceCompensation { get; set; }

        public string ServicePeriod { get; set; }

        public double EndOfServiceCompensation { get; set; }

        public double RemainingYearsCountInService { get; set; }

        public double AdditionalCompensation { get; set; }


        public List<EmployeesBenefitsAfterEndOfServiceBLL> GetEmployeesBenefits(string EmployeeCodeNo)
        {
            try
            {
                List<EmployeesBenefitsAfterEndOfServiceBLL> EmployeesBenefitsBLLList = new List<EmployeesBenefitsAfterEndOfServiceBLL>();
                List<SalaryDetailsBLL> SalaryDetailsList = new List<SalaryDetailsBLL>();
                List<EmployeesCareersHistory> ActiveEmployeesList = string.IsNullOrEmpty(EmployeeCodeNo) ? new EmployeesCareersHistoryDAL().GetActiveEmployeesCareersHistory().ToList() : new EmployeesCareersHistoryDAL().GetActiveEmployeesCareersHistory(EmployeeCodeNo).ToList();
                List<EmployeesCareersHistory> EmployeesHiringRecordsList = string.IsNullOrEmpty(EmployeeCodeNo) ? new EmployeesCareersHistoryDAL().GetHiringRecordsForEmployees().ToList() : new EmployeesCareersHistoryDAL().GetHiringRecordsForEmployees(EmployeeCodeNo).ToList();
                List<EmployeesAllowancesBLL> AllowancesList = string.IsNullOrEmpty(EmployeeCodeNo) ? new EmployeesAllowancesBLL().GetEmployeesAllowances().ToList() : new EmployeesAllowancesBLL().GetEmployeesAllowances(EmployeeCodeNo);
                //List<EmployeesAllowancesBLL> AllowancesList = new List<EmployeesAllowancesBLL>();
                List<GovernmentFundsBLL> GovernmentFundsList = string.IsNullOrEmpty(EmployeeCodeNo) ? new GovernmentFundsBLL().GetGovernmentFunds() : new GovernmentFundsBLL().GetGovernmentFunds(EmployeeCodeNo);
                List<BasicSalariesBLL> BasicSalariesDefination = new BasicSalariesBLL().GetBasicSalaries();
                List<RanksBLL> Ranks = new RanksBLL().GetRanks();
                List<EmployeesNormalVacationsBalances> EmployeesNormalVacationsBalancesList = new EmployeesNormalVacationsBalancesDAL().GetNormalVacationsBalances();

                foreach (var item in ActiveEmployeesList)
                {
                    EmployeesBenefitsAfterEndOfServiceBLL EmployeeBenefitsAfterEndOfServiceBLL = new EmployeesBenefitsAfterEndOfServiceBLL();
                    EmployeesCareersHistoryBLL EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item);
                    EmployeeCareerHistory.EmployeeCode.EmployeeCurrentJob = EmployeeCareerHistory;

                    EmployeeBenefitsAfterEndOfServiceBLL.EmployeeCareerHistory = EmployeeCareerHistory;
                    EmployeeBenefitsAfterEndOfServiceBLL.SalaryDetails = new SalaryDetailsBLL().GetSalaryDetailsByEmployeeCodeNo(EmployeeCareerHistory.EmployeeCode,
                                                                                                                                AllowancesList,
                                                                                                                                GovernmentFundsList,
                                                                                                                                BasicSalariesDefination,
                                                                                                                                Ranks);
                    EmployeeBenefitsAfterEndOfServiceBLL.SalaryDetails.Employee = EmployeeCareerHistory;

                    #region NormalVacationBalanceCompensation
                    EmployeesNormalVacationsBalances ee = EmployeesNormalVacationsBalancesList.FirstOrDefault(x => x.EmployeeCodeID == EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
                    if (ee is null) ee = new EmployeesNormalVacationsBalances();
                    EmployeeBenefitsAfterEndOfServiceBLL.TotalRemainingBalance = ee.TotalRemainingBalance;
                    EmployeeBenefitsAfterEndOfServiceBLL.RemainingNormalVacationBalanceCompensation = Math.Round(CompensationCalculationsForMICEmployees.GetNormalVacationCompensation(EmployeeCareerHistory,
                                                                                                                                                                          EmployeeBenefitsAfterEndOfServiceBLL.SalaryDetails,
                                                                                                                                                                          ee.TotalRemainingBalance), 2);
                    #endregion

                    #region EndOfServiceCompensation
                    EmployeeBenefitsAfterEndOfServiceBLL.EmployeeHiringRecord = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(EmployeesHiringRecordsList.FirstOrDefault(x => x.EmployeeCodeID == item.EmployeeCodeID));
                    EmployeeBenefitsAfterEndOfServiceBLL.ServicePeriod = CompensationCalculationsForMICEmployees.GetServicePeriodByUmAlquraYears(EmployeeBenefitsAfterEndOfServiceBLL.EmployeeHiringRecord != null ? EmployeeBenefitsAfterEndOfServiceBLL.EmployeeHiringRecord.JoinDate.Date : (DateTime?)null).ToString();
                    EmployeeBenefitsAfterEndOfServiceBLL.EndOfServiceCompensation = CompensationCalculationsForMICEmployees.GetEndOfServiceCompensation(EmployeeBenefitsAfterEndOfServiceBLL.EmployeeCareerHistory,
                                                                                                                                                        EmployeeBenefitsAfterEndOfServiceBLL.EmployeeHiringRecord,
                                                                                                                                                        EmployeeBenefitsAfterEndOfServiceBLL.SalaryDetails,
                                                                                                                                                        Enums.EndOfServicesCasesEnum.Retirement);
                    #endregion

                    #region AdditionalCompensation
                    EmployeeBenefitsAfterEndOfServiceBLL.RemainingYearsCountInService = CompensationCalculationsForMICEmployees.GetRemainingYearsInService(item.EmployeesCodes.Employees.EmployeeBirthDate != null ? item.EmployeesCodes.Employees.EmployeeBirthDate : (DateTime?)null);

                    EmployeeBenefitsAfterEndOfServiceBLL.AdditionalCompensation = CompensationCalculationsForMICEmployees.GetAdditionalCompensation(EmployeeBenefitsAfterEndOfServiceBLL.EmployeeCareerHistory,
                                                                                                                                                    EmployeeBenefitsAfterEndOfServiceBLL.SalaryDetails,
                                                                                                                                                    EmployeeBenefitsAfterEndOfServiceBLL.RemainingYearsCountInService,
                                                                                                                                                    double.Parse(EmployeeBenefitsAfterEndOfServiceBLL.ServicePeriod));
                    #endregion                                                                                                                      

                    EmployeesBenefitsBLLList.Add(EmployeeBenefitsAfterEndOfServiceBLL);
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
