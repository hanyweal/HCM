using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class DelegationsDetailsDAL
    {
        public List<DelegationsDetails> GetDelegationsDetails()
        {
            try
            {
                var db = new HCMEntities();
                return db.DelegationsDetails.Include("Delegations")
                                            //.Include("EmployeesCareersHistory")
                                            //.Include("EmployeesCareersHistory.EmployeesCodes")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees").ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<DelegationsDetails> GetDelegationsDetailsByDelegationID(int DelegationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.DelegationsDetails.Include("Delegations")
                                            .Include("Delegations.DelegationsTypes")
                                            .Include("Delegations.DelegationsKinds")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                            .Where(x => x.DelegationID == DelegationID).ToList();

            }
            catch
            {
                throw;
            }
        }

        public DelegationsDetails GetDelegationsDetailsByDelegationDetailID(int DelegationDetailID)
        {
            try
            {
                var db = new HCMEntities();
                return db.DelegationsDetails.Include("Delegations")
                                            .Include("Delegations.DelegationsTypes")
                                            .Include("Delegations.DelegationsKinds")
                                                //.Include("EmployeesCareersHistory")
                                                //.Include("EmployeesCareersHistory.EmployeesCodes")
                                                .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                                .SingleOrDefault(x => x.DelegationDetailID == DelegationDetailID);

            }
            catch
            {
                throw;
            }
        }

        public int Delete(int DelegationDetailID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    DelegationsDetails DelegationDetailObj = db.DelegationsDetails.SingleOrDefault(x => x.DelegationDetailID.Equals(DelegationDetailID));
                    db.DelegationsDetails.Remove(DelegationDetailObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int Insert(DelegationsDetails DelegationDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.DelegationsDetails.Add(DelegationDetail);
                    db.SaveChanges();
                    return DelegationDetail.DelegationDetailID;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<DelegationsDetails> GetDelegationsDetailsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.DelegationsDetails.Include("Delegations")
                                            .Include("Delegations.DelegationsTypes")
                                            .Include("Delegations.DelegationsKinds")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                            .Where(x => x.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeID == EmployeeCodeID).ToList();

            }
            catch
            {
                throw;
            }
        }

        public List<DelegationsDetails> GetEmployeesDelegationsByDate(DateTime DelegationDate, int DelegationTypeID, IList<int> EmployeesCareerHistoryIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.DelegationsDetails
                                            .Include("Delegations.DelegationsTypes")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                            .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks.RanksCategories")
                                            .Where(x => (x.Delegations.DelegationStartDate >= DelegationDate)
                                                    //(DelegationDate >= x.Delegations.DelegationStartDate && DelegationDate <= x.Delegations.DelegationEndDate)
                                                    && (DelegationTypeID != 0 ? x.Delegations.DelegationTypeID == DelegationTypeID : x.Delegations.DelegationTypeID == x.Delegations.DelegationTypeID)
                                                    && EmployeesCareerHistoryIDs.Contains(x.EmployeeCareerHistoryID)
                                            ).ToList();

            }
            catch
            {
                throw;
            }
        }
    }
}
