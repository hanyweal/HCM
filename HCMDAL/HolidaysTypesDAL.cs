using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class HolidaysTypesDAL
    {
        public List<HolidaysTypes> GetHolidaysTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.HolidaysTypes.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
