using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class EvaluationsQuartersDAL
    {
        public List<EvaluationsQuarters> GetEvaluationsQuarters()
        {
            try
            {
                var db = new HCMEntities();
                return db.EvaluationsQuarters.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}

