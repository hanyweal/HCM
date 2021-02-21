using HCMBLL.Enums;
using HCMDAL;
using System;

namespace HCMBLL
{
    public abstract class ExpensesBLL : CommonEntity, IEntity
    {
        public bool IsApproved { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public virtual Result Approve(int id)
        {
            Result result = null;
            if (this.GetType() == typeof(OverTimesBLL))
            {
                OverTimesBLL overTime = new OverTimesBLL().GetByOverTimeID(id);
                overTime.ApprovedBy = this.ApprovedBy;
                result = ApproveOverTime(overTime, true);
            }
            if (this.GetType() == typeof(BaseDelegationsBLL))
            {
                BaseDelegationsBLL delegation = new BaseDelegationsBLL().GetByDelegationID(id);
                delegation.ApprovedBy = this.ApprovedBy;
                result = ApproveDelegation(delegation, true);
            }
            return result;
        }

        public virtual Result ApproveCancel(int id)
        {
            Result result = null;
            if (this.GetType() == typeof(OverTimesBLL))
            {
                OverTimesBLL overTime = new OverTimesBLL().GetByOverTimeID(id);
                overTime.ApprovedBy = this.ApprovedBy;
                result = ApproveOverTime(overTime, false);
            }
            if (this.GetType() == typeof(BaseDelegationsBLL))
            {
                BaseDelegationsBLL delegation = new BaseDelegationsBLL().GetByDelegationID(id);
                delegation.ApprovedBy = this.ApprovedBy;
                result = ApproveDelegation(delegation, false);
            }
            return result;
        }

        private Result ApproveDelegation(BaseDelegationsBLL BaseDelegationBll, bool IsApprove)
        {
            try
            {
                Result result = new Result();
                // Delegations delegation = new DelegationsDAL().GetByDelegationID(BaseDelegationBll.DelegationID);
                if (BaseDelegationBll.IsApproved && IsApprove)
                {
                    result.Entity = BaseDelegationBll;
                    result.EnumType = typeof(DelegationsValidationEnum);
                    result.EnumMember = DelegationsValidationEnum.RejectedBecauseOfAlreadyApprove.ToString();
                    return result;
                }
                if (!BaseDelegationBll.IsApproved && !IsApprove)
                {
                    result.Entity = this;
                    result.EnumType = typeof(DelegationsValidationEnum);
                    result.EnumMember = DelegationsValidationEnum.RejectedBecauseOfAlreadyApproveCancel.ToString();
                    return result;
                }

                Delegations delegation = new Delegations();
                delegation.IsApproved = IsApprove;
                delegation.DelegationID = BaseDelegationBll.DelegationID;
                delegation.DelegationStartDate = BaseDelegationBll.DelegationStartDate;
                if (IsApprove)
                {
                    delegation.ApprovedBy = BaseDelegationBll.ApprovedBy;
                    delegation.ApprovedDate = DateTime.Now;
                    BaseDelegationBll.DelegationsDetails = new DelegationsDetailsBLL().GetDelegationsDetailsByDelegationID(BaseDelegationBll.DelegationID);
                    delegation.LastUpdatedBy = BaseDelegationBll.ApprovedBy;
                }
                else
                {
                    BaseDelegationBll.DelegationsDetails = new DelegationsDetailsBLL().GetDelegationsDetailsByDelegationID(BaseDelegationBll.DelegationID);
                    delegation.LastUpdatedBy = BaseDelegationBll.ApprovedBy;
                    delegation.ApprovedBy = null;
                    delegation.ApprovedDate = null;
                }
                new DelegationsDAL().Approve(delegation);
                //send SMS message.
                SendSMS(BaseDelegationBll);
                if (BaseDelegationBll.DelegationID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(DelegationsValidationEnum);
                    result.EnumMember = DelegationsValidationEnum.Done.ToString();
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        private Result ApproveOverTime(OverTimesBLL OverTimeBll, bool IsApprove)
        {
            try
            {
                Result result = new Result();
                //OverTimes overTime = new OverTimesDAL().GetByOverTimeID(OverTimeBll.OverTimeID);
                //
                if (OverTimeBll.IsApproved && IsApprove)
                {
                    result.Entity = OverTimeBll;
                    result.EnumType = typeof(OverTimeValidationEnum);
                    result.EnumMember = OverTimeValidationEnum.RejectedBecauseOfAlreadyApprove.ToString();
                    return result;
                }
                //
                if (!OverTimeBll.IsApproved && !IsApprove)
                {
                    result.Entity = this;
                    result.EnumType = typeof(OverTimeValidationEnum);
                    result.EnumMember = OverTimeValidationEnum.RejectedBecauseOfAlreadyApproveCancel.ToString();
                    return result;
                }
                OverTimes overTime = new OverTimes();
                overTime.IsApproved = IsApprove;
                overTime.OverTimeID = OverTimeBll.OverTimeID;
                overTime.OverTimeStartDate = OverTimeBll.OverTimeStartDate;
                if (IsApprove)
                {
                    overTime.ApprovedBy = OverTimeBll.ApprovedBy;
                    overTime.ApprovedDate = DateTime.Now;
                    overTime.LastUpdatedBy = OverTimeBll.ApprovedBy;
                    OverTimeBll.OverTimesDetails = new OverTimesDetailsBLL().GetOverTimeDetailsByOverTimeID(OverTimeBll.OverTimeID); //overTime.OverTimesDetails.Select(x => new OverTimesDetailsBLL().MapOverTimeDetail(x)).ToList();// new DelegationsDetailsBLL().MapDelegationDetail(Delegation.DelegationsDetails);
                }
                else
                {
                    OverTimeBll.OverTimesDetails = new OverTimesDetailsBLL().GetOverTimeDetailsByOverTimeID(OverTimeBll.OverTimeID);// new DelegationsDetailsBLL().MapDelegationDetail(Delegation.DelegationsDetails);
                    overTime.LastUpdatedBy = OverTimeBll.ApprovedBy;
                    overTime.ApprovedBy = null;
                    overTime.ApprovedDate = null;

                }
                new OverTimesDAL().Approve(overTime);
                //send SMS message.
                SendSMS(OverTimeBll);
                if (OverTimeBll.OverTimeID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(OverTimeValidationEnum);
                    result.EnumMember = OverTimeValidationEnum.Done.ToString();
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        private void SendSMS(ExpensesBLL Expense)
        {
            SMSLogsBLL smslog = new SMSLogsBLL()
            {
                CreatedDate = DateTime.Now,
                CreatedBy = new EmployeesCodesBLL() { EmployeeCodeID = Expense.ApprovedBy.Value }
            };


            if (this.GetType() == typeof(BaseDelegationsBLL))
            {
                BaseDelegationsBLL Delegation = (BaseDelegationsBLL)Expense;
                smslog.DetailID = Delegation.DelegationID;
                smslog.BusinssSubCategory = BusinessSubCategoriesEnum.Delegations;
                if (!Expense.IsApproved)
                {
                    smslog.Message = string.Format(Globalization.SMSDelegationApprovalMessageText, Delegation.DelegationStartDate.ToShortDateString(), Delegation.DelegationPeriod);
                }
                else if (Expense.IsApproved)
                {
                    smslog.Message = string.Format(Globalization.SMSDelegationApprovalCancelMessageText, Delegation.DelegationStartDate.ToShortDateString(), Delegation.DelegationPeriod);
                }
                foreach (DelegationsDetailsBLL dd in Delegation.DelegationsDetails)
                {
                    smslog.MobileNo = dd.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeMobileNo;
                    new SMSBLL().SendSMS(smslog);
                }
            }

            if (this.GetType() == typeof(OverTimesBLL))
            {
                OverTimesBLL OverTime = (OverTimesBLL)Expense;
                smslog.DetailID = OverTime.OverTimeID;
                smslog.BusinssSubCategory = BusinessSubCategoriesEnum.OverTimes;
                if (!Expense.IsApproved)
                {
                    smslog.Message = string.Format(Globalization.SMSOverTimeApprovalMessageText, OverTime.OverTimeStartDate.ToShortDateString(), OverTime.OverTimePeriod);
                }
                else if (Expense.IsApproved)
                {
                    smslog.Message = string.Format(Globalization.SMSOverTimeApprovalCancelMessageText, OverTime.OverTimeStartDate.ToShortDateString(), OverTime.OverTimePeriod);
                }
                foreach (OverTimesDetailsBLL dd in OverTime.OverTimesDetails)
                {
                    smslog.MobileNo = dd.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeMobileNo;
                    new SMSBLL().SendSMS(smslog);
                }
            }
        }



        public static Result IsValidExpenseActionInSameFinancialYear(DateTime StartDate, DateTime EndDate, ExpensesBLL Expense)
        {
            Result result = null;
            ///// this validation was stoped on 20/02/2020 with task no #93

            //List<FinancialYearsBLL> FinancialYearsBLLList = new FinancialYearsBLL().GetFinancialYears();
            //if (new Globals.Calendar().IsGreg(FinancialYearsBLLList.FirstOrDefault().FinancialYearStartDate.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString())))
            //{
            //    FinancialYearsBLLList.ForEach(x => x.FinancialYearStartDate = Convert.ToDateTime(Globals.Calendar.GregToUmAlQura(x.FinancialYearStartDate)));
            //    FinancialYearsBLLList.ForEach(x => x.FinancialYearEndDate = Convert.ToDateTime(Globals.Calendar.GregToUmAlQura(x.FinancialYearEndDate)));
            //}

            //FinancialYearsBLL FinancialYearBLL = FinancialYearsBLLList.Where(x => StartDate >= x.FinancialYearStartDate && EndDate <= x.FinancialYearEndDate).FirstOrDefault();
            //if (FinancialYearBLL == null)
            //{
            //    result = new Result();
            //    FinancialYearBLL = FinancialYearsBLLList.FirstOrDefault(x => x.FinancialYearStartDate <= StartDate && x.FinancialYearEndDate >= StartDate); // to get the start date and end date of the fainancial year
            //    result.Entity = FinancialYearBLL;
            //    if (Expense is OverTimesBLL)
            //    {
            //        result.EnumType = typeof(OverTimeValidationEnum);
            //        result.EnumMember = OverTimeValidationEnum.RejectedBecauseOfOverTimeDatesMustBeInSameFinancialYear.ToString();
            //    }
            //    else if (Expense is BaseDelegationsBLL)
            //    {
            //        result.EnumType = typeof(DelegationsValidationEnum);
            //        result.EnumMember = DelegationsValidationEnum.RejectedBecauseOfDelegationDatesMustBeInSameFinancialYear.ToString();
            //    }

            //    return result;
            //}

            return result;
        }
    }
}