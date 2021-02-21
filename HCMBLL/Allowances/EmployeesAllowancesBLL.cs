using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCMBLL
{
    public class EmployeesAllowancesBLL : CommonEntity, IEntity
    {
        public int EmployeeAllowanceID { get; set; }

        public AllowancesBLL Allowance { get; set; }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public DateTime AllowanceStartDate { get; set; }

        public bool IsActive { get; set; }

        public virtual Result Add()
        {
            Result result = new Result();
            result.EnumType = typeof(AllowanceValidationEnum);
            EmployeesCareersHistoryBLL EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(this.EmployeeCareerHistory.EmployeeCareerHistoryID);

            //if ((new JobsAllowancesBLL()).IsAllowedAllowanceForJob(EmployeeCareerHistory.OrganizationJob.Job.JobID, this.Allowance.AllowanceID) == AllowanceAllowEnum.NotAllowedBecauseOfNotAllowedForJob)          // not in jobAllowance table
            //{
            //   /result.EnumMember = AllowanceValidationEnum.RejectedBecauseOfNotAllowedForJob.ToString();
            //}
            // else if ((new JobsAllowancesBLL()).IsJobAllowanceActive(EmployeeCareerHistory.OrganizationJob.Job.JobID, this.Allowance.AllowanceID) == AllowanceAllowEnum.NotAllowedBecauseOfStopedForJob)         // job inactive
            //{
            //    result.EnumMember = AllowanceValidationEnum.RejectedBecauseOfStopedForJob.ToString();
            //}

            if ((new AllowancesBLL()).IsAllowanceActive(this.Allowance.AllowanceID) == AllowanceAllowEnum.NotAllowedBecauseOfStoped)               // allowance inactive
            {
                result.EnumMember = AllowanceValidationEnum.RejectedBecauseOfStoped.ToString();
            }

            else
            {
                result.Entity = this;
                result.EnumMember = AllowanceValidationEnum.Done.ToString();
            }

            if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
            {
                EmployeesAllowances EmployeeAllowance = new EmployeesAllowances();
                EmployeeAllowance.AllowanceID = this.Allowance.AllowanceID;
                EmployeeAllowance.EmployeeCareerHistoryID = this.EmployeeCareerHistory.EmployeeCareerHistoryID;
                EmployeeAllowance.AllowanceStartDate = this.AllowanceStartDate;
                EmployeeAllowance.IsActive = true;
                EmployeeAllowance.CreatedDate = DateTime.Now;
                EmployeeAllowance.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                this.EmployeeAllowanceID = new EmployeesAllowancesDAL().Insert(EmployeeAllowance);
                if (this.EmployeeAllowanceID != 0)
                {
                    result.Entity = this;
                    result.EnumMember = AllowanceValidationEnum.Done.ToString();
                }
            }

            return result;
        }

        public virtual Result Update()
        {
            Result result = new Result();
            result.EnumType = typeof(AllowanceValidationEnum);
            EmployeesAllowances EmployeeAllowance = new EmployeesAllowances();
            EmployeesCareersHistoryBLL EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(this.EmployeeCareerHistory.EmployeeCareerHistoryID);

            //if ((new JobsAllowancesBLL()).IsAllowedAllowanceForJob(EmployeeCareerHistory.OrganizationJob.Job.JobID, this.Allowance.AllowanceID) == AllowanceAllowEnum.NotAllowedBecauseOfNotAllowedForJob)          // not in jobAllowance table
            //{
            //    result.EnumMember = AllowanceValidationEnum.RejectedBecauseOfNotAllowedForJob.ToString();
            //}

            if ((new AllowancesBLL()).IsAllowanceActive(this.Allowance.AllowanceID) == AllowanceAllowEnum.NotAllowedBecauseOfStoped)               // allowance inactive
            {
                result.EnumMember = AllowanceValidationEnum.RejectedBecauseOfStoped.ToString();
            }

            //else if ((new JobsAllowancesBLL()).IsJobAllowanceActive(EmployeeCareerHistory.OrganizationJob.Job.JobID, this.Allowance.AllowanceID) == AllowanceAllowEnum.NotAllowedBecauseOfStopedForJob)         // job inactive
            //{
            //    result.EnumMember = AllowanceValidationEnum.RejectedBecauseOfStopedForJob.ToString();
            //}

            else
            {
                result.Entity = this;
                result.EnumMember = AllowanceValidationEnum.Done.ToString();
            }


            if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
            {
                EmployeeAllowance.EmployeeAllowanceID = this.EmployeeAllowanceID;
                EmployeeAllowance.AllowanceID = this.Allowance.AllowanceID;
                EmployeeAllowance.EmployeeCareerHistoryID = this.EmployeeCareerHistory.EmployeeCareerHistoryID;
                EmployeeAllowance.AllowanceStartDate = this.AllowanceStartDate;
                EmployeeAllowance.IsActive = this.IsActive;
                EmployeeAllowance.LastUpdatedDate = DateTime.Now;
                EmployeeAllowance.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                this.EmployeeAllowanceID = new EmployeesAllowancesDAL().Update(EmployeeAllowance);
                if (this.EmployeeAllowanceID != 0)
                {
                    result.Entity = this;
                    result.EnumMember = AllowanceValidationEnum.Done.ToString();
                }
            }

            return result;
        }

        public Result Remove(int EmployeeAllowanceID)
        {
            try
            {
                Result result = null;
                new EmployeesAllowancesDAL().Delete(EmployeeAllowanceID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(AllowanceValidationEnum),
                    EnumMember = AllowanceValidationEnum.Done.ToString()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<EmployeesAllowancesBLL> GetEmployeesAllowances(out int totalRecordsOut, out int recFilterOut)
        {
            List<EmployeesAllowancesBLL> EmployeesAllowancesBLL = new List<EmployeesAllowancesBLL>();
            List<EmployeesAllowances> EmployeesAllowances = new EmployeesAllowancesDAL()
            {
                search = Search,
                order = Order,
                orderDir = OrderDir,
                startRec = StartRec,
                pageSize = PageSize

            }.GetEmployeesAllowances(out totalRecordsOut, out recFilterOut);
            foreach (var item in EmployeesAllowances)
                EmployeesAllowancesBLL.Add(MapEmployeeAllowance(item));

            return EmployeesAllowancesBLL;
        }

        public virtual List<EmployeesAllowancesBLL> GetEmployeesAllowances()
        {
            List<EmployeesAllowancesBLL> EmployeesAllowancesBLL = new List<EmployeesAllowancesBLL>();
            List<EmployeesAllowances> EmployeesAllowances = new EmployeesAllowancesDAL().GetActiveEmployeesAllowances();
            foreach (var item in EmployeesAllowances)
                EmployeesAllowancesBLL.Add(MapEmployeeAllowance(item));

            return EmployeesAllowancesBLL;
        }

        public virtual List<EmployeesAllowancesBLL> GetEmployeesAllowances(string EmployeeCodeNo)
        {
            List<EmployeesAllowancesBLL> EmployeesAllowancesBLL = new List<EmployeesAllowancesBLL>();
            List<EmployeesAllowances> EmployeesAllowances = new EmployeesAllowancesDAL().GetActiveEmployeesAllowances(EmployeeCodeNo);
            foreach (var item in EmployeesAllowances)
                EmployeesAllowancesBLL.Add(MapEmployeeAllowance(item));

            return EmployeesAllowancesBLL;
        }

        public EmployeesAllowancesBLL GetByEmployeeAllowanceID(int EmployeeAllowanceID)
        {
            EmployeesAllowancesBLL EmployeesAllowancesBLL = null;
            EmployeesAllowances EmployeesAllowance = new EmployeesAllowancesDAL().GetEmployeesAllowancesByAllowanceID(EmployeeAllowanceID);
            if (EmployeesAllowance != null)
                EmployeesAllowancesBLL = MapEmployeeAllowance(EmployeesAllowance);

            return EmployeesAllowancesBLL;
        }

        internal EmployeesAllowancesBLL MapEmployeeAllowance(EmployeesAllowances EmployeeAllowance)
        {
            try
            {
                EmployeesAllowancesBLL EmployeeAllowanceBLL = null;
                if (EmployeeAllowance != null)
                {
                    EmployeeAllowanceBLL = new EmployeesAllowancesBLL()
                    {
                        EmployeeAllowanceID = EmployeeAllowance.EmployeeAllowanceID,
                        Allowance = new AllowancesBLL().MapAllowance(EmployeeAllowance.Allowances),
                        EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(EmployeeAllowance.EmployeesCareersHistory),
                        AllowanceStartDate = EmployeeAllowance.AllowanceStartDate.Date,
                        IsActive = EmployeeAllowance.IsActive,
                        CreatedDate = EmployeeAllowance.CreatedDate
                    };
                }
                return EmployeeAllowanceBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}