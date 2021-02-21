using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class GovernmentFundsTypesDAL
    {
        public int Insert(GovernmentFundsTypes GovernmentFundsType)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.GovernmentFundsTypes.Add(GovernmentFundsType);
                    db.SaveChanges();
                    return GovernmentFundsType.GovernmentFundTypeID;
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

        public int Update(GovernmentFundsTypes GovernmentFundsType)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    GovernmentFundsTypes GovernmentFundsTypeObj = db.GovernmentFundsTypes.SingleOrDefault(x => x.GovernmentFundTypeID.Equals(GovernmentFundsType.GovernmentFundTypeID));
                    GovernmentFundsTypeObj.GovernmentFundTypeName = GovernmentFundsType.GovernmentFundTypeName;
                    GovernmentFundsTypeObj.LastUpdatedDate = DateTime.Now;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(GovernmentFundsTypes GovernmentFundsType)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    GovernmentFundsTypes GovernmentFundsTypeObj = db.GovernmentFundsTypes.SingleOrDefault(x => x.GovernmentFundTypeID.Equals(GovernmentFundsType.GovernmentFundTypeID));
                    db.GovernmentFundsTypes.Remove(GovernmentFundsTypeObj);
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<GovernmentFundsTypes> GetGovernmentFundsTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.GovernmentFundsTypes.ToList();
            }
            catch
            {
                throw;
            }
        }

        public GovernmentFundsTypes GetByGovernmentFundTypeID(int GovernmentFundTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.GovernmentFundsTypes.SingleOrDefault(x => x.GovernmentFundTypeID.Equals(GovernmentFundTypeID));
            }
            catch
            {
                throw;
            }
        }
    }
}
