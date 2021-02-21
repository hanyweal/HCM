using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class SenioritysPointsDeclarationsDAL
    {
        public List<SenioritysPointsDeclarations> GetSenioritysPointsDeclarations()
        {
            try
            {
                var db = new HCMEntities();
                return db.SenioritysPointsDeclarations.ToList();
            }
            catch
            {
                throw;
            }
        }


        public SenioritysPointsDeclarations GetByMonths(int months)
        {
            try
            {
                var db = new HCMEntities();
                return db.SenioritysPointsDeclarations.FirstOrDefault(e => e.Months == months);
            }
            catch
            {
                throw;
            }
        }

    }
}