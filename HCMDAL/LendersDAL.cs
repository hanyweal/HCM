using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class LendersDAL
    {
        public int Insert(Lenders Lender)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Lenders.Add(Lender);
                    db.SaveChanges();
                    return Lender.LenderID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Update(Lenders Lender)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Lenders LenderObj = db.Lenders.SingleOrDefault(x => x.LenderID.Equals(Lender.LenderID));
                    LenderObj.LenderStartDate = Lender.LenderStartDate;
                    LenderObj.LenderEndDate = Lender.LenderEndDate;
                    LenderObj.EmployeeCareerHistoryID = Lender.EmployeeCareerHistoryID;
                    LenderObj.IsFinished = Lender.IsFinished;
                    LenderObj.LenderOrganization = Lender.LenderOrganization;
                    LenderObj.KSACityID = Lender.KSACityID;
                    LenderObj.LenderEndReason = "" + Lender.LenderEndReason;
                    LenderObj.LastUpdatedDate = Lender.LastUpdatedDate;
                    LenderObj.LastUpdatedBy = Lender.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int EndLender(Lenders Lender)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Lenders LenderObj = db.Lenders.SingleOrDefault(x => x.LenderID.Equals(Lender.LenderID));
                    LenderObj.LenderStartDate = Lender.LenderStartDate;
                    LenderObj.LenderEndDate = Lender.LenderEndDate;
                    LenderObj.EmployeeCareerHistoryID = Lender.EmployeeCareerHistoryID;
                    LenderObj.IsFinished = Lender.IsFinished;
                    LenderObj.LenderOrganization = Lender.LenderOrganization;
                    LenderObj.KSACityID = Lender.KSACityID;
                    LenderObj.LenderEndReason = "" + Lender.LenderEndReason;
                    LenderObj.LastUpdatedDate = Lender.LastUpdatedDate;
                    LenderObj.LastUpdatedBy = Lender.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(Lenders Lender, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Lenders LenderObj = db.Lenders.SingleOrDefault(x => x.LenderID.Equals(Lender.LenderID));
                    db.Lenders.Remove(LenderObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Lenders> GetLenders()
        {
            try
            {
                var db = new HCMEntities();
                return db.Lenders.Include("EmployeesCareersHistory")
                                .Include("EmployeesCareersHistory.EmployeesCodes")
                                .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("KSACities.KSARegions")
                                .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<Lenders> GetLendersByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Lenders.Include("EmployeesCareersHistory")
                        .Where(x => x.EmployeesCareersHistory.EmployeeCodeID.Equals(EmployeeCodeID)).ToList();
            }
            catch
            {
                throw;
            }
        }

        public Lenders GetByLenderID(int LenderID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Lenders.Include("EmployeesCareersHistory")
                                .Include("EmployeesCareersHistory.EmployeesCodes")
                                .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                .Include("KSACities.KSARegions")
                                .Include("CreatedByNav.Employees")
                                .Include("LastUpdatedByNav.Employees")
                                .FirstOrDefault(x => x.LenderID.Equals(LenderID));
            }
            catch
            {
                throw;
            }
        }

        public int GetCountByEmployeeCodeIDAndDateDuration(int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var db = new HCMEntities();
                return db.Lenders.Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID &&
                                        (
                                            (StartDate >= x.LenderStartDate && StartDate <= x.LenderEndDate) ||
                                            (EndDate >= x.LenderStartDate && EndDate <= x.LenderEndDate) ||
                                            (StartDate >= x.LenderStartDate && EndDate <= x.LenderEndDate) ||
                                            (StartDate <= x.LenderStartDate && EndDate >= x.LenderEndDate)
                                        )
                                    ).Count();
            }
            catch
            {
                throw;
            }
        }

    }
}
