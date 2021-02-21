using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PeriodsDAL
    {
        public List<Periods> GetPeriods()
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.Periods.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}


