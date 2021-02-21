using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class VacationsActionsDAL
    {
        public List<VacationsActions> GetVacationsActions()
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.VacationsActions.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
