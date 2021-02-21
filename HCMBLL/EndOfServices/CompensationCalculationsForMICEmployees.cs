using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class CompensationCalculationsForMICEmployees : BaseCompensationCalculations
    {
        private static int MaxNormalVacationBalanceForNormalEmployee
        {
            get
            {
                return 180;
            }
        }

        internal static double DaysInUmAlquraYear
        {
            get
            {
                return 354.5;
            }
        }

        internal static double DaysInUmAlquraMonth
        {
            get
            {
                return 30;
            }
        }

        internal static double GetNormalVacationCompensation(EmployeesCareersHistoryBLL Employee, SalaryDetailsBLL Salary, int RemainingNormalVacationBalance)
        {
            var RankCategory = Employee.EmployeeCode.EmployeeCurrentJob.OrganizationJob.Rank.RankCategory.RankCategoryID;

            if (RankCategory == (int)RanksCategoriesEnum.Employee || RankCategory == (int)RanksCategoriesEnum.User)
            {
                var DaysWillBeCalculatedBasedOnIt = RemainingNormalVacationBalance <= MaxNormalVacationBalanceForNormalEmployee ? RemainingNormalVacationBalance : MaxNormalVacationBalanceForNormalEmployee;
                return DaysWillBeCalculatedBasedOnIt * (Salary.Benefits.BasicSalary / DaysInUmAlquraMonth);
            }
            else if (RankCategory == (int)RanksCategoriesEnum.ContractualSaudis || RankCategory == (int)RanksCategoriesEnum.SaudiLabor || RankCategory == (int)RanksCategoriesEnum.ContractualExpats)
                return RemainingNormalVacationBalance * (Salary.TotalSalary / DaysInUmAlquraMonth);

            return 0;
        }

        internal static double GetEndOfServiceCompensation(EmployeesCareersHistoryBLL Employee, EmployeesCareersHistoryBLL HiringRecord, SalaryDetailsBLL Salary, EndOfServicesCasesEnum Reason)
        {
            double Compensation = 0.0;
            var RankCategory = Employee.EmployeeCode.EmployeeCurrentJob.OrganizationJob.Rank.RankCategory.RankCategoryID;

            if (RankCategory == (int)RanksCategoriesEnum.Employee || RankCategory == (int)RanksCategoriesEnum.User)
            {
                if (Reason == EndOfServicesCasesEnum.Death || Reason == EndOfServicesCasesEnum.Resignation || Reason == EndOfServicesCasesEnum.Retirement || Reason == EndOfServicesCasesEnum.Termination)
                    return Salary.Benefits.BasicSalary * 6;
                else
                    return Salary.Benefits.BasicSalary * 4;
            }

            else if (RankCategory == (int)RanksCategoriesEnum.ContractualSaudis || RankCategory == (int)RanksCategoriesEnum.SaudiLabor || RankCategory == (int)RanksCategoriesEnum.ContractualExpats)
            {
                double YearsCount = CompensationCalculationsForMICEmployees.GetServicePeriodByUmAlquraYears(HiringRecord.JoinDate);
                if (Reason == EndOfServicesCasesEnum.Death || Reason == EndOfServicesCasesEnum.Resignation || Reason == EndOfServicesCasesEnum.Retirement || Reason == EndOfServicesCasesEnum.Termination)
                    Compensation = CommonHelper.CalculateEndOfServiceCompensation(Salary.TotalSalary, YearsCount);
                else
                    // later we will calcuate it
                    Compensation = 0;
            }

            return Math.Round(Compensation, 2);
        }

        internal static double GetAdditionalCompensation(EmployeesCareersHistoryBLL Employee, SalaryDetailsBLL Salary, double RemainingYearsCountInService, double YearsCountInService)
        {
            var RankCategory = Employee.EmployeeCode.EmployeeCurrentJob.OrganizationJob.Rank.RankCategory.RankCategoryID;

            if (RankCategory == (int)RanksCategoriesEnum.Employee || RankCategory == (int)RanksCategoriesEnum.User)
            {
                if (YearsCountInService >= 20) // 20 years
                    return RemainingYearsCountInService * Salary.Benefits.BasicSalary * 2;
                else
                    return AdditionalCompensationInSomeCases;
            }
            else if (RankCategory == (int)RanksCategoriesEnum.ContractualSaudis || RankCategory == (int)RanksCategoriesEnum.SaudiLabor || RankCategory == (int)RanksCategoriesEnum.ContractualExpats)
                if (YearsCountInService >= 25) // 25 years
                    return RemainingYearsCountInService * Salary.TotalSalary * 2;
                else
                    return AdditionalCompensationInSomeCases;

            return 0;
        }

        internal static double GetServicePeriodByUmAlquraYears(DateTime? HiringDate)
        {
            double period;
            period = Math.Round((GetServicePeriodByDays(HiringDate) / DaysInUmAlquraYear), 2);
            return period;
        }

        internal static double GetServicePeriodByDays(DateTime? HiringDate)
        {
            int period;
            if (HiringDate.HasValue)
                period = (DateTime.Now.Date - HiringDate.Value.Date).Days;
            else
                period = 0;
            return period;
        }

        internal static double GetAgeByUmAlquraYears(DateTime? BirthDate)
        {
            double period;
            if (BirthDate.HasValue)
                period = Math.Round(((DateTime.Now.Date - BirthDate.Value.Date).Days / DaysInUmAlquraYear), 2);
            else
                period = 0;
            return period;
        }

        internal static double GetRemainingYearsInService(DateTime? BirthDate)
        {
            double RemainingYearsCountInService = 0;
            if (BirthDate.HasValue)
                RemainingYearsCountInService = RetirementAge - GetAgeByUmAlquraYears(BirthDate);

            return RemainingYearsCountInService;
        }
    }
}
