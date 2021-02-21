using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class HolidaysAttendanceDetailsDAL
    {
        public int Delete(int HolidayAttendanceDetailID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    HolidaysAttendanceDetails HolidayAttendanceDetailObj = db.HolidaysAttendanceDetails.SingleOrDefault(x => x.HolidayAttendanceDetailID.Equals(HolidayAttendanceDetailID));
                    db.HolidaysAttendanceDetails.Remove(HolidayAttendanceDetailObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int Insert(HolidaysAttendanceDetails HolidayAttendanceDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.HolidaysAttendanceDetails.Add(HolidayAttendanceDetail);
                    db.SaveChanges();
                    return HolidayAttendanceDetail.HolidayAttendanceDetailID;
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

        public List<HolidaysAttendanceDetails> GetHolidaysAttendanceDetailsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.HolidaysAttendanceDetails.Include("HolidaysAttendance")
                                           .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                           .Include("HolidaysAttendance.HolidaysAttendanceDays")
                                           .Where(x => x.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeID == EmployeeCodeID).ToList();

            }
            catch
            {
                throw;
            }
        }

        public List<HolidaysAttendanceDetails> GetHolidaysAttendanceDetailsByHolidayAttendanceID(int HolidayAttendanceID)
        {
            try
            {
                var db = new HCMEntities();
                return db.HolidaysAttendanceDetails.Include("EmployeesCareersHistory.EmployeesCodes")
                                           .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                           .Where(x => x.HolidayAttendanceID == HolidayAttendanceID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public HolidaysAttendanceDetails GetByHolidayAttendanceDetailID(int HolidayAttendanceDetailID)
        {
            try
            {
                var db = new HCMEntities();

                var query = db.HolidaysAttendanceDetails
                                              //  .Include("CreatedByNav.Employees")
                                              //.Include("LastUpdatedByNav.Employees")
                                              .Include("HolidaysAttendance");
                //.Include("DelegationsDetails.Delegations")
                //.Include("DelegationsDetails.EmployeesCareersHistory.EmployeesCodes.Employees");

                return query.SingleOrDefault(x => x.HolidayAttendanceDetailID.Equals(HolidayAttendanceDetailID));
            }
            catch
            {
                throw;
            }
        }
    }
}
