using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class GendersDAL
    {
        public List<Genders> GetGenders()
        {
            try
            {
                var db = new HCMEntities();
                return db.Genders.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
