using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class JobsCategoriesQualificationsDAL : CommonEntityDAL
    {
        public int Insert(JobsCategoriesQualifications JobCategoryQualification)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.JobsCategoriesQualifications.Add(JobCategoryQualification);
                    db.SaveChanges();
                    return JobCategoryQualification.JobCategoryQualificationID;
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

        public int Update(JobsCategoriesQualifications JobCategoryQualification)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    JobsCategoriesQualifications JobCategoryQualificationObj = db.JobsCategoriesQualifications.SingleOrDefault(x => x.JobCategoryQualificationID.Equals(JobCategoryQualification.JobCategoryQualificationID));
                    JobCategoryQualificationObj.JobCategoryQualificationID = JobCategoryQualification.JobCategoryQualificationID;
                    JobCategoryQualificationObj.JobCategoryID = JobCategoryQualification.JobCategoryID;
                    JobCategoryQualificationObj.GeneralSpecializationID = JobCategoryQualification.GeneralSpecializationID;
                    JobCategoryQualificationObj.QualificationDegreeID = JobCategoryQualification.QualificationDegreeID;
                    JobCategoryQualificationObj.QualificationID = JobCategoryQualification.QualificationID;
                    JobCategoryQualificationObj.IsMinQualification = JobCategoryQualification.IsMinQualification;
                    JobCategoryQualificationObj.LastUpdatedDate = DateTime.Now;
                    JobCategoryQualificationObj.LastUpdatedBy = JobCategoryQualification.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int JobCategoryQualificationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    JobsCategoriesQualifications JobCategoryQualification = db.JobsCategoriesQualifications.SingleOrDefault(x => x.JobCategoryQualificationID.Equals(JobCategoryQualificationID));
                    db.JobsCategoriesQualifications.Remove(JobCategoryQualification);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }
        public List<JobsCategoriesQualifications> GetJobsCategoriesQualifications()
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsCategoriesQualifications.ToList();
            }
            catch
            {
                throw;
            }
        }

        public JobsCategoriesQualifications GetByJobCategoryQualificationID(int JobCategoryQualificationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsCategoriesQualifications.SingleOrDefault(c => c.JobCategoryQualificationID == JobCategoryQualificationID);
            }
            catch
            {
                throw;
            }
        }

        public List<JobsCategoriesQualifications> GetByJobCategoryID(int JobCategoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsCategoriesQualifications.Include("JobsCategories").Where(x => x.JobCategoryID == JobCategoryID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public JobsCategoriesQualifications GetByQualificationID(int QualificationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsCategoriesQualifications
                    .Include("Qualifications")
                    .FirstOrDefault(x => x.QualificationID == QualificationID);
            }
            catch
            {
                throw;
            }
        }
        public JobsCategoriesQualifications GetByGeneralSpecializationID(int GeneralSpecializationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsCategoriesQualifications
                    .Include("Qualifications")
                    .FirstOrDefault(x => x.GeneralSpecializationID == GeneralSpecializationID);
            }
            catch
            {
                throw;
            }
        }
    }
}
