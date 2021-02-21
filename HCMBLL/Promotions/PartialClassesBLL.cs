using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public partial class PromotionsRecordsBLL : CommonEntity, IEntity
    {
        public List<PromotionsRecordsBLL> GetPromotionsRecords()
        {
            List<PromotionsRecords> PromotionRecordList = new PromotionsRecordsDAL().GetPromotionsRecords();
            List<PromotionsRecordsBLL> PromotionRecordBLLList = new List<PromotionsRecordsBLL>();
            PromotionsRecordsBLL PromotionRecordBLL = new PromotionsRecordsBLL();

            if (PromotionRecordList.Count > 0)
            {
                foreach (var item in PromotionRecordList)
                {
                    PromotionRecordBLL = new PromotionsRecordsBLL().MapPromotionRecord(item);

                    PromotionRecordBLL.TotalJobVacancies = item.PromotionsRecordsJobsVacancies.Count;
                    PromotionRecordBLL.TotalIncludedCandidates = item.PromotionsRecordsEmployees.Where(x => x.IsIncluded).Count();
                    PromotionRecordBLL.TotalPromotedCandidates = item.PromotionsRecordsEmployees.Where(x => x.IsIncluded
                                                                                                        && x.PromotionRecordJobVacancyID != null
                                                                                                        && x.NewEmployeeCareerHistoryID != null).Count();

                    PromotionRecordBLLList.Add(PromotionRecordBLL);
                }
            }
            return PromotionRecordBLLList;
        }

        /// <summary>
        /// Validations:
        /// 1 - Promotion Record Status must be opened
        /// 2 - there must be job vacancies
        /// 3 - there must be candidates to preference
        /// 4 - In PromotionRecordQualificationPoints table (if any data), there must be no null value in points and KindID Cols.
        /// Actions:
        /// 1 - 
        /// </summary>
        /// <returns></returns>
        public Result Preference()
        {
            try
            {
                Result result = null;

                PromotionsRecords PromotionRecord = new PromotionsRecordsDAL().GetByPromotionRecordID(this.PromotionRecordID);

                #region Validate Promotion Record Status must 'Open'
                if (PromotionRecord.PromotionRecordStatusID != (int)PromotionsRecordsStatusEnum.Open)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpen.ToString();
                    return result;
                }
                #endregion

                List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployeeList = new PromotionsRecordsEmployeesBLL().GetByPromotionRecordID(PromotionRecord.PromotionRecordID).Where(e => e.IsIncluded && !e.IsRemovedAfterIncluding).ToList();
                this.PromotionsRecordsJobsVacancies = new PromotionsRecordsJobsVacanciesBLL().GetByPromotionRecordID(PromotionRecord.PromotionRecordID);
                result = DistributeJobVacancies(PromotionRecord, PromotionsRecordsJobsVacancies, PromotionRecordIncludedEmployeeList);

                if (result != null)
                    return result;

                #region preference
                PromotionRecord.PromotionRecordStatusID = (int)PromotionsRecordsStatusEnum.Preferenced;
                PromotionRecord.PreferenceTime = DateTime.Now;
                PromotionRecord.PreferenceBy = this.LoginIdentity.EmployeeCodeID;
                PromotionRecord.LastUpdatedDate = DateTime.Now;
                PromotionRecord.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                PromotionRecord.PromotionsRecordsEmployees = MapPromotionRecordEmployeeBLLtoEntity(PromotionRecordIncludedEmployeeList);

                new PromotionsRecordsDAL().Preference(PromotionRecord);
                this.PromotionRecordStatus = new PromotionsRecordsStatusBLL().GetByPromotionRecordStatusID((int)PromotionsRecordsStatusEnum.Preferenced);
                #endregion

                int TotalJobAssignedToCandidates = PromotionRecord.PromotionsRecordsEmployees.Where(x => x.PromotionRecordJobVacancyID != null).Count();

                if (this.PromotionRecordID != 0)
                {
                    #region Adding Log
                    result = new PromotionsRecordsLogsBLL()
                    {
                        PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecord.PromotionRecordID },
                        PromotionRecordNo = PromotionRecord.PromotionRecordNo,
                        PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.Preference },
                        ActionDescription = string.Format(Globalization.PromotionRecordActionDescriptionPreferencedText, TotalJobAssignedToCandidates.ToString()),
                        LoginIdentity = this.LoginIdentity,
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

        public Result DistributeJobVacancies(PromotionsRecords PromotionRecord, List<PromotionsRecordsJobsVacanciesBLL> PromotionRecordJobVacanciesList, List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployeeList)
        {
            Result result = null;
            PromotionsRecordsJobsVacanciesBLL PromotionRecordJobVacancy;
            //PromotionRecordIncludedEmployeeList = new List<PromotionsRecordsEmployeesBLL> ();

            result = CommonValidation(result);

            #region Validate if there are candidates should be entered in promotion record
            List<PromotionsRecordsEmployees> CandidatesList = new List<PromotionsRecordsEmployees>();
            CandidatesList = new PromotionsRecordsEmployeesDAL().GetIncludedByPromotionRecordID(PromotionRecord.PromotionRecordID);

            if (CandidatesList == null || CandidatesList.Count == 0)
            {
                result = new Result();
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString();
                return result;
            }
            else
            {
                if (CandidatesList.Where(e => e.IsIncluded &&
                    (e.IsRemovedAfterIncluding.HasValue ? e.IsRemovedAfterIncluding.Value == false : 1 == 1)).Count() == 0)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString();
                    return result;
                }
            }

            #endregion

            #region Validate if there is PromotionRecords Qualification Points has null pts
            List<PromotionsRecordsQualificationsPoints> QualificationsPointsList = new PromotionsRecordsQualificationsPointsDAL().GetByPromotionRecordID(PromotionRecord.PromotionRecordID);
            if (QualificationsPointsList.Count > 0 && QualificationsPointsList.Where(q => q.Points == null || q.PromotionRecordQualificationKindID == null).Count() > 0)
            {
                result = new Result();
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfSomeQualifitionPointsAreNull.ToString();
                return result;
            }
            #endregion

            //PromotionRecordIncludedEmployeeList = new PromotionsRecordsEmployeesBLL().GetByPromotionRecordID(PromotionRecord.PromotionRecordID).Where(e => e.IsIncluded && !e.IsRemovedAfterIncluding).ToList();
            //this.PromotionsRecordsJobsVacancies = new PromotionsRecordsJobsVacanciesBLL().GetByPromotionRecordID(PromotionRecord.PromotionRecordID);
            //List<SenioritysPointsDeclarations> SeniorityPointList = new SenioritysPointsDeclarationsDAL().GetSenioritysPointsDeclarations();
            //List<EmployeesEvaluations> EmployeeEvaluationList = new EmployeesEvaluationsDAL().GetEmployeesEvaluations();
            //List<EmployeesExperiences> EmployeeExperienceList = new EmployeesExperiencesDAL().GetEmployeesExperiences();

            // remove all JobVacancies if already assigned to employees in PromotionRecordIncludedEmployeeList
            foreach (var item in PromotionRecordIncludedEmployeeList.Where(e => e.PromotionRecordJobVacancy != null && e.PromotionRecordJobVacancy.PromotionRecordJobVacancyID > 0))
            {
                PromotionRecordJobVacancy = PromotionRecordJobVacanciesList.FirstOrDefault(j => j.PromotionRecordJobVacancyID == item.PromotionRecordJobVacancy.PromotionRecordJobVacancyID);
                if (PromotionRecordJobVacancy != null && PromotionRecordJobVacancy.PromotionRecordJobVacancyID > 0)
                    PromotionRecordJobVacanciesList.Remove(PromotionRecordJobVacancy);
            }

            // filter list and get on PromotionRecordJobVacancy is empty/null (it means available for promotion)
            PromotionRecordIncludedEmployeeList = PromotionRecordIncludedEmployeeList.Where(e => e.IsIncluded && !e.IsRemovedAfterIncluding && e.PromotionRecordJobVacancy == null).ToList();

            List<int> PromotionRecordEmployeeCodeIDs = PromotionRecordIncludedEmployeeList.Select(e => e.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID).ToList<int>();
            List<SenioritysPointsDeclarations> SeniorityPointList = new SenioritysPointsDeclarationsDAL().GetSenioritysPointsDeclarations();
            List<EmployeesEvaluations> EmployeeEvaluationList = new EmployeesEvaluationsDAL().GetEmployeesEvaluationsByEmployeeCodeIDs(PromotionRecordEmployeeCodeIDs);
            //List<EmployeesExperiences> EmployeeExperienceList = new EmployeesExperiencesDAL().GetEmployeesExperiencesByEmployeeCodeIDs(PromotionRecordEmployeeCodeIDs);
            List<EmployeeExperiencesWithDetails> EmployeeExperienceWithDetailsList = new EmployeeExperiencesWithDetailsDAL().GetEmployeeExperiencesWithDetailsByEmployeeCodeIDs(PromotionRecordEmployeeCodeIDs);
            List<EmployeesQualifications> EmployeeQualificationList = new EmployeesQualificationsDAL().GetEmployeesQualificationsByEmployeeCodeIDs(PromotionRecordEmployeeCodeIDs);
            List<EmployeesCareersHistory> EmployeeCareerHistoryList = new EmployeesCareersHistoryDAL().GetByEmployeeCodeIDs(PromotionRecordEmployeeCodeIDs)
                                                                                                            .Where(x => x.CareerHistoryTypeID.Equals((int)CareersHistoryTypesEnum.Hiring))
                                                                                                            .ToList();

            // Total points and update into list "PromotionRecordIncludedEmployeeList"
            PromotionRecordIncludedEmployeeList = CalculatePoints(PromotionRecordIncludedEmployeeList, QualificationsPointsList, SeniorityPointList, EmployeeEvaluationList, EmployeeQualificationList);

            // Total TotalExperience and update into list "PromotionRecordIncludedEmployeeList"
            PromotionRecordIncludedEmployeeList = CalculateTotalExperience(PromotionRecordIncludedEmployeeList, EmployeeExperienceWithDetailsList, EmployeeCareerHistoryList);

            // Distribute jobs on the basis of points
            StartDistributingJobVacancies(PromotionRecordJobVacanciesList, PromotionRecordIncludedEmployeeList.ToList());          //.Where(c => c.TotalPoints > 0) No need if any candidate has TotalPts=0 but jobs available so he can get it.

            return result;
        }

        /// <summary>
        /// This method is used to calculate points using respective methods, then set 'HasSamePoints' col based on if same value in TotalPoints col.
        /// </summary>
        /// <param name="QualificationsPointsList"></param>
        /// <param name="SeniorityPointList"></param>
        /// <param name="EmployeeEvaluationList"></param>
        private List<PromotionsRecordsEmployeesBLL> CalculatePoints(List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployeeList,
                                                            List<PromotionsRecordsQualificationsPoints> QualificationsPointsList, List<SenioritysPointsDeclarations> SeniorityPointList,
                                                            List<EmployeesEvaluations> EmployeeEvaluationList, List<EmployeesQualifications> EmployeeQualificationList)
        {
            #region Calculate Total Points = EducationPoints + SeniorityPoints + EvaluationPoints
            foreach (var Candidate in PromotionRecordIncludedEmployeeList)
            {
                Candidate.EducationPoints = this.CalculateEducationPoints(Candidate, QualificationsPointsList, EmployeeQualificationList);
                Candidate.SeniorityPoints = this.CalculateSeniorityPoints(Candidate, SeniorityPointList);
                Candidate.EvaluationPoints = this.CalculateEvaluationPoints(Candidate, EmployeeEvaluationList);
                //Candidate.ActualTaskPerformancePoints =      // Setting 2-Extra points from Toggle Button
            }
            #endregion

            #region Setting HasSamePoints Column
            // Setting all Employees HasSamePoints = false
            PromotionRecordIncludedEmployeeList.ForEach(j => j.HasSamePoints = false);
            foreach (var Candidate in PromotionRecordIncludedEmployeeList.Where(e => e.HasSamePoints == false).ToList())
            {
                if (Candidate.HasSamePoints)
                    continue;

                if (PromotionRecordIncludedEmployeeList.Where(c => c.TotalPoints == Candidate.TotalPoints).Count() > 1)
                {
                    foreach (var SameCandidate in PromotionRecordIncludedEmployeeList.Where(c => c.TotalPoints == Candidate.TotalPoints).ToList())
                        SameCandidate.HasSamePoints = true;
                }
            }
            #endregion

            return PromotionRecordIncludedEmployeeList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PromotionRecordIncludedEmployeeList"></param>
        /// <param name="EmployeeExperienceList"></param>
        /// <returns></returns>
        private List<PromotionsRecordsEmployeesBLL> CalculateTotalExperience(List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployeeList,
                                                            List<EmployeeExperiencesWithDetails> EmployeeExperienceWithDetailsList, List<EmployeesCareersHistory> EmployeeCareerHistoryList)
        {
            int TotalExcludedDaysCount = 0;
            List<EmployeeExperiencesWithDetails> EmployeeExperienceWithDetailList;
            EmployeesCareersHistory EmployeeCareer;
            #region Calculate TotalExperience using OnGoingServiceDaysCount + (TotalExperience from EmployeesExperiences Table)
            foreach (var Cand in PromotionRecordIncludedEmployeeList.Where(e => e.PromotionRecordJobVacancy == null).ToList())
            {
                EmployeeExperienceWithDetailList = EmployeeExperienceWithDetailsList.Where(ee => ee.EmployeeCodeID == Cand.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID).ToList();
                if (EmployeeExperienceWithDetailList != null && EmployeeExperienceWithDetailList.Count > 0)
                    Cand.PriorServiceDaysCount = new EmployeeExperiencesWithDetailsBLL().GetTotalDaysByEmployeeExperience(EmployeeExperienceWithDetailList);
                else
                    Cand.PriorServiceDaysCount = 0;

                EmployeeCareer = EmployeeCareerHistoryList.FirstOrDefault(ee => ee.EmployeeCodeID == Cand.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID);

                if (EmployeeCareer != null && EmployeeCareer.EmployeeCareerHistoryID > 0)
                {
                    Cand.OnGoingServiceDaysCount = new EmployeesCareersHistoryBLL().GetOnGoingExperienceDays(EmployeeCareer,
                                                                                                                Cand.PromotionRecord.PromotionPeriod.PromotionEndDate.Date);

                    if (EmployeeCareer != null && EmployeeCareer.EmployeeCareerHistoryID > 0)
                        TotalExcludedDaysCount = new PromotionsRecordsEmployeesBLL().GetTotalExcludedDaysCount(EmployeeCareer.EmployeesCodes.EmployeeCodeNo, EmployeeCareer.EmployeeCodeID,
                                                                                EmployeeCareer.JoinDate, Cand.PromotionRecord.PromotionPeriod.PromotionEndDate.Date);

                    Cand.OnGoingServiceDaysCount = Cand.OnGoingServiceDaysCount - TotalExcludedDaysCount;

                }
                else
                    Cand.OnGoingServiceDaysCount = 0;

            }
            #endregion

            #region Setting HasSameTotalExperience Column
            // Setting all Employees HasSamePoints = false
            PromotionRecordIncludedEmployeeList.ForEach(j => j.HasSameTotalExperience = false);
            foreach (var Candidate in PromotionRecordIncludedEmployeeList.Where(e => e.TotalExperience > 0).ToList())
            {
                if (PromotionRecordIncludedEmployeeList.Where(c => c.TotalExperience == Candidate.TotalExperience).Count() > 1)
                {
                    foreach (var SameCandidate in PromotionRecordIncludedEmployeeList.Where(c => c.TotalExperience == Candidate.TotalExperience).ToList())
                        SameCandidate.HasSameTotalExperience = true;
                }
            }
            #endregion

            return PromotionRecordIncludedEmployeeList;
        }

        /// <summary>
        /// This is the main function to distribute jobs as per Total Points, or ByLastQualification, or ByCurrentJobSeniority, or ByLastJobSeniority, or ByTotalExperience or ByExam
        /// In this function main focus on job distribution by Total points (EducationPoint+SeniorityPoint+EvaluationPoint), 
        /// Logic of distribute jobs by Total Points: 
        /// Step 1- First calculate Total points & setting HasSamePoint fields (which is done in 'CalculatePoints' function.
        /// Step 2- Do iteration on Vancany table until there is IsAssigned field is false
        /// Step 3- Start job distribution by ordering the candidate list by TotalPoints in descending order
        /// Step 4- Then check is HasSamePoints is false assign job to this candidate and repeat step#3
        /// Step 5- Check if HasSamePoints = true then Calculate totalVacantJobs and totalCandidatesWithSamePoints, 
        /// Step 6- If totalVacantJobs >= totalCandidatesWithSamePoints then assign job to samePoint's candidates.
        /// Step 7- If totalVacantJobs lessThan totalCandidatesWithSamePoints then check candidates must have LastQualificationDegree and Call 'DistributeJobVacanciesByLastQualification' function
        /// "some notes related to step#5,6,7": If Some candidates have 'SAME Total POINTs' in this case System will distibute job as per 
        /// different mechanism (ByLastQualification, ByCurrentJobSeniorityDays, ByLastJobSeniorityDays, ByTotalExperienceDays)
        /// </summary>
        /// <param name="PromotionsRecordsJobsVacancies"></param>
        /// <param name="PromotionRecordIncludedEmployees"></param>
        private void StartDistributingJobVacancies(List<PromotionsRecordsJobsVacanciesBLL> PromotionsRecordsJobsVacancies, List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployees)
        {
            int JobVacanciesCount, HasSamePointsCount;

            // Setting all JobVacancies IsAssigned = false 
            PromotionsRecordsJobsVacancies.ForEach(j => j.IsAssigned = false);

            foreach (PromotionsRecordsJobsVacanciesBLL JobVacancy in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
            {
                if (JobVacancy.IsAssigned) continue;

                foreach (PromotionsRecordsEmployeesBLL Candidate in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null).OrderByDescending(e => e.TotalPoints))
                {
                    // This candidate has unique TotalPoints 
                    if (Candidate.HasSamePoints == false)
                    {
                        Candidate.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancy.PromotionRecordJobVacancyID };
                        JobVacancy.IsAssigned = true;
                        Candidate.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByTotalPoints };
                        Candidate.IsRedistributeJob = true;
                        break;
                    }
                    else
                    {
                        // Some Candidates has Same TotalPoints
                        JobVacanciesCount = PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count();
                        HasSamePointsCount = PromotionRecordIncludedEmployees.Where(c => c.PromotionRecordJobVacancy == null && c.TotalPoints == Candidate.TotalPoints).Count();

                        // if more 'job vacancies' available then assign jobVacancies to 'SamePoint's Candidate'
                        if (JobVacanciesCount >= HasSamePointsCount)
                        {
                            foreach (var JobVacancyForSamePoints in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
                            {
                                foreach (var CandidateForSamePoints in PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                                && e.PromotionRecordJobVacancy == null).ToList())
                                {
                                    CandidateForSamePoints.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancyForSamePoints.PromotionRecordJobVacancyID };
                                    JobVacancyForSamePoints.IsAssigned = true;
                                    CandidateForSamePoints.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByTotalPoints };
                                    Candidate.IsRedistributeJob = true;
                                    break;
                                }
                            }
                            break;
                        }
                        else    // if vacancies count is less than 'SamePoint's Candidate' 
                        // then System assign JobVacancise By different mechanism (ByLastQualification, ByCurrentJobSeniorityDays, ByLastJobSeniorityDays, ByTotalExperienceDays
                        {
                            // By LastQualifications
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null && e.LastQualificationDegree != null
                                                                            && e.LastQualificationDegree.QualificationDegreeID > 0).Count() > 0)
                            {
                                DistributeJobVacanciesByLastQualification(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                            PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                && e.PromotionRecordJobVacancy == null && e.LastQualificationDegree != null
                                                                                && e.LastQualificationDegree.QualificationDegreeID > 0).ToList(), Candidate.TotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                            // By CurrentJobSeniority
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null && e.CurrentJobSeniorityDays > 0).Count() > 0)
                            {
                                DistributeJobVacanciesByCurrentJobSeniority(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                            PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                                    && e.PromotionRecordJobVacancy == null
                                                                                                                    && e.CurrentJobSeniorityDays > 0).ToList(), Candidate.TotalPoints, 0);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                            // By LastJobSeniority
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null && e.LastJobSeniorityDays > 0).Count() > 0)
                            {
                                DistributeJobVacanciesByLastJobSeniority(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                            PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                                    && e.PromotionRecordJobVacancy == null
                                                                                                                    && e.LastJobSeniorityDays > 0).ToList(), Candidate.TotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                            // By TotalExperience
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null && e.TotalExperience > 0).Count() > 0)
                            {
                                DistributeJobVacanciesByTotalExperience(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                        && e.PromotionRecordJobVacancy == null
                                                                                                        && e.TotalExperience > 0).ToList(), Candidate.TotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                            // By ExamResult
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && (e.ByExamResult.HasValue ? e.ByExamResult.Value : 0) > 0
                                                                            && e.PromotionRecordJobVacancy == null).Count() > 0)
                            {
                                DistributeJobVacanciesByExam(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                        && (e.ByExamResult.HasValue ? e.ByExamResult.Value : 0) > 0
                                                                                                        && e.PromotionRecordJobVacancy == null).ToList(), Candidate.TotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                            // ShouldBePromotedByExam
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null).Count() > 0)
                            {
                                DistributeJobVacanciesShouldBePromotedByExam(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                        && e.PromotionRecordJobVacancy == null).ToList(), Candidate.TotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function is a part of DistributeJobVacancies Function, this function only called with there is a tie b/w two or more candidates.
        /// This function assigned jobs on the basis on LastQualificationDegree Weight
        /// Step - Check if HasSameLastQualification = true then Calculate totalVacantJobs and totalCandidatesWithSameLastQualification, 
        /// Step - If totalVacantJobs >= totalCandidatesWithSameLastQualification then assign job to sameLastQualification's candidates.
        /// Step - If totalVacantJobs lessThan totalCandidatesWithSameLastQualification then check candidates must have CurrentJobSeniorityDays>0 and Call 'DistributeJobVacanciesByCurrentJobSeniority' function
        /// different mechanism (ByCurrentJobSeniorityDays, ByLastJobSeniorityDays, ByTotalExperienceDays)
        /// </summary>
        /// <param name="PromotionsRecordsJobsVacancies"></param>
        /// <param name="PromotionRecordIncludedEmployees"></param>
        /// <returns></returns>
        private void DistributeJobVacanciesByLastQualification(List<PromotionsRecordsJobsVacanciesBLL> PromotionsRecordsJobsVacancies, List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployees,
                                                                decimal SameTotalPoints)
        {
            int JobVacanciesCount, HasSameLastQualificationCount;

            #region Setting HasSameLastQualification Column
            // Setting all Employees HasSameLastQualification = false
            PromotionRecordIncludedEmployees.ForEach(j => j.HasSameLastQualification = false);

            foreach (var CandidateLastQualification in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null).ToList())
            {
                if (PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null
                                                            && e.LastQualificationDegree.Weight == CandidateLastQualification.LastQualificationDegree.Weight).Count() > 1)
                {
                    foreach (var SameCandidate in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null
                                                                                        && e.LastQualificationDegree.Weight == CandidateLastQualification.LastQualificationDegree.Weight).ToList())
                        SameCandidate.HasSameLastQualification = true;
                }
            }
            #endregion

            foreach (PromotionsRecordsJobsVacanciesBLL JobVacancy in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
            {
                if (JobVacancy.IsAssigned) continue;

                foreach (PromotionsRecordsEmployeesBLL Candidate in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null).OrderByDescending(e => e.LastQualificationDegree.Weight))
                {
                    // This candidate has unique Last Qualification 
                    if (Candidate.HasSameLastQualification == false)
                    {
                        Candidate.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancy.PromotionRecordJobVacancyID };
                        JobVacancy.IsAssigned = true;
                        Candidate.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByLastQualifications };
                        Candidate.IsRedistributeJob = true;
                        break;
                    }
                    else
                    {
                        // Some Candidates has Same Last Qualification 
                        JobVacanciesCount = PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count();
                        HasSameLastQualificationCount = PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.LastQualificationDegree.Weight == Candidate.LastQualificationDegree.Weight).Count();

                        // if more 'job vacancies' available then assign jobVacancies to 'SameLastQualification's Candidate'
                        if (JobVacanciesCount >= HasSameLastQualificationCount)
                        {
                            foreach (var JobVacancyForSameLastQualification in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
                            {
                                foreach (var CandidateForSameLastQualification in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null
                                                                                                                    && e.LastQualificationDegree.Weight == Candidate.LastQualificationDegree.Weight).ToList())
                                {
                                    CandidateForSameLastQualification.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancyForSameLastQualification.PromotionRecordJobVacancyID };
                                    JobVacancyForSameLastQualification.IsAssigned = true;
                                    CandidateForSameLastQualification.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByLastQualifications };
                                    Candidate.IsRedistributeJob = true;
                                    break;
                                }
                            }
                            break;
                        }
                        else    // if vacancies count is less than 'SameLastQualification's Candidate' 
                        // then System assign JobVacancise By different mechanism (ByCurrentJobSeniorityDays, ByLastJobSeniorityDays, ByTotalExperienceDays
                        {
                            // By CurrentJobSeniority
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSameLastQualification && e.PromotionRecordJobVacancy == null
                                                                        && e.TotalPoints == SameTotalPoints
                                                                        && e.LastQualificationDegree != null
                                                                        && e.LastQualificationDegree.QualificationDegreeID > 0
                                                                        && e.LastQualificationDegree.Weight == Candidate.LastQualificationDegree.Weight
                                                                        && e.CurrentJobSeniorityDays > 0).Count() > 0)
                            {
                                DistributeJobVacanciesByCurrentJobSeniority(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.HasSameLastQualification && e.PromotionRecordJobVacancy == null
                                                                        && e.TotalPoints == SameTotalPoints
                                                                        && e.LastQualificationDegree != null
                                                                        && e.LastQualificationDegree.QualificationDegreeID > 0
                                                                        && e.LastQualificationDegree.Weight == Candidate.LastQualificationDegree.Weight
                                                                        && e.CurrentJobSeniorityDays > 0).ToList(),
                                                                SameTotalPoints, Candidate.LastQualificationDegree.Weight);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                            // By LastJobSeniority
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null
                                                                            && e.LastQualificationDegree != null
                                                                            && e.LastQualificationDegree.QualificationDegreeID > 0
                                                                            && e.LastQualificationDegree.Weight == Candidate.LastQualificationDegree.Weight
                                                                            && e.LastJobSeniorityDays > 0).Count() > 0)
                            {
                                DistributeJobVacanciesByLastJobSeniority(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                            PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                                    && e.PromotionRecordJobVacancy == null
                                                                                                                    && e.LastQualificationDegree != null
                                                                                                                    && e.LastQualificationDegree.QualificationDegreeID > 0
                                                                                                                    && e.LastQualificationDegree.Weight == Candidate.LastQualificationDegree.Weight
                                                                                                                    && e.LastJobSeniorityDays > 0).ToList(), Candidate.TotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                            // By TotalExperience
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null
                                                                            && e.LastQualificationDegree != null
                                                                            && e.LastQualificationDegree.QualificationDegreeID > 0
                                                                            && e.LastQualificationDegree.Weight == Candidate.LastQualificationDegree.Weight
                                                                            && e.TotalExperience > 0).Count() > 0)
                            {
                                DistributeJobVacanciesByTotalExperience(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                        && e.PromotionRecordJobVacancy == null
                                                                                                        && e.LastQualificationDegree != null
                                                                                                        && e.LastQualificationDegree.QualificationDegreeID > 0
                                                                                                        && e.LastQualificationDegree.Weight == Candidate.LastQualificationDegree.Weight
                                                                                                        && e.TotalExperience > 0).ToList(), Candidate.TotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                            // ByExam
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null
                                                                            && e.LastQualificationDegree != null
                                                                            && e.LastQualificationDegree.QualificationDegreeID > 0
                                                                            && e.LastQualificationDegree.Weight == Candidate.LastQualificationDegree.Weight).Count() > 0)
                            {
                                DistributeJobVacanciesShouldBePromotedByExam(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                        && e.PromotionRecordJobVacancy == null
                                                                                                        && e.LastQualificationDegree != null
                                                                                                        && e.LastQualificationDegree.QualificationDegreeID > 0
                                                                                                        && e.LastQualificationDegree.Weight == Candidate.LastQualificationDegree.Weight).ToList(),
                                                                                                        Candidate.TotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function is a part of DistributeJobVacancies & DistributeJobVacanciesByLastQualification Function, this function only called with there is a tie b/w two or more candidates.
        /// This function assigned jobs on the basis on CurrentJobSeniorityDays
        /// Step - Check if HasSameCurrentJobSeniority = true then Calculate totalVacantJobs and totalCandidatesWithSameCurrentJobSeniority, 
        /// Step - If totalVacantJobs >= totalCandidatesWithSameCurrentJobSeniority then assign job to sameCurrentJobSeniority's candidates.
        /// Step - If totalVacantJobs lessThan totalCandidatesWithSameCurrentJobSeniority then check candidates must have LastJobSeniorityDays>0 and Call 'DistributeJobVacanciesByLastJobSeniority' function
        /// different mechanism (ByLastJobSeniorityDays, ByTotalExperienceDays)
        /// </summary>
        /// <param name="PromotionsRecordsJobsVacancies"></param>
        /// <param name="PromotionRecordIncludedEmployees"></param>
        /// <param name="SameTotalPoints"></param>
        /// <param name="LastQualificationDegreeWeight"></param>
        private void DistributeJobVacanciesByCurrentJobSeniority(List<PromotionsRecordsJobsVacanciesBLL> PromotionsRecordsJobsVacancies, List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployees,
                                                                    decimal SameTotalPoints, int LastQualificationDegreeWeight)
        {
            int JobVacanciesCount, HasSameCurrentJobSeniorityCount;

            #region Setting HasSameCurrentJobSeniority Column
            // Setting all Employees HasSameCurrentJobSeniority = false
            PromotionRecordIncludedEmployees.ForEach(j => j.HasSameCurrentJobSeniority = false);

            foreach (var CandidateCurrentJobSeniority in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.CurrentJobSeniorityDays > 0).ToList())
            {
                if (PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.CurrentJobSeniorityDays > 0).Count() > 1)
                {
                    foreach (var SameCandidate in PromotionRecordIncludedEmployees.Where(e => e.CurrentJobSeniorityDays == CandidateCurrentJobSeniority.CurrentJobSeniorityDays
                                                                                            && e.PromotionRecordJobVacancy == null).ToList())
                        SameCandidate.HasSameCurrentJobSeniority = true;
                }
            }
            #endregion

            foreach (PromotionsRecordsJobsVacanciesBLL JobVacancy in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
            {
                if (JobVacancy.IsAssigned) continue;

                foreach (PromotionsRecordsEmployeesBLL Candidate in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null).OrderByDescending(e => e.CurrentJobSeniorityDays))
                {
                    // This candidate has unique 'Current Job Seniority'
                    if (Candidate.HasSameCurrentJobSeniority == false)
                    {
                        Candidate.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancy.PromotionRecordJobVacancyID };
                        JobVacancy.IsAssigned = true;
                        Candidate.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByCurrentJobSeniority };
                        Candidate.IsRedistributeJob = true;
                        break;
                    }
                    else
                    {
                        // Some Candidates has Same 'Current Job Seniority'
                        JobVacanciesCount = PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count();
                        HasSameCurrentJobSeniorityCount = PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.CurrentJobSeniorityDays == Candidate.CurrentJobSeniorityDays).Count();

                        // if more 'job vacancies' available then assign jobVacancies to 'Same Current Job Seniority's Candidate'
                        if (JobVacanciesCount >= HasSameCurrentJobSeniorityCount)
                        {
                            foreach (var JobVacancyForSameCurrentJobSeniority in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
                            {
                                foreach (var CandidateForSameCurrentJobSeniority in PromotionRecordIncludedEmployees.Where(e => e.CurrentJobSeniorityDays == Candidate.CurrentJobSeniorityDays
                                                                                                                                && e.PromotionRecordJobVacancy == null).ToList())
                                {
                                    CandidateForSameCurrentJobSeniority.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancyForSameCurrentJobSeniority.PromotionRecordJobVacancyID };
                                    JobVacancyForSameCurrentJobSeniority.IsAssigned = true;
                                    CandidateForSameCurrentJobSeniority.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByCurrentJobSeniority };
                                    Candidate.IsRedistributeJob = true;
                                    break;
                                }
                            }
                            break;
                        }
                        else    // if vacancies count is less than 'Same Current Job Seniority's Candidate' 
                        // then System assign JobVacancise By different mechanism (ByLastJobSeniorityDays, ByTotalExperienceDays
                        {
                            // By LastJobSeniority
                            if (PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalPoints == SameTotalPoints
                                                                        && e.HasSameCurrentJobSeniority
                                                                        && e.CurrentJobSeniorityDays == Candidate.CurrentJobSeniorityDays
                                                                        && e.LastJobSeniorityDays > 0).Count() > 0)
                            {
                                DistributeJobVacanciesByLastJobSeniority(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalPoints == SameTotalPoints
                                                                                                        && e.HasSameCurrentJobSeniority
                                                                                                        && e.CurrentJobSeniorityDays == Candidate.CurrentJobSeniorityDays
                                                                                                        && e.LastJobSeniorityDays > 0).ToList(), SameTotalPoints);

                            }

                            // By TotalExperience
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null
                                                                            && e.HasSameCurrentJobSeniority
                                                                            && e.CurrentJobSeniorityDays == Candidate.CurrentJobSeniorityDays
                                                                            && e.TotalExperience > 0).Count() > 0)
                            {
                                DistributeJobVacanciesByTotalExperience(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                        && e.PromotionRecordJobVacancy == null
                                                                                                        && e.HasSameCurrentJobSeniority
                                                                                                        && e.CurrentJobSeniorityDays == Candidate.CurrentJobSeniorityDays
                                                                                                        && e.TotalExperience > 0).ToList(), Candidate.TotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                            // ByExam
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null
                                                                            && e.HasSameCurrentJobSeniority
                                                                            && e.CurrentJobSeniorityDays == Candidate.CurrentJobSeniorityDays).Count() > 0)
                            {
                                DistributeJobVacanciesShouldBePromotedByExam(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                                                        && e.PromotionRecordJobVacancy == null
                                                                                                        && e.HasSameCurrentJobSeniority
                                                                                                        && e.CurrentJobSeniorityDays == Candidate.CurrentJobSeniorityDays).ToList(),
                                                                                                        Candidate.TotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PromotionsRecordsJobsVacancies"></param>
        /// <param name="PromotionRecordIncludedEmployees"></param>
        /// <param name="SameTotalPoints"></param>
        private void DistributeJobVacanciesByLastJobSeniority(List<PromotionsRecordsJobsVacanciesBLL> PromotionsRecordsJobsVacancies, List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployees,
                                                                decimal SameTotalPoints)
        {
            int JobVacanciesCount, HasSameLastJobSeniorityCount;

            #region Setting HasSameLastJobSeniority Column
            // Setting all Employees HasSameLastJobSeniority = false
            PromotionRecordIncludedEmployees.ForEach(j => j.HasSameLastJobSeniority = false);

            foreach (var CandidateLastJobSeniority in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.LastJobSeniorityDays > 0).ToList())
            {
                if (PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.LastJobSeniorityDays > 0).Count() > 1)
                {
                    foreach (var SameCandidate in PromotionRecordIncludedEmployees.Where(e => e.LastJobSeniorityDays == CandidateLastJobSeniority.LastJobSeniorityDays
                                                                                            && e.PromotionRecordJobVacancy == null).ToList())
                        SameCandidate.HasSameLastJobSeniority = true;
                }
            }
            #endregion

            foreach (PromotionsRecordsJobsVacanciesBLL JobVacancy in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
            {
                if (JobVacancy.IsAssigned) continue;

                foreach (PromotionsRecordsEmployeesBLL Candidate in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null).OrderByDescending(e => e.LastJobSeniorityDays))
                {
                    // This candidate has unique 'Last Job Seniority'
                    if (Candidate.HasSameLastJobSeniority == false)
                    {
                        Candidate.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancy.PromotionRecordJobVacancyID };
                        JobVacancy.IsAssigned = true;
                        Candidate.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByLastJobSeniority };
                        Candidate.IsRedistributeJob = true;
                        break;
                    }
                    else
                    {
                        // Some Candidates has Same 'Last Job Seniority'
                        JobVacanciesCount = PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count();
                        HasSameLastJobSeniorityCount = PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.LastJobSeniorityDays == Candidate.LastJobSeniorityDays).Count();

                        // if more 'job vacancies' available then assign jobVacancies to 'Same Current Job Seniority's Candidate'
                        if (JobVacanciesCount >= HasSameLastJobSeniorityCount)
                        {
                            foreach (var JobVacancyForSameLastJobSeniority in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
                            {
                                foreach (var CandidateForSameLastJobSeniority in PromotionRecordIncludedEmployees.Where(e => e.LastJobSeniorityDays == Candidate.LastJobSeniorityDays
                                                                                                                                && e.PromotionRecordJobVacancy == null).ToList())
                                {
                                    CandidateForSameLastJobSeniority.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancyForSameLastJobSeniority.PromotionRecordJobVacancyID };
                                    JobVacancyForSameLastJobSeniority.IsAssigned = true;
                                    CandidateForSameLastJobSeniority.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByLastJobSeniority };
                                    Candidate.IsRedistributeJob = true;
                                    break;
                                }
                            }
                            break;
                        }
                        else    // if vacancies count is less than 'Same Last Job Seniority's Candidate' 
                        // then System assign JobVacancise By different mechanism (ByTotalExperienceDays)
                        {
                            // By TotalExperience
                            if (PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalPoints == SameTotalPoints
                                                                        && e.HasSameLastJobSeniority
                                                                        && e.LastJobSeniorityDays == Candidate.LastJobSeniorityDays
                                                                        && e.TotalExperience > 0).Count() > 0)
                            {
                                DistributeJobVacanciesByTotalExperience(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalPoints == SameTotalPoints
                                                                                                        && e.HasSameLastJobSeniority
                                                                                                        && e.LastJobSeniorityDays == Candidate.LastJobSeniorityDays
                                                                                                        && e.TotalExperience > 0).ToList(), SameTotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                            // ByExam
                            if (PromotionRecordIncludedEmployees.Where(e => e.HasSamePoints && e.TotalPoints == Candidate.TotalPoints
                                                                            && e.PromotionRecordJobVacancy == null
                                                                            && e.HasSameLastJobSeniority
                                                                            && e.LastJobSeniorityDays == Candidate.LastJobSeniorityDays).Count() > 0)
                            {
                                DistributeJobVacanciesShouldBePromotedByExam(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalPoints == SameTotalPoints
                                                                                                        && e.HasSameLastJobSeniority
                                                                                                        && e.LastJobSeniorityDays == Candidate.LastJobSeniorityDays).ToList(), SameTotalPoints);

                                if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                    break;
                            }

                        }
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PromotionsRecordsJobsVacancies"></param>
        /// <param name="PromotionRecordIncludedEmployees"></param>
        /// <param name="SameTotalPoints"></param>
        private void DistributeJobVacanciesByTotalExperience(List<PromotionsRecordsJobsVacanciesBLL> PromotionsRecordsJobsVacancies, List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployees,
                                                                decimal SameTotalPoints)
        {
            int JobVacanciesCount, HasSameTotalExperienceCount;

            #region Setting HasSameTotalExperience Column
            // Setting all Employees HasSameTotalExperience = false
            PromotionRecordIncludedEmployees.ForEach(j => j.HasSameTotalExperience = false);

            foreach (var CandidateTotalExperience in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalExperience > 0).ToList())
            {
                if (PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalExperience > 0).Count() > 1)
                {
                    foreach (var SameCandidate in PromotionRecordIncludedEmployees.Where(e => e.TotalExperience == CandidateTotalExperience.TotalExperience
                                                                                            && e.PromotionRecordJobVacancy == null).ToList())
                        SameCandidate.HasSameTotalExperience = true;
                }
            }
            #endregion

            foreach (PromotionsRecordsJobsVacanciesBLL JobVacancy in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
            {
                if (JobVacancy.IsAssigned) continue;

                foreach (PromotionsRecordsEmployeesBLL Candidate in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null).OrderByDescending(e => e.TotalExperience))
                {
                    // This candidate has unique 'Total Experience'
                    if (Candidate.HasSameTotalExperience == false)
                    {
                        Candidate.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancy.PromotionRecordJobVacancyID };
                        JobVacancy.IsAssigned = true;
                        Candidate.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByTotalExperience };
                        Candidate.IsRedistributeJob = true;
                        break;
                    }
                    else
                    {
                        // Some Candidates has Same 'Total Experience'
                        JobVacanciesCount = PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count();
                        HasSameTotalExperienceCount = PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalExperience == Candidate.TotalExperience).Count();

                        // if more 'job vacancies' available then assign jobVacancies to 'Same TotalExperience's Candidate'
                        if (JobVacanciesCount >= HasSameTotalExperienceCount)
                        {
                            foreach (var JobVacancyForSameTotalExperience in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
                            {
                                foreach (var CandidateForSameTotalExperience in PromotionRecordIncludedEmployees.Where(e => e.TotalExperience == Candidate.TotalExperience
                                                                                                                                && e.PromotionRecordJobVacancy == null).ToList())
                                {
                                    CandidateForSameTotalExperience.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancyForSameTotalExperience.PromotionRecordJobVacancyID };
                                    JobVacancyForSameTotalExperience.IsAssigned = true;
                                    CandidateForSameTotalExperience.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByTotalExperience };
                                    Candidate.IsRedistributeJob = true;
                                    break;
                                }
                            }
                            break;
                        }
                        else    // if vacancies count is less than 'Same TotalExperience's Candidate' 
                        // then System assign JobVacancise By Exam
                        {
                            decimal? totalExamResult = PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalPoints == SameTotalPoints
                                                                                                    && e.HasSameTotalExperience
                                                                                                    && e.TotalExperience == Candidate.TotalExperience)
                                                                                        .Sum(x => x.ByExamResult.HasValue ? x.ByExamResult : 0);

                            // ByExam
                            if (totalExamResult.HasValue && totalExamResult.Value > 0)
                            {                            
                                DistributeJobVacanciesByExam(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalPoints == SameTotalPoints
                                                                                                        && e.HasSameTotalExperience
                                                                                                        && e.TotalExperience == Candidate.TotalExperience).ToList(), SameTotalPoints);
                            }
                            else
                            {
                                DistributeJobVacanciesShouldBePromotedByExam(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                                                                   PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null && e.TotalPoints == SameTotalPoints
                                                                                                           && e.HasSameTotalExperience
                                                                                                           && e.TotalExperience == Candidate.TotalExperience).ToList(), SameTotalPoints);
                            }

                            if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                                break;
                        }
                    }
                }
            }

        }


        private void DistributeJobVacanciesByExam(List<PromotionsRecordsJobsVacanciesBLL> PromotionsRecordsJobsVacancies, List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployees,
                                                                decimal SameTotalPoints)
        {
            int JobVacanciesCount, HasSameExamResultCount;

            #region Setting HasSameExamResult Column
            // Setting all Employees HasSameExamResult = false
            PromotionRecordIncludedEmployees.ForEach(j => j.HasSameExamResult = false);

            foreach (var CandidateTotalExperience in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null
                                                                                                && (e.ByExamResult.HasValue ? e.ByExamResult.Value : 0) > 0).ToList())
            {
                if (PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null
                                                                && (e.ByExamResult.HasValue ? e.ByExamResult.Value : 0) > 0).Count() > 1)
                {
                    foreach (var SameCandidate in PromotionRecordIncludedEmployees.Where(e => e.ByExamResult == CandidateTotalExperience.ByExamResult
                                                                                            && (e.ByExamResult.HasValue ? e.ByExamResult.Value : 0) > 0
                                                                                            && e.PromotionRecordJobVacancy == null).ToList())
                        SameCandidate.HasSameExamResult = true;
                }
            }
            #endregion

            foreach (PromotionsRecordsJobsVacanciesBLL JobVacancy in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
            {
                if (JobVacancy.IsAssigned) continue;

                foreach (PromotionsRecordsEmployeesBLL Candidate in PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null).OrderByDescending(e => (e.ByExamResult.HasValue ? e.ByExamResult.Value : 0)))
                {
                    // This candidate has unique 'Exam Result'
                    if (Candidate.HasSameExamResult == false)
                    {
                        Candidate.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancy.PromotionRecordJobVacancyID };
                        JobVacancy.IsAssigned = true;
                        Candidate.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByExam };
                        Candidate.IsRedistributeJob = true;
                        break;
                    }
                    else
                    {
                        // Some Candidates has Same 'Total Experience'
                        JobVacanciesCount = PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count();
                        HasSameExamResultCount = PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null 
                                                                                            && e.ByExamResult == Candidate.ByExamResult
                                                                                            && (e.ByExamResult.HasValue ? e.ByExamResult.Value : 0) > 0).Count();

                        // if more 'job vacancies' available then assign jobVacancies to 'Same ExamResult's Candidate'
                        if (JobVacanciesCount > 0)
                        {
                            foreach (var JobVacancyForSameTotalExperience in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
                            {
                                foreach (var CandidateForSameTotalExperience in PromotionRecordIncludedEmployees.Where(e => e.ByExamResult == Candidate.ByExamResult
                                                                                                                        && (e.ByExamResult.HasValue ? e.ByExamResult.Value : 0) > 0
                                                                                                                        && e.PromotionRecordJobVacancy == null).ToList())
                                {
                                    CandidateForSameTotalExperience.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = JobVacancyForSameTotalExperience.PromotionRecordJobVacancyID };
                                    JobVacancyForSameTotalExperience.IsAssigned = true;
                                    CandidateForSameTotalExperience.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ByExam };
                                    Candidate.IsRedistributeJob = true;
                                    break;
                                }
                            }
                            break;
                        }
                        //else    // if vacancies count is less than 'Same ExamResult's Candidate' 
                        //// then System assign JobVacancise 'Should Be By Exam'
                        //{
                        //    // ByExam
                        //    DistributeJobVacanciesShouldBePromotedByExam(PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList(),
                        //                                    PromotionRecordIncludedEmployees.Where(e => e.PromotionRecordJobVacancy == null 
                        //                                                                            && e.TotalPoints == SameTotalPoints
                        //                                                                            && e.HasSameExamResult
                        //                                                                            && e.ByExamResult == Candidate.ByExamResult
                        //                                                                            && (e.ByExamResult.HasValue ? e.ByExamResult.Value : 0) > 0).ToList(), SameTotalPoints);

                        //    if (PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).Count() <= 0)
                        //        break;
                        //}
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PromotionsRecordsJobsVacancies"></param>
        /// <param name="PromotionRecordIncludedEmployees"></param>
        /// <param name="SameTotalPoints"></param>
        private void DistributeJobVacanciesShouldBePromotedByExam(List<PromotionsRecordsJobsVacanciesBLL> PromotionsRecordsJobsVacancies, List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployees,
                                                                decimal SameTotalPoints)
        {
            //foreach (PromotionsRecordsJobsVacanciesBLL JobVacancy in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
            //{
            //    if (JobVacancy.IsAssigned) continue;

            foreach (PromotionsRecordsEmployeesBLL Candidate in PromotionRecordIncludedEmployees
                                                                        .Where(e => e.PromotionRecordJobVacancy == null && e.HasSamePoints)
                                                                        .OrderByDescending(e => e.TotalPoints))
            {
                Candidate.PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesBLL() { PromotionRecordJobVacancyID = 0 };      // setting temp to exit from the candidate-loop
                //JobVacancy.IsAssigned = true;
                Candidate.PromotionDecision = new PromotionsDecisionsBLL() { PromotionDecisionID = (int)PromotionsDecisionsEnum.ShouldBePromotedByExam };
                Candidate.IsRedistributeJob = true;
                Candidate.ByExamResult = 0;
                //break;
            }
            //}

            // set all jobVacancy as Assigned
            foreach (PromotionsRecordsJobsVacanciesBLL JobVacancy in PromotionsRecordsJobsVacancies.Where(j => j.IsAssigned == false).ToList())
                JobVacancy.IsAssigned = true;

            // Setting all PromotionRecordJobVacancy to null if setted as Zero
            PromotionRecordIncludedEmployees
                    .Where(e => e.PromotionRecordJobVacancy != null && e.PromotionRecordJobVacancy.PromotionRecordJobVacancyID == 0).ToList()
                    .ForEach(j => j.PromotionRecordJobVacancy = null);
        }

        /// <summary>
        /// Step 1: First needs to check LastGeneralSpecializaion is not null
        /// then Get pts from PromotionsRecordsQualificationPonits table based on LastGeneralSpecializaion & LastQualification
        /// Step 2: repeat step1 for LastQualification
        /// Step 3: repeat step1 for LastQualificationDegree
        /// Step 4: if step1, step2 & step3 fails then return 0
        /// </summary>
        /// <param name="Candidate"></param>
        /// <returns></returns>
        private decimal CalculateEducationPoints(PromotionsRecordsEmployeesBLL Candidate, List<PromotionsRecordsQualificationsPoints> QualificationsPointsList, List<EmployeesQualifications> EmployeeQualificationList)
        {
            decimal EduPoints = 0;

            if (QualificationsPointsList.Count <= 0 || EmployeeQualificationList.Count <= 0)
                return EduPoints;

            PromotionsRecordsQualificationsPoints QualPt;
            PromotionsRecordsQualificationsPoints NotIncluded;
            List<ComputeEducationPoints> CandidatePointsList = new List<ComputeEducationPoints>();
            Candidate.EducationPointsDetail = new List<PromotionsRecordsEmployeesEducationsDetailsBLL>();

            // get all empQualifications exists in QualPts and save into new list
            foreach (var item in EmployeeQualificationList.Where(eq => eq.EmployeesCodes.EmployeeCodeID == Candidate.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID &&
                                                                    (eq.QualificationsDegrees.Weight.HasValue ? eq.QualificationsDegrees.Weight.Value : 0) > 0))
            {
                QualPt = QualificationsPointsList.FirstOrDefault(eq => eq.QualificationDegreeID == item.QualificationDegreeID
                                                                        && eq.QualificationID == item.QualificationID
                                                                        && eq.GeneralSpecializationID == item.GeneralSpecializationID
                                                                        && eq.PromotionRecordQualificationPointID != (int)PromotionsRecordsQualificationsKindsEnum.NotIncluded
                                                                        && (eq.Points.HasValue ? eq.Points.Value : 0) > 0);

                if (QualPt != null && QualPt.PromotionRecordQualificationPointID > 0)
                    CandidatePointsList.Add(new ComputeEducationPoints()
                    {
                        PromotionRecordQualificationPointID = QualPt.PromotionRecordQualificationPointID,
                        Weight = item.QualificationsDegrees.Weight.Value,
                        PointsFromQualificationsPts = QualPt.Points.Value,
                        HasSameWeight = false,
                        IsProcessed = false
                    });
            }

            if (CandidatePointsList.Count <= 0)
                return EduPoints;

            // setting HasSameWeight = true 
            foreach (var item in CandidatePointsList)
            {
                if (item.HasSameWeight) continue;
                CandidatePointsList.Where(c => c.Weight == item.Weight).ToList().ForEach(x => x.HasSameWeight = true);
            }

            foreach (var item in CandidatePointsList)
            {
                if (item.IsProcessed) continue;

                if (!item.HasSameWeight)
                {
                    EduPoints = item.PointsFromQualificationsPts;

                    // saving how education points calculated
                    QualPt = QualificationsPointsList.FirstOrDefault(eq => eq.PromotionRecordQualificationPointID == item.PromotionRecordQualificationPointID);
                    if (QualPt != null && QualPt.QualificationDegreeID.HasValue && QualPt.QualificationID.HasValue && QualPt.GeneralSpecializationID.HasValue)
                    {
                        Candidate.EducationPointsDetail.Add(new PromotionsRecordsEmployeesEducationsDetailsBLL()
                        {
                            PromotionRecordEmployee = Candidate,
                            QualificationDegree = new QualificationsDegreesBLL() { QualificationDegreeID = QualPt.QualificationDegreeID.Value },
                            Qualification = new QualificationsBLL() { QualificationID = QualPt.QualificationID.Value },
                            GeneralSpecialization = new GeneralSpecializationsBLL() { GeneralSpecializationID = QualPt.GeneralSpecializationID.Value },
                            Weight = item.Weight,
                            Points = item.PointsFromQualificationsPts,
                            IsIncluded = true
                        });
                    }
                }
                else
                {
                    // has same weight
                    ComputeEducationPoints pt = CandidatePointsList.Where(c => c.Weight == item.Weight)
                                                                .OrderByDescending(c => c.PointsFromQualificationsPts).FirstOrDefault();
                    if (pt != null && pt.Weight > 0)
                    {
                        EduPoints += pt.PointsFromQualificationsPts;
                        CandidatePointsList.Where(c => c.Weight == item.Weight).ToList().ForEach(x => x.IsProcessed = true);

                        // saving how education points calculated
                        QualPt = QualificationsPointsList.FirstOrDefault(eq => eq.PromotionRecordQualificationPointID == pt.PromotionRecordQualificationPointID);
                        if (QualPt != null && QualPt.QualificationDegreeID.HasValue && QualPt.QualificationID.HasValue && QualPt.GeneralSpecializationID.HasValue)
                        {
                            Candidate.EducationPointsDetail.Add(new PromotionsRecordsEmployeesEducationsDetailsBLL()
                            {
                                PromotionRecordEmployee = Candidate,
                                QualificationDegree = new QualificationsDegreesBLL() { QualificationDegreeID = QualPt.QualificationDegreeID.Value },
                                Qualification = new QualificationsBLL() { QualificationID = QualPt.QualificationID.Value },
                                GeneralSpecialization = new GeneralSpecializationsBLL() { GeneralSpecializationID = QualPt.GeneralSpecializationID.Value },
                                Weight = pt.Weight,
                                Points = pt.PointsFromQualificationsPts,
                                IsIncluded = true
                            });
                        }

                        // saving how education points calculated: these records added to list to compare why these records in notIncluded in calculation of total points
                        foreach (var NotIncludedQual in CandidatePointsList.Where(c => c.Weight == item.Weight && c.PromotionRecordQualificationPointID != pt.PromotionRecordQualificationPointID).ToList())
                        {
                            NotIncluded = QualificationsPointsList.FirstOrDefault(x => x.PromotionRecordQualificationPointID == NotIncludedQual.PromotionRecordQualificationPointID);
                            if (NotIncluded != null && NotIncluded.QualificationDegreeID.HasValue && NotIncluded.QualificationID.HasValue && NotIncluded.GeneralSpecializationID.HasValue)
                            {
                                Candidate.EducationPointsDetail.Add(new PromotionsRecordsEmployeesEducationsDetailsBLL()
                                {
                                    PromotionRecordEmployee = Candidate,
                                    QualificationDegree = new QualificationsDegreesBLL() { QualificationDegreeID = NotIncluded.QualificationDegreeID.Value },
                                    Qualification = new QualificationsBLL() { QualificationID = NotIncluded.QualificationID.Value },
                                    GeneralSpecialization = new GeneralSpecializationsBLL() { GeneralSpecializationID = NotIncluded.GeneralSpecializationID.Value },
                                    Weight = NotIncludedQual.Weight,
                                    Points = NotIncludedQual.PointsFromQualificationsPts,
                                    IsIncluded = false
                                });
                            }
                        }
                    }
                }
            }

            EduPoints = (EduPoints > Candidate.MaxEducationPoints) ? Candidate.MaxEducationPoints : EduPoints;

            return EduPoints;
        }

        /// <summary>
        /// first we need to convert CurrentJobDaysCount into months (after substracting "354*3=3yearExps" from CurrentJobDaysCount)
        /// this divide the remaining by 30 to get months
        /// after that get points from SeniorityPointsDeclarations table according to months
        /// </summary>
        /// <param name="SenioritysPointsList"></param>
        /// <param name="Candidate"></param>
        /// <returns></returns>
        private decimal CalculateSeniorityPoints(PromotionsRecordsEmployeesBLL Candidate, List<SenioritysPointsDeclarations> SenioritysPointsList)
        {
            decimal SenPoints = 0;
            SenioritysPointsDeclarations SeniorityPointDeclaration;

            if (SenioritysPointsList.Count <= 0 && Candidate.CurrentJobDaysCount <= 0)
                return SenPoints;

            if (Candidate.CurrentJobSeniorityMonths > 0)
            {
                SeniorityPointDeclaration = SenioritysPointsList.FirstOrDefault(s => s.Months == Candidate.CurrentJobSeniorityMonths);
                if (SeniorityPointDeclaration != null && SeniorityPointDeclaration.SeniorityPointDeclarationID > 0)
                {
                    SenPoints = SeniorityPointDeclaration.Points;

                    // saving how seniority points calculated
                    Candidate.SeniorityPointsDetail = new List<PromotionsRecordsEmployeesSeniorityDetailsBLL>();
                    Candidate.SeniorityPointsDetail.Add(new PromotionsRecordsEmployeesSeniorityDetailsBLL()
                    {
                        PromotionRecordEmployee = Candidate,
                        Months = SeniorityPointDeclaration.Months,
                        Points = SeniorityPointDeclaration.Points
                    });

                }
            }


            return SenPoints;
        }

        /// <summary>
        /// Initial:
        /// Get Evaluation point from EmployeeEvaluations Table as per years definded EvaluationYears Property and EmployeeCodeID        
        /// Changes Dated: 18-02-2020
        /// Get last N years evaluation from EmployeeEvaluations based on property EvaluationYearNCountToIncludeInPromotionPreference
        /// </summary>
        /// <param name="Candidate"></param>
        /// <param name="EmployeeEvaluationList"></param>
        /// <returns></returns>
        private decimal CalculateEvaluationPoints(PromotionsRecordsEmployeesBLL Candidate, List<EmployeesEvaluations> EmployeeEvaluationList)
        {
            decimal EvalPoints = 0;
            List<EmployeesEvaluations> EmpEvalNLastYearsList = EmployeeEvaluationList
                                                                        .Where(e => e.EmployeeCodeID == Candidate.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID)
                                                                        .OrderByDescending(x => x.MaturityYearsBalances.MaturityYear)
                                                                        .Take(Candidate.EvaluationYearNCountToIncludeInPromotionPreference).ToList();


            if (EmpEvalNLastYearsList.Count <= 0 && Candidate.EvaluationYearNCountToIncludeInPromotionPreference <= 0)
                return EvalPoints;

            Candidate.EvaluationPointsDetail = new List<PromotionsRecordsEmployeesEvaluationsDetailsBLL>();
            foreach (EmployeesEvaluations EmployeeEvaluation in EmpEvalNLastYearsList)
            {
                if (EmployeeEvaluation != null && EmployeeEvaluation.EmployeeEvaluationID > 0)
                {
                    EvalPoints += (decimal)EmployeeEvaluation.EvaluationPoints.EvaluationPoint;

                    // saving how Evaluation Points calculated
                    Candidate.EvaluationPointsDetail.Add(new PromotionsRecordsEmployeesEvaluationsDetailsBLL()
                    {
                        PromotionRecordEmployee = Candidate,
                        EvaluationYear = EmployeeEvaluation.MaturityYearsBalances.MaturityYear,
                        Evaluation = EmployeeEvaluation.EvaluationPoints.Evaluation,
                        Points = (decimal)EmployeeEvaluation.EvaluationPoints.EvaluationPoint
                    });
                }
            }

            return EvalPoints;
        }

        internal List<PromotionsRecordsEmployees> MapPromotionRecordEmployeeBLLtoEntity(List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployeeList)
        {
            List<PromotionsRecordsEmployees> List = new List<PromotionsRecordsEmployees>();
            PromotionsRecordsEmployees Employee = new PromotionsRecordsEmployees();

            foreach (var item in PromotionRecordIncludedEmployeeList)
            {
                Employee = new PromotionsRecordsEmployees();
                Employee.PromotionRecordEmployeeID = item.PromotionRecordEmployeeID;
                Employee.EducationPoints = item.EducationPoints;
                Employee.SeniorityPoints = item.SeniorityPoints;
                Employee.EvaluationPoints = item.EvaluationPoints;
                Employee.ActualTaskPerformancePoints = item.ActualTaskPerformancePoints;
                Employee.PriorServiceDaysCount = item.PriorServiceDaysCount;
                Employee.OnGoingServiceDaysCount = item.OnGoingServiceDaysCount;
                Employee.PromotionRecordJobVacancyID = item.PromotionRecordJobVacancy != null ? item.PromotionRecordJobVacancy.PromotionRecordJobVacancyID : (int?)null;
                Employee.PromotionDecisionID = item.PromotionDecision != null ? item.PromotionDecision.PromotionDecisionID : (int?)null;
                Employee.RemovingReason = item.RemovingReason;
                Employee.RemovingBy = item.RemovingBy != null ? item.RemovingBy.EmployeeCodeID : (int?)null;
                Employee.IsRemovedAfterIncluding = item.IsRemovedAfterIncluding;

                if (item.SeniorityPointsDetail != null && item.SeniorityPointsDetail.Count > 0)
                {
                    Employee.PromotionsRecordsEmployeesSeniorityDetails = item.SeniorityPointsDetail.Select(x => new PromotionsRecordsEmployeesSeniorityDetails()
                    {
                        PromotionRecordEmployeeID = item.PromotionRecordEmployeeID,
                        Months = x.Months,
                        Points = x.Points.HasValue ? x.Points.Value : 0
                    }).ToList();
                }

                if (item.EvaluationPointsDetail != null && item.EvaluationPointsDetail.Count > 0)
                {
                    Employee.PromotionsRecordsEmployeesEvaluationsDetails = item.EvaluationPointsDetail.Select(x => new PromotionsRecordsEmployeesEvaluationsDetails()
                    {
                        PromotionRecordEmployeeID = item.PromotionRecordEmployeeID,
                        EvaluationYear = x.EvaluationYear,
                        Evaluation = x.Evaluation,
                        Points = x.Points.HasValue ? x.Points.Value : 0
                    }).ToList();
                }

                if (item.EducationPointsDetail != null && item.EducationPointsDetail.Count > 0)
                {
                    Employee.PromotionsRecordsEmployeesEducationsDetails = item.EducationPointsDetail.Select(x => new PromotionsRecordsEmployeesEducationsDetails()
                    {
                        PromotionRecordEmployeeID = item.PromotionRecordEmployeeID,
                        QualificationDegreeID = x.QualificationDegree.QualificationDegreeID,
                        QualificationID = x.Qualification.QualificationID,
                        GeneralSpecializationID = x.GeneralSpecialization.GeneralSpecializationID,
                        Weight = x.Weight,
                        Points = x.Points.HasValue ? x.Points.Value : 0,
                        IsIncluded = x.IsIncluded
                    }).ToList();
                }

                List.Add(Employee);
            }

            return List;
        }

        public Result UndoPreference()
        {
            try
            {
                Result result = null;

                PromotionsRecords PromotionRecord = new PromotionsRecordsDAL().GetByPromotionRecordID(this.PromotionRecordID);

                #region Validate Promotion Record Status must 'Preferenced'
                if (PromotionRecord.PromotionRecordStatusID != (int)PromotionsRecordsStatusEnum.Preferenced)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBePreferenced.ToString();
                    return result;
                }
                #endregion

                result = CommonValidation(result);

                #region Validate if there are candidates should be entered in promotion record
                List<PromotionsRecordsEmployees> CandidatesList = new List<PromotionsRecordsEmployees>();
                CandidatesList = new PromotionsRecordsEmployeesDAL().GetIncludedByPromotionRecordID(this.PromotionRecordID);

                if (CandidatesList == null || CandidatesList.Count == 0)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString();
                    return result;
                }
                else
                {
                    if (CandidatesList.Where(e => e.IsIncluded &&
                        (e.IsRemovedAfterIncluding.HasValue ? e.IsRemovedAfterIncluding.Value == false : 1 == 1)).Count() == 0)
                    {
                        result = new Result();
                        result.EnumType = typeof(PromotionsRecordsValidationEnum);
                        result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString();
                        return result;
                    }
                }

                #endregion

                #region undo preference
                PromotionRecord.PromotionRecordStatusID = (int)PromotionsRecordsStatusEnum.Open;
                PromotionRecord.PreferenceTime = null;
                PromotionRecord.PreferenceBy = null;
                PromotionRecord.LastUpdatedDate = DateTime.Now;
                PromotionRecord.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                PromotionRecord.PromotionsRecordsEmployees.ToList().ForEach(e =>
                {
                    e.PromotionRecordJobVacancyID = null;
                    e.EducationPoints = 0;
                    e.SeniorityPoints = 0;
                    e.EvaluationPoints = 0;
                    e.PriorServiceDaysCount = 0;
                    e.OnGoingServiceDaysCount = 0;
                    e.PromotionDecisionID = null;
                });

                new PromotionsRecordsDAL().UndoPreference(PromotionRecord);
                this.PromotionRecordStatus = new PromotionsRecordsStatusBLL().GetByPromotionRecordStatusID((int)PromotionsRecordsStatusEnum.Open);
                #endregion

                if (this.PromotionRecordID != 0)
                {
                    #region Adding Log
                    result = new PromotionsRecordsLogsBLL()
                    {
                        PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecord.PromotionRecordID },
                        PromotionRecordNo = PromotionRecord.PromotionRecordNo,
                        PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.UndoPreference },
                        ActionDescription = string.Empty,
                        LoginIdentity = this.LoginIdentity,
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

        public Result Install()
        {
            try
            {
                Result result = null;

                PromotionsRecords PromotionRecord = new PromotionsRecordsDAL().GetByPromotionRecordID(this.PromotionRecordID);

                #region Validate Promotion Record Status must 'Preferenced'
                if (PromotionRecord.PromotionRecordStatusID != (int)PromotionsRecordsStatusEnum.Preferenced)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBePreferenced.ToString();
                    return result;
                }
                #endregion

                result = CommonValidation(result);

                #region Validate if there are candidates should be entered in promotion record
                List<PromotionsRecordsEmployees> CandidatesList = new List<PromotionsRecordsEmployees>();
                CandidatesList = new PromotionsRecordsEmployeesDAL().GetIncludedByPromotionRecordID(this.PromotionRecordID);

                if (CandidatesList == null || CandidatesList.Count == 0)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString();
                    return result;
                }
                #endregion

                // Install
                PromotionRecord.PromotionRecordStatusID = (int)PromotionsRecordsStatusEnum.Installed;
                PromotionRecord.InstallationTime = DateTime.Now;
                PromotionRecord.InstallationBy = this.LoginIdentity.EmployeeCodeID;
                PromotionRecord.LastUpdatedDate = DateTime.Now;
                PromotionRecord.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;

                if (new PromotionsRecordsDAL().Install(PromotionRecord) > 0)
                {
                    PromotionRecord.PromotionsRecordsEmployees.Where(e => e.IsIncluded && e.PromotionRecordJobVacancyID != null).ToList().ForEach(e =>
                    {
                        new OrganizationsJobsBLL() { LoginIdentity = this.LoginIdentity }.UpdateIsReserved(e.PromotionsRecordsJobsVacancies.OrganizationJobID, true);
                    });

                    this.PromotionRecordStatus = new PromotionsRecordsStatusBLL().GetByPromotionRecordStatusID((int)PromotionsRecordsStatusEnum.Installed);

                    if (this.PromotionRecordID != 0)
                    {
                        #region Adding Log
                        result = new PromotionsRecordsLogsBLL()
                        {
                            PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecord.PromotionRecordID },
                            PromotionRecordNo = PromotionRecord.PromotionRecordNo,
                            PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.Install },
                            ActionDescription = string.Empty,
                            LoginIdentity = this.LoginIdentity,
                        }.Add();
                        #endregion

                        result = new Result();
                        result.Entity = this;
                        result.EnumType = typeof(PromotionsRecordsValidationEnum);
                        result.EnumMember = PromotionsRecordsValidationEnum.Done.ToString();
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public Result UndoInstall()
        {
            try
            {
                Result result = null;

                PromotionsRecords PromotionRecord = new PromotionsRecordsDAL().GetByPromotionRecordID(this.PromotionRecordID);

                #region Validate Promotion Record Status must 'Installed'
                if (PromotionRecord.PromotionRecordStatusID != (int)PromotionsRecordsStatusEnum.Installed)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeInstalled.ToString();
                    return result;
                }
                #endregion

                result = CommonValidation(result);

                #region Validate if there are candidates should be entered in promotion record
                List<PromotionsRecordsEmployees> CandidatesList = new List<PromotionsRecordsEmployees>();
                CandidatesList = new PromotionsRecordsEmployeesDAL().GetIncludedByPromotionRecordID(this.PromotionRecordID);

                if (CandidatesList == null || CandidatesList.Count == 0)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString();
                    return result;
                }
                #endregion

                #region Validate if PromotionRecord exists after current PromotionRecord in promotion period.                
                //// ??? remove this comments before deploy
                PromotionsRecords FirstInstalledPromotionRecord =
                                new PromotionsRecordsDAL().GetPromotionsRecordsByPromotionPeriodAndAfterPromotionRecordDate(PromotionRecord.PromotionPeriodID, PromotionRecord.CreationDate);

                if (FirstInstalledPromotionRecord != null && FirstInstalledPromotionRecord.PromotionRecordID > 0)
                {
                    result = new Result();
                    result.Entity = this.GetByPromotionRecordID(FirstInstalledPromotionRecord.PromotionRecordID);
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfAssignedJobCategoryIsInInstalledStatus.ToString();
                    return result;
                }
                #endregion

                // Undo Install
                PromotionRecord.PromotionRecordStatusID = (int)PromotionsRecordsStatusEnum.Preferenced;
                PromotionRecord.InstallationTime = null;
                PromotionRecord.InstallationBy = null;
                PromotionRecord.LastUpdatedDate = DateTime.Now;
                PromotionRecord.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;

                if (new PromotionsRecordsDAL().UndoInstall(PromotionRecord) > 0)
                {
                    PromotionRecord.PromotionsRecordsEmployees.Where(e => e.IsIncluded && e.PromotionRecordJobVacancyID != null).ToList().ForEach(e =>
                    {
                        new OrganizationsJobsBLL() { LoginIdentity = this.LoginIdentity }.UpdateIsReserved(e.PromotionsRecordsJobsVacancies.OrganizationJobID, false);
                    });

                    this.PromotionRecordStatus = new PromotionsRecordsStatusBLL().GetByPromotionRecordStatusID((int)PromotionsRecordsStatusEnum.Preferenced);

                    if (this.PromotionRecordID != 0)
                    {
                        #region Adding Log
                        result = new PromotionsRecordsLogsBLL()
                        {
                            PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecord.PromotionRecordID },
                            PromotionRecordNo = PromotionRecord.PromotionRecordNo,
                            PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.UndoInstall },
                            ActionDescription = string.Empty,
                            LoginIdentity = this.LoginIdentity,
                        }.Add();
                        #endregion

                        result = new Result();
                        result.Entity = this;
                        result.EnumType = typeof(PromotionsRecordsValidationEnum);
                        result.EnumMember = PromotionsRecordsValidationEnum.Done.ToString();
                    }
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        //private List<PromotionsRecordsBLL> GetPromotionsRecordsByStatusAndPromotionPeriod(int PromotionRecordStatusID, int PromotionPeriodID)
        //{
        //    List<PromotionsRecords> PromotionRecordList = new PromotionsRecordsDAL().GetPromotionsRecordsByStatusAndPromotionPeriod(PromotionRecordStatusID, PromotionPeriodID);
        //    List<PromotionsRecordsBLL> PromotionRecordBLLList = new List<PromotionsRecordsBLL>();
        //    if (PromotionRecordList.Count > 0)
        //        foreach (var item in PromotionRecordList)
        //            PromotionRecordBLLList.Add(new PromotionsRecordsBLL().MapPromotionRecord(item));

        //    return PromotionRecordBLLList;
        //}

        private Result CommonValidation(Result result)
        {
            #region Validate if there are job vacacies under the rank and category or not
            List<PromotionsRecordsJobsVacancies> PromotionRecordJobVacancyList = new PromotionsRecordsJobsVacanciesDAL().GetByPromotionRecordID(this.PromotionRecordID);
            if (PromotionRecordJobVacancyList.Count == 0)
            {
                result = new Result();
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString();
            }
            #endregion

            return result;
        }

    }

    public partial class PromotionsRecordsEmployeesBLL : CommonEntity, IEntity
    {
        // public int PromotionRecordEmployeeID { get; set; }

        //public PromotionsRecordsEmployeesBLL GetByPromotionRecordEmployeeID(int PromotionRecordEmployeeID)
        //{
        //    try
        //    {
        //        PromotionsRecordsEmployees PromotionsRecordsEmployees = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID);
        //        return new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(PromotionsRecordsEmployees);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public List<PromotionsRecordsEmployeesBLL> GetByPromotionRecordID(int PromotionRecordID, bool? IsIncluded, out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                List<PromotionsRecordsEmployees> PromotionsRecordsEmployeesList = new PromotionsRecordsEmployeesDAL()
                {
                    search = Search,
                    order = Order,
                    orderDir = OrderDir,
                    startRec = StartRec,
                    pageSize = PageSize
                }.GetByPromotionRecordID(PromotionRecordID, IsIncluded, out totalRecordsOut, out recFilterOut);

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

        public Result RemoveAndRedistributeJobs()
        {
            try
            {
                Result result = null;
                List<PromotionsRecordsEmployees> PromotionRecordEmployeeList = new List<PromotionsRecordsEmployees>();
                PromotionsRecordsEmployees PromotionRecordEmployee = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordEmployeeID(this.PromotionRecordEmployeeID);

                #region Validate Promotion Record Status must 'Open' or 'Preferenced'
                if (!(PromotionRecordEmployee.PromotionsRecords.PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Open ||
                        PromotionRecordEmployee.PromotionsRecords.PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Preferenced))
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpenOrPreferenced.ToString();
                    return result;
                }
                #endregion

                // Redistributes job call only if status is preferenced.
                if (PromotionRecordEmployee.PromotionsRecords.PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Preferenced)
                {
                    // get all job vacancies and in 'DistributeJobVacancies' we'll delete occupied jobs from list (in-memory)
                    List<PromotionsRecordsJobsVacanciesBLL> PromotionRecordJobVacanciesList = new PromotionsRecordsJobsVacanciesBLL().GetByPromotionRecordID(PromotionRecordEmployee.PromotionsRecords.PromotionRecordID);

                    // get all Included Employees
                    List<PromotionsRecordsEmployeesBLL> PromotionRecordIncludedEmployeeList
                                = new PromotionsRecordsEmployeesBLL().GetByPromotionRecordID(PromotionRecordEmployee.PromotionsRecords.PromotionRecordID)
                                                                                .Where(e => e.IsIncluded && !e.IsRemovedAfterIncluding).ToList();

                    // Setting all Employees HasSameTotalExperience = false
                    PromotionRecordIncludedEmployeeList.ForEach(j => j.IsRedistributeJob = false);

                    // setting PromotionRecordJobVacancyID empty/null in PromotionRecordIncludedEmployeeList (in-memory) to get this job available and assign to next eligible candidate.
                    PromotionsRecordsEmployeesBLL CurrentSelectedEmployeeToBeRemoved = PromotionRecordIncludedEmployeeList.FirstOrDefault(e => e.PromotionRecordEmployeeID == this.PromotionRecordEmployeeID);
                    if (CurrentSelectedEmployeeToBeRemoved != null && CurrentSelectedEmployeeToBeRemoved.PromotionRecordEmployeeID > 0)
                    {
                        CurrentSelectedEmployeeToBeRemoved.PromotionRecordJobVacancy = null;
                        CurrentSelectedEmployeeToBeRemoved.PromotionDecision = null;
                        CurrentSelectedEmployeeToBeRemoved.IsRemovedAfterIncluding = true;
                        CurrentSelectedEmployeeToBeRemoved.IsRedistributeJob = true;
                        CurrentSelectedEmployeeToBeRemoved.RemovingReason = this.RemovingReason;
                        CurrentSelectedEmployeeToBeRemoved.RemovingBy = this.LoginIdentity;
                        CurrentSelectedEmployeeToBeRemoved.LastUpdatedDate = DateTime.Now;
                        CurrentSelectedEmployeeToBeRemoved.LastUpdatedBy = this.LoginIdentity;
                    }

                    // Redistribute jobs now because one job is vacant from removal
                    result = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecordEmployee.PromotionsRecords.PromotionRecordID }
                                        .DistributeJobVacancies(PromotionRecordEmployee.PromotionsRecords, PromotionRecordJobVacanciesList, PromotionRecordIncludedEmployeeList);

                    // get list of candidate who removed and new assigned job
                    PromotionRecordEmployeeList = new PromotionsRecordsBLL().MapPromotionRecordEmployeeBLLtoEntity(PromotionRecordIncludedEmployeeList.Where(e => e.IsRedistributeJob).ToList());

                    PromotionRecordEmployeeList.ForEach(e => { e.LastUpdatedDate = DateTime.Now; e.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID; });

                    if (result != null)
                        return result;
                }
                else
                {
                    // Update cols as per removing logic
                    PromotionRecordEmployee.PromotionRecordJobVacancyID = null;
                    PromotionRecordEmployee.RemovingReason = this.RemovingReason;
                    PromotionRecordEmployee.RemovingBy = this.LoginIdentity.EmployeeCodeID;
                    PromotionRecordEmployee.PromotionDecisionID = null;
                    PromotionRecordEmployee.IsRemovedAfterIncluding = true;
                    PromotionRecordEmployee.LastUpdatedDate = DateTime.Now;
                    PromotionRecordEmployee.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                    PromotionRecordEmployeeList.Add(PromotionRecordEmployee);
                }

                if (new PromotionsRecordsEmployeesDAL().RemoveAndRedistributeJobs(PromotionRecordEmployeeList, this.LoginIdentity.EmployeeCodeID) > 0)
                {
                    #region Adding Log
                    string ActionDescription = string.Format(Globalization.PromotionRecordActionDescriptionRemoveCandidateAfterIncludeText,
                                                                                    PromotionRecordEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo,
                                                                                    PromotionRecordEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " +
                                                                                    PromotionRecordEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " +
                                                                                    PromotionRecordEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " +
                                                                                    PromotionRecordEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr);
                    result = new PromotionsRecordsLogsBLL()
                    {
                        PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecordEmployee.PromotionsRecords.PromotionRecordID },
                        PromotionRecordNo = PromotionRecordEmployee.PromotionsRecords.PromotionRecordNo,
                        PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.RemoveCandidateAfterInclude },
                        ActionDescription = ActionDescription,
                        LoginIdentity = this.LoginIdentity,
                    }.Add();
                    #endregion                    

                    result = new Result()
                    {
                        Entity = new PromotionsRecordsEmployeesBLL().GetByPromotionRecordEmployeeID(this.PromotionRecordEmployeeID),
                        EnumType = typeof(PromotionsRecordsEmployeesValidationEnum),
                        EnumMember = PromotionsRecordsEmployeesValidationEnum.Done.ToString()
                    };
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public Result IncludeAgainAfterRemoved()
        {
            Result result = null;
            PromotionsRecordsEmployees PromotionRecordEmployee = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordEmployeeID(this.PromotionRecordEmployeeID);

            //#region Validate Promotion Record Status must 'Open' or 'Preferenced'
            //if (!(PromotionRecordEmployee.PromotionsRecords.PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Open ||
            //        PromotionRecordEmployee.PromotionsRecords.PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Preferenced))
            #region Validate Promotion Record Status must 'Open'
            if (!(PromotionRecordEmployee.PromotionsRecords.PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Open))
            {
                result = new Result();
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpen.ToString();
                return result;
            }
            #endregion

            new PromotionsRecordsEmployeesDAL().IncludeAgainAfterRemoved(this.PromotionRecordEmployeeID, this.LoginIdentity.EmployeeCodeID);

            #region Adding Log
            string ActionDescription = string.Format(Globalization.PromotionRecordActionDescriptionIncludeCandidateAgainText,
                                                                            PromotionRecordEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo,
                                                                            PromotionRecordEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " +
                                                                            PromotionRecordEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " +
                                                                            PromotionRecordEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " +
                                                                            PromotionRecordEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr);
            result = new PromotionsRecordsLogsBLL()
            {
                PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecordEmployee.PromotionsRecords.PromotionRecordID },
                PromotionRecordNo = PromotionRecordEmployee.PromotionsRecords.PromotionRecordNo,
                PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.IncludeCandidateAgain },
                ActionDescription = ActionDescription,
                LoginIdentity = this.LoginIdentity,
            }.Add();
            #endregion 

            return new Result()
            {
                Entity = new PromotionsRecordsEmployeesBLL().GetByPromotionRecordEmployeeID(this.PromotionRecordEmployeeID),
                EnumType = typeof(PromotionsRecordsEmployeesValidationEnum),
                EnumMember = PromotionsRecordsEmployeesValidationEnum.Done.ToString()
            };
        }

        /// <summary>
        /// validation: first check NewEmployeeCodeNo exists in PromotionRecordEmployees (IsInclude=true && IsRemovedAfterInclude=false)
        /// shuffle job between PromotionRecordEmployeeID and NewEmployeeCodeNo
        /// </summary>
        /// <param name="PromotionRecordEmployeeID"></param>
        /// <param name="NewEmployeeCodeNo"></param>
        /// <returns></returns>
        public Result ShuffleJob(int PromotionRecordEmployeeID, string NewEmployeeCodeNo)
        {
            try
            {
                Result result = null;

                PromotionsRecordsEmployees CurrentEmployee = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID);

                #region Validate Promotion Record Status must 'Preferenced'
                if (CurrentEmployee.PromotionsRecords.PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Open ||
                    CurrentEmployee.PromotionsRecords.PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Closed)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBePreferencedOrInstalled.ToString();
                    return result;
                }
                #endregion

                #region Validate if 'NewEmployeeCodeNo' is exists in PromotionRecordEmployees
                PromotionsRecordsEmployees NewEmployee = new PromotionsRecordsEmployeesDAL().GetIncludedByPromotionRecordID(CurrentEmployee.PromotionsRecords.PromotionRecordID)
                                                                            .FirstOrDefault(e => (e.IsRemovedAfterIncluding.HasValue ? e.IsRemovedAfterIncluding.Value : false) == false
                                                                                                && e.CurrentEmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo == NewEmployeeCodeNo
                                                                                                && e.PromotionRecordJobVacancyID != null);


                if (NewEmployee == null)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfNewEmployeeCodeNoNotExitsInPromotionRecordEmployeesOrNotPromoted.ToString();
                    return result;
                }
                #endregion

                #region both Promotion Record Employee should be not approved
                bool CurrentEmployeeIsApproved, NewEmployeeIsApproved;
                CurrentEmployeeIsApproved = CurrentEmployee.IsApproved.HasValue ? CurrentEmployee.IsApproved.Value : false;
                NewEmployeeIsApproved = NewEmployee.IsApproved.HasValue ? NewEmployee.IsApproved.Value : false;
                if (CurrentEmployeeIsApproved || NewEmployeeIsApproved)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecauseOfOneOfEmployeeInRedistributeJobIsApproved.ToString();
                    return result;
                }
                #endregion

                // Shuffle Job 
                CurrentEmployee.LastUpdatedDate = DateTime.Now;
                CurrentEmployee.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                NewEmployee.LastUpdatedDate = DateTime.Now;
                NewEmployee.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;

                if (new PromotionsRecordsEmployeesDAL().ShuffleJob(CurrentEmployee, NewEmployee) > 0)
                {
                    #region Adding Log
                    string ActionDescription = string.Format(Globalization.PromotionRecordActionDescriptionRedistributeJobsText,
                                                                                    CurrentEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo,
                                                                                    CurrentEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " +
                                                                                    CurrentEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " +
                                                                                    CurrentEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " +
                                                                                    CurrentEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr,
                                                                                    CurrentEmployee.PromotionsRecordsJobsVacancies.OrganizationsJobs.JobNo,
                                                                                    CurrentEmployee.PromotionsRecordsJobsVacancies.OrganizationsJobs.Jobs.JobName,
                                                                                    NewEmployee.PromotionsRecordsJobsVacancies.OrganizationsJobs.JobNo,
                                                                                    NewEmployee.PromotionsRecordsJobsVacancies.OrganizationsJobs.Jobs.JobName,
                                                                                    NewEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo,
                                                                                    NewEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " +
                                                                                    NewEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " +
                                                                                    NewEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " +
                                                                                    NewEmployee.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr,
                                                                                    NewEmployee.PromotionsRecordsJobsVacancies.OrganizationsJobs.JobNo,
                                                                                    NewEmployee.PromotionsRecordsJobsVacancies.OrganizationsJobs.Jobs.JobName,
                                                                                    CurrentEmployee.PromotionsRecordsJobsVacancies.OrganizationsJobs.JobNo,
                                                                                    CurrentEmployee.PromotionsRecordsJobsVacancies.OrganizationsJobs.Jobs.JobName);

                    result = new PromotionsRecordsLogsBLL()
                    {
                        PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = CurrentEmployee.PromotionsRecords.PromotionRecordID },
                        PromotionRecordNo = CurrentEmployee.PromotionsRecords.PromotionRecordNo,
                        PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.RedistributeJobs },
                        ActionDescription = ActionDescription,//string.Format(Globalization.PromotionRecordActionDescriptionRedistributeJobsText, TotalJobAssignedToCandidates.ToString()),
                        LoginIdentity = this.LoginIdentity,
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

        public List<PromotionsRecordsEmployeesBLL> GetCandidatesForRedistributeJobsByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesBLLList = new List<PromotionsRecordsEmployeesBLL>();
                List<PromotionsRecordsEmployees> promotionsRecordsEmployees =
                    new PromotionsRecordsEmployeesDAL().GetByPromotionRecordID(PromotionRecordID).Where(c =>
                        c.PromotionRecordJobVacancyID != null &&
                        c.PromotionsDecisions != null
                    ).ToList();

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

        public List<PromotionsRecordsEmployeesBLL> GetCandidatesByExamByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesBLLList = new List<PromotionsRecordsEmployeesBLL>();
                List<PromotionsRecordsEmployees> promotionsRecordsEmployees =
                    new PromotionsRecordsEmployeesDAL().GetByPromotionRecordID(PromotionRecordID).Where(c =>
                        c.PromotionDecisionID == (int)PromotionsDecisionsEnum.ShouldBePromotedByExam
                    ).ToList();

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

        public Result UpdateActualTaskPerformancePoints()
        {
            Result result = null;
            PromotionsRecordsEmployees PromotionRecordEmployee = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordEmployeeID(this.PromotionRecordEmployeeID);
            int PromotionActionTypeID;

            #region Validate Promotion Record Status must 'Open'  
            if (PromotionRecordEmployee.PromotionsRecords.PromotionRecordStatusID != (int)PromotionsRecordsStatusEnum.Open)
            {
                result = new Result();
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpen.ToString();
                return result;
            }
            #endregion

            new PromotionsRecordsEmployeesDAL().UpdateActualTaskPerformancePoints(this.PromotionRecordEmployeeID,
                this.ActualTaskPerformancePoints,
                this.LoginIdentity.EmployeeCodeID);

            PromotionActionTypeID = this.ActualTaskPerformancePoints > 0 ?
                (int)PromotionsRecordsActionsTypesEnum.AddActualTaskPerformancePoints :
                (int)PromotionsRecordsActionsTypesEnum.RemoveActualTaskPerformancePoints;

            #region Adding Log            
            result = new PromotionsRecordsLogsBLL()
            {
                PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecordEmployee.PromotionsRecords.PromotionRecordID },
                PromotionRecordNo = PromotionRecordEmployee.PromotionsRecords.PromotionRecordNo,
                PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = PromotionActionTypeID },
                ActionDescription = string.Empty,
                LoginIdentity = this.LoginIdentity,
            }.Add();
            #endregion 

            return new Result()
            {
                Entity = new PromotionsRecordsEmployeesBLL().GetByPromotionRecordEmployeeID(this.PromotionRecordEmployeeID),
                EnumType = typeof(PromotionsRecordsEmployeesValidationEnum),
                EnumMember = PromotionsRecordsEmployeesValidationEnum.Done.ToString()
            };
        }

        /// <summary>
        /// validation: first check PromotionRecordStatus should be Preferenced, & PromotionDecision must be 'ShouldBePromotedByExam'
        /// </summary>
        /// <param name="PromotionRecordEmployeeID"></param>
        /// <param name="ExamResult"></param>
        /// <returns></returns>
        public Result UpdateExamResult(int PromotionRecordEmployeeID, decimal ExamResult)
        {
            Result result = null;
            PromotionsRecordsEmployees PromotionRecordEmployee = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID);

            #region Validate Promotion Record Status must 'Preferenced' & PromotionDecisionID ShouldBePromotedByExam
            if (PromotionRecordEmployee.PromotionsRecords.PromotionRecordStatusID != (int)PromotionsRecordsStatusEnum.Preferenced)
            {
                result = new Result();
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBePreferenced.ToString();
                return result;
            }
            else if (PromotionRecordEmployee.PromotionDecisionID != (int)PromotionsDecisionsEnum.ShouldBePromotedByExam)
            {
                result = new Result();
                result.EnumType = typeof(PromotionsRecordsValidationEnum);
                result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionDecisionMustBeShouldBePromotedByExam.ToString();
                return result;
            }
            #endregion

            new PromotionsRecordsEmployeesDAL().UpdateExamResult(PromotionRecordEmployeeID,
                ExamResult,
                this.LoginIdentity.EmployeeCodeID);

            #region Adding Log            
            result = new PromotionsRecordsLogsBLL()
            {
                PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecordEmployee.PromotionsRecords.PromotionRecordID },
                PromotionRecordNo = PromotionRecordEmployee.PromotionsRecords.PromotionRecordNo,
                PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.UpdateExamResult },
                ActionDescription = string.Empty,
                LoginIdentity = this.LoginIdentity,
            }.Add();
            #endregion 

            return new Result()
            {
                Entity = new PromotionsRecordsEmployeesBLL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID),
                EnumType = typeof(PromotionsRecordsEmployeesValidationEnum),
                EnumMember = PromotionsRecordsEmployeesValidationEnum.Done.ToString()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PromotionRecordID"></param>
        /// <returns></returns>
        public Result ResetByExamResults(int PromotionRecordID)
        {
            try
            {
                Result result = null;
                PromotionsRecords PromotionRecord = new PromotionsRecordsDAL().GetByPromotionRecordID(PromotionRecordID);

                #region Validate Promotion Record Status must 'Open'
                if (PromotionRecord.PromotionRecordStatusID != (int)PromotionsRecordsStatusEnum.Open)
                {
                    result = new Result();
                    result.EnumType = typeof(PromotionsRecordsValidationEnum);
                    result.EnumMember = PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpen.ToString();
                    return result;
                }
                #endregion

                List<PromotionsRecordsEmployees> PromotionRecordEmployees = new PromotionsRecordsEmployeesDAL().GetByPromotionRecordID(PromotionRecordID);

                PromotionRecordEmployees.ForEach(e =>
                {
                    e.ByExamResult = null;
                    e.LastUpdatedDate = DateTime.Now;
                    e.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;                    
                });

                if (new PromotionsRecordsEmployeesDAL().ResetByExamResults(PromotionRecordEmployees) > 0)
                {
                    #region Adding Log
                    result = new PromotionsRecordsLogsBLL()
                    {
                        PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecord.PromotionRecordID },
                        PromotionRecordNo = PromotionRecord.PromotionRecordNo,
                        PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.ResetExamResult},
                        ActionDescription = string.Empty,
                        LoginIdentity = this.LoginIdentity,
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

    }

    public class ComputeEducationPoints
    {
        public int PromotionRecordQualificationPointID { get; set; }
        public int Weight { get; set; }
        public decimal PointsFromQualificationsPts { get; set; }
        public bool HasSameWeight { get; set; }
        public bool IsProcessed { get; set; }
    }


}