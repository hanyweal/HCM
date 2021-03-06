﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HCMDAL;
using HCMBLL.Enums;


namespace HCMBLL
{
    public class EmployeesEvaluationsDetailsBLL : CommonEntity, IEntity
    {
        public int EmployeeEvaluationDetailID
        {
            get;
            set;
        }

        public EvaluationsQuartersBLL EvaluationQuarter
        {
            get;
            set;
        }

        public decimal DirectManagerEvaluation
        {
            get;
            set;
        }

        public decimal TimeAttendanceEvaluation { get; set; }

        public decimal ViolationsEvaluation { get; set; }

        public virtual EmployeesEvaluationsBLL EmployeeEvaluation
        {
            get;
            set;
        }

        public virtual List<EmployeesEvaluationsDetailsBLL> GetEmployeeEvaluationDetailsByEmployeeEvaluationID(int EmployeeEvaluationID)
        {
            try
            {
                List<EmployeesEvaluationsDetails> EmployeesEvaluationsDetailsList = new EmployeesEvaluationsDetailsDAL().GetEmployeeEvaluationDetailsByEmployeeEvaluationID(EmployeeEvaluationID);
                List<EmployeesEvaluationsDetailsBLL> EmployeeEvaluationDetailsBLLList = new List<EmployeesEvaluationsDetailsBLL>();
                foreach (var item in EmployeesEvaluationsDetailsList)
                {
                    EmployeeEvaluationDetailsBLLList.Add(new EmployeesEvaluationsDetailsBLL().MapEmployeeEvaluationDetails(item));
                }
                return EmployeeEvaluationDetailsBLLList;
            }
            catch
            {
                throw;
            }
        }

        public virtual Result Add()
        {
            try
            {
                Result result = null;
                if (this.DirectManagerEvaluation > 50)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(EmployeesEvaluationsValidationEnum);
                    result.EnumMember = EmployeesEvaluationsValidationEnum.RejectedBecauseOfDirectManagerEvaluationIsNotBetweenZeroAndFifty.ToString();
                    return result;
                }
                if (this.TimeAttendanceEvaluation > 35)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(EmployeesEvaluationsValidationEnum);
                    result.EnumMember = EmployeesEvaluationsValidationEnum.RejectedBecauseOfTimeAttendanceEvaluationIsNotBetweenZeroAndThirtyFive.ToString();
                    return result;
                }
                if (this.ViolationsEvaluation > 15)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(EmployeesEvaluationsValidationEnum);
                    result.EnumMember = EmployeesEvaluationsValidationEnum.RejectedBecauseOfViolationsEvaluationIsNotBetweenZeroAndFifteen.ToString();
                    return result;
                }

                EmployeesEvaluationsBLL EmployeesEvaluationBLL = new EmployeesEvaluationsBLL().GetByEmployeeEvaluationID(this.EmployeeEvaluation.EmployeeEvaluationID);
                List<EmployeesEvaluationsDetailsBLL> EmployeesEvaluationBLLList = new EmployeesEvaluationsBLL().GetByEmployeeEvaluationID(this.EmployeeEvaluation.EmployeeEvaluationID).EmployeeEvaluationDetails;
                if (EmployeesEvaluationBLLList.Exists(x => x.EvaluationQuarter.EvaluationQuarterID.Equals(this.EvaluationQuarter.EvaluationQuarterID)) && EmployeesEvaluationBLL.MaturityYearsBalances.MaturityYearID.Equals(this.EmployeeEvaluation.MaturityYearsBalances.MaturityYearID))
                {
                    result = new Result()
                    {
                        Entity = this,
                        EnumType = typeof(EmployeesEvaluationsValidationEnum),
                        EnumMember = EmployeesEvaluationsValidationEnum.RejectedBecauseOfEvaluationQuarterAlreadyExistsInCurrentYear.ToString(),
                    };
                    return result;
                }

                EmployeesEvaluationsDetails EmployeeEvaluationDetail = new EmployeesEvaluationsDetails()
                {
                    EmployeeEvaluationID = this.EmployeeEvaluation.EmployeeEvaluationID,
                    DirectManagerEvaluation = this.DirectManagerEvaluation,
                    TimeAttendanceEvaluation = this.TimeAttendanceEvaluation,
                    ViolationsEvaluation = this.ViolationsEvaluation,
                    EvaluationQuarterID = this.EvaluationQuarter.EvaluationQuarterID,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID,
                    CreatedDate = DateTime.Now,

                };
                int RecordID = new EmployeesEvaluationsDetailsDAL().Insert(EmployeeEvaluationDetail);
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
                if (this.DirectManagerEvaluation > 50)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(EmployeesEvaluationsValidationEnum);
                    result.EnumMember = EmployeesEvaluationsValidationEnum.RejectedBecauseOfDirectManagerEvaluationIsNotBetweenZeroAndFifty.ToString();
                    return result;
                }
                if (this.TimeAttendanceEvaluation > 35)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(EmployeesEvaluationsValidationEnum);
                    result.EnumMember = EmployeesEvaluationsValidationEnum.RejectedBecauseOfTimeAttendanceEvaluationIsNotBetweenZeroAndThirtyFive.ToString();
                    return result;
                }
                if (this.ViolationsEvaluation > 15)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(EmployeesEvaluationsValidationEnum);
                    result.EnumMember = EmployeesEvaluationsValidationEnum.RejectedBecauseOfViolationsEvaluationIsNotBetweenZeroAndFifteen.ToString();
                    return result;
                }

                EmployeesEvaluationsDetails EmployeeEvaluationDetail = new EmployeesEvaluationsDetails()
                {
                    EmployeeEvaluationDetailID = this.EmployeeEvaluationDetailID,
                    DirectManagerEvaluation = this.DirectManagerEvaluation,
                    TimeAttendanceEvaluation = this.TimeAttendanceEvaluation,
                    ViolationsEvaluation = this.ViolationsEvaluation,
                    LastUpdatedBy= this.LoginIdentity.EmployeeCodeID,
                    LastUpdatedDate=DateTime.Now

            };
                int RecordID = new EmployeesEvaluationsDetailsDAL().Update(EmployeeEvaluationDetail);
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

        internal EmployeesEvaluationsDetailsBLL MapEmployeeEvaluationDetails(EmployeesEvaluationsDetails EmployeeEvaluationDetail)
        {
            try
            {
                EmployeesEvaluationsDetailsBLL EmployeeEvaluationDetailBLL = null;
                if (EmployeeEvaluationDetail != null)
                {
                    EmployeeEvaluationDetailBLL = new EmployeesEvaluationsDetailsBLL()
                    {
                        EmployeeEvaluationDetailID = EmployeeEvaluationDetail.EmployeeEvaluationDetailID,
                        EvaluationQuarter = new EvaluationsQuartersBLL().MapEvaluationQuarter(EmployeeEvaluationDetail.EvaluationsQuarters),
                        DirectManagerEvaluation = EmployeeEvaluationDetail.DirectManagerEvaluation,
                        TimeAttendanceEvaluation = EmployeeEvaluationDetail.TimeAttendanceEvaluation,
                        ViolationsEvaluation = EmployeeEvaluationDetail.ViolationsEvaluation
                    };
                }
                return EmployeeEvaluationDetailBLL;
            }
            catch
            {
                throw;
            }
        }

    }
}
