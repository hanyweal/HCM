using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class EmployeesCareersHistoryDAL
    {
        public int Insert(EmployeesCareersHistory EmployeeCareerHistory)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeesCareersHistory.Add(EmployeeCareerHistory);
                    db.SaveChanges();
                    return EmployeeCareerHistory.EmployeeCareerHistoryID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(EmployeesCareersHistory EmployeeCareerHistory)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesCareersHistory EmployeesCareersHistoryObj = db.EmployeesCareersHistory.FirstOrDefault(x => x.EmployeeCareerHistoryID.Equals(EmployeeCareerHistory.EmployeeCareerHistoryID));
                    EmployeesCareersHistoryObj.OrganizationJobID = EmployeeCareerHistory.OrganizationJobID;
                    EmployeesCareersHistoryObj.TransactionStartDate = EmployeeCareerHistory.JoinDate;
                    EmployeesCareersHistoryObj.JoinDate = EmployeeCareerHistory.JoinDate;
                    EmployeesCareersHistoryObj.CareerHistoryTypeID = EmployeeCareerHistory.CareerHistoryTypeID;
                    EmployeesCareersHistoryObj.CareerDegreeID = EmployeeCareerHistory.CareerDegreeID;
                    EmployeesCareersHistoryObj.LastUpdatedDate = EmployeeCareerHistory.LastUpdatedDate;
                    EmployeesCareersHistoryObj.LastUpdatedBy = EmployeeCareerHistory.LastUpdatedBy;
                    EmployeesCareersHistoryObj.IsActive = EmployeeCareerHistory.IsActive;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateTransactionStartDate(EmployeesCareersHistory EmployeeCareerHistory)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesCareersHistory EmployeesCareersHistoryObj
                        = db.EmployeesCareersHistory.FirstOrDefault(x => x.EmployeeCareerHistoryID.Equals(EmployeeCareerHistory.EmployeeCareerHistoryID));
                    EmployeesCareersHistoryObj.TransactionStartDate = EmployeeCareerHistory.TransactionStartDate;
                    EmployeesCareersHistoryObj.JoinDate = EmployeeCareerHistory.JoinDate;
                    EmployeesCareersHistoryObj.LastUpdatedBy = EmployeeCareerHistory.LastUpdatedBy;
                    EmployeesCareersHistoryObj.LastUpdatedDate = EmployeeCareerHistory.LastUpdatedDate;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateIsActive(EmployeesCareersHistory EmployeeCareerHistory)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesCareersHistory EmployeesCareersHistoryObj = db.EmployeesCareersHistory.FirstOrDefault(x => x.EmployeeCareerHistoryID.Equals(EmployeeCareerHistory.EmployeeCareerHistoryID));
                    EmployeesCareersHistoryObj.IsActive = EmployeeCareerHistory.IsActive;
                    EmployeesCareersHistoryObj.LastUpdatedBy = EmployeeCareerHistory.LastUpdatedBy.HasValue ? EmployeeCareerHistory.LastUpdatedBy : EmployeesCareersHistoryObj.LastUpdatedBy;
                    EmployeesCareersHistoryObj.LastUpdatedDate = EmployeeCareerHistory.LastUpdatedDate;
                    return db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateCareerDegree(EmployeesCareersHistory EmployeeCareerHistory)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesCareersHistory EmployeesCareersHistoryObj = db.EmployeesCareersHistory.FirstOrDefault(x => x.EmployeeCareerHistoryID.Equals(EmployeeCareerHistory.EmployeeCareerHistoryID));
                    EmployeesCareersHistoryObj.CareerDegreeID = EmployeeCareerHistory.CareerDegreeID;
                    EmployeesCareersHistoryObj.LastUpdatedBy = EmployeeCareerHistory.LastUpdatedBy;
                    EmployeesCareersHistoryObj.LastUpdatedDate = EmployeeCareerHistory.LastUpdatedDate;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateCareerDegree(List<EmployeesCareersHistory> EmployeeCareerHistory)
        {
            try
            {
                EmployeesCareersHistory Emp;
                List<int> IDs = EmployeeCareerHistory.Select(e => e.EmployeeCareerHistoryID).ToList<int>();
                using (var db = new HCMEntities())
                {
                    var EmployeesCareersHistoryList = db.EmployeesCareersHistory.Where(e => IDs.Contains(e.EmployeeCareerHistoryID)).ToList();

                    foreach (var item in EmployeesCareersHistoryList)
                    {
                        Emp = EmployeeCareerHistory.FirstOrDefault(e => e.EmployeeCareerHistoryID == item.EmployeeCareerHistoryID);
                        if (Emp != null && Emp.EmployeeCareerHistoryID > 0)
                        {
                            item.CareerDegreeID = Emp.CareerDegreeID;
                            item.LastUpdatedBy = Emp.LastUpdatedBy;
                            item.LastUpdatedDate = Emp.LastUpdatedDate;
                        }
                    }

                    //db.EmployeesCareersHistory.Attach(item);
                    //DbEntityEntry<EmployeesCareersHistory> entry = db.Entry(item);
                    //entry.State = EntityState.Modified;

                    //EmployeesCareersHistory EmployeesCareersHistoryObj = db.EmployeesCareersHistory.FirstOrDefault(x => x.EmployeeCareerHistoryID.Equals(EmployeeCareerHistory.EmployeeCareerHistoryID));
                    //EmployeesCareersHistoryObj.CareerDegreeID = EmployeeCareerHistory.CareerDegreeID;
                    //EmployeesCareersHistoryObj.LastUpdatedBy = EmployeeCareerHistory.LastUpdatedBy;
                    //EmployeesCareersHistoryObj.LastUpdatedDate = EmployeeCareerHistory.LastUpdatedDate;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int EmployeeCareerHistoryID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesCareersHistory EmployeeCareerHistoryObj = db.EmployeesCareersHistory.FirstOrDefault(x => x.EmployeeCareerHistoryID.Equals(EmployeeCareerHistoryID));
                    db.EmployeesCareersHistory.Remove(EmployeeCareerHistoryObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public EmployeesCareersHistory GetEmployeesCareersHistoryByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .FirstOrDefault(x => x.EmployeeCareerHistoryID == EmployeeCareerHistoryID);
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCareersHistory> GetActiveEmployeesByRankID(int RankID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .Where(x => x.OrganizationsJobs.RankID == RankID && x.IsActive == true).ToList();
            }
            catch
            {

                throw;
            }
        }

        public List<EmployeesCareersHistory> GetEmployeesByOrganizationJobID(int OrganizationJobID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .Where(x => x.OrganizationsJobs.OrganizationJobID == OrganizationJobID).ToList();
            }
            catch
            {

                throw;
            }
        }

        public EmployeesCareersHistory GetActiveJobByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks.RanksCategories")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .FirstOrDefault(x => x.EmployeeCodeID == EmployeeCodeID && x.IsActive == true);
            }
            catch
            {
                throw;
            }
        }

        public EmployeesCareersHistory GetByEmployeeCodeIDRankID(int EmployeeCodeID, int RankID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks.RanksCategories")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .FirstOrDefault(x => x.EmployeeCodeID == EmployeeCodeID && x.OrganizationsJobs.RankID == RankID);
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCareersHistory> GetEmployeesCareersHistoryByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                        .Include("EmployeesCodes.Employees")
                        .Include("OrganizationsJobs.OrganizationsStructures")
                        .Include("OrganizationsJobs.Jobs")
                        .Include("OrganizationsJobs.Ranks.RanksCategories")
                        .Include("CareersHistoryTypes")
                        .Include("CareersDegrees")
                        .Where(x => x.EmployeeCodeID == EmployeeCodeID)
                        .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<spGetDeservedEmployeesToBeIncludedPromotion_Result> GetDeservedEmployeesToBeIncludedPromotion(int RankID, int JobCategoryID, int PromotionPeriodID)
        {
            using (var db = new HCMEntities())
            {
                return db.spGetDeservedEmployeesToBeIncludedPromotion(RankID, JobCategoryID, PromotionPeriodID).ToList();
            }
        }

        public List<EmployeesCareersHistory> GetByEmployeeCodeIDs(List<int> EmployeeCodeIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                                .Where(x => EmployeeCodeIDs.Contains(x.EmployeeCodeID))
                                .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCareersHistory> GetNotActiveJobsByRankID(int RankID, List<int> EmployeeCodeIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks.RanksCategories")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .Where(x => x.OrganizationsJobs.RankID == RankID && x.IsActive == false && EmployeeCodeIDs.Contains(x.EmployeeCodeID)).ToList();
            }
            catch
            {

                throw;
            }
        }

        public List<EmployeesCareersHistory> GetActiveEmployeesCareersHistory()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks.RanksCategories")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .Where(x => x.IsActive == true)
                       .OrderBy(x => x.EmployeeCodeID)
                       .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCareersHistory> GetActiveEmployeesCareersHistory(string EmployeeCodeNo)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks.RanksCategories")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .Where(x => x.IsActive == true && x.EmployeesCodes.EmployeeCodeNo == EmployeeCodeNo)
                       .OrderBy(x => x.EmployeeCodeID)
                       .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCareersHistory> GetHiringRecordsForEmployees()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks.RanksCategories")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .Where(x => x.CareersHistoryTypes.CareerHistoryTypeID == 1) // it mean Hiring
                       .OrderBy(x => x.EmployeeCodeID)
                       .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCareersHistory> GetHiringRecordsForEmployees(string EmployeeCodeNo)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks.RanksCategories")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .Where(x => x.CareersHistoryTypes.CareerHistoryTypeID == 1 && x.EmployeesCodes.EmployeeCodeNo == EmployeeCodeNo) // it mean Hiring
                       .OrderBy(x => x.EmployeeCodeID)
                       .ToList();
            }
            catch
            {
                throw;
            }
        }

        public EmployeesCareersHistory GetByOrganizationJobIDEmployeeCodeIDCareerHistoryTypeID(int OrganizationJobID, int CareerHistoryTypeID, int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                       .Include("EmployeesCodes.Employees")
                       .Include("OrganizationsJobs.OrganizationsStructures")
                       .Include("OrganizationsJobs.Jobs")
                       .Include("OrganizationsJobs.Ranks.RanksCategories")
                       .Include("CareersHistoryTypes")
                       .Include("CareersDegrees")
                       .FirstOrDefault(x => x.EmployeeCodeID == EmployeeCodeID && x.OrganizationsJobs.OrganizationJobID == OrganizationJobID && x.CareerHistoryTypeID == CareerHistoryTypeID);
            }
            catch
            {
                throw;
            }
        }

        //public List<EmployeesCareersHistory> GetAllPreviousEmployeeCarrersHistoryByEmployeeCodeID(int EmployeeCodeID)
        //{
        //    //List<spGetAllPreviousCareerByEmployeeCodeID_Result> allPreviousJobResult = new List<spGetAllPreviousCareerByEmployeeCodeID_Result>();
        //    List<EmployeesCareersHistory> allPreviousEmployeeCarrersHistory = new List<EmployeesCareersHistory>();
        //    //try
        //    //{
        //    //    using (var db = new HCMEntities())
        //    //    {
        //    //        allPreviousJobResult = db.spGetAllPreviousCareerByEmployeeCodeID(EmployeeCodeID).ToList();
        //    //    }
        //    //    allPreviousJobResult.ForEach(c => allPreviousEmployeeCarrersHistory.Add(new EmployeesCareersHistory() { EmployeeCareerHistoryID=c.EmployeeCareerHistoryID })
        //    //        );
        //    //    return allPreviousEmployeeCarrersHistory;
        //    //}
        //    //catch
        //    //{
        //    //    throw;
        //    //}
        //}

    }
}