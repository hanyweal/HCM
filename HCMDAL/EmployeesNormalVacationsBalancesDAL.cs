using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class EmployeesNormalVacationsBalancesDAL
    {
        public bool InsertUpdate(EmployeesNormalVacationsBalances arg)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesNormalVacationsBalances Obj = db.EmployeesNormalVacationsBalances.FirstOrDefault(x => x.EmployeeCodeID == arg.EmployeeCodeID);
                    if (Obj != null && Obj.EmployeeNormalVacationBalanceID > 0)
                        Update(arg);
                    else
                        Insert(arg);

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

        public int Insert(EmployeesNormalVacationsBalances arg)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    arg.LastUpdatedDate = DateTime.Now;
                    db.EmployeesNormalVacationsBalances.Add(arg);
                    db.SaveChanges();
                    return arg.EmployeeNormalVacationBalanceID;
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

        public int Update(EmployeesNormalVacationsBalances arg)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesNormalVacationsBalances Obj = db.EmployeesNormalVacationsBalances.FirstOrDefault(x => x.EmployeeCodeID == arg.EmployeeCodeID);
                    Obj.ConsumedCurrentBalance = arg.ConsumedCurrentBalance;
                    Obj.ConsumedOldBalance = arg.ConsumedOldBalance;
                    Obj.DeservedCurrentBalance = arg.DeservedCurrentBalance;
                    Obj.DeservedOldBalance = arg.DeservedOldBalance;
                    Obj.ExpiredCurrentBalance = arg.ExpiredCurrentBalance;
                    Obj.InAdvanceBalance = arg.InAdvanceBalance;
                    Obj.InAdvConsumed = arg.InAdvConsumed;
                    Obj.RemainingCurrentBalance = arg.RemainingCurrentBalance;
                    Obj.RemainingOldBalance = arg.RemainingOldBalance;
                    Obj.TotalAvailableVacationBalance = arg.TotalAvailableVacationBalance;
                    Obj.TotalConsumedBalance = arg.TotalConsumedBalance;
                    Obj.TotalDeservedBalance = arg.TotalDeservedBalance;
                    Obj.TotalRemainingBalance = arg.TotalRemainingBalance;
                    Obj.LastUpdatedDate = DateTime.Now;
                    
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesNormalVacationsBalances> GetNormalVacationsBalances()
        {
            try
            {
                HCMEntities db = new HCMEntities();
                return db.EmployeesNormalVacationsBalances.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
