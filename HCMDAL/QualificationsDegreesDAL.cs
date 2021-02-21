using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class QualificationsDegreesDAL
    {
        public int Insert(QualificationsDegrees QualificationsDegree)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.QualificationsDegrees.Add(QualificationsDegree);
                    db.SaveChanges();
                    return QualificationsDegree.QualificationDegreeID;
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

        public List<QualificationsDegrees> GetQualificationsDegrees()
        {
            try
            {
                var db = new HCMEntities();
                return db.QualificationsDegrees.ToList();
            }
            catch
            {
                throw;
            }
        }

        public int Update(QualificationsDegrees QualificationsDegree)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    QualificationsDegrees QualificationsDegreeObj = db.QualificationsDegrees.FirstOrDefault(x => x.QualificationDegreeID.Equals(QualificationsDegree.QualificationDegreeID));
                    QualificationsDegreeObj.QualificationDegreeName = QualificationsDegree.QualificationDegreeName;
                    //QualificationsDegreeObj.Weight = QualificationsDegree.Weight;
                    //QualificationsDegreeObj.DirectPoints = QualificationsDegree.DirectPoints;
                    //QualificationsDegreeObj.IndirectPoints = QualificationsDegree.IndirectPoints;
                    QualificationsDegreeObj.LastUpdatedDate = QualificationsDegree.LastUpdatedDate;
                    QualificationsDegreeObj.LastUpdatedBy = QualificationsDegree.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int QualificationDegreeID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    QualificationsDegrees QualificationsDegreeObj = db.QualificationsDegrees.FirstOrDefault(x => x.QualificationDegreeID.Equals(QualificationDegreeID));
                    db.QualificationsDegrees.Remove(QualificationsDegreeObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public QualificationsDegrees GetByQualificationDegreeID(int QualificationDegreeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.QualificationsDegrees.FirstOrDefault(c => c.QualificationDegreeID == QualificationDegreeID);
            }
            catch
            {
                throw;
            }
        }
    }
}
