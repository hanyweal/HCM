using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class CareersDegreesDAL
    {
        public List<CareersDegrees> GetCareersDegrees()
        {
            try
            {
                var db = new HCMEntities();
                return db.CareersDegrees.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
