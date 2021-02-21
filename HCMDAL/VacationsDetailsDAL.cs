using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class VacationsDetailsDAL
    {
        public int Insert(VacationsDetails VacationDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.VacationsDetails.Add(VacationDetail);
                    db.SaveChanges();
                    return VacationDetail.VacationDetailID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Update(VacationsDetails VacationDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    VacationsDetails VacationDetailObj = db.VacationsDetails.FirstOrDefault(x => x.VacationDetailID.Equals(VacationDetail.VacationDetailID));
                    VacationDetailObj.IsApproved = VacationDetail.IsApproved;
                    VacationDetailObj.ApprovedBy = VacationDetail.ApprovedBy;
                    VacationDetailObj.ApprovedDate = VacationDetail.ApprovedDate;
                    VacationDetailObj.LastUpdatedBy = VacationDetail.LastUpdatedBy;
                    VacationDetailObj.LastUpdatedDate = VacationDetail.LastUpdatedDate;

                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateCreationDates(VacationsDetails VacationDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    VacationsDetails VacationDetailObj = db.VacationsDetails.FirstOrDefault(x => x.VacationID.Equals(VacationDetail.VacationID));
                    VacationDetailObj.FromDate = VacationDetail.FromDate;
                    VacationDetailObj.ToDate = VacationDetail.ToDate;
                    VacationDetailObj.Notes = VacationDetail.Notes;
                    VacationDetailObj.LastUpdatedBy = VacationDetail.LastUpdatedBy;
                    VacationDetailObj.LastUpdatedDate = VacationDetail.LastUpdatedDate;

                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int VacationDetailID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    VacationsDetails VacationDetailObj = db.VacationsDetails.FirstOrDefault(x => x.VacationDetailID.Equals(VacationDetailID));
                    db.VacationsDetails.Remove(VacationDetailObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<VacationsDetails> GetByVacationID(int VacationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.VacationsDetails.Include("Vacations")
                                            .Include("VacationsActions")
                                            .Include("CreatedByNav.Employees")
                                            .Include("ApprovedByNav.Employees")
                                            .Include("LastUpdatedByNav.Employees")
                                            .Where(x => x.VacationID == VacationID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public VacationsDetails GetByVacationDetailID(int VacationDetailID)
        {
            try
            {
                var db = new HCMEntities();
                return db.VacationsDetails.Include("Vacations")
                                            .Include("VacationsActions")
                                            .Include("CreatedByNav.Employees")
                                            .Include("ApprovedByNav.Employees")
                                            .Include("LastUpdatedByNav.Employees")
                                            .FirstOrDefault(x => x.VacationDetailID == VacationDetailID);
            }
            catch
            {
                throw;
            }
        }

        public List<VacationsDetails> GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.VacationsDetails.Include("Vacations")
                                            .Include("Vacations.VacationsTypes")
                                            .Include("VacationsActions")
                                            .Include("Vacations.EmployeesCareersHistory")
                                            .Include("CreatedByNav.Employees")
                                            .Include("ApprovedByNav.Employees")
                                            .Include("LastUpdatedByNav.Employees")
                                            .Where(x => x.Vacations.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public VacationsDetails GetVacationsDetailsByVacationID(int VacationID, int VacationActionID)
        {
            try
            {
                var db = new HCMEntities();
                return db.VacationsDetails.Where(x => x.VacationID == VacationID && x.VacationActionID == VacationActionID).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
    }
}