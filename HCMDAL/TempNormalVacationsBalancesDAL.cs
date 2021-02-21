using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class TempNormalVacationsBalancesDAL
    {
        //public bool Insert(List<TempNormalVacationsBalances> TempNormalVacationBalanceList)
        public bool Insert(TempNormalVacationsBalances TempNormalVacationBalance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.TempNormalVacationsBalances.Add(TempNormalVacationBalance);
                    db.SaveChanges();
                    return true;
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

        public List<TempNormalVacationsBalances> GetNormalVacationsBalances()
        {
            try
            {
                HCMEntities db = new HCMEntities();
                return db.TempNormalVacationsBalances.ToList();

            }
            catch
            {
                throw;
            }
        }
    }
}
