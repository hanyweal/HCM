using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class OverTimesDetailsBLL : CommonEntity, IEntity
    {
        public int OverTimeDetailID { get; set; }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public OverTimesBLL OverTime { get; set; }

        public List<DateTime> VacationDays { get; set; }

        public Result Add(OverTimesDetailsBLL OverTimeDetailBLL)
        {
            Result result = new Result();

            OverTimesDetails OverTimeDetail = new OverTimesDetails()
            {
                OverTimeID = OverTimeDetailBLL.OverTime.OverTimeID,
                EmployeeCareerHistoryID = OverTimeDetailBLL.EmployeeCareerHistory.EmployeeCareerHistoryID,
                CreateDate = DateTime.Now,
                CreatedBy = OverTimeDetailBLL.LoginIdentity.EmployeeCodeID
            };
            this.OverTimeDetailID = new OverTimesDetailsDAL().Insert(OverTimeDetail);
            if (this.OverTimeDetailID != 0)
            {
                result.Entity = null;
                result.EnumType = typeof(OverTimeValidationEnum);
                result.EnumMember = OverTimeValidationEnum.Done.ToString();
            }
            return result;
        }

        public Result Remove(int OverTimeDetailID)
        {
            try
            {
                Result result = null;
                new OverTimesDetailsDAL().Delete(OverTimeDetailID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(OverTimeValidationEnum),
                    EnumMember = OverTimeValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        public List<OverTimesDetailsBLL> GetOverTimeDetailsByOverTimeID(int id)
        {
            List<OverTimesDetails> OverTimesDetailsList = new OverTimesDetailsDAL().GetOverTimesDetailsByOverTimeID(id);
            List<OverTimesDetailsBLL> OverTimesDetailsBLLList = new List<OverTimesDetailsBLL>();
            if (OverTimesDetailsList.Count > 0)
            {
                foreach (var item in OverTimesDetailsList)
                {
                    OverTimesDetailsBLLList.Add(new OverTimesDetailsBLL()
                    {
                        OverTimeDetailID = item.OverTimeDetailID,
                        EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item.EmployeesCareersHistory),
                    });
                }
            }

            return OverTimesDetailsBLLList;
        }

        public OverTimesDetailsBLL GetOverTimeDetailsByID(int id)
        {
            OverTimesDetails OverTimeDetail = new OverTimesDetailsDAL().GetOverTimeDetailsByID(id);
            return MapOverTimeDetail(OverTimeDetail);
        }

        public Result IsValid()
        {
            Result result;

            //#region Validation for OverTime Dates
            //FinancialYearsBLL FinancialYearBLL = new FinancialYearsBLL().GetByFinancialYear(this.OverTimes.OverTimeFiscalYear);
            //if (FinancialYearBLL != null)
            //{
            //    if (this.OverTimes.OverTimeStartDate < FinancialYearBLL.FinancialYearStartDate && this.OverTimes.OverTimeEndDate < FinancialYearBLL.FinancialYearStartDate)
            //    {
            //        FinancialYearBLL = new FinancialYearsBLL().GetByFinancialYear(this.OverTimes.OverTimeFiscalYear - 1);
            //    }
            //    if (this.OverTimes.OverTimeStartDate < FinancialYearBLL.FinancialYearStartDate || this.OverTimes.OverTimeEndDate > FinancialYearBLL.FinancialYearEndDate)
            //    {
            //        result.EnumType = typeof(OverTimeValidationEnum);
            //        result.EnumMember = OverTimeValidationEnum.RejectedBecauseOfOverTimeDatesMustBeInSameFinancialYear.ToString();
            //        result.Entity = FinancialYearBLL;
            //        return result;
            //    }
            //}
            //#endregion


            #region Validation for OverTimes Dates
            //CultureInfo cul = Thread.CurrentThread.CurrentCulture;
            //List<FinancialYearsBLL> FinancialYearsBLLList = new FinancialYearsBLL().GetFinancialYears();
            //if (new Globals.Calendar().IsGreg(FinancialYearsBLLList.FirstOrDefault().FinancialYearStartDate.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString())))
            //{
            //    FinancialYearsBLLList.ForEach(x => x.FinancialYearStartDate = Convert.ToDateTime(Globals.Calendar.GregToUmAlQura(x.FinancialYearStartDate)));
            //    FinancialYearsBLLList.ForEach(x => x.FinancialYearEndDate = Convert.ToDateTime(Globals.Calendar.GregToUmAlQura(x.FinancialYearEndDate)));
            //}

            //FinancialYearsBLL FinancialYearBLL = FinancialYearsBLLList.Where(x => this.OverTime.OverTimeStartDate >= x.FinancialYearStartDate && this.OverTime.OverTimeEndDate <= x.FinancialYearEndDate).FirstOrDefault();
            //if (FinancialYearBLL == null)
            //{
            //    FinancialYearBLL = FinancialYearsBLLList.FirstOrDefault(x => x.FinancialYearStartDate <= this.OverTime.OverTimeStartDate && x.FinancialYearEndDate >= this.OverTime.OverTimeEndDate);
            //    result.EnumType = typeof(OverTimeValidationEnum);
            //    result.EnumMember = OverTimeValidationEnum.RejectedBecauseOfOverTimeDatesMustBeInSameFinancialYear.ToString();
            //    result.Entity = FinancialYearBLL;
            //    return result;
            //}
            result = ExpensesBLL.IsValidExpenseActionInSameFinancialYear(this.OverTime.OverTimeStartDate, this.OverTime.OverTimeEndDate, new OverTimesBLL());
            if (result != null)
                return result;
            #endregion

            /*  As per business no need to validate Trainings related delegation    */
            #region Validaion for employee Exist
            OverTimesDetails employee = new OverTimesDetailsDAL().GetOverTimesDetailsByOverTimeID(this.OverTime.OverTimeID).Where(d => d.EmployeeCareerHistoryID == this.EmployeeCareerHistory.EmployeeCareerHistoryID).FirstOrDefault();
            if (employee != null)
            {
                result.Entity = null;
                result.EnumType = typeof(OverTimeValidationEnum);
                result.EnumMember = OverTimeValidationEnum.RejectedBecauseAlreadyExist.ToString();

                return result;
            }
            #endregion

            #region validate for Conflict With Other Process
            if (this.OverTime.OverTimesDays == null || this.OverTime.OverTimesDays.Count == 0)
            {
                result = CommonHelper.IsNoConflictWithOtherProcess(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                                                                        this.OverTime.OverTimeStartDate, this.OverTime.OverTimeEndDate, BusinessSubCategoriesEnum.Vacations);
                if (result != null)
                    return result;
            }
            else
            {
                foreach (var item in this.OverTime.OverTimesDays)
                {
                    result = CommonHelper.IsNoConflictWithOtherProcess(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                                                                            item.OverTimeDay, item.OverTimeDay, BusinessSubCategoriesEnum.Vacations);
                    if (result != null)
                        return result;
                }
            }         
            #endregion

            result = new Result();
            result.EnumType = typeof(OverTimeValidationEnum);
            result.EnumMember = OverTimeValidationEnum.Done.ToString();

            return result;
        }

        internal OverTimesDetailsBLL MapOverTimeDetail(OverTimesDetails OverTimeDetail)
        {
            try
            {
                OverTimesDetailsBLL OverTimeDetailBLL = null;
                if (OverTimeDetail != null)
                {
                    OverTimeDetailBLL = new OverTimesDetailsBLL()
                    {
                        OverTimeDetailID = OverTimeDetail.OverTimeDetailID,
                        OverTime = new OverTimesBLL().MapOverTime(OverTimeDetail.OverTimes),
                        EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(OverTimeDetail.EmployeesCareersHistory)
                    };
                }
                return OverTimeDetailBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}