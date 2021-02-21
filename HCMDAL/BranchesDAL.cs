using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class BranchesDAL
    {
        public List<Branches> GetBranches()
        {
            try
            {
                var db = new HCMEntities();
                return db.Branches.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
