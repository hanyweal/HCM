using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HCMDAL;
using HCMBLL.Enums;


namespace HCMBLL
{
    public class EmployeesEvaluationsBLL : CommonEntity, IEntity
    {
        public virtual int EmployeeEvaluationID
        {
            get;
            set;
        }

        public virtual MaturityYearsBalancesBLL MaturityYearsBalances
        {
            get;
            set;
        }

        public virtual EvaluationPointsBLL EvaluationPoints
        {
            get;
            set;
        }

        public virtual EvaluationPointsEnum EvaluationPointsEnum { get; set; }

        public virtual float EvaluationDegree
        {
            get;
            set;
        }

        public virtual EmployeesCodesBLL EmployeeCode
        {
            get;
            set;
        }

        public virtual List<EmployeesEvaluationsDetailsBLL> EmployeeEvaluationDetails { get; set; }

        public decimal Points { get; set; }

        public virtual Result Add()
        {
            try
            {
                Result result = null;

                EmployeesEvaluations EmployeeEvaluation = new EmployeesEvaluations()
                {
                    EmployeeCodeID = this.EmployeeCode.EmployeeCodeID,
                    MaturityYearID = this.MaturityYearsBalances.MaturityYearID,
                    EvaluationPointID = this.EvaluationPoints.EvaluationPointID,
                    EvaluationDegree = this.EvaluationDegree,
                    CreatedDate = DateTime.Now,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID
                };
                this.EmployeeEvaluationID = new EmployeesEvaluationsDAL().Insert(EmployeeEvaluation);
                result = new Result()
                {
                    Entity = this,
                    EnumType = typeof(EmployeesEvaluationsValidationEnum),
                    EnumMember = EmployeesEvaluationsValidationEnum.Done.ToString(),
                };
                return result;

            }
            catch
            {
                throw;
            }
        }

        public virtual Result Update()
        {
            try
            {
                Result result = null;

                EmployeesEvaluations EmployeeEvaluation = new EmployeesEvaluations()
               {
                   EmployeeEvaluationID = this.EmployeeEvaluationID,
                   EmployeeCodeID = this.EmployeeCode.EmployeeCodeID,
                   MaturityYearID = this.MaturityYearsBalances.MaturityYearID,
                   EvaluationPointID = this.EvaluationPoints.EvaluationPointID,
                   EvaluationDegree = this.EvaluationDegree,
                   LastUpdatedDate = DateTime.Now,
                   LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
               };
                this.EmployeeEvaluationID = new EmployeesEvaluationsDAL().Update(EmployeeEvaluation);
                result = new Result()
                {
                    Entity = this,
                    EnumType = typeof(EmployeesEvaluationsValidationEnum),
                    EnumMember = EmployeesEvaluationsValidationEnum.Done.ToString(),
                };
                return result;
            }
            catch
            {
                throw;
            }
        }

        public virtual Result Remove(int EmployeeEvaluationID)
        {
            try
            {
                new EmployeesEvaluationsDAL().Delete(EmployeeEvaluationID, this.LoginIdentity.EmployeeCodeID);
                Result result = new Result()
                {
                    //Entity = this,
                    EnumType = typeof(EmployeesEvaluationsValidationEnum),
                    EnumMember = EmployeesEvaluationsValidationEnum.Done.ToString(),
                };
                return result;
            }
            catch
            {
                throw;
            }
        }

        public virtual List<EmployeesEvaluationsBLL> GetEmployeesEvaluations()
        {
            try
            {
                List<EmployeesEvaluations> EmployeesEvaluationsList = new EmployeesEvaluationsDAL().GetEmployeesEvaluations();
                List<EmployeesEvaluationsBLL> EmployeesEvaluationsBLLList = new List<EmployeesEvaluationsBLL>();
                foreach (var item in EmployeesEvaluationsList)
                {
                    EmployeesEvaluationsBLLList.Add(new EmployeesEvaluationsBLL().MapEmployeeEvaluation(item));
                }
                return EmployeesEvaluationsBLLList;
            }
            catch
            {
                throw;
            }
        }

        public virtual List<EmployeesEvaluationsBLL> GetEmployeeEvaluationsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                List<EmployeesEvaluations> EmployeesEvaluationsList = new EmployeesEvaluationsDAL().GetEmployeeEvaluationsByEmployeeCodeID(EmployeeCodeID);
                List<EmployeesEvaluationsBLL> EmployeesEvaluationsBLLList = new List<EmployeesEvaluationsBLL>();
                foreach (var item in EmployeesEvaluationsList)
                {
                    EmployeesEvaluationsBLLList.Add(new EmployeesEvaluationsBLL().MapEmployeeEvaluation(item));
                }
                return EmployeesEvaluationsBLLList;
            }
            catch
            {
                throw;
            }
        }

        public virtual List<EmployeesEvaluationsBLL> GetEmployeeEvaluationsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            try
            {
                List<EmployeesEvaluations> EmployeesEvaluationsList = new EmployeesEvaluationsDAL().GetEmployeeEvaluationsByEmployeeCode(EmployeeCodeNo);
                List<EmployeesEvaluationsBLL> EmployeesEvaluationsBLLList = new List<EmployeesEvaluationsBLL>();
                foreach (var item in EmployeesEvaluationsList)
                {
                    EmployeesEvaluationsBLLList.Add(new EmployeesEvaluationsBLL().MapEmployeeEvaluation(item));
                }
                return EmployeesEvaluationsBLLList;
            }
            catch
            {
                throw;

            }
        }

        public EmployeesEvaluationsBLL GetLastEmployeeEvaluationByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                return this.GetEmployeeEvaluationsByEmployeeCodeID(EmployeeCodeID).OrderByDescending(x => x.MaturityYearsBalances.MaturityYear).Take(1).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }

        public virtual EmployeesEvaluationsBLL GetByEmployeeEvaluationID(int EmployeeEvaluationID)
        {
            try
            {
                EmployeesEvaluations EmployeeEvaluation = new EmployeesEvaluationsDAL().GetByEmployeeEvaluationID(EmployeeEvaluationID);
                return new EmployeesEvaluationsBLL().MapEmployeeEvaluation(EmployeeEvaluation);
            }
            catch
            {
                throw;
            }
        }

        internal EmployeesEvaluationsBLL MapEmployeeEvaluation(EmployeesEvaluations EmployeeEvaluation)
        {
            try
            {
                EmployeesEvaluationsBLL EmployeeEvaluationBLL = null;
                if (EmployeeEvaluation != null)
                {
                    EmployeeEvaluationBLL = new EmployeesEvaluationsBLL()
                    {
                        EmployeeEvaluationID = EmployeeEvaluation.EmployeeEvaluationID,
                        MaturityYearsBalances = new MaturityYearsBalancesBLL().MapMaturityYearBalance(EmployeeEvaluation.MaturityYearsBalances),
                        EvaluationPoints = new EvaluationPointsBLL().MapEvaluationPoint(EmployeeEvaluation.EvaluationPoints),
                        EvaluationDegree = (float)EmployeeEvaluation.EvaluationDegree,
                        EvaluationPointsEnum = (EvaluationPointsEnum)EmployeeEvaluation.EvaluationPointID,
                        EmployeeCode = new EmployeesCodesBLL().MapEmployeeCode(EmployeeEvaluation.EmployeesCodes),
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(EmployeeEvaluation.CreatedByNav),
                        CreatedDate = EmployeeEvaluation.CreatedDate.Value
                    };
                    EmployeeEvaluationBLL.EmployeeEvaluationDetails = new List<EmployeesEvaluationsDetailsBLL>();

                    foreach (var item in EmployeeEvaluation.EmployeesEvaluationsDetails)
                    {
                        EmployeeEvaluationBLL.EmployeeEvaluationDetails.Add(new EmployeesEvaluationsDetailsBLL().MapEmployeeEvaluationDetails(item));
                    }
                }
                return EmployeeEvaluationBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
