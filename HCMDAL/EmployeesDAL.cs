using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class EmployeesDAL
    {
        public int Insert(Employees Employee)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Employees.Add(Employee);
                    db.SaveChanges();
                    return Employee.EmployeeID;
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
      
        public int Delete(Employees Employee, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Employees EmployeeObj = db.Employees.FirstOrDefault(x => x.EmployeeID.Equals(Employee.EmployeeID));
                    db.Employees.Remove(EmployeeObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int Update(Employees Employee)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Employees EmployeeObj = db.Employees.FirstOrDefault(x => x.EmployeeID.Equals(Employee.EmployeeID));
                    EmployeeObj.EmployeeBirthDate = Employee.EmployeeBirthDate;
                    EmployeeObj.EmployeeBirthPlace = Employee.EmployeeBirthPlace;
                    EmployeeObj.EmployeeEMail = Employee.EmployeeEMail;
                    EmployeeObj.EmployeeIDIssueDate = Employee.EmployeeIDIssueDate;
                    EmployeeObj.EmployeeIDNo = Employee.EmployeeIDNo;
                    EmployeeObj.EmployeePassportEndDate = Employee.EmployeePassportEndDate;
                    EmployeeObj.EmployeePassportIssueDate = Employee.EmployeePassportIssueDate;
                    EmployeeObj.EmployeePassportNo = Employee.EmployeePassportNo;
                    EmployeeObj.EmployeePassportSource = Employee.EmployeePassportSource;

                    EmployeeObj.EmployeeIDExpiryDate = Employee.EmployeeIDExpiryDate;
                    EmployeeObj.EmployeeIDCopyNo = Employee.EmployeeIDCopyNo;
                    EmployeeObj.EmployeeIDIssuePlace = Employee.EmployeeIDIssuePlace;
                    EmployeeObj.DependentCount = Employee.DependentCount;
                    EmployeeObj.MaritalStatusID = Employee.MaritalStatusID;
                    EmployeeObj.GenderID = Employee.GenderID;
                    EmployeeObj.NationalityID = Employee.NationalityID;

                    EmployeeObj.LastUpdatedDate = DateTime.Now;
                    EmployeeObj.LastUpdatedBy = Employee.LastUpdatedBy;
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

        public List<Employees> GetEmployees()
        {
            try
            {
                var db = new HCMEntities();
                return db.Employees.ToList();
            }
            catch
            {
                throw;
            }
        }

        public Employees GetByEmployeeID(int EmployeeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Employees.FirstOrDefault(x => x.EmployeeID.Equals(EmployeeID));
            }
            catch
            {
                throw;
            }
        }

        public Employees GetByEmployeeIDNo(string EmployeeIDNo)
        {
            try
            {
                var db = new HCMEntities();
                return db.Employees.FirstOrDefault(x => x.EmployeeIDNo.Equals(EmployeeIDNo));
            }
            catch
            {
                throw;
            }
        }

        public Employees GetByEmployeeNameAr(string EmployeeNameAr)
        {
            try
            {
                var db = new HCMEntities();
                //var query = db.Employees;
                //return query.SingleOrDefault(x => x.EmployeeNameAr.Contains(EmployeeNameAr));
                return db.Employees.FirstOrDefault(x => x.FirstNameAr + " " + x.MiddleNameAr + " " + x.GrandFatherNameAr + " " + x.FifthNameAr + " " + x.LastNameAr == EmployeeNameAr);
            }
            catch
            {
                throw;
            }
        }

        public int UpdateMobileNo(Employees Employee)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Employees EmployeeObj = db.Employees.FirstOrDefault(x => x.EmployeeID.Equals(Employee.EmployeeID));
                    EmployeeObj.EmployeeMobileNo = Employee.EmployeeMobileNo;
                    EmployeeObj.LastUpdatedDate = DateTime.Now;
                    EmployeeObj.LastUpdatedBy = Employee.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateEmployeeName(Employees Employee)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Employees EmployeeObj = db.Employees.FirstOrDefault(x => x.EmployeeID.Equals(Employee.EmployeeID));
                    EmployeeObj.FirstNameAr = Employee.FirstNameAr;
                    EmployeeObj.MiddleNameAr = Employee.MiddleNameAr;
                    EmployeeObj.GrandFatherNameAr = Employee.GrandFatherNameAr;
                    EmployeeObj.FifthNameAr = Employee.FifthNameAr;
                    EmployeeObj.LastNameAr = Employee.LastNameAr;
                    EmployeeObj.FirstNameEn = Employee.FirstNameEn;
                    EmployeeObj.MiddleNameEn = Employee.MiddleNameEn;
                    EmployeeObj.GrandFatherNameEn = Employee.GrandFatherNameEn;
                    EmployeeObj.FifthNameEn = Employee.FifthNameEn;
                    EmployeeObj.LastNameEn = Employee.LastNameEn;
                    EmployeeObj.LastUpdatedDate = DateTime.Now;
                    EmployeeObj.LastUpdatedBy = Employee.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
