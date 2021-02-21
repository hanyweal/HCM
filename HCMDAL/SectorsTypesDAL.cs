using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class SectorsTypesDAL
    {
        public List<SectorsTypes> GetSectorsTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.SectorsTypes.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
