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
using System.Linq;

namespace HCMBLL
{
    public class StudiesVacationsBLL : BaseVacationsBLL
    {
        public override VacationsTypesEnum VacationType
        {
            get
            {
                return VacationsTypesEnum.Studies;
            }
        }

        public override float VacationConsumedBalance
        {
            get
            {
                return GetVacationConsumedByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID, 0);
            }
            set
            {
                VacationConsumedBalance = value;
            }
        }

        public DateTime? StudiesVacationStartDate
        {
            get;
            set;
        }

        internal override Vacations DALInstance
        {
            get
            {
                Vacations Vacation = base.DALInstance;
                Vacation.StudiesVacationStartDate = this.StudiesVacationStartDate;
                return Vacation;

            }
        }

        public override Result Add()
        {
            try
            {
                Result result = new Result();
                //bool IsValid = false;

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

                //#region Validation for balanace
                //IsValid = this.VacationPeriod > this.VacationRemainingBalance ? false : true;
                //if (!IsValid)
                //{
                //    result = new Result();
                //    result.Entity = null;
                //    result.EnumType = typeof(VacationsValidationEnum);
                //    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString();
                //    return result;
                //}
                //#endregion

                #region base validation
                result = base.Add();
                //if (result != null)
                //    return result;
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
            //bool IsValid = false;



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

            //#region Validation for balanace
            //IsValid = this.VacationPeriod > this.VacationRemainingBalance ? false : true;
            //if (!IsValid)
            //{
            //    result = new Result();
            //    result.Entity = null;
            //    result.EnumType = typeof(VacationsValidationEnum);
            //    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString();
            //    return result;
            //}
            //#endregion

            #region base validation
            result = base.Edit();
            //if (result != null)
            //    return result;
            #endregion


            return result;
        }

        public override Result Extend()
        {
            try
            {
                this.EmployeeCareerHistory = this.GetByVacationID(this.VacationID).EmployeeCareerHistory; // to get employee code id
                Result result = new Result();
                //bool IsValid = false;


                //#region Validation for balanace
                //IsValid = this.VacationPeriod > this.VacationRemainingBalance ? false : true;
                //if (!IsValid)
                //{
                //    result = new Result();
                //    result.Entity = null;
                //    result.EnumType = typeof(VacationsValidationEnum);
                //    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString();
                //    return result;
                //}
                //#endregion

                #region base validation
                result = base.Extend();
                //if (result != null)
                //    return result;
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

        //public override List<BaseVacationsBLL> GetByEmployeeCodeVacationType(int EmployeeCodeID)
        //{
        //    return base.GetByEmployeeCodeVacationType(EmployeeCodeID);
        //}

        internal override float GetVacationConsumedByEmployeeCodeID(int EmployeeCodeID, int ExceptVacationID)
        {
            return this.GetByEmployeeCodeVacationTypeNotCanceled(EmployeeCodeID)
                .ToList().Sum(d => d.VacationPeriod);
        }

        public List<StudiesVacationsBLL> GetStudiesVacationsByEmployeeCodeID(int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                return this.GetByEmployeeCodeID(EmployeeCodeID)
                .Where(
                     x => x.VacationType == this.VacationType & x.IsCanceled == false && (
                        (StartDate >= x.VacationStartDate && StartDate <= x.VacationEndDate) ||
                        (EndDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                        (StartDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                        (StartDate <= x.VacationStartDate && EndDate >= x.VacationEndDate))
                      ).Select(x => x as StudiesVacationsBLL)
                .ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}


