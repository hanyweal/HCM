using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class VacationsTypesDAL
    {
        public List<VacationsTypes> GetVacationsTypes()
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.VacationsTypes.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
