using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class TransfersTypesDAL
    {
        public List<TransfersTypes> GetTransfersTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.TransfersTypes.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
