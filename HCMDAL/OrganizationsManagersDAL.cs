using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class OrganizationsManagersDAL
    {
        public List<OrganizationsManagers> GetOrganizationsManagers(int OrganizationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsManagers.Include("OrganizationsStructures")
                                               .Include("ManagersCodesNav")
                                               .Where(x => x.OrganizationID == OrganizationID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OrganizationsManagers> GetOrganizationsOfManager(int ManagerCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsManagers.Include("OrganizationsStructures")
                                               .Include("ManagersCodesNav")
                                               .Where(x => x.ManagerCodeID == ManagerCodeID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
