using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HCMDAL;
using HCMBLL.Enums;

namespace HCMBLL
{
    public class OrganizationsJobsBLL : CommonEntity, IEntity
    {
        public virtual int OrganizationJobID
        {
            get;
            set;
        }

        public virtual bool IsVacant
        {
            get;
            set;
        }

        public virtual bool IsReserved
        {
            get;
            set;
        }

        public virtual RanksBLL Rank
        {
            get;
            set;
        }

        public virtual JobsBLL Job
        {
            get;
            set;
        }

        public virtual OrganizationsJobsStatusBLL OrganizationJobStatus
        {
            get;
            set;
        }

        public virtual OrganizationsJobsStatusEnum OrganizationJobStatusEnum
        {
            get;
            set;
        }

        public virtual OrganizationsStructuresBLL OrganizationStructure
        {
            get;
            set;
        }

        public virtual string JobNo
        {
            get;
            set;
        }

        public virtual int? OrganizationJobParentID
        {
            get;
            set;
        }

        public virtual JobsOperationsTypesBLL JobOperationType
        {
            get;
            set;
        }

        public virtual DateTime? JobOperationDate
        {
            get;
            set;
        }

        public virtual bool IsActive
        {
            get;
            set;
        }


        public virtual int Remove()
        {
            throw new System.NotImplementedException();
        }

        //Get IsActive True
        public virtual OrganizationsJobsBLL GetByOrganizationJobID(int OrganizationJobID)
        {
            try
            {
                OrganizationsJobs OrganizationJob = Get(OrganizationJobID);
                OrganizationsJobsBLL OrganizationJobBLL = MapOrganizationJob(OrganizationJob);
                return OrganizationJobBLL;

            }
            catch
            {
                throw;
            }
        }

        public virtual List<OrganizationsJobsBLL> GetOrganizationsJobs(string JobNo, string OrganizationName, string JobName, string RankName, string JobCode, string JobCategoryName,
                                                                        out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                List<OrganizationsJobs> OrganizationJob = new OrganizationsJobsDAL()
                {
                    search = Search,
                    order = Order,
                    orderDir = OrderDir,
                    startRec = StartRec,
                    pageSize = PageSize
                }.GetOrganizationsJobs(JobNo, OrganizationName, JobName, RankName, JobCode, JobCategoryName, out totalRecordsOut, out recFilterOut);
                List<OrganizationsJobsBLL> OrganizationsJobsBLLList = new List<OrganizationsJobsBLL>();
                if (OrganizationJob != null)
                {
                    foreach (var item in OrganizationJob.Where(x => !x.JobNo.StartsWith("EOS"))) // exclude any jobs were created to End Of Service Module
                        OrganizationsJobsBLLList.Add(new OrganizationsJobsBLL().MapOrganizationJob(item));

                }
                return OrganizationsJobsBLLList.ToList();
            }
            catch
            {
                throw;
            }
        }

        public virtual List<OrganizationsJobsBLL> GetActiveOrganizationsJobs(string JobNo, string OrganizationName, string JobName, string RankName, string JobCode, string JobCategoryName,
                                                                      out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                List<OrganizationsJobs> OrganizationJob = new OrganizationsJobsDAL()
                {
                    search = Search,
                    order = Order,
                    orderDir = OrderDir,
                    startRec = StartRec,
                    pageSize = PageSize
                }.GetActiveOrganizationsJobs(JobNo, OrganizationName, JobName, RankName, JobCode, JobCategoryName, out totalRecordsOut, out recFilterOut);
                List<OrganizationsJobsBLL> OrganizationsJobsBLLList = new List<OrganizationsJobsBLL>();
                if (OrganizationJob != null)
                {
                    foreach (var item in OrganizationJob.Where(x => !x.JobNo.StartsWith("EOS"))) // exclude any jobs were created to End Of Service Module
                        OrganizationsJobsBLLList.Add(new OrganizationsJobsBLL().MapOrganizationJob(item));

                }
                return OrganizationsJobsBLLList.ToList();
            }
            catch
            {
                throw;
            }
        }

        public virtual List<OrganizationsJobsBLL> GetAllOrganizationsJobs()
        {
            try
            {
                List<OrganizationsJobs> OrganizationJob = new OrganizationsJobsDAL().GetAllOrganizationsJobs();
                List<OrganizationsJobsBLL> OrganizationsJobsBLLList = new List<OrganizationsJobsBLL>();
                if (OrganizationJob != null)
                {
                    foreach (var item in OrganizationJob.Where(x => !x.JobNo.StartsWith("EOS"))) // exclude any jobs were created to End Of Service Module
                        OrganizationsJobsBLLList.Add(new OrganizationsJobsBLL().MapOrganizationJob(item));

                }
                return OrganizationsJobsBLLList.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<OrganizationsJobsBLL> GetJobsVacancies(int JobCategoryID, int RankID)
        {
            try
            {
                List<OrganizationsJobs> OrganizationJob = new OrganizationsJobsDAL().GetActiveJobs(JobCategoryID, RankID, true);
                List<OrganizationsJobsBLL> OrganizationsJobsBLLList = new List<OrganizationsJobsBLL>();
                if (OrganizationJob != null)
                {
                    foreach (var item in OrganizationJob.Where(x => !x.JobNo.StartsWith("EOS"))) // exclude any jobs were created to End Of Service Module
                    {
                        OrganizationsJobsBLLList.Add(new OrganizationsJobsBLL().MapOrganizationJob(item));
                    }
                }
                return OrganizationsJobsBLLList.Where(x => x.OrganizationJobStatusEnum == OrganizationsJobsStatusEnum.Active).ToList();
            }
            catch
            {
                throw;
            }
        }

        #region Job Operations
        public virtual Result Creation()
        {
            /*
                Business :-
                  A - No chance to repeated job no and rank and active.
                  B - After creation of organization job, This must be as a job vacant.
             */
            try
            {
                Result result = new Result();
                OrganizationsJobs OrganizationsJobs = new OrganizationsJobs()
                {
                    JobNo = this.JobNo,
                    OrganizationID = this.OrganizationStructure.OrganizationID,
                    JobID = this.Job.JobID,
                    RankID = this.Rank.RankID,
                    CreatedDate = DateTime.Now,
                    IsVacant = true,
                    OrganizationJobStatusID = this.OrganizationJobStatus.OrganizationJobStatusID,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID,
                    JobOperationDate = this.JobOperationDate ?? DateTime.Now,
                    IsActive = true,
                    JobOperationTypeID = (int)JobsOperationsTypesEnum.Creation,
                    OrganizationJobParentID = null,
                    JobKindID = (int)JobsKindsTypesEnum.FormalJob, //<-----======= IMPORTANAT CHange IT Befor CheckIN
                };
                result = Add(OrganizationsJobs);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public virtual Result Modulation()
        {
            /*
             Business :-
                  A - No chance to repeated job no and rank and active.
                  B - If this organization job is not vacant in modulation time, the current employee who has this job will be                            
                        affected after modulation.
                  C - this function Just Update eighter OrganizationID or Job ID
           */
            OrganizationsJobs currentOrganizationsJobs = new OrganizationsJobs();
            try
            {
                Result result = new Result();
                List<OrganizationsJobsBLL> validateOrganizationsJobsOperationDate = GetOrganizationJobHistoryOperationByOrganizationJobID(this.OrganizationJobID);
                if (validateOrganizationsJobsOperationDate.Exists(c => c.JobOperationDate >= this.JobOperationDate))
                {
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseOperationDateLessThanOtherExists.ToString();
                    return result;
                }
                currentOrganizationsJobs = Get(this.OrganizationJobID);
                if (currentOrganizationsJobs != null)
                {
                    currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                    currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                    OrganizationsJobs processOrganizationsJobs = new OrganizationsJobs()
                    {
                        OrganizationJobParentID = currentOrganizationsJobs.OrganizationJobID,
                        OrganizationID = currentOrganizationsJobs.OrganizationID,
                        JobID = currentOrganizationsJobs.JobID,
                        JobNo = currentOrganizationsJobs.JobNo,
                        RankID = currentOrganizationsJobs.RankID,
                        CreatedDate = currentOrganizationsJobs.CreatedDate,
                        IsVacant = currentOrganizationsJobs.IsVacant,
                        OrganizationJobStatusID = currentOrganizationsJobs.OrganizationJobStatusID,
                        CreatedBy = currentOrganizationsJobs.CreatedBy,
                        JobOperationDate = currentOrganizationsJobs.JobOperationDate,
                        IsActive = currentOrganizationsJobs.IsActive,
                        JobOperationTypeID = currentOrganizationsJobs.JobOperationTypeID,
                        JobKindID = currentOrganizationsJobs.JobKindID, //<-----======= IMPORTANAT CHange IT Befor CheckIN
                        //---
                        LastUpdatedBy = currentOrganizationsJobs.LastUpdatedBy,
                        OrganizationJobID = currentOrganizationsJobs.OrganizationJobID,
                        IsReserved = currentOrganizationsJobs.IsReserved,
                        LastUpdatedDate = currentOrganizationsJobs.LastUpdatedDate,
                    };
                    processOrganizationsJobs.IsActive = false;
                    processOrganizationsJobs.IsVacant = false;
                    result = Update(processOrganizationsJobs);
                    if (result.EnumMember == OrganizationJobValidationEnum.Done.ToString())
                    {
                        processOrganizationsJobs.OrganizationID = this.OrganizationStructure.OrganizationID;
                        processOrganizationsJobs.JobID = this.Job.JobID;
                        processOrganizationsJobs.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                        processOrganizationsJobs.JobOperationDate = this.JobOperationDate ?? DateTime.Now;
                        processOrganizationsJobs.JobOperationTypeID = (int)JobsOperationsTypesEnum.Modulation;
                        processOrganizationsJobs.CreatedDate = DateTime.Now;
                        processOrganizationsJobs.LastUpdatedDate = null;
                        processOrganizationsJobs.LastUpdatedBy = null;
                        processOrganizationsJobs.OrganizationJobParentID = currentOrganizationsJobs.OrganizationJobID;
                        processOrganizationsJobs.IsActive = currentOrganizationsJobs.IsActive;
                        processOrganizationsJobs.IsVacant = currentOrganizationsJobs.IsVacant;
                        processOrganizationsJobs.OrganizationJobID = 0;
                        result = Add(processOrganizationsJobs);
                        if (result.EnumMember != OrganizationJobValidationEnum.Done.ToString())
                        {
                            //Rollback
                            Update(currentOrganizationsJobs);
                            return result;

                        }
                        if (!currentOrganizationsJobs.IsVacant)
                        {
                            List<EmployeesCareersHistoryBLL> employeesCareersHistories = new EmployeesCareersHistoryBLL().GetEmployeesByOrganizationJobID(currentOrganizationsJobs.OrganizationJobID);
                            EmployeesCareersHistoryBLL employeesCareersHistory = employeesCareersHistories.FirstOrDefault(c => c.IsActive);
                            if (employeesCareersHistory != null)
                            {
                                employeesCareersHistory.LoginIdentity = this.LoginIdentity;
                                employeesCareersHistory.OrganizationJob = MapOrganizationJob(processOrganizationsJobs);
                                result = employeesCareersHistory.Update();
                            }

                        }
                    }
                    return result;
                }
                else
                {
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString();
                    return result;
                }
            }
            catch (Exception ex)
            {
                //Rollback
                currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                Update(currentOrganizationsJobs);
                throw ex;
            }
        }

        public virtual Result PushingUp()
        {
            /*
              Business :-
                  A - No chance to repeated job no and rank and active.
                  B - The organization job must be a job vacant in pushing Up time.
                  C - End user can change the job no.
                  D - the new rank must be greater than the old rank of organization job.
                  E - this function Just Update eighter Rank or JobNo
           */
            OrganizationsJobs currentOrganizationsJobs = new OrganizationsJobs();
            try
            {
                Result result = new Result();
                List<OrganizationsJobsBLL> validateOrganizationsJobsOperationDate = GetOrganizationJobHistoryOperationByOrganizationJobID(this.OrganizationJobID);
                if (validateOrganizationsJobsOperationDate.Exists(c => c.JobOperationDate >= this.JobOperationDate))
                {
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseOperationDateLessThanOtherExists.ToString();
                    return result;
                }
                currentOrganizationsJobs = Get(this.OrganizationJobID);
                if (currentOrganizationsJobs != null)
                {
                    currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                    currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                    // The organization job must be a job vacant in pushing Up time.
                    if (!currentOrganizationsJobs.IsVacant)
                    {
                        result.EnumType = typeof(OrganizationJobValidationEnum);
                        result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseThisOrganizationJobIsNotVacant.ToString();
                        return result;
                    }
                    var currentRank = new RanksBLL().GetByRankID(currentOrganizationsJobs.RankID);
                    var nextRank = new RanksBLL().GetByRankID(this.Rank.RankID);
                    if (nextRank.RankID <= currentRank.RankID)
                    {
                        result.EnumType = typeof(OrganizationJobValidationEnum);
                        result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldBiggerThanCurrent.ToString();
                        return result;
                    }

                    OrganizationsJobs processOrganizationsJobs = new OrganizationsJobs()
                    {
                        OrganizationJobParentID = currentOrganizationsJobs.OrganizationJobID,
                        OrganizationID = currentOrganizationsJobs.OrganizationID,
                        JobID = currentOrganizationsJobs.JobID,
                        JobNo = currentOrganizationsJobs.JobNo,
                        RankID = currentOrganizationsJobs.RankID,
                        CreatedDate = currentOrganizationsJobs.CreatedDate,
                        IsVacant = currentOrganizationsJobs.IsVacant,
                        OrganizationJobStatusID = currentOrganizationsJobs.OrganizationJobStatusID,
                        CreatedBy = currentOrganizationsJobs.CreatedBy,
                        JobOperationDate = currentOrganizationsJobs.JobOperationDate,
                        IsActive = currentOrganizationsJobs.IsActive,
                        JobOperationTypeID = currentOrganizationsJobs.JobOperationTypeID,
                        JobKindID = currentOrganizationsJobs.JobKindID, //<-----======= IMPORTANAT CHange IT Befor CheckIN
                        //---
                        LastUpdatedBy = currentOrganizationsJobs.LastUpdatedBy,
                        OrganizationJobID = currentOrganizationsJobs.OrganizationJobID,
                        IsReserved = currentOrganizationsJobs.IsReserved,
                        LastUpdatedDate = currentOrganizationsJobs.LastUpdatedDate,
                    };
                    processOrganizationsJobs.IsActive = false;
                    processOrganizationsJobs.IsVacant = false;
                    result = Update(processOrganizationsJobs);
                    if (result.EnumMember == OrganizationJobValidationEnum.Done.ToString())
                    {
                        processOrganizationsJobs.JobNo = this.JobNo;
                        processOrganizationsJobs.RankID = nextRank.RankID;
                        processOrganizationsJobs.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                        processOrganizationsJobs.JobOperationDate = this.JobOperationDate ?? DateTime.Now;
                        processOrganizationsJobs.JobOperationTypeID = (int)JobsOperationsTypesEnum.PushingUp;
                        processOrganizationsJobs.CreatedDate = DateTime.Now;
                        processOrganizationsJobs.LastUpdatedDate = null;
                        processOrganizationsJobs.LastUpdatedBy = null;
                        processOrganizationsJobs.OrganizationJobParentID = currentOrganizationsJobs.OrganizationJobID;
                        processOrganizationsJobs.IsActive = currentOrganizationsJobs.IsActive;
                        processOrganizationsJobs.IsVacant = currentOrganizationsJobs.IsVacant;
                        processOrganizationsJobs.OrganizationJobID = 0;
                        result = Add(processOrganizationsJobs);
                        if (result.EnumMember != OrganizationJobValidationEnum.Done.ToString())
                        {
                            //Rollback
                            Update(currentOrganizationsJobs);
                            return result;

                        }
                    }
                    return result;
                }
                else
                {
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString();
                    return result;
                }
            }
            catch (Exception ex)
            {
                //Rollback
                currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                Update(currentOrganizationsJobs);
                throw ex;
            }
        }

        public virtual Result ScalingDown()
        {
            /*
               Business :-
                  A - No chance to repeated job no and rank and active.
                  B - The organization job must be be a job vacant in scaling down time.
                  C - End user can change the job no.
                  D - the new rank must be less than the old rank of organization job.
           */
            OrganizationsJobs currentOrganizationsJobs = new OrganizationsJobs();
            try
            {
                Result result = new Result();
                List<OrganizationsJobsBLL> validateOrganizationsJobsOperationDate = GetOrganizationJobHistoryOperationByOrganizationJobID(this.OrganizationJobID);
                if (validateOrganizationsJobsOperationDate.Exists(c => c.JobOperationDate >= this.JobOperationDate))
                {
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseOperationDateLessThanOtherExists.ToString();
                    return result;
                }
                currentOrganizationsJobs = Get(this.OrganizationJobID);
                if (currentOrganizationsJobs != null)
                {
                    currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                    currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                    // The organization job must be a job vacant in pushing Up time.
                    if (!currentOrganizationsJobs.IsVacant)
                    {
                        result.EnumType = typeof(OrganizationJobValidationEnum);
                        result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseThisOrganizationJobIsNotVacant.ToString();
                        return result;
                    }
                    var currentRank = new RanksBLL().GetByRankID(currentOrganizationsJobs.RankID);
                    var nextRank = new RanksBLL().GetByRankID(this.Rank.RankID);
                    if (nextRank.RankID >= currentRank.RankID)
                    {
                        result.EnumType = typeof(OrganizationJobValidationEnum);
                        result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldLessThanCurrent.ToString();
                        return result;
                    }

                    OrganizationsJobs processOrganizationsJobs = new OrganizationsJobs()
                    {
                        OrganizationJobParentID = currentOrganizationsJobs.OrganizationJobID,
                        OrganizationID = currentOrganizationsJobs.OrganizationID,
                        JobID = currentOrganizationsJobs.JobID,
                        JobNo = currentOrganizationsJobs.JobNo,
                        RankID = currentOrganizationsJobs.RankID,
                        CreatedDate = currentOrganizationsJobs.CreatedDate,
                        IsVacant = currentOrganizationsJobs.IsVacant,
                        OrganizationJobStatusID = currentOrganizationsJobs.OrganizationJobStatusID,
                        CreatedBy = currentOrganizationsJobs.CreatedBy,
                        JobOperationDate = currentOrganizationsJobs.JobOperationDate,
                        IsActive = currentOrganizationsJobs.IsActive,
                        JobOperationTypeID = currentOrganizationsJobs.JobOperationTypeID,
                        JobKindID = currentOrganizationsJobs.JobKindID, //<-----======= IMPORTANAT CHange IT Befor CheckIN
                        //---
                        LastUpdatedBy = currentOrganizationsJobs.LastUpdatedBy,
                        OrganizationJobID = currentOrganizationsJobs.OrganizationJobID,
                        IsReserved = currentOrganizationsJobs.IsReserved,
                        LastUpdatedDate = currentOrganizationsJobs.LastUpdatedDate,
                    };
                    processOrganizationsJobs.IsActive = false;
                    processOrganizationsJobs.IsVacant = false;
                    result = Update(processOrganizationsJobs);
                    if (result.EnumMember == OrganizationJobValidationEnum.Done.ToString())
                    {
                        processOrganizationsJobs.JobNo = this.JobNo;
                        processOrganizationsJobs.RankID = nextRank.RankID;
                        processOrganizationsJobs.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                        processOrganizationsJobs.JobOperationDate = this.JobOperationDate ?? DateTime.Now;
                        processOrganizationsJobs.JobOperationTypeID = (int)JobsOperationsTypesEnum.ScalingDown;
                        processOrganizationsJobs.CreatedDate = DateTime.Now;
                        processOrganizationsJobs.LastUpdatedDate = null;
                        processOrganizationsJobs.LastUpdatedBy = null;
                        processOrganizationsJobs.OrganizationJobParentID = currentOrganizationsJobs.OrganizationJobID;
                        processOrganizationsJobs.IsActive = currentOrganizationsJobs.IsActive;
                        processOrganizationsJobs.IsVacant = currentOrganizationsJobs.IsVacant;
                        processOrganizationsJobs.OrganizationJobID = 0;
                        result = Add(processOrganizationsJobs);
                        if (result.EnumMember != OrganizationJobValidationEnum.Done.ToString())
                        {
                            //Rollback
                            Update(currentOrganizationsJobs);
                            return result;

                        }
                    }
                    return result;
                }
                else
                {
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString();
                    return result;
                }
            }
            catch (Exception ex)
            {
                //Rollback
                currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                Update(currentOrganizationsJobs);
                throw ex;
            }
        }

        public virtual Result Cancelation()
        {
            /*
               Business :-
                  A -  The organization job must be a job vacant in cancellation time.
           */
            OrganizationsJobs currentOrganizationsJobs = new OrganizationsJobs();
            try
            {
                Result result = new Result();
                List<OrganizationsJobsBLL> validateOrganizationsJobsOperationDate = GetOrganizationJobHistoryOperationByOrganizationJobID(this.OrganizationJobID);
                if (validateOrganizationsJobsOperationDate.Exists(c => c.JobOperationDate >= this.JobOperationDate))
                {
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseOperationDateLessThanOtherExists.ToString();
                    return result;
                }
                currentOrganizationsJobs = Get(this.OrganizationJobID);
                if (currentOrganizationsJobs != null)
                {
                    currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                    currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                    // The organization job must be a job vacant in pushing Up time.
                    if (!currentOrganizationsJobs.IsVacant)
                    {
                        result.EnumType = typeof(OrganizationJobValidationEnum);
                        result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseThisOrganizationJobIsNotVacant.ToString();
                        return result;
                    }

                    OrganizationsJobs processOrganizationsJobs = new OrganizationsJobs()
                    {
                        OrganizationJobParentID = currentOrganizationsJobs.OrganizationJobID,
                        OrganizationID = currentOrganizationsJobs.OrganizationID,
                        JobID = currentOrganizationsJobs.JobID,
                        JobNo = currentOrganizationsJobs.JobNo,
                        RankID = currentOrganizationsJobs.RankID,
                        CreatedDate = currentOrganizationsJobs.CreatedDate,
                        IsVacant = false,
                        IsActive = false,
                        OrganizationJobStatusID = currentOrganizationsJobs.OrganizationJobStatusID,
                        CreatedBy = currentOrganizationsJobs.CreatedBy,
                        JobOperationDate = currentOrganizationsJobs.JobOperationDate,
                        JobOperationTypeID = currentOrganizationsJobs.JobOperationTypeID,
                        JobKindID = currentOrganizationsJobs.JobKindID, //<-----======= IMPORTANAT CHange IT Befor CheckIN
                        LastUpdatedBy = currentOrganizationsJobs.LastUpdatedBy,
                        OrganizationJobID = currentOrganizationsJobs.OrganizationJobID,
                        IsReserved = currentOrganizationsJobs.IsReserved,
                        LastUpdatedDate = currentOrganizationsJobs.LastUpdatedDate,
                    };
                
                    result = Update(processOrganizationsJobs);

                    if (result.EnumMember == OrganizationJobValidationEnum.Done.ToString())
                    {
                        processOrganizationsJobs.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                        processOrganizationsJobs.JobOperationDate = this.JobOperationDate ?? DateTime.Now;
                        processOrganizationsJobs.JobOperationTypeID = (int)JobsOperationsTypesEnum.Cancelation;
                        processOrganizationsJobs.CreatedDate = DateTime.Now;
                        processOrganizationsJobs.LastUpdatedDate = null;
                        processOrganizationsJobs.LastUpdatedBy = null;
                        processOrganizationsJobs.OrganizationJobParentID = currentOrganizationsJobs.OrganizationJobID;
                        processOrganizationsJobs.IsActive = false;
                        processOrganizationsJobs.IsVacant = false;
                        processOrganizationsJobs.OrganizationJobID = 0;

                        result = Add(processOrganizationsJobs);

                        if (result.EnumMember != OrganizationJobValidationEnum.Done.ToString())
                        {
                            //Rollback
                            Update(currentOrganizationsJobs);
                            return result;
                        }
                    }
                    return result;
                }
                else
                {
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString();
                    return result;
                }
            }
            catch (Exception ex)
            {
                //Rollback
                currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                Update(currentOrganizationsJobs);
                throw ex;
            }
        }

        public virtual Result Modification()//<---==== This will be Modification
        {
            /*
               Business :-
                  A - No chance to repeated job no and rank and active.
                  C - Editing any field of any organization job record.
                  D - The organization job must be be a Avtive in Editing time.
           */
            OrganizationsJobs currentOrganizationsJobs = new OrganizationsJobs();
            try
            {
                Result result = new Result();
                currentOrganizationsJobs = Get(this.OrganizationJobID);
                #region Validation to check if OrganizationJob Exist in PromotionsRecordsEmployees as PromotionRecordJobVacancyID
                //check if OrganizationJob Exist in PromotionsRecordsJobsVacancies as PromotionRecordJobVacancyID
                // Then Can't update OrganizationID,RankID,JobID
                PromotionsRecordsJobsVacanciesBLL PromotionsRecordsJobsVacancies = new PromotionsRecordsJobsVacanciesBLL().GetByOrganizationJobID(this.OrganizationJobID);
                if (PromotionsRecordsJobsVacancies != null)
                {
                    if (PromotionsRecordsJobsVacancies.JobVacancy.OrganizationStructure.OrganizationID != this.OrganizationStructure.OrganizationID || PromotionsRecordsJobsVacancies.JobVacancy.Job.JobID != this.Job.JobID || PromotionsRecordsJobsVacancies.JobVacancy.Rank.RankID != this.Rank.RankID)
                    {
                        result.EnumType = typeof(OrganizationJobValidationEnum);
                        result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseOfExistsInPromotionsRecordsJobsVacancies.ToString();
                        return result;
                    }
                }
                #endregion
                if (currentOrganizationsJobs != null)
                {
                    currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                    currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                    // The organization job must be a job vacant in pushing Up time.
                    if (!currentOrganizationsJobs.IsActive)
                    {
                        result.EnumType = typeof(OrganizationJobValidationEnum);
                        result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString();
                        return result;
                    }


                    currentOrganizationsJobs.OrganizationJobID = this.OrganizationJobID;
                    currentOrganizationsJobs.JobNo = this.JobNo;
                    currentOrganizationsJobs.OrganizationID = this.OrganizationStructure.OrganizationID;
                    currentOrganizationsJobs.JobID = this.Job.JobID;
                    currentOrganizationsJobs.RankID = this.Rank.RankID;
                    currentOrganizationsJobs.OrganizationJobStatusID = this.OrganizationJobStatus.OrganizationJobStatusID;
                    currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                    currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                    currentOrganizationsJobs.IsVacant = this.IsVacant;

                    result = Update(currentOrganizationsJobs);
                    return result;
                }
                else
                {
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString();
                    return result;
                }
            }
            catch (Exception ex)
            {
                //Rollback
                currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                Update(currentOrganizationsJobs);
                throw ex;
            }
        }

        internal Result Transferring(int OrganizationJobID)
        {
            try
            {
                Result result = new Result();

                OrganizationsJobs currentOrganizationsJobs = new OrganizationsJobs();

                currentOrganizationsJobs = new OrganizationsJobsDAL().GetByOrganizationJobID(OrganizationJobID);
                if (currentOrganizationsJobs != null)
                {
                    currentOrganizationsJobs.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                    currentOrganizationsJobs.LastUpdatedDate = DateTime.Now;
                   
                    OrganizationsJobs processOrganizationsJobs = new OrganizationsJobs()
                    {
                        OrganizationJobParentID = currentOrganizationsJobs.OrganizationJobID,
                        OrganizationID = currentOrganizationsJobs.OrganizationID,
                        JobID = currentOrganizationsJobs.JobID,
                        JobNo = currentOrganizationsJobs.JobNo,
                        RankID = currentOrganizationsJobs.RankID,
                        OrganizationJobStatusID = currentOrganizationsJobs.OrganizationJobStatusID,
                        JobOperationDate = currentOrganizationsJobs.JobOperationDate,
                        IsActive = false,
                        IsVacant = false,
                        JobOperationTypeID = currentOrganizationsJobs.JobOperationTypeID,
                        JobKindID = currentOrganizationsJobs.JobKindID, //<-----======= IMPORTANAT CHange IT Befor CheckIN
                        OrganizationJobID = currentOrganizationsJobs.OrganizationJobID,
                        IsReserved = currentOrganizationsJobs.IsReserved,
                        CreatedBy = currentOrganizationsJobs.CreatedBy,
                        CreatedDate = currentOrganizationsJobs.CreatedDate,
                        LastUpdatedDate = currentOrganizationsJobs.LastUpdatedDate,
                        LastUpdatedBy = currentOrganizationsJobs.LastUpdatedBy,
                    };
                   
                    result = Update(processOrganizationsJobs);

                    if (result.EnumMember == OrganizationJobValidationEnum.Done.ToString())
                    {
                        processOrganizationsJobs.JobOperationDate = this.JobOperationDate ?? DateTime.Now;
                        processOrganizationsJobs.JobOperationTypeID = (int)JobsOperationsTypesEnum.Transferring;
                        processOrganizationsJobs.OrganizationJobParentID = currentOrganizationsJobs.OrganizationJobID;
                        processOrganizationsJobs.OrganizationJobID = 0;
                        processOrganizationsJobs.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                        processOrganizationsJobs.CreatedDate = DateTime.Now;
                        processOrganizationsJobs.LastUpdatedDate = null;
                        processOrganizationsJobs.LastUpdatedBy = null;

                        result = Add(processOrganizationsJobs);

                        if (result.EnumMember != OrganizationJobValidationEnum.Done.ToString())
                        {
                            //Rollback
                            Update(currentOrganizationsJobs);
                            return result;
                        }
                    }
                    return result;
                }
                else
                {
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString();
                    return result;
                }

                //OrganizationsJobs OrganizationsJobs = new OrganizationsJobs()
                //{
                //    OrganizationJobID = OrganizationJobID,
                //    //OrganizationJobStatusID = (int)OrganizationsJobsStatusEnum.NotActive,
                //    IsActive = false,
                //    IsVacant = false,
                  
                //};


                //new OrganizationsJobsDAL().UpdateJobStatus(OrganizationsJobs);
                //result.EnumMember = OrganizationJobValidationEnum.Done.ToString();
                //result.Entity = this;

                //return result;
            }
            catch
            {
                throw;
            }
        }

        public virtual List<OrganizationsJobsBLL> GetOrganizationJobHistoryOperationByOrganizationJobID(int OrganizationJobID)
        {
            List<OrganizationsJobsBLL> organizationsJobsBLLs = new List<OrganizationsJobsBLL>();
            List<OrganizationsJobs> organizationsJobs = new OrganizationsJobsDAL().GetOrganizationJobHistoryOperationByOrganizationJobID(OrganizationJobID);
            organizationsJobs?.ForEach(c => organizationsJobsBLLs.Add(MapOrganizationJob(c)));
            return organizationsJobsBLLs;
        }
        #endregion

        //public virtual Result Update()//<---==== This will be Removed
        //{
        //    return new Result();
        //}

        internal Result UpdateIsReserved(int OrganizationJobID, bool IsReserved)
        {
            try
            {
                Result result = new Result();
                OrganizationsJobs OrganizationsJobs = new OrganizationsJobs()
                {
                    OrganizationJobID = OrganizationJobID,
                    IsReserved = IsReserved,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                };

                new OrganizationsJobsDAL().UpdateIsReserved(OrganizationsJobs);
                result.Entity = this;

                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// to be the job as vacant, the status of this job it should be active, otherwise no chane to be vacant
        /// </summary>
        /// <param name="OrganizationJobID"></param>
        /// <returns></returns>
        internal Result SetJobAsVacant(int OrganizationJobID)
        {
            try
            {
                Result result = null;

                #region Checking if the job status is active or not
                OrganizationsJobsBLL OrganizationJobObj = this.GetByOrganizationJobID(OrganizationJobID);
                if (OrganizationJobObj.OrganizationJobStatusEnum == OrganizationsJobsStatusEnum.NotActive)
                {
                    result = new Result();
                    result.Entity = OrganizationJobObj;
                    result.EnumMember = OrganizationStructureValidationEnum.RejectedBecauseOfJobStatusNotActive.ToString();

                    return result;
                }

                #endregion

                OrganizationsJobs OrganizationsJobs = new OrganizationsJobs()
                {
                    OrganizationJobID = OrganizationJobID,
                    IsVacant = true,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                };

                new OrganizationsJobsDAL().UpdateIsVacant(OrganizationsJobs);
                result = new Result();
                result.EnumMember = OrganizationJobValidationEnum.Done.ToString();
                result.Entity = this;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal OrganizationsJobsBLL MapOrganizationJob(OrganizationsJobs OrganizationJob)
        {
            try
            {
                OrganizationsJobsBLL OrganizationJobBLL = null;
                if (OrganizationJob != null)
                {
                    OrganizationJobBLL = new OrganizationsJobsBLL();
                    //{
                    OrganizationJobBLL.OrganizationJobID = OrganizationJob.OrganizationJobID;
                    OrganizationJobBLL.JobNo = OrganizationJob.JobNo;
                    OrganizationJobBLL.IsVacant = OrganizationJob.IsVacant;
                    OrganizationJobBLL.IsReserved = OrganizationJob.IsReserved != null ? (OrganizationJob.IsReserved.HasValue ? OrganizationJob.IsReserved.Value : false) : false;
                    OrganizationJobBLL.Job = new JobsBLL().MapJob(OrganizationJob.Jobs);
                    OrganizationJobBLL.Rank = new RanksBLL().MapRank(OrganizationJob.Ranks);
                    OrganizationJobBLL.OrganizationStructure = new OrganizationsStructuresBLL().MapOrganization(OrganizationJob.OrganizationsStructures);
                    OrganizationJobBLL.CreatedDate = OrganizationJob.CreatedDate;
                    OrganizationJobBLL.OrganizationJobStatus = new OrganizationsJobsStatusBLL().MapOrganizationsJobsStatus(OrganizationJob.OrganizationsJobsStatus);
                    OrganizationJobBLL.OrganizationJobStatusEnum = OrganizationJob.OrganizationsJobsStatus != null ? (OrganizationsJobsStatusEnum)OrganizationJob.OrganizationsJobsStatus.OrganizationJobStatusID : OrganizationsJobsStatusEnum.NotActive;
                    OrganizationJobBLL.JobOperationDate = OrganizationJob.JobOperationDate;
                    OrganizationJobBLL.IsActive = OrganizationJob.IsActive;
                    OrganizationJobBLL.JobOperationType = new JobsOperationsTypesBLL().MapJobsOperationsTypesBLL(OrganizationJob.JobsOperationsTypes);
                    //};
                }
                return OrganizationJobBLL;
            }
            catch
            {
                throw;
            }
        }
        private Result Add(OrganizationsJobs organizationJob)
        {
            Result result = new Result();
            #region General Validation
            //Business Rule : No chance to repeated job no and rank and active.
            OrganizationsJobs activeOrganizationJobWithJobNoAndRankID = new OrganizationsJobsDAL().GetIsActiveWithJobNoAndRankID(organizationJob.JobNo, organizationJob.RankID);
            if (activeOrganizationJobWithJobNoAndRankID != null)
            {
                result.EnumType = typeof(OrganizationJobValidationEnum);
                result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseOfExistsActiveJobWithJobNoAndRankID.ToString();

                return result;
            }
            #endregion
            new OrganizationsJobsDAL().Insert(organizationJob);
            result.Entity = new OrganizationsJobsBLL().MapOrganizationJob(organizationJob);
            result.EnumType = typeof(OrganizationJobValidationEnum);
            result.EnumMember = OrganizationJobValidationEnum.Done.ToString();
            return result;
        }

        private Result Update(OrganizationsJobs organizationJob)
        {
            try
            {
                Result result = new Result();
                #region General Validation
                //Business Rule : No chance to repeated job no and rank and active.
                OrganizationsJobs activeOrganizationJobWithJobNoAndRankID = new OrganizationsJobsDAL().GetIsActiveWithJobNoAndRankID(organizationJob.JobNo, organizationJob.RankID);
                if (activeOrganizationJobWithJobNoAndRankID != null && activeOrganizationJobWithJobNoAndRankID.OrganizationJobID != organizationJob.OrganizationJobID)
                {

                    //  Entity= new OrganizationsJobsBLL().MapOrganizationJob(activeOrganizationJobWithJobNoAndRankID),
                    result.EnumType = typeof(OrganizationJobValidationEnum);
                    result.EnumMember = OrganizationJobValidationEnum.RejectedBecauseOfExistsActiveJobWithJobNoAndRankID.ToString();

                    return result;
                }
                #endregion

                new OrganizationsJobsDAL().Update(organizationJob);
                result.Entity = new OrganizationsJobsBLL().MapOrganizationJob(organizationJob);
                result.EnumType = typeof(OrganizationJobValidationEnum);
                result.EnumMember = OrganizationJobValidationEnum.Done.ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private Result Delete(OrganizationsJobs organizationJob)
        //{
        //    try
        //    {
        //        Result result = new Result();
        //        new OrganizationsJobsDAL().Delete(organizationJob.OrganizationJobID, this.LoginIdentity.EmployeeCodeID);
        //        result.Entity = new OrganizationsJobsBLL().MapOrganizationJob(organizationJob);
        //        result.EnumType = typeof(OrganizationJobValidationEnum);
        //        result.EnumMember = OrganizationJobValidationEnum.Done.ToString();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private OrganizationsJobs Get(int OrganizationJobID)
        {
            //Business Rule : Get Just Active With Not repeated jobNo and rank.
            return new OrganizationsJobsDAL().GetActiveByOrganizationJobID(OrganizationJobID);

        }
    }
}

