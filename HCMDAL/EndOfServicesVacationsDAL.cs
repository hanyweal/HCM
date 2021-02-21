using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class EndOfServicesVacationsDAL : CommonEntityDAL
    {
        public int Insert(EndOfServicesVacations EndOfServiceVacation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EndOfServicesVacations.Add(EndOfServiceVacation);
                    db.SaveChanges();
                    return EndOfServiceVacation.EndOfServiceVacationID;
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

        public int Update(EndOfServicesVacations EndOfServiceVacation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EndOfServicesVacations EndOfServiceVacationObj = db.EndOfServicesVacations.SingleOrDefault(x => x.EndOfServiceVacationID.Equals(EndOfServiceVacation.EndOfServiceVacationID));
                    EndOfServiceVacationObj.EndOfServiceVacationID = EndOfServiceVacation.EndOfServiceVacationID;
                    EndOfServiceVacationObj.EndOfServiceID = EndOfServiceVacation.EndOfServiceID;
                    EndOfServiceVacationObj.VacationTypeID = EndOfServiceVacation.VacationTypeID;
                    EndOfServiceVacationObj.LastUpdatedDate = DateTime.Now;
                    EndOfServiceVacationObj.LastUpdatedBy = EndOfServiceVacation.LastUpdatedBy;
                    EndOfServiceVacationObj.VacationStartDate = EndOfServiceVacation.VacationStartDate;
                    EndOfServiceVacationObj.VacationEndDate = EndOfServiceVacation.VacationEndDate;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int EndOfServiceVacationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EndOfServicesVacations EndOfServiceVacation = db.EndOfServicesVacations.SingleOrDefault(x => x.EndOfServiceVacationID.Equals(EndOfServiceVacationID));
                    db.EndOfServicesVacations.Remove(EndOfServiceVacation);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }
        public List<EndOfServicesVacations> GetEndOfServicesVacations()
        {
            try
            {
                var db = new HCMEntities();
                return db.EndOfServicesVacations.ToList();
            }
            catch
            {
                throw;
            }
        }

        public EndOfServicesVacations GetByEndOfServiceVacationID(int EndOfServiceVacationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EndOfServicesVacations.SingleOrDefault(c => c.EndOfServiceVacationID == EndOfServiceVacationID);
            }
            catch
            {
                throw;
            }
        }

        public List<EndOfServicesVacations> GetByEndOfServiceID(int EndOfServiceID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EndOfServicesVacations.Include("EndOfServices").Where(x => x.EndOfServiceID == EndOfServiceID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
