using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class EmployeesExperiencesDAL
    {
        public int Insert(EmployeesExperiences EmployeeExperience)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeesExperiences.Add(EmployeeExperience);
                    db.SaveChanges();
                    return EmployeeExperience.EmployeeExperienceID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Update(EmployeesExperiences EmployeeExperience)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesExperiences EmployeeExperienceObj = db.EmployeesExperiences.SingleOrDefault(x => x.EmployeeExperienceID.Equals(EmployeeExperience.EmployeeExperienceID));
                    EmployeeExperienceObj.EmployeeCodeID = EmployeeExperience.EmployeeCodeID;
                    EmployeeExperienceObj.TotalYears = EmployeeExperience.TotalYears;
                    EmployeeExperienceObj.TotalMonths = EmployeeExperience.TotalMonths;
                    EmployeeExperienceObj.TotalDays = EmployeeExperience.TotalDays;
                    EmployeeExperienceObj.LastUpdatedDate = EmployeeExperience.LastUpdatedDate;
                    EmployeeExperienceObj.LastUpdatedBy = EmployeeExperience.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int EmployeeExperienceID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesExperiences EmployeeExperienceObj = db.EmployeesExperiences.SingleOrDefault(x => x.EmployeeExperienceID.Equals(EmployeeExperienceID));
                    db.EmployeesExperiences.Remove(EmployeeExperienceObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesExperiences> GetEmployeesExperiences()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesExperiences.Include("EmployeesCodes")
                                 .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesExperiences> GetEmployeeExperiencesByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesExperiences.Where(x => x.EmployeeCodeID.Equals(EmployeeCodeID)).ToList();
            }
            catch
            {
                throw;
            }
        }

        public EmployeesExperiences GetByEmployeeExperienceID(int EmployeeExperienceID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesExperiences.Include("EmployeesCodes").SingleOrDefault(x => x.EmployeeExperienceID.Equals(EmployeeExperienceID));
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesExperiences> GetEmployeesExperiencesByEmployeeCodeIDs(List<int> EmployeeCodeIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesExperiences
                                .Where(x => EmployeeCodeIDs.Contains(x.EmployeeCodeID))
                                .ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
