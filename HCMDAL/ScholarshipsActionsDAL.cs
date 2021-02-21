using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class ScholarshipsActionsDAL
    {
        public List<ScholarshipsActions> GetScholarshipsActions()
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.ScholarshipsActions.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
