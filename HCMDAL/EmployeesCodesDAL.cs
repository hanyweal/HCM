using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class EmployeesCodesDAL : CommonEntityDAL
    {

        public List<EmployeesCodes> GetEmployeesCodes(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();

                var odData = db.EmployeesCodes.Include("Employees").Where(x => x.IsActive.Equals(true));

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<EmployeesCodes> iq = odData.Where(p => p.EmployeeID != null);

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                                       p.Employees.EmployeeIDNo.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.EmployeeCodeNo.ToString().ToLower().Contains(search.ToLower()) ||
                                       (p.Employees.FirstNameAr.ToLower() + " " + p.Employees.MiddleNameAr + " " + p.Employees.LastNameAr + " " + p.Employees.GrandFatherNameAr).Contains(search.ToLower())
                                       );
                }

                // Sorting.
                iq = iq.OrderBy(p => p.Employees.FirstNameAr);

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
            //try
            //{
            //    var db = new HCMEntities();
            //    return db.EmployeesCodes.Include("Employees").Where(x => x.IsActive.Equals(true)).ToList();
            //}
            //catch
            //{
            //    throw;
            //}
        }

        public EmployeesCodes GetByEmployeeCodeNo(EmployeesCodes EmployeeCode)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCodes.Include("Employees")
                                        //.Include("EmployeesCareersHistory.OrganizationsJobs")
                                        .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                                        .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks")
                                        .Include("EmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                        .FirstOrDefault(x => x.EmployeeCodeNo.Equals(EmployeeCode.EmployeeCodeNo));

            }
            catch
            {
                throw;
            }
        }

        public EmployeesCodes GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCodes.Include("Employees")
                                        //.Include("EmployeesCareersHistory.OrganizationsJobs")
                                        .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                                        .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks.RanksCategories")
                                        .Include("EmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                        .FirstOrDefault(x => x.EmployeeCodeID.Equals(EmployeeCodeID)
                                            //  && x.EmployeesCareersHistory.SingleOrDefault(c=> c.IsActive.Equals(true))
                                            );
            }
            catch
            {
                throw;
            }
        }

        public EmployeesCodes GetIsActiveByEmployeeID(int EmployeeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCodes.Include("Employees")
                                        .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                                        .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks")
                                        .Include("EmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                                        .Include("EmployeesTypes")
                                        .FirstOrDefault(x => (x.EmployeeID.Equals(EmployeeID) && (x.IsActive == true)));
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCodes> GetBasedOnAssigningsByOrganizationID(int OrganizationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Assignings
                            .Include("EmployeesCareersHistory").Include("EmployeesCareersHistory.EmployeesCodes")
                            .Where(x => x.OrganizationID == OrganizationID && x.IsFinished == false)
                            .Select(x => x.EmployeesCareersHistory.EmployeesCodes)
                            .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCodes> GetByOrganizationID(int OrganizationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCareersHistory
                            .Include("EmployeesCareersHistory.EmployeesCodes").Include("OrganizationsJobs")
                            .Where(x => x.OrganizationsJobs.OrganizationID == OrganizationID && x.IsActive)
                            .Select(x => x.EmployeesCodes)
                            .ToList();
            }
            catch
            {
                throw;
            }
        }

        public int Insert(EmployeesCodes EmployeesCode)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeesCodes.Add(EmployeesCode);
                    db.SaveChanges();
                    return EmployeesCode.EmployeeCodeID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Update(EmployeesCodes EmployeesCode)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesCodes EmployeesCodeObj = db.EmployeesCodes.FirstOrDefault(x => x.EmployeeCodeID.Equals(EmployeesCode.EmployeeCodeID));
                    EmployeesCodeObj.EmployeeID = EmployeesCode.EmployeeID;
                    EmployeesCodeObj.EmployeeCodeNo = EmployeesCode.EmployeeCodeNo;
                    EmployeesCodeObj.EmployeeTypeID = EmployeesCode.EmployeeTypeID;
                    EmployeesCodeObj.IsActive = EmployeesCode.IsActive;
                    EmployeesCodeObj.LastUpdatedDate = EmployeesCode.LastUpdatedDate;
                    EmployeesCodeObj.LastUpdatedBy = EmployeesCode.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateIsActive(EmployeesCodes EmployeesCode)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesCodes EmployeesCodeObj = db.EmployeesCodes.FirstOrDefault(x => x.EmployeeCodeID.Equals(EmployeesCode.EmployeeCodeID));
                    EmployeesCodeObj.IsActive = EmployeesCode.IsActive;
                    EmployeesCodeObj.LastUpdatedDate = EmployeesCode.LastUpdatedDate;
                    EmployeesCodeObj.LastUpdatedBy = EmployeesCode.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCodes> GetEmployees()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCodes
                            .Include("Employees")
                            .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCodes> GetEmployeesWithDetails()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCodes
                            .Include("Employees")
                            .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                            //.Include("EmployeesCareersHistory.OrganizationsJobs.Ranks")
                            .Include("EmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                            .Include("EmployeesTypes")
                            .Where(x => x.IsActive == true)
                            .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCodes> GetAllEmployeesWithDetails()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCodes
                            .Include("Employees")
                            .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                            //.Include("EmployeesCareersHistory.OrganizationsJobs.Ranks")
                            .Include("EmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                            .Include("EmployeesTypes")
                            .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCodes> GetEmployeesCodesWithMoreDetails()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesCodes
                            .Include("OrganizationsStructures")
                            .Include("Employees.Genders")
                            .Include("Employees.Nationalities")
                            .Include("EmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures")
                            .Include("EmployeesCareersHistory.Assignings.Jobs")
                            .Include("EmployeesCareersHistory.Assignings.OrganizationsStructures")
                            .Include("EmployeesQualifications.Qualifications")
                            .Include("EmployeesQualifications.QualificationsDegrees")
                            .Include("EmployeesQualifications.GeneralSpecializations")
                            .Where(c => c.IsActive == true)
                            .ToList();
            }
            catch
            {
                throw;
            }
        }

    }
}
