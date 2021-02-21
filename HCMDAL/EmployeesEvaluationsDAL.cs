using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class EmployeesEvaluationsDAL
    {
        public int Insert(EmployeesEvaluations EmployeeEvaluation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeesEvaluations.Add(EmployeeEvaluation);
                    db.SaveChanges();
                    return EmployeeEvaluation.EmployeeEvaluationID;
                }
            }
            catch
            {
                throw;
            }
        }

        public int Update(EmployeesEvaluations EmployeeEvaluation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesEvaluations EmployeeEvaluationObj = db.EmployeesEvaluations.SingleOrDefault(x => x.EmployeeEvaluationID.Equals(EmployeeEvaluation.EmployeeEvaluationID));
                    EmployeeEvaluationObj.MaturityYearID = EmployeeEvaluation.MaturityYearID;
                    EmployeeEvaluationObj.EvaluationPointID = EmployeeEvaluation.EvaluationPointID;
                    EmployeeEvaluationObj.EvaluationDegree = EmployeeEvaluation.EvaluationDegree;
                    EmployeeEvaluationObj.EmployeeCodeID = EmployeeEvaluation.EmployeeCodeID;
                    EmployeeEvaluationObj.LastUpdatedDate = EmployeeEvaluation.LastUpdatedDate;
                    EmployeeEvaluationObj.LastUpdatedBy = EmployeeEvaluation.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int EmployeeEvaluationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesEvaluations EmployeeEvaluationObj = db.EmployeesEvaluations.SingleOrDefault(x => x.EmployeeEvaluationID.Equals(EmployeeEvaluationID));
                    db.EmployeesEvaluations.Remove(EmployeeEvaluationObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesEvaluations> GetEmployeesEvaluations()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesEvaluations
                                .Include("EmployeesCodes")
                                .Include("EvaluationPoints")
                                .Include("MaturityYearsBalances")
                                .ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesEvaluations> GetEmployeeEvaluationsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesEvaluations
                                .Include("EmployeesCodes")
                                .Include("EvaluationPoints")
                                .Include("MaturityYearsBalances")
                                .Include("EmployeesEvaluationsDetails.EvaluationsQuarters")
                                .Where(x => x.EmployeeCodeID.Equals(EmployeeCodeID)).ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesEvaluations> GetEmployeeEvaluationsByEmployeeCode(string EmployeeCode)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesEvaluations
                                .Include("EmployeesCodes")
                                .Include("EvaluationPoints")
                                .Include("MaturityYearsBalances")
                                .Include("EmployeesEvaluationsDetails.EvaluationsQuarters")
                                .Where(x => x.EmployeesCodes.EmployeeCodeNo.Equals(EmployeeCode)).ToList();
            }
            catch
            {
                throw;
            }
        }

        public EmployeesEvaluations GetByEmployeeEvaluationID(int EmployeeEvaluationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesEvaluations
                                .Include("EmployeesCodes")
                                .Include("EvaluationPoints")
                                .Include("MaturityYearsBalances")
                                .FirstOrDefault(x => x.EmployeeEvaluationID.Equals(EmployeeEvaluationID));
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesEvaluations> GetEmployeesEvaluationsByEmployeeCodeIDs(List<int> EmployeeCodeIDs)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesEvaluations
                                .Include("EmployeesCodes")
                                .Include("EvaluationPoints")
                                .Include("MaturityYearsBalances")
                                .Where(x => EmployeeCodeIDs.Contains(x.EmployeeCodeID))
                                .ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}

