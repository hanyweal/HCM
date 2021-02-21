﻿using HCMBLL.Enums;
using HCMDAL;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCMBLL
{
    public class JobsCategoriesBLL : CommonEntity, IEntity
    {
        public virtual int JobCategoryID
        {
            get;
            set;
        }

        public virtual QualificationsDegreesBLL MinQualificationDegree { get; set; }

        public virtual QualificationsBLL MinQualification { get; set; }

        public virtual GeneralSpecializationsBLL MinGeneralSpecialization { get; set; }

        public virtual string JobCategoryName
        {
            get;
            set;
        }

        public virtual string JobCategoryNo { get; set; }

        public virtual JobsGroupsBLL JobGroup
        {
            get;
            set;
        }

        public virtual List<JobsBLL> Jobs
        {
            get;
            set;
        }

        //public virtual List<PromotionsRecordsBLL> PromotionsRecords
        //{
        //    get;
        //    private set;
        //}

        public virtual Result Add()
        {
            Result result = null;
            result = new Result();
            JobsCategories JobCategory = new JobsCategories();
            JobCategory.JobCategoryName = this.JobCategoryName;
            JobCategory.JobGroupID = this.JobGroup.JobGroupID;
            if (this.MinQualification!=null)
            JobCategory.MinQualificationID = this.MinQualification.QualificationID;
            if (this.MinQualificationDegree != null)
            JobCategory.MinQualificationDegreeID = this.MinQualificationDegree.QualificationDegreeID;
            if (this.MinGeneralSpecialization != null)
            JobCategory.MinGeneralSpecializationID = this.MinGeneralSpecialization.GeneralSpecializationID;
            JobCategory.JobCategoryNo = this.JobCategoryNo;
            JobCategory.CreatedDate = DateTime.Now;
            JobCategory.CreatedBy = this.LoginIdentity.EmployeeCodeID;

            this.JobCategoryID = new JobsCategoriesDAL().Insert(JobCategory);
            if (this.JobCategoryID != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(LookupsValidationEnum);
                result.EnumMember = LookupsValidationEnum.Done.ToString();
            }

            return result;
        }

        public virtual Result Remove(int JobCategoryID)
        {
            try
            {
                Result result = null;
                new JobsCategoriesDAL().Delete(JobCategoryID, this.LoginIdentity.EmployeeCodeID);
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

        public virtual Result Update()
        {
            Result result = new Result();
            #region Check if JobCategory It has Promotion Record
            if (new PromotionsRecordsBLL().GetByJobCategory(this.JobCategoryID).Count>0)
            {
                result = new Result();
                result.Entity = this;
                result.EnumMember = JobsCategoriesValidationEnum.RejectedBecauseOfItHasPromotionRecord.ToString();
                result.EnumType = typeof(JobsCategoriesValidationEnum);
                return result;
            }
            #endregion
            //--== Get Current Object from database to check if JobCategoryNo is Updated by user or not.
            JobsCategories JobCategoryObj = new JobsCategoriesDAL().GetByJobCategoryID(this.JobCategoryID);
            JobsCategories JobCategory = new JobsCategories();
            JobCategory.JobCategoryID = this.JobCategoryID;
            JobCategory.JobCategoryName = this.JobCategoryName;
            JobCategory.JobGroupID = this.JobGroup.JobGroupID;
            if (this.MinQualification != null)
                JobCategory.MinQualificationID = this.MinQualification.QualificationID;
            if (this.MinQualificationDegree != null)
                JobCategory.MinQualificationDegreeID = this.MinQualificationDegree.QualificationDegreeID;
            if (this.MinGeneralSpecialization != null)
                JobCategory.MinGeneralSpecializationID = this.MinGeneralSpecialization.GeneralSpecializationID;
            JobCategory.LastUpdatedDate = DateTime.Now;
            JobCategory.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
            JobCategory.JobCategoryNo = this.JobCategoryNo;
            this.JobCategoryID = new JobsCategoriesDAL().Update(JobCategory);
            if (JobCategoryObj.JobCategoryNo != this.JobCategoryNo)
            {
                foreach (Jobs job in JobCategoryObj.Jobs)
                {
                    JobsBLL JobsBLL= new JobsBLL().MapJob(job);
                    JobsBLL.JobCode = this.JobCategoryNo;
                    JobsBLL.LoginIdentity = this.LoginIdentity;
                    JobsBLL.Update();
                }
            }
            if (this.JobCategoryID != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(LookupsValidationEnum);
                result.EnumMember = LookupsValidationEnum.Done.ToString();
            }
            return result;
        }

        public virtual List<JobsCategoriesBLL> GetJobsCategories()
        {
            List<JobsCategories> JobsCategoriesList = new JobsCategoriesDAL().GetJobsCategories();
            List<JobsCategoriesBLL> JobsCategoriesBLLList = new List<JobsCategoriesBLL>();
            if (JobsCategoriesList.Count > 0)
            {
                foreach (var item in JobsCategoriesList)
                {
                    JobsCategoriesBLLList.Add(new JobsCategoriesBLL().MapJobCategory(item));
                }
            }

            return JobsCategoriesBLLList;
        }

        public virtual List<JobsCategoriesBLL> GetJobsCategories(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                List<JobsCategories> JobsCategoriesList = new JobsCategoriesDAL()
                {
                    search = Search,
                    order = Order,
                    orderDir = OrderDir,
                    startRec = StartRec,
                    pageSize = PageSize
                }.GetJobsCategories(out totalRecordsOut, out recFilterOut).ToList();

                List<JobsCategoriesBLL> JobsCategoriesBLLList = new List<JobsCategoriesBLL>();
                foreach (var item in JobsCategoriesList)
                {
                    JobsCategoriesBLLList.Add(MapJobCategory(item));
                }

                return JobsCategoriesBLLList.ToList();
            }
            catch
            {
                throw;
            }
        }

        public virtual JobsCategoriesBLL GetByJobCategoryID(int JobCategoryID)
        {
            JobsCategories JobCategory = new JobsCategoriesDAL().GetByJobCategoryID(JobCategoryID);
            JobsCategoriesBLL JobsCategoriesBLL = new JobsCategoriesBLL();
            if (JobCategory != null)
            {
                if (JobCategory.JobCategoryID > 0)
                {
                    return new JobsCategoriesBLL().MapJobCategory(JobCategory);
                }
            }
            return null;
        }

        public virtual List<JobsCategoriesBLL> GetByJobGroupID(int JobGroupID)
        {
            List<JobsCategories> JobsCategoriesList = new JobsCategoriesDAL().GetByJobGroupID(JobGroupID);
            List<JobsCategoriesBLL> JobsCategoriesBLLList = new List<JobsCategoriesBLL>();
            if (JobsCategoriesList.Count > 0)
            {
                foreach (var item in JobsCategoriesList)
                {
                    JobsCategoriesBLLList.Add(new JobsCategoriesBLL().MapJobCategory(item));
                }
            }

            return JobsCategoriesBLLList;
        }

        public JobsCategoriesBLL MapJobCategory(JobsCategories JobCategory)
        {
            try
            {
                JobsCategoriesBLL JobCategoryBLL = null;
                if (JobCategory != null)
                {
                    JobCategoryBLL = new JobsCategoriesBLL()
                    {
                        JobCategoryID = JobCategory.JobCategoryID,
                        JobCategoryName = JobCategory.JobCategoryName,
                        JobCategoryNo = JobCategory.JobCategoryNo
                    };
                    if (JobCategory.JobsGroups != null)
                    {
                        JobCategoryBLL.JobGroup = new JobsGroupsBLL().MapJobGroup(JobCategory.JobsGroups);
                    }
                    if (JobCategory.Qualifications != null)
                    {
                        JobCategoryBLL.MinQualification = new QualificationsBLL().MapQualification(JobCategory.Qualifications);
                    }
                    if (JobCategory.GeneralSpecializations != null)
                    {
                        JobCategoryBLL.MinGeneralSpecialization = new GeneralSpecializationsBLL().MapGeneralSpecialization(JobCategory.GeneralSpecializations);
                    }
                    if (JobCategory.QualificationsDegrees != null)
                    {
                        JobCategoryBLL.MinQualificationDegree = new QualificationsDegreesBLL().MapQualificationDegree(JobCategory.QualificationsDegrees);
                    }
                }
                return JobCategoryBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}

