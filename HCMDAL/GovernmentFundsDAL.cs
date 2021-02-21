using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class GovernmentFundsDAL : CommonEntityDAL
    {
        public int Insert(GovernmentFunds GovernmentFund)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.GovernmentFunds.Add(GovernmentFund);
                    db.SaveChanges();
                    return GovernmentFund.GovernmentFundID;
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

        public int Update(GovernmentFunds GovernmentFund)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    GovernmentFunds GovernmentFundObj = db.GovernmentFunds.SingleOrDefault(x => x.GovernmentFundID.Equals(GovernmentFund.GovernmentFundID));
                    GovernmentFundObj.EmployeeCodeID = GovernmentFund.EmployeeCodeID;
                    GovernmentFundObj.GovernmentFundTypeID = GovernmentFund.GovernmentFundTypeID;
                    GovernmentFundObj.GovernmentDeductionTypeID = GovernmentFund.GovernmentDeductionTypeID;
                    GovernmentFundObj.MonthlyDeductionAmount = GovernmentFund.MonthlyDeductionAmount;
                    GovernmentFundObj.TotalDeductionAmount = GovernmentFund.TotalDeductionAmount;
                    GovernmentFundObj.LoanNo = GovernmentFund.LoanNo;
                    GovernmentFundObj.LoanDate = GovernmentFund.LoanDate;
                    GovernmentFundObj.DeductionStartDate = GovernmentFund.DeductionStartDate;
                    GovernmentFundObj.ContractNo = GovernmentFund.ContractNo;
                    //GovernmentFundObj.ContractDate = GovernmentFund.ContractDate;
                    GovernmentFundObj.KSACityID = GovernmentFund.KSACityID;
                    GovernmentFundObj.SponserToName = GovernmentFund.SponserToName;
                    GovernmentFundObj.SponserToIDNo = GovernmentFund.SponserToIDNo;
                    GovernmentFundObj.LastUpdatedDate = GovernmentFund.LastUpdatedDate;
                    GovernmentFundObj.LastUpdatedBy = GovernmentFund.LastUpdatedBy;
                    GovernmentFundObj.BankIPAN = GovernmentFund.BankIPAN;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Deactivate(GovernmentFunds GovernmentFund)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    GovernmentFunds GovernmentFundObj = db.GovernmentFunds.SingleOrDefault(x => x.GovernmentFundID.Equals(GovernmentFund.GovernmentFundID));
                    GovernmentFundObj.LetterDate = GovernmentFund.LetterDate;
                    GovernmentFundObj.LetterNo = GovernmentFund.LetterNo;
                    GovernmentFundObj.Notes = GovernmentFund.Notes;
                    GovernmentFundObj.DeactiveDate = GovernmentFund.DeactiveDate;
                    GovernmentFundObj.IsActive = GovernmentFund.IsActive;
                    GovernmentFundObj.LastUpdatedDate = GovernmentFund.LastUpdatedDate;
                    GovernmentFundObj.LastUpdatedBy = GovernmentFund.LastUpdatedBy;
                    GovernmentFundObj.GovernmentFundDeactiveReasonID = GovernmentFund.GovernmentFundDeactiveReasonID;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int GovernmentFundID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    GovernmentFunds GovernmentFundsObj = db.GovernmentFunds.SingleOrDefault(x => x.GovernmentFundID.Equals(GovernmentFundID));
                    db.GovernmentFunds.Remove(GovernmentFundsObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }
        public List<GovernmentFunds> GetGovernmentFunds()
        {
            try
            {
                var db = new HCMEntities();
                var odData = db.GovernmentFunds
                    //.Include("EmployeesCodes")
                    .Include("EmployeesCodes.Employees")
                    .Include("GovernmentFundsTypes")
                    .Include("GovernmentDeductionsTypes")
                    .Include("CreatedByNav.Employees")
                    .Include("KSACities")
                    .Include("GovernmentFundsDeactiveReasons");

                var data = odData.ToList();

                return data;
            }
            catch
            {
                throw;
            }
        }

        public List<GovernmentFunds> GetGovernmentFunds(string EmployeeCodeNo)
        {
            try
            {
                var db = new HCMEntities();
                return db.GovernmentFunds
                    .Include("EmployeesCodes.Employees")
                    .Include("GovernmentFundsTypes")
                    .Include("GovernmentDeductionsTypes")
                    .Include("CreatedByNav.Employees")
                    .Include("KSACities")
                    .Include("GovernmentFundsDeactiveReasons")
                    .Where(x=> x.EmployeesCodes.EmployeeCodeNo == EmployeeCodeNo).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<GovernmentFunds> GetGovernmentFunds(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();

                var odData = db.GovernmentFunds
                    .Include("EmployeesCodes.Employees")
                    .Include("GovernmentFundsTypes")
                    .Include("GovernmentDeductionsTypes")
                    .Include("CreatedByNav.Employees")
                    .Include("KSACities")
                    .Include("GovernmentFundsDeactiveReasons");

                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<GovernmentFunds> iq = odData.Where(p => p.GovernmentFundID != null);
                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p => p.GovernmentFundID.ToString().ToLower().Contains(search.ToLower()) ||
                                    p.EmployeesCodes.EmployeeCodeNo.Contains(search.ToLower()) ||
                                    p.GovernmentDeductionsTypes.GovernmentDeductionTypeName.ToLower().Contains(search.ToLower()) ||
                                    p.GovernmentFundsTypes.GovernmentFundTypeName.ToLower().Contains(search.ToLower()) ||
                                    (p.EmployeesCodes.Employees.FirstNameAr.ToLower() + " " + p.EmployeesCodes.Employees.MiddleNameAr.ToLower()+ " "
                                        + p.EmployeesCodes.Employees.GrandFatherNameAr.ToLower() + " " + p.EmployeesCodes.Employees.LastNameAr.ToLower())
                                    .Contains(search.ToLower())
                                );
                }

                // Sorting.                
                //iq = iq.OrderBy(p => p.GovernmentFundID);
                iq = this.SortExpression(iq);

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

        public GovernmentFunds GetByGovernmentFundTypeID(int GovernmentFundID)
        {
            try
            {
                var db = new HCMEntities();
                return db.GovernmentFunds
                    //.Include("EmployeesCodes")
                    .Include("EmployeesCodes.Employees")
                    .Include("CreatedByNav.Employees")
                    .Include("GovernmentFundsTypes")
                    .Include("GovernmentDeductionsTypes")
                    .Include("GovernmentFundsDeactiveReasons")
                    .SingleOrDefault(x => x.GovernmentFundID.Equals(GovernmentFundID));
            }
            catch
            {
                throw;
            }
        }

        public List<GovernmentFunds> GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.GovernmentFunds.Include("EmployeesCodes.Employees")
                                         .Include("GovernmentFundsTypes")
                                         .Include("GovernmentDeductionsTypes")
                                         .Include("GovernmentFundsDeactiveReasons")
                                         .Where(x => x.EmployeeCodeID.Equals(EmployeeCodeID)).ToList();
            }
            catch
            {
                throw;
            }
        }

        private IQueryable<GovernmentFunds> SortExpression(IQueryable<GovernmentFunds> iq)
        {
            this.orderDir = string.IsNullOrEmpty(this.orderDir) ? "asc" : this.orderDir;
            this.orderByColumnName = string.IsNullOrEmpty(this.orderByColumnName) ? "" : this.orderByColumnName;
            switch (this.orderByColumnName)
            {
                case "GovernmentFundID":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.GovernmentFundID) : iq.OrderBy(p => p.GovernmentFundID);
                    break;
                case "EmployeeCodeNo":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.EmployeesCodes.EmployeeCodeNo) : iq.OrderBy(p => p.EmployeesCodes.EmployeeCodeNo);
                    break;
                case "EmployeeNameAr":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.EmployeesCodes.Employees.FirstNameAr + " " + p.EmployeesCodes.Employees.MiddleNameAr + " " + p.EmployeesCodes.Employees.GrandFatherNameAr + " " + p.EmployeesCodes.Employees.LastNameAr) : iq.OrderBy(p => p.EmployeesCodes.Employees.FirstNameAr + " " + p.EmployeesCodes.Employees.MiddleNameAr + " " + p.EmployeesCodes.Employees.GrandFatherNameAr + " " + p.EmployeesCodes.Employees.LastNameAr);
                    break;
                case "GovernmentDeductionTypeName":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.GovernmentDeductionsTypes.GovernmentDeductionTypeName) : iq.OrderBy(p => p.GovernmentDeductionsTypes.GovernmentDeductionTypeName);
                    break;
                case "GovernmentFundTypeName":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.GovernmentFundsTypes.GovernmentFundTypeName) : iq.OrderBy(p => p.GovernmentFundsTypes.GovernmentFundTypeName);
                    break;
                case "MonthlyDeductionAmount":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.MonthlyDeductionAmount) : iq.OrderBy(p => p.MonthlyDeductionAmount);
                    break;
                case "TotalDeductionAmount":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.TotalDeductionAmount) : iq.OrderBy(p => p.TotalDeductionAmount);
                    break;
                case "DeductionStartDate":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.DeductionStartDate) : iq.OrderBy(p => p.DeductionStartDate);
                    break;
                case "IsActive":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.IsActive) : iq.OrderBy(p => p.IsActive);
                    break;
                default:
                    break;
            }

            return iq;
        }

    }
}
