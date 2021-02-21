using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class CompensationCalculationsForPrivateCompaniesEmployees : BaseCompensationCalculations
    {
        internal static double DaysInGregYear
        {
            get
            {
                return 12 * DaysInGregMonth;
            }
        }

        internal static double DaysInGregMonth
        {
            get
            {
                return 30;
            }
        }

        internal static double GetNormalVacationCompensation(double TotalSalary, double RemainingNormalVacationBalance)
        {
            return RemainingNormalVacationBalance * (TotalSalary / DaysInGregMonth);
        }

        internal static double GetEndOfServiceCompensation(DateTime? HiringDate, double TotalSalary, EndOfServicesCasesEnum Reason)
        {
            double Compensation = 0.0;

            double YearsCount = CompensationCalculationsForMICEmployees.GetServicePeriodByUmAlquraYears(HiringDate.Value.Date);
            if (Reason == EndOfServicesCasesEnum.Death || Reason == EndOfServicesCasesEnum.Resignation || Reason == EndOfServicesCasesEnum.Retirement || Reason == EndOfServicesCasesEnum.Termination)
                Compensation = CommonHelper.CalculateEndOfServiceCompensation(TotalSalary, YearsCount);
            else
                // later we will calcuate it
                Compensation = 0;

            return Math.Round(Compensation, 2);
        }

        internal static double GetAdditionalCompensation(DateTime? HiringDate, double TotalSalary, double RemainingYearsCountInService, double YearsCountInService)
        {
            if (YearsCountInService >= 25) // 25 years
                return RemainingYearsCountInService * TotalSalary * 2;
            else
                return AdditionalCompensationInSomeCases;
        }

        internal static double GetServicePeriodByGregYears(DateTime? HiringDate)
        {
            double period;
            period = Math.Round((GetServicePeriodByDays(HiringDate) / DaysInGregYear), 2);
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

        internal static double GetAgeByGregYears(DateTime? BirthDate)
        {
            double period;
            if (BirthDate.HasValue)
                period = Math.Round(((DateTime.Now.Date - BirthDate.Value.Date).Days / DaysInGregYear), 2);
            else
                period = 0;
            return period;
        }

        internal static double GetRemainingYearsInService(DateTime? BirthDate)
        {
            double RemainingYearsCountInService = 0;
            if (BirthDate.HasValue)
                RemainingYearsCountInService = RetirementAge - GetAgeByGregYears(BirthDate);

            if (RemainingYearsCountInService < 0) // it is means the age is more than 60 years
                RemainingYearsCountInService = 0;

            return Math.Round(RemainingYearsCountInService, 2);
        }
    }
}
