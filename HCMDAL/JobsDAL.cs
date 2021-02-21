using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class JobsDAL
    {
        public int Insert(Jobs Job)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Jobs.Add(Job);
                    db.SaveChanges();
                    return Job.JobID;
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

        public int Update(Jobs Job)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Jobs JobObj = db.Jobs.SingleOrDefault(x => x.JobID.Equals(Job.JobID));
                    JobObj.JobCategoryID = Job.JobCategoryID;
                    JobObj.JobCode = Job.JobCode;
                    JobObj.JobName = Job.JobName;
                    JobObj.LastUpdatedDate = DateTime.Now;
                    JobObj.LastUpdatedBy = Job.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int JobID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Jobs JobObj = db.Jobs.SingleOrDefault(x => x.JobID.Equals(JobID));
                    db.Jobs.Remove(JobObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Jobs> GetJobs()
        {
            try
            {
                HCMEntities db = new HCMEntities();
                return db.Jobs.Include("JobsCategories").ToList();

            }
            catch
            {
                throw;
            }
        }

        public Jobs GetByJobID(int JobID)
        {
            try
            {
                HCMEntities db = new HCMEntities();
                return db.Jobs.Include("JobsCategories").FirstOrDefault(c => c.JobID == JobID);
            }
            catch
            {
                throw;
            }
        }

        public List<Jobs> GetByJobCategoryID(int JobCategoryID)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.Jobs.Include("JobsCategories").Where(x => x.JobCategoryID == JobCategoryID).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
