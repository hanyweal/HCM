using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using HCMBLL.Enums; 

namespace HCMBLL
{
    public class EmployeesOldJobsBLL : CommonEntity, IEntity
    {
        public virtual int EmployeeOldJobID
        {
            get;
            set;
        }
        public virtual EmployeesCodesBLL EmployeeCode { get; set; }
        public virtual string JobName
        {
            get;
            set;
        }
        public virtual string OrganizationName
        {
            get;
            set;
        }
        public virtual string RankName
        {
            get;
            set;
        }
        public virtual string CareerDegreeName
        {
            get;
            set;
        }
        public virtual DateTime? EmployeeOldJobStartDate
        {
            get;
            set;
        }
        public virtual DateTime? EmployeeOldJobEndDate
        {
            get;
            set;
        }

        public virtual Result Add()
        {
            Result result = null;
            result = new Result();
            EmployeesOldJobs EmployeesOldJob = new EmployeesOldJobs();
            EmployeesOldJob.EmployeeCodeID = this.EmployeeCode.EmployeeCodeID;
            EmployeesOldJob.JobName = this.JobName;
            EmployeesOldJob.OrganizationName = this.OrganizationName;
            EmployeesOldJob.RankName = this.RankName;
            EmployeesOldJob.CareerDegreeName = this.CareerDegreeName;
            EmployeesOldJob.EmployeeOldJobStartDate = this.EmployeeOldJobStartDate;
            EmployeesOldJob.EmployeeOldJobEndDate = this.EmployeeOldJobEndDate;
            EmployeesOldJob.CreatedDate = DateTime.Now;
            EmployeesOldJob.CreatedBy = this.LoginIdentity.EmployeeCodeID;
            this.EmployeeOldJobID = new EmployeesOldJobsDAL().Insert(EmployeesOldJob);
            if (this.EmployeeOldJobID != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(LookupsValidationEnum);
                result.EnumMember = LookupsValidationEnum.Done.ToString();
            }

            return result;
        }
        public virtual Result Remove(int EmployeeOldJobID)
        {
            try
            {
                Result result = null;
                new EmployeesOldJobsDAL().Delete(EmployeeOldJobID, this.LoginIdentity.EmployeeCodeID);
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
            Result result = null;
            result = new Result();
            EmployeesOldJobs EmployeesOldJob = new EmployeesOldJobs();
            EmployeesOldJob.EmployeeOldJobID = this.EmployeeOldJobID;
            EmployeesOldJob.EmployeeCodeID = this.EmployeeCode.EmployeeCodeID;
            EmployeesOldJob.JobName = this.JobName;
            EmployeesOldJob.OrganizationName = this.OrganizationName;
            EmployeesOldJob.RankName = this.RankName;
            EmployeesOldJob.CareerDegreeName = this.CareerDegreeName;
            EmployeesOldJob.EmployeeOldJobStartDate = this.EmployeeOldJobStartDate;
            EmployeesOldJob.EmployeeOldJobEndDate = this.EmployeeOldJobEndDate;
            EmployeesOldJob.LastUpdatedDate = DateTime.Now;
            EmployeesOldJob.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
 
            if (new EmployeesOldJobsDAL().Update(EmployeesOldJob) != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(LookupsValidationEnum);
                result.EnumMember = LookupsValidationEnum.Done.ToString();
            }

            return result;
        }
        public virtual List<EmployeesOldJobsBLL> GetEmployeesOldJobs()
        {
            List<EmployeesOldJobs> EmployeesOldJobsList = new EmployeesOldJobsDAL().GetEmployeesOldJobs();
            List<EmployeesOldJobsBLL> EmployeesOldJobsBLLList = new List<EmployeesOldJobsBLL>();
            if (EmployeesOldJobsList.Count > 0)
            {
                foreach (var item in EmployeesOldJobsList)
                {
                    EmployeesOldJobsBLLList.Add(new EmployeesOldJobsBLL()
                    {
                        EmployeeOldJobID = item.EmployeeOldJobID,
                        EmployeeCode =new EmployeesCodesBLL().MapEmployeeCode(item.EmployeesCodes),
                        JobName = item.JobName,
                        OrganizationName = item.OrganizationName,
                        RankName = item.RankName,
                        CareerDegreeName = item.CareerDegreeName,
                        EmployeeOldJobStartDate = item.EmployeeOldJobStartDate,
                        EmployeeOldJobEndDate = item.EmployeeOldJobEndDate,
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(item.CreatedByNav),
                        CreatedDate = item.CreatedDate,
                        LastUpdatedBy = new EmployeesCodesBLL().MapEmployeeCode(item.LastUpdatedByNav),
                        LastUpdatedDate = item.LastUpdatedDate,
                    });
                }
            }

            return EmployeesOldJobsBLLList;
        }

        public virtual List<EmployeesOldJobsBLL> GetByEmployeeCodeID(int EmployeeCodeID)
        {
            List<EmployeesOldJobs> EmployeesOldJobsList = new EmployeesOldJobsDAL().GetByEmployeeCodeID(EmployeeCodeID);
            List<EmployeesOldJobsBLL> EmployeesOldJobsBLLList = new List<EmployeesOldJobsBLL>();
            if (EmployeesOldJobsList.Count > 0)
            {
                foreach (var item in EmployeesOldJobsList)
                {
                    EmployeesOldJobsBLLList.Add(new EmployeesOldJobsBLL()
                    {
                        EmployeeOldJobID = item.EmployeeOldJobID,
                        EmployeeCode = new EmployeesCodesBLL().MapEmployeeCode(item.EmployeesCodes),
                        JobName = item.JobName,
                        OrganizationName = item.OrganizationName,
                        RankName = item.RankName,
                        CareerDegreeName = item.CareerDegreeName,
                        EmployeeOldJobStartDate = item.EmployeeOldJobStartDate,
                        EmployeeOldJobEndDate = item.EmployeeOldJobEndDate,
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(item.CreatedByNav),
                        CreatedDate = item.CreatedDate,
                        LastUpdatedBy = new EmployeesCodesBLL().MapEmployeeCode(item.LastUpdatedByNav),
                        LastUpdatedDate = item.LastUpdatedDate,
                    });
                }
            }

            return EmployeesOldJobsBLLList;
        }
        public EmployeesOldJobsBLL GetByEmployeeOldJobID(int EmployeeOldJobID)
        {
            return MapEmployeesOldJobsBLL(new EmployeesOldJobsDAL().GetByEmployeeOldJobID(EmployeeOldJobID));
        }

        internal EmployeesOldJobsBLL MapEmployeesOldJobsBLL(EmployeesOldJobs EmployeesOldJob)
        {
            EmployeesOldJobsBLL EmployeesOldJobsBLL = null;
            if (EmployeesOldJob != null)
            {
                EmployeesOldJobsBLL = new EmployeesOldJobsBLL()
                {


                    EmployeeOldJobID = EmployeesOldJob.EmployeeOldJobID,
                    EmployeeCode = new EmployeesCodesBLL().MapEmployeeCode(EmployeesOldJob.EmployeesCodes),
                    JobName = EmployeesOldJob.JobName,
                    OrganizationName = EmployeesOldJob.OrganizationName,
                    RankName = EmployeesOldJob.RankName,
                    CareerDegreeName = EmployeesOldJob.CareerDegreeName,
                    EmployeeOldJobStartDate = EmployeesOldJob.EmployeeOldJobStartDate.Value.Date,
                    EmployeeOldJobEndDate = EmployeesOldJob.EmployeeOldJobEndDate.Value.Date,
                    CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(EmployeesOldJob.CreatedByNav),
                    CreatedDate = EmployeesOldJob.CreatedDate,
                    LastUpdatedBy = new EmployeesCodesBLL().MapEmployeeCode(EmployeesOldJob.LastUpdatedByNav),
                    LastUpdatedDate = EmployeesOldJob.LastUpdatedDate,


                };
            }
            return EmployeesOldJobsBLL;
        }
    }
}

