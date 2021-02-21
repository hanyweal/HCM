using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class NormalVacationsTypesDAL
    {
        public List<NormalVacationsTypes> GetNormalVacationsTypes()
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.NormalVacationsTypes.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
