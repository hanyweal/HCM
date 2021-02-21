using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class OrganizationsJobsDAL : CommonEntityDAL
    {
        public OrganizationsJobs GetActiveByOrganizationJobID(int OrganizationJobID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsJobs.Include("Ranks")
                                            .Include("Ranks.RanksCategories")
                                            .Include("Jobs")
                                            .Include("OrganizationsStructures")
                                            .Include("OrganizationsJobsStatus")
                                            .Where(c => c.IsActive)
                                            .FirstOrDefault(x => x.OrganizationJobID.Equals(OrganizationJobID));
            }
            catch
            {
                throw;
            }
        }
        public OrganizationsJobs GetByOrganizationJobID(int OrganizationJobID)
        {
            try
            {
                var db = new HCMEntities();
                return db.OrganizationsJobs.Include("Ranks")
                                            .Include("Jobs")
                                            .Include("OrganizationsStructures")
                                            .Include("OrganizationsJobsStatus")
                                            .Include("JobsOperationsTypes")
                                            .FirstOrDefault(x => x.OrganizationJobID.Equals(OrganizationJobID));
            }
            catch
            {
                throw;
            }
        }

        public List<OrganizationsJobs> GetOrganizationsJobs(string JobNo, string OrganizationName, string JobName, string RankName, string JobCode, string JobCategoryName,
                                                                out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                JobNo = JobNo.Replace("null", "");
                OrganizationName = OrganizationName.Replace("null", "");
                JobName = JobName.Replace("null", "");
                RankName = RankName.Replace("null", "");
                JobCode = JobCode.Replace("null", "");
                JobCategoryName = JobCategoryName.Replace("null", "");

                var db = new HCMEntities();
                var odData = db.OrganizationsJobs.Include("Ranks")
                                     .Include("Jobs")
                                     .Include("OrganizationsStructures")
                                     .Include("OrganizationsJobsStatus");

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<OrganizationsJobs> iq = odData.Where(p => p.JobNo.Contains(JobNo) &&
                                                                p.OrganizationsStructures.OrganizationName.Contains(OrganizationName) &&
                                                                p.Jobs.JobName.Contains(JobName) &&
                                                                p.Ranks.RankName.Contains(RankName) &&
                                                                p.Jobs.JobCode.Contains(JobCode) &&
                                                                p.Jobs.JobsCategories.JobCategoryName.Contains(JobCategoryName)
                                                            );

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                                       p.JobNo.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.OrganizationsStructures.OrganizationName.ToLower().Contains(search.ToLower()) ||
                                       p.Jobs.JobName.Contains(search.ToLower()) ||
                                       p.Ranks.RankName.ToLower().Contains(search.ToLower())
                                       );
                }
                // Sorting.
                iq = iq.OrderBy(p => p.OrganizationJobID);
                // Filter record count.
                recFilterOut = iq.Count();

                // Apply pagination.
                iq = iq.Skip(startRec).Take(pageSize);
                //Get list of data
                var data = iq.ToList();

                return data;
            }
            catch
            {
                throw;
            }
        }
        public List<OrganizationsJobs> GetActiveOrganizationsJobs(string JobNo, string OrganizationName, string JobName, string RankName, string JobCode, string JobCategoryName,
                                                                out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                JobNo = JobNo.Replace("null", "");
                OrganizationName = OrganizationName.Replace("null", "");
                JobName = JobName.Replace("null", "");
                RankName = RankName.Replace("null", "");
                JobCode = JobCode.Replace("null", "");
                JobCategoryName = JobCategoryName.Replace("null", "");

                var db = new HCMEntities();
                var odData = db.OrganizationsJobs.Include("Ranks")
                                     .Include("Jobs")
                                     .Include("OrganizationsStructures")
                                     .Include("OrganizationsJobsStatus");

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<OrganizationsJobs> iq = odData.Where(p => p.JobNo.Contains(JobNo) &&
                                                                p.OrganizationsStructures.OrganizationName.Contains(OrganizationName) &&
                                                                p.Jobs.JobName.Contains(JobName) &&
                                                                p.Ranks.RankName.Contains(RankName) &&
                                                                p.Jobs.JobCode.Contains(JobCode) &&
                                                                p.Jobs.JobsCategories.JobCategoryName.Contains(JobCategoryName) &&
                                                                p.IsActive
                                                            );

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                                       p.JobNo.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.OrganizationsStructures.OrganizationName.ToLower().Contains(search.ToLower()) ||
                                       p.Jobs.JobName.Contains(search.ToLower()) ||
                                       p.Ranks.RankName.ToLower().Contains(search.ToLower())
                                       );
                }
                // Sorting.
                iq = iq.OrderBy(p => p.OrganizationJobID);
                // Filter record count.
                recFilterOut = iq.Count();

                // Apply pagination.
                iq = iq.Skip(startRec).Take(pageSize);
                //Get list of data
                var data = iq.ToList();

                return data;
            }
            catch
            {
                throw;
            }
        }

        public List<OrganizationsJobs> GetAllOrganizationsJobs()
        {
            try
            {
                var db = new HCMEntities();
                var odData = db.OrganizationsJobs.Include("Ranks")
                                     .Include("Jobs")
                                     .Include("OrganizationsStructures")
                                     .Include("OrganizationsJobsStatus")
                                     .Where(x => x.IsActive);
                return odData.ToList();
            }
            catch
            {
                throw;
            }
        }

        public int Insert(OrganizationsJobs OrganizationJob)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.OrganizationsJobs.Add(OrganizationJob);
                    return db.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }
        }

        public int Update(OrganizationsJobs OrganizationJob)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OrganizationsJobs OrganizationJobObj = db.OrganizationsJobs.FirstOrDefault(x => x.OrganizationJobID.Equals(OrganizationJob.OrganizationJobID));
                    OrganizationJobObj.JobNo = OrganizationJob.JobNo;
                    OrganizationJobObj.OrganizationID = OrganizationJob.OrganizationID;
                    OrganizationJobObj.JobID = OrganizationJob.JobID;
                    OrganizationJobObj.RankID = OrganizationJob.RankID;
                    OrganizationJobObj.IsVacant = OrganizationJob.IsVacant;
                    OrganizationJobObj.LastUpdatedDate = OrganizationJob.LastUpdatedDate;
                    OrganizationJobObj.OrganizationJobStatusID = OrganizationJob.OrganizationJobStatusID;
                    OrganizationJobObj.LastUpdatedBy = OrganizationJob.LastUpdatedBy;
                    OrganizationJobObj.IsActive = OrganizationJob.IsActive;
                    return db.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int OrganizationJobID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OrganizationsJobs OrganizationJobObj = db.OrganizationsJobs.FirstOrDefault(x => x.OrganizationJobID.Equals(OrganizationJobID));
                    db.OrganizationsJobs.Remove(OrganizationJobObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<OrganizationsJobs> GetActiveJobs(int JobCategoryID, int RankID, bool IsVacant)
        {
            try
            {
                var db = new HCMEntities();
                var query = db.OrganizationsJobs
                                     .Include("Ranks")
                                     .Include("Jobs")
                                     .Include("OrganizationsStructures")
                                     //.Include("Jobs.JobsCategories")
                                     //.Include("Jobs.JobsCategories.JobsGroups")
                                     .Include("Jobs.JobsCategories.JobsGroups.JobsGeneralGroups")
                                     .Include("OrganizationsJobsStatus")
                                     .Where(x => x.IsVacant == IsVacant && x.RankID == RankID && x.Jobs.JobCategoryID == JobCategoryID && x.IsActive);

                return query.ToList();
            }
            catch
            {
                throw;
            }
        }

        public int UpdateIsReserved(OrganizationsJobs OrganizationJob)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OrganizationsJobs OrganizationJobObj = db.OrganizationsJobs.FirstOrDefault(x => x.OrganizationJobID.Equals(OrganizationJob.OrganizationJobID));
                    OrganizationJobObj.IsReserved = OrganizationJob.IsReserved;
                    OrganizationJobObj.LastUpdatedDate = OrganizationJob.LastUpdatedDate;
                    OrganizationJobObj.LastUpdatedBy = OrganizationJob.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateJobStatus(OrganizationsJobs OrganizationJob)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OrganizationsJobs OrganizationJobObj = db.OrganizationsJobs.FirstOrDefault(x => x.OrganizationJobID.Equals(OrganizationJob.OrganizationJobID));
                    //OrganizationJobObj.OrganizationJobStatusID = OrganizationJob.OrganizationJobStatusID;
                    OrganizationJobObj.IsVacant = false;
                    OrganizationJobObj.IsActive = false;
                    OrganizationJobObj.LastUpdatedDate = OrganizationJob.LastUpdatedDate;
                    OrganizationJobObj.LastUpdatedBy = OrganizationJob.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateIsVacant(OrganizationsJobs OrganizationJob)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    OrganizationsJobs OrganizationJobObj = db.OrganizationsJobs.FirstOrDefault(x => x.OrganizationJobID.Equals(OrganizationJob.OrganizationJobID));
                    OrganizationJobObj.IsVacant = OrganizationJob.IsVacant;
                    OrganizationJobObj.LastUpdatedDate = OrganizationJob.LastUpdatedDate;
                    OrganizationJobObj.LastUpdatedBy = OrganizationJob.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        #region Help Function for Business Validation
        //No chance to repeated job no and rank and active.
        public OrganizationsJobs GetIsActiveWithJobNoAndRankID(string JobNo, int RankID)
        {
            try
            {
                var db = new HCMEntities();
                var odData = db.OrganizationsJobs.FirstOrDefault(c => c.JobNo == JobNo && c.RankID == RankID && c.IsActive);
                return odData;
            }
            catch
            {
                throw;
            }
        }
        #endregion
        public List<OrganizationsJobs> GetOrganizationJobHistoryOperationByOrganizationJobID(int OrganizationJobID)
        {
            List<OrganizationsJobs> OrganizationJobHistoryOperation = new List<OrganizationsJobs>();
            OrganizationsJobs organizationsJobs = GetByOrganizationJobID(OrganizationJobID);
            if (organizationsJobs != null)
            {
                OrganizationJobHistoryOperation.AddRange(GetOrganizationJobChildren(new List<OrganizationsJobs>(), OrganizationJobID));
                OrganizationJobHistoryOperation.Add(organizationsJobs);
                OrganizationJobHistoryOperation.AddRange(GetOrganizationJobParent(new List<OrganizationsJobs>(), OrganizationJobID));

            }
            return OrganizationJobHistoryOperation.OrderBy(c => c.OrganizationJobID).ToList();
        }

        private List<OrganizationsJobs> GetOrganizationJobParent(List<OrganizationsJobs> withParentOrganizationsJobs, int OrganizationJobID)
        {

            try
            {
                 
                    OrganizationsJobs SelectedOrganizationJob = GetByOrganizationJobID(OrganizationJobID);
                    if (SelectedOrganizationJob.OrganizationJobParentID != null)
                    {
                        var item = GetByOrganizationJobID((int)SelectedOrganizationJob.OrganizationJobParentID);
                        withParentOrganizationsJobs.Add(item);
                        GetOrganizationJobParent(withParentOrganizationsJobs, item.OrganizationJobID);
                    }
                    return withParentOrganizationsJobs;
                 
            }
            catch
            {
                throw;
            }
        }
        private List<OrganizationsJobs> GetOrganizationJobChildren(List<OrganizationsJobs> withChildrenOrganizationsJobs, int OrganizationJobID)
        {

            try
            {
             
                OrganizationsJobs SelectedOrganizationJob = GetByOrganizationJobID(OrganizationJobID);

                    foreach (var item in new HCMEntities().OrganizationsJobs
                        .Include("Ranks")
                        .Include("Jobs")
                        .Include("OrganizationsStructures")
                        .Include("OrganizationsJobsStatus")
                        .Include("JobsOperationsTypes")
                        .Where(x => x.OrganizationJobParentID == SelectedOrganizationJob.OrganizationJobID))
                    {
                        withChildrenOrganizationsJobs.Add(item);
                        GetOrganizationJobChildren(withChildrenOrganizationsJobs, item.OrganizationJobID);
                    }

                    return withChildrenOrganizationsJobs;
                 
            }
            catch
            {
                throw;
            }
        }


    }
}
