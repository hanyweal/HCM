using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PromotionsRecordsJobsVacanciesDAL : CommonEntityDAL
    {
        public int Insert(PromotionsRecordsJobsVacancies PromotionRecordJobVacancy)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PromotionsRecordsJobsVacancies.Add(PromotionRecordJobVacancy);
                    db.SaveChanges();
                    return PromotionRecordJobVacancy.PromotionRecordJobVacancyID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Insert(List<PromotionsRecordsJobsVacancies> PromotionRecordJobVacancy)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PromotionsRecordsJobsVacancies.AddRange(PromotionRecordJobVacancy);
                    return db.SaveChanges();
                    //return PromotionRecordJobVacancy.PromotionRecordJobVacancyID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int PromotionRecordJobVacancyID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PromotionsRecordsJobsVacancies PromotionRecordJobVacancyObj = db.PromotionsRecordsJobsVacancies.FirstOrDefault(x => x.PromotionRecordJobVacancyID.Equals(PromotionRecordJobVacancyID));
                    db.PromotionsRecordsJobsVacancies.Remove(PromotionRecordJobVacancyObj);
                    return db.SaveChanges(UserIdentity);
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
                    List<PromotionsRecordsJobsVacancies> PromotionRecordJobVacancyObj = db.PromotionsRecordsJobsVacancies.Where(x => x.PromotionRecordID.Equals(PromotionRecordID)).ToList();
                    db.PromotionsRecordsJobsVacancies.RemoveRange(PromotionRecordJobVacancyObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsJobsVacancies> GetPromotionsRecordsJobsVacancies()
        {
            try
            {
                var db = new HCMEntities();
                return db.PromotionsRecordsJobsVacancies
                                .Include("PromotionsRecords")
                                .Include("OrganizationsJobs")
                                .Include("OrganizationsJobs.Ranks")
                                .Include("OrganizationsJobs.Jobs")
                                .Include("OrganizationsJobs.OrganizationsStructures")
                                .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsJobsVacancies> GetByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                var db = new HCMEntities();

                return db.PromotionsRecordsJobsVacancies
                                .Include("PromotionsRecords")
                                .Include("OrganizationsJobs")
                                .Include("OrganizationsJobs.Ranks")
                                .Include("OrganizationsJobs.Jobs")
                                .Include("OrganizationsJobs.OrganizationsStructures")
                                .Where(x => x.PromotionRecordID == PromotionRecordID)
                                .ToList();

            }
            catch
            {
                throw;
            }
        }

        public PromotionsRecordsJobsVacancies GetByPromotionRecordJobVacancyID(int PromotionRecordJobVacancyID)
        {
            try
            {
                var db = new HCMEntities();

                return db.PromotionsRecordsJobsVacancies
                                .Include("PromotionsRecords")
                                .Include("OrganizationsJobs")
                                .Include("OrganizationsJobs.Ranks")
                                .Include("OrganizationsJobs.Jobs")
                                .Include("OrganizationsJobs.OrganizationsStructures")
                                .FirstOrDefault(x => x.PromotionRecordJobVacancyID == PromotionRecordJobVacancyID);
            }
            catch
            {
                throw;
            }
        }

        public PromotionsRecordsJobsVacancies GetByOrganizationJobID(int OrganizationJobID)
        {
            try
            {
                var db = new HCMEntities();

                return db.PromotionsRecordsJobsVacancies
                                .Include("PromotionsRecords")
                                .Include("OrganizationsJobs")
                                .Include("OrganizationsJobs.Ranks")
                                .Include("OrganizationsJobs.Jobs")
                                .Include("OrganizationsJobs.OrganizationsStructures")
                                .FirstOrDefault(x => x.OrganizationJobID == OrganizationJobID);
            }
            catch
            {
                throw;
            }
        }

    }
}