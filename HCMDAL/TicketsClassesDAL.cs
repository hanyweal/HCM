using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class TicketsClassesDAL
    {
        public List<TicketsClasses> GetTicketsClasses()
        {
            try
            {
                var db = new HCMEntities();
                return db.TicketsClasses.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
