using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class OverTimesDaysDAL
    {
        public int Delete(int OverTimeDayID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OverTimesDays OverTimeDayObj = db.OverTimesDays.SingleOrDefault(x => x.OverTimeDayID.Equals(OverTimeDayID));
                    db.OverTimesDays.Remove(OverTimeDayObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int Insert(OverTimesDays OverTimeDay)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.OverTimesDays.Add(OverTimeDay);
                    db.SaveChanges();
                    return OverTimeDay.OverTimeDayID;
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

    }
}
