using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class ContractorsBasicSalariesDAL : CommonEntityDAL
    {
        public int Insert(ContractorsBasicSalaries ContractorBasicSalary)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.ContractorsBasicSalaries.Add(ContractorBasicSalary);
                    db.SaveChanges();
                    return ContractorBasicSalary.ContractorBasicSalaryID;
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

        public int Update(ContractorsBasicSalaries ContractorBasicSalary)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    ContractorsBasicSalaries Obj = db.ContractorsBasicSalaries.SingleOrDefault(x => x.ContractorBasicSalaryID.Equals(ContractorBasicSalary.ContractorBasicSalaryID));
                    Obj.EmployeeCodeID = ContractorBasicSalary.EmployeeCodeID;
                    Obj.BasicSalary = ContractorBasicSalary.BasicSalary;
                    Obj.TransfareAllowance = ContractorBasicSalary.TransfareAllowance;
                    Obj.LastUpdatedDate = ContractorBasicSalary.LastUpdatedDate;
                    Obj.LastUpdatedBy = ContractorBasicSalary.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int ContractorBasicSalaryID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    ContractorsBasicSalaries Obj = db.ContractorsBasicSalaries.SingleOrDefault(x => x.ContractorBasicSalaryID.Equals(ContractorBasicSalaryID));
                    db.ContractorsBasicSalaries.Remove(Obj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<ContractorsBasicSalaries> GetContractorsBasicSalaries()
        {   
            try
            {
                var db = new HCMEntities();
                return db.ContractorsBasicSalaries.Include("EmployeesCodes").ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<ContractorsBasicSalaries> GetContractorsBasicSalaries(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();
                var odData = db.ContractorsBasicSalaries
                    .Include("EmployeesCodes.Employees");

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<ContractorsBasicSalaries> iq = odData.Where(p => p.ContractorBasicSalaryID > 0);
                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p => p.ContractorBasicSalaryID.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.EmployeesCodes.EmployeeCodeNo.ToLower().Contains(search.ToLower()) ||
                                    p.EmployeesCodes.Employees.FirstNameAr.ToLower().Contains(search.ToLower()));
                }
                // Sorting.
                //iqOD = this.SortByColumnWithOrder2(order, orderDir, iqOD);
                iq = iq.OrderBy(p => p.ContractorBasicSalaryID);
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

        public ContractorsBasicSalaries GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.ContractorsBasicSalaries.Include("EmployeesCodes").FirstOrDefault(x=>x.EmployeeCodeID == EmployeeCodeID);
            }
            catch
            {
                throw;
            }
        }

        public ContractorsBasicSalaries GetByContractorBasicSalaryID(int ContractorBasicSalaryID)
        {
            try
            {
                var db = new HCMEntities();
                return db.ContractorsBasicSalaries
                    //.Include("EmployeesCodes")
                    .Include("EmployeesCodes.Employees") 
                    .SingleOrDefault(x => x.ContractorBasicSalaryID.Equals(ContractorBasicSalaryID));
            }
            catch
            {
                throw;
            }
        }

    }
}