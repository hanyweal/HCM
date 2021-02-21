using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class RanksDAL
    {
        public List<Ranks> GetRanks()
        {
            try
            {
                var db = new HCMEntities();
                return db.Ranks.Include("RanksCategories").Include("NextRanks").ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Ranks> GetByRankCategoryID(int RankCategoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Ranks.Include("RanksCategories").Include("NextRanks")
                                .Where(c => c.RankCategoryID == RankCategoryID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateIsPromotion(Ranks Rank)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    // first marked all IsActiveForPromotion = false
                    List<Ranks> RankList = db.Ranks.Where(x=> x.IsActiveForPromotion == true).ToList();
                    foreach (var item in RankList)
                    {
                        item.IsActiveForPromotion = false;
                        item.LastUpdatedDate = Rank.LastUpdatedDate;
                        item.LastUpdatedBy = Rank.LastUpdatedBy;
                    } 

                    if (Rank.RankID > 0)
                    {
                        Ranks RankObj = db.Ranks.FirstOrDefault(x => x.RankID == Rank.RankID);
                        RankObj.IsActiveForPromotion = true;
                        RankObj.LastUpdatedDate = Rank.LastUpdatedDate;
                        RankObj.LastUpdatedBy = Rank.LastUpdatedBy;
                    }

                    return db.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                //}
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
