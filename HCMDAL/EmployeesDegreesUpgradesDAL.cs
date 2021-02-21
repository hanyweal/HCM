using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class EmployeesDegreesUpgradesDAL
    {
        public int Insert(EmployeesDegreesUpgrades EmployeeDegreeUpgrade)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeesDegreesUpgrades.Add(EmployeeDegreeUpgrade);
                    db.SaveChanges();
                    return EmployeeDegreeUpgrade.EmployeeDegreeUpgradeID;
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

        //public int Delete(int EmployeeDegreeUpgradeID, int UserIdentity)
        //{
        //    try
        //    {
        //        using (var db = new HCMEntities())
        //        {
        //            EmployeesDegreesUpgrades EmployeeDegreeUpgradeObj = db.EmployeesDegreesUpgrades.SingleOrDefault(x => x.EmployeeDegreeUpgradeID.Equals(EmployeeDegreeUpgradeID));
        //            db.EmployeesDegreesUpgrades.Remove(EmployeeDegreeUpgradeObj);
        //            return db.SaveChanges(UserIdentity);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public List<EmployeesDegreesUpgrades> GetEmployeesDegreesUpgrades()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesDegreesUpgrades.Include("EmployeesCodes").ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}
