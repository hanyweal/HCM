using HCMDAL;
using System.Collections.Generic;

namespace HCMBLL
{
    public class PromotionsRecordsJobsVacanciesBLL : CommonEntity, IEntity
    {
        public int PromotionRecordJobVacancyID { get; set; }

        public PromotionsRecordsBLL PromotionRecord { get; set; }

        public OrganizationsJobsBLL JobVacancy { get; set; }

        /// <summary>
        /// Using this property in Job Distrubte in Preference Process
        /// </summary>
        public bool IsAssigned { get; set; }

        public List<PromotionsRecordsJobsVacanciesBLL> GetPromotionsRecordsJobsVacancies()
        {
            List<PromotionsRecordsJobsVacancies> PromotionsRecordsJobsVacanciesList = new PromotionsRecordsJobsVacanciesDAL().GetPromotionsRecordsJobsVacancies();
            List<PromotionsRecordsJobsVacanciesBLL> PromotionsRecordsJobsVacanciesBLLList = new List<PromotionsRecordsJobsVacanciesBLL>();
            if (PromotionsRecordsJobsVacanciesList.Count > 0)
                foreach (var item in PromotionsRecordsJobsVacanciesList)
                    PromotionsRecordsJobsVacanciesBLLList.Add(new PromotionsRecordsJobsVacanciesBLL().MapPromotionRecordJobVacancy(item));

            return PromotionsRecordsJobsVacanciesBLLList;
        }

        public int RemoveAllFromPromotionRecord(int PromotionRecordID)
        {
            try
            {
                return new PromotionsRecordsJobsVacanciesDAL().DeleteByPromotionRecordID(PromotionRecordID, LoginIdentity.EmployeeCodeID);
            }
            catch
            {
                throw;
            }
        }

        public List<PromotionsRecordsJobsVacanciesBLL> GetByPromotionRecordID(int PromotionRecordID)
        {
            try
            {
                List<PromotionsRecordsJobsVacancies> PromotionsRecordsJobsVacanciesList = new PromotionsRecordsJobsVacanciesDAL().GetByPromotionRecordID(PromotionRecordID);
                List<PromotionsRecordsJobsVacanciesBLL> PromotionsRecordsJobsVacanciesBLLList = new List<PromotionsRecordsJobsVacanciesBLL>();
                if (PromotionsRecordsJobsVacanciesList.Count > 0)
                    foreach (var item in PromotionsRecordsJobsVacanciesList)
                        PromotionsRecordsJobsVacanciesBLLList.Add(new PromotionsRecordsJobsVacanciesBLL().MapPromotionRecordJobVacancy(item));

                return PromotionsRecordsJobsVacanciesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public PromotionsRecordsJobsVacanciesBLL GetByPromotionRecordJobVacancyID(int PromotionRecordJobVacancyID)
        {
            try
            {
                PromotionsRecordsJobsVacancies PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesDAL().GetByPromotionRecordJobVacancyID(PromotionRecordJobVacancyID);
                PromotionsRecordsJobsVacanciesBLL PromotionRecordJobVacancyBLL = new PromotionsRecordsJobsVacanciesBLL().MapPromotionRecordJobVacancy(PromotionRecordJobVacancy);

                return PromotionRecordJobVacancyBLL;
            }
            catch
            {
                throw;
            }
        }

        public PromotionsRecordsJobsVacanciesBLL GetByOrganizationJobID(int OrganizationJobID)
        {
            try
            {
                PromotionsRecordsJobsVacancies PromotionRecordJobVacancy = new PromotionsRecordsJobsVacanciesDAL().GetByOrganizationJobID(OrganizationJobID);
                PromotionsRecordsJobsVacanciesBLL PromotionRecordJobVacancyBLL = new PromotionsRecordsJobsVacanciesBLL().MapPromotionRecordJobVacancy(PromotionRecordJobVacancy);

                return PromotionRecordJobVacancyBLL;
            }
            catch
            {
                throw;
            }
        }

        internal PromotionsRecordsJobsVacanciesBLL MapPromotionRecordJobVacancy(PromotionsRecordsJobsVacancies PromotionRecordJobVacancy)
        {
            try
            {
                PromotionsRecordsJobsVacanciesBLL PromotionRecordJobVacancyBLL = null;
                if (PromotionRecordJobVacancy != null)
                {
                    PromotionRecordJobVacancyBLL = new PromotionsRecordsJobsVacanciesBLL();
                    PromotionRecordJobVacancyBLL.PromotionRecordJobVacancyID = PromotionRecordJobVacancy.PromotionRecordJobVacancyID;
                    PromotionRecordJobVacancyBLL.PromotionRecord = new PromotionsRecordsBLL().MapPromotionRecord(PromotionRecordJobVacancy.PromotionsRecords);
                    PromotionRecordJobVacancyBLL.JobVacancy = new OrganizationsJobsBLL().MapOrganizationJob(PromotionRecordJobVacancy.OrganizationsJobs);
                }
                return PromotionRecordJobVacancyBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}