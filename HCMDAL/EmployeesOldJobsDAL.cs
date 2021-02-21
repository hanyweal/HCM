using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
namespace HCMDAL
{
    public class EmployeesOldJobsDAL
    {
        public int Insert(EmployeesOldJobs EmployeesOldJob)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeesOldJobs.Add(EmployeesOldJob);
                    db.SaveChanges();
                    return EmployeesOldJob.EmployeeOldJobID;
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
        public int Update(EmployeesOldJobs EmployeesOldJob)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesOldJobs EmployeesOldJobObj = db.EmployeesOldJobs.SingleOrDefault(x => x.EmployeeOldJobID.Equals(EmployeesOldJob.EmployeeOldJobID));
                    EmployeesOldJobObj.EmployeeCodeID = EmployeesOldJob.EmployeeCodeID;
                    EmployeesOldJobObj.JobName = EmployeesOldJob.JobName;
                    EmployeesOldJobObj.OrganizationName = EmployeesOldJob.OrganizationName;
                    EmployeesOldJobObj.RankName = EmployeesOldJob.RankName;
                    EmployeesOldJobObj.CareerDegreeName = EmployeesOldJob.CareerDegreeName;
                    EmployeesOldJobObj.EmployeeOldJobStartDate = EmployeesOldJob.EmployeeOldJobStartDate;
                    EmployeesOldJobObj.EmployeeOldJobEndDate = EmployeesOldJob.EmployeeOldJobEndDate;
                    EmployeesOldJobObj.LastUpdatedBy = EmployeesOldJob.LastUpdatedBy;
                    EmployeesOldJobObj.LastUpdatedDate = DateTime.Now;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int EmployeeOldJobID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesOldJobs EmployeesOldJobObj = db.EmployeesOldJobs.SingleOrDefault(x => x.EmployeeOldJobID.Equals(EmployeeOldJobID));
                    db.EmployeesOldJobs.Remove(EmployeesOldJobObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }

        }


        public List<EmployeesOldJobs> GetEmployeesOldJobs()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesOldJobs
                 .Include("EmployeesCodes.Employees")
                                  .Include("CreatedByNav.Employees")
                                  .Include("LastUpdatedByNav.Employees")
                 .ToList();

            }
            catch
            {
                throw;
            }
        }

        public EmployeesOldJobs GetByEmployeeOldJobID(int EmployeeOldJobID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesOldJobs
                 .Include("EmployeesCodes.Employees")
                                  .Include("CreatedByNav.Employees")
                                  .Include("LastUpdatedByNav.Employees")
                 .SingleOrDefault(x => x.EmployeeOldJobID.Equals(EmployeeOldJobID));

            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesOldJobs> GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesOldJobs
                 .Include("EmployeesCodes.Employees")
                                  .Include("CreatedByNav.Employees")
                                  .Include("LastUpdatedByNav.Employees")
                 .Where(x => x.EmployeeCodeID.Equals(EmployeeCodeID)).ToList();

            }
            catch
            {
                throw;
            }
        }
    }
}
