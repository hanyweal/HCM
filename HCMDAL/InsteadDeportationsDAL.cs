using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class InsteadDeportationsDAL : CommonEntityDAL
    {
        public int Insert(InsteadDeportations InsteadDeportation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.InsteadDeportations.Add(InsteadDeportation);
                    db.SaveChanges();
                    return InsteadDeportation.InsteadDeportationID;
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

        public int Update(InsteadDeportations InsteadDeportation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    InsteadDeportations InsteadDeportationObj = db.InsteadDeportations.SingleOrDefault(x => x.InsteadDeportationID.Equals(InsteadDeportation.InsteadDeportationID));
                    InsteadDeportationObj.EmployeeCareerHistoryID = InsteadDeportation.EmployeeCareerHistoryID;
                    InsteadDeportationObj.DeportationDate = InsteadDeportation.DeportationDate;
                    InsteadDeportationObj.Amount = InsteadDeportation.Amount;
                    InsteadDeportationObj.Note = InsteadDeportation.Note;
                    InsteadDeportationObj.LastUpdatedDate = InsteadDeportation.LastUpdatedDate;
                    InsteadDeportationObj.LastUpdatedBy = InsteadDeportation.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int InsteadDeportationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    InsteadDeportations InsteadDeportationObj = db.InsteadDeportations.SingleOrDefault(x => x.InsteadDeportationID.Equals(InsteadDeportationID));
                    db.InsteadDeportations.Remove(InsteadDeportationObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<InsteadDeportations> GetInsteadDeportationsByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.InsteadDeportations.Include("EmployeesCareersHistory")
                                            .Include("CreatedByNav.Employees")
                                            .Include("LastUpdatedByNav.Employees")
                                            .Where(c => c.EmployeeCareerHistoryID == EmployeeCareerHistoryID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<InsteadDeportations> GetInsteadDeportations()
        {
            try
            {
                var db = new HCMEntities();


                var odData = db.InsteadDeportations
                                       .Include("EmployeesCareersHistory");

                odData = odData.Include("EmployeesCareersHistory.EmployeesCodes.Employees");


                odData = odData.Include("CreatedByNav.Employees")
                             .Include("LastUpdatedByNav.Employees");


                return odData.ToList();
            }
            catch
            {
                throw;
            }
        }


        public List<InsteadDeportations> GetInsteadDeportations(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();
                //return db.Delegations.Include("DelegationsKinds")
                //                     .Include("DelegationsTypes")
                //                     .Include("KSACities")
                //                     .Include("KSACities.KSARegions")
                //                     .Include("Countries")
                //                     .Include("CreatedByNav.Employees")
                //                     .Include("LastUpdatedByNav.Employees")
                //                     .ToList();


                //var odData = db.InsteadDeportations
                //                        .Include("Deportations")
                //                        .Include("EmployeesCareersHistory");

                var odData = db.InsteadDeportations
                                       .Include("EmployeesCareersHistory");

                odData = odData.Include("EmployeesCareersHistory.EmployeesCodes.Employees");


                odData = odData.Include("CreatedByNav.Employees")
                             .Include("LastUpdatedByNav.Employees");

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<InsteadDeportations> iq = odData.Where(p => p.InsteadDeportationID != null);
                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                                       p.InsteadDeportationID.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo.ToLower().Contains(search.ToLower()) ||
                                       (p.EmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " + p.EmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " + p.EmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " + p.EmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr).ToLower().Contains(search.ToLower()) ||
                                        p.EmployeesCareersHistory.OrganizationsJobs.Jobs.JobName.ToLower().Contains(search.ToLower()) ||
                                       p.DeportationDate.ToString().ToLower().Contains(search.ToLower())
                                       );
                }
                // Sorting.
                //iqOD = this.SortByColumnWithOrder2(order, orderDir, iqOD);
                iq = iq.OrderBy(p => p.InsteadDeportationID);
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



        public List<InsteadDeportations> GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.InsteadDeportations.Include("EmployeesCareersHistory")
                                            .Include("CreatedByNav.Employees")
                                            .Include("LastUpdatedByNav.Employees")
                                                 .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID).ToList();
            }
            catch
            {
                throw;
            }
        }
        public InsteadDeportations GetInsteadDeportationByInsteadDeportationID(int InsteadDeportationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.InsteadDeportations.Include("EmployeesCareersHistory")
                                            .Include("CreatedByNav.Employees")
                                            .Include("LastUpdatedByNav.Employees")
                                            .FirstOrDefault(x => x.InsteadDeportationID.Equals(InsteadDeportationID));

            }
            catch
            {
                throw;
            }
        }
    }
}
