using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class OrganizationsJobsStatusDAL
    {
        public List<OrganizationsJobsStatus> GetOrganizationsJobsStatus()
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsJobsStatus.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
