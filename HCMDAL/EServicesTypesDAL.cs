using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class EServicesTypesDAL
    {
        public List<EServicesTypes> GetEServicesTypes()
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.EServicesTypes.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}