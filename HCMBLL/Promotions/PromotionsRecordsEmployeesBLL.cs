using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public partial class PromotionsRecordsEmployeesBLL : CommonEntity, IEntity
    {
        public int PromotionRecordEmployeeID { get; set; }

        public PromotionsRecordsBLL PromotionRecord { get; set; }

        public PromotionsRecordsJobsVacanciesBLL PromotionRecordJobVacancy { get; set; }

        public EmployeesCareersHistoryBLL CurrentEmployeeCareer { get; set; }

        public int CurrentJobDaysCount { get; set; }

        /// <summary>
        /// Initial version: previously we'r using 'this.CurrentJobDaysCount' this property count substracted 'TotalExcludedDaysCount'  //{ get { return this.CurrentJobDaysCount; } }
        /// Change 1: Dated: 26-04-2020
        /// Change 1 Details: Now CurrentJobSeniorityDays should calculate by 'CurrentJobJoinDate' & Promotion End Date
        /// </summary>
        public int CurrentJobSeniorityDays { get { return int.Parse(((this.PromotionRecord.PromotionPeriod.PromotionEndDate - this.CurrentJobJoinDate.Value).TotalDays).ToString()); } }

        public int JobSeniorityMaxMonths { get { return 96; } }

        public DateTime? CurrentJobJoinDate { get; set; }

        public DateTime? PreviousJobJoinDate { get; set; }

        /// <summary>
        /// initial version: to get CurrentJobSeniorityMonth divided by 30 days, but there is some error in months calculation
        /// Change 1: Dated: 28-Jan-2020
        /// now to get CurrentJobSeniorityMonth divided by 29.5 and apply floor function to get proper month count
        /// </summary>
        public int CurrentJobSeniorityMonths
        {
            get
            {
                decimal months = (this.CurrentJobDaysCount > 0 && (this.CurrentJobDaysCount - this.DaysCountBetweenRanksToPromotion) > 0) ? (this.CurrentJobDaysCount - this.DaysCountBetweenRanksToPromotion) / this.DaysCountInUmAlquraMonth : 0;
                return Convert.ToInt32(Math.Floor(months > this.JobSeniorityMaxMonths ? this.JobSeniorityMaxMonths : months));
            }
        }

        public EmployeesCareersHistoryBLL PreviousEmployeeCareer { get; set; }

        public int PreviousJobDaysCount { get; set; }

        public int LastJobSeniorityDays { get { return this.PreviousJobDaysCount; } }

        public EmployeesCareersHistoryBLL NewEmployeeCareer { get; set; }

        public QualificationsDegreesBLL LastQualificationDegree { get; set; }

        public QualificationsBLL LastQualification { get; set; }

        public GeneralSpecializationsBLL LastGeneralSpecialization { get; set; }

        public int OnGoingServiceDaysCount { get; set; }

        public int PriorServiceDaysCount { get; set; }

        public int TotalExperience { get { return this.OnGoingServiceDaysCount + this.PriorServiceDaysCount; } }

        public decimal MaxEducationPoints { get { return 12; } }

        public decimal EducationPoints { get; set; }

        public decimal SeniorityPoints { get; set; }

        public decimal ActualTaskPerformancePoints { get; set; }

        /// <summary>
        /// Initial: Get Two years evaluations mentioned in this property EvaluationYears
        /// Dated: 18-02-2020
        /// This property is no longer in used because now we are using last two years evaluations
        /// </summary>
        //public int[] EvaluationYears { get { return new[] { 1438, 1439 }; } }

        /// <summary>
        /// Dated: 18-02-2020 (initial)
        /// This Property is used to get last N years, from employees Evaluation table.
        /// </summary>
        public int EvaluationYearNCountToIncludeInPromotionPreference { get { return 2; } }

        public decimal EvaluationPoints { get; set; }

        public decimal TotalPoints { get { return this.EducationPoints + this.SeniorityPoints + this.EvaluationPoints + this.ActualTaskPerformancePoints; } }

        public string ManualAddedReason { get; set; }

        public PromotionCandidateAddedWayBLL PromotionCandidateAddedWay { get; set; }

        public PromotionsCandidatesAddedWaysEnum PromotionCandidateAddedWayEnum { get; set; }

        public bool IsIncluded { get; set; }

        public int DaysCountBetweenRanksToPromotion
        {
            get
            {
                return this.DaysCountInUmAlquraYear * 3;
            }
        }

        public bool? IsDeserveExtraBonus { get; set; }

        public bool IsRemovedAfterIncluding { get; set; }

        public string RemovingReason { get; set; }

        public EmployeesCodesBLL RemovingBy { get; set; }

        public bool? IsApproved { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public EmployeesCodesBLL ApprovedBy { get; set; }

        public bool HasSamePoints { get; set; }

        public bool HasSameLastQualification { get; set; }

        public bool HasSameCurrentJobSeniority { get; set; }

        public bool HasSameLastJobSeniority { get; set; }

        public bool HasSameTotalExperience { get; set; }

        public bool HasSameExamResult { get; set; }

        public PromotionsDecisionsBLL PromotionDecision { get; set; }

        /// <summary>
        /// this property used to check if this record effected after removeEmployee from PromotionRecordEmployee 
        /// </summary>
        public bool IsRedistributeJob { get; set; }

        public int AbsentDays { get; set; }

        public List<PromotionsRecordsEmployeesEducationsDetailsBLL> EducationPointsDetail { get; set; }

        public List<PromotionsRecordsEmployeesEvaluationsDetailsBLL> EvaluationPointsDetail { get; set; }

        public List<PromotionsRecordsEmployeesSeniorityDetailsBLL> SeniorityPointsDetail { get; set; }

        //public List<PromotionsRecordsEmployeesByExamsResultsBLL> ByExamsResultsDetail { get; set; }        

        public decimal? ByExamResult { get; set; }

        public List<PromotionsRecordsEmployeesBLL> GetPromotionsRecordsEmployees()
        {
            List<PromotionsRecordsEmployees> PromotionsRecordsEmployeesList = new PromotionsRecordsEmployeesDAL().GetPromotionsRecordsEmployees();
            List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesBLLList = new List<PromotionsRecordsEmployeesBLL>();
            if (PromotionsRecordsEmployeesList.Count > 0)
                foreach (var item in PromotionsRecordsEmployeesList)
                    PromotionsRecordsEmployeesBLLList.Add(new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(item));

            return PromotionsRecordsEmployeesBLLList;
        }

        public List<PromotionsRecordsEmployeesBLL> GetByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                List<PromotionsRecordsEmployees> PromotionsRecordsEmployeesList = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordID(PromotionRecordID);
                List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesBLLList = new List<PromotionsRecordsEmployeesBLL>();
                if (PromotionsRecordsEmployeesList.Count > 0)
                    foreach (var item in PromotionsRecordsEmployeesList)
                        PromotionsRecordsEmployeesBLLList.Add(new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(item));

                return PromotionsRecordsEmployeesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployeesBLL> GetIncludedByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                List<PromotionsRecordsEmployees> PromotionsRecordsEmployeesList = new PromotionsRecordsEmployeesDAL().GetIncludedByPromotionRecordID(PromotionRecordID);
                List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesBLLList = new List<PromotionsRecordsEmployeesBLL>();
                if (PromotionsRecordsEmployeesList.Count > 0)
                    foreach (var item in PromotionsRecordsEmployeesList)
                        PromotionsRecordsEmployeesBLLList.Add(new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(item));

                return PromotionsRecordsEmployeesBLLList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// this to delete all employees under promotion record
        /// </summary>
        /// <param name="PromotionRecordID"></param>
        /// <returns></returns>
        public int RemoveAllFromPromotionRecord(int PromotionRecordID)
        {
            try
            {
                return new PromotionsRecordsEmployeesDAL().DeleteByPromotionRecordID(PromotionRecordID, this.LoginIdentity.EmployeeCodeID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  there are conditions to allow adding employee in the promotion record manually :
        /// 1 - The candidate current rank must be previous rank of new rank that he will be promoted to it.
        /// 2 - The candidate spent 3 years (normal employee) or 4 years (not normal employee) in his current job.
        /// 3 - The last evaluation of candidate must be not weak or not accepted.
        /// 4 - The Candidate should not be in other promotions records and this records did not be installed or closed.
        /// 5 - The candidate should not reserve job or be promoted in tha same promotion period.
        /// 6 - The promotion record must be opened still.
        /// </summary>
        /// <param name="this.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID"></param>
        /// <param name="this.PromotionRecord.PromotionRecordID"></param>
        /// <returns>Result</returns>
        public Result AddCandidateManually()
        {
            try
            {
                Result result = null;
                PromotionsRecordsBLL PromotionRecord = new PromotionsRecordsBLL().GetByPromotionRecordID(this.PromotionRecord.PromotionRecordID);
                RanksBLL PreviousRank = new RanksBLL().GetPreviousRankByRankID(PromotionRecord.Rank.RankID);
                EmployeesCareersHistoryBLL CurrentEmployeeJob = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(this.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID);
                EmployeesCareersHistoryBLL PreviousEmployeeJob = new EmployeesCareersHistoryBLL().GetEmployeePreviousJob(this.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID);
                EmployeesCareersHistoryBLL HiringRecord = new EmployeesCareersHistoryBLL().GetHiringRecordByEmployeeCodeID(this.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID);
                List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesList = new EmployeesCodesBLL().GetPromotionRecordsByEmployeeCodeID(this.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID).Where(x => x.IsIncluded == true).ToList();
                EvaluationPointsEnum[] EvalExcludedFromPromotion = new EvaluationPointsBLL().GetEvalutionPointsExcludedFromPromotion();

                #region Validate if candidate or manual added reason are not passed
                if (CurrentEmployeeJob.EmployeeCode == null || string.IsNullOrEmpty(this.ManualAddedReason))
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                    result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateAndReasonAreRequiredToAdding.ToString();
                    return result;
                }
                #endregion

                #region Validate if the rank is not right
                if (CurrentEmployeeJob.OrganizationJob.Rank.RankID != PreviousRank.RankID)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateRankNotDeserveToPromote.ToString();
                    return result;
                }
                #endregion

                #region Validate if the candidate job period was completed or not
                int NetDays = this.GetCandidatePeriodAfterExcludingExceptionsPeriods(CurrentEmployeeJob, PromotionRecord.PromotionPeriod.PromotionEndDate);
                if (!this.IsCandidateCompletedPeriodAfterExcluding(NetDays))
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                    result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateJobPeriodNotCompleted.ToString();
                    return result;
                }

                if (CurrentEmployeeJob.OrganizationJob.Rank.RankID != PreviousRank.RankID)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                    result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateRankNotDeserveToPromote.ToString();
                    return result;
                }
                #endregion

                #region Validate if last candidate evaluation is Weak or not
                if (!this.IsLastCandidateEvaluationValidToBeIncluded(CurrentEmployeeJob, EvalExcludedFromPromotion))
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                    result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfLastCandidateEvaluationWeak.ToString();
                    return result;
                }
                #endregion

                #region Validate if the candidate has hiring record or not
                if (HiringRecord == null)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                    result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateHiringRecordNotExists.ToString();
                    return result;
                }
                #endregion

                #region Validate if the candidate in other promotions records and this promotions records did not be installed or closed
                PromotionsRecordsEmployeesBLL PromotionRecordEmployeeObj = PromotionsRecordsEmployeesList.FirstOrDefault(x => x.PromotionRecord.PromotionRecordStatusEnum == PromotionsRecordsStatusEnum.Open || x.PromotionRecord.PromotionRecordStatusEnum == PromotionsRecordsStatusEnum.Preferenced);
                if (PromotionRecordEmployeeObj != null)
                {
                    result = new Result();
                    result.Entity = PromotionRecordEmployeeObj;
                    result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                    result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateAlreadyInPromotionRecordNotInstalled.ToString();
                    return result;
                }
                #endregion

                #region Validate if the candidate already reserved a job vacancy or was promoted in tha same promotion period
                PromotionRecordEmployeeObj = PromotionsRecordsEmployeesList.FirstOrDefault(x => (x.PromotionRecord.PromotionRecordStatusEnum != PromotionsRecordsStatusEnum.Open)
                                                                                            && x.PromotionRecordJobVacancy != null
                                                                                            && x.PromotionRecord.PromotionPeriod.PromotionPeriodID == PromotionRecord.PromotionPeriod.PromotionPeriodID); // that means he already reserved a job vacancy or was promoted in the same period
                if (PromotionRecordEmployeeObj != null)
                {
                    result = new Result();
                    result.Entity = PromotionRecordEmployeeObj;
                    result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                    result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateAlreadyReservedJobVacancy.ToString();
                    return result;
                }
                #endregion

                #region Validate if the promotion record still opened or not
                if (PromotionRecord.PromotionRecordStatusEnum != PromotionsRecordsStatusEnum.Open)
                {
                    result = new Result();
                    result.Entity = PromotionRecord;
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfPromotionIsNotOpen.ToString();
                    return result;
                }
                #endregion

                #region Get last candidate qualification
                EmployeesQualificationsBLL LastEmployeeQualification = new EmployeesQualificationsBLL().GetLastEmployeeQualification(CurrentEmployeeJob.EmployeeCode.EmployeeCodeID);
                if (LastEmployeeQualification == null)
                    LastEmployeeQualification = new EmployeesQualificationsBLL();
                #endregion

                #region Adding candidate
                PromotionsRecordsEmployees PromotionRecordEmployee = new PromotionsRecordsEmployees()
                {
                    PromotionRecordID = this.PromotionRecord.PromotionRecordID,

                    CurrentEmployeeCareerHistoryID = CurrentEmployeeJob.EmployeeCareerHistoryID,
                    CurrentJobDaysCount = GetCandidatePeriodAfterExcludingExceptionsPeriods(CurrentEmployeeJob, PromotionRecord.PromotionPeriod.PromotionEndDate),
                    CurrentJobJoinDate = CurrentEmployeeJob.JoinDate,

                    PreviousEmployeeCareerHistoryID = CurrentEmployeeJob.EmployeeCareerHistoryID,
                    PreviousJobDaysCount = PreviousEmployeeJob != null ? int.Parse((CurrentEmployeeJob.JoinDate.Date - PreviousEmployeeJob.JoinDate.Date).TotalDays.ToString()) : 0,
                    PreviousJobJoinDate = PreviousEmployeeJob != null ? PreviousEmployeeJob.JoinDate : (DateTime?)null,

                    LastQualificationDegreeID = LastEmployeeQualification.QualificationDegree.QualificationDegreeID != 0 ? LastEmployeeQualification.QualificationDegree.QualificationDegreeID : (int?)null,
                    LastQualificationID = LastEmployeeQualification.Qualification.QualificationID != 0 ? LastEmployeeQualification.Qualification.QualificationID : (int?)null,
                    LastGeneralSpecializationID = LastEmployeeQualification.GeneralSpecialization.GeneralSpecializationID != 0 ? LastEmployeeQualification.GeneralSpecialization.GeneralSpecializationID : (int?)null,

                    //OnGoingServiceDaysCount = HiringRecord != null ? int.Parse((PromotionRecordBLL.PromotionPeriod.PromotionEndDate.Date - HiringRecord.JoinDate.Date).TotalDays.ToString()) : 0,
                    OnGoingServiceDaysCount = 0,  // it will be calculated by preference
                    PriorServiceDaysCount = 0, // it will be calculated by preference

                    PromotionCandidateAddedWayID = (int)PromotionsCandidatesAddedWaysEnum.Manual,
                    ManualAddedReason = this.ManualAddedReason,
                    IsIncluded = true,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID,
                    CreatedDate = DateTime.Now,
                };
                this.PromotionRecordEmployeeID = new PromotionsRecordsEmployeesDAL().Insert(PromotionRecordEmployee);

                #region Add log
                result = new PromotionsRecordsLogsBLL()
                {
                    PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecord.PromotionRecordID, },
                    PromotionRecordNo = PromotionRecord.PromotionRecordNo,
                    PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.AddingCandidateManually },
                    ActionDescription = string.Format(Globalization.PromotionRecordActionDescriptionAddingCandidateManuallyText, CurrentEmployeeJob.EmployeeCode.EmployeeCodeNo, CurrentEmployeeJob.EmployeeCode.Employee.EmployeeNameAr),
                    LoginIdentity = this.LoginIdentity
                }.Add();
                #endregion

                #endregion

                #region Adding the all qualification of the candidate in promotion record qualifications if this qualification did not be added before
                List<PromotionsRecordsQualificationsPointsBLL> PromotionsRecordsQualificationsPointsList = new PromotionsRecordsQualificationsPointsBLL().GetByPromotionRecordID(this.PromotionRecord.PromotionRecordID);
                //&& x.GeneralSpecialization == LastEmployeeQualification.GeneralSpecialization);

                PromotionsRecordsQualificationsPointsBLL PromotionRecordQualificationPoint;
                List<EmployeesQualificationsBLL> EmployeesQualificationsBLLList = new EmployeesQualificationsBLL().GetEmployeeQualificationByEmployeeCodeID(CurrentEmployeeJob.EmployeeCode.EmployeeCodeID);

                var DistinctQualifications = EmployeesQualificationsBLLList
                .Where(x => !PromotionsRecordsQualificationsPointsList.Any(c => x.QualificationDegree.QualificationDegreeID == c.QualificationDegree.QualificationDegreeID
                                                                            && x.Qualification.QualificationID == c.Qualification.QualificationID
                                                                            && x.GeneralSpecialization.GeneralSpecializationID == c.GeneralSpecialization.GeneralSpecializationID
                                                                          ))
                //.Where(x => PromotionsRecordsQualificationsPointsList.Any(c => (x.QualificationDegree.QualificationDegreeID != c.QualificationDegree.QualificationDegreeID 
                //                                                                && (x.Qualification != null) ? x.Qualification.QualificationID != c.Qualification.QualificationID : x.Qualification.QualificationID != x.Qualification.QualificationID
                //                                                                && (x.GeneralSpecialization != null) ? x.GeneralSpecialization.GeneralSpecializationID != c.GeneralSpecialization.GeneralSpecializationID : x.GeneralSpecialization.GeneralSpecializationID != x.GeneralSpecialization.GeneralSpecializationID)
                //                                                          ))
                .Select(x => new { x.QualificationDegree, x.Qualification, x.GeneralSpecialization }).Distinct();
                foreach (var item in DistinctQualifications)
                {
                    PromotionRecordQualificationPoint = new PromotionsRecordsQualificationsPointsBLL();
                    PromotionRecordQualificationPoint.QualificationDegree = item.QualificationDegree;
                    PromotionRecordQualificationPoint.Qualification = item.Qualification;
                    PromotionRecordQualificationPoint.GeneralSpecialization = item.GeneralSpecialization;
                    PromotionRecordQualificationPoint.PromotionRecord = this.PromotionRecord;
                    PromotionRecordQualificationPoint.LoginIdentity = this.LoginIdentity;
                    new PromotionsRecordsQualificationsPointsBLL().Add(PromotionRecordQualificationPoint);
                }


                //foreach (var item in EmployeesQualificationsBLLList)
                //{
                //    if (!PromotionsRecordsQualificationsPointsList.Any(x => x.QualificationDegree.QualificationDegreeID == item.QualificationDegree.QualificationDegreeID
                //                                                        && x.Qualification.QualificationID == item.Qualification.QualificationID
                //                                                        && x.GeneralSpecialization.GeneralSpecializationID == item.GeneralSpecialization.GeneralSpecializationID))
                //    {

                //        PromotionRecordQualificationPoint = new PromotionsRecordsQualificationsPointsBLL();
                //        PromotionRecordQualificationPoint.QualificationDegree = item.QualificationDegree;
                //        PromotionRecordQualificationPoint.Qualification = item.Qualification;
                //        PromotionRecordQualificationPoint.GeneralSpecialization = item.GeneralSpecialization;
                //        PromotionRecordQualificationPoint.PromotionRecord = this.PromotionRecord;
                //        PromotionRecordQualificationPoint.LoginIdentity = this.LoginIdentity;
                //        new PromotionsRecordsQualificationsPointsBLL().Add(PromotionRecordQualificationPoint);
                //    }
                //}



                //PromotionsRecordsQualificationsPointsBLL PromotionRecordQualificationPoint = PromotionsRecordsQualificationsPointsList.FirstOrDefault(x => x.QualificationDegree == LastEmployeeQualification.QualificationDegree
                //                                                                                                                                        && ((LastEmployeeQualification.Qualification != null) ? x.Qualification.QualificationID == LastEmployeeQualification.Qualification.QualificationID : x.Qualification.QualificationID != x.Qualification.QualificationID)
                //                                                                                                                                        && ((LastEmployeeQualification.GeneralSpecialization != null) ? x.GeneralSpecialization.GeneralSpecializationID == LastEmployeeQualification.GeneralSpecialization.GeneralSpecializationID : x.GeneralSpecialization.GeneralSpecializationID != x.GeneralSpecialization.GeneralSpecializationID));
                //  
                //if (LastEmployeeQualification.QualificationDegree != null)
                //{
                //    PromotionsRecordsQualificationsPointsBLL PromotionRecordQualificationPoint = PromotionsRecordsQualificationsPointsList.FirstOrDefault(x => x.QualificationDegree.QualificationDegreeID == LastEmployeeQualification.QualificationDegree.QualificationDegreeID
                //                                                                                                                                                          && ((LastEmployeeQualification.Qualification != null) ? x.Qualification.QualificationID == LastEmployeeQualification.Qualification.QualificationID : x.Qualification.QualificationID != x.Qualification.QualificationID)
                //                                                                                                                                                          && ((LastEmployeeQualification.GeneralSpecialization != null) ? x.GeneralSpecialization.GeneralSpecializationID == LastEmployeeQualification.GeneralSpecialization.GeneralSpecializationID : x.GeneralSpecialization.GeneralSpecializationID != x.GeneralSpecialization.GeneralSpecializationID));

                //    if (PromotionRecordQualificationPoint == null)
                //    {
                //        PromotionRecordQualificationPoint = new PromotionsRecordsQualificationsPointsBLL();
                //        PromotionRecordQualificationPoint.QualificationDegree = LastEmployeeQualification.QualificationDegree;
                //        PromotionRecordQualificationPoint.Qualification = LastEmployeeQualification.Qualification;
                //        PromotionRecordQualificationPoint.GeneralSpecialization = LastEmployeeQualification.GeneralSpecialization;
                //        PromotionRecordQualificationPoint.PromotionRecord = this.PromotionRecord;
                //        PromotionRecordQualificationPoint.LoginIdentity = this.LoginIdentity;
                //        new PromotionsRecordsQualificationsPointsBLL().Add(PromotionRecordQualificationPoint);
                //    }
                //}

                #endregion

                if (this.PromotionRecordEmployeeID != 0)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                    result.EnumMember = PromotionsRecordsEmployeesValidationEnum.Done.ToString();
                    return result;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        private bool IsCandidateCompletedPeriod(EmployeesCareersHistoryBLL EmployeeCurrentJob, DateTime EndDate)
        {
            if ((EndDate.Date - EmployeeCurrentJob.JoinDate.Date).TotalDays >= this.DaysCountBetweenRanksToPromotion)
                return true;

            return false;
        }

        /// <summary>
        /// This to caculate days should be excluded from employee if he already completed the period on current job : (ExceptionalVacations + StudingVacations + Lenders + ScholarhipsExcluded(not passed) + StopWork (not Convice) + Absence) more than greater than 3 um alqura years should be not included
        /// </summary>
        /// <param name="EmployeeCurrentJob"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        internal bool IsCandidateCompletedPeriodAfterExcluding(int NetDays)
        {
            return NetDays >= this.DaysCountBetweenRanksToPromotion ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmployeeCurrentJob"></param>
        /// <returns></returns>
        internal bool IsLastCandidateEvaluationValidToBeIncluded(EmployeesCareersHistoryBLL EmployeeCurrentJob, EvaluationPointsEnum[] EvalExcludedFromPromotion)
        {
            EmployeesEvaluationsBLL EmployeeEvaluation = new EmployeesEvaluationsBLL().GetLastEmployeeEvaluationByEmployeeCodeID(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID);
            if (EmployeeEvaluation == null)
                return false;

            else if (EmployeeEvaluation != null)
            {
                foreach (var item in EvalExcludedFromPromotion)
                {
                    if (EmployeeEvaluation.EvaluationPointsEnum == item)
                        return false;
                }

                //if (EmployeeEvaluation.EvaluationPointsEnum != EvaluationPointsEnum.Weak)
                //    return true;
            }

            return true;
        }

        /// <summary>
        /// this is to check the candidate should be include in the promotion record after implement conditions:
        /// 1 - Last evaluation must be not weak
        /// 2 - Total period in the last job that he spent must be 3 years or more after excluding (ExceptionalVacations + StudingVacations + Lenders + ScholarhipsExcluded(not passed) + StopWork(not Convice) + Absence)
        /// </summary>
        /// <param name="CurrentEmployeeJob"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        internal bool IsValidToIncludeAfterCheckConditions(EmployeesCareersHistoryBLL CurrentEmployeeJob, int NetDays, PromotionsPeriodsBLL PromotionPeriod, EvaluationPointsEnum[] EvalExcludedFromPromotion)
        {
            //if(this.IsLastCandidateEvaluationNotWeak(EmployeeCurrentJob) 

            if (this.IsLastCandidateEvaluationValidToBeIncluded(CurrentEmployeeJob, EvalExcludedFromPromotion)
                    && this.IsCandidateCompletedPeriodAfterExcluding(NetDays)
                    && this.IsCandidateNotInOtherActivities(CurrentEmployeeJob, PromotionPeriod))

                return true;

            return false;
        }

        /// <summary>
        /// Initial task: check employee, if he is on bellow mentioned 5 activities during promotionPeriod then he'll not a part of promotion
        /// 5 activities: StopWork, Scholarship, Lender, Exceptional Vacation, Studying vacation
        /// New Rule: dated: 04-02-2020
        /// check employee, if he is on bellow mentioned 5 activities on promotionEndDate (only end date) then he'll not a part of promotion
        /// </summary>
        /// <param name="EmployeeCurrentJob"></param>
        /// <param name="PromotionPeriod"></param>
        /// <returns></returns>
        private bool IsCandidateNotInOtherActivities(EmployeesCareersHistoryBLL EmployeeCurrentJob, PromotionsPeriodsBLL PromotionPeriod)
        {
            //List<StopWorksBLL> StopWorkList ;
            //List<Scholarships> ScholarshipsList;
            //List<Lenders> LendersList;
            //ScholarshipsList = new ScholarshipsDAL().GetScholarshipsByEmployeeCodeID(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID);
            //LendersList = new LendersDAL().GetLendersByEmployeeCodeID(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID);

            // Checking stop work
            int AvailableOnPromotionEndDateDays = new StopWorksBLL().GetByEmployeeCodeIDConvicted(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID,
                                                                            PromotionPeriod.PromotionEndDate, PromotionPeriod.PromotionEndDate).Count();
            // Checking scholarsips
            if (AvailableOnPromotionEndDateDays <= 0)
                AvailableOnPromotionEndDateDays = new BaseScholarshipsBLL().GetByEmployeeCodeIDNotPassed(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID,
                                                                    PromotionPeriod.PromotionEndDate, PromotionPeriod.PromotionEndDate).Count();

            //AvailableOnPromotionEndDateDays = new ScholarshipsDAL().GetCountByEmployeeCodeIDAndDateDuration(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID,
            //                                                                PromotionPeriod.PromotionEndDate, PromotionPeriod.PromotionEndDate);                         
            // Checking Lenders
            if (AvailableOnPromotionEndDateDays <= 0)
                AvailableOnPromotionEndDateDays = new LendersDAL().GetCountByEmployeeCodeIDAndDateDuration(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID,
                                                                                PromotionPeriod.PromotionEndDate, PromotionPeriod.PromotionEndDate);

            // Checking Studying Vacation
            if (AvailableOnPromotionEndDateDays <= 0)
                AvailableOnPromotionEndDateDays = new VacationsDAL().GetCountByTypeEmployeeCareerHistoryIDAndDateDuration((int)VacationsTypesEnum.Studies,
                                                                            EmployeeCurrentJob.EmployeeCareerHistoryID,
                                                                            PromotionPeriod.PromotionEndDate, PromotionPeriod.PromotionEndDate);

            // Checking Exceptional Vacation
            if (AvailableOnPromotionEndDateDays <= 0)
                AvailableOnPromotionEndDateDays = new VacationsDAL().GetCountByTypeEmployeeCareerHistoryIDAndDateDuration((int)VacationsTypesEnum.Exceptional,
                                                                            EmployeeCurrentJob.EmployeeCareerHistoryID,
                                                                            PromotionPeriod.PromotionEndDate, PromotionPeriod.PromotionEndDate);

            return (AvailableOnPromotionEndDateDays > 0) ? false : true;
            //return (AvailableOnPromotionEndDateDays <= 0) ? true : false;
        }

        /// <summary>
        /// This to get period of candidate after excluding the exceptions days count, exceptions days count means:
        /// 1 - Exceptional Vacations
        /// 2 - Studing Vacations
        /// 3 - Scholarhips not passed
        /// 4 - Convicted Stop Work
        /// 5 - Lenders
        /// 6 - Absence
        /// </summary>
        /// <param name="EmployeeCurrentJob"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        internal int GetCandidatePeriodAfterExcludingExceptionsPeriods(EmployeesCareersHistoryBLL EmployeeCurrentJob, DateTime EndDate)
        {
            //int ExceptionalVacationsExcludedDaysCount = 0;
            //int StudingVacationsExcludedDaysCount = 0;
            //int ScholarhipsExcludedDaysCount = 0;
            //int StopWorkExcludedDaysCount = 0;
            //int LendersExcludedDaysCount = 0;
            //int AbsenceExcludedDaysCount = 0;
            int TotalExcludedDaysCount = 0;

            if (IsCandidateCompletedPeriod(EmployeeCurrentJob, EndDate))
            {
                DateTime StartDate = EmployeeCurrentJob.JoinDate.Date;
                ////EmployeesAbsencesCountDTO AbsenceDays = new TimeAttendanceBLL().GetEmployeesAbsenceDaysCountByEmployeeCodeNo(AbsenceDaysList, EmployeeCurrentJob.EmployeeCode.EmployeeCodeNo);
                //TimeAttendanceBLL.ToDate = EndDate;
                //int AbsenceDays = TimeAttendanceBLL.AbsenceDays.Where(x => x.EmployeeCode == EmployeeCurrentJob.EmployeeCode.EmployeeCodeNo && x.Date >= StartDate).Count();

                //LendersExcludedDaysCount = int.Parse(new LendersBLL().GetByEmployeeCodeID(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID, StartDate, EndDate).Sum(x => (x.LenderEndDate - x.LenderStartDate).TotalDays).ToString());
                //ExceptionalVacationsExcludedDaysCount = new ExceptionalVacationsBLL().GetExceptionalVacationsByEmployeeCodeID(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID, StartDate, EndDate).Sum(x => x.VacationPeriod);
                //StudingVacationsExcludedDaysCount = new StudiesVacationsBLL().GetStudiesVacationsByEmployeeCodeID(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID, StartDate, EndDate).Sum(x => x.VacationPeriod);
                //ScholarhipsExcludedDaysCount = new BaseScholarshipsBLL().GetByEmployeeCodeIDNotPassed(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID, StartDate, EndDate).Sum(x => x.ScholarshipPeriod);
                //StopWorkExcludedDaysCount = new StopWorksBLL().GetByEmployeeCodeIDConvicted(EmployeeCurrentJob.EmployeeCode.EmployeeCodeID, StartDate, EndDate).Sum(x => x.StopWorkPeriod);
                //AbsenceExcludedDaysCount = AbsenceDays;
                //TotalExcludedDaysCount = ExceptionalVacationsExcludedDaysCount +
                //                         StudingVacationsExcludedDaysCount +
                //                         ScholarhipsExcludedDaysCount +
                //                         StopWorkExcludedDaysCount +
                //                         LendersExcludedDaysCount +
                //                         AbsenceExcludedDaysCount;

                TotalExcludedDaysCount = GetTotalExcludedDaysCount(EmployeeCurrentJob.EmployeeCode.EmployeeCodeNo,
                                                                    EmployeeCurrentJob.EmployeeCode.EmployeeCodeID,
                                                                    StartDate, EndDate);
                return int.Parse(((EndDate - StartDate).TotalDays - TotalExcludedDaysCount).ToString());
            }
            return 0;
        }

        internal int GetTotalExcludedDaysCount(string EmployeeCodeNo, int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            int ExceptionalVacationsExcludedDaysCount = 0;
            int StudingVacationsExcludedDaysCount = 0;
            int ScholarhipsExcludedDaysCount = 0;
            int StopWorkExcludedDaysCount = 0;
            int LendersExcludedDaysCount = 0;
            int AbsenceExcludedDaysCount = 0;
            int TotalExcludedDaysCount = 0;

            TimeAttendanceBLL.ToDate = EndDate;
            int AbsenceDays = TimeAttendanceBLL.AbsenceDays.Where(x => x.EmployeeCode == EmployeeCodeNo && x.Date >= StartDate).Count();

            LendersExcludedDaysCount = int.Parse(new LendersBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate).Sum(x => (x.LenderEndDate - x.LenderStartDate).TotalDays).ToString());
            ExceptionalVacationsExcludedDaysCount = new ExceptionalVacationsBLL().GetExceptionalVacationsByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate).Sum(x => x.VacationPeriod);
            StudingVacationsExcludedDaysCount = new StudiesVacationsBLL().GetStudiesVacationsByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate).Sum(x => x.VacationPeriod);
            ScholarhipsExcludedDaysCount = new BaseScholarshipsBLL().GetByEmployeeCodeIDNotPassed(EmployeeCodeID, StartDate, EndDate).Sum(x => x.ScholarshipPeriod);
            StopWorkExcludedDaysCount = new StopWorksBLL().GetByEmployeeCodeIDConvicted(EmployeeCodeID, StartDate, EndDate).Sum(x => x.StopWorkPeriod);
            AbsenceExcludedDaysCount = AbsenceDays;
            TotalExcludedDaysCount = ExceptionalVacationsExcludedDaysCount +
                                     StudingVacationsExcludedDaysCount +
                                     ScholarhipsExcludedDaysCount +
                                     StopWorkExcludedDaysCount +
                                     LendersExcludedDaysCount +
                                     AbsenceExcludedDaysCount;

            return TotalExcludedDaysCount;
        }

        public PromotionsRecordsEmployeesBLL GetByPromotionRecordEmployeeID(int PromotionRecordEmployeeID)
        {
            try
            {
                PromotionsRecordsEmployees PromotionsRecordsEmployees = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID);
                return new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(PromotionsRecordsEmployees);
            }
            catch
            {
                throw;
            }
        }

        public Result Approve()
        {
            Result result = new Result();
            PromotionsRecordsEmployees PromotionsRecordsEmployees = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordEmployeeID(this.PromotionRecordEmployeeID);

            new PromotionsRecordsEmployeesDAL().UpdateApproval(new PromotionsRecordsEmployees()
            {
                PromotionRecordEmployeeID = PromotionsRecordsEmployees.PromotionRecordEmployeeID,
                IsApproved = true,
                ApprovedDate = DateTime.Now,
                ApprovedBy = this.LoginIdentity.EmployeeCodeID,
                LastUpdatedDate = DateTime.Now,
                LastUpdatedBy = this.LoginIdentity.EmployeeCodeID,
            });

            #region Adding Log
            string ActionDescription = string.Format(Globalization.PromotionRecordActionDescriptionApproveText,
                                                                            PromotionsRecordsEmployees.CurrentEmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo,
                                                                            PromotionsRecordsEmployees.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " +
                                                                            PromotionsRecordsEmployees.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " +
                                                                            PromotionsRecordsEmployees.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " +
                                                                            PromotionsRecordsEmployees.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr);
            result = new PromotionsRecordsLogsBLL()
            {
                PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionsRecordsEmployees.PromotionsRecords.PromotionRecordID },
                PromotionRecordNo = PromotionsRecordsEmployees.PromotionsRecords.PromotionRecordNo,
                PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.Approve },
                ActionDescription = ActionDescription,
                LoginIdentity = this.LoginIdentity,
            }.Add();
            #endregion 

            result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
            result.EnumMember = PromotionsRecordsEmployeesValidationEnum.Done.ToString();
            return result;
        }

        public Result ApproveCancel()
        {
            Result result = new Result();
            PromotionsRecordsEmployees PromotionsRecordsEmployees = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordEmployeeID(this.PromotionRecordEmployeeID);

            new PromotionsRecordsEmployeesDAL().UpdateApproval(new PromotionsRecordsEmployees()
            {
                PromotionRecordEmployeeID = PromotionsRecordsEmployees.PromotionRecordEmployeeID,
                IsApproved = false,
                ApprovedDate = DateTime.Now,
                ApprovedBy = this.LoginIdentity.EmployeeCodeID,
                LastUpdatedDate = DateTime.Now,
                LastUpdatedBy = this.LoginIdentity.EmployeeCodeID,
            });

            #region Adding Log
            string ActionDescription = string.Format(Globalization.PromotionRecordActionDescriptionCancelApproveText,
                                                                            PromotionsRecordsEmployees.CurrentEmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo,
                                                                            PromotionsRecordsEmployees.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " +
                                                                            PromotionsRecordsEmployees.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " +
                                                                            PromotionsRecordsEmployees.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " +
                                                                            PromotionsRecordsEmployees.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr);
            result = new PromotionsRecordsLogsBLL()
            {
                PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionsRecordsEmployees.PromotionsRecords.PromotionRecordID },
                PromotionRecordNo = PromotionsRecordsEmployees.PromotionsRecords.PromotionRecordNo,
                PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.CancelApprove },
                ActionDescription = ActionDescription,
                LoginIdentity = this.LoginIdentity,
            }.Add();
            #endregion 

            result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
            result.EnumMember = PromotionsRecordsEmployeesValidationEnum.Done.ToString();
            return result;
        }

        public Result UpdateDeserveExtraBonus()
        {
            Result result = null;
            //--== Get PromotionRecordEmployee info from database.
            PromotionsRecordsEmployees PromotionRecordEmployee = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordEmployeeID(this.PromotionRecordEmployeeID);

            if (PromotionRecordEmployee.IsApproved.HasValue)
            {
                if (PromotionRecordEmployee.IsApproved.Value)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                    result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfApproved.ToString();
                    return result;
                }

            }

            //---== Check this.IsDeserveExtraBonus it has value or null if null return.
            if (this.IsDeserveExtraBonus.HasValue)
            {
                //--== Here we make sure the IsDeserveExtraBonus from database it has value and it's diffrent than this.IsDeserveExtraBonus
                //--== if both have same value no need to update and return.
                if (PromotionRecordEmployee.IsDeserveExtraBonus.HasValue)
                {
                    if (this.IsDeserveExtraBonus.Value == PromotionRecordEmployee.IsDeserveExtraBonus.Value)
                    {
                        result = new Result();
                        result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                        result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfIsDeserveExtraBonusUpdateWithSameValue.ToString();
                        return result;
                    }
                }
                //--== we need the new EmployeeCareerHistory to Increase CareerDegree (1) Befor update IsDeserveExtraBonus = true in database.
                //--== and Decrease CareerDegree (1) befor update IsDeserveExtraBonus = false in Database,if IsDeserveExtraBonus value not null.
                //--== if IsDeserveExtraBonus null then no need to Decrease CareerDegree.
                EmployeesCareersHistoryBLL EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(this.NewEmployeeCareer.EmployeeCareerHistoryID);
                EmployeeCareerHistory.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = this.LoginIdentity.EmployeeCodeID };
                //--== If check this.IsDeserveExtraBonus value True Or false.
                if (this.IsDeserveExtraBonus.Value)
                {
                    //Increase CareerDegree +1
                    result = new EmployeesCareersHistoryBLL().IncreaseCareerDegree(EmployeeCareerHistory);
                    if (result.EnumMember == CareersHistoryValidationEnum.RejectedBecauseOfCareerDegreeOutOfRange.ToString())
                    {
                        return result;
                    }
                }
                else
                {
                    //Check IsDeserveExtraBonus value in database is not null, Meen we need to DecreaseCareerDegree
                    //If null that meen it's first time and we don't need to DecreaseCareerDegree
                    if (PromotionRecordEmployee.IsDeserveExtraBonus.HasValue)
                    {
                        //decrease CareerDegree -1
                        EmployeeCareerHistory.DecreaseCareerDegree();
                    }
                }
                // Commet Update of IsDeserveExtraBonus
                new PromotionsRecordsEmployeesDAL().UpdateDeserveExtraBonus(new PromotionsRecordsEmployees()
                {
                    PromotionRecordEmployeeID = PromotionRecordEmployee.PromotionRecordEmployeeID,
                    IsDeserveExtraBonus = this.IsDeserveExtraBonus.Value,
                    ApprovedBy = this.LoginIdentity.EmployeeCodeID,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID,
                });


            }
            else
            {
                result = new Result();
                result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
                result.EnumMember = PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfIsDeserveExtraBonusNotSpecifiedValue.ToString();
                return result;
            }

            result = new Result();
            result.EnumType = typeof(PromotionsRecordsEmployeesValidationEnum);
            result.EnumMember = PromotionsRecordsEmployeesValidationEnum.Done.ToString();
            return result;
        }

        /// <summary>
        /// Show Absent Days of each promoted employee from 'one year before promotion end date' to 'promotion end date'
        /// </summary>
        /// <param name="PromotionRecordID"></param>
        /// <returns></returns>
        public List<PromotionsRecordsEmployeesBLL> GetCandidatesAlreadyPromoted(int PromotionRecordID)
        {
            try
            {
                int AbsentDays;
                PromotionsRecordsEmployeesBLL PromotionRecordEmployee;
                List<TimeAttendanceBLL> EmployeesEvaluationsBLLList;
                List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesBLLList = new List<PromotionsRecordsEmployeesBLL>();
                List<PromotionsRecordsEmployees> promotionsRecordsEmployees =
                    new PromotionsRecordsEmployeesDAL().GetByPromotionRecordID(PromotionRecordID).Where(c =>
                        c.PromotionRecordJobVacancyID != null &&
                        c.NewEmployeeCareerHistoryID != null &&
                        c.PromotionsDecisions != null
                    ).ToList();
                if (promotionsRecordsEmployees.Count > 0)
                {
                    foreach (var item in promotionsRecordsEmployees)
                    {
                        PromotionRecordEmployee = new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(item);
                        AbsentDays = 0;

                        if (item.CurrentEmployeeCareerHistoryID.HasValue)
                        {
                            EmployeesEvaluationsBLLList = new EmployeesCodesBLL().GetAbsenceByEmployeeCodeID(item.CurrentEmployeesCareersHistory.EmployeeCodeID,
                                                                                                            //item.CurrentEmployeesCareersHistory.JoinDate, 
                                                                                                            item.PromotionsRecords.PromotionsPeriods.PromotionEndDate.AddYears(-1),
                                                                                                            item.PromotionsRecords.PromotionsPeriods.PromotionEndDate);
                            AbsentDays = EmployeesEvaluationsBLLList.Count();
                        }

                        PromotionRecordEmployee.AbsentDays = AbsentDays;
                        PromotionsRecordsEmployeesBLLList.Add(PromotionRecordEmployee);
                    }
                }
                return PromotionsRecordsEmployeesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployeesBLL> CheckIsDeserveExtraBonusNotSpecifiedValueForCandidatesAlreadyPromoted(int PromotionRecordID)
        {
            return GetCandidatesAlreadyPromoted(PromotionRecordID).Where(c => c.IsDeserveExtraBonus == null).ToList();
        }

        public List<PromotionsRecordsEmployeesBLL> GetCandidatesAlreadyInstalled(int PromotionRecordID)
        {
            try
            {
                List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesBLLList = new List<PromotionsRecordsEmployeesBLL>();
                List<PromotionsRecordsEmployees> promotionsRecordsEmployees =
                    new PromotionsRecordsEmployeesDAL().GetByPromotionRecordID(PromotionRecordID).Where(c => c.PromotionRecordJobVacancyID != null).ToList();
                if (promotionsRecordsEmployees.Count > 0)
                {
                    foreach (var item in promotionsRecordsEmployees)
                        PromotionsRecordsEmployeesBLLList.Add(new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(item));
                }
                return PromotionsRecordsEmployeesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployeesBLL> GetByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            try
            {
                List<PromotionsRecordsEmployees> PromotionsRecordsEmployeesList = new PromotionsRecordsEmployeesDAL().GetByEmployeeCareerHistoryID(EmployeeCareerHistoryID);
                List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesBLLList = new List<PromotionsRecordsEmployeesBLL>();
                if (PromotionsRecordsEmployeesList.Count > 0)
                    foreach (var item in PromotionsRecordsEmployeesList)
                        PromotionsRecordsEmployeesBLLList.Add(new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(item));

                return PromotionsRecordsEmployeesBLLList;
            }
            catch
            {
                throw;
            }
        }

        internal PromotionsRecordsEmployeesBLL MapPromotionRecordEmployee(PromotionsRecordsEmployees PromotionRecordEmployee)
        {
            try
            {
                PromotionsRecordsEmployeesBLL PromotionRecordEmployeeBLL = null;
                if (PromotionRecordEmployee != null)
                {
                    PromotionRecordEmployeeBLL = new PromotionsRecordsEmployeesBLL();
                    PromotionRecordEmployeeBLL.PromotionRecordEmployeeID = PromotionRecordEmployee.PromotionRecordEmployeeID;
                    PromotionRecordEmployeeBLL.PromotionRecord = new PromotionsRecordsBLL().MapPromotionRecord(PromotionRecordEmployee.PromotionsRecords);
                    PromotionRecordEmployeeBLL.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL().MapPromotionRecordJobVacancy(PromotionRecordEmployee.PromotionsRecordsJobsVacancies);
                    PromotionRecordEmployeeBLL.CurrentEmployeeCareer = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(PromotionRecordEmployee.CurrentEmployeesCareersHistory);
                    PromotionRecordEmployeeBLL.NewEmployeeCareer = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(PromotionRecordEmployee.NewEmployeesCareersHistory);
                    PromotionRecordEmployeeBLL.CurrentJobDaysCount = PromotionRecordEmployee.CurrentJobDaysCount.HasValue ? PromotionRecordEmployee.CurrentJobDaysCount.Value : 0;
                    PromotionRecordEmployeeBLL.PreviousEmployeeCareer = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(PromotionRecordEmployee.PreviousEmployeesCareersHistory);
                    PromotionRecordEmployeeBLL.PreviousJobDaysCount = PromotionRecordEmployee.PreviousJobDaysCount;
                    PromotionRecordEmployeeBLL.OnGoingServiceDaysCount = PromotionRecordEmployee.OnGoingServiceDaysCount.HasValue ? PromotionRecordEmployee.OnGoingServiceDaysCount.Value : 0;
                    PromotionRecordEmployeeBLL.PriorServiceDaysCount = PromotionRecordEmployee.PriorServiceDaysCount.HasValue ? PromotionRecordEmployee.PriorServiceDaysCount.Value : 0;
                    PromotionRecordEmployeeBLL.EducationPoints = PromotionRecordEmployee.EducationPoints.HasValue ? PromotionRecordEmployee.EducationPoints.Value : 0;
                    PromotionRecordEmployeeBLL.SeniorityPoints = PromotionRecordEmployee.SeniorityPoints.HasValue ? PromotionRecordEmployee.SeniorityPoints.Value : 0;
                    PromotionRecordEmployeeBLL.EvaluationPoints = PromotionRecordEmployee.EvaluationPoints.HasValue ? PromotionRecordEmployee.EvaluationPoints.Value : 0;
                    PromotionRecordEmployeeBLL.ActualTaskPerformancePoints = PromotionRecordEmployee.ActualTaskPerformancePoints;
                    PromotionRecordEmployeeBLL.PromotionCandidateAddedWay = new PromotionCandidateAddedWayBLL().MapPromotionCandidateAddedWay(PromotionRecordEmployee.PromotionsCandidatesAddedWays);
                    PromotionRecordEmployeeBLL.ManualAddedReason = PromotionRecordEmployee.ManualAddedReason;
                    PromotionRecordEmployeeBLL.IsIncluded = PromotionRecordEmployee.IsIncluded;

                    PromotionRecordEmployeeBLL.IsDeserveExtraBonus = PromotionRecordEmployee.IsDeserveExtraBonus;
                    PromotionRecordEmployeeBLL.IsRemovedAfterIncluding = PromotionRecordEmployee.IsRemovedAfterIncluding.HasValue ? PromotionRecordEmployee.IsRemovedAfterIncluding.Value : false;
                    PromotionRecordEmployeeBLL.RemovingReason = PromotionRecordEmployee.RemovingReason;
                    PromotionRecordEmployeeBLL.RemovingBy = new EmployeesCodesBLL().MapEmployeeCode(PromotionRecordEmployee.RemovingByNav);
                    PromotionRecordEmployeeBLL.IsApproved = PromotionRecordEmployee.IsApproved;
                    PromotionRecordEmployeeBLL.ApprovedDate = PromotionRecordEmployee.ApprovedDate;
                    PromotionRecordEmployeeBLL.ApprovedBy = new EmployeesCodesBLL().MapEmployeeCode(PromotionRecordEmployee.ApprovedByNav);

                    PromotionRecordEmployeeBLL.LastQualificationDegree = new QualificationsDegreesBLL().MapQualificationDegree(PromotionRecordEmployee.LastQualificationsDegrees);
                    PromotionRecordEmployeeBLL.LastQualification = new QualificationsBLL().MapQualification(PromotionRecordEmployee.LastQualifications);
                    PromotionRecordEmployeeBLL.LastGeneralSpecialization = new GeneralSpecializationsBLL().MapGeneralSpecialization(PromotionRecordEmployee.LastGeneralSpecializations);

                    PromotionRecordEmployeeBLL.PromotionDecision = new PromotionsDecisionsBLL().MapPromotionDecision(PromotionRecordEmployee.PromotionsDecisions);
                    PromotionRecordEmployeeBLL.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL().MapPromotionRecordJobVacancy(PromotionRecordEmployee.PromotionsRecordsJobsVacancies);

                    PromotionRecordEmployeeBLL.ByExamResult = PromotionRecordEmployee.ByExamResult;

                    PromotionRecordEmployeeBLL.CurrentJobJoinDate = PromotionRecordEmployee.CurrentJobJoinDate;
                    PromotionRecordEmployeeBLL.PreviousJobJoinDate = PromotionRecordEmployee.PreviousJobJoinDate;

                    PromotionRecordEmployeeBLL.CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(PromotionRecordEmployee.CreatedByNav);
                    PromotionRecordEmployeeBLL.CreatedDate = PromotionRecordEmployee.CreatedDate;
                }
                return PromotionRecordEmployeeBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}