using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakafulDTO;

namespace HCMBLL
{
    public class SalaryDetailsBLL
    {
        public EmployeesCareersHistoryBLL Employee { get; set; }

        public SalaryBenefits Benefits { get; set; }

        public SalaryDeductions Deductions { get; set; }

        public double TotalSalary
        {
            get
            {
                return Math.Round((Benefits.BasicSalary +
                                    Benefits.TransfareAllowance +
                                    Benefits.TotalAllowances), 2);
            }
        }

        public double NetSalary
        {
            get
            {
                return Math.Round((Benefits.BasicSalary +
                                  Benefits.TransfareAllowance +
                                  Benefits.TotalAllowances) -
                                  (Deductions != null ? Deductions.TotalDeductions : 0), 2);
            }
        }

        ///// <summary>
        ///// Dated : 24-08-2020 : getting ContractualSaudi TransfareAllowance from ContractorBasicSalary Table
        ///// </summary>
        ///// <param name="EmployeeCode"></param>
        ///// <returns></returns>
        //private SalaryDetailsBLL GetSalaryDetailsBenefitsByEmployeeCodeNo(EmployeesCodesBLL EmployeeCode)
        //{
        //    try
        //    {
        //        double FirstDegreeBasicSalary = 0, CurrentDegreeBasicSalary = 0;
        //        double BasicSalary = 0, TransfareAllowance = 0, TotalAllowances = 0;
        //        SalaryDetailsBLL SalaryDetails = new SalaryDetailsBLL();
        //        //EmployeesCodesBLL EmployeeCode = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);
        //        if (EmployeeCode != null)
        //        {
        //            EmployeesCareersHistoryBLL Employee = EmployeeCode.EmployeeCurrentJob;
        //            if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.Employee)
        //                BasicSalary = new BasicSalariesBLL().GetBasicSalary(Employee.OrganizationJob.Rank.RankID, Employee.CareerDegree.CareerDegreeID).BasicSalary;
        //            else if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.User)
        //                BasicSalary = new BasicSalariesBLL().GetBasicSalary(Employee.OrganizationJob.Rank.RankID, Employee.CareerDegree.CareerDegreeID).BasicSalary;
        //            else if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.SaudiLabor)
        //                BasicSalary = new BasicSalariesBLL().GetBasicSalary(Employee.OrganizationJob.Rank.RankID, Employee.CareerDegree.CareerDegreeID).BasicSalary;
        //            else if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.ContractualSaudis)
        //                BasicSalary = new ContractorsBasicSalariesBLL().GetByEmployeeCodeID(EmployeeCode.EmployeeCodeID).BasicSalary;
        //            else if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.ContractualExpats)
        //                BasicSalary = new ContractorsBasicSalariesBLL().GetByEmployeeCodeID(EmployeeCode.EmployeeCodeID).BasicSalary;
        //            else
        //                BasicSalary = 0;

        //            // Dated : 24-08-2020 : getting ContractualSaudi TransfareAllowance from ContractorBasicSalary Table
        //            if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.ContractualSaudis)
        //                TransfareAllowance = new ContractorsBasicSalariesBLL().GetByEmployeeCodeID(EmployeeCode.EmployeeCodeID).TransfareAllowance;
        //            else
        //                TransfareAllowance = new RanksBLL().GetByRankID(Employee.OrganizationJob.Rank.RankID).TransfareAllowance;

        //            SalaryDetails.Benefits = new SalaryBenefits()
        //            {
        //                BasicSalary = BasicSalary,
        //                TransfareAllowance = TransfareAllowance,
        //                EmployeesAllowances = new EmployeesCodesBLL().GetActiveAllownacessByEmployeeCodeID(EmployeeCode.EmployeeCodeID),
        //            };

        //            // some allowances should be calcualted based on current basic salary of employee, and some should be calculated based on basic salary of first degree of employee current rank 
        //            FirstDegreeBasicSalary = new BasicSalariesBLL().GetBasicSalary(EmployeeCode.EmployeeCurrentJob.OrganizationJob.Rank.RankID, 1).BasicSalary;
        //            CurrentDegreeBasicSalary = SalaryDetails.Benefits.BasicSalary;

        //            foreach (var item in SalaryDetails.Benefits.EmployeesAllowances)
        //            {
        //                if (item.Allowance.AllowanceAmountType.AllowanceAmountTypeID == Convert.ToInt16(AllowancesAmountTypesEnum.Fixed))
        //                    SalaryDetails.Benefits.TotalAllowances = TotalAllowances + item.Allowance.AllowanceAmount;
        //                else if (item.Allowance.AllowanceAmountType.AllowanceAmountTypeID == Convert.ToInt16(AllowancesAmountTypesEnum.Percentage))
        //                {
        //                    if (item.Allowance.AllowanceCalculationType.AllowanceCalculationTypeID == Convert.ToInt16(AllowancesCalculationTypesEnum.BasedOnBasicSalaryOfFirstDegree))
        //                        TotalAllowances = TotalAllowances + ((item.Allowance.AllowanceAmount / 100) * FirstDegreeBasicSalary);
        //                    else
        //                        TotalAllowances = TotalAllowances + ((item.Allowance.AllowanceAmount / 100) * CurrentDegreeBasicSalary);
        //                }
        //            }

        //            SalaryDetails.Benefits.TotalAllowances = TotalAllowances;
        //        }
        //        return SalaryDetails;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //private SalaryDetailsBLL GetSalaryDetailsDeductionsByEmployeeCodeNo(EmployeesCodesBLL EmployeeCode, double BasicSalary)
        //{
        //    try
        //    {
        //        SalaryDetailsBLL SalaryDetails = new SalaryDetailsBLL();
        //        //EmployeesCodesBLL EmployeeCode = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);

        //        SalaryDetails.Deductions = new SalaryDeductions()
        //        {
        //            //RetirmentDeduction = Math.Round(new BasicSalariesBLL().GetBasicSalary(EmployeeCode.EmployeeCurrentJob.OrganizationJob.Rank.RankID, EmployeeCode.EmployeeCurrentJob.CareerDegree.CareerDegreeID).BasicSalary * .09, 2)
        //            RetirmentDeduction = Math.Round(BasicSalary * EmployeeCode.EmployeeCurrentJob.OrganizationJob.Rank.RankCategory.RetirementPercentage / 100, 2),
        //            //TakafulDeductions = new TakafulDeductionsBLL().GetActiveTakafulDeductions(EmployeeCode.Employee.EmployeeID),
        //            TakafulDeductions = new List<Deduction>(),
        //            GovernmentFundsDeductions = new GovernmentFundsBLL().GetActiveGovernmentFundsByEmployeeCodeID(EmployeeCode.EmployeeCodeID),
        //        };

        //        SalaryDetails.Deductions.TotalDeductions = SalaryDetails.Deductions.RetirmentDeduction +
        //                                                   double.Parse(SalaryDetails.Deductions.TakafulDeductions.Sum(x => x.DeductionAmount).ToString()) +
        //                                                   SalaryDetails.Deductions.GovernmentFundsDeductions.Sum(x => x.MonthlyDeductionAmount);

        //        return SalaryDetails;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// This is to calculate employee net salary, formula of net salary :
        ///// (BasicSalary + TransfareAllowance + TotalAllowances) - (RetirmentDeduction + TakafulDeduction + GovernmentFundsDeductions)
        ///// </summary>
        ///// <param name="EmployeeCodeNo"></param>
        ///// <returns></returns>
        //public SalaryDetailsBLL GetSalaryDetailsByEmployeeCodeNo(string EmployeeCodeNo)
        //{
        //    try
        //    {
        //        EmployeesCodesBLL EmployeeCode = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);
        //        SalaryDetailsBLL SalaryDetails = new SalaryDetailsBLL();
        //        SalaryDetails.Benefits = SalaryDetails.GetSalaryDetailsBenefitsByEmployeeCodeNo(EmployeeCode).Benefits;
        //        SalaryDetails.Deductions = SalaryDetails.GetSalaryDetailsDeductionsByEmployeeCodeNo(EmployeeCode, SalaryDetails.Benefits.BasicSalary).Deductions;

        //        //#region Total Allowances
        //        ////only Active allowances
        //        //double TotalAllowances = 0;
        //        //double FirstDegreeBasicSalary = 0;
        //        //double CurrentDegreeBasicSalary = 0;
        //        //foreach (var item in SalaryDetails.Benefits.EmployeesAllowances)
        //        //{
        //        //    if (item.Allowance.AllowanceAmountType.AllowanceAmountTypeID == Convert.ToInt16(AllowancesAmountTypesEnum.Fixed))
        //        //        TotalAllowances = TotalAllowances + item.Allowance.AllowanceAmount;
        //        //    else if (item.Allowance.AllowanceAmountType.AllowanceAmountTypeID == Convert.ToInt16(AllowancesAmountTypesEnum.Percentage))
        //        //    {
        //        //        if(item.Allowance.AllowanceCalculationType.AllowanceCalculationTypeID == Convert.ToInt16(AllowancesCalculationTypesEnum.BasedOnBasicSalaryOfFirstDegree))
        //        //            TotalAllowances = TotalAllowances + ((item.Allowance.AllowanceAmount / 100) * FirstDegreeBasicSalary);
        //        //        else
        //        //            TotalAllowances = TotalAllowances + ((item.Allowance.AllowanceAmount / 100) * CurrentDegreeBasicSalary);
        //        //    }
        //        //}
        //        //#endregion

        //        #region Net salary
        //        SalaryDetails.NetSalary = Math.Round((SalaryDetails.Benefits.BasicSalary +
        //                                   SalaryDetails.Benefits.TransfareAllowance +
        //                                   SalaryDetails.Benefits.TotalAllowances) -
        //                                   (SalaryDetails.Deductions.TotalDeductions), 2);
        //        #endregion

        //        return SalaryDetails;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}



        /// <summary>
        /// Dated : 24-08-2020 : getting ContractualSaudi TransfareAllowance from ContractorBasicSalary Table
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <returns></returns>
        private SalaryDetailsBLL GetSalaryDetailsBenefitsByEmployeeCodeNo(EmployeesCodesBLL EmployeeCode, List<EmployeesAllowancesBLL> Allowances, List<BasicSalariesBLL> BasicSalaries, List<RanksBLL> Ranks)
        {
            try
            {
                double FirstDegreeBasicSalary = 0, CurrentDegreeBasicSalary = 0;
                double BasicSalary = 0, TransfareAllowance = 0, TotalAllowances = 0;
                SalaryDetailsBLL SalaryDetails = new SalaryDetailsBLL();

                if (EmployeeCode != null)
                {
                    EmployeesCareersHistoryBLL Employee = EmployeeCode.EmployeeCurrentJob;
                    if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.Employee)
                        BasicSalary = new BasicSalariesBLL().GetBasicSalary(Employee.OrganizationJob.Rank.RankID, Employee.CareerDegree.CareerDegreeID).BasicSalary;
                    else if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.User)
                        BasicSalary = new BasicSalariesBLL().GetBasicSalary(Employee.OrganizationJob.Rank.RankID, Employee.CareerDegree.CareerDegreeID).BasicSalary;
                    else if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.SaudiLabor)
                        BasicSalary = new BasicSalariesBLL().GetBasicSalary(Employee.OrganizationJob.Rank.RankID, Employee.CareerDegree.CareerDegreeID).BasicSalary;
                    else if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.ContractualSaudis)
                        BasicSalary = new ContractorsBasicSalariesBLL().GetByEmployeeCodeID(EmployeeCode.EmployeeCodeID).BasicSalary;
                    else if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.ContractualExpats)
                        BasicSalary = new ContractorsBasicSalariesBLL().GetByEmployeeCodeID(EmployeeCode.EmployeeCodeID).BasicSalary;
                    else
                        BasicSalary = 0;

                    // Dated : 24-08-2020 : getting ContractualSaudi TransfareAllowance from ContractorBasicSalary Table
                    if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.ContractualSaudis)
                        TransfareAllowance = new ContractorsBasicSalariesBLL().GetByEmployeeCodeID(EmployeeCode.EmployeeCodeID).TransfareAllowance;
                    else
                        TransfareAllowance = Ranks.FirstOrDefault(x => x.RankID == Employee.OrganizationJob.Rank.RankID).TransfareAllowance;

                    SalaryDetails.Benefits = new SalaryBenefits()
                    {
                        BasicSalary = BasicSalary,
                        TransfareAllowance = TransfareAllowance,
                        EmployeesAllowances = Allowances.Where(x => x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID == EmployeeCode.EmployeeCodeID).ToList(),
                    };

                    // some allowances should be calcualted based on current basic salary of employee, and some should be calculated based on basic salary of first degree of employee current rank 
                    if (Employee.OrganizationJob.Rank.RankCategory.RankCategoryID != (int)RanksCategoriesEnum.ContractualExpats && Employee.OrganizationJob.Rank.RankCategory.RankCategoryID != (int)RanksCategoriesEnum.ContractualSaudis)
                    {
                        BasicSalariesBLL basicSalary = BasicSalaries.FirstOrDefault(x => x.Rank.RankID == EmployeeCode.EmployeeCurrentJob.OrganizationJob.Rank.RankID && x.CareerDegree.CareerDegreeID == 1);
                        FirstDegreeBasicSalary = basicSalary != null ? basicSalary.BasicSalary : 0;
                    }

                    CurrentDegreeBasicSalary = SalaryDetails.Benefits.BasicSalary;

                    foreach (var item in SalaryDetails.Benefits.EmployeesAllowances)
                    {
                        if (item.Allowance.AllowanceAmountType.AllowanceAmountTypeID == Convert.ToInt16(AllowancesAmountTypesEnum.Fixed))
                            TotalAllowances += item.Allowance.AllowanceAmount;
                        else if (item.Allowance.AllowanceAmountType.AllowanceAmountTypeID == Convert.ToInt16(AllowancesAmountTypesEnum.Percentage))
                        {
                            if (item.Allowance.AllowanceCalculationType.AllowanceCalculationTypeID == Convert.ToInt16(AllowancesCalculationTypesEnum.BasedOnBasicSalaryOfFirstDegree))
                                TotalAllowances += ((item.Allowance.AllowanceAmount / 100) * FirstDegreeBasicSalary);
                            else
                                TotalAllowances += ((item.Allowance.AllowanceAmount / 100) * CurrentDegreeBasicSalary);
                        }
                    }

                    SalaryDetails.Benefits.TotalAllowances = Math.Round(TotalAllowances, 2);
                    //SalaryDetails.TotalSalary = SalaryDetails.Benefits.BasicSalary + SalaryDetails.Benefits.TotalAllowances + SalaryDetails.Benefits.TransfareAllowance;
                }
                return SalaryDetails;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Dated : 05-12-2020 
        /// Task # 217 -- Deduct GovernmentFundsDeductions only when (IsActive = true && RemainingDeductionAmount > 0)
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="BasicSalary"></param>
        /// <param name="GovernmentFunds"></param>
        /// <returns></returns>
        private SalaryDetailsBLL GetSalaryDetailsDeductionsByEmployeeCodeNo(EmployeesCodesBLL EmployeeCode, double BasicSalary, List<GovernmentFundsBLL> GovernmentFunds)
        {
            try
            {
                SalaryDetailsBLL SalaryDetails = new SalaryDetailsBLL();

                SalaryDetails.Deductions = new SalaryDeductions()
                {
                    RetirmentDeduction = Math.Round(BasicSalary * EmployeeCode.EmployeeCurrentJob.OrganizationJob.Rank.RankCategory.RetirementPercentage / 100, 2),
                    TakafulDeductions = new List<Deduction>(),
                    GovernmentFundsDeductions = GovernmentFunds.Where(x => x.EmployeeCode.EmployeeCodeID == EmployeeCode.EmployeeCodeID
                                                                    && (x.IsActive.HasValue ? x.IsActive.Value : false) == true
                                                                    && x.RemainingDeductionAmount > 0).ToList(),
                };

                SalaryDetails.Deductions.TotalDeductions = Math.Round(SalaryDetails.Deductions.RetirmentDeduction +
                                                                      double.Parse(SalaryDetails.Deductions.TakafulDeductions.Sum(x => x.DeductionAmount).ToString()) +
                                                                      SalaryDetails.Deductions.GovernmentFundsDeductions.Sum(x => x.MonthlyDeductionAmount), 2);
                return SalaryDetails;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This is to calculate employee net salary, formula of net salary :
        /// (BasicSalary + TransfareAllowance + TotalAllowances) - (RetirmentDeduction + TakafulDeduction + GovernmentFundsDeductions)
        /// </summary>
        /// <param name="EmployeeCodeNo"></param>
        /// <returns></returns>
        public SalaryDetailsBLL GetSalaryDetailsByEmployeeCodeNo(EmployeesCodesBLL EmployeeCode, List<EmployeesAllowancesBLL> Allowances, List<GovernmentFundsBLL> GovernmentFunds, List<BasicSalariesBLL> BasicSalariesDefination, List<RanksBLL> Ranks)
        {
            try
            {
                SalaryDetailsBLL SalaryDetails = new SalaryDetailsBLL();
                SalaryDetails.Benefits = SalaryDetails.GetSalaryDetailsBenefitsByEmployeeCodeNo(EmployeeCode, Allowances, BasicSalariesDefination, Ranks).Benefits;
                SalaryDetails.Deductions = SalaryDetails.GetSalaryDetailsDeductionsByEmployeeCodeNo(EmployeeCode, SalaryDetails.Benefits.BasicSalary, GovernmentFunds).Deductions;


                #region Net salary
                //SalaryDetails.NetSalary = Math.Round((SalaryDetails.Benefits.BasicSalary +
                //                           SalaryDetails.Benefits.TransfareAllowance +
                //                           SalaryDetails.Benefits.TotalAllowances) -
                //                           (SalaryDetails.Deductions.TotalDeductions), 2);
                #endregion

                return SalaryDetails;
            }
            catch
            {
                throw;
            }
        }

        public List<SalaryDetailsBLL> GetSalaryDetails(string EmployeeCodeNo)
        {
            try
            {
                List<SalaryDetailsBLL> SalaryDetailsList = new List<SalaryDetailsBLL>();
                List<EmployeesCareersHistory> EmployeesBLLs = string.IsNullOrEmpty(EmployeeCodeNo) ? new EmployeesCareersHistoryDAL().GetActiveEmployeesCareersHistory() : new EmployeesCareersHistoryDAL().GetActiveEmployeesCareersHistory(EmployeeCodeNo);//.Take(200).ToList();//.Where(x=> x.EmployeesCodes.EmployeeCodeNo == EmployeeCodeNo).ToList();
                List<EmployeesAllowancesBLL> Allowances = string.IsNullOrEmpty(EmployeeCodeNo) ? new EmployeesAllowancesBLL().GetEmployeesAllowances() : new EmployeesAllowancesBLL().GetEmployeesAllowances(EmployeeCodeNo);
                List<GovernmentFundsBLL> GovernmentFunds = string.IsNullOrEmpty(EmployeeCodeNo) ? new GovernmentFundsBLL().GetGovernmentFunds() : new GovernmentFundsBLL().GetGovernmentFunds(EmployeeCodeNo);
                List<BasicSalariesBLL> BasicSalariesDefination = new BasicSalariesBLL().GetBasicSalaries();
                List<RanksBLL> Ranks = new RanksBLL().GetRanks();

                foreach (var item in EmployeesBLLs)
                {
                    SalaryDetailsBLL SalaryDetail = new SalaryDetailsBLL();
                    EmployeesCareersHistoryBLL EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item);
                    EmployeeCareerHistory.EmployeeCode.EmployeeCurrentJob = EmployeeCareerHistory;

                    SalaryDetail = this.GetSalaryDetailsByEmployeeCodeNo(EmployeeCareerHistory.EmployeeCode, Allowances, GovernmentFunds, BasicSalariesDefination, Ranks);
                    SalaryDetail.Employee = EmployeeCareerHistory;

                    SalaryDetailsList.Add(SalaryDetail);
                }
                return SalaryDetailsList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}