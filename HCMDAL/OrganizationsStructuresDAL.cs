using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class OrganizationsStructuresDAL
    {
        public int Insert(OrganizationsStructures OrganizationStructure)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.OrganizationsStructures.Add(OrganizationStructure);
                    db.SaveChanges();
                    return OrganizationStructure.OrganizationID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(OrganizationsStructures OrganizationStructure)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OrganizationsStructures OrganizationStructureObj = db.OrganizationsStructures.FirstOrDefault(x => x.OrganizationID.Equals(OrganizationStructure.OrganizationID));
                    OrganizationStructureObj.OrganizationName = OrganizationStructure.OrganizationName;
                    OrganizationStructureObj.OrganizationCode = OrganizationStructure.OrganizationCode;
                    OrganizationStructureObj.BranchID = OrganizationStructure.BranchID;
                    OrganizationStructureObj.OrganizationParentID = OrganizationStructure.OrganizationParentID;
                    OrganizationStructureObj.ManagerCodeID = OrganizationStructure.ManagerCodeID;
                    OrganizationStructureObj.LastUpdatedDate = OrganizationStructure.LastUpdatedDate;
                    OrganizationStructureObj.LastUpdatedBy = OrganizationStructure.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateManager(OrganizationsStructures OrganizationStructure)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OrganizationsStructures OrganizationStructureObj = db.OrganizationsStructures.FirstOrDefault(x => x.OrganizationID.Equals(OrganizationStructure.OrganizationID));
                    OrganizationStructureObj.ManagerCodeID = OrganizationStructure.ManagerCodeID;
                    OrganizationStructureObj.LastUpdatedDate = OrganizationStructure.LastUpdatedDate;
                    OrganizationStructureObj.LastUpdatedBy = OrganizationStructure.LastUpdatedBy;

                    // update old manager todate
                    OrganizationsManagers OrganizationsManagerObj = db.OrganizationsManagers.OrderByDescending(x => x.FromDate).FirstOrDefault(x => x.OrganizationID.Equals(OrganizationStructure.OrganizationID) && !x.ToDate.HasValue);
                    if (OrganizationsManagerObj != null)
                    {
                        OrganizationsManagerObj.ToDate = DateTime.Now.Date;
                        OrganizationsManagerObj.LastUpdatedDate = OrganizationStructure.LastUpdatedDate;
                        OrganizationsManagerObj.LastUpdatedBy = OrganizationStructure.LastUpdatedBy;
                    }

                    // insert new record of new manager
                    db.OrganizationsManagers.Add(new OrganizationsManagers()
                    {
                        ManagerCodeID = OrganizationStructure.ManagerCodeID.Value,
                        OrganizationID = OrganizationStructure.OrganizationID,
                        FromDate = DateTime.Now,
                        CreatedBy = OrganizationStructure.LastUpdatedBy.Value,
                        CreatedDate = OrganizationStructure.LastUpdatedDate.Value
                    });

                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateShowInECM(OrganizationsStructures OrganizationStructure)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OrganizationsStructures OrganizationStructureObj = db.OrganizationsStructures.FirstOrDefault(x => x.OrganizationID.Equals(OrganizationStructure.OrganizationID));
                    OrganizationStructureObj.ShowInECM = OrganizationStructure.ShowInECM;
                    OrganizationStructureObj.LastUpdatedDate = OrganizationStructure.LastUpdatedDate;
                    OrganizationStructureObj.LastUpdatedBy = OrganizationStructure.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateWorkingWithDigitalDecisionInECM(OrganizationsStructures OrganizationStructure)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OrganizationsStructures OrganizationStructureObj = db.OrganizationsStructures.FirstOrDefault(x => x.OrganizationID.Equals(OrganizationStructure.OrganizationID));
                    OrganizationStructureObj.WorkingWithDigitalDecisionInECM = OrganizationStructure.WorkingWithDigitalDecisionInECM;
                    OrganizationStructureObj.LastUpdatedDate = OrganizationStructure.LastUpdatedDate;
                    OrganizationStructureObj.LastUpdatedBy = OrganizationStructure.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(int OrganizationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OrganizationsStructures OrganizationStructureObj = db.OrganizationsStructures.FirstOrDefault(x => x.OrganizationID.Equals(OrganizationID));
                    db.OrganizationsStructures.Remove(OrganizationStructureObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OrganizationsStructures> GetOrganizationStructure()
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsStructures.Include("Branches")
                                                 .Include("ParentOrganization").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OrganizationsStructures GetByOrganizationID(int OrganizationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsStructures.Include("Branches")
                                                 .Include("ParentOrganization")
                                                 //.Include("ParentOrganization.Branches")
                                                 //.Include("EmployeesCodes")
                                                 .Include("EmployeesCodes.Employees")
                                                 .Include("EmployeesCodes.EmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                                 .FirstOrDefault(x => x.OrganizationID.Equals(OrganizationID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OrganizationsStructures> GetFirstLevel()
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsStructures.Include("Branches").Where(x => x.OrganizationParentID == null).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OrganizationsStructures> GetChildByOrganizationID(int OrganizationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsStructures
                    .Include("Branches")
                    .Where(x => x.OrganizationParentID == OrganizationID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OrganizationsStructures> GetOrganizationsUnderManagerByManagerCodeID(int ManagerCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsStructures
                    .Include("Branches")
                    .Include("ParentOrganization")
                    .Where(x => x.ManagerCodeID == ManagerCodeID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OrganizationsStructures> GetByOrganizationIDs(List<int> OrganizationIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsStructures
                        .Include("EmployeesCodes")
                        .Where(x => OrganizationIDs.Contains(x.OrganizationID)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
