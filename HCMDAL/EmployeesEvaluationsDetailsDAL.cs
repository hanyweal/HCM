using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class EmployeesEvaluationsDetailsDAL
    {
        public List<EmployeesEvaluationsDetails> GetEmployeeEvaluationDetailsByEmployeeEvaluationID(int EmployeeEvaluationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesEvaluationsDetails
                                .Include("EvaluationsQuarters")
                                .Where(x => x.EmployeeEvaluationID.Equals(EmployeeEvaluationID)).ToList();
            }
            catch
            {
                throw;
            }
        }

        public int Insert(EmployeesEvaluationsDetails EmployeeEvaluationDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.EmployeesEvaluationsDetails.Add(EmployeeEvaluationDetail);
                    db.SaveChanges();
                    return EmployeeEvaluationDetail.EmployeeEvaluationDetailID;
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

        public int Update(EmployeesEvaluationsDetails EmployeeEvaluationDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    EmployeesEvaluationsDetails EmployeesEvaluationsDetailsObj = db.EmployeesEvaluationsDetails.SingleOrDefault(x => x.EmployeeEvaluationDetailID.Equals(EmployeeEvaluationDetail.EmployeeEvaluationDetailID));
                    EmployeesEvaluationsDetailsObj.DirectManagerEvaluation = EmployeeEvaluationDetail.DirectManagerEvaluation;
                    EmployeesEvaluationsDetailsObj.TimeAttendanceEvaluation = EmployeeEvaluationDetail.TimeAttendanceEvaluation;
                    EmployeesEvaluationsDetailsObj.ViolationsEvaluation = EmployeeEvaluationDetail.ViolationsEvaluation;
                    EmployeesEvaluationsDetailsObj.LastUpdatedBy = EmployeeEvaluationDetail.LastUpdatedBy;
                    EmployeesEvaluationsDetailsObj.LastUpdatedDate = EmployeeEvaluationDetail.LastUpdatedDate;
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
