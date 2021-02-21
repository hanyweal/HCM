using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class EmployeesAllowancesDAL : CommonEntityDAL
    {
        public int Insert(EmployeesAllowances EmployeeAllowance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeesAllowances.Add(EmployeeAllowance);
                    db.SaveChanges();
                    return EmployeeAllowance.EmployeeAllowanceID;
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

        public int Update(EmployeesAllowances EmployeeAllowance)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesAllowances EmployeeAllowanceObj = db.EmployeesAllowances.SingleOrDefault(x => x.EmployeeAllowanceID.Equals(EmployeeAllowance.EmployeeAllowanceID));
                    EmployeeAllowanceObj.EmployeeCareerHistoryID = EmployeeAllowance.EmployeeCareerHistoryID;
                    EmployeeAllowanceObj.AllowanceID = EmployeeAllowance.AllowanceID;
                    EmployeeAllowanceObj.AllowanceStartDate = EmployeeAllowance.AllowanceStartDate;
                    EmployeeAllowanceObj.IsActive = EmployeeAllowance.IsActive;
                    EmployeeAllowanceObj.LastUpdatedDate = EmployeeAllowance.LastUpdatedDate;
                    EmployeeAllowanceObj.LastUpdatedBy = EmployeeAllowance.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int EmployeeAllowanceID,int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesAllowances EmployeeAllowanceObj = db.EmployeesAllowances.SingleOrDefault(x => x.EmployeeAllowanceID.Equals(EmployeeAllowanceID));
                    db.EmployeesAllowances.Remove(EmployeeAllowanceObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesAllowances> GetEmployeesAllowancesByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesAllowances.Include("EmployeesCareersHistory")
                                            .Include("Allowances")
                                            .Where(c => c.EmployeeCareerHistoryID == EmployeeCareerHistoryID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesAllowances> GetActiveEmployeesAllowances()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesAllowances.Include("Allowances")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                            .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                                            .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks")
                                            .Include("CreatedByNav.Employees")
                                            .Include("LastUpdatedByNav.Employees")
                                            .Where(x => x.IsActive).ToList();

                //var db = new HCMEntities();
                //var odData = db.EmployeesAllowances.Include("Allowances")
                //                                   .Include("EmployeesCareersHistory");
                //odData = odData.Include("EmployeesCareersHistory.EmployeesCodes.Employees");
                //odData = odData.Include("CreatedByNav.Employees")
                //               .Include("LastUpdatedByNav.Employees");
                //odData = odData.Include("EmployeesCareersHistory.OrganizationsJobs")
                //                .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                //                .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks");

                //return odData.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesAllowances> GetActiveEmployeesAllowances(string EmployeeCodeNo)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesAllowances.Include("Allowances")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                            .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                                            .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks")
                                            .Where(x => x.IsActive 
                                                     && x.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo == EmployeeCodeNo).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesAllowances> GetEmployeesAllowances(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();

                var odData = db.EmployeesAllowances
                                            .Include("Allowances")
                                       .Include("EmployeesCareersHistory");

                odData = odData.Include("EmployeesCareersHistory.EmployeesCodes.Employees");


                odData = odData.Include("CreatedByNav.Employees")
                             .Include("LastUpdatedByNav.Employees");

                odData = odData.Include("EmployeesCareersHistory.OrganizationsJobs")
                             .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                             .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks");
                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<EmployeesAllowances> iq = odData.Where(p => p.EmployeeAllowanceID != null);
                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                                       p.EmployeeAllowanceID.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo.ToLower().Contains(search.ToLower()) ||
                                       (p.EmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " + p.EmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr + " " + p.EmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr + " " + p.EmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr).ToLower().Contains(search.ToLower()) ||
                                       p.Allowances.AllowanceName.ToLower().Contains(search.ToLower()) ||
                                       p.EmployeesCareersHistory.OrganizationsJobs.Jobs.JobName.ToLower().Contains(search.ToLower()) ||
                                       p.AllowanceStartDate.ToString().ToLower().Contains(search.ToLower())
                                       );
                }
                // Sorting.
                //iqOD = this.SortByColumnWithOrder2(order, orderDir, iqOD);
                iq = iq.OrderBy(p => p.EmployeeAllowanceID);
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

        public EmployeesAllowances GetEmployeesAllowancesByAllowanceID(int EmployeeAllowanceID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesAllowances.Include("EmployeesCareersHistory")
                                            .Include("Allowances")
                                            .Include("Allowances.AllowancesCalculationTypes")
                                            .FirstOrDefault(x => x.EmployeeAllowanceID.Equals(EmployeeAllowanceID));
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesAllowances> GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesAllowances.Include("EmployeesCareersHistory")
                                                 .Include("Allowances")
                                                 .Include("Allowances.AllowancesCalculationTypes")
                                                 .Where(x => x.EmployeesCareersHistory.EmployeeCodeID == EmployeeCodeID).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
