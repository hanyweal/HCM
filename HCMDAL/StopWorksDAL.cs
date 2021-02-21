using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class StopWorksDAL
    {
        public int Insert(StopWorks StopWork)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.StopWorks.Add(StopWork);
                    db.SaveChanges();
                    return StopWork.StopWorkID;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }
        }

        public int Update(StopWorks StopWork)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    StopWorks StopWorkObj = db.StopWorks.SingleOrDefault(x => x.StopWorkID.Equals(StopWork.StopWorkID));
                    StopWorkObj.StopWorkTypeID = StopWork.StopWorkTypeID;
                    StopWorkObj.StopWorkStartDate = StopWork.StopWorkStartDate;
                    StopWorkObj.StopWorkEndDate = StopWork.StopWorkEndDate;
                    StopWorkObj.StartStopWorkDecisionNumber = StopWork.StartStopWorkDecisionNumber;
                    StopWorkObj.StartStopWorkDecisionDate = StopWork.StartStopWorkDecisionDate;
                    StopWorkObj.EndStopWorkDecisionNumber = StopWork.EndStopWorkDecisionNumber;
                    StopWorkObj.EndStopWorkDecisionDate = StopWork.EndStopWorkDecisionDate;
                    StopWorkObj.StopPoint = StopWork.StopPoint;
                    StopWorkObj.IsConvicted = StopWork.IsConvicted;
                    StopWorkObj.Note = StopWork.Note;
                    StopWorkObj.LastUpdatedBy = StopWork.LastUpdatedBy;
                    StopWorkObj.LastUpdatedDate = StopWork.LastUpdatedDate;
                    StopWorkObj.EmployeeCareerHistoryID = StopWork.EmployeeCareerHistoryID;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int StopWorkID)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    StopWorks StopWorkObj = db.StopWorks.FirstOrDefault(x => x.StopWorkID.Equals(StopWorkID));
                    db.StopWorks.Remove(StopWorkObj);
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int EndStopWork(StopWorks StopWork)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    StopWorks StopWorkObj = db.StopWorks.SingleOrDefault(x => x.StopWorkID.Equals(StopWork.StopWorkID));
                    StopWorkObj.StopWorkEndDate = StopWork.StopWorkEndDate;
                    StopWorkObj.IsConvicted = StopWork.IsConvicted;
                    StopWorkObj.Note = StopWork.Note;
                    StopWorkObj.LastUpdatedBy = StopWork.LastUpdatedBy;
                    StopWorkObj.LastUpdatedDate = StopWork.LastUpdatedDate;
                    StopWorkObj.EmployeeCareerHistoryID = StopWork.EmployeeCareerHistoryID;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<StopWorks> GetEmployeeStopWorks(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.StopWorks.Include("StopWorksTypes")
                                    .Include("StopWorksCategories")
                                    .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<StopWorks> GetStopWorks()
        {
            try
            {
                var db = new HCMEntities();
                return db.StopWorks.Include("StopWorksTypes")
                          .Include("CreatedByNav.Employees").ToList();
            }
            catch
            {
                throw;
            }
        }

        public StopWorks GetByStopWorkID(int StopWorkID)
        {
            try
            {
                var db = new HCMEntities();
                return db.StopWorks.Include("StopWorksTypes")
                          .Include("CreatedByNav.Employees")
                                    .FirstOrDefault(x => x.StopWorkID == StopWorkID);
            }
            catch
            {
                throw;
            }
        }

        public List<StopWorks> GetStopWorksByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.StopWorks.Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                          .Include("CreatedByNav.Employees")
                                           .Where(x => x.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeID == EmployeeCodeID).ToList();

            }
            catch
            {
                throw;
            }
        }

        public List<StopWorks> GetStopWorksByEmployeeCareerHistoryID(int EmployeeCareerHistoryID, DateTime Date)
        {
            try
            {
                var db = new HCMEntities();
                return db.StopWorks.Where(x => x.EmployeeCareerHistoryID == EmployeeCareerHistoryID &&
                                            Date >= x.StopWorkStartDate && Date <= x.StopWorkEndDate).ToList();

            }
            catch
            {
                throw;
            }
        }

    }
}
