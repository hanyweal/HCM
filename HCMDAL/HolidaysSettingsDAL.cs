using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class HolidaysSettingsDAL
    {
        public int Insert(HolidaysSettings HolidaySetting)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.HolidaysSettings.Add(HolidaySetting);
                    db.SaveChanges();
                    return HolidaySetting.HolidaySettingID;
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

        public int Update(HolidaysSettings HolidaySetting)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    HolidaysSettings HolidaySettingObj = db.HolidaysSettings.SingleOrDefault(x => x.HolidaySettingID.Equals(HolidaySetting.HolidaySettingID));
                    HolidaySettingObj.HolidaySettingEndDate = HolidaySetting.HolidaySettingEndDate;
                    HolidaySettingObj.HolidaySettingStartDate = HolidaySetting.HolidaySettingStartDate;
                    HolidaySettingObj.MaturityYearID = HolidaySetting.MaturityYearID;
                    HolidaySettingObj.HolidayTypeID = HolidaySetting.HolidayTypeID;
                    HolidaySettingObj.LastUpdatedDate = DateTime.Now;
                    HolidaySettingObj.LastUpdatedBy = HolidaySetting.LastUpdatedBy;
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

        public int Delete(int HolidaySettingID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    HolidaysSettings HolidaySettingObj = db.HolidaysSettings.SingleOrDefault(x => x.HolidaySettingID.Equals(HolidaySettingID));
                    db.HolidaysSettings.Remove(HolidaySettingObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<HolidaysSettings> GetHolidaysSettings()
        {
            try
            {
                var db = new HCMEntities();
                return db.HolidaysSettings.Include("HolidaysTypes").ToList();
            }
            catch
            {
                throw;
            }
        }


        public List<HolidaysSettings> GetByMaturityYearID(int MaturityYearID)
        {
            try
            {
                var db = new HCMEntities();
                return db.HolidaysSettings.Include("CreatedByNav.Employees")
                                   .Include("LastUpdatedByNav.Employees")
                                   .Where(c => c.MaturityYearID == MaturityYearID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public HolidaysSettings GetByHolidaySettingID(int HolidaySettingID)
        {
            try
            {
                var db = new HCMEntities();
                return db.HolidaysSettings.Include("CreatedByNav.Employees")
                                   .Include("LastUpdatedByNav.Employees")
                                   .Include("MaturityYearsBalances")
                                   .SingleOrDefault(x => x.HolidaySettingID.Equals(HolidaySettingID));
            }
            catch
            {
                throw;
            }
        }
    }
}
