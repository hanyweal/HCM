using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class AssigningsDAL : CommonEntityDAL
    {
        public int Insert(Assignings Assigning)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Assignings.Add(Assigning);
                    db.SaveChanges();
                    return Assigning.AssigningID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(Assignings assigning)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Assignings assigningObj = db.Assignings.FirstOrDefault(x => x.AssigningID.Equals(assigning.AssigningID));
                    assigningObj.AssigningTypID = assigning.AssigningTypID;
                    assigningObj.AssigningStartDate = assigning.AssigningStartDate;

                    //assigningObj.AssigningEndDate = assigning.AssigningEndDate;
                    //assigningObj.IsFinished = assigning.IsFinished;
                    //assigningObj.EmployeeCareerHistoryID = assigning.EmployeeCareerHistoryID;
                  
                    assigningObj.ExternalOrganization = assigning.ExternalOrganization;
                    assigningObj.ExternalKSACityID = assigning.ExternalKSACityID;
                    assigningObj.JobID = assigning.JobID;
                    assigningObj.OrganizationID = assigning.OrganizationID;
                    assigningObj.ManagerCodeID = assigning.ManagerCodeID;
                    assigningObj.AssigningReasonID = assigning.AssigningReasonID;
                    assigningObj.AssigningReasonOther = assigning.AssigningReasonOther;
                    assigningObj.LastUpdatedDate = assigning.LastUpdatedDate;
                    assigningObj.LastUpdatedBy = assigning.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(int AssigningID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Assignings AssigningObj = db.Assignings.FirstOrDefault(x => x.AssigningID.Equals(AssigningID));
                    db.Assignings.Remove(AssigningObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateOrganizationManager(Assignings assigning)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    List<Assignings> assigningObj = db.Assignings.Where(x => x.OrganizationID == assigning.OrganizationID && x.IsFinished == false).ToList();
                    assigningObj.ForEach(a =>
                                              {
                                                  a.ManagerCodeID = assigning.ManagerCodeID;
                                                  a.LastUpdatedBy = assigning.LastUpdatedBy;
                                                  a.LastUpdatedDate = assigning.LastUpdatedDate;
                                              }
                                        );

                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int BreakAssigning(Assignings assigning)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Assignings assigningObj = db.Assignings.FirstOrDefault(x => x.AssigningID.Equals(assigning.AssigningID));
                    assigningObj.AssigningEndDate = assigning.AssigningEndDate;
                    assigningObj.IsFinished = assigning.IsFinished;
                    assigningObj.EndAssigningReasonID = assigning.EndAssigningReasonID;
                    assigningObj.Notes = assigning.Notes;
                    assigningObj.LastUpdatedDate = assigning.LastUpdatedDate;
                    assigningObj.LastUpdatedBy = assigning.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Assignings> GetAssignings(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();
                var odData = db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("Jobs")
                                    .Include("OrganizationsStructures")
                                    .Include("CreatedByNav.Employees")
                                    .Include("LastUpdatedByNav.Employees")
                                    .Include("KSACities");

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<Assignings> iq = odData.Where(p => p.AssigningID > 0);

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                                       p.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.EmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr.ToLower().Contains(search.ToLower()) ||
                                       p.EmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr.ToLower().Contains(search.ToLower()) ||
                                       p.EmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr.ToLower().Contains(search.ToLower()) ||
                                       p.EmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr.ToLower().Contains(search.ToLower()) || 
                                       p.AssigningStartDate.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.AssigningEndDate.ToString().ToLower().Contains(search.ToLower())
                                       );
                }
                // Sorting.
                //iqOD = this.SortByColumnWithOrder2(order, orderDir, iqOD);
                iq = iq.OrderByDescending(p => p.AssigningStartDate);
                
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

        public List<Assignings> GetAssignings()
        {
            try
            {
                var db = new HCMEntities();
                return db.Assignings.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Assignings> GetAssigningsWillExpire(DateTime FromDate, int MonthPeriod)
        {
            try
            {
                var ToDate = DateTime.Now.AddMonths(MonthPeriod);
                var db = new HCMEntities();

                return db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("Jobs")
                                    .Include("KSACities")
                                    .Include("OrganizationsStructures")
                                    .Include("CreatedByNav.Employees")
                                    .Include("LastUpdatedByNav.Employees")
                                    .Where(x => x.AssigningEndDate > FromDate && x.AssigningEndDate <= ToDate)
                                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Assignings> GetAssigningsAlreadyExpiredNotFinished()
        {
            try
            {
                var db = new HCMEntities();

                return db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("Jobs")
                                    .Include("OrganizationsStructures")
                                    .Include("ManagerCode.Employees")
                                    .Include("CreatedByNav.Employees")
                                    .Include("LastUpdatedByNav.Employees")
                                    .Include("KSACities")
                                    .Where(x => x.AssigningEndDate <= DateTime.Now 
                                                && x.IsFinished == false)
                                    .ToList();

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Assignings GetByAssigningID(int AssigningID)
        {
            try
            {
                var db = new HCMEntities();

                return db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("Jobs")
                                    .Include("OrganizationsStructures")
                                    .Include("ManagerCode.Employees")
                                    .Include("CreatedByNav.Employees")
                                    .Include("LastUpdatedByNav.Employees")
                                    .Include("KSACities")
                                    .Include("KSACities.KSARegions")
                                    .FirstOrDefault(x => x.AssigningID.Equals(AssigningID));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Assignings> GetAssigningsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();

                return db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("Jobs")
                                    .Include("OrganizationsStructures")
                                    .Include("ManagerCode.Employees")
                                    .Include("KSACities")
                                    .Include("EndAssigningsReasonsNav")
                                    .Where(x => x.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeID == EmployeeCodeID).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Assignings> GetInternalAssigningsByManagerCodeIDOrganizationID(int ManagerCodeID, int OrganizationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("Jobs")
                                    .Include("OrganizationsStructures")
                                    .Include("ManagerCode.Employees")
                                    .Include("KSACities")
                                    .Where(x => x.ManagerCodeID == ManagerCodeID 
                                                && x.OrganizationID == OrganizationID 
                                                && x.IsFinished == false
                                                && x.AssigningTypID == 1).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Assignings GetActiveAssigningByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("Jobs")
                                    .Include("OrganizationsStructures")
                                    .Include("ManagerCode.Employees")
                                    .Include("KSACities")
                                    .FirstOrDefault(x => x.EmployeeCareerHistoryID == EmployeeCareerHistoryID 
                                                        && x.IsFinished == false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Assignings GetActiveAssigningByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("Jobs")
                                    .Include("OrganizationsStructures")
                                    .Include("ManagerCode.Employees")
                                    .Include("KSACities")
                                    .FirstOrDefault(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID
                                                        && x.IsFinished == false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Assignings GetActiveAssigningByEmployeeCodeNo(string EmployeeCodeNo)
        {
            try
            {
                var db = new HCMEntities();
                return db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("Jobs")
                                    .Include("OrganizationsStructures")
                                    .Include("ManagerCode.Employees")
                                    .Include("KSACities")
                                    .FirstOrDefault(x => x.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo == EmployeeCodeNo 
                                                        && x.IsFinished == false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<vwActualEmployeesBasedOnAssignings> GetActualEmployeeBasedOnAssignings()
        {
            try
            {
                var db = new HCMEntities();
                return db.vwActualEmployeesBasedOnAssignings.Where(x=> x.IsFinished == false).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Assignings> GetActiveInternalAssigningsByOrganizationID(int OrganizationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                    .Include("Jobs")
                                    .Include("OrganizationsStructures")
                                    .Include("ManagerCode.Employees")
                                    .Include("KSACities")
                                    .Where(x => x.OrganizationID == OrganizationID
                                            && x.IsFinished == false
                                            && x.AssigningTypID == 1).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Assignings> GetByOrganizationIDs(List<int> OrganizationIDs, int AssigningTypID, bool IsFinished)
        {
            try
            {
                var db = new HCMEntities();
                return db.Assignings.Include("AssigningsTypes")
                                    .Include("EmployeesCareersHistory.EmployeesCodes.Employees")                                    
                                    .Where(x => x.AssigningTypID == AssigningTypID
                                            && x.IsFinished == IsFinished
                                            && OrganizationIDs.Contains(x.OrganizationID.Value)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}