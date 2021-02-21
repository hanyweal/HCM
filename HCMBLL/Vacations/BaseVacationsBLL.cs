using HCMBLL.DTO;
using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HCMBLL
{
    public class BaseVacationsBLL : CommonEntity, IVacations
    {
        public virtual int VacationID { get; set; }

        public virtual VacationsTypesEnum VacationType { get; set; }

        public virtual EVacationsRequestsBLL EVacationsRequest { get; set; }

        public virtual bool IsApprovedFromManager { get; set; } = true; // default value in c#6 and above

        /// <summary>
        /// 1- Dated: Initial Releae
        /// Rule: 
        /// Get Vacation balance from VacationTypes table, based on VacationTypeID
        /// 2- Dated: 02-01-2020
        /// Rule:
        /// Get Vacation balance from RanksCategoriesVacationsBalances table, based on VacationTypeID & RankCategoryID
        /// </summary>
        public virtual float VacationBalance
        {
            get
            {
                int RankCategoryID = 0;
                if (this.EmployeeCareerHistory.OrganizationJob.Rank != null
                    && this.EmployeeCareerHistory.OrganizationJob.Rank.RankCategory != null
                    && this.EmployeeCareerHistory.OrganizationJob.Rank.RankCategory.RankCategoryID > 0)
                    RankCategoryID = this.EmployeeCareerHistory.OrganizationJob.Rank.RankCategory.RankCategoryID;

                if (RankCategoryID == 0)
                    throw new NullReferenceException();

                return new RanksCategoriesVacationsBalancesBLL().GetByRankCategoryIDVacationTypeID(RankCategoryID, Convert.ToInt32(this.VacationType));
            }
            set
            {
                VacationBalance = value;
            }
        }

        public virtual float VacationRemainingBalance
        {
            get
            {
                return VacationBalance - VacationConsumedBalance;
            }
            set
            {
                VacationRemainingBalance = value;
            }
        }

        public virtual float VacationConsumedBalance { get; set; }

        public virtual string VacationTypeName { get; set; }

        public virtual EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public virtual DateTime VacationStartDate { get; set; }

        public virtual DateTime VacationEndDate { get; set; }

        public virtual int VacationPeriod
        {
            get
            {
                return this.VacationEndDate.Subtract(this.VacationStartDate).Days + 1;
            }
        }

        public virtual DateTime WorkDate
        {
            get
            {
                return this.VacationEndDate.AddDays(1);
            }
        }

        private int LastVacationCount { get { return 5; } }

        public virtual bool IsCanceled { get; set; }

        public string Notes { get; set; }

        public string MainframeNo { get; set; }

        public EmployeesCodesBLL ApprovedBy { get; set; }

        public NormalVacationsTypesBLL NormalVacationsType { get; set; }

        public bool HasDetailsWithoutCreationPermission { get; set; }

        public virtual List<VacationsDetailsBLL> VacationDetails { get; set; }

        internal virtual Vacations DALInstance
        {
            get
            {
                Vacations Vacation = new Vacations()
                {
                    VacationID = this.VacationID,
                    VacationTypeID = (int)this.VacationType,
                    VacationStartDate = this.VacationStartDate,
                    VacationEndDate = this.VacationEndDate,
                    IsCanceled = false,
                    EmployeeCareerHistoryID = this.EmployeeCareerHistory != null ? this.EmployeeCareerHistory.EmployeeCareerHistoryID : 0,
                    CreatedDate = DateTime.Now,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID,
                    EVacationRequestID = this.EVacationsRequest != null ? this.EVacationsRequest.EVacationRequestID : (int?)null
                };
                return Vacation;
            }
        }

        public virtual Result Add()
        {
            Result result = null;

            #region Validation for vacation before hiring or not
            EmployeesCareersHistoryBLL HiringRecord = new EmployeesCareersHistoryBLL().GetHiringRecordByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
            if (HiringRecord != null)
            {
                if (HiringRecord.JoinDate > this.VacationStartDate)
                {
                    result = new Result();
                    result.Entity = HiringRecord;
                    result.EnumType = typeof(VacationsValidationEnum);
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfBeforeHiringDate.ToString();
                    return result;
                }
            }
            #endregion

            #region Validation for vacation creation during probation period
            /* remove this validation as per task # 264 */
            ////EmployeesCareersHistoryBLL HiringRecord = new EmployeesCareersHistoryBLL().GetHiringRecordByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
            //if (HiringRecord != null)
            //{
            //    // probation period = hiring date + 1 year
            //    DateTime ProbationEndDate = HiringRecord.JoinDate.AddYears(1).AddDays(-10);     // this workaround to create Probation End Date in Hijri
            //    if (ProbationEndDate > this.VacationStartDate)
            //    {
            //        result = new Result();
            //        result.Entity = HiringRecord;
            //        result.EnumType = typeof(VacationsValidationEnum);
            //        result.EnumMember = VacationsValidationEnum.RejectedBecauseOfDuringProbation.ToString();
            //        return result;
            //    }
            //}
            #endregion

            #region Validation for conflict With Other Process
            result = CommonHelper.IsNoConflictWithOtherProcess(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                                                                this.VacationStartDate, this.VacationEndDate, BusinessSubCategoriesEnum.OverTimes);
            #endregion

            #region Validation for vacation start day and vacation end day
            //if vacation start day is sunday ... check if there is vaction (same vacation type) on last thursday or not.. prevent the adding if there is and employee must extend last vacation
            if (this.VacationStartDate.DayOfWeek == DayOfWeek.Sunday)
            {
                BaseVacationsBLL LastVacationBeforeWeekEnd = this.GetEmployeeVacationsBetweenTwoDates(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                                                                                                      this.VacationStartDate.Date.AddDays(-3), 
                                                                                                      this.VacationStartDate.Date.AddDays(-3))?.FirstOrDefault(x=> x.VacationType == this.VacationType);
                if (LastVacationBeforeWeekEnd != null)
                {
                    result = new Result();
                    result.Entity = LastVacationBeforeWeekEnd;
                    result.EnumType = typeof(VacationsValidationEnum);
                    result.EnumMember = VacationsValidationEnum.RejectedBeacuseOfNotAllowedWeekEndBetweenTwoVacations.ToString();
                    return result;
                }
            }

            //if vacation end day is thursday ... check if there is vaction (same vacation type) on next sunday or not.. prevent the adding if there is and employee must edit last vacation
            if (this.VacationEndDate.DayOfWeek == DayOfWeek.Thursday)
            {
                BaseVacationsBLL NextVacationAfterWeekEnd = this.GetEmployeeVacationsBetweenTwoDates(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID, 
                                                                                                     this.VacationEndDate.Date.AddDays(3), 
                                                                                                     this.VacationEndDate.Date.AddDays(3))?.FirstOrDefault(x => x.VacationType == this.VacationType);
                if (NextVacationAfterWeekEnd != null)
                {
                    result = new Result();
                    result.Entity = NextVacationAfterWeekEnd;
                    result.EnumType = typeof(VacationsValidationEnum);
                    result.EnumMember = VacationsValidationEnum.RejectedBeacuseOfNotAllowedWeekEndBetweenTwoVacations.ToString();
                    return result;
                }
            }
            #endregion

            if (result != null)
                return result;

            result = this.AddVacations();
            return result;
        }

        private Result AddVacations()
        {
            try
            {
                Result result = null;

                /* Approved from manager has 2 meanings : 
                 * 1 - electronic approval in e vacation request    
                 * 2 - manual approval in paper */

                if (this.IsApprovedFromManager) // save into vacations table
                {
                    if (this.EVacationsRequest == null) // thats mean this vacation created from vacations module
                    {
                        #region Validation to check there is any e vacation request is pending or not  
                        // pending means that no action by authorized person till now or this e vacation is not cancelled by creator
                        result = new EVacationsRequestsBLL().IsEVacationRequestPendingExist(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo);
                        if (result != null) return result;
                        #endregion

                        #region Check the employee has organization or not
                        PlacementBLL CurrentActualOrgAndActualJob = new PlacementBLL().GetCurrentActualOrgAndActualJob(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo);
                        if (CurrentActualOrgAndActualJob == null)
                        {
                            result = new Result();
                            result.EnumType = typeof(VacationsValidationEnum);
                            result.EnumMember = VacationsValidationEnum.RejectedBeacuseOfEmployeeNotHasActualOrganization.ToString();
                            return result;
                        }

                        #endregion
                    }

                    List<VacationsDetails> VacationDetail = new List<VacationsDetails>();
                    VacationDetail.Add(this.GetVacationDetailInstance(VacationsActionsEnum.Add));
                    Vacations Vacation = this.DALInstance;
                    Vacation.EmployeeCareerHistoryID = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID).EmployeeCareerHistoryID;

                    Vacation.VacationsDetails = VacationDetail;
                    this.VacationID = new VacationsDAL().Insert(Vacation);

                    if (this.VacationID != 0)
                    {
                        result = new Result();
                        result.Entity = this;
                        result.EnumType = typeof(VacationsValidationEnum);

                        #region Send this vacation of employee to Time Attendance App
                        result = new TimeAttendanceBLL().AddEmployeeVacation(this.VacationID,
                                                                            this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                                                                            this.VacationType,
                                                                            this.VacationStartDate,
                                                                            this.VacationEndDate,
                                                                            this.LoginIdentity.EmployeeCodeNo,
                                                                            DateTime.Now
                                                                            );
                        #endregion

                        if (result.EnumMember != VacationsValidationEnum.Done.ToString())
                        {
                            this.Remove();
                            result.EnumMember = VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString();
                        }
                        else
                            result.EnumMember = VacationsValidationEnum.Done.ToString();
                    }
                }
                else // save into EVacationRequests table
                {
                    EVacationsRequestsBLL EVacationRequestBLL = new EVacationsRequestsBLL()
                    {
                        EmployeeCareerHistory = this.EmployeeCareerHistory,
                        VacationTypeEnum = this.VacationType,
                        VacationStartDate = this.VacationStartDate,
                        VacationEndDate = this.VacationEndDate,
                        CreatorNotes = this.Notes,
                    };

                    result = EVacationRequestBLL.AddEVacationRequest();

                    if (result.Entity is EServicesProxiesBLL)
                        result.Entity = (EVacationsRequestsBLL)result.Entity;

                    else
                    {
                        this.EVacationsRequest = (EVacationsRequestsBLL)result.Entity;
                        result.Entity = this;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                this.Remove();
                throw ex;
            }
        }

        /// <summary>
        /// Task 247: Adding new action in vacation actions (Edit action)
        /// 1- Save EDIT action in vacations details table like (Extend,Break,and cancel)
        /// 2- Remove existing validation: validate if vacation creation action approved "RejectedBeacuseOfAlreadyApproved"
        /// 3- Add new validations
        ///    i-  Previous vacation detail action must be approved
        ///    ii- Vacation must not be cancel, break, or extend
        /// </summary>  
        /// <returns></returns>
        public virtual Result Edit()
        {
            Result result = IsValid();

            if (result != null)
                return result;

            // Removed this validation based on Task 247
            //#region validate if vacation creation action approved or not
            //if (new VacationsDetailsBLL().GetVacationsDetailsByVacationID(this.VacationID).FirstOrDefault().IsApproved)
            //{
            //    result = new Result();
            //    result.EnumType = typeof(VacationsValidationEnum);
            //    result.EnumMember = VacationsValidationEnum.RejectedBeacuseOfAlreadyApproved.ToString();
            //    return result;
            //}
            //#endregion

            #region Validate the vacation is break before or not
            VacationsDetails vd = new VacationsDetailsDAL().GetVacationsDetailsByVacationID(VacationID, (int)VacationsActionsEnum.Break);
            if (vd != null && vd.VacationDetailID > 0)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(VacationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.RejectedBecauseOfAlreadyBreak.ToString();
                return result;
            }
            #endregion

            #region Validate the vacation is extended before or not
            vd = new VacationsDetailsDAL().GetVacationsDetailsByVacationID(VacationID, (int)VacationsActionsEnum.Extend);
            if (vd != null && vd.VacationDetailID > 0)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(VacationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.RejectedBecauseOfAlreadyExtend.ToString();
                return result;
            }
            #endregion

            #region validate for vacation before hiring or not
            EmployeesCareersHistoryBLL HiringRecord = new EmployeesCareersHistoryBLL().GetHiringRecordByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
            if (HiringRecord != null)
            {
                if (HiringRecord.JoinDate > this.VacationStartDate)
                {
                    result = new Result();
                    result.Entity = HiringRecord;
                    result.EnumType = typeof(VacationsValidationEnum);
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfBeforeHiringDate.ToString();
                    return result;
                }
            }
            #endregion

            #region validate for Conflict With Other Process
            result = CommonHelper.IsNoConflictWithOtherProcess(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                                                                this.VacationStartDate, this.VacationEndDate, BusinessSubCategoriesEnum.OverTimes);
            if (result != null)
            {
                // we should exclude this vacation from validation
                if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString())
                {
                    List<BaseVacationsBLL> BaseVacationBLLList = new EmployeesCodesBLL().GetVacationsByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID)
                .Where(
                     x => x.IsCanceled == false && x.VacationID != this.VacationID && (
                        (VacationStartDate >= x.VacationStartDate && VacationStartDate <= x.VacationEndDate) ||
                        (VacationEndDate >= x.VacationStartDate && VacationEndDate <= x.VacationEndDate) ||
                        (VacationStartDate >= x.VacationStartDate && VacationEndDate <= x.VacationEndDate) ||
                        (VacationStartDate <= x.VacationStartDate && VacationEndDate >= x.VacationEndDate))
                      )
                .ToList();
                    if (BaseVacationBLLList.Count() != 0)
                    {
                        result = new Result();
                        result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                        result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString();
                        return result;
                    }
                    else
                        result = null;
                }
                //return result;
            }
            #endregion

            if (result != null)
                return result;

            result = this.EditVacations();
            return result;
        }

        private Result EditVacations()
        {
            Result result = null;

            Vacations Vacation = DALInstance;

            #region Validation to check there is any e vacation request is pending or not  
            // pending means that no action by authorized person till now or this e vacation is not cancelled by creator
            result = new EVacationsRequestsBLL().IsEVacationRequestPendingExist(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo);
            if (result != null) return result;
            #endregion

            Vacation.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
            new VacationsDAL().Update(Vacation);
            // update dates of creation record
            new VacationsDetailsDAL().UpdateCreationDates(new VacationsDetails()
            {
                FromDate = this.VacationStartDate,
                Notes = this.Notes,
                ToDate = VacationEndDate,
                VacationID = this.VacationID,
                LastUpdatedDate = DateTime.Now,
                LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
            });

            int VacationDetailID = new VacationsDetailsDAL().Insert(this.GetVacationDetailInstance(VacationsActionsEnum.Edit));

            #region Send this vacation of employee to Time Attendance App to edit
            result = new Result();
            result.Entity = this;
            result.EnumType = typeof(VacationsValidationEnum);

            result = new TimeAttendanceBLL().EditEmployeeVacation(this.VacationID, this.VacationStartDate, this.VacationEndDate,
                                                                    this.LoginIdentity.EmployeeCodeNo);

            if (result.EnumMember != VacationsValidationEnum.Done.ToString())
            {
                //this.Remove();
                result.EnumMember = VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString();
            }
            else
                result.EnumMember = VacationsValidationEnum.Done.ToString();
            #endregion

            return result;
        }

        public virtual Result Cancel()
        {
            Result result = IsValid();
            //return result;
            if (result != null)
                return result;

            result = this.CancelVacations();
            return result;
        }

        private Result CancelVacations()
        {

            #region Validation to check there is any e vacation request is pending or not  
            // pending means that no action by authorized person till now or this e vacation is not cancelled by creator
            Result result = new EVacationsRequestsBLL().IsEVacationRequestPendingExist(EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo);
            if (result != null) return result;
            #endregion

            Vacations Vacation = DALInstance;
            Vacation.IsCanceled = true;
            new VacationsDAL().Update(Vacation);
            int VacationDetailID = new VacationsDetailsDAL().Insert(this.GetVacationDetailInstance(VacationsActionsEnum.Cancel));

            if (VacationDetailID != 0)
            {
                #region In case of cancellation the vacation, this vacation should be deleted from Time Attendance App
                new TimeAttendanceBLL().DeleteEmployeeVacation(Vacation.VacationID, this.LoginIdentity.EmployeeCodeNo);
                #endregion

                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(DelegationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.Done.ToString();
            }
            return result;
        }

        public virtual Result Extend()
        {
            Result result = IsValid();

            if (result != null)
                return result;

            #region validate for Conflict With Other Process
            result = CommonHelper.IsNoConflictWithOtherProcess(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID, this.VacationStartDate, this.VacationEndDate, BusinessSubCategoriesEnum.OverTimes);
            #endregion

            if (result != null)
                return result;

            result = this.ExtendVacations();
            return result;
        }

        private Result ExtendVacations()
        {
            Vacations Vacation = DALInstance;

            #region Validation to check there is any e vacation request is pending or not  
            // pending means that no action by authorized person till now or this e vacation is not cancelled by creator
            Result result = new EVacationsRequestsBLL().IsEVacationRequestPendingExist(EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo);
            if (result != null) return result;
            #endregion

            Vacation.VacationStartDate = this.GetByVacationID(this.VacationID).VacationStartDate;
            new VacationsDAL().UpdateVacationDates(Vacation);

            int VacationDetailID = new VacationsDetailsDAL().Insert(this.GetVacationDetailInstance(VacationsActionsEnum.Extend));

            if (VacationDetailID != 0)
            {
                #region Send the vacation of employee to Time Attendance App To edit
                new TimeAttendanceBLL().EditEmployeeVacation(Vacation.VacationID, Vacation.VacationStartDate, Vacation.VacationEndDate, this.LoginIdentity.EmployeeCodeNo);
                #endregion

                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(DelegationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.Done.ToString();
            }
            return result;
        }

        public virtual Result Break()
        {
            Result result = IsValid();

            if (result != null)
                return result;

            #region Validation for new work date greater than previous work date
            BaseVacationsBLL Vacation = this.GetByVacationID(this.VacationID);
            if (this.WorkDate >= Vacation.WorkDate)
            {
                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(DelegationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.RejectedBecauseOfNewWorkDateGreaterThanPreviosWorkDate.ToString();
            }
            if (result != null)
                return result;
            #endregion

            result = this.BreakVacations();
            return result;
        }

        private Result BreakVacations()
        {

            #region Validation to check there is any e vacation request is pending or not  
            // pending means that no action by authorized person till now or this e vacation is not cancelled by creator
            Result result = new EVacationsRequestsBLL().IsEVacationRequestPendingExist(EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo);
            if (result != null) return result;
            #endregion

            int VacationDetailID = new VacationsDetailsDAL().Insert(this.GetVacationDetailInstance(VacationsActionsEnum.Break));
            new VacationsDAL().UpdateVacationDates(DALInstance);

            if (VacationDetailID != 0)
            {
                #region Send the vacation of employee to Time Attendance App To edit
                new TimeAttendanceBLL().EditEmployeeVacation(DALInstance.VacationID, DALInstance.VacationStartDate, DALInstance.VacationEndDate, this.LoginIdentity.EmployeeCodeNo);
                #endregion

                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(DelegationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.Done.ToString();
            }
            return result;
        }

        public virtual Result Remove()
        {
            new VacationsDAL().Delete(this.VacationID, this.LoginIdentity.EmployeeCodeID);
            return new Result();
        }

        public BaseVacationsBLL GetByVacationID(int VacationID)
        {
            try
            {
                Vacations Vacation = new VacationsDAL().GetByVacationID(VacationID);
                return new BaseVacationsBLL().MapVacation(Vacation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<BaseVacationsBLL> GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                List<BaseVacationsBLL> VacationsBLLList = new List<BaseVacationsBLL>();
                List<Vacations> VacationsList = new VacationsDAL().GetEmployeeVacations(EmployeeCodeID).ToList();
                foreach (var Vacation in VacationsList)
                    VacationsBLLList.Add(this.MapVacation(Vacation));

                return VacationsBLLList.OrderBy(x => x.VacationStartDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<BaseVacationsBLL> GetByEmployeeCodeID(int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            return this.GetByEmployeeCodeID(EmployeeCodeID)
                .Where(
                     x => x.IsCanceled == false && (
                        (StartDate >= x.VacationStartDate && StartDate <= x.VacationEndDate) ||
                        (EndDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                        (StartDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                        (StartDate <= x.VacationStartDate && EndDate >= x.VacationEndDate))
                      )
                .ToList();
        }

        public virtual List<BaseVacationsBLL> GetByEmployeeCodeVacationType(int EmployeeCodeID)
        {
            try
            {
                List<BaseVacationsBLL> VacationsBLLList = this.GetByEmployeeCodeID(EmployeeCodeID).Where(x => x.VacationType == this.VacationType).ToList();
                return VacationsBLLList.OrderBy(x => x.VacationStartDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<BaseVacationsBLL> GetByEmployeeCodeVacationTypeNotCanceled(int EmployeeCodeID, int? ExceptVactionID = 0)
        {
            List<BaseVacationsBLL> VacationsList = this.GetByEmployeeCodeVacationType(EmployeeCodeID).Where
                               (
                                   x => x.IsCanceled == false
                                && (ExceptVactionID != 0 ? x.VacationID != ExceptVactionID : x.VacationID == x.VacationID)
                               //&& (
                               //       (StartDate >= x.VacationStartDate && StartDate <= x.VacationEndDate) ||
                               //       (EndDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                               //       (StartDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                               //       (StartDate <= x.VacationStartDate && EndDate >= x.VacationEndDate)
                               //   )
                               ).ToList();
            return VacationsList;
        }

        public virtual List<BaseVacationsBLL> GetByEmployeeCodeVacationTypeNotCanceled(int EmployeeCodeID, DateTime StartDate, DateTime EndDate, int? ExceptVactionID = 0)
        {
            List<BaseVacationsBLL> VacationsList = this.GetByEmployeeCodeVacationType(EmployeeCodeID).Where
                               (
                                   x => x.IsCanceled == false
                                && (ExceptVactionID != 0 ? x.VacationID != ExceptVactionID : x.VacationID == x.VacationID)
                                && (
                                       (StartDate >= x.VacationStartDate && StartDate <= x.VacationEndDate) ||
                                       (EndDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                                       (StartDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                                       (StartDate <= x.VacationStartDate && EndDate >= x.VacationEndDate)
                                   )
                               ).ToList();
            return VacationsList;
        }

        public virtual int GetCountVacationPeriodByEmployeeCodeVacationType(List<Vacations> VacationsList, int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                List<Vacations> Vacations = VacationsList.Where(
                                                            x =>
                                                                ((StartDate >= x.VacationStartDate && StartDate <= x.VacationEndDate) ||
                                                                (EndDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                                                                (StartDate >= x.VacationStartDate && EndDate <= x.VacationEndDate) ||
                                                                (StartDate <= x.VacationStartDate && EndDate >= x.VacationEndDate))
                                                            ).ToList();
                //return VacationsList.Sum(x => x.VacationPeriod);

                int TotalDays = 0;
                DateTime Date;

                for (DateTime i = StartDate; i <= EndDate;)
                {
                    Date = i;

                    foreach (var item in VacationsList)
                    {
                        if (Date >= item.VacationStartDate.Date && Date <= item.VacationEndDate.Date)
                            TotalDays++;
                    }

                    i = i.AddDays(1);
                }

                return TotalDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<BaseVacationsBLL> GetCanceledVacations(int EmployeeCodeID, VacationsTypesEnum VacationType)
        {
            //return this.GetByEmployeeCodeVacationType(EmployeeCodeID, VacationType).Where(x => x.IsCanceled == true).OrderBy(x => x.VacationStartDate).ToList();
            List<BaseVacationsBLL> VacationsBLLList = new List<BaseVacationsBLL>();
            List<Vacations> VacationsList = new VacationsDAL().GetCanceledByEmployeeCodeIDVacationTypeID(EmployeeCodeID, (int)VacationType).ToList();
            foreach (var Vacation in VacationsList)
                VacationsBLLList.Add(this.MapVacation(Vacation));

            return VacationsBLLList;
        }

        public virtual List<BaseVacationsBLL> GetFinishedVacations(int EmployeeCodeID, VacationsTypesEnum VacationType)
        {
            //return this.GetByEmployeeCodeVacationType(EmployeeCodeID, VacationType).Where(x => x.VacationEndDate < DateTime.Now.Date && x.IsCanceled != true).OrderBy(x => x.VacationStartDate).ToList();
            List<BaseVacationsBLL> VacationsBLLList = new List<BaseVacationsBLL>();
            List<Vacations> VacationsList = new VacationsDAL().GetFinishedByEmployeeCodeIDVacationTypeID(EmployeeCodeID, (int)VacationType).ToList();
            foreach (var Vacation in VacationsList)
                VacationsBLLList.Add(this.MapVacation(Vacation));

            return VacationsBLLList;
        }

        public virtual List<BaseVacationsBLL> GetValidVacations(int EmployeeCodeID, VacationsTypesEnum VacationType)
        {
            //return this.GetByEmployeeCodeVacationType(EmployeeCodeID, VacationType).Where(x => x.VacationEndDate > DateTime.Now.Date && x.IsCanceled != true).OrderBy(x => x.VacationStartDate).ToList();
            List<BaseVacationsBLL> VacationsBLLList = new List<BaseVacationsBLL>();
            List<Vacations> VacationsList = new VacationsDAL().GetValidByEmployeeCodeIDVacationTypeID(EmployeeCodeID, (int)VacationType).ToList();
            foreach (var Vacation in VacationsList)
                VacationsBLLList.Add(this.MapVacation(Vacation));

            return VacationsBLLList;
        }

        public List<EmployeesVacationsDuringPeriodDTO> GetValidEmployeesVacationsDuringPeriod(VacationsTypesEnum? VacationType, DateTime StartDate, DateTime EndDate)
        {
            List<EmployeesVacationsDuringPeriodDTO> VacationsBLLList = new List<EmployeesVacationsDuringPeriodDTO>();
            List<Vacations> vacations = new VacationsDAL().GetValidByVacationTypeIDDuraingPeriod((int?)VacationType, StartDate, EndDate);

            foreach (var vacation in vacations)
            {
                var EmployeeCareerHistory = vacation.EmployeesCareersHistory;
                var EmployeeCode = EmployeeCareerHistory.EmployeesCodes;
                var Employee = EmployeeCode.Employees;
                var OrganizationJob = EmployeeCareerHistory.OrganizationsJobs;
                var VacationStartDate = vacation.VacationStartDate;
                var VacationEndDate = vacation.VacationEndDate;
                var VacationTypeName = vacation.VacationsTypes.VacationTypeName;
                VacationsBLLList.Add(new EmployeesVacationsDuringPeriodDTO(
                    EmployeeCode.EmployeeCodeNo,
                    Employee.FirstNameAr + ' ' + Employee.MiddleNameAr + ' ' + Employee.GrandFatherNameAr + ' ' + Employee.LastNameAr,
                    Employee.EmployeeIDNo,
                    OrganizationJob.OrganizationsStructures.OrganizationName,
                    vacation.VacationStartDate,
                     vacation.VacationEndDate,
                     vacation.VacationsTypes.VacationTypeName
                    ));
            }
            return VacationsBLLList;
        }

        public virtual List<BaseVacationsBLL> GetLastNVacationsEmployeeCodeID(int EmployeeCodeID)
        {
            List<BaseVacationsBLL> VacationsBLLList = new List<BaseVacationsBLL>();
            List<Vacations> VacationsList = new VacationsDAL().GetLastNVacationsEmployeeCodeID(EmployeeCodeID, this.LastVacationCount).ToList();

            VacationsList.ForEach(x => VacationsBLLList.Add(this.MapVacation(x)));

            return VacationsBLLList;
        }

        public bool IsEmployeeVacationExistsBeforeDate(int EmployeeCodeID, DateTime date)
        {
            bool IsVacationExists = new VacationsDAL().GetEmployeeVacations(EmployeeCodeID).Where(x => x.VacationStartDate < date.Date).Any();
            if (IsVacationExists)
                return true;

            return false;
        }

        internal VacationsDetails GetVacationDetailInstance(VacationsActionsEnum VacationsAction)
        {
            return new VacationsDetails()
            {
                VacationID = this.VacationID,
                FromDate = this.VacationStartDate,
                ToDate = this.VacationEndDate,
                IsApproved = false,
                VacationActionID = (int)VacationsAction,
                Notes = this.Notes,
                MainframeNo = this.MainframeNo,
                CreatedDate = DateTime.Now,
                CreatedBy = this.LoginIdentity.EmployeeCodeID
            };
        }

        public bool IsVacationApproved(int VacationID)
        {
            bool IsAnyNotApproved = new VacationsDetailsBLL().GetVacationsDetailsByVacationID(VacationID).Where(x => x.IsApproved == false).Any();
            if (IsAnyNotApproved)
                return false;

            return true;
        }

        private bool IsVacationCanceled(int VacationID)
        {
            bool IsCanceled = this.GetByVacationID(VacationID).IsCanceled;
            if (IsCanceled)
                return true;

            return false;
        }

        public Result Approve()
        {
            VacationsDetailsBLL VacationDetailBLL = new VacationsDetailsBLL().GetVacationsDetailsByVacationID(this.VacationID).OrderBy(x => x.VacationDetailID).Where(x => x.IsApproved == false).LastOrDefault();

            new VacationsDetailsDAL().Update(new VacationsDetails()
            {
                VacationDetailID = VacationDetailBLL.VacationDetailID,
                IsApproved = true,
                ApprovedDate = DateTime.Now,
                ApprovedBy = this.LoginIdentity.EmployeeCodeID,
                LastUpdatedDate = DateTime.Now,
                LastUpdatedBy = this.LoginIdentity.EmployeeCodeID,
            });

            //return vacation info after updating
            VacationDetailBLL = new VacationsDetailsBLL().GetVacationsDetailsByVacationID(this.VacationID).OrderBy(x => x.VacationDetailID).LastOrDefault();

            SendSMS(VacationDetailBLL);

            Result result = new Result();
            result.EnumType = typeof(VacationsValidationEnum);
            result.EnumMember = VacationsValidationEnum.Done.ToString();
            return result;
        }

        public Result ApproveCancel()
        {
            VacationsDetailsBLL VacationDetailBLL = new VacationsDetailsBLL().GetVacationsDetailsByVacationID(this.VacationID).OrderBy(x => x.VacationDetailID).LastOrDefault();
            new VacationsDetailsDAL().Update(new VacationsDetails()
            {
                VacationDetailID = VacationDetailBLL.VacationDetailID,
                IsApproved = false,
                ApprovedBy = null,
                ApprovedDate = null,
                LastUpdatedBy = this.LoginIdentity.EmployeeCodeID,
                LastUpdatedDate = DateTime.Now
            });

            //return vacation info after updating
            VacationDetailBLL = new VacationsDetailsBLL().GetVacationsDetailsByVacationID(this.VacationID).OrderBy(x => x.VacationDetailID).LastOrDefault();

            SendSMS(VacationDetailBLL);

            Result result = new Result();
            result.EnumType = typeof(VacationsValidationEnum);
            result.EnumMember = VacationsValidationEnum.Done.ToString();
            return result;
        }

        private Result IsValid()
        {
            Result result = null;

            #region validate the previous action for vacation has been approved or not
            if (this.IsVacationApproved(this.VacationID) == false)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(VacationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.RejectedBeacuseOfPreviousNotApproved.ToString();
                return result;
            }
            #endregion

            #region  validate the vacation is canceled before or not
            if (this.IsVacationCanceled(this.VacationID) == true)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(VacationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.RejectedBecauseOfAlreadyCanceled.ToString();
                return result;
            }
            #endregion

            return result;
        }

        private void SendSMS(VacationsDetailsBLL VacationDetail)
        {
            SMSLogsBLL SMSLog = new SMSLogsBLL()
            {
                CreatedDate = DateTime.Now,
                CreatedBy = new EmployeesCodesBLL()
                {
                    EmployeeCodeID = VacationDetail.LastUpdatedBy.EmployeeCodeID
                },
                BusinssSubCategory = BusinessSubCategoriesEnum.Vacations,
                DetailID = VacationDetail.Vacation.VacationID,
                MobileNo = VacationDetail.Vacation.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeMobileNo,
            };

            if (VacationDetail.IsApproved)
                SMSLog.Message = string.Format(Globalization.SMSVacationApprovalMessageText, VacationDetail.VacationAction.VacationActionName, VacationDetail.Vacation.VacationTypeName, VacationDetail.Vacation.VacationStartDate.ToShortDateString(), VacationDetail.Vacation.VacationPeriod);

            else if (!VacationDetail.IsApproved)
                SMSLog.Message = string.Format(Globalization.SMSVacationApprovalCancelMessageText, VacationDetail.VacationAction.VacationActionName, VacationDetail.Vacation.VacationTypeName, VacationDetail.Vacation.VacationStartDate.ToShortDateString(), VacationDetail.Vacation.VacationPeriod);

            new SMSBLL().SendSMS(SMSLog);
        }

        internal BaseVacationsBLL CustomMapVacation(Vacations Vacation)
        {
            try
            {
                BaseVacationsBLL BaseVacationBLL = new BaseVacationsBLL();
                BaseVacationBLL.EmployeeCareerHistory = new EmployeesCareersHistoryBLL();
                BaseVacationBLL.EmployeeCareerHistory.EmployeeCode = new EmployeesCodesBLL();
                BaseVacationBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo = Vacation.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo;
                BaseVacationBLL.EmployeeCareerHistory.EmployeeCode.Employee = new EmployeesBLL();
                BaseVacationBLL.EmployeeCareerHistory.EmployeeCode.Employee.FirstNameAr = Vacation.EmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr;
                BaseVacationBLL.EmployeeCareerHistory.EmployeeCode.Employee.MiddleNameAr = Vacation.EmployeesCareersHistory.EmployeesCodes.Employees.MiddleNameAr;
                BaseVacationBLL.EmployeeCareerHistory.EmployeeCode.Employee.GrandFatherNameAr = Vacation.EmployeesCareersHistory.EmployeesCodes.Employees.GrandFatherNameAr;
                BaseVacationBLL.EmployeeCareerHistory.EmployeeCode.Employee.LastNameAr = Vacation.EmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr;
                BaseVacationBLL.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeIDNo = Vacation.EmployeesCareersHistory.EmployeesCodes.Employees.EmployeeIDNo;
                BaseVacationBLL.EmployeeCareerHistory.OrganizationJob = new OrganizationsJobsBLL();
                BaseVacationBLL.EmployeeCareerHistory.OrganizationJob.OrganizationStructure = new OrganizationsStructuresBLL();
                BaseVacationBLL.EmployeeCareerHistory.OrganizationJob.OrganizationStructure.OrganizationName = Vacation.EmployeesCareersHistory.OrganizationsJobs.OrganizationsStructures.OrganizationName;
                BaseVacationBLL.VacationStartDate = Vacation.VacationStartDate;
                BaseVacationBLL.VacationEndDate = Vacation.VacationEndDate;
                BaseVacationBLL.VacationTypeName = Vacation.VacationsTypes.VacationTypeName;
                return BaseVacationBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal virtual float GetVacationConsumedByEmployeeCodeID(int EmployeeCodeID, int ExceptVacationID = 0)
        {
            //List<MotherHoodVacationsBLL> MotherHoodVacations = this.GetByEmployeeCodeVacationTypeNotCanceled(EmployeeCodeID, ExceptVacationID).Select(x => x as this.VacationType).ToList();
            //return MotherHoodVacations.Sum(x => x.VacationPeriod);
            return 0;
        }

        internal virtual Result IsBalanceValid(int ExceptVacationID = 0)
        {
            Result result = null;
            this.VacationConsumedBalance = this.GetVacationConsumedByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID, ExceptVacationID);
            if ((this.VacationConsumedBalance + this.VacationPeriod) > this.VacationBalance)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(VacationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString();
            }

            return result;
        }

        public void GetVacationBalance()
        {
            this.VacationConsumedBalance = this.GetVacationConsumedByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
        }

        public List<BaseVacationsBLL> GetVacationsBySportsSeasonID(int SportsSeasonID)
        {
            List<BaseVacationsBLL> VacationsBLLList = new List<BaseVacationsBLL>();
            List<Vacations> VacationsList = new VacationsDAL().GetVacationsBySportsSeasonID(SportsSeasonID).ToList();
            foreach (var Vacation in VacationsList)
                VacationsBLLList.Add(this.MapVacation(Vacation));

            return VacationsBLLList;
        }

        internal Result IsNormalVacationExistsAndConsumedMaxLimit()
        {
            try
            {
                Result result = null;
                NormalVacationsBLL NormalVacation = new NormalVacationsBLL().GetVacationBalances(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                    Globals.Calendar.GetUmAlQuraYear(this.VacationStartDate),
                    Globals.Calendar.GetUmAlQuraMonth(this.VacationStartDate),
                    Globals.Calendar.GetUmAlQuraDay(this.VacationStartDate),
                    0);
                if (NormalVacation.TotalAvailableVacationBalance > 0)
                {
                    NormalVacation.VacationStartDate = this.VacationStartDate;
                    NormalVacation.VacationEndDate = this.VacationEndDate;
                    NormalVacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetHiringRecordByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
                    // we should validate about the employee Reached To normal vacation Consumed Max Limit , if he reached to max limit , he can get exceptional vacation
                    if (!NormalVacation.IsReachedToConsumedMaxLimitForOtherVacationTypes())
                    {
                        result = new Result();
                        result.Entity = null;
                        result.EnumType = typeof(VacationsValidationEnum);
                        result.EnumMember = VacationsValidationEnum.RejectedBecauseOfNormalVacationBalanceExists.ToString();
                        return result;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<EmployeesVacationsBasedOnAssigngingsDTO> GetEmployeesVacationsBasedOnAssigningsAsRanksCategories(int VacationTypeID, DateTime VacationDate, int OrganizationID)
        {
            try
            {
                List<int> OrganizationIDsList = new OrganizationsStructuresBLL().GetByOrganizationIDsWithhAllChilds(OrganizationID);
                DateTime VacationDateGr = Convert.ToDateTime(Globals.Calendar.UmAlquraToGreg(string.Format("{0}/{1}/{2}", VacationDate.Day, VacationDate.Month, VacationDate.Year)), new CultureInfo("en-US"));
                List<vwActualEmployeesBasedOnAssignings> ActualEmployeesHaveVacations = new List<vwActualEmployeesBasedOnAssignings>();

                // Get actual employees Based On Assignings by date
                List<vwActualEmployeesBasedOnAssignings> ActualEmployeesBasedOnAssignings = new AssigningsDAL().GetActualEmployeeBasedOnAssignings().Where(x => (VacationDateGr.Date >= x.AssigningStartDate.Date && VacationDateGr.Date <= (!x.AssigningEndDate.HasValue ? DateTime.Now.Date : x.AssigningEndDate.Value.Date))
                                                                                                                                                                 && OrganizationIDsList.Contains(x.OrganizationID.Value)).ToList();

                List<int> EmployeeCareerHistoryIDs = new List<int>();
                ActualEmployeesBasedOnAssignings.ForEach(x => EmployeeCareerHistoryIDs.Add(x.EmployeeCareerHistoryID.Value));

                List<Vacations> EmployeesVacationsOfActualEmployeesList = new VacationsDAL().GetEmployeesVacationsByDate(VacationDateGr, VacationTypeID, EmployeeCareerHistoryIDs);

                var query = EmployeesVacationsOfActualEmployeesList.Select(y => new EmployeesVacationsBasedOnAssigngingsDTO(y.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo,
                                                                         ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).EmployeeNameAr,
                                                                         ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).OrganizationName,
                                                                         ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).JobName,
                                                                         ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).RankCategoryName,
                                                                         ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).RankName,
                                                                         y.VacationStartDate,
                                                                         y.VacationEndDate,
                                                                         y.VacationsTypes.VacationTypeName,
                                                                         ActualEmployeesBasedOnAssignings.FirstOrDefault(x => x.EmployeeCodeID == y.EmployeesCareersHistory.EmployeeCodeID).Sorting
                                                                         ));

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BaseVacationsBLL> GetEmployeesHaveVacationsBetweenTwoDates(int OrganizationID, DateTime VacationStartDate, DateTime VacationEndDate)
        {
            try
            {
                List<BaseVacationsBLL> VacationsBLLList = new List<BaseVacationsBLL>();

                List<InternalAssigningBLL> Assignings = new InternalAssigningBLL().GetActiveEmployeesAssigingByOrganization(OrganizationID);
                List<int> EmployeeCareeseHistoryIDs = new List<int>();
                Assignings.ForEach(x => EmployeeCareeseHistoryIDs.Add(x.EmployeeCareerHistory.EmployeeCareerHistoryID));

                List<Vacations> VacationsList = new VacationsDAL().GetEmployeesVacationsByTwoDates(EmployeeCareeseHistoryIDs, VacationStartDate, VacationEndDate).ToList();
                VacationsList.ForEach(x => VacationsBLLList.Add(this.MapVacation(x)));

                return VacationsBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<BaseVacationsBLL> GetEmployeeVacationsBetweenTwoDates(int EmployeeCodeID, DateTime VacationStartDate, DateTime VacationEndDate)
        {
            try
            {
                List<BaseVacationsBLL> VacationsBLLList = new List<BaseVacationsBLL>();
                List<Vacations> VacationsList = new VacationsDAL().GetEmployeeVacationsBetweenTwoDates(EmployeeCodeID, VacationStartDate, VacationEndDate).ToList();
                VacationsList.ForEach(x => VacationsBLLList.Add(this.MapVacation(x)));

                return VacationsBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal BaseVacationsBLL MapVacation(Vacations Vacation)
        {
            try
            {
                BaseVacationsBLL BaseVacationBLL = null;
                if (Vacation != null)
                {
                    if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Exceptional))
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Sick))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, SickVacationsBLL>.CreateInstance();
                        ((SickVacationsBLL)BaseVacationBLL).SickVacationType = Vacation.SickVacationTypeID.HasValue ? (SickVacationsTypesEnum)Vacation.SickVacationTypeID : SickVacationsTypesEnum.Normal;
                        ((SickVacationsBLL)BaseVacationBLL).IsSickLeaveAmountPaid = Vacation.IsSickLeaveAmountPaid.HasValue ? Vacation.IsSickLeaveAmountPaid.Value : false;
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.AccompanimentSick))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, AccompanimentSickVacationsBLL>.CreateInstance();
                        ((AccompanimentSickVacationsBLL)BaseVacationBLL).SickVacationType = Vacation.SickVacationTypeID.HasValue ? (SickVacationsTypesEnum)Vacation.SickVacationTypeID : SickVacationsTypesEnum.Normal;
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Accompaniment))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, AccompanimentVacationsBLL>.CreateInstance();
                        ((AccompanimentVacationsBLL)BaseVacationBLL).SickVacationType = Vacation.SickVacationTypeID.HasValue ? (SickVacationsTypesEnum)Vacation.SickVacationTypeID : SickVacationsTypesEnum.Normal;
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Sports))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, SportsVacationsBLL>.CreateInstance();
                        //((SportsVacationsBLL)BaseVacationBLL).SportsSeasonID = Vacation.SportsSeasonID;
                        ((SportsVacationsBLL)BaseVacationBLL).SportsSeason = Vacation.SportsSeasonID != null ? new SportsSeasonsBLL().GetBySportsSeasonID((int)Vacation.SportsSeasonID) : null;
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Born))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, BornVacationsBLL>.CreateInstance();
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Dead))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, DeadVacationsBLL>.CreateInstance();
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.MotherHood))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, MotherHoodVacationsBLL>.CreateInstance();
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Birth))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, BirthVacationsBLL>.CreateInstance();
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Studies))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, StudiesVacationsBLL>.CreateInstance();
                        ((StudiesVacationsBLL)BaseVacationBLL).StudiesVacationStartDate = Vacation.StudiesVacationStartDate;
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Normal))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                        ((NormalVacationsBLL)BaseVacationBLL).ConsumedOldBalance = Vacation.OldBalanceConsumed.HasValue ? Vacation.OldBalanceConsumed.Value : 0;
                        ((NormalVacationsBLL)BaseVacationBLL).NormalVacationType = Vacation.NormalVacationsTypes != null ? new NormalVacationsTypesBLL().MapNormalVacationsTypes(Vacation.NormalVacationsTypes) : null;
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Compensatory))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, CompensatoryVacationsBLL>.CreateInstance();
                        ((CompensatoryVacationsBLL)BaseVacationBLL).HolidayAttendanceDetail = Vacation.HolidayAttendanceDetailID != null ? new HolidaysAttendanceDetailsBLL().GetByHolidayAttendanceDetailID((int)Vacation.HolidayAttendanceDetailID) : null;
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Emergency))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, EmergencyVacationsBLL>.CreateInstance();
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Marriage))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, MarriageVacationsBLL>.CreateInstance();
                    }
                    else if (Vacation.VacationTypeID == Convert.ToInt16(VacationsTypesEnum.Exam))
                    {
                        BaseVacationBLL = GenericFactoryPattern<BaseVacationsBLL, ExamVacationsBLL>.CreateInstance();
                    }
                    else
                        BaseVacationBLL = new BaseVacationsBLL();

                    BaseVacationBLL.VacationID = Vacation.VacationID;
                    BaseVacationBLL.VacationType = (VacationsTypesEnum)Vacation.VacationTypeID;
                    BaseVacationBLL.VacationTypeName = Vacation.VacationsTypes.VacationTypeName;
                    BaseVacationBLL.VacationStartDate = Vacation.VacationStartDate;
                    BaseVacationBLL.VacationEndDate = Vacation.VacationEndDate;
                    BaseVacationBLL.IsCanceled = Vacation.IsCanceled;
                    BaseVacationBLL.EmployeeCareerHistory = Vacation.EmployeesCareersHistory != null ? new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(Vacation.EmployeesCareersHistory) : null;
                    BaseVacationBLL.HasDetailsWithoutCreationPermission = Vacation.VacationsDetails != null ? Vacation.VacationsDetails.Count > 1 ? true : false : false;
                    BaseVacationBLL.CreatedDate = Vacation.CreatedDate;
                    BaseVacationBLL.CreatedBy = Vacation.CreatedByNav != null ? new EmployeesCodesBLL().MapEmployeeCode(Vacation.CreatedByNav) : null;
                    BaseVacationBLL.LastUpdatedDate = Vacation.LastUpdatedDate;
                    BaseVacationBLL.LastUpdatedBy = Vacation.LastUpdatedByNav != null ? new EmployeesCodesBLL().MapEmployeeCode(Vacation.LastUpdatedByNav) : null;
                    BaseVacationBLL.NormalVacationsType = Vacation.NormalVacationsTypes != null ? new NormalVacationsTypesBLL().MapNormalVacationsTypes(Vacation.NormalVacationsTypes) : null;
                }
                return BaseVacationBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

