using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class QualificationsDAL
    {
        public List<Qualifications> GetQualifications()
        {
            try
            {
                var db = new HCMEntities();
                return db.Qualifications.Include("QualificationsDegrees").ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<Qualifications> GetByQualificationDegreeID(int QualificationDegreeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Qualifications.Include("QualificationsDegrees").Where(c => c.QualificationDegreeID == QualificationDegreeID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public Qualifications GetByQualificationID(int QualificationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Qualifications.Include("QualificationsDegrees").FirstOrDefault(c => c.QualificationID == QualificationID);
            }
            catch
            {
                throw;
            }
        }

        public int Insert(Qualifications Qualification)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Qualifications.Add(Qualification);
                    db.SaveChanges();
                    return Qualification.QualificationID;
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
        public int Update(Qualifications Qualification)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Qualifications QualificationObj = db.Qualifications.SingleOrDefault(x => x.QualificationID.Equals(Qualification.QualificationID));
                    QualificationObj.QualificationName = Qualification.QualificationName;
                    QualificationObj.QualificationDegreeID = Qualification.QualificationDegreeID;
                    QualificationObj.DirectPoints = Qualification.DirectPoints;
                    QualificationObj.IndirectPoints = Qualification.IndirectPoints;
                    QualificationObj.LastUpdatedDate = Qualification.LastUpdatedDate;
                    QualificationObj.LastUpdatedBy = Qualification.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public int Delete(int QualificationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Qualifications QualificationObj = db.Qualifications.SingleOrDefault(x => x.QualificationID.Equals(QualificationID));
                    db.Qualifications.Remove(QualificationObj);
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
