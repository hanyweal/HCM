﻿using HCMBLL.Enums;
using HCMDAL;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HCMBLL
{
    public class MarriageVacationsBLL : BaseVacationsBLL
    {
        public override VacationsTypesEnum VacationType
        {
            get
            {
                return VacationsTypesEnum.Marriage;
            }
        }
       
        public override Result Add()
        {
            try
            {
                Result result = new Result();

                #region Validation for balanace
                //result = IsBalanceValid();
                //if (result != null)
                //    return result;
                if (this.VacationPeriod > this.VacationBalance)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(VacationsValidationEnum);
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString();
                    return result;
                }
                #endregion
                #region Validation for dates
                List<BaseVacationsBLL> MarriageVacations = this.GetByEmployeeCodeVacationTypeNotCanceled(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID).ToList();
                if (MarriageVacations.Count > 0)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(VacationsValidationEnum);
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfMarriageVacationAcceptedOneTime.ToString();
                    return result;
                }
                #endregion
                #region base validation
                result = base.Add();
               
                #endregion

                return result;
            }
            catch
            {
                throw;
            } 
        }

        public override Result Edit()
        {
            this.EmployeeCareerHistory = this.GetByVacationID(this.VacationID).EmployeeCareerHistory; // to get EmployeeCodeID
            Result result = new Result();           

            #region Validation for vacation creation during probation period
            EmployeesCareersHistoryBLL HiringRecord = new EmployeesCareersHistoryBLL().GetHiringRecordByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
            if (HiringRecord != null)
            {
                // probation period = hiring date + 1 year
                DateTime ProbationEndDate = HiringRecord.ProbationEndDate;//.JoinDate.AddYears(1).AddDays(-10);
                if (ProbationEndDate > this.VacationStartDate)
                {
                    result = new Result();
                    result.Entity = HiringRecord;
                    result.EnumType = typeof(VacationsValidationEnum);
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfDuringProbation.ToString();
                    return result;
                }
            }
            #endregion

            #region Validation for balanace
            //result = IsBalanceValid(this.VacationID);
            //if (result != null)
            //    return result;
            if (this.VacationPeriod > this.VacationBalance)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(VacationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString();
                return result;
            }
            #endregion

            #region Validation for dates
            if (this.VacationStartDate > DateTime.Now)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(VacationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.RejectedBecauseOfInvalidStartDate.ToString();
                return result;
            }
            #endregion

            #region base validation
            result = base.Edit();
          
            #endregion

            return result;
        }

        public override Result Extend()
        {
            try
            {
                this.EmployeeCareerHistory = this.GetByVacationID(this.VacationID).EmployeeCareerHistory; // to get employee code id
                Result result = new Result();

                #region Validation for balanace
                //result = IsBalanceValid();
                //if (result != null)
                //    return result;
                bool IsValid = false;
                bool CheckOldBalance = false;

                CheckOldBalance = (this.GetByVacationID(this.VacationID).VacationEndDate.Subtract(this.GetByVacationID(this.VacationID).VacationStartDate).Days + 1) + this.VacationPeriod > this.VacationBalance ? false : true;
                IsValid = (this.VacationPeriod > this.VacationBalance) ? false : true;
                if (!IsValid || !CheckOldBalance)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(VacationsValidationEnum);
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString();
                    return result;
                }

                #endregion

                #region base validation
                 result = base.Extend();
              
                #endregion
              
                return result;
            }
            catch
            {
                throw;
            }
        }

        public override Result Break()
        {
            try
            {
                Result result = base.Break();
               
                return result;
            }
            catch
            {
                throw;
            }
        }

        public override Result Cancel()
        {
            try
            {
                Result result = base.Cancel();
              
                return result;
            }
            catch
            {
                throw;
            }
        }

        internal override float GetVacationConsumedByEmployeeCodeID(int EmployeeCodeID, int ExceptVacationID = 0)
        {
          
            List<BaseVacationsBLL> MarriageVacations = this.GetByEmployeeCodeVacationTypeNotCanceled(EmployeeCodeID, ExceptVacationID).ToList();
            if (MarriageVacations.Count > 0)
            {
                MarriageVacationsBLL _vacation = this.GetByVacationID(MarriageVacations[0].VacationID) as MarriageVacationsBLL;
                return (_vacation.VacationEndDate.Subtract(_vacation.VacationStartDate).Days + 1);
            }
            else
            {
                return 0;
            } 
        }
    }
}

