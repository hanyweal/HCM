using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class OverTimesDetailsDAL
    {
        public int Delete(int OverTimeDetailID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OverTimesDetails OverTimeDetailObj = db.OverTimesDetails.SingleOrDefault(x => x.OverTimeDetailID.Equals(OverTimeDetailID));
                    db.OverTimesDetails.Remove(OverTimeDetailObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int Insert(OverTimesDetails OverTimeDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.OverTimesDetails.Add(OverTimeDetail);
                    db.SaveChanges();
                    return OverTimeDetail.OverTimeDetailID;
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

        public List<OverTimesDetails> GetOverTimesDetailsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OverTimesDetails.Include("OverTimes")
                                           .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                           .Include("OverTimes.OverTimesDays")
                                           .Where(x => x.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeID == EmployeeCodeID).ToList();

            }
            catch
            {
                throw;
            }
        }

        public List<OverTimesDetails> GetOverTimesDetailsByOverTimeID(int OverTimeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OverTimesDetails.Include("EmployeesCareersHistory.EmployeesCodes")
                                           .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                           .Where(x => x.OverTimeID == OverTimeID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public OverTimesDetails GetOverTimeDetailsByID(int OverTimeDetailID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OverTimesDetails.Include("EmployeesCareersHistory.EmployeesCodes")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                            .FirstOrDefault(x => x.OverTimeDetailID == OverTimeDetailID);
            }
            catch
            {
                throw;
            }
        }

        public List<OverTimesDetails> GetEmployeesOverTimesByDate(DateTime OverTimeDate, IList<int> EmployeesCareerHistoryIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.OverTimesDetails
                                            .Include("OverTimes")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                            .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks.RanksCategories")
                                            .Where(x => (x.OverTimes.OverTimeStartDate >= OverTimeDate)
                                                    && EmployeesCareerHistoryIDs.Contains(x.EmployeeCareerHistoryID)
                                            ).ToList();

            }
            catch
            {
                throw;
            }
        }
    }
}
