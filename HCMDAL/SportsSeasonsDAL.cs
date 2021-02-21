using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class SportsSeasonsDAL
    {
        public int Insert(SportsSeasons SportSeason)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.SportsSeasons.Add(SportSeason);
                    db.SaveChanges();
                    return SportSeason.SportsSeasonID;
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
            catch
            {
                throw;
            }
        }

        public int Update(SportsSeasons SportSeason)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    SportsSeasons SportSeasonObj = db.SportsSeasons.SingleOrDefault(x => x.SportsSeasonID.Equals(SportSeason.SportsSeasonID));
                    SportSeasonObj.SportsSeasonStartDate = SportSeason.SportsSeasonStartDate;
                    SportSeasonObj.SportsSeasonEndDate = SportSeason.SportsSeasonEndDate;
                    SportSeasonObj.SportsSeasonDescription = SportSeason.SportsSeasonDescription;
                    SportSeasonObj.MaturityYearID = SportSeason.MaturityYearID;
                    SportSeasonObj.UpdatedDate = DateTime.Now;
                    SportSeasonObj.Updatedby = SportSeason.Updatedby;
                    return db.SaveChanges();
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
            catch
            {
                throw;
            }
        }

        public int Delete(int SportsSeasonID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    SportsSeasons SportsSeasonsObj = db.SportsSeasons.SingleOrDefault(x => x.SportsSeasonID.Equals(SportsSeasonID));
                    db.SportsSeasons.Remove(SportsSeasonsObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<SportsSeasons> GetSportsSeasons()
        {
            try
            {
                var db = new HCMEntities();
                return db.SportsSeasons.Include("MaturityYearsBalances").ToList();
            }
            catch
            {
                throw;
            }
        }

        //public List<HolidaysSettings> GetHolidaysSettingsByHolidayTypeID(int HolidayTypeID)
        //{
        //    try
        //    {
        //        var db = new HCMEntities();
        //        return db.HolidaysSettings.Where(c => c.HolidayTypeID == HolidayTypeID).ToList();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public List<SportsSeasons> GetSportsSeasonsBySportsSeasonID(int SportsSeasonID)
        {
            try
            {
                var db = new HCMEntities();
                return db.SportsSeasons.Where(c => c.SportsSeasonID == SportsSeasonID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public SportsSeasons GetBySportsSeasonID(int SportsSeasonID)
        {
            try
            {
                var db = new HCMEntities();
                return db.SportsSeasons
                                   .SingleOrDefault(x => x.SportsSeasonID.Equals(SportsSeasonID));
            }
            catch
            {
                throw;
            }
        }
    }
}

