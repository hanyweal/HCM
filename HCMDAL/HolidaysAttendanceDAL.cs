using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class HolidaysAttendanceDAL
    {
        public int Insert(HolidaysAttendance HolidayAttendance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.HolidaysAttendance.Add(HolidayAttendance);
                    db.SaveChanges();
                    return HolidayAttendance.HolidayAttendanceID;
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

        public int Update(HolidaysAttendance HolidayAttendance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    HolidaysAttendance HolidayAttendanceObj = db.HolidaysAttendance.SingleOrDefault(x => x.HolidayAttendanceID.Equals(HolidayAttendance.HolidayAttendanceID));
                    HolidayAttendanceObj.HolidayAttendanceID = HolidayAttendance.HolidayAttendanceID;
                    HolidayAttendanceObj.HolidaySettingID = HolidayAttendance.HolidaySettingID;
                    HolidayAttendanceObj.OrganizationID = HolidayAttendance.OrganizationID;
                    HolidayAttendanceObj.LastUpdatedDate = HolidayAttendance.LastUpdatedDate;
                    HolidayAttendanceObj.LastUpdatedBy = HolidayAttendance.LastUpdatedBy;
                    return db.SaveChanges();
                }

            }
            catch
            {
                throw;
            }
        }

        public int Delete(HolidaysAttendance HolidayAttendance, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    HolidaysAttendance HolidayAttendanceObj = db.HolidaysAttendance.SingleOrDefault(x => x.HolidayAttendanceID.Equals(HolidayAttendance.HolidayAttendanceID));
                    HolidayAttendanceObj.HolidaysAttendanceDetails.ToList().ForEach(d => db.HolidaysAttendanceDetails.Remove(d));
                    db.HolidaysAttendance.Remove(HolidayAttendanceObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<HolidaysAttendance> GetHolidaysAttendance()
        {
            try
            {
                var db = new HCMEntities();
                return db.HolidaysAttendance.ToList();
            }
            catch
            {
                throw;
            }
        }

        public HolidaysAttendance GetByHolidayAttendanceID(int HolidayAttendanceID)
        {
            try
            {
                var db = new HCMEntities();
                return db.HolidaysAttendance.Include("CreatedByNav.Employees")
                                   .Include("LastUpdatedByNav.Employees")
                                   .SingleOrDefault(x => x.HolidayAttendanceID.Equals(HolidayAttendanceID));
            }
            catch
            {
                throw;
            }
        }

        public List<HolidaysAttendance> GetByHolidaySettingID(int HolidaySettingID)
        {
            try
            {
                var db = new HCMEntities();
                return db.HolidaysAttendance.Include("CreatedByNav.Employees")
                                   .Include("LastUpdatedByNav.Employees")
                                   .Where(x => x.HolidaySettingID.Equals(HolidaySettingID)).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}