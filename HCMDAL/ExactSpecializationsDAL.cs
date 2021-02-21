using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class ExactSpecializationsDAL
    {
        public List<ExactSpecializations> GetExactSpecializations()
        {
            try
            {
                var db = new HCMEntities();
                return db.ExactSpecializations.Include("GeneralSpecializations").ToList();
            }
            catch
            {
                throw;
            }
        }
        public List<ExactSpecializations> GetByGeneralSpecializationID(int GeneralSpecializationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.ExactSpecializations.Include("GeneralSpecializations").Where(c => c.GeneralSpecializationID == GeneralSpecializationID).ToList();
            }
            catch
            {
                throw;
            }
        }
        public int Insert(ExactSpecializations ExactSpecialization)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.ExactSpecializations.Add(ExactSpecialization);
                    db.SaveChanges();
                    return ExactSpecialization.ExactSpecializationID;
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
        public int Update(ExactSpecializations ExactSpecialization)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    ExactSpecializations ExactSpecializationObj = db.ExactSpecializations.SingleOrDefault(x => x.ExactSpecializationID.Equals(ExactSpecialization.ExactSpecializationID));
                    ExactSpecializationObj.ExactSpecializationName = ExactSpecialization.ExactSpecializationName;
                    ExactSpecializationObj.GeneralSpecializationID = ExactSpecialization.GeneralSpecializationID;
                    //QualificationObj.DirectPoints = Qualification.DirectPoints;
                    //QualificationObj.IndirectPoints = Qualification.IndirectPoints;
                    ExactSpecializationObj.LastUpdatedDate = ExactSpecialization.LastUpdatedDate;
                    ExactSpecializationObj.LastUpdatedBy = ExactSpecialization.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public int Delete(int ExactSpecializationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    ExactSpecializations ExactSpecializationObj = db.ExactSpecializations.SingleOrDefault(x => x.ExactSpecializationID.Equals(ExactSpecializationID));
                    db.ExactSpecializations.Remove(ExactSpecializationObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
