using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsRecordsEmployeesDAL : CommonEntityDAL
    {
        public List<PromotionsRecordsEmployees> GetPromotionsRecordsEmployees()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsEmployees
                                .Include("PromotionsRecords")
                                .Include("CurrentEmployeesCareersHistory")
                                .Include("CurrentEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("NewEmployeesCareersHistory")
                                .Include("NewEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("PreviousEmployeesCareersHistory")
                                .Include("PreviousEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("PromotionsCandidatesAddedWays")
                                .Include("LastQualifications")
                                .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployees> GetByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            try
            {
                var db = new HCMEntities();

                return db.PromotionsRecordsEmployees
                                .Include("PromotionsRecords")
                                .Include("CurrentEmployeesCareersHistory")
                                .Include("CurrentEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("NewEmployeesCareersHistory")
                                .Include("NewEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("PreviousEmployeesCareersHistory")
                                .Include("PreviousEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("LastQualificationsDegrees")
                                .Include("LastQualifications")
                                .Include("LastGeneralSpecializations")
                                .Include("PromotionsCandidatesAddedWays")
                                .Include("PromotionsDecisions")
                                .Include("PromotionsRecordsJobsVacancies")
                                .Include("PromotionsRecordsJobsVacancies.OrganizationsJobs")
                                .Where(x => x.CurrentEmployeeCareerHistoryID == EmployeeCareerHistoryID || x.NewEmployeeCareerHistoryID == EmployeeCareerHistoryID).ToList();

            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployees> GetByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                var db = new HCMEntities();

                return db.PromotionsRecordsEmployees
                                .Include("PromotionsRecords")
                                .Include("PromotionsRecords.CreatedByNav")
                                .Include("CurrentEmployeesCareersHistory")
                                .Include("CurrentEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("NewEmployeesCareersHistory")
                                .Include("NewEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("PreviousEmployeesCareersHistory")
                                .Include("PreviousEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("LastQualificationsDegrees")
                                .Include("LastQualifications")
                                .Include("LastGeneralSpecializations")
                                .Include("PromotionsCandidatesAddedWays")
                                .Include("PromotionsDecisions")
                                .Include("PromotionsRecordsJobsVacancies")
                                .Include("PromotionsRecordsJobsVacancies.OrganizationsJobs")
                                .Where(x => x.PromotionRecordID == PromotionRecordID).ToList();

            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployees> GetIncludedByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                var db = new HCMEntities();

                return db.PromotionsRecordsEmployees
                                .Include("PromotionsRecords")
                                .Include("CurrentEmployeesCareersHistory")
                                .Include("CurrentEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("NewEmployeesCareersHistory")
                                .Include("NewEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("PreviousEmployeesCareersHistory")
                                .Include("PreviousEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("LastQualificationsDegrees")
                                .Include("LastQualifications")
                                .Include("LastGeneralSpecializations")
                                .Include("PromotionsCandidatesAddedWays")
                                .Include("PromotionsDecisions")
                                .Include("PromotionsRecordsJobsVacancies")
                                .Include("PromotionsRecordsJobsVacancies.OrganizationsJobs")
                                .Where(x => x.PromotionRecordID == PromotionRecordID && x.IsIncluded == true)
                                .ToList();

            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployees> GetByPromotionRecordID(int PromotionRecordID, bool? IsIncluded, out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();
                PromotionsRecords PromotionRecord = new PromotionsRecordsDAL().GetByPromotionRecordID(PromotionRecordID);

                var odData = db.PromotionsRecordsEmployees
                                .Include("PromotionsRecords")
                                .Include("CurrentEmployeesCareersHistory")
                                .Include("CurrentEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("NewEmployeesCareersHistory")
                                .Include("NewEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("PreviousEmployeesCareersHistory")
                                .Include("PreviousEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("PromotionsCandidatesAddedWays")
                                .Include("ApprovedByNav")
                                .Include("ApprovedByNav.Employees")
                                .Include("RemovingByNav")
                                .Include("RemovingByNav.Employees")
                                .Include("LastQualificationsDegrees")
                                .Include("LastQualifications")
                                .Include("LastGeneralSpecializations")
                                .Include("PromotionsDecisions")
                                .Include("PromotionsRecordsJobsVacancies")
                                .Include("PromotionsRecordsJobsVacancies.OrganizationsJobs")
                                .Where(x => x.PromotionRecordID == PromotionRecordID
                                        && (IsIncluded.HasValue ? x.IsIncluded == IsIncluded.Value : 1 == 1));

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<PromotionsRecordsEmployees> iq = odData.Where(p => p.PromotionRecordEmployeeID != null);

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                        (p.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " + p.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " + p.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " + p.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr).ToLower().Contains(search.ToLower()) ||
                        p.CurrentEmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo.ToLower().Contains(search.ToLower()) ||
                        p.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.EmployeeIDNo.ToLower().Contains(search.ToLower()) ||

                        p.CurrentEmployeesCareersHistory.OrganizationsJobs.JobNo.ToLower().Contains(search.ToLower()) ||
                        p.CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs.JobCode.ToLower().Contains(search.ToLower()) ||
                        p.CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs.JobName.ToLower().Contains(search.ToLower()) ||
                        p.CurrentEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures.OrganizationName.ToLower().Contains(search.ToLower()) ||
                        p.CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs.JobsCategories.JobCategoryName.ToLower().Contains(search.ToLower()) ||
                        p.CurrentEmployeesCareersHistory.OrganizationsJobs.Ranks.RankName.ToLower().Contains(search.ToLower()) ||

                        (p.NewEmployeesCareersHistory != null && (p.NewEmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " + p.NewEmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " + p.NewEmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " + p.NewEmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr).ToLower().Contains(search.ToLower())) ||
                        (p.NewEmployeesCareersHistory != null && p.NewEmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo.ToLower().Contains(search.ToLower())) ||
                        (p.NewEmployeesCareersHistory != null && p.NewEmployeesCareersHistory.EmployeesCodes.Employees.EmployeeIDNo.ToLower().Contains(search.ToLower())) ||
                        (p.NewEmployeesCareersHistory != null && p.CurrentEmployeesCareersHistory.OrganizationsJobs.JobNo.ToLower().Contains(search.ToLower())) ||
                        (p.NewEmployeesCareersHistory != null && p.CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs.JobCode.ToLower().Contains(search.ToLower())) ||
                        (p.NewEmployeesCareersHistory != null && p.CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs.JobName.ToLower().Contains(search.ToLower())) ||
                        (p.NewEmployeesCareersHistory != null && p.CurrentEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures.OrganizationName.ToLower().Contains(search.ToLower())) ||
                        (p.NewEmployeesCareersHistory != null && p.CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs.JobsCategories.JobCategoryName.ToLower().Contains(search.ToLower())) ||
                        (p.NewEmployeesCareersHistory != null && p.CurrentEmployeesCareersHistory.OrganizationsJobs.Ranks.RankName.ToLower().Contains(search.ToLower()))

                    );
                }

                // Sorting.
                if (PromotionRecord.PromotionRecordStatusID == 1)        // Open
                {
                    iq = iq.OrderBy(p => (p.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " + p.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " + p.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " + p.CurrentEmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr));
                }
                else
                {
                    iq = iq.OrderBy(p => p.PromotionDecisionID.HasValue ? p.PromotionDecisionID.Value : 9999)
                        .OrderByDescending(p => new
                        {
                            //Included = (p.IsIncluded ? 1 : 0) + (p.IsRemovedAfterIncluding.HasValue ? (p.IsRemovedAfterIncluding.Value ? 0 : 1) : 1),
                            //PromotionDecisionID = p.PromotionDecisionID.HasValue ? p.PromotionDecisionID.Value : 999,
                            TotalPoints = (p.EducationPoints.HasValue ? p.EducationPoints.Value : 0) +
                                        (p.SeniorityPoints.HasValue ? p.SeniorityPoints.Value : 0) +
                                        (p.EvaluationPoints.HasValue ? p.EvaluationPoints.Value : 0) +
                                        (p.ActualTaskPerformancePoints),
                            LastQualification = p.LastQualificationsDegrees != null ? (p.LastQualificationsDegrees.Weight.HasValue ? p.LastQualificationsDegrees.Weight.Value : 0) : 0,
                            CurrentJobDaysCount = p.CurrentJobDaysCount.HasValue ? p.CurrentJobDaysCount.Value : 0,
                            PreviousJobDaysCount = p.PreviousJobDaysCount,
                            TotalExperience = (p.OnGoingServiceDaysCount.HasValue ? p.OnGoingServiceDaysCount.Value : 0) + (p.PriorServiceDaysCount.HasValue ? p.PriorServiceDaysCount.Value : 0),
                            ByExamResults = (p.ByExamResult.HasValue ? p.ByExamResult.Value : 0)
                        });
                }

                // Filter record count.
                recFilterOut = iq.Count();

                // Apply pagination.
                iq = iq.Skip(startRec).Take(pageSize);
                //Get list of data
                var data = iq.ToList();

                return data;

            }
            catch
            {
                throw;
            }
        }

        public PromotionsRecordsEmployees GetByPromotionRecordEmployeeID(int PromotionRecordEmployeeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsEmployees
                                .Include("PromotionsRecords")
                                .Include("CurrentEmployeesCareersHistory")
                                .Include("CurrentEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("NewEmployeesCareersHistory")
                                .Include("NewEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("NewEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("PreviousEmployeesCareersHistory")
                                .Include("PreviousEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("PreviousEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("PromotionsCandidatesAddedWays")
                                .Include("LastQualifications")
                                .FirstOrDefault(x => x.PromotionRecordEmployeeID == PromotionRecordEmployeeID);
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int PromotionRecordEmployeeID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecordsEmployees PromotionRecordEmployeeObj = db.PromotionsRecordsEmployees.FirstOrDefault(x => x.PromotionRecordEmployeeID.Equals(PromotionRecordEmployeeID));
                    db.PromotionsRecordsEmployees.Remove(PromotionRecordEmployeeObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int DeleteByPromotionRecordID(int PromotionRecordID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    List<PromotionsRecordsEmployees> PromotionRecordEmployeeObj = db.PromotionsRecordsEmployees.Where(x => x.PromotionRecordID.Equals(PromotionRecordID)).ToList();
                    db.PromotionsRecordsEmployees.RemoveRange(PromotionRecordEmployeeObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int Insert(PromotionsRecordsEmployees PromotionRecordEmployee)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PromotionsRecordsEmployees.Add(PromotionRecordEmployee);
                    db.SaveChanges();
                    return PromotionRecordEmployee.PromotionRecordEmployeeID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Insert(List<PromotionsRecordsEmployees> PromotionRecordEmployee)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PromotionsRecordsEmployees.AddRange(PromotionRecordEmployee);
                    return db.SaveChanges();
                    //return PromotionRecordEmployee.PromotionRecordEmployeeID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateApproval(PromotionsRecordsEmployees PromotionRecordEmployee)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecordsEmployees PromotionsRecordsEmployeesObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(PromotionRecordEmployee.PromotionRecordEmployeeID));
                    PromotionsRecordsEmployeesObj.IsApproved = PromotionRecordEmployee.IsApproved;
                    PromotionsRecordsEmployeesObj.ApprovedBy = PromotionRecordEmployee.ApprovedBy;
                    PromotionsRecordsEmployeesObj.ApprovedDate = PromotionRecordEmployee.ApprovedDate;
                    PromotionsRecordsEmployeesObj.LastUpdatedBy = PromotionRecordEmployee.LastUpdatedBy;
                    PromotionsRecordsEmployeesObj.LastUpdatedDate = PromotionRecordEmployee.LastUpdatedDate;

                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateDeserveExtraBonus(PromotionsRecordsEmployees PromotionRecordEmployee)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecordsEmployees PromotionsRecordsEmployeesObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(PromotionRecordEmployee.PromotionRecordEmployeeID));
                    PromotionsRecordsEmployeesObj.IsDeserveExtraBonus = PromotionRecordEmployee.IsDeserveExtraBonus;
                    PromotionsRecordsEmployeesObj.LastUpdatedBy = PromotionRecordEmployee.LastUpdatedBy;
                    PromotionsRecordsEmployeesObj.LastUpdatedDate = PromotionRecordEmployee.LastUpdatedDate;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int RemoveAndRedistributeJobs(List<PromotionsRecordsEmployees> PromotionRecordEmployeeList, int UserIdentity)
        {
            try
            {
                int result = 0;
                using (var db = new HCMEntities())
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var PromotionRecordEmployee in PromotionRecordEmployeeList.OrderBy(e => e.PromotionRecordJobVacancyID.HasValue ? e.PromotionRecordJobVacancyID.Value : 0))
                            {
                                PromotionsRecordsEmployees PromotionsRecordsEmployeesObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(PromotionRecordEmployee.PromotionRecordEmployeeID));
                                PromotionsRecordsEmployeesObj.PromotionRecordJobVacancyID = PromotionRecordEmployee.PromotionRecordJobVacancyID;
                                if (PromotionRecordEmployee.RemovingBy != null)
                                {
                                    PromotionsRecordsEmployeesObj.RemovingBy = PromotionRecordEmployee.RemovingBy;
                                    PromotionsRecordsEmployeesObj.RemovingReason = PromotionRecordEmployee.RemovingReason;
                                }
                                PromotionsRecordsEmployeesObj.IsRemovedAfterIncluding = PromotionRecordEmployee.IsRemovedAfterIncluding;
                                PromotionsRecordsEmployeesObj.PromotionDecisionID = PromotionRecordEmployee.PromotionDecisionID;
                                PromotionsRecordsEmployeesObj.LastUpdatedBy = PromotionRecordEmployee.LastUpdatedBy;
                                PromotionsRecordsEmployeesObj.LastUpdatedDate = PromotionRecordEmployee.LastUpdatedDate;

                                result = db.SaveChanges(UserIdentity);// db.PromotionsRecordsEmployees.AddOrUpdate(PromotionsRecordsEmployeesObj);

                            }
                            dbContextTransaction.Commit();

                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsEmployees> GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsEmployees
                                .Include("PromotionsRecords")
                                .Include("PromotionsRecordsJobsVacancies.OrganizationsJobs.Jobs")
                                .Include("CurrentEmployeesCareersHistory")
                                .Include("CurrentEmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Jobs")
                                .Include("CurrentEmployeesCareersHistory.OrganizationsJobs.Ranks")
                                .Include("PromotionsCandidatesAddedWays")
                                .Where(x => x.CurrentEmployeesCareersHistory.EmployeesCodes.EmployeeCodeID == EmployeeCodeID
                                && x.IsIncluded == true).ToList();
            }
            catch
            {
                throw;
            }
        }

        public int IncludeAgainAfterRemoved(int PromotionRecordEmployeeID, int LastUpdatedBy)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecordsEmployees PromotionsRecordsEmployeesObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(PromotionRecordEmployeeID));
                    PromotionsRecordsEmployeesObj.PromotionRecordJobVacancyID = null;
                    PromotionsRecordsEmployeesObj.RemovingBy = null;
                    PromotionsRecordsEmployeesObj.RemovingReason = string.Empty;
                    PromotionsRecordsEmployeesObj.IsRemovedAfterIncluding = (bool?)null;
                    PromotionsRecordsEmployeesObj.PromotionDecisionID = null;
                    PromotionsRecordsEmployeesObj.LastUpdatedBy = LastUpdatedBy;
                    PromotionsRecordsEmployeesObj.LastUpdatedDate = DateTime.Now;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int ShuffleJob(PromotionsRecordsEmployees CurrentEmployee, PromotionsRecordsEmployees NewEmployee)
        {
            try
            {
                int result = 0;
                using (var db = new HCMEntities())
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {

                            int? CurrentPromotionRecordJobVacancyID = CurrentEmployee.PromotionRecordJobVacancyID;
                            int? NewPromotionRecordJobVacancyID = NewEmployee.PromotionRecordJobVacancyID;

                            // first make both objects vacancyID = null
                            PromotionsRecordsEmployees CurrentEmployeeObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(CurrentEmployee.PromotionRecordEmployeeID));
                            CurrentEmployeeObj.PromotionRecordJobVacancyID = null;
                            CurrentEmployeeObj.LastUpdatedBy = CurrentEmployee.LastUpdatedBy;
                            CurrentEmployeeObj.LastUpdatedDate = DateTime.Now;

                            PromotionsRecordsEmployees NewEmployeeObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(NewEmployee.PromotionRecordEmployeeID));
                            NewEmployeeObj.PromotionRecordJobVacancyID = null;
                            NewEmployeeObj.LastUpdatedBy = NewEmployeeObj.LastUpdatedBy;
                            NewEmployeeObj.LastUpdatedDate = DateTime.Now;

                            db.PromotionsRecordsEmployees.AddOrUpdate(CurrentEmployeeObj);
                            db.PromotionsRecordsEmployees.AddOrUpdate(NewEmployeeObj);

                            result = db.SaveChanges();

                            // now shuffle the vacancy ID

                            CurrentEmployeeObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(CurrentEmployee.PromotionRecordEmployeeID));
                            CurrentEmployeeObj.PromotionRecordJobVacancyID = NewPromotionRecordJobVacancyID;
                            CurrentEmployeeObj.LastUpdatedBy = CurrentEmployee.LastUpdatedBy;
                            CurrentEmployeeObj.LastUpdatedDate = DateTime.Now;

                            NewEmployeeObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(NewEmployee.PromotionRecordEmployeeID));
                            NewEmployeeObj.PromotionRecordJobVacancyID = CurrentPromotionRecordJobVacancyID;
                            NewEmployeeObj.LastUpdatedBy = NewEmployeeObj.LastUpdatedBy;
                            NewEmployeeObj.LastUpdatedDate = DateTime.Now;

                            db.PromotionsRecordsEmployees.AddOrUpdate(CurrentEmployeeObj);
                            db.PromotionsRecordsEmployees.AddOrUpdate(NewEmployeeObj);

                            result = db.SaveChanges();

                            dbContextTransaction.Commit();

                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateActualTaskPerformancePoints(int PromotionRecordEmployeeID, decimal ActualTaskPerformancePoints, int LastUpdatedBy)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecordsEmployees PromotionsRecordsEmployeesObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(PromotionRecordEmployeeID));
                    PromotionsRecordsEmployeesObj.ActualTaskPerformancePoints = ActualTaskPerformancePoints;
                    PromotionsRecordsEmployeesObj.LastUpdatedBy = LastUpdatedBy;
                    PromotionsRecordsEmployeesObj.LastUpdatedDate = DateTime.Now;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateExamResult(int PromotionRecordEmployeeID, decimal? ExamResult, int LastUpdatedBy)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecordsEmployees PromotionsRecordsEmployeesObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(PromotionRecordEmployeeID));
                    PromotionsRecordsEmployeesObj.ByExamResult = ExamResult;
                    PromotionsRecordsEmployeesObj.LastUpdatedBy = LastUpdatedBy;
                    PromotionsRecordsEmployeesObj.LastUpdatedDate = DateTime.Now;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        
        public int ResetByExamResults(List<PromotionsRecordsEmployees> PromotionRecordEmployeeList)
        {
            try
            {
                int result = 0;
                using (var db = new HCMEntities())
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var PromotionRecordEmployee in PromotionRecordEmployeeList.OrderBy(e => e.PromotionRecordJobVacancyID.HasValue ? e.PromotionRecordJobVacancyID.Value : 0))
                            {
                                PromotionsRecordsEmployees PromotionsRecordsEmployeesObj = db.PromotionsRecordsEmployees.SingleOrDefault(x => x.PromotionRecordEmployeeID.Equals(PromotionRecordEmployee.PromotionRecordEmployeeID));
                                PromotionsRecordsEmployeesObj.ByExamResult = PromotionRecordEmployee.ByExamResult;
                                PromotionsRecordsEmployeesObj.LastUpdatedBy = PromotionRecordEmployee.LastUpdatedBy;
                                PromotionsRecordsEmployeesObj.LastUpdatedDate = PromotionRecordEmployee.LastUpdatedDate;

                                result = db.SaveChanges();// db.PromotionsRecordsEmployees.AddOrUpdate(PromotionsRecordsEmployeesObj);

                            }
                            dbContextTransaction.Commit();

                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}