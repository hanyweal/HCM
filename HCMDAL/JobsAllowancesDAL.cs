using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class JobsAllowancesDAL
    {
        public int Insert(JobsAllowances JobAllowance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.JobsAllowances.Add(JobAllowance);
                    db.SaveChanges();
                    return JobAllowance.JobAllowanceID;
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

        public int Update(JobsAllowances JobAllowance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    JobsAllowances JobAllowanceObj = db.JobsAllowances.SingleOrDefault(x => x.JobAllowanceID.Equals(JobAllowance.JobAllowanceID));
                    JobAllowanceObj.JobID = JobAllowance.JobID;
                    JobAllowanceObj.AllowanceID = JobAllowance.AllowanceID;
                    JobAllowanceObj.IsActive = JobAllowance.IsActive;
                    JobAllowanceObj.LastUpdatedDate = DateTime.Now;
                    JobAllowanceObj.LastUpdatedBy = JobAllowance.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int JobAllowanceID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    JobsAllowances JobAllowanceObj = db.JobsAllowances.SingleOrDefault(x => x.JobAllowanceID.Equals(JobAllowanceID));
                    db.JobsAllowances.Remove(JobAllowanceObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<JobsAllowances> GetJobsAllowances()
        {
            try
            {
                var db = new HCMEntities();
                var query = db.JobsAllowances.Include("Jobs").Include("Allowances");
                query = query.Include("Allowances.AllowancesAmountTypes");
                return query.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<JobsAllowances> GetJobsAllowancesByJobID(int JobID)
        {
            try
            {
                var db = new HCMEntities();
                var query = db.JobsAllowances.Include("Jobs").Include("Allowances");
                query = query.Include("Allowances.AllowancesAmountTypes");
                return query.Where(c => c.JobID == JobID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<JobsAllowances> GetJobsAllowancesByAllowanceID(int AllowanceID)
        {
            try
            {
                var db = new HCMEntities();
                var query = db.JobsAllowances.Include("Jobs").Include("Allowances");
                query = query.Include("Allowances.AllowancesAmountTypes");
                return query.Where(c => c.AllowanceID == AllowanceID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
