using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class TeachersDAL
    {
        public int Insert(Teachers Teacher)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Teachers.Add(Teacher);
                    db.SaveChanges();
                    return Teacher.TeacherID;
                }
            }
            catch
            {
                throw;
            }
        }
        public int Update(Teachers Teacher)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Teachers TeacherObj = db.Teachers.FirstOrDefault(x => x.TeacherID.Equals(Teacher.TeacherID));
                    TeacherObj.EmployeeCareerHistoryID = Teacher.EmployeeCareerHistoryID;
                    TeacherObj.StartDate = Teacher.StartDate;
                    TeacherObj.EndDate = Teacher.EndDate;
                    TeacherObj.LastUpdatedBy = Teacher.LastUpdatedBy;
                    TeacherObj.LastUpdatedDate = DateTime.Now;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public int Delete(Teachers Teacher, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Teachers TeacherObj = db.Teachers.FirstOrDefault(x => x.TeacherID.Equals(Teacher.TeacherID));
                    db.Teachers.Remove(TeacherObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Teachers> GetTeachers()
        {
            try
            {
                var db = new HCMEntities();
                var query = db.Teachers.Include("EmployeesCareersHistory");
                query = query.Include("EmployeesCodes");
                return query.ToList();
            }
            catch
            {
                throw;
            }
        }
        public Teachers GetByTeacherID(int TeacherID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Teachers.Include("EmployeesCareersHistory")
                                  .Include("EmployeesCodes")
                                  .FirstOrDefault(x => x.TeacherID.Equals(TeacherID));
            }
            catch
            {
                throw;
            }
        }
        public List<Teachers> GetTeachersByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                var query = db.Teachers.Include("EmployeesCareersHistory");
                query = query.Include("EmployeesCodes");
                return query.Where(c => c.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}