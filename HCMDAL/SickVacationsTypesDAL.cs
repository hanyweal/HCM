using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class SickVacationsTypesDAL
    {
        public List<SickVacationsTypes> GetSickVacationsTypes()
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.SickVacationsTypes.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}