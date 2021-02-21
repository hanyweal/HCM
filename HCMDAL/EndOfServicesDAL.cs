using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class EndOfServicesDAL : CommonEntityDAL
    {
        public int Insert(EndOfServices EndOfService)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EndOfServices.Add(EndOfService);
                    db.SaveChanges();
                    return EndOfService.EndOfServiceID;
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

        public int Update(EndOfServices EndOfService)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EndOfServices EndOfServiceObj = db.EndOfServices.SingleOrDefault(x => x.EndOfServiceID.Equals(EndOfService.EndOfServiceID));
                    EndOfServiceObj.EmployeeCareerHistoryID = EndOfService.EmployeeCareerHistoryID;
                    EndOfServiceObj.EndOfServiceDate = EndOfService.EndOfServiceDate;
                    EndOfServiceObj.EndOfServiceDecisionNo = EndOfService.EndOfServiceDecisionNo;
                    EndOfServiceObj.EndOfServiceDecisionDate = EndOfService.EndOfServiceDecisionDate;
                    EndOfServiceObj.EndOfServiceReasonID = EndOfService.EndOfServiceReasonID;
                    EndOfServiceObj.LastUpdatedDate = DateTime.Now;
                    EndOfServiceObj.LastUpdatedBy = EndOfService.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int EndOfServiceID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EndOfServices EndOfServiceObj = db.EndOfServices.SingleOrDefault(x => x.EndOfServiceID.Equals(EndOfServiceID));
                    db.EndOfServices.Remove(EndOfServiceObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<EndOfServices> GetEndOfServices()
        {
            try
            {
                var db = new HCMEntities();
                return db.EndOfServices
                            .Include("EmployeesCareersHistory")
                            .Include("EndOfServicesReasons")
                            .Include("EndOfServicesReasons.EndOfServicesCases")
                            .Include("CreatedByNav")
                            .Include("CreatedByNav.Employees")
                            .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EndOfServices> GetEndOfServicesNotProccessed()
        {
            try
            {
                var db = new HCMEntities();
                return db.EndOfServices
                            .Include("EmployeesCareersHistory")
                            .Include("EndOfServicesReasons")
                            .Include("EndOfServicesReasons.EndOfServicesCases")
                            .Include("CreatedByNav")
                            .Include("CreatedByNav.Employees")
                            .Where(x => (x.isProcessed.HasValue ? x.isProcessed.Value : false) == false)
                            .ToList();
            }
            catch
            {
                throw;
            }
        }

        public int UpdateIsProcess(EndOfServices EOS)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EndOfServices Obj = db.EndOfServices.FirstOrDefault(x => x.EndOfServiceID.Equals(EOS.EndOfServiceID));
                    Obj.isProcessed = EOS.isProcessed;
                    Obj.LastUpdatedDate = EOS.LastUpdatedDate;
                    Obj.LastUpdatedBy = EOS.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EndOfServices> GetEndOfServices(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();
                var odData = db.EndOfServices
                               .Include("EmployeesCareersHistory")
                               .Include("EndOfServicesReasons")
                               .Include("EndOfServicesReasons.EndOfServicesCases");

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<EndOfServices> iq = odData.Where(p => p.EndOfServiceID != null);

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p => p.EndOfServiceDecisionNo.ToLower().Contains(search.ToLower()) ||
                                           (p.EmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " + p.EmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " + p.EmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " + p.EmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr).ToLower().Contains(search.ToLower()) ||
                                           p.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo.ToLower().Contains(search.ToLower()) ||
                                           p.EndOfServicesReasons.EndOfServiceReason.ToLower().Contains(search.ToLower()) ||
                                           p.EndOfServicesReasons.EndOfServicesCases.EndOfServiceCase.ToLower().Contains(search.ToLower()));
                }
                // Sorting.
                //iqOD = this.SortByColumnWithOrder2(order, orderDir, iqOD);
                iq = iq.OrderBy(p => p.EndOfServiceID);
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

        public List<EndOfServices> GetEndOfServicesByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EndOfServices.Where(c => c.EmployeeCareerHistoryID == EmployeeCareerHistoryID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
