using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsJobsCategoriesDAL : CommonEntityDAL
    {
        public int Insert(PromotionsJobsCategories PromotionJobCategory)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PromotionsJobsCategories.Add(PromotionJobCategory);
                    db.SaveChanges();
                    return PromotionJobCategory.PromotionJobCategoryID;
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

        public int Delete(int PromotionJobCategoryID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsJobsCategories PromotionJobCategory = db.PromotionsJobsCategories.SingleOrDefault(x => x.PromotionJobCategoryID.Equals(PromotionJobCategoryID));
                    db.PromotionsJobsCategories.Remove(PromotionJobCategory);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsJobsCategories> GetByJobCategoryIDPromotionPeriodID(int PromotionPeriodID, int JobCategoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsJobsCategories.Include("AssignedJobsCategories")
                                                  .Where(x => x.JobCategoryID == JobCategoryID & x.PromotionPeriodID == PromotionPeriodID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsJobsCategories> GetAssignedJobCategoriesByJobCategoryID(int JobCategoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsJobsCategories.Include("JobsCategories")
                                                    .Include("AssignedJobsCategories")
                                                    .Include("PromotionsPeriods")
                                                    .Where(x => x.JobCategoryID == JobCategoryID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsJobsCategories> GetMainJobCategoriesByAssignedJobCategoryID(int AssignedJobCategoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsJobsCategories.Include("JobsCategories")
                                                  .Include("AssignedJobsCategories")
                                                  .Include("PromotionsPeriods")
                                                  .Where(x => x.AssignedJobCategoryID == AssignedJobCategoryID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public PromotionsJobsCategories GetPromotionJobCategoryByPromotionJobCategoryID(int PromotionJobCategoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsJobsCategories.Include("JobsCategories")
                                                    .Include("AssignedJobsCategories")
                                                    .First(x => x.PromotionJobCategoryID == PromotionJobCategoryID);
            }
            catch
            {
                throw;
            }
        }
    }
}