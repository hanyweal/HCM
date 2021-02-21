using System.Linq;

namespace HCMDAL
{
    public class EmployeesVacationsOpeningBalancesDAL
    {
        public int Insert(EmployeesVacationsOpeningBalances EmployeeVacationOpeningBalance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeesVacationsOpeningBalances.Add(EmployeeVacationOpeningBalance);
                    db.SaveChanges();
                    return EmployeeVacationOpeningBalance.EmployeeVacationOpeningBalanceID;
                }
            }
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
            //    }
            //    throw;
            //}
            catch
            {
                throw;
            }
        }

        public int Update(EmployeesVacationsOpeningBalances EmployeeVacationOpeningBalance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesVacationsOpeningBalances EmployeesVacationsOpeningBalancesObj = db.EmployeesVacationsOpeningBalances.SingleOrDefault(x => x.EmployeeVacationOpeningBalanceID.Equals(EmployeeVacationOpeningBalance.EmployeeVacationOpeningBalanceID));
                    EmployeesVacationsOpeningBalancesObj.OpeningBalance = EmployeeVacationOpeningBalance.OpeningBalance;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public EmployeesVacationsOpeningBalances GetByEmployeeVacationOpeningBalanceID(int EmployeeVacationOpeningBalanceID)
        {
            try
            {
                var db = new HCMEntities();

                var query = db.EmployeesVacationsOpeningBalances;

                return query.Include("EmployeesCodes").Include("VacationsTypes").SingleOrDefault(x => x.EmployeeVacationOpeningBalanceID.Equals(EmployeeVacationOpeningBalanceID));
            }
            catch
            {
                throw;
            }
        }

        public EmployeesVacationsOpeningBalances GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();

                var query = db.EmployeesVacationsOpeningBalances;

                return query.Include("EmployeesCodes").Include("VacationsTypes").SingleOrDefault(x => x.EmployeeCodeID.Equals(EmployeeCodeID));
            }
            catch
            {
                throw;
            }
        }


    }
}
