using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class EServicesAuthorizationsDAL
    {
        public int Insert(EServicesAuthorizations EServiceAuthorization)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EServicesAuthorizations.Add(EServiceAuthorization);
                    db.SaveChanges();
                    return EServiceAuthorization.EServiceAuthorizationID;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<EServicesAuthorizations> Get(int OrganizationID, int EServiceTypeID)
        {
			try
			{
                var db = new HCMEntities();
                return db.EServicesAuthorizations.Include("AuthorizedPersonNav")
                                                 .Include("EServicesTypes")
                                                 .Where(x =>  x.OrganizationID == OrganizationID && x.EServiceTypeID == EServiceTypeID).ToList();

            }
			catch (Exception ex)
			{
				throw ex;
			}
        }

        public List<EServicesAuthorizations> GetOrganizations(string  AuthorizedPersonCodeNo, int EServiceTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EServicesAuthorizations.Include("AuthorizedPersonNav")
                                                 .Include("EServicesTypes")
                                                 .Include("OrganizationsStructures")
                                                 .Where(x => x.AuthorizedPersonNav.EmployeeCodeNo == AuthorizedPersonCodeNo && x.EServiceTypeID == EServiceTypeID).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EServicesAuthorizations> GetOrganizationsByOrganizationIDs(List<int> OrganizationIDs, int EServiceTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EServicesAuthorizations.Include("AuthorizedPersonNav")
                                                 .Include("EServicesTypes")
                                                 .Include("OrganizationsStructures")
                                                 .Where(x => x.EServiceTypeID == EServiceTypeID
                                                        && OrganizationIDs.Contains(x.OrganizationID)).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EServicesAuthorizations GetByOrganizationID(int OrganizationID, int EServiceTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EServicesAuthorizations.Include("AuthorizedPersonNav")
                                                 .Include("EServicesTypes")
                                                 .Include("OrganizationsStructures")
                                                 .FirstOrDefault(x => x.EServiceTypeID == EServiceTypeID && x.OrganizationID == OrganizationID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SetAuthorizations(int EServiceTypeID, List<int> ChildOrganizationIDs, int AuthorizedPersonCodeID, int LastUpdatedBy)
        {
            try
            {                
                using (var db = new HCMEntities())
                {
                    List<EServicesAuthorizations> EServiceAuthorizationList = db.EServicesAuthorizations
                                                                                .Where(x => x.EServiceTypeID == EServiceTypeID
                                                                                    && ChildOrganizationIDs.Contains(x.OrganizationID)).ToList();
                    foreach (EServicesAuthorizations item in EServiceAuthorizationList)
                    {
                        item.AuthorizedPersonCodeID = AuthorizedPersonCodeID;
                        item.LastUpdatedBy = LastUpdatedBy;
                        item.LastUpdatedDate = DateTime.Now;
                    }
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ChangeAuthorizedPersonForAllChildByOrganizationID(List<int> ChildOrganizationIDs, int OldAuthorizedPersonCodeID, int NewAuthorizedPersonCodeID, int LastUpdatedBy)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    List<EServicesAuthorizations> EServiceAuthorizationList = db.EServicesAuthorizations
                                                                                .Where(x => x.AuthorizedPersonCodeID == OldAuthorizedPersonCodeID
                                                                                    && ChildOrganizationIDs.Contains(x.OrganizationID)).ToList();
                    foreach (EServicesAuthorizations item in EServiceAuthorizationList)
                    {
                        item.AuthorizedPersonCodeID = NewAuthorizedPersonCodeID;
                        item.LastUpdatedBy = LastUpdatedBy;
                        item.LastUpdatedDate = DateTime.Now;
                    }
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
