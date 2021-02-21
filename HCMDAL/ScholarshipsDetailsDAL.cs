using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class ScholarshipsDetailsDAL
    {
        public int Insert(ScholarshipsDetails ScholarshipDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.ScholarshipsDetails.Add(ScholarshipDetail);
                    db.SaveChanges();
                    return ScholarshipDetail.ScholarshipDetailID;
                }
            }
            catch
            {
                throw;
            }
        }

        //public int Update(ScholarshipsDetails ScholarshipDetail)
        //{
        //    try
        //    {
        //        using (var db = new HCMEntities())
        //        {
        //            ScholarshipsDetails ScholarshipDetailObj = db.ScholarshipsDetails.FirstOrDefault(x => x.ScholarshipDetailID.Equals(ScholarshipDetail.ScholarshipDetailID));
        //            ScholarshipDetailObj.LastUpdatedBy = ScholarshipDetail.LastUpdatedBy;
        //            ScholarshipDetailObj.LastUpdatedDate = ScholarshipDetail.LastUpdatedDate;
        //            return db.SaveChanges();
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public int UpdateCreationDates(ScholarshipsDetails ScholarshipDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    ScholarshipsDetails ScholarshipDetailObj = db.ScholarshipsDetails.FirstOrDefault(x => x.ScholarshipID.Equals(ScholarshipDetail.ScholarshipID));
                    ScholarshipDetailObj.FromDate = ScholarshipDetail.FromDate;
                    ScholarshipDetailObj.ToDate = ScholarshipDetail.ToDate;
                    ScholarshipDetailObj.LastUpdatedBy = ScholarshipDetail.LastUpdatedBy;
                    ScholarshipDetailObj.LastUpdatedDate = ScholarshipDetail.LastUpdatedDate;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }


        public int Delete(int ScholarshipDetailID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    ScholarshipsDetails ScholarshipDetailObj = db.ScholarshipsDetails.FirstOrDefault(x => x.ScholarshipDetailID.Equals(ScholarshipDetailID));
                    db.ScholarshipsDetails.Remove(ScholarshipDetailObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<ScholarshipsDetails> GetByScholarshipID(int ScholarshipID)
        {
            try
            {
                var db = new HCMEntities();
                return db.ScholarshipsDetails.Include("Scholarships")
                                            .Include("ScholarshipsActions")
                                            .Include("CreatedByNav.Employees")
                                            .Include("LastUpdatedByNav.Employees")
                                            .Where(x => x.ScholarshipID == ScholarshipID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public ScholarshipsDetails GetByScholarshipDetailID(int ScholarshipDetailID)
        {
            try
            {
                var db = new HCMEntities();
                return db.ScholarshipsDetails.Include("Scholarships")
                                            .Include("ScholarshipsActions")
                                            .Include("CreatedByNav.Employees")
                                            .Include("LastUpdatedByNav.Employees")
                                            .FirstOrDefault(x => x.ScholarshipDetailID == ScholarshipDetailID);
            }
            catch
            {
                throw;
            }
        }
    }
}