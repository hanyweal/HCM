using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class QualificationsTypesDAL
    {
        public List<QualificationsTypes> GetQualificationsTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.QualificationsTypes.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
