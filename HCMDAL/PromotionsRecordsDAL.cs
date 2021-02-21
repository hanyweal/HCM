using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsRecordsDAL : CommonEntityDAL
    {
        public int Insert(PromotionsRecords PromotionRecord)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;      // to increase performance
                    db.PromotionsRecords.Add(PromotionRecord);
                    db.Configuration.AutoDetectChangesEnabled = true;      // to increase performance
                    db.SaveChanges();
                    return PromotionRecord.PromotionRecordID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Refresh(PromotionsRecords PromotionRecord)
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
                            PromotionsRecords PromotionRecordObj = db.PromotionsRecords.FirstOrDefault(x => x.PromotionRecordID == PromotionRecord.PromotionRecordID);
                            PromotionRecordObj.PromotionsRecordsJobsVacancies.ToList()
                                                                        .ForEach(x => db.PromotionsRecordsJobsVacancies.Remove(x));
                            PromotionRecordObj.PromotionsRecordsQualificationsPoints.ToList()
                                                                        .ForEach(x => db.PromotionsRecordsQualificationsPoints.Remove(x));
                            PromotionRecordObj.PromotionsRecordsEmployees.ToList()
                                                                        .ForEach(x => db.PromotionsRecordsEmployees.Remove(x));
                            result = db.SaveChanges();

                            PromotionRecordObj.PromotionsRecordsJobsVacancies = PromotionRecord.PromotionsRecordsJobsVacancies;
                            PromotionRecordObj.PromotionsRecordsQualificationsPoints = PromotionRecord.PromotionsRecordsQualificationsPoints;
                            PromotionRecordObj.PromotionsRecordsEmployees = PromotionRecord.PromotionsRecordsEmployees;

                            result = db.SaveChanges();

                            dbContextTransaction.Commit();
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdatePromotionDate(PromotionsRecords PromotionRecord)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecords PromotionRecordObj = db.PromotionsRecords.SingleOrDefault(x => x.PromotionRecordID.Equals(PromotionRecord.PromotionRecordID));
                    PromotionRecordObj.PromotionDate = PromotionRecord.PromotionDate;
                    PromotionRecordObj.LastUpdatedBy = PromotionRecord.LastUpdatedBy;
                    PromotionRecordObj.LastUpdatedDate = PromotionRecord.LastUpdatedDate;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Preference(PromotionsRecords PromotionRecord)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    //db.Configuration.AutoDetectChangesEnabled = false;      // to increase performance

                    PromotionsRecordsEmployees PromotionRecordEmployeeObj;
                    PromotionsRecords PromotionRecordObj = db.PromotionsRecords.Include("PromotionsRecordsEmployees").FirstOrDefault(d => d.PromotionRecordID == PromotionRecord.PromotionRecordID);
                    if (PromotionRecordObj != null && PromotionRecordObj.PromotionRecordID > 0)
                    {
                        PromotionRecordObj.PromotionRecordStatusID = PromotionRecord.PromotionRecordStatusID;
                        PromotionRecordObj.PreferenceTime = PromotionRecord.PreferenceTime;
                        PromotionRecordObj.PreferenceBy = PromotionRecord.PreferenceBy;
                        PromotionRecordObj.LastUpdatedBy = PromotionRecord.LastUpdatedBy;
                        PromotionRecordObj.LastUpdatedDate = PromotionRecord.LastUpdatedDate;
                        foreach (var Candidate in PromotionRecord.PromotionsRecordsEmployees)
                        {
                            PromotionRecordEmployeeObj = PromotionRecordObj.PromotionsRecordsEmployees.FirstOrDefault(c => c.PromotionRecordEmployeeID == Candidate.PromotionRecordEmployeeID);
                            if (PromotionRecordEmployeeObj != null && PromotionRecordEmployeeObj.PromotionRecordEmployeeID > 0)
                            {
                                PromotionRecordEmployeeObj.PromotionRecordJobVacancyID = Candidate.PromotionRecordJobVacancyID;
                                PromotionRecordEmployeeObj.EducationPoints = Candidate.EducationPoints;
                                PromotionRecordEmployeeObj.SeniorityPoints = Candidate.SeniorityPoints;
                                PromotionRecordEmployeeObj.EvaluationPoints = Candidate.EvaluationPoints;
                                PromotionRecordEmployeeObj.ActualTaskPerformancePoints = Candidate.ActualTaskPerformancePoints;
                                PromotionRecordEmployeeObj.PriorServiceDaysCount = Candidate.PriorServiceDaysCount;
                                PromotionRecordEmployeeObj.OnGoingServiceDaysCount = Candidate.OnGoingServiceDaysCount;
                                PromotionRecordEmployeeObj.PromotionDecisionID = Candidate.PromotionDecisionID;

                                PromotionRecordEmployeeObj.PromotionsRecordsEmployeesEducationsDetails = Candidate.PromotionsRecordsEmployeesEducationsDetails;
                                PromotionRecordEmployeeObj.PromotionsRecordsEmployeesSeniorityDetails = Candidate.PromotionsRecordsEmployeesSeniorityDetails;
                                PromotionRecordEmployeeObj.PromotionsRecordsEmployeesEvaluationsDetails = Candidate.PromotionsRecordsEmployeesEvaluationsDetails;

                                PromotionRecordEmployeeObj.LastUpdatedBy = PromotionRecord.LastUpdatedBy;
                                PromotionRecordEmployeeObj.LastUpdatedDate = PromotionRecord.LastUpdatedDate;
                            }
                        }
                        //PromotionRecordObj.PromotionsRecordsEmployees = PromotionRecordObj.PromotionsRecordsEmployees;

                        //db.Configuration.AutoDetectChangesEnabled = true;      // to increase performance
                        //db.ChangeTracker.DetectChanges();
                        return db.SaveChanges();
                    }
                    else
                        throw new Exception();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UndoPreference(PromotionsRecords PromotionRecord)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    //db.Configuration.AutoDetectChangesEnabled = false;      // to increase performance

                    List<PromotionsRecordsEmployeesSeniorityDetails> SeniorityToBeRemoved = new List<PromotionsRecordsEmployeesSeniorityDetails>();
                    List<PromotionsRecordsEmployeesEducationsDetails> EducationToBeRemoved = new List<PromotionsRecordsEmployeesEducationsDetails>();
                    List<PromotionsRecordsEmployeesEvaluationsDetails> EvaluationToBeRemoved = new List<PromotionsRecordsEmployeesEvaluationsDetails>();

                    PromotionsRecordsEmployees PromotionRecordEmployeeObj;
                    PromotionsRecords PromotionRecordObj = db.PromotionsRecords
                                                                    .Include("PromotionsRecordsEmployees")
                                                                    .Include("PromotionsRecordsEmployees.PromotionsRecordsEmployeesSeniorityDetails")
                                                                    .Include("PromotionsRecordsEmployees.PromotionsRecordsEmployeesEvaluationsDetails")
                                                                    .Include("PromotionsRecordsEmployees.PromotionsRecordsEmployeesEducationsDetails")
                                                                    .FirstOrDefault(d => d.PromotionRecordID == PromotionRecord.PromotionRecordID);
                    if (PromotionRecordObj != null && PromotionRecordObj.PromotionRecordID > 0)
                    {
                        PromotionRecordObj.PromotionRecordStatusID = PromotionRecord.PromotionRecordStatusID;
                        PromotionRecordObj.PreferenceTime = PromotionRecord.PreferenceTime;
                        PromotionRecordObj.PreferenceBy = PromotionRecord.PreferenceBy;
                        PromotionRecordObj.LastUpdatedBy = PromotionRecord.LastUpdatedBy;
                        PromotionRecordObj.LastUpdatedDate = PromotionRecord.LastUpdatedDate;
                        foreach (var Candidate in PromotionRecord.PromotionsRecordsEmployees)
                        {
                            PromotionRecordEmployeeObj = PromotionRecordObj.PromotionsRecordsEmployees.FirstOrDefault(c => c.PromotionRecordEmployeeID == Candidate.PromotionRecordEmployeeID);
                            if (PromotionRecordEmployeeObj != null && PromotionRecordEmployeeObj.PromotionRecordEmployeeID > 0)
                            {
                                PromotionRecordEmployeeObj.PromotionRecordJobVacancyID = Candidate.PromotionRecordJobVacancyID;
                                PromotionRecordEmployeeObj.EducationPoints = Candidate.EducationPoints;
                                PromotionRecordEmployeeObj.SeniorityPoints = Candidate.SeniorityPoints;
                                PromotionRecordEmployeeObj.EvaluationPoints = Candidate.EducationPoints;
                                PromotionRecordEmployeeObj.ActualTaskPerformancePoints = Candidate.ActualTaskPerformancePoints;
                                PromotionRecordEmployeeObj.PriorServiceDaysCount = Candidate.PriorServiceDaysCount;
                                PromotionRecordEmployeeObj.PromotionDecisionID = Candidate.PromotionDecisionID;

                                // deleting PREmps Seniority
                                PromotionRecordEmployeeObj.PromotionsRecordsEmployeesSeniorityDetails.ToList()      //.RemoveAll(x => x.PromotionRecordEmployeeID == Candidate.PromotionRecordEmployeeID);
                                                                                                .ForEach(x => SeniorityToBeRemoved.Add(x));

                                // deleting PREmps Educations
                                PromotionRecordEmployeeObj.PromotionsRecordsEmployeesEducationsDetails.ToList()     //.RemoveAll(x => x.PromotionRecordEmployeeID == Candidate.PromotionRecordEmployeeID);
                                                                                                .ForEach(x => EducationToBeRemoved.Add(x));

                                // deleting PREmps Evaluations
                                PromotionRecordEmployeeObj.PromotionsRecordsEmployeesEvaluationsDetails.ToList()    //.RemoveAll(x => x.PromotionRecordEmployeeID == Candidate.PromotionRecordEmployeeID);
                                                                                                .ForEach(x => EvaluationToBeRemoved.Add(x));

                                PromotionRecordEmployeeObj.LastUpdatedBy = PromotionRecord.LastUpdatedBy;
                                PromotionRecordEmployeeObj.LastUpdatedDate = PromotionRecord.LastUpdatedDate;
                            }
                        }

                        //db.Configuration.AutoDetectChangesEnabled = true;      // to increase performance 
                        //db.ChangeTracker.DetectChanges();

                        if (SeniorityToBeRemoved.Count > 0)
                            db.PromotionsRecordsEmployeesSeniorityDetails.RemoveRange(SeniorityToBeRemoved);
                        if (EducationToBeRemoved.Count > 0)
                            db.PromotionsRecordsEmployeesEducationsDetails.RemoveRange(EducationToBeRemoved);
                        if (EvaluationToBeRemoved.Count > 0)
                            db.PromotionsRecordsEmployeesEvaluationsDetails.RemoveRange(EvaluationToBeRemoved);

                        return db.SaveChanges();
                    }
                    else
                        throw new Exception();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Install(PromotionsRecords PromotionRecord)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecords PromotionRecordObj = db.PromotionsRecords.FirstOrDefault(d => d.PromotionRecordID == PromotionRecord.PromotionRecordID);
                    if (PromotionRecordObj != null && PromotionRecordObj.PromotionRecordID > 0)
                    {
                        PromotionRecordObj.PromotionRecordStatusID = PromotionRecord.PromotionRecordStatusID;
                        PromotionRecordObj.InstallationTime = PromotionRecord.InstallationTime;
                        PromotionRecordObj.InstallationBy = PromotionRecord.InstallationBy;
                        PromotionRecordObj.LastUpdatedBy = PromotionRecord.LastUpdatedBy;
                        PromotionRecordObj.LastUpdatedDate = PromotionRecord.LastUpdatedDate;
                        return db.SaveChanges();
                    }
                    else
                        throw new Exception();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UndoInstall(PromotionsRecords PromotionRecord)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecords PromotionRecordObj = db.PromotionsRecords.FirstOrDefault(d => d.PromotionRecordID == PromotionRecord.PromotionRecordID);
                    if (PromotionRecordObj != null && PromotionRecordObj.PromotionRecordID > 0)
                    {
                        PromotionRecordObj.PromotionRecordStatusID = PromotionRecord.PromotionRecordStatusID;
                        PromotionRecordObj.InstallationTime = PromotionRecord.InstallationTime;
                        PromotionRecordObj.InstallationBy = PromotionRecord.InstallationBy;
                        PromotionRecordObj.LastUpdatedBy = PromotionRecord.LastUpdatedBy;
                        PromotionRecordObj.LastUpdatedDate = PromotionRecord.LastUpdatedDate;
                        return db.SaveChanges();
                    }
                    else
                        throw new Exception();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int PromotionRecordID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecords PromotionRecord = db.PromotionsRecords.FirstOrDefault(x => x.PromotionRecordID.Equals(PromotionRecordID));
                    db.PromotionsRecords.Remove(PromotionRecord);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecords> GetPromotionsRecords()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecords
                                .Include("Ranks")
                                .Include("JobsCategories")
                                .Include("PromotionsRecordsStatus")
                                .Include("PromotionsPeriods")
                                .Include("PromotionsPeriods.Periods")
                                .Include("PromotionsPeriods.MaturityYearsBalances")
                                .ToList();
            }
            catch
            {
                throw;
            }
        }

        public PromotionsRecords GetPromotionsRecordsByPromotionPeriodAndAfterPromotionRecordDate(int PromotionPeriodID, DateTime PromotionRecordCreationDateTime)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecords
                                .Include("Ranks")
                                .Include("JobsCategories")
                                .Include("PromotionsRecordsStatus")
                                .Include("PromotionsPeriods")
                                .Include("PromotionsPeriods.Periods")
                                .Include("PromotionsPeriods.MaturityYearsBalances")
                                .Where(x => x.PromotionPeriodID == PromotionPeriodID && x.CreationDate > PromotionRecordCreationDateTime)
                                .OrderBy(x => x.CreationDate)
                                .FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public PromotionsRecords GetByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecords
                                .Include("Ranks")
                                .Include("JobsCategories")
                                .Include("PromotionsRecordsStatus")
                                .Include("PromotionsPeriods")
                                .Include("PromotionsPeriods.Periods")
                                .Include("PromotionsRecordsEmployees")
                                .Include("CreatedByNav")
                                .FirstOrDefault(c => c.PromotionRecordID == PromotionRecordID);
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecords> GetByJobCategoryID(int JobCategoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecords.Include("Ranks")
                                            .Include("JobsCategories")
                                            .Include("PromotionsRecordsStatus")
                                            .Include("PromotionsPeriods")
                                            .Include("PromotionsPeriods.Periods")
                                            .Where(x => x.JobCategoryID == JobCategoryID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public int GetMaxPromotionRecordNo(int PromotionPeriodID)
        {
            try
            {
                var db = new HCMEntities();
                int result = db.PromotionsRecords.Where(x => x.PromotionPeriodID == PromotionPeriodID).Any() ? db.PromotionsRecords.Where(x => x.PromotionPeriodID == PromotionPeriodID).Max(x => x.PromotionRecordNo) : 0;
                return result + 1;

            }
            catch
            {
                throw;
            }
        }

        public int Close(PromotionsRecords PromotionRecordObj)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecords PromotionRecord = db.PromotionsRecords.Find(PromotionRecordObj.PromotionRecordID);
                    PromotionRecord.PromotionRecordStatusID = PromotionRecordObj.PromotionRecordStatusID;
                    PromotionRecord.ClosingBy = PromotionRecordObj.ClosingBy;
                    PromotionRecord.ClosingTime = PromotionRecordObj.ClosingTime;
                    PromotionRecord.LastUpdatedBy = PromotionRecordObj.LastUpdatedBy;
                    PromotionRecord.LastUpdatedDate = PromotionRecordObj.LastUpdatedDate;

                    foreach (PromotionsRecordsEmployees PromotionRecordEmployeeObj in PromotionRecordObj.PromotionsRecordsEmployees)
                    {
                        PromotionsRecordsEmployees PromotionRecordEmployee = db.PromotionsRecordsEmployees.Find(PromotionRecordEmployeeObj.PromotionRecordEmployeeID);
                        PromotionsRecordsJobsVacancies PromotionRecordJobVacancy = db.PromotionsRecordsJobsVacancies.Find(PromotionRecordEmployeeObj.PromotionsRecordsJobsVacancies.PromotionRecordJobVacancyID);
                        OrganizationsJobs OrganizationJobVacancy = db.OrganizationsJobs.Find(PromotionRecordEmployeeObj.PromotionsRecordsJobsVacancies.OrganizationsJobs.OrganizationJobID);
                        OrganizationJobVacancy.IsVacant = PromotionRecordEmployeeObj.PromotionsRecordsJobsVacancies.OrganizationsJobs.IsVacant;
                        OrganizationJobVacancy.IsReserved = PromotionRecordEmployeeObj.PromotionsRecordsJobsVacancies.OrganizationsJobs.IsReserved;
                        OrganizationJobVacancy.LastUpdatedBy = PromotionRecordEmployeeObj.PromotionsRecordsJobsVacancies.OrganizationsJobs.LastUpdatedBy.Value;
                        OrganizationJobVacancy.LastUpdatedDate = PromotionRecordEmployeeObj.PromotionsRecordsJobsVacancies.OrganizationsJobs.LastUpdatedDate;

                        EmployeesCareersHistory CurrentEmployeeCareerHistory = db.EmployeesCareersHistory.Find(PromotionRecordEmployeeObj.CurrentEmployeesCareersHistory.EmployeeCareerHistoryID);
                        CurrentEmployeeCareerHistory.IsActive = PromotionRecordEmployeeObj.CurrentEmployeesCareersHistory.IsActive;
                        CurrentEmployeeCareerHistory.LastUpdatedBy = PromotionRecordEmployeeObj.CurrentEmployeesCareersHistory.LastUpdatedBy.Value;
                        CurrentEmployeeCareerHistory.LastUpdatedDate = PromotionRecordEmployeeObj.CurrentEmployeesCareersHistory.LastUpdatedDate;
                        OrganizationsJobs CurrentOrganizationJob = db.OrganizationsJobs.Find(PromotionRecordEmployeeObj.CurrentEmployeesCareersHistory.OrganizationsJobs.OrganizationJobID);
                        CurrentOrganizationJob.IsVacant = PromotionRecordEmployeeObj.CurrentEmployeesCareersHistory.OrganizationsJobs.IsVacant;
                        CurrentOrganizationJob.LastUpdatedBy = PromotionRecordEmployeeObj.CurrentEmployeesCareersHistory.OrganizationsJobs.LastUpdatedBy.Value;
                        CurrentOrganizationJob.LastUpdatedDate = PromotionRecordEmployeeObj.CurrentEmployeesCareersHistory.OrganizationsJobs.LastUpdatedDate;

                        EmployeesCareersHistory NewEmployeeCareerHistory = new EmployeesCareersHistory();
                        NewEmployeeCareerHistory.EmployeeCodeID = PromotionRecordEmployeeObj.NewEmployeesCareersHistory.EmployeeCodeID;
                        NewEmployeeCareerHistory.CareerHistoryTypeID = PromotionRecordEmployeeObj.NewEmployeesCareersHistory.CareerHistoryTypeID;
                        NewEmployeeCareerHistory.OrganizationJobID = PromotionRecordEmployeeObj.NewEmployeesCareersHistory.OrganizationJobID;
                        NewEmployeeCareerHistory.JoinDate = PromotionRecordEmployeeObj.NewEmployeesCareersHistory.JoinDate;
                        NewEmployeeCareerHistory.CareerDegreeID = PromotionRecordEmployeeObj.NewEmployeesCareersHistory.CareerDegreeID;
                        NewEmployeeCareerHistory.TransactionStartDate = PromotionRecordEmployeeObj.NewEmployeesCareersHistory.TransactionStartDate;
                        NewEmployeeCareerHistory.CreatedBy = PromotionRecordEmployeeObj.NewEmployeesCareersHistory.CreatedBy;
                        NewEmployeeCareerHistory.CreatedDate = PromotionRecordEmployeeObj.NewEmployeesCareersHistory.CreatedDate;
                        NewEmployeeCareerHistory.IsActive = PromotionRecordEmployeeObj.NewEmployeesCareersHistory.IsActive;
                        PromotionRecordEmployee.NewEmployeesCareersHistory = NewEmployeeCareerHistory;

                    }
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

    }
}