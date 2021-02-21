using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class AllowancesDAL
    {
        public int Insert(Allowances Allowance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Allowances.Add(Allowance);
                    db.SaveChanges();
                    return Allowance.AllowanceID;
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

        public int Update(Allowances Allowance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Allowances AllowanceObj = db.Allowances.FirstOrDefault(x => x.AllowanceID.Equals(Allowance.AllowanceID));
                    AllowanceObj.AllowanceAmountTypeID = Allowance.AllowanceAmountTypeID;
                    AllowanceObj.AllowanceCalculationTypeID = Allowance.AllowanceCalculationTypeID;
                    AllowanceObj.AllowanceName = Allowance.AllowanceName;
                    AllowanceObj.AllowanceAmount = Allowance.AllowanceAmount;
                    AllowanceObj.IsActive = Allowance.IsActive;
                    AllowanceObj.LastUpdatedDate = DateTime.Now;
                    AllowanceObj.LastUpdatedBy = Allowance.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int AllowanceID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Allowances AllowanceObj = db.Allowances.FirstOrDefault(x => x.AllowanceID.Equals(AllowanceID));
                    db.Allowances.Remove(AllowanceObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Allowances> GetAllowances()
        {
            try
            {
                var db = new HCMEntities();
                return db.Allowances.Include("AllowancesAmountTypes").Include("AllowancesCalculationTypes").ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<Allowances> GetAllowancesByAllowanceAmountTypeID(int AllowanceAmountTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Allowances.Where(c => c.AllowanceAmountTypeID == AllowanceAmountTypeID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
