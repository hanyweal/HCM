using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq.Dynamic;

namespace HCMDAL
{
    public class PromotionsRecordsQualificationsPointsDAL : CommonEntityDAL
    {
        public int Insert(PromotionsRecordsQualificationsPoints PromotionRecordQualificationPoint)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PromotionsRecordsQualificationsPoints.Add(PromotionRecordQualificationPoint);
                    db.SaveChanges();
                    return PromotionRecordQualificationPoint.PromotionRecordQualificationPointID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Insert(List<PromotionsRecordsQualificationsPoints> PromotionRecordQualificationsPoints)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PromotionsRecordsQualificationsPoints.AddRange(PromotionRecordQualificationsPoints);
                    return db.SaveChanges();
                    //return PromotionRecordJobVacancy.PromotionRecordJobVacancyID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int DeleteByPromotionRecordID(int PromotionRecordID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    List<PromotionsRecordsQualificationsPoints> PromotionRecordQualificationsPointsObj = db.PromotionsRecordsQualificationsPoints.Where(x => x.PromotionRecordID.Equals(PromotionRecordID)).ToList();
                    db.PromotionsRecordsQualificationsPoints.RemoveRange(PromotionRecordQualificationsPointsObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateQualificationKindAndPoints(PromotionsRecordsQualificationsPoints Obj)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecordsQualificationsPoints PromotionRecordQualificationPointObj =
                            db.PromotionsRecordsQualificationsPoints.SingleOrDefault(x => x.PromotionRecordQualificationPointID.Equals(Obj.PromotionRecordQualificationPointID));

                    PromotionRecordQualificationPointObj.PromotionRecordQualificationKindID = Obj.PromotionRecordQualificationKindID;
                    PromotionRecordQualificationPointObj.Points = Obj.Points;
                    PromotionRecordQualificationPointObj.LastUpdatedBy = Obj.LastUpdatedBy;
                    PromotionRecordQualificationPointObj.LastUpdatedDate = Obj.LastUpdatedDate;

                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int PromotionRecordQualificationPointID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecordsQualificationsPoints PromotionRecordEmployeeObj = db.PromotionsRecordsQualificationsPoints.FirstOrDefault(x => x.PromotionRecordQualificationPointID.Equals(PromotionRecordQualificationPointID));
                    db.PromotionsRecordsQualificationsPoints.Remove(PromotionRecordEmployeeObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsQualificationsPoints> GetPromotionsRecordsQualificationsPoints()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsQualificationsPoints
                                .Include("PromotionsRecords")
                                .Include("GeneralSpecializations")
                                .Include("Qualifications")
                                .Include("QualificationsDegrees")
                                .Include("PromotionsRecordsQualificationsKinds")
                                .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsQualificationsPoints> GetByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                var db = new HCMEntities();

                return db.PromotionsRecordsQualificationsPoints
                                .Include("PromotionsRecords")
                                .Include("GeneralSpecializations")
                                .Include("Qualifications")
                                .Include("QualificationsDegrees")
                                .Include("PromotionsRecordsQualificationsKinds")
                                .Where(x => x.PromotionRecordID == PromotionRecordID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsQualificationsPoints> GetByPromotionRecordID(int PromotionRecordID, out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();
                var odData = db.PromotionsRecordsQualificationsPoints
                                .Include("PromotionsRecords")
                                .Include("GeneralSpecializations")
                                .Include("Qualifications")
                                .Include("QualificationsDegrees")
                                .Include("PromotionsRecordsQualificationsKinds")
                                .Where(x => x.PromotionRecordID == PromotionRecordID);

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<PromotionsRecordsQualificationsPoints> iq = odData.Where(p => p.PromotionRecordQualificationPointID != null);

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                        p.QualificationsDegrees.QualificationDegreeName.ToLower().Contains(search.ToLower()) ||
                        p.Qualifications.QualificationName.ToLower().Contains(search.ToLower()) ||
                        p.GeneralSpecializations.GeneralSpecializationName.ToLower().Contains(search.ToLower())
                    );
                }

                // Sorting.
                iq = iq.OrderBy(p => p.PromotionRecordQualificationPointID);
                //orderByColumnName = " QualificationDegrees.QualificationDegreeName ";
                //iq = iq.OrderBy(orderByColumnName + " " + orderDir);

                // Filter record count.
                recFilterOut = iq.Count();

                // Apply pagination.
                iq = iq.Skip(startRec).Take(pageSize);
                //Get list of data
                var data = iq.ToList();

                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}