using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class EmployeesQualificationsDAL
    {

        public int Insert(EmployeesQualifications EmployeeQualification)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeesQualifications.Add(EmployeeQualification);
                    db.SaveChanges();
                    return EmployeeQualification.EmployeeQualificationID;
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

        public int Update(EmployeesQualifications EmployeeQualification)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesQualifications EmployeeQualificationObj = db.EmployeesQualifications.SingleOrDefault(x => x.EmployeeQualificationID.Equals(EmployeeQualification.EmployeeQualificationID));
                    EmployeeQualificationObj.QualificationDegreeID = EmployeeQualification.QualificationDegreeID;
                    EmployeeQualificationObj.QualificationID = EmployeeQualification.QualificationID;
                    EmployeeQualificationObj.GeneralSpecializationID = EmployeeQualification.GeneralSpecializationID;
                    EmployeeQualificationObj.ExactSpecializationID = EmployeeQualification.ExactSpecializationID;
                    EmployeeQualificationObj.UniSchName = EmployeeQualification.UniSchName;
                    EmployeeQualificationObj.Department = EmployeeQualification.Department;
                    EmployeeQualificationObj.FullGPA = EmployeeQualification.FullGPA;
                    EmployeeQualificationObj.GPA = EmployeeQualification.GPA;
                    EmployeeQualificationObj.StudyPlace = EmployeeQualification.StudyPlace;
                    EmployeeQualificationObj.GraduationDate = EmployeeQualification.GraduationDate;
                    EmployeeQualificationObj.GraduationYear = EmployeeQualification.GraduationYear;
                    //EmployeeQualificationObj.Percentage = EmployeeQualification.Percentage;
                    EmployeeQualificationObj.QualificationTypeID = EmployeeQualification.QualificationTypeID;
                    EmployeeQualificationObj.LastUpdatedDate = EmployeeQualification.LastUpdatedDate;
                    EmployeeQualificationObj.LastUpdatedBy = EmployeeQualification.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int EmployeeQualificationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesQualifications EmployeeQualificationObj = db.EmployeesQualifications.FirstOrDefault(x => x.EmployeeQualificationID.Equals(EmployeeQualificationID));
                    db.EmployeesQualifications.Remove(EmployeeQualificationObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesQualifications> GetEmployeeQualificationByEmployeeCodeID(int? EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesQualifications
                        .Include("Qualifications")
                        .Include("QualificationsDegrees")
                        .Where(eq => eq.EmployeeCodeID == EmployeeCodeID)
                        .OrderByDescending(eq => eq.QualificationsDegrees.Weight)
                        .OrderByDescending(eq => eq.GraduationDate)
                        .ToList();
            }
            catch
            {
                throw;
            }
        }

        public EmployeesQualifications GetByEmployeeQualificationID(int EmployeeQualificationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesQualifications
                       .Include("Qualifications").Include("QualificationsDegrees")
                       .FirstOrDefault(x => x.EmployeeQualificationID.Equals(EmployeeQualificationID));
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesQualifications> GetEmployeesQualifications()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesQualifications
                        .Include("QualificationsDegrees")
                        .Include("Qualifications")
                        .Include("GeneralSpecializations")
                        .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesQualifications> GetEmployeesQualifications(List<int> EmployeesCodesIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesQualifications
                        .Include("QualificationsDegrees")
                        .Include("Qualifications")
                        .Include("GeneralSpecializations")
                        .Where (x=> EmployeesCodesIDs.Contains(x.EmployeeCodeID.Value))
                        .ToList();
            }
            catch
            {
                throw;
            }
        }

        public EmployeesQualifications GetByQualificationID(int QualificationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesQualifications
                       .Include("Qualifications")
                       .Include("QualificationsDegrees")
                       .FirstOrDefault(x => x.QualificationID == QualificationID);
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesQualifications> GetEmployeesQualificationsByEmployeeCodeIDs(List<int> PromotionRecordEmployeeCodeIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesQualifications
                        .Include("Qualifications")
                        .Include("QualificationsDegrees")
                        .Where(x => PromotionRecordEmployeeCodeIDs.Contains(x.EmployeeCodeID.HasValue ? x.EmployeeCodeID.Value : 0))
                        .ToList();
            }
            catch
            {
                throw;
            }
        }

        public EmployeesQualifications GetByGeneralSpecializationID(int GeneralSpecializationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesQualifications
                       .Include("Qualifications")
                       .FirstOrDefault(x => x.GeneralSpecializationID == GeneralSpecializationID);
            }
            catch
            {
                throw;
            }
        }

        public EmployeesQualifications GetByExactSpecializationID(int ExactSpecializationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesQualifications
                       .Include("GeneralSpecializations")
                       .FirstOrDefault(x => x.ExactSpecializationID == ExactSpecializationID);
            }
            catch
            {
                throw;
            }
        }

        //public List<EmployeesQualifications> GetQualificationsBasedOnAssignings()
        //{
        //    try
        //    {
        //        var db = new HCMEntities();
        //        return db.EmployeesQualifications
        //               .Include("Qualifications")
        //               .Include("QualificationsDegrees")
        //               .Include("EmployeesCodes.Employees")
        //               .Include("EmployeesCodes.EmployeesCareersHistory.Ranks.RanksCategories")
        //               .Where(x=> x.EmployeesCodes.EmployeesCareersHistory.FirstOrDefault(z => z.IsActive == true))
        //        .ToList();
                       
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public List<vwQualificationsBasedOnAssignings> GetQualificationsBasedOnAssignings(int QualificationDegreeID, int QualificationID, int GeneralSpecializationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.vwQualificationsBasedOnAssignings
                       .Where(x => x.QualificationDegreeID == QualificationDegreeID 
                                && (QualificationID != 0 ? x.QualificationID == QualificationID  : x.QualificationID == x.QualificationID)
                                && (GeneralSpecializationID != 0 ? x.GeneralSpecializationID == GeneralSpecializationID : x.GeneralSpecializationID == x.GeneralSpecializationID)
                             )
                .ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}