using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class GovernmentFundsDeactiveReasonsDAL
    {
        public List<GovernmentFundsDeactiveReasons> GetGovernmentFundsDeactiveReasons()
        {
            try
            {
                var db = new HCMEntities();
                return db.GovernmentFundsDeactiveReasons
                .ToList();

            }
            catch
            {
                throw;
            }
        }

        public GovernmentFundsDeactiveReasons GetByGovernmentFundDeactiveReasonID(int GovernmentFundDeactiveReasonID)
        {
            try
            {
                var db = new HCMEntities();
                return db.GovernmentFundsDeactiveReasons
                .SingleOrDefault(x => x.GovernmentFundDeactiveReasonID.Equals(GovernmentFundDeactiveReasonID));

            }
            catch
            {
                throw;
            }
        }
    }
}
