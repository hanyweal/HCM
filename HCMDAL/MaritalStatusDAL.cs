using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class MaritalStatusDAL
    {
        public List<MaritalStatus> GetMaritalStatus()
        {
            try
            {
                var db = new HCMEntities();
                return db.MaritalStatus.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
