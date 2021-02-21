using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PassengerOrdersTypesDAL
    {
        public List<PassengerOrdersTypes> GetPassengerOrdersTypes()
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.PassengerOrdersTypes.ToList();
                }
            }
            catch
            {

                throw;
            }
        }
    }
}
