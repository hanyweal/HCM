using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class CareersHistoryTypesDAL
    {
        public List<CareersHistoryTypes> GetCareersHistoryTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.CareersHistoryTypes.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
