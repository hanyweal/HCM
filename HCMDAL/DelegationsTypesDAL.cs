using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class DelegationsTypesDAL
    {
        public List<DelegationsTypes> GetDelegationsTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.DelegationsTypes.ToList();
            }
            catch
            {
                throw;
            }
        }

        public DelegationsTypes GetByDelegationTypeID(int DelegationTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.DelegationsTypes.SingleOrDefault(x => x.DelegationTypeID.Equals(DelegationTypeID));
            }
            catch
            {
                throw;
            }
        }
    }
}
