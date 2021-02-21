using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
   public class EmployeeExperiencesWithDetailsDAL
    {
        public int Insert(EmployeeExperiencesWithDetails EmployeeExperiencesWithDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeeExperiencesWithDetails.Add(EmployeeExperiencesWithDetail);
                    db.SaveChanges();
                    return EmployeeExperiencesWithDetail.EmployeeExperienceWithDetailID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Update(EmployeeExperiencesWithDetails EmployeeExperiencesWithDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeeExperiencesWithDetails EmployeeExperiencesWithDetailObj = db.EmployeeExperiencesWithDetails.SingleOrDefault(x => x.EmployeeExperienceWithDetailID.Equals(EmployeeExperiencesWithDetail.EmployeeExperienceWithDetailID));
                    EmployeeExperiencesWithDetailObj.EmployeeCodeID = EmployeeExperiencesWithDetail.EmployeeCodeID;
                    EmployeeExperiencesWithDetailObj.FromDate = EmployeeExperiencesWithDetail.FromDate;
                    EmployeeExperiencesWithDetailObj.ToDate = EmployeeExperiencesWithDetail.ToDate;
                    EmployeeExperiencesWithDetailObj.JobName = EmployeeExperiencesWithDetail.JobName;
                    EmployeeExperiencesWithDetailObj.SectorName = EmployeeExperiencesWithDetail.SectorName;
                    EmployeeExperiencesWithDetailObj.SectorTypeID = EmployeeExperiencesWithDetail.SectorTypeID;
                    EmployeeExperiencesWithDetailObj.LastUpdatedDate = EmployeeExperiencesWithDetail.LastUpdatedDate;
                    EmployeeExperiencesWithDetailObj.LastUpdatedBy = EmployeeExperiencesWithDetail.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int EmployeeExperienceWithDetailID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeeExperiencesWithDetails EmployeeExperiencesWithDetailObj = db.EmployeeExperiencesWithDetails.SingleOrDefault(x => x.EmployeeExperienceWithDetailID.Equals(EmployeeExperienceWithDetailID));
                    db.EmployeeExperiencesWithDetails.Remove(EmployeeExperiencesWithDetailObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeeExperiencesWithDetails> GetEmployeeExperienceWithDetails()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeeExperiencesWithDetails.Include("EmployeesCodes").Include("SectorsTypes")
                                                        .OrderBy(x => x.EmployeeCodeID)
                                                        .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeeExperiencesWithDetails> GetEmployeeExperiencesByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeeExperiencesWithDetails.Where(x => x.EmployeeCodeID.Equals(EmployeeCodeID)).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeeExperiencesWithDetails> GetEmployeeExperiencesByEmployeeCodeID(int EmployeeCodeID, int SectorTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeeExperiencesWithDetails.Where(x => x.EmployeeCodeID.Equals(EmployeeCodeID) && x.SectorTypeID.Equals(SectorTypeID)).ToList();
            }
            catch
            {
                throw;
            }
        }

        public EmployeeExperiencesWithDetails GetByEmployeeExperienceWithDetailID(int EmployeeExperienceWithDetailID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeeExperiencesWithDetails.Include("EmployeesCodes").Include("SectorsTypes").SingleOrDefault(x => x.EmployeeExperienceWithDetailID.Equals(EmployeeExperienceWithDetailID));
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeeExperiencesWithDetails> GetEmployeeExperiencesWithDetailsByEmployeeCodeIDs(List<int> EmployeeCodeIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeeExperiencesWithDetails
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
