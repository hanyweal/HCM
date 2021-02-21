using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsPeriodsDAL
    {
        public PromotionsPeriods GetByYearIDPeriodID(int YearID, int PeriodID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsPeriods.Include("Periods").FirstOrDefault(x => x.PeriodID == PeriodID & x.YearID == YearID);
            }
            catch
            {
                throw;
            }
        }

        public PromotionsPeriods GetByPromotionPeriodID(int PromotionPeriodID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsPeriods.Include("Periods").FirstOrDefault(x => x.PromotionPeriodID == PromotionPeriodID);
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsPeriods> GetPromotionsPeriods()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsPeriods
                                .Include("Periods")
                                .Include("MaturityYearsBalances")
                                .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsPeriods> GetByYearID(int YearID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsPeriods.Include("Periods").Where(x => x.YearID == YearID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public int Insert(PromotionsPeriods PromotionsPeriod)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PromotionsPeriods.Add(PromotionsPeriod);
                    db.SaveChanges();
                    return PromotionsPeriod.PromotionPeriodID;
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

        public int Update(PromotionsPeriods PromotionsPeriod)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsPeriods PromotionsPeriodObj = db.PromotionsPeriods.SingleOrDefault(x => x.PromotionPeriodID.Equals(PromotionsPeriod.PromotionPeriodID));
                    PromotionsPeriodObj.PeriodID = PromotionsPeriod.PeriodID;
                    PromotionsPeriodObj.YearID = PromotionsPeriod.YearID;
                    PromotionsPeriodObj.PromotionStartDate = PromotionsPeriod.PromotionStartDate;
                    PromotionsPeriodObj.PromotionEndDate = PromotionsPeriod.PromotionEndDate;
                    PromotionsPeriodObj.IsActive = PromotionsPeriod.IsActive;
                    PromotionsPeriodObj.LastUpdatedDate = PromotionsPeriod.LastUpdatedDate;
                    PromotionsPeriodObj.LastUpdatedBy = PromotionsPeriod.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int PromotionPeriodID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsPeriods PromotionsPeriodObj = db.PromotionsPeriods.SingleOrDefault(x => x.PromotionPeriodID.Equals(PromotionPeriodID));
                    db.PromotionsPeriods.Remove(PromotionsPeriodObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}