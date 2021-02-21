using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class JobsCategoriesDAL : CommonEntityDAL
    {
        public int Insert(JobsCategories JobCategory)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.JobsCategories.Add(JobCategory);
                    db.SaveChanges();
                    return JobCategory.JobCategoryID;
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

        public int Update(JobsCategories JobCategory)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    JobsCategories JobCategoryObj = db.JobsCategories.SingleOrDefault(x => x.JobCategoryID.Equals(JobCategory.JobCategoryID));
                    JobCategoryObj.JobCategoryID = JobCategory.JobCategoryID;
                    JobCategoryObj.JobCategoryName = JobCategory.JobCategoryName;
                    JobCategoryObj.JobGroupID = JobCategory.JobGroupID;
                    JobCategoryObj.LastUpdatedDate = DateTime.Now;
                    JobCategoryObj.LastUpdatedBy = JobCategory.LastUpdatedBy;
                    JobCategoryObj.MinGeneralSpecializationID = JobCategory.MinGeneralSpecializationID;
                    JobCategoryObj.MinQualificationDegreeID = JobCategory.MinQualificationDegreeID;
                    JobCategoryObj.MinQualificationID = JobCategory.MinQualificationID;
                    JobCategoryObj.JobCategoryNo = JobCategory.JobCategoryNo;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int JobCategoryID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    JobsCategories JobCategory = db.JobsCategories.SingleOrDefault(x => x.JobCategoryID.Equals(JobCategoryID));
                    db.JobsCategories.Remove(JobCategory);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }
        public List<JobsCategories> GetJobsCategories()
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsCategories.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<JobsCategories> GetJobsCategories(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();
                var odData = db.JobsCategories
                                    .Include("JobsGroups").Include("JobsGroups.JobsGeneralGroups")
                                    .Include("Qualifications").Include("QualificationsDegrees")
                                    .Include("GeneralSpecializations");

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<JobsCategories> iq = odData.Where(p => p.JobCategoryID != null);

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                                       p.JobCategoryName.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.JobsGroups.JobGroupName.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.JobsGroups.JobsGeneralGroups.JobGeneralGroupName.ToString().ToLower().Contains(search.ToLower())
                                       );
                }

                // Sorting.
                iq = iq.OrderBy(p => p.JobCategoryName);

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

        public JobsCategories GetByJobCategoryID(int JobCategoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsCategories.SingleOrDefault(c => c.JobCategoryID == JobCategoryID);
            }
            catch
            {
                throw;
            }
        }

        public List<JobsCategories> GetByJobGroupID(int JobGroupID)
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsCategories.Include("JobsGroups").Where(x => x.JobGroupID == JobGroupID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
