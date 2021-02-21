using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class JobsGroupsDAL : CommonEntityDAL
    {
        public int Insert(JobsGroups JobGroup)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.JobsGroups.Add(JobGroup);
                    db.SaveChanges();
                    return JobGroup.JobGroupID;
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

        public int Update(JobsGroups JobGroup)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    JobsGroups JobGroupObj = db.JobsGroups.SingleOrDefault(x => x.JobGroupID.Equals(JobGroup.JobGroupID));
                    JobGroupObj.JobGroupID = JobGroup.JobGroupID;
                    JobGroupObj.JobGroupName = JobGroup.JobGroupName;
                    JobGroupObj.JobGeneralGroupID = JobGroup.JobGeneralGroupID;
                    JobGroupObj.LastUpdatedDate = DateTime.Now;
                    JobGroupObj.LastUpdatedBy = JobGroup.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int JobGroupID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    JobsGroups JobGroup = db.JobsGroups.SingleOrDefault(x => x.JobGroupID.Equals(JobGroupID));
                    db.JobsGroups.Remove(JobGroup);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<JobsGroups> GetJobsGroups()
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsGroups.ToList();
            }
            catch
            {
                throw;
            }
        }

        public JobsGroups GetByJobCategoryID(int JobGroupID)
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsGroups.SingleOrDefault(c => c.JobGroupID == JobGroupID);
            }
            catch
            {
                throw;
            }
        }

        public List<JobsGroups> GetByJobGeneralGroupID(int JobGeneralGroupID)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.JobsGroups.Include("JobsGeneralGroups").Where(x => x.JobGeneralGroupID == JobGeneralGroupID).ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
