using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class JobsGeneralGroupsDAL : CommonEntityDAL
    {
        public int Insert(JobsGeneralGroups JobGeneralGroup)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.JobsGeneralGroups.Add(JobGeneralGroup);
                    db.SaveChanges();
                    return JobGeneralGroup.JobGeneralGroupID;
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

        public int Update(JobsGeneralGroups JobGeneralGroup)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    JobsGeneralGroups JobGeneralGroupObj = db.JobsGeneralGroups.SingleOrDefault(x => x.JobGeneralGroupID.Equals(JobGeneralGroup.JobGeneralGroupID));
                    JobGeneralGroupObj.JobGeneralGroupID = JobGeneralGroup.JobGeneralGroupID;
                    JobGeneralGroupObj.JobGeneralGroupName = JobGeneralGroup.JobGeneralGroupName;
                    JobGeneralGroupObj.LastUpdatedDate = DateTime.Now;
                    JobGeneralGroupObj.LastUpdatedBy = JobGeneralGroup.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int JobGeneralGroupID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    JobsGeneralGroups JobGeneralGroup = db.JobsGeneralGroups.SingleOrDefault(x => x.JobGeneralGroupID.Equals(JobGeneralGroupID));
                    db.JobsGeneralGroups.Remove(JobGeneralGroup);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<JobsGeneralGroups> GetJobsGeneralGroups()
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsGeneralGroups.ToList();
            }
            catch
            {
                throw;
            }
        }

        public JobsGeneralGroups GetByJobGeneralGroupID(int JobGeneralGroupID)
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsGeneralGroups.SingleOrDefault(c => c.JobGeneralGroupID == JobGeneralGroupID);
            }
            catch
            {
                throw;
            }
        }
    }
}
