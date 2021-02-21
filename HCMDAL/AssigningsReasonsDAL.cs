using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class AssigningsReasonsDAL
    {
        public List<AssigningsReasons> GetAssigningsReasons()
        {
            try
            {
                var db = new HCMEntities();
                return db.AssigningsReasons.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
