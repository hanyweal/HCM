using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class CountriesDAL
    {
        public int Insert(Countries Country)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Countries.Add(Country);
                    db.SaveChanges();
                    return Country.CountryID;
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

        public int Update(Countries Country)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Countries CountryObj = db.Countries.SingleOrDefault(x => x.CountryID.Equals(Country.CountryID));
                    CountryObj.CountryName = Country.CountryName;
                    CountryObj.LastUpdatedDate = DateTime.Now;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int CountryID)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Countries CountryObj = db.Countries.SingleOrDefault(x => x.CountryID.Equals(CountryID));
                    db.Countries.Remove(CountryObj);
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Countries> GetCountries()
        {
            try
            {
                var db = new HCMEntities();
                return db.Countries.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
