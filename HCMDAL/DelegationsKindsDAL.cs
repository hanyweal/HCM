using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class DelegationsKindsDAL
    {
        public List<DelegationsKinds> GetDelegationsKinds()
        {
            try
            {
                var db = new HCMEntities();
                return db.DelegationsKinds.ToList();
            }
            catch
            {
                throw;
            }
        }

        public DelegationsKinds GetByDelegationkindID(int DelegationKindID)
        {
            try
            {
                var db = new HCMEntities();
                return db.DelegationsKinds.SingleOrDefault(x => x.DelegationKindID.Equals(DelegationKindID));
            }
            catch
            {
                throw;
            }
        }
    }
}
