using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class EVacationsRequestsDAL
    {
        public int Insert(EVacationsRequests EVacationRequest)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EVacationsRequests.Add(EVacationRequest);
                    db.SaveChanges();
                    return EVacationRequest.EVacationRequestID;
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(EVacationsRequests EVacationRequest)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EVacationsRequests EVacationRequestObj = db.EVacationsRequests.FirstOrDefault(x => x.EVacationRequestID.Equals(EVacationRequest.EVacationRequestID));
                    EVacationRequestObj.EVacationRequestStatusID = EVacationRequest.EVacationRequestStatusID;
                    EVacationRequestObj.ApprovedBy = EVacationRequest.ApprovedBy;
                    EVacationRequestObj.ApprovalDateTime = EVacationRequest.ApprovalDateTime;
                    EVacationRequestObj.ApproverNotes = EVacationRequest.ApproverNotes;
                    EVacationRequestObj.LastUpdatedDate = DateTime.Now;
                    EVacationRequestObj.LastUpdatedBy = EVacationRequest.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateStatus(EVacationsRequests EVacationRequest)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EVacationsRequests EVacationRequestObj = db.EVacationsRequests.FirstOrDefault(x => x.EVacationRequestID.Equals(EVacationRequest.EVacationRequestID));
                    EVacationRequestObj.EVacationRequestStatusID = EVacationRequest.EVacationRequestStatusID;
                    EVacationRequestObj.CancellationReasonByHR = EVacationRequest.CancellationReasonByHR;
                    EVacationRequestObj.LastUpdatedDate = EVacationRequest.LastUpdatedDate;
                    EVacationRequestObj.LastUpdatedBy = EVacationRequest.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int EVacationRequestID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EVacationsRequests EVacationRequestObj = db.EVacationsRequests.FirstOrDefault(x => x.EVacationRequestID.Equals(EVacationRequestID));
                    db.EVacationsRequests.Remove(EVacationRequestObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int GetMaxEVacationRequestNoByYear(int Year)
        {
            try
            {
                var db = new HCMEntities();
                var EVacationRequetsList = db.EVacationsRequests.Where(x => x.CreatedDate.Year == Year).ToList();
                return EVacationRequetsList.Count() > 0 ? EVacationRequetsList.Max(x => x.EVacationRequestNo) : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EVacationsRequests GetByEVacationRequestID(int EVacationRequestID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EVacationsRequests.Include("EmployeesCareersHistory.EmployeesCodes")
                                            .Include("EVacationRequestsStatus")
                                            .Include("VacationsTypes")
                                            .Include("CreatedByNav")
                                            .Include("LastUpdatedByNav")
                                            .Include("ApprovedByNav")
                                            .FirstOrDefault(x => x.EVacationRequestID == EVacationRequestID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EVacationsRequests> Get()
        {
            try
            {
                var db = new HCMEntities();
                return db.EVacationsRequests.Include("EmployeesCareersHistory.EmployeesCodes")
                                            .Include("EVacationRequestsStatus")
                                            .Include("VacationsTypes")
                                            .Include("CreatedByNav")
                                            .Include("LastUpdatedByNav")
                                            .Include("ApprovedByNav")
                                            //.Where(x => x.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo == EmployeeCodeNo)
                                            .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EVacationsRequests> GetByEmployeeCodeNo(string EmployeeCodeNo)
        {
            try
            {
                var db = new HCMEntities();
                return db.EVacationsRequests.Include("EmployeesCareersHistory.EmployeesCodes")
                                            .Include("EVacationRequestsStatus")
                                            .Include("VacationsTypes")
                                            .Include("CreatedByNav")
                                            .Include("LastUpdatedByNav")
                                            .Include("ApprovedByNav")
                                            .Where(x => x.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo == EmployeeCodeNo).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EVacationsRequests> GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EVacationsRequests.Include("EmployeesCareersHistory")
                                            .Include("EVacationRequestsStatus")
                                            .Include("CreatedByNav")
                                            .Include("LastUpdatedByNav")
                                            .Include("ApprovedByNav")
                                            .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EVacationsRequests> GetByEmployeeCodeID(int EmployeeCodeID, int EVacationRequestStatusID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EVacationsRequests.Include("EmployeesCareersHistory")
                                            .Include("EVacationRequestsStatus")
                                            .Include("CreatedByNav")
                                            .Include("LastUpdatedByNav")
                                            .Include("ApprovedByNav")
                                            .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID && x.EVacationRequestStatusID == EVacationRequestStatusID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EVacationsRequests> GetByApproverCodeNo(string EmployeeCodeNo)
        {
            try
            {
                var db = new HCMEntities();
                return db.EVacationsRequests.Include("EmployeesCareersHistory.EmployeesCodes")
                                            .Include("EVacationRequestsStatus")
                                            .Include("VacationsTypes")
                                            .Include("CreatedByNav")
                                            .Include("LastUpdatedByNav")
                                            .Include("ApprovedByNav")
                                            .Where(x => x.ApprovedByNav.EmployeeCodeNo == EmployeeCodeNo).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EVacationsRequests> GetEVacationsRequestsByOrganizations(List<int> OrganizationsIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.EVacationsRequests.Include("EmployeesCareersHistory.EmployeesCodes")
                                            .Include("EVacationRequestsStatus")
                                            .Include("Jobs")
                                            .Include("OrganizationsStructures")
                                            .Include("VacationsTypes")
                                            .Include("CreatedByNav")
                                            .Include("LastUpdatedByNav")
                                            .Include("ApprovedByNav")
                                            .Where(x => OrganizationsIDs.Contains(x.ActualOrganizationID.Value)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
