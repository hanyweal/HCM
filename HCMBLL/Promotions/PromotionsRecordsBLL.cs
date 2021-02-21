using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public partial class PromotionsRecordsBLL : CommonEntity, IEntity
    {
        public int PromotionRecordID { get; set; }

        public int PromotionRecordNo { get; set; }

        public DateTime PromotionRecordDate { get; set; }

        public RanksBLL Rank { get; set; }

        public JobsCategoriesBLL JobCategory { get; set; }

        public PromotionsPeriodsBLL PromotionPeriod { get; set; }

        public PromotionsRecordsStatusEnum PromotionRecordStatusEnum { get; set; }

        public PromotionsRecordsStatusBLL PromotionRecordStatus { get; set; }

        public DateTime OpeningTime { get; set; }

        public EmployeesCodesBLL OpeningBy { get; set; }

        public DateTime? InstallationTime { get; set; }

        public EmployeesCodesBLL InstallationBy { get; set; }

        public DateTime? ClosingTime { get; set; }

        public EmployeesCodesBLL ClosingBy { get; set; }

        public DateTime PromotionDate { get; set; }

        public int TotalJobVacancies { get; set; }

        public int TotalIncludedCandidates { get; set; }

        public int TotalPromotedCandidates { get; set; }

        public List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployees { get; set; }

        public List<PromotionsRecordsJobsVacanciesBLL> PromotionsRecordsJobsVacancies { get; set; }

        /// <summary>
        /// there are conditions to add new promotion record :
        /// 1 - if there is any promotion record is not closed in other ranks on tha same period, no chance to add this new one
        /// 2 - if there is any promotion record is not installed on the same period, no chance to add this new one /////////////////////////////and its job category has relation with the new job category in the new promotion record, no chance to add this new one, the promotion record must be installed first
        /// 3 - if there is no any job vacancy under the job category, no chance to add this new one
        /// 4 - if there is no any candidates eligibale under the job category, no chance to add this new one
        /// </summary>
        /// <returns></returns>
        public Result Add()
        {
            try
            {
                Result result = null;

                #region Get promotion period data
                this.PromotionPeriod = new PromotionsPeriodsBLL().GetByPromotionPeriodID(this.PromotionPeriod.PromotionPeriodID);
                #endregion

                List<PromotionsRecordsBLL> PromotionsRecordsList = this.GetPromotionsRecords();
                #region Validate if there is any promotion record is not closed in other rank on the same period, no chance to add this new one, the other promotion record must be closed first
                result = AreThereOtherPromotionsRecordNotClosedInOtherRanks(PromotionsRecordsList, this.PromotionPeriod.PromotionPeriodID, this.Rank.RankID);
                if (result != null)
                    return result;
                #endregion

                #region Validate if there is any promotion record is not installed, no chance to add this new one, the promotion record must be installed first
                result = AreThereOtherPromotionsRecordNotInstalled(PromotionsRecordsList, this.PromotionPeriod.PromotionPeriodID);
                if (result != null)
                    return result;
                #endregion

                #region Validate if there are job vacancies under the rank and category or not
                List<PromotionsRecordsJobsVacancies> JobsVacanciesList = new List<PromotionsRecordsJobsVacancies>();
                foreach (var item in new OrganizationsJobsBLL().GetJobsVacancies(this.JobCategory.JobCategoryID, this.Rank.RankID))
                {
                    JobsVacanciesList.Add(new PromotionsRecordsJobsVacancies()
                    {
                        OrganizationJobID = item.OrganizationJobID,
                        CreatedBy = this.LoginIdentity.EmployeeCodeID,
                        CreatedDate = DateTime.Now
                    });
                }

                if (JobsVacanciesList.Count == 0)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString();
                    return result;
                }
                #endregion

                #region Validate if there are candidates should be entered in promotion record
                List<PromotionsRecordsEmployees> CandidatesList = new List<PromotionsRecordsEmployees>();
                //List<PromotionsRecordsEmployeesBLL> CandidatesBLLList = this.GetCandidatesInPromotionRecord(this.JobCategory.JobCategoryID, this.PromotionPeriod.PromotionPeriodID, this.Rank.RankID);
                //List<PromotionsRecordsEmployeesBLL> CandidatesBLLList = this.PromotionsRecordsEmployees != null ? this.PromotionsRecordsEmployees : new List<PromotionsRecordsEmployeesBLL>();

                if (this.PromotionsRecordsEmployees == null || this.PromotionsRecordsEmployees.Where(x => x.IsIncluded == true).Count() == 0)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString();
                    return result;
                }
                else
                {
                    foreach (var item in this.PromotionsRecordsEmployees)
                    {
                        CandidatesList.Add(new PromotionsRecordsEmployees()
                        {
                            CurrentEmployeeCareerHistoryID = item.CurrentEmployeeCareer.EmployeeCareerHistoryID,
                            CurrentEmployeesCareersHistory = new EmployeesCareersHistory() { EmployeeCareerHistoryID = item.CurrentEmployeeCareer.EmployeeCareerHistoryID, EmployeeCodeID = item.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID },
                            CurrentJobDaysCount = item.CurrentJobDaysCount,
                            CurrentJobJoinDate = item.CurrentJobJoinDate,
                            PreviousEmployeeCareerHistoryID = item.PreviousEmployeeCareer != null ? item.PreviousEmployeeCareer.EmployeeCareerHistoryID : (int?)null,
                            PreviousJobDaysCount = item.PreviousJobDaysCount,
                            PreviousJobJoinDate = item.PreviousEmployeeCareer != null ? item.PreviousEmployeeCareer.JoinDate : (DateTime?)null,
                            LastQualificationDegreeID = item.LastQualificationDegree.QualificationDegreeID != 0 ? item.LastQualificationDegree.QualificationDegreeID : (int?)null,
                            LastQualificationID = item.LastQualification.QualificationID != 0 ? item.LastQualification.QualificationID : (int?)null,
                            LastGeneralSpecializationID = item.LastGeneralSpecialization.GeneralSpecializationID != 0 ? item.LastGeneralSpecialization.GeneralSpecializationID : (int?)null,
                            OnGoingServiceDaysCount = item.OnGoingServiceDaysCount,
                            PriorServiceDaysCount = item.PriorServiceDaysCount,
                            PromotionCandidateAddedWayID = (int)item.PromotionCandidateAddedWay.PromotionCandidateAddedWayID,
                            IsIncluded = item.IsIncluded,
                            CreatedBy = this.LoginIdentity.EmployeeCodeID,
                            CreatedDate = DateTime.Now,
                        });
                    }
                }

                #endregion

                #region Get distinct qualification of all candidates that will be entered in promotion record
                List<PromotionsRecordsQualificationsPoints> PromotionRecordQualificationsList = GetDistinctQualifications(CandidatesList);
                CandidatesList.ForEach(x => x.CurrentEmployeesCareersHistory = null);
                #endregion

                #region Adding new record
                this.PromotionRecordNo = new PromotionsRecordsDAL().GetMaxPromotionRecordNo(this.PromotionPeriod.PromotionPeriodID);
                PromotionsRecords PromotionRecord = new PromotionsRecords()
                {
                    PromotionRecordNo = this.PromotionRecordNo,
                    PromotionRecordDate = this.PromotionRecordDate,
                    JobCategoryID = this.JobCategory.JobCategoryID,
                    RankID = this.Rank.RankID,
                    PromotionPeriodID = this.PromotionPeriod.PromotionPeriodID,
                    PromotionRecordStatusID = (int)PromotionsRecordsStatusEnum.Open,
                    OpeningTime = DateTime.Now,
                    OpeningBy = this.LoginIdentity.EmployeeCodeID,
                    CreationDate = DateTime.Now,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID,
                    PromotionsRecordsJobsVacancies = JobsVacanciesList,
                    PromotionsRecordsEmployees = CandidatesList,
                    PromotionsRecordsQualificationsPoints = PromotionRecordQualificationsList
                };
                this.PromotionRecordID = new PromotionsRecordsDAL().Insert(PromotionRecord);
                this.PromotionRecordStatus = new PromotionsRecordsStatusBLL().GetByPromotionRecordStatusID((int)PromotionsRecordsStatusEnum.Open);
                #endregion

                if (this.PromotionRecordID != 0)
                {
                    #region Add log
                    result = new PromotionsRecordsLogsBLL()
                    {
                        PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = this.PromotionRecordID },
                        PromotionRecordNo = PromotionRecord.PromotionRecordNo,
                        PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.Open },
                        ActionDescription = string.Format(Globalization.PromotionRecordActionDescriptionOpenedText, CandidatesList.Count, JobsVacanciesList.Count),
                        LoginIdentity = this.LoginIdentity
                    }.Add();
                    #endregion

                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.Done.ToString();
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// there is conditions before promotion deleting:
        /// 1 - promotion status must be opened only
        /// </summary>
        /// <returns></returns>
        public Result Remove(int PromotionRecordID)
        {
            try
            {
                Result result = null;

                PromotionsRecordsBLL promotionRecord = new PromotionsRecordsBLL().GetByPromotionRecordID(PromotionRecordID);
                int PromotionRecordNo = promotionRecord.PromotionRecordNo;

                #region Validate if PromotionRecordID is passed with correct number or not
                if (PromotionRecordID == 0)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoPromotionRecordSelectedToDelete.ToString();
                    return result;
                }
                #endregion

                #region Validate if the promotion status open or not
                if (promotionRecord.PromotionRecordStatusEnum != PromotionsRecordsStatusEnum.Open)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfPromotionIsNotOpen.ToString();
                    return result;
                }
                #endregion

                #region Deleting
                new PromotionsRecordsDAL().Delete(PromotionRecordID, LoginIdentity.EmployeeCodeID);

                #region Add log
                result = new PromotionsRecordsLogsBLL()
                {
                    PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecordID },
                    PromotionRecordNo = PromotionRecordNo,
                    PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.Remove },
                    ActionDescription = string.Empty,
                    LoginIdentity = this.LoginIdentity
                }.Add();
                #endregion

                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.Done.ToString();
                return result;
                #endregion
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// there are conditions to fetch employees as candidates in promotion record: 
        /// 1 - the employee must be in the previous rank of promotion record rank.
        /// 2 - the employee must complete 3 years in previous rank (his last job).
        /// 3 - the employee must have minimum qualification of the job category.
        /// 4 - the employee must have last year evaluation higher than weak evaluation.

        /// </summary>
        /// <param name="JobCategoryID"></param>
        /// <param name="PromotionPeriodID"></param>
        /// <param name="RankID"></param>
        /// <returns></returns>
        public Result GetCandidatesShouldBeInPromotionRecord(int JobCategoryID, int PromotionPeriodID, int RankID, int ExceptPromotionRecordID = 0)
        {
            try
            {
                Result result = null;
                List<PromotionsRecordsBLL> PromotionsRecordsList = this.GetPromotionsRecords();
                #region Validate if there is any promotion record is not installed, no chance to get candidates, the promotion record must be installed first
                result = AreThereOtherPromotionsRecordNotInstalled(PromotionsRecordsList, PromotionPeriodID, ExceptPromotionRecordID);
                if (result != null)
                    return result;
                #endregion

                RanksBLL CurrentEmployeesRank = new RanksBLL().GetPreviousRankByRankID(RankID);
                RanksBLL PreviousEmployeesRank = new RanksBLL().GetPreviousRankByRankID(CurrentEmployeesRank.RankID); // we need this to get previous jobs of employees to calculate Previous Job Days Count
                if (PreviousEmployeesRank == null) PreviousEmployeesRank = new RanksBLL() { RankID = 0 };
                PromotionsRecordsEmployeesBLL PromotionRecordEmployee = new PromotionsRecordsEmployeesBLL();
                PromotionsPeriodsBLL PromotionPeriod = new PromotionsPeriodsBLL().GetByPromotionPeriodID(PromotionPeriodID);
                List<spGetDeservedEmployeesToBeIncludedPromotion_Result> DeservedEmployeesList = new EmployeesCareersHistoryDAL().GetDeservedEmployeesToBeIncludedPromotion(CurrentEmployeesRank.RankID, JobCategoryID, PromotionPeriodID).Where(x => PromotionPeriod.PromotionEndDate >= x.JoinDate.Value.AddDays(PromotionRecordEmployee.DaysCountBetweenRanksToPromotion)).ToList();
                List<EmployeesCareersHistoryBLL> ActiveEmployees = new EmployeesCareersHistoryBLL().GetActiveEmployeesByRankID(CurrentEmployeesRank.RankID);
                List<EmployeesCareersHistoryBLL> PreviousJobs = new EmployeesCareersHistoryBLL().GetNotActiveJobsByRankID(PreviousEmployeesRank.RankID, DeservedEmployeesList.Select(x => x.EmployeeCodeID.Value).ToList());
                EvaluationPointsEnum[] EvalExcludedFromPromotion = new EvaluationPointsBLL().GetEvalutionPointsExcludedFromPromotion();

                List<PromotionsRecordsEmployeesBLL> CandidatesList = new List<PromotionsRecordsEmployeesBLL>();

                DateTime EndDate = PromotionPeriod.PromotionEndDate;
                foreach (var item in DeservedEmployeesList)
                {
                    //StartDate = item.JoinDate;
                    EmployeesCareersHistoryBLL CurrentEmployeeJob = ActiveEmployees.FirstOrDefault(x => x.EmployeeCareerHistoryID == item.EmployeeCareerHistoryID);
                    EmployeesCareersHistoryBLL PreviousEmployeeJob = PreviousJobs != null ? PreviousJobs.FirstOrDefault(x => x.EmployeeCode.EmployeeCodeID == item.EmployeeCodeID) : null;
                    EmployeesQualificationsBLL LastEmployeeQualification = new EmployeesQualificationsBLL().GetLastEmployeeQualification(CurrentEmployeeJob.EmployeeCode.EmployeeCodeID);

                    PromotionsRecordsEmployeesBLL Candidate = new PromotionsRecordsEmployeesBLL();

                    Candidate.LastQualificationDegree = LastEmployeeQualification.QualificationDegree;
                    Candidate.LastQualification = LastEmployeeQualification.Qualification;
                    Candidate.LastGeneralSpecialization = LastEmployeeQualification.GeneralSpecialization;

                    Candidate.CurrentEmployeeCareer = CurrentEmployeeJob;
                    Candidate.CurrentJobDaysCount = PromotionRecordEmployee.GetCandidatePeriodAfterExcludingExceptionsPeriods(CurrentEmployeeJob, PromotionPeriod.PromotionEndDate);
                    Candidate.CurrentJobJoinDate = CurrentEmployeeJob.JoinDate;

                    Candidate.PreviousEmployeeCareer = PreviousEmployeeJob;
                    Candidate.PreviousJobDaysCount = PreviousEmployeeJob != null ? int.Parse((CurrentEmployeeJob.JoinDate.Date - PreviousEmployeeJob.JoinDate.Date).TotalDays.ToString()) : 0;
                    Candidate.PreviousJobJoinDate = PreviousEmployeeJob != null ? PreviousEmployeeJob.JoinDate.Date : (DateTime?)null;

                    //Candidate.OnGoingServiceDaysCount = item.HiringDate.HasValue ? int.Parse((PromotionPeriodBLL.PromotionEndDate.Date - item.HiringDate.Value.Date).TotalDays.ToString()) : 0;
                    Candidate.OnGoingServiceDaysCount = 0;  // it will be calculated by preference
                    Candidate.PriorServiceDaysCount = 0; // it will be calculated by preference

                    Candidate.IsIncluded = PromotionRecordEmployee.IsValidToIncludeAfterCheckConditions(CurrentEmployeeJob, Candidate.CurrentJobDaysCount, PromotionPeriod, EvalExcludedFromPromotion);
                    Candidate.PromotionCandidateAddedWay = new PromotionCandidateAddedWayBLL() { PromotionCandidateAddedWayID = item.PromotionCandidateAddedWayID.Value, PromotionCandidateAddedWayName = item.PromotionCandidateAddedWayName };
                    Candidate.CreatedBy = this.LoginIdentity;
                    Candidate.CreatedDate = DateTime.Now;
                    CandidatesList.Add(Candidate);
                }

                PromotionsRecordsBLL PromotionRecord = new PromotionsRecordsBLL() { PromotionsRecordsEmployees = CandidatesList };
                result = new Result();
                result.Entity = PromotionRecord;
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.Done.ToString();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public PromotionsRecordsBLL GetByPromotionRecordID(int PromotionRecordID)
        {
            PromotionsRecords PromotionRecord = new PromotionsRecordsDAL().GetByPromotionRecordID(PromotionRecordID);
            PromotionsRecordsBLL PromotionRecordBLL = new PromotionsRecordsBLL();
            if (PromotionRecord != null && PromotionRecord.PromotionPeriodID > 0)
                PromotionRecordBLL = new PromotionsRecordsBLL().MapPromotionRecord(PromotionRecord);

            return PromotionRecordBLL;
        }

        /// <summary>
        /// to refresh promotion records there are conditions:
        /// 1 - promotion record status must be open
        /// to refresh promotion record :
        /// 1 - Deleteing Jobs Vacancies that were assigned to promotion record before
        /// 2 - Deleting Qualifications that were assigned to promotion record before
        /// 3 - Deleting Employees that were assigned automatically to promotion record before
        /// 4 - Call again the jobs vacancies depend on job category and rank of promotion record and save
        /// 5 - Call again the candidates that should be candidated to promotion record and save
        /// 6 - Call again the qualification of candidates that will be assigned in promotion record and save
        /// </summary>
        /// <param name="PromotionRecordID"></param>
        /// <returns></returns>
        public Result RefreshPromotionRecord(int PromotionRecordID)
        {
            try
            {
                Result result = null;

                PromotionsRecordsBLL PromotionRecordBLL = new PromotionsRecordsBLL().GetByPromotionRecordID(PromotionRecordID);

                #region Validate if the promotion status is open or not
                if (PromotionRecordBLL.PromotionRecordStatusEnum != PromotionsRecordsStatusEnum.Open)
                {
                    result = this.GetResultByPromotionsRecordsStatus(PromotionsRecordsStatusEnum.Open);
                    return result;
                }
                #endregion

                #region Get candidates
                List<PromotionsRecordsEmployees> CandidatesList = new List<PromotionsRecordsEmployees>();
                result = this.GetCandidatesShouldBeInPromotionRecord(PromotionRecordBLL.JobCategory.JobCategoryID, PromotionRecordBLL.PromotionPeriod.PromotionPeriodID, PromotionRecordBLL.Rank.RankID, PromotionRecordBLL.PromotionRecordID);

                if (result.EnumMember != PromotionsRecordsValidationEnum.Done.ToString())
                    return result;

                PromotionRecordBLL.PromotionsRecordsEmployees = ((PromotionsRecordsBLL)result.Entity).PromotionsRecordsEmployees;

                foreach (var item in PromotionRecordBLL.PromotionsRecordsEmployees)
                {
                    CandidatesList.Add(new PromotionsRecordsEmployees()
                    {
                        PromotionRecordID = PromotionRecordID,

                        CurrentEmployeeCareerHistoryID = item.CurrentEmployeeCareer.EmployeeCareerHistoryID,
                        CurrentEmployeesCareersHistory = new EmployeesCareersHistory() { EmployeeCareerHistoryID = item.CurrentEmployeeCareer.EmployeeCareerHistoryID, EmployeeCodeID = item.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID },
                        CurrentJobDaysCount = item.CurrentJobDaysCount,
                        CurrentJobJoinDate = item.CurrentEmployeeCareer.JoinDate,

                        PreviousEmployeeCareerHistoryID = item.PreviousEmployeeCareer != null ? item.PreviousEmployeeCareer.EmployeeCareerHistoryID : (int?)null,
                        PreviousJobDaysCount = item.PreviousJobDaysCount,
                        PreviousJobJoinDate = item.PreviousEmployeeCareer != null ? item.PreviousEmployeeCareer.JoinDate : (DateTime?)null,

                        OnGoingServiceDaysCount = 0, // it will be calculated by preference
                        PriorServiceDaysCount = 0, // it will be calculated by preference

                        LastQualificationDegreeID = item.LastQualificationDegree.QualificationDegreeID != 0 ? item.LastQualificationDegree.QualificationDegreeID : (int?)null,
                        LastQualificationID = item.LastQualification.QualificationID != 0 ? item.LastQualification.QualificationID : (int?)null,
                        LastGeneralSpecializationID = item.LastGeneralSpecialization.GeneralSpecializationID != 0 ? item.LastGeneralSpecialization.GeneralSpecializationID : (int?)null,

                        PromotionCandidateAddedWayID = (int)item.PromotionCandidateAddedWay.PromotionCandidateAddedWayID,
                        IsIncluded = item.IsIncluded,
                        CreatedBy = this.LoginIdentity.EmployeeCodeID,
                        CreatedDate = DateTime.Now,
                    }); 
                }
                #endregion

                #region Get jobs vacancies
                List<PromotionsRecordsJobsVacancies> JobsVacanciesList = new List<PromotionsRecordsJobsVacancies>();
                foreach (var item in new OrganizationsJobsBLL().GetJobsVacancies(PromotionRecordBLL.JobCategory.JobCategoryID, PromotionRecordBLL.Rank.RankID))
                {
                    JobsVacanciesList.Add(new PromotionsRecordsJobsVacancies()
                    {
                        PromotionRecordID = PromotionRecordID,
                        OrganizationJobID = item.OrganizationJobID,
                        CreatedBy = this.LoginIdentity.EmployeeCodeID,
                        CreatedDate = DateTime.Now
                    });
                }

                if (JobsVacanciesList.Count == 0)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString();
                }
                #endregion

                #region Get distinct qualification of all candidates that will be entered in promotion record
                List<PromotionsRecordsQualificationsPoints> PromotionRecordQualificationsList = GetDistinctQualifications(CandidatesList, PromotionRecordID);
                CandidatesList.ForEach(x => x.CurrentEmployeesCareersHistory = null);
                #endregion

                #region Deleting old data
                //new PromotionsRecordsJobsVacanciesBLL() { LoginIdentity = this.LoginIdentity }.RemoveAllFromPromotionRecord(PromotionRecordID);
                //new PromotionsRecordsQualificationsPointsBLL() { LoginIdentity = this.LoginIdentity }.RemoveAllFromPromotionRecord(PromotionRecordID);
                //new PromotionsRecordsEmployeesBLL() { LoginIdentity = this.LoginIdentity }.RemoveAllFromPromotionRecord(PromotionRecordID);
                #endregion

                #region Save new data
                PromotionsRecords PromotionRecord = new PromotionsRecords()
                {
                    PromotionRecordID = PromotionRecordBLL.PromotionRecordID,
                    PromotionsRecordsJobsVacancies = JobsVacanciesList,
                    PromotionsRecordsQualificationsPoints = PromotionRecordQualificationsList,
                    PromotionsRecordsEmployees = CandidatesList
                };
                new PromotionsRecordsDAL().Refresh(PromotionRecord);

                #region Add log
                result = new PromotionsRecordsLogsBLL()
                {
                    PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecordBLL.PromotionRecordID },
                    PromotionRecordNo = PromotionRecordBLL.PromotionRecordNo,
                    PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.Refresh },
                    ActionDescription = string.Format(Globalization.PromotionRecordActionDescriptionOpenedText, CandidatesList.Count, JobsVacanciesList.Count),
                    LoginIdentity = this.LoginIdentity
                }.Add();
                #endregion
                //new PromotionsRecordsJobsVacanciesDAL().Insert(JobsVacanciesList);
                //new PromotionsRecordsQualificationsPointsDAL().Insert(PromotionRecordQualificationsList);
                //new PromotionsRecordsEmployeesDAL().Insert(CandidatesList);
                #endregion

                result = new Result();
                result.Entity = PromotionRecordBLL;
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.Done.ToString();

                return result;
            }
            catch
            {
                throw;
            }
        }

        public Result GetResultByPromotionsRecordsStatus(PromotionsRecordsStatusEnum PromotionRecordStatus)
        {
            Result result = null;
            // PromotionsRecordsBLL PromotionRecordBLL = new PromotionsRecordsBLL().GetByPromotionRecordID(PromotionRecordID);

            #region Validate if the promotion status open or not
            if (PromotionRecordStatus != PromotionsRecordsStatusEnum.Open)
            {
                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfPromotionIsNotOpen.ToString();
                return result;
            }
            #endregion

            return result;
        }

        /// <summary>
        /// this to get all promotions record under job category or under (job category and promotion period)
        /// </summary>
        /// <param name="JobCategoryID"></param>
        /// <param name="PromotionPeriodID"></param>
        /// <returns></returns>
        public List<PromotionsRecordsBLL> GetByJobCategory(int JobCategoryID, int? PromotionPeriodID = 0)
        {
            try
            {
                List<PromotionsRecords> PromotionsRecordsList = new PromotionsRecordsDAL().GetByJobCategoryID(JobCategoryID);
                List<PromotionsRecordsBLL> PromotionsRecordsBLLList = new List<PromotionsRecordsBLL>();
                foreach (var item in PromotionsRecordsList)
                {
                    PromotionsRecordsBLLList.Add(new PromotionsRecordsBLL().MapPromotionRecord(item));
                }

                return PromotionsRecordsBLLList.Where(x => (PromotionPeriodID != 0) ? x.PromotionPeriod.PromotionPeriodID == PromotionPeriodID : x.PromotionPeriod.PromotionPeriodID == x.PromotionPeriod.PromotionPeriodID).ToList();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// this to get distinct qualifications of candidates that will be included in promotion record
        /// this method is called in add method and Refresh method
        /// </summary>
        /// <param name="CandidatesList"></param>
        /// <returns></returns>
        private List<PromotionsRecordsQualificationsPoints> GetDistinctQualifications(List<PromotionsRecordsEmployees> CandidatesList, int PromotionRecordID = 0)
        {
            var DistinctQualifications = new EmployeesQualificationsDAL().GetEmployeesQualifications()
                .Where(x => CandidatesList.Any(c => c.IsIncluded == true && c.CurrentEmployeesCareersHistory.EmployeeCodeID == x.EmployeeCodeID))
                .Select(x => new { x.QualificationDegreeID, x.QualificationID, x.GeneralSpecializationID }).Distinct();

            //var DistinctQualifications = CandidatesList.Where(x => x.IsIncluded == true).Select(x => new { x.LastQualificationDegreeID, x.LastQualificationID, x.LastGeneralSpecializationID }).Distinct();
            List<PromotionsRecordsQualificationsPoints> PromotionRecordQualificationsList = new List<PromotionsRecordsQualificationsPoints>();
            foreach (var item in DistinctQualifications)
            {
                PromotionRecordQualificationsList.Add(new PromotionsRecordsQualificationsPoints()
                {
                    PromotionRecordID = PromotionRecordID, // we need this in refresh promotion record
                    QualificationDegreeID = item.QualificationDegreeID,
                    QualificationID = item.QualificationID,
                    GeneralSpecializationID = item.GeneralSpecializationID,
                    Points = 0, //default value
                    CreatedBy = this.LoginIdentity.EmployeeCodeID,
                    CreatedDate = DateTime.Now
                });
            }
            return PromotionRecordQualificationsList;
        }

        public Result UpdatePromotionDate()
        {
            Result result = null;
            PromotionsRecordsBLL PromotionRecordBLL = this.GetByPromotionRecordID(this.PromotionRecordID);
            if (this.PromotionDate < PromotionRecordBLL.PromotionRecordDate)
            {
                return new Result()
                {
                    EnumType = typeof(PromotionsRecordsValidationEnum),
                    EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionDateShouldBeLessThanPromotionRecordDate.ToString()
                };
            }

            PromotionsRecords PromotionRecord = new PromotionsRecords()
            {
                PromotionRecordID = this.PromotionRecordID,
                PromotionDate = this.PromotionDate,
                LastUpdatedBy = this.LoginIdentity.EmployeeCodeID,
                LastUpdatedDate = DateTime.Now
            };
            new PromotionsRecordsDAL().UpdatePromotionDate(PromotionRecord);

            #region Update TransactionStartDate in EmployeeCareerHistory
            EmployeesCareersHistoryBLL EmployeesCareersHistory = new EmployeesCareersHistoryBLL();
            List<PromotionsRecordsEmployees> PromotedEmployees = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordID(this.PromotionRecordID);
            foreach (var emp in PromotedEmployees)
            {
                if (emp.NewEmployeeCareerHistoryID.HasValue)
                {
                    EmployeesCareersHistory.UpdateTransactionStartDate(emp.NewEmployeeCareerHistoryID.Value,
                                                                        this.PromotionDate,
                                                                        this.LoginIdentity.EmployeeCodeID);
                }
            }
            #endregion

            #region Add log
            result = new PromotionsRecordsLogsBLL()
            {
                PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecordBLL.PromotionRecordID },
                PromotionRecordNo = PromotionRecordBLL.PromotionRecordNo,
                PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.PromotionDecisionCreation },
                ActionDescription = string.Empty,
                LoginIdentity = this.LoginIdentity
            }.Add();
            #endregion

            result = new Result()
            {
                Entity = this,
                EnumType = typeof(PromotionsRecordsValidationEnum),
                EnumMember = PromotionsRecordsValidationEnum.Done.ToString()
            };
            return result;
        }

        /// <summary>
        /// this is to check there are promotions records have relation with job category did not be installed
        /// </summary>
        /// <param name="JobCategoryID"></param>
        /// <param name="PromotionPeriodID"></param>
        /// <returns></returns>
        private Result AreThereOtherPromotionsRecordHasRelationNotInstalled(int JobCategoryID, int PromotionPeriodID)
        {
            try
            {
                Result result = null;
                List<JobsCategoriesBLL> JobsCategoriesBLLList = GetAllJobCategoriesByJobCategoryAndPromotionPeriod(JobCategoryID, PromotionPeriodID);

                List<PromotionsRecordsBLL> PromotionsRecordsList = this.GetPromotionsRecords().Where(x =>
                                                                                                            x.PromotionPeriod.PromotionPeriodID == PromotionPeriodID
                                                                                                            && (x.PromotionRecordStatusEnum == PromotionsRecordsStatusEnum.Open || x.PromotionRecordStatusEnum == PromotionsRecordsStatusEnum.Preferenced)
                                                                                                            && JobsCategoriesBLLList.Any(c => x.JobCategory.JobCategoryID == c.JobCategoryID)).ToList();

                if (PromotionsRecordsList.Count() > 0) // thats mean there are other promotion record has relation with the new promotion record in the same period but still were not installed
                {
                    result = new Result();
                    result.Entity = PromotionsRecordsList.FirstOrDefault(); // this is to show in frontend what is the promotion record information
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseThereArePormotionRecordsHasRelationNotInstalled.ToString();
                    return result;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  this is to check there are promotions records did not be installed
        /// </summary>
        /// <param name="JobCategoryID"></param>
        /// <param name="PromotionPeriodID"></param>
        /// <returns></returns>
        private Result AreThereOtherPromotionsRecordNotInstalled(List<PromotionsRecordsBLL> PromotionsRecordsList, int PromotionPeriodID, int ExceptPromotionRecordID = 0)
        {
            try
            {
                Result result = null;


                List<PromotionsRecordsBLL> PromotionsRecordsListObj = PromotionsRecordsList.Where(x => x.PromotionPeriod.PromotionPeriodID == PromotionPeriodID
                                                                                                      && (x.PromotionRecordStatusEnum == PromotionsRecordsStatusEnum.Open || x.PromotionRecordStatusEnum == PromotionsRecordsStatusEnum.Preferenced)
                                                                                                      && (ExceptPromotionRecordID != 0 ? x.PromotionRecordID != ExceptPromotionRecordID : x.PromotionRecordID == x.PromotionRecordID)
                                                                                                            ).ToList();

                if (PromotionsRecordsListObj.Count() > 0) // thats mean there are other promotion record in the same period but still were not installed
                {
                    result = new Result();
                    result.Entity = PromotionsRecordsListObj.FirstOrDefault(); // this is to show in frontend what is the promotion record information
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseThereArePormotionRecordsNotInstalled.ToString();
                    return result;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  this is to check there are promotions records did not be installed in other ranks
        /// </summary>
        /// <param name="PromotionsRecordsList"></param>
        /// <param name="PromotionPeriodID"></param>
        /// <param name="ExceptRankID"></param>
        /// <returns></returns>
        private Result AreThereOtherPromotionsRecordNotClosedInOtherRanks(List<PromotionsRecordsBLL> PromotionsRecordsList, int PromotionPeriodID, int ExceptRankID)
        {
            try
            {
                Result result = null;


                List<PromotionsRecordsBLL> PromotionsRecordsListObj = PromotionsRecordsList.Where(x => x.PromotionPeriod.PromotionPeriodID == PromotionPeriodID
                                                                                                      && (x.PromotionRecordStatusEnum != PromotionsRecordsStatusEnum.Closed)
                                                                                                      && (x.Rank.RankID != ExceptRankID)
                                                                                                            ).ToList();

                if (PromotionsRecordsListObj.Count() > 0) // thats mean there are other promotion record in the same period in other ranks but still were not closed
                {
                    result = new Result();
                    result.Entity = PromotionsRecordsListObj.FirstOrDefault(); // this is to show in frontend what is the promotion record information
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseThereArePormotionRecordsNotClosedInOtherRanks.ToString();
                    return result;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        internal List<JobsCategoriesBLL> GetAllJobCategoriesByJobCategoryAndPromotionPeriod(int JobCategoryID, int PromotionPeriodID)
        {
            List<PromotionsJobsCategoriesBLL> AssignedJobsCategoriesList = new PromotionsJobsCategoriesBLL().GetAssignedJobCategoriesByJobCategoryID(JobCategoryID).Where(x => x.PromotionPeriod.PromotionPeriodID == PromotionPeriodID).ToList();
            List<PromotionsJobsCategoriesBLL> MainJobsCategoriesList = new PromotionsJobsCategoriesBLL().GetMainJobCategoriesByAssignedJobCategoryID(JobCategoryID).Where(x => x.PromotionPeriod.PromotionPeriodID == PromotionPeriodID).ToList();

            List<JobsCategoriesBLL> JobsCategoriesBLLList = new List<JobsCategoriesBLL>();

            //AssignedJobsCategoriesList.Union(MainJobsCategoriesList);
            foreach (var item in MainJobsCategoriesList)
            {
                JobsCategoriesBLLList.Add(new JobsCategoriesBLL() { JobCategoryID = item.JobCategory.JobCategoryID });
            }
            foreach (var item in AssignedJobsCategoriesList)
            {
                JobsCategoriesBLLList.Add(new JobsCategoriesBLL() { JobCategoryID = item.AssignedJobCategory.JobCategoryID });
            }
            return JobsCategoriesBLLList;
        }

        public PromotionsRecordsBLL MapPromotionRecord(PromotionsRecords item)
        {
            PromotionsRecordsBLL PromotionRecordBLL = new PromotionsRecordsBLL();
            if (item != null)
            {
                PromotionRecordBLL.PromotionRecordID = item.PromotionRecordID;
                PromotionRecordBLL.PromotionRecordNo = item.PromotionRecordNo;
                PromotionRecordBLL.PromotionRecordDate = item.PromotionRecordDate;
                PromotionRecordBLL.PromotionDate = item.PromotionDate.HasValue ? item.PromotionDate.Value : DateTime.Now.Date;
                PromotionRecordBLL.PromotionRecordStatus = new PromotionsRecordsStatusBLL().MapPromotionRecordStatus(item.PromotionsRecordsStatus);
                PromotionRecordBLL.PromotionRecordStatusEnum = (PromotionsRecordsStatusEnum)item.PromotionsRecordsStatus.PromotionRecordStatusID;
                PromotionRecordBLL.Rank = new RanksBLL().MapRank(item.Ranks);
                PromotionRecordBLL.JobCategory = new JobsCategoriesBLL().MapJobCategory(item.JobsCategories);
                PromotionRecordBLL.PromotionPeriod = new PromotionsPeriodsBLL().MapPromotionPeriod(item.PromotionsPeriods);
                PromotionRecordBLL.CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(item.CreatedByNav);
                PromotionRecordBLL.CreatedDate = item.CreationDate;
            }
            return PromotionRecordBLL;
        }

        //public List<EmployeesCareersHistoryBLL> GetDeservedEmployeesInPromotion(int RankID, int JobCategoryID, int PromotionPeriodID, DateTime PromotionEndDate, int DaysCount)
        //{
        //    List<EmployeesCareersHistoryBLL> DeservedEmployeesBLLList = new List<EmployeesCareersHistoryBLL>();
        //    List<spGetDeservedEmployeesToBeIncludedPromotion_Result> DeservedEmployeesList = new EmployeesCareersHistoryDAL().GetDeservedEmployeesToBeIncludedPromotion(RankID, JobCategoryID, PromotionPeriodID).Where(x => PromotionEndDate >= x.JoinDate.AddDays(DaysCount)).ToList();
        //    foreach (var item in DeservedEmployeesList)
        //    {
        //        DeservedEmployeesBLLList.Add(new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item));
        //    }
        //    return DeservedEmployeesBLLList;
        //}

        /// <summary>
        /// There are the steps to close promotion record:
        /// 1- Check if PromotionRecord is installed or not. If not return.
        /// 2- Get PromotionRecordsEmployees that already installed and they will take new job vacancies for this promotion record. And Start Loop on them
        /// 3- Get PromotionRecordJobVacancy by PromotionRecordsEmployees.PromotionRecordJobVacancyID.
        /// 4- Get OrganizationJob by PromotionRecordJobVacancy.OrganizationJobID.
        /// 5- Update OrganizationJob set IsVacant=false, IsReserved=false
        /// 6- Get CurrentEmployeesCareersHistory to get EmployeeCodeID
        /// 7- Update IsActive= 0 for CurrentEmployeesCareersHistory That we got it from step 6
        /// 8- Get OrganizationsJobs of CurrentEmployeesCareersHistory And then update IsVacant= 1  
        /// 9- Add new EmployeesCareersHistory with :
        /// --1 EmployeeCodeID=CurrentEmployeesCareersHistory.EmployeeCodeID
        /// --2 CareerHistoryTypeID= 2
        /// --3 OrganizationJobID= PromotionsRecordsJobsVacancies.OrganizationJobID
        /// --4 CarrerDegreeID=(step 8).
        /// --5 JoinDate=DateTime.Now.
        /// --6 TransactionStartDate=DateTime.Now.
        /// --7 TransactionEndDate=null
        /// --8 IsActive= 1 .
        /// 10- BasicSalariesBLL.BasicSalary of the next rank with higher BasicSalary
        /// 11- Update PromotionsRecordsEmployees.NewEmployeeCareerHistory with the previeus add action.       
        /// 12- After finish work on all PromotionRecordsEmployees, Update PromotionsRecords with:
        /// --1 PromotionRecordStatusID=4
        /// --2 ClosingBy=LoginIdentity.EmployeeCodeID
        /// --3 ClosingTime=DateTime.Now
        /// --4 LastUpdatedBy=LoginIdentity.EmployeeCodeID
        /// --5 LastUpdatedDate=DateTime.Now       
        /// </summary>
        /// <param name="PromotionRecordID"></param>
        /// <returns>Result</returns>
        public Result Close(int PromotionRecordID)
        {
            Result result = null;
            PromotionsRecordsBLL PromotionRecord = new PromotionsRecordsBLL().GetByPromotionRecordID(PromotionRecordID);

            #region Validate if the promotion record status is installed or not
            if (PromotionRecord.PromotionRecordStatusEnum != PromotionsRecordsStatusEnum.Installed)
            {
                result = new Result();
                result.Entity = PromotionRecord;
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeInstalled.ToString();
                return result;
            }
            #endregion

            List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesList = new PromotionsRecordsEmployeesBLL()
                    .GetCandidatesAlreadyInstalled(PromotionRecordID);
            foreach (var PromotionRecordEmployee in PromotionsRecordsEmployeesList)
            {

                PromotionsRecordsJobsVacanciesBLL PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL()
                    .GetByPromotionRecordJobVacancyID(PromotionRecordEmployee.PromotionRecordJobVacancy.PromotionRecordJobVacancyID);
                PromotionRecordEmployee.PromotionRecordJobVacancy = PromotionRecordJobVacancy;//<========
                OrganizationsJobsBLL OrganizationJobVacancy = new OrganizationsJobsBLL()
                    .GetByOrganizationJobID(PromotionRecordJobVacancy.JobVacancy.OrganizationJobID);
                PromotionRecordEmployee.PromotionRecordJobVacancy.JobVacancy = OrganizationJobVacancy;//<=====
                OrganizationJobVacancy.IsVacant = false;
                OrganizationJobVacancy.IsReserved = false;
                OrganizationJobVacancy.LastUpdatedBy = LoginIdentity;
                OrganizationJobVacancy.LastUpdatedDate = DateTime.Now;

                EmployeesCareersHistoryBLL CurrentEmployeeCareerHistory = new EmployeesCareersHistoryBLL()
                    .GetActiveByEmployeeCareerHistoryID(PromotionRecordEmployee.CurrentEmployeeCareer.EmployeeCareerHistoryID);
                CurrentEmployeeCareerHistory.IsActive = false;
                CurrentEmployeeCareerHistory.LastUpdatedBy = LoginIdentity;
                CurrentEmployeeCareerHistory.LastUpdatedDate = DateTime.Now;
                PromotionRecordEmployee.CurrentEmployeeCareer = CurrentEmployeeCareerHistory;//<====
                OrganizationsJobsBLL CurrentOrganizationJob = new OrganizationsJobsBLL().GetByOrganizationJobID(CurrentEmployeeCareerHistory.OrganizationJob.OrganizationJobID);
                CurrentOrganizationJob.IsVacant = true;
                CurrentOrganizationJob.LastUpdatedBy = LoginIdentity;
                CurrentOrganizationJob.LastUpdatedDate = DateTime.Now;
                PromotionRecordEmployee.CurrentEmployeeCareer.OrganizationJob = CurrentOrganizationJob;//<======
                BasicSalariesBLL NextBasicSalary = new BasicSalariesBLL().GetGreaterBasicSalaryOfNextRank(CurrentOrganizationJob.Rank.RankID, CurrentEmployeeCareerHistory.CareerDegree.CareerDegreeID);

                EmployeesCareersHistoryBLL NewEmployeeCareerHistory = new EmployeesCareersHistoryBLL();
                NewEmployeeCareerHistory.EmployeeCode = CurrentEmployeeCareerHistory.EmployeeCode;
                NewEmployeeCareerHistory.CareerHistoryType = new CareersHistoryTypesBLL() { CareerHistoryTypeID = (int)CareersHistoryTypesEnum.Promotion };
                NewEmployeeCareerHistory.OrganizationJob = OrganizationJobVacancy;
                NewEmployeeCareerHistory.JoinDate = DateTime.Now;
                NewEmployeeCareerHistory.CareerDegree = new CareersDegreesBLL() { CareerDegreeID = NextBasicSalary.CareerDegree.CareerDegreeID };
                NewEmployeeCareerHistory.TransactionStartDate = DateTime.Now;
                NewEmployeeCareerHistory.CreatedBy = LoginIdentity;
                NewEmployeeCareerHistory.CreatedDate = DateTime.Now;
                NewEmployeeCareerHistory.IsActive = true;

                PromotionRecordEmployee.NewEmployeeCareer = NewEmployeeCareerHistory;//<=====
            }
            PromotionRecord.PromotionRecordStatus = new PromotionsRecordsStatusBLL().GetByPromotionRecordStatusID((int)PromotionsRecordsStatusEnum.Closed);
            PromotionRecord.ClosingBy = LoginIdentity;
            PromotionRecord.ClosingTime = DateTime.Now;
            PromotionRecord.LastUpdatedBy = LoginIdentity;
            PromotionRecord.LastUpdatedDate = DateTime.Now;
            PromotionRecord.PromotionsRecordsEmployees = new List<PromotionsRecordsEmployeesBLL>();
            PromotionRecord.PromotionsRecordsEmployees.AddRange(PromotionsRecordsEmployeesList);

            new PromotionsRecordsDAL().Close(ConvertBllToEntity(PromotionRecord));

            // No need to call break assigning from here, because it called from CREATE employee career history (TASK 310)
            //// as TASK # 226 - Needs to Finish / Break LastAssigningRecord for all promoted employees
            //foreach (var emp in PromotionsRecordsEmployeesList)
            //{
            //    new BaseAssigningsBLL() { LoginIdentity = LoginIdentity }.BreakLastAssigning(
            //                                    emp.CurrentEmployeeCareer.EmployeeCareerHistoryID,
            //                                    emp.NewEmployeeCareer.TransactionStartDate,
            //                                    AssigningsReasonsEnum.EmployeePromoted);
            //}

            #region Add log
            result = new PromotionsRecordsLogsBLL()
            {
                PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecord.PromotionRecordID },
                PromotionRecordNo = PromotionRecord.PromotionRecordNo,
                PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.Close },
                ActionDescription = string.Empty,
                LoginIdentity = LoginIdentity
            }.Add();
            #endregion

            result = new Result();
            result.Entity = PromotionRecord;
            result.EnumType = typeof(PromotionsRecordsValidationEnum);
            result.EnumMember = PromotionsRecordsValidationEnum.Done.ToString();
            return result;
        }

        private PromotionsRecords ConvertBllToEntity(PromotionsRecordsBLL PromotionsRecordsBLL)
        {
            PromotionsRecords PromotionRecord = new PromotionsRecords();
            PromotionRecord.PromotionRecordID = PromotionsRecordsBLL.PromotionRecordID;
            PromotionRecord.PromotionRecordStatusID = (int)PromotionsRecordsBLL.PromotionRecordStatus.PromotionRecordStatusID;
            PromotionRecord.ClosingBy = PromotionsRecordsBLL.ClosingBy.EmployeeCodeID;
            PromotionRecord.ClosingTime = PromotionsRecordsBLL.ClosingTime;
            PromotionRecord.LastUpdatedBy = PromotionsRecordsBLL.LastUpdatedBy.EmployeeCodeID;
            PromotionRecord.LastUpdatedDate = PromotionsRecordsBLL.ClosingTime;
            List<HCMDAL.PromotionsRecordsEmployees> PromotionsRecordsEmployeesLst = new List<PromotionsRecordsEmployees>();
            foreach (var PromotionRecordEmployeeBLL in PromotionsRecordsBLL.PromotionsRecordsEmployees)
            {
                HCMDAL.PromotionsRecordsEmployees PromotionsRecordsEmployee = new PromotionsRecordsEmployees();
                PromotionsRecordsEmployee.PromotionRecordEmployeeID = PromotionRecordEmployeeBLL.PromotionRecordEmployeeID;
                PromotionsRecordsJobsVacancies PromotionRecordJobVacancy = new HCMDAL.PromotionsRecordsJobsVacancies() { PromotionRecordJobVacancyID = PromotionRecordEmployeeBLL.PromotionRecordJobVacancy.PromotionRecordJobVacancyID };
                OrganizationsJobs OrganizationJobVacancy = new OrganizationsJobs() { OrganizationJobID = PromotionRecordEmployeeBLL.PromotionRecordJobVacancy.JobVacancy.OrganizationJobID };
                OrganizationJobVacancy.IsVacant = PromotionRecordEmployeeBLL.PromotionRecordJobVacancy.JobVacancy.IsVacant;
                OrganizationJobVacancy.IsReserved = PromotionRecordEmployeeBLL.PromotionRecordJobVacancy.JobVacancy.IsReserved;
                OrganizationJobVacancy.LastUpdatedBy = PromotionRecordEmployeeBLL.PromotionRecordJobVacancy.JobVacancy.LastUpdatedBy.EmployeeCodeID;
                OrganizationJobVacancy.LastUpdatedDate = PromotionRecordEmployeeBLL.PromotionRecordJobVacancy.JobVacancy.LastUpdatedDate;
                PromotionRecordJobVacancy.OrganizationsJobs = OrganizationJobVacancy;
                PromotionsRecordsEmployee.PromotionsRecordsJobsVacancies = PromotionRecordJobVacancy;


                HCMDAL.EmployeesCareersHistory CurrentEmployeeCareerHistory = new EmployeesCareersHistory() { EmployeeCareerHistoryID = PromotionRecordEmployeeBLL.CurrentEmployeeCareer.EmployeeCareerHistoryID };
                CurrentEmployeeCareerHistory.IsActive = PromotionRecordEmployeeBLL.CurrentEmployeeCareer.IsActive;
                CurrentEmployeeCareerHistory.LastUpdatedBy = PromotionRecordEmployeeBLL.CurrentEmployeeCareer.LastUpdatedBy.EmployeeCodeID;
                CurrentEmployeeCareerHistory.LastUpdatedDate = PromotionRecordEmployeeBLL.CurrentEmployeeCareer.LastUpdatedDate;

                HCMDAL.OrganizationsJobs CurrentOrganizationJob = new OrganizationsJobs() { OrganizationJobID = PromotionRecordEmployeeBLL.CurrentEmployeeCareer.OrganizationJob.OrganizationJobID };
                CurrentOrganizationJob.IsVacant = PromotionRecordEmployeeBLL.CurrentEmployeeCareer.OrganizationJob.IsVacant;
                CurrentOrganizationJob.LastUpdatedBy = PromotionRecordEmployeeBLL.CurrentEmployeeCareer.OrganizationJob.LastUpdatedBy.EmployeeCodeID;
                CurrentOrganizationJob.LastUpdatedDate = PromotionRecordEmployeeBLL.CurrentEmployeeCareer.OrganizationJob.LastUpdatedDate;
                CurrentEmployeeCareerHistory.OrganizationsJobs = CurrentOrganizationJob;
                PromotionsRecordsEmployee.CurrentEmployeesCareersHistory = CurrentEmployeeCareerHistory;

                EmployeesCareersHistory NewEmployeeCareerHistory = new EmployeesCareersHistory();
                NewEmployeeCareerHistory.EmployeeCodeID = PromotionRecordEmployeeBLL.NewEmployeeCareer.EmployeeCode.EmployeeCodeID;
                NewEmployeeCareerHistory.CareerHistoryTypeID = PromotionRecordEmployeeBLL.NewEmployeeCareer.CareerHistoryType.CareerHistoryTypeID;
                NewEmployeeCareerHistory.OrganizationJobID = PromotionRecordEmployeeBLL.NewEmployeeCareer.OrganizationJob.OrganizationJobID;
                NewEmployeeCareerHistory.JoinDate = PromotionRecordEmployeeBLL.NewEmployeeCareer.JoinDate;
                NewEmployeeCareerHistory.CareerDegreeID = PromotionRecordEmployeeBLL.NewEmployeeCareer.CareerDegree.CareerDegreeID;
                NewEmployeeCareerHistory.TransactionStartDate = PromotionRecordEmployeeBLL.NewEmployeeCareer.TransactionStartDate;
                NewEmployeeCareerHistory.CreatedBy = (int)PromotionRecordEmployeeBLL.NewEmployeeCareer.CreatedBy.EmployeeCodeID;
                NewEmployeeCareerHistory.CreatedDate = PromotionRecordEmployeeBLL.NewEmployeeCareer.CreatedDate;
                NewEmployeeCareerHistory.IsActive = PromotionRecordEmployeeBLL.NewEmployeeCareer.IsActive;
                PromotionsRecordsEmployee.NewEmployeesCareersHistory = NewEmployeeCareerHistory;


                PromotionsRecordsEmployeesLst.Add(PromotionsRecordsEmployee);
                PromotionRecord.PromotionsRecordsEmployees = PromotionsRecordsEmployeesLst;
            }

            //--=======   



            return PromotionRecord;
        }
    }
}