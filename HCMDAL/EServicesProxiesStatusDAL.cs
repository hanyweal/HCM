using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class EServicesProxiesStatusDAL
    {
        public List<EServicesProxiesStatus> GetEServicesProxiesStatus()
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.EServicesProxiesStatus.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}