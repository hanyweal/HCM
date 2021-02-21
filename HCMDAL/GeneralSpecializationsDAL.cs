using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class GeneralSpecializationsDAL
    {
        public List<GeneralSpecializations> GetGeneralSpecializations()
        {
            try
            {
                var db = new HCMEntities();
                return db.GeneralSpecializations.Include("Qualifications").Include("Qualifications.QualificationsDegrees").ToList();
            }
            catch
            {
                throw;
            }
        }
        public GeneralSpecializations GetByGeneralSpecializationID(int GeneralSpecializationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.GeneralSpecializations.Include("Qualifications").FirstOrDefault(c => c.GeneralSpecializationID == GeneralSpecializationID);
            }
            catch
            {
                throw;
            }
        }

        public List<GeneralSpecializations> GetByQualificationID(int QualificationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.GeneralSpecializations.Include("Qualifications").Where(c => c.QualificationID == QualificationID).ToList();
            }
            catch
            {
                throw;
            }
        }
        public int Insert(GeneralSpecializations GeneralSpecialization)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.GeneralSpecializations.Add(GeneralSpecialization);
                    db.SaveChanges();
                    return GeneralSpecialization.GeneralSpecializationID;
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
        public int Update(GeneralSpecializations GeneralSpecialization)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    GeneralSpecializations GeneralSpecializationObj = db.GeneralSpecializations.SingleOrDefault(x => x.GeneralSpecializationID.Equals(GeneralSpecialization.GeneralSpecializationID));
                    GeneralSpecializationObj.GeneralSpecializationName = GeneralSpecialization.GeneralSpecializationName;
                    GeneralSpecializationObj.QualificationID = GeneralSpecialization.QualificationID;
                    GeneralSpecializationObj.DirectPoints = GeneralSpecialization.DirectPoints;
                    GeneralSpecializationObj.IndirectPoints = GeneralSpecialization.IndirectPoints;
                    GeneralSpecializationObj.LastUpdatedDate = GeneralSpecialization.LastUpdatedDate;
                    GeneralSpecializationObj.LastUpdatedBy = GeneralSpecialization.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public int Delete(int GeneralSpecializationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    GeneralSpecializations GeneralSpecializationObj = db.GeneralSpecializations.SingleOrDefault(x => x.GeneralSpecializationID.Equals(GeneralSpecializationID));
                    db.GeneralSpecializations.Remove(GeneralSpecializationObj);
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
