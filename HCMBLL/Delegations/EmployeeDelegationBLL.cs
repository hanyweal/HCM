using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EmployeeDelegationBLL
    {
        public EmployeeDelegationBLL()
        {

        }

        public EmployeeDelegationBLL(DateTime DelegationStartDate, DateTime DelegationEndDate, int DelegationKindID, EmployeesCodesBLL EmployeeCode)
        {
            this.DelegationStartDate = DelegationStartDate;
            this.DelegationEndDate = DelegationEndDate;
            this.EmployeeCode = EmployeeCode;
            this.DelegationKindID = DelegationKindID;
        }

        private EmployeesCodesBLL EmployeeCode { get; set; }

        private DateTime DelegationStartDate { get; set; }

        private DateTime DelegationEndDate { get; set; }

        private int DelegationKindID { get; set; }

        public int DelegationFiscalYear
        {
            get
            {
                //int year = new Globals.Calendar().IsHijri(DelegationStartDate) ? Globals.Calendar.GetGregYear(DelegationStartDate) : DelegationStartDate.Year;
                //int year = Globals.Calendar.GetUmAlQuraYear(DelegationStartDate.Date);
                int year = DelegationStartDate.Year;
                //return new FinancialYearsBLL().GetByFinancialYear(year).FinancialYear;
                return year;
            }
        }

        public int DelegationBalance
        {
            get
            {
                return 60;
            }
            set
            {
                DelegationBalance = value;
            }
        }

        public virtual int DelegationRemainingBalance
        {
            get
            {
                return DelegationBalance - DelegationConsumedBalance;
            }
            set
            {
                DelegationRemainingBalance = value;
            }
        }

        public virtual int DelegationConsumedBalance
        {
            get
            {
                return new DelegationsDetailsBLL().GetDelegationConsumedByEmployeeCodeID(this.DelegationStartDate.Year, this.EmployeeCode.EmployeeCodeID, (int)DelegationsKindsEnum.Tasks);
            }
            set
            {
                DelegationConsumedBalance = value;
            }
        }

        public Result IsValid()
        {
            Result result = null;

            #region Validation for Delegation Dates
            //CultureInfo cul = Thread.CurrentThread.CurrentCulture;
            ////Thread.CurrentThread.CurrentCulture = cul;
            ////Thread.CurrentThread.CurrentUICulture = cul;
            //List<FinancialYearsBLL> FinancialYearsBLLList = new FinancialYearsBLL().GetFinancialYears();
            //if (new Globals.Calendar().IsGreg(FinancialYearsBLLList.FirstOrDefault().FinancialYearStartDate.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString())))
            //{
            //    FinancialYearsBLLList.ForEach(x => x.FinancialYearStartDate = Convert.ToDateTime(Globals.Calendar.GregToUmAlQura(x.FinancialYearStartDate)));
            //    FinancialYearsBLLList.ForEach(x => x.FinancialYearEndDate = Convert.ToDateTime(Globals.Calendar.GregToUmAlQura(x.FinancialYearEndDate)));
            //}

            //FinancialYearsBLL FinancialYearBLL = FinancialYearsBLLList.Where(x => this.DelegationStartDate >= x.FinancialYearStartDate && this.DelegationEndDate <= x.FinancialYearEndDate).FirstOrDefault();
            //if (FinancialYearBLL == null)
            //{
            //    FinancialYearBLL = FinancialYearsBLLList.FirstOrDefault(x => x.FinancialYearStartDate <= this.DelegationStartDate && x.FinancialYearEndDate >= this.DelegationStartDate);
            //    result.EnumType = typeof(DelegationsValidationEnum);
            //    result.EnumMember = DelegationsValidationEnum.RejectedBecauseOfDelegationDatesMustBeInSameFinancialYear.ToString();
            //    result.Entity = FinancialYearBLL;
            //    return result;
            //}

            result = ExpensesBLL.IsValidExpenseActionInSameFinancialYear(this.DelegationStartDate, this.DelegationEndDate, new BaseDelegationsBLL());
            if (result != null)
                return result;
            #endregion

            #region validate for Conflict With Other Process
            result = CommonHelper.IsNoConflictWithOtherProcess(this.EmployeeCode.EmployeeCodeID, this.DelegationStartDate, this.DelegationEndDate);
            if (result != null)
                return result;
            else
                result = new Result();
            #endregion

            #region Validation for balance
            /*  As per business no need to validate the balance if the kind is not tasks related delegation    */
            result = new Result();
            if (DelegationKindID != (int)DelegationsKindsEnum.Tasks)
            {
                
                result.EnumType = typeof(DelegationsValidationEnum);
                result.EnumMember = DelegationsValidationEnum.Done.ToString();
                return result;
            }
            else
            {
                BaseDelegationsBLL dd = new BaseDelegationsBLL() { DelegationStartDate = this.DelegationStartDate, DelegationEndDate = this.DelegationEndDate };
                //bool IsValid = DelegationRemainingBalance >= DelegationEndDate.Subtract(DelegationStartDate).Days ? true : false;

                bool IsValid = DelegationRemainingBalance >= dd.DelegationPeriod ? true : false;
                if (!IsValid)
                {
                    result.EnumType = typeof(DelegationsValidationEnum);
                    result.EnumMember = DelegationsValidationEnum.RejectedBecauseOfMaxLimit.ToString();
                }
                else
                {
                    result.EnumType = typeof(DelegationsValidationEnum);
                    result.EnumMember = DelegationsValidationEnum.Done.ToString();
                }

                return result;
            }
            #endregion
        }

        public List<EmployeeDelegationBLL> GetEmployeeDelegationsBalanceForYears(int EmployeeCodeID)
        {
            try
            {
                EmployeesCodesBLL EmployeeCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(EmployeeCodeID); // check if employeecodeid exist or not
                List<FinancialYearsBLL> FinancialYearsBLLList = new FinancialYearsBLL().GetFinancialYears();
                List<EmployeeDelegationBLL> EmployeeDelegationBLLList = new List<EmployeeDelegationBLL>();
                if (FinancialYearsBLLList != null)
                {
                    if (EmployeeCodeBLL != null)
                    {
                        foreach (var item in FinancialYearsBLLList)
                        {
                            EmployeeDelegationBLLList.Add(new EmployeeDelegationBLL(item.FinancialYearStartDate,
                                                                                    item.FinancialYearEndDate,
                                                                                    (int)DelegationsKindsEnum.Tasks,
                                                                                    this.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID }));
                        }
                    }
                }

               return EmployeeDelegationBLLList.OrderBy(x => x.DelegationFiscalYear).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
