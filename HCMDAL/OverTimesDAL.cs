using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class OverTimesDAL : CommonEntityDAL
    {
        public int Insert(OverTimes OverTime)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.OverTimes.Add(OverTime);
                    db.SaveChanges();
                    return OverTime.OverTimeID;
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

        public int Update(OverTimes OverTime)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OverTimes OverTimeObj = db.OverTimes.SingleOrDefault(x => x.OverTimeID.Equals(OverTime.OverTimeID));
                    OverTimeObj.OverTimeID = OverTime.OverTimeID;
                    OverTimeObj.Tasks = OverTime.Tasks;
                    OverTimeObj.WeekWorkHoursAvg = OverTime.WeekWorkHoursAvg;
                    OverTimeObj.FridayHoursAvg = OverTime.FridayHoursAvg;
                    OverTimeObj.SaturdayHoursAvg = OverTime.SaturdayHoursAvg;
                    OverTimeObj.LastUpdatedDate = OverTime.LastUpdatedDate;
                    OverTimeObj.LastUpdatedBy = OverTime.LastUpdatedBy;
                    return db.SaveChanges();
                }

            }
            catch
            {
                throw;
            }
        }

        public int Delete(OverTimes OverTime, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OverTimes OverTimeObj = db.OverTimes.SingleOrDefault(x => x.OverTimeID.Equals(OverTime.OverTimeID));
                    OverTimeObj.OverTimesDetails.ToList().ForEach(d => db.OverTimesDetails.Remove(d));
                    db.OverTimes.Remove(OverTimeObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<OverTimes> GetOverTimes()
        {
            try
            {
                var db = new HCMEntities();
                return db.OverTimes.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<OverTimes> GetOverTimes(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();

                var odData = db.OverTimes;

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<OverTimes> iq = odData.Where(p => p.OverTimeID != null);

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                                       p.OverTimeID.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.Tasks.ToLower().Contains(search.ToLower()) //||
                                                                                    //System.Data.Objects.SqlClient.SqlFunctions.StringConvert(p.OverTimeStartDate).Contains(search.ToLower()) 
                                                                                    //p.OverTimeStartDate.ToString().ToLower().Contains(search.ToLower()) ||
                                                                                    //p.OverTimeEndDate.ToString().ToLower().Contains(search.ToLower()) 
                                       );
                }

                // Sorting.
                iq = iq.OrderBy(p => p.OverTimeID);

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

        public OverTimes GetByOverTimeID(int OverTimeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OverTimes.Include("CreatedByNav.Employees")
                                   .Include("LastUpdatedByNav.Employees")
                                   .SingleOrDefault(x => x.OverTimeID.Equals(OverTimeID));
            }
            catch
            {
                throw;
            }
        }

        public int Approve(OverTimes OverTime)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OverTimes OverTimeObj = db.OverTimes.SingleOrDefault(x => x.OverTimeID.Equals(OverTime.OverTimeID));
                    OverTimeObj.ApprovedBy = OverTime.ApprovedBy;
                    OverTimeObj.ApprovedDate = OverTime.ApprovedDate;
                    OverTimeObj.IsApproved = OverTime.IsApproved;
                    OverTimeObj.LastUpdatedBy = OverTime.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public bool IsApproved(int OverTimeID)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.OverTimes.Find(OverTimeID).IsApproved;
                }
            }
            catch
            {
                throw;
            }
        }

    }
}