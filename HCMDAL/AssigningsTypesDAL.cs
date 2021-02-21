using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class AssigningsTypesDAL
    {
        public List<AssigningsTypes> GetAssigningsTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.AssigningsTypes.ToList();
            }
            catch
            {
                throw;
            }
        }

        public AssigningsTypes GetByAssigningTypeID(int AssigningTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.AssigningsTypes.SingleOrDefault(x => x.AssigningTypID.Equals(AssigningTypeID));
            }
            catch
            {
                throw;
            }
        }
    }
}
