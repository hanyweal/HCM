using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class RanksCategoriesVacationsBalancesDAL
    {
        public List<RanksCategoriesVacationsBalances> GetRanksCategoriesVacationsBalances()
        {
            try
            {
                var db = new HCMEntities();
                return db.RanksCategoriesVacationsBalances.Include("VacationsTypes").Include("RanksCategories").ToList();
            }
            catch
            {
                throw;
            }
        }

        public RanksCategoriesVacationsBalances GetByRankCategoryIDVacationTypeID(int RankCategoryID, int VacationTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.RanksCategoriesVacationsBalances
                                .Include("VacationsTypes")
                                .Include("RanksCategories")
                                .FirstOrDefault(x => x.RankCategoryID == RankCategoryID && x.VacationTypeID == VacationTypeID);
            }
            catch
            {
                throw;
            }
        }
    }
}
