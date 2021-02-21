using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class KSACitiesDAL
    {
        public int Insert(KSACities KSACity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.KSACities.Add(KSACity);
                    db.SaveChanges();
                    return KSACity.KSACityID;
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

        public int Update(KSACities KSACity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    KSACities KSACityObj = db.KSACities.SingleOrDefault(x => x.KSACityID.Equals(KSACity.KSACityID));
                    KSACityObj.KSARegionID = KSACity.KSARegionID;
                    KSACityObj.KSACityName = KSACity.KSACityName;
                    KSACityObj.LastUpdatedDate = DateTime.Now;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int KSACityID)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    KSACities KSACityObj = db.KSACities.SingleOrDefault(x => x.KSACityID.Equals(KSACityID));
                    db.KSACities.Remove(KSACityObj);
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<KSACities> GetKSACities()
        {
            try
            {
                var db = new HCMEntities();
                return db.KSACities.Include("KSARegions").ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<KSACities> GetKSACitiesByRegion(int KSARegionID)
        {
            try
            {
                var db = new HCMEntities();
                return db.KSACities.Where(c => c.KSARegionID == KSARegionID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
