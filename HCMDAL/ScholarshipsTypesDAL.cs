using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class ScholarshipsTypesDAL
    {
        public List<ScholarshipsTypes> GetScholarshipsTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.ScholarshipsTypes.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
