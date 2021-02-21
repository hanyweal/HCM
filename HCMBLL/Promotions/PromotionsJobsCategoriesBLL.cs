using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;

namespace HCMBLL
{
    public class PromotionsJobsCategoriesBLL : CommonEntity, IEntity
    {
        public int PromotionJobCategoryID { get; set; }

        public PromotionsPeriodsBLL PromotionPeriod { get; set; }

        public JobsCategoriesBLL JobCategory { get; set; }

        public JobsCategoriesBLL AssignedJobCategory { get; set; }

        public virtual Result Add()
        {
            Result result = null;
            #region Check AssignedJobCategory same JobCategory
            if (this.AssignedJobCategory.JobCategoryID == this.JobCategory.JobCategoryID)
            {
                result = new Result();
                result.Entity = this;
                result.EnumMember = PromotionsJobsCategoriesValidationEnum.RejectedBecauseAssignedJobCategorySameJobCategory.ToString();
                result.EnumType = typeof(PromotionsJobsCategoriesValidationEnum);
                return result;
            }
            #endregion

            #region Check if JobCategory It has Promotion Record
            if (new PromotionsRecordsBLL().GetByJobCategory(this.JobCategory.JobCategoryID, this.PromotionPeriod.PromotionPeriodID).Count > 0)
            {
                result = new Result();
                result.Entity = this;
                result.EnumMember = JobsCategoriesValidationEnum.RejectedBecauseOfItHasPromotionRecord.ToString();
                result.EnumType = typeof(JobsCategoriesValidationEnum);
                return result;
            }
            #endregion

            result = new Result();
            PromotionsJobsCategories PromotionJobCategory = new PromotionsJobsCategories();
            PromotionJobCategory.JobCategoryID = this.JobCategory.JobCategoryID;
            PromotionJobCategory.PromotionPeriodID = this.PromotionPeriod.PromotionPeriodID;
            PromotionJobCategory.AssignedJobCategoryID = this.AssignedJobCategory.JobCategoryID;
            PromotionJobCategory.CreatedDate = DateTime.Now;
            PromotionJobCategory.CreatedBy = this.LoginIdentity.EmployeeCodeID;
            this.PromotionJobCategoryID = new PromotionsJobsCategoriesDAL().Insert(PromotionJobCategory);
            if (this.PromotionJobCategoryID != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(LookupsValidationEnum);
                result.EnumMember = LookupsValidationEnum.Done.ToString();
            }

            return result;
        }

        public virtual Result Remove(int PromotionJobCategoryID)
        {
            try
            {
                Result result = null;
                #region Check if JobCategory It has Promotion Record
                PromotionsJobsCategoriesBLL PromotionJobCategory = new PromotionsJobsCategoriesBLL().GetPromotionsJobsCategoriesByPromotionJobCategoryID(PromotionJobCategoryID);
                if (PromotionJobCategory != null)
                {
                    if (new PromotionsRecordsBLL().GetByJobCategory(PromotionJobCategory.JobCategory.JobCategoryID, PromotionJobCategory.PromotionPeriod.PromotionPeriodID).Count > 0)
                    {
                        result = new Result();
                        result.Entity = this;
                        result.EnumMember = JobsCategoriesValidationEnum.RejectedBecauseOfItHasPromotionRecord.ToString();
                        result.EnumType = typeof(JobsCategoriesValidationEnum);
                        return result;
                    }
                }
                #endregion

                new PromotionsJobsCategoriesDAL().Delete(PromotionJobCategoryID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(LookupsValidationEnum),
                    EnumMember = LookupsValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        public PromotionsJobsCategoriesBLL GetPromotionsJobsCategoriesByPromotionJobCategoryID(int PromotionJobCategoryID)
        {
            try
            {
                PromotionsJobsCategories PromotionJobCategory = new PromotionsJobsCategoriesDAL().GetPromotionJobCategoryByPromotionJobCategoryID(PromotionJobCategoryID);
                List<PromotionsJobsCategoriesBLL> PromotionsJobsCategoriesBLList = new List<PromotionsJobsCategoriesBLL>();
                if (PromotionJobCategory != null)
                {
                    return new PromotionsJobsCategoriesBLL().MapPromotionJobCategory(PromotionJobCategory);
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        public List<JobsCategoriesBLL> GetJobsCategoriesToIncludeInPromotionRecord(int PromotionPeriodID, int JobCategoryID)
        {
            try
            {
                List<PromotionsJobsCategories> PromotionsPeriodsList = new PromotionsJobsCategoriesDAL().GetByJobCategoryIDPromotionPeriodID(PromotionPeriodID, JobCategoryID);
                List<JobsCategoriesBLL> JobsCategoriesBLLList = new List<JobsCategoriesBLL>();
                JobsCategoriesBLLList.Add(new JobsCategoriesBLL().GetByJobCategoryID(JobCategoryID));
                foreach (var item in PromotionsPeriodsList)
                {
                    JobsCategoriesBLLList.Add(new JobsCategoriesBLL().MapJobCategory(item.AssignedJobsCategories));
                }
                return JobsCategoriesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsJobsCategoriesBLL> GetAssignedJobCategoriesByJobCategoryID(int JobCategoryID)
        {
            try
            {
                List<PromotionsJobsCategories> AssignedJobCategoryList = new PromotionsJobsCategoriesDAL().GetAssignedJobCategoriesByJobCategoryID(JobCategoryID);
                List<PromotionsJobsCategoriesBLL> AssignedJobCategoryBLLList = new List<PromotionsJobsCategoriesBLL>();

                foreach (var item in AssignedJobCategoryList)
                {
                    AssignedJobCategoryBLLList.Add(new PromotionsJobsCategoriesBLL().MapPromotionJobCategory(item));
                }
                return AssignedJobCategoryBLLList;
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsJobsCategoriesBLL> GetMainJobCategoriesByAssignedJobCategoryID(int JobCategoryID)
        {
            try
            {
                List<PromotionsJobsCategories> MainJobCategoryList = new PromotionsJobsCategoriesDAL().GetMainJobCategoriesByAssignedJobCategoryID(JobCategoryID);
                List<PromotionsJobsCategoriesBLL> MainJobCategoryBLLList = new List<PromotionsJobsCategoriesBLL>();

                foreach (var item in MainJobCategoryList)
                {
                    MainJobCategoryBLLList.Add(new PromotionsJobsCategoriesBLL().MapPromotionJobCategory(item));
                }
                return MainJobCategoryBLLList;
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsJobsCategoriesBLL> GetPromotionsJobsCategoriesByJobCategoryID(int JobCategoryID)
        {
            try
            {
                List<PromotionsJobsCategories> AssignedJobCategoryList = new PromotionsJobsCategoriesDAL().GetAssignedJobCategoriesByJobCategoryID(JobCategoryID);
                List<PromotionsJobsCategoriesBLL> PromotionsJobsCategoriesBLList = new List<PromotionsJobsCategoriesBLL>();
                if (AssignedJobCategoryList.Count > 0)
                {
                    foreach (var item in AssignedJobCategoryList)
                    {
                        PromotionsJobsCategoriesBLList.Add(new PromotionsJobsCategoriesBLL().MapPromotionJobCategory(item));
                    }
                }

                return PromotionsJobsCategoriesBLList;
            }
            catch
            {
                throw;
            }
        }

        public PromotionsJobsCategoriesBLL MapPromotionJobCategory(PromotionsJobsCategories PromotionJobCategory)
        {
            try
            {
                PromotionsJobsCategoriesBLL PromotionsJobsCategoriesBLL = null;
                if (PromotionJobCategory != null)
                {
                    PromotionsJobsCategoriesBLL = new PromotionsJobsCategoriesBLL()
                    {
                        PromotionJobCategoryID = PromotionJobCategory.PromotionJobCategoryID,
                    };
                    if (PromotionJobCategory.JobsCategories != null)
                    {
                        PromotionsJobsCategoriesBLL.JobCategory = new JobsCategoriesBLL().MapJobCategory(PromotionJobCategory.JobsCategories);
                    }
                    if (PromotionJobCategory.PromotionsPeriods != null)
                    {
                        PromotionsJobsCategoriesBLL.PromotionPeriod = new PromotionsPeriodsBLL().MapPromotionPeriod(PromotionJobCategory.PromotionsPeriods);
                    }
                    else
                    {
                        PromotionsJobsCategoriesBLL.PromotionPeriod = new PromotionsPeriodsBLL();
                    }
                    if (PromotionJobCategory.AssignedJobsCategories != null)
                    {
                        PromotionsJobsCategoriesBLL.AssignedJobCategory = new JobsCategoriesBLL().MapJobCategory(PromotionJobCategory.AssignedJobsCategories);
                    }
                    else
                    {
                        PromotionsJobsCategoriesBLL.AssignedJobCategory = new JobsCategoriesBLL();
                    }
                }
                return PromotionsJobsCategoriesBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}