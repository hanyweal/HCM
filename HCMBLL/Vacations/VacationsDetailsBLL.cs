using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class VacationsDetailsBLL : CommonEntity
    {
        public virtual BaseVacationsBLL Vacation { get; set; }

        public virtual int VacationDetailID { get; set; }

        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }

        public virtual int VacationPeriod
        {
            get
            {
                return this.ToDate.Subtract(this.FromDate).Days + 1;
            }
        }

        public virtual string Notes { get; set; }

        public virtual string MainframeNo { get; set; }

        public virtual bool IsApproved { get; set; }

        public virtual EmployeesCodesBLL ApprovedBy { get; set; }

        public virtual DateTime? ApprovedDate { get; set; }

        public virtual VacationsActionsBLL VacationAction { get; set; }

        public List<VacationsDetailsBLL> GetVacationsDetailsByVacationID(int VacationID)
        {
            List<VacationsDetailsBLL> VacationsDetailsBLLList = new List<VacationsDetailsBLL>();
            List<VacationsDetails> VacationsDetailsList = new VacationsDetailsDAL().GetByVacationID(VacationID);
            foreach (var item in VacationsDetailsList)
            {
                VacationsDetailsBLLList.Add(new VacationsDetailsBLL().MapVacationDetail(item));
            }
            return VacationsDetailsBLLList;
        }

        public VacationsDetailsBLL GetVacationDetailByVacationIDActionID(int VacationID, int VacationActionID)
        {
            return new VacationsDetailsBLL().MapVacationDetail(new VacationsDetailsDAL().GetVacationsDetailsByVacationID(VacationID, VacationActionID));
        }

        public VacationsDetailsBLL GetVacationsDetailsByVacationDetailID(int VacationDetailID)
        {
            VacationsDetailsBLL VacationDetailBLL = new VacationsDetailsBLL();
            VacationsDetails VacationDetail = new VacationsDetailsDAL().GetByVacationDetailID(VacationDetailID);
            return new VacationsDetailsBLL().MapVacationDetail(VacationDetail);
        }

        public Result Remove(int VacationDetailID)
        {
            Result result = null;
            VacationsDetailsBLL VacationDetail = this.GetVacationsDetailsByVacationDetailID(VacationDetailID);
            #region validate if this action approved or not
            if (this.IsVacationDetailApproved(VacationDetailID))
            {
                result = new Result();
                result.EnumType = typeof(VacationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.RejectedBeacuseOfAlreadyApproved.ToString();
                return result;
            }
            #endregion

            #region Validate if this action is cancelling or not
            if (VacationDetail.VacationAction.VacationActionID == (int)VacationsActionsEnum.Cancel)
            {
                result = new Result();
                result.EnumType = typeof(VacationsValidationEnum);
                result.EnumMember = VacationsValidationEnum.RejectedBeacuseOfNoChanceCancelCancelling.ToString();
                return result;
            }
            #endregion

            #region Delete vacation detail and update the main vacation data in HCM App And Time Attendance App

            //delete the vacation detail
            new VacationsDetailsDAL().Delete(VacationDetailID, this.LoginIdentity.EmployeeCodeID);

            // get last vacation detail to update the the main data of the vacation
            VacationsDetailsBLL FirstVacationDetail = new VacationsDetailsBLL().GetVacationsDetailsByVacationID(VacationDetail.Vacation.VacationID).FirstOrDefault();
            VacationsDetailsBLL LastVacationDetail = new VacationsDetailsBLL().GetVacationsDetailsByVacationID(VacationDetail.Vacation.VacationID).LastOrDefault();
            if (LastVacationDetail == null) // thats mean no action more in details (created record was deleted) in this case we are going to delete main vacation data from HCM App and delete vacation also from Time Attendance App
            {
                new BaseVacationsBLL()
                {
                    VacationID = VacationDetail.Vacation.VacationID,
                    LoginIdentity = this.LoginIdentity
                }.Remove();

                new TimeAttendanceBLL().DeleteEmployeeVacation(VacationDetail.Vacation.VacationID, this.LoginIdentity.EmployeeCodeNo);
            }
            else
            {
                new VacationsDAL().UpdateVacationDates(new Vacations()
                {
                    VacationID = LastVacationDetail.Vacation.VacationID,
                    VacationStartDate = FirstVacationDetail.FromDate,
                    VacationEndDate = LastVacationDetail.ToDate,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                });

                new TimeAttendanceBLL().EditEmployeeVacation(VacationDetail.Vacation.VacationID, FirstVacationDetail.FromDate, LastVacationDetail.ToDate, this.LoginIdentity.EmployeeCodeNo);
            }

            result = new Result();
            result.EnumType = typeof(VacationsValidationEnum);
            result.EnumMember = VacationsValidationEnum.Done.ToString();
            return result;

            #endregion
        }

        internal bool IsVacationDetailApproved(int VacationDetailID)
        {
            bool IsApproved = this.GetVacationsDetailsByVacationDetailID(VacationDetailID).IsApproved;
            if (IsApproved)
                return true;

            return false;
        }

        internal VacationsDetailsBLL MapVacationDetail(VacationsDetails VacationDetail)
        {
            try
            {
                VacationsDetailsBLL VacationDetailBLL = null;
                if (VacationDetail != null)
                {
                    VacationDetailBLL = new VacationsDetailsBLL()
                    {
                        Vacation = VacationDetail.Vacations != null ? new BaseVacationsBLL().MapVacation(VacationDetail.Vacations) : null,
                        VacationDetailID = VacationDetail.VacationDetailID,
                        FromDate = VacationDetail.FromDate,
                        ToDate = VacationDetail.ToDate,
                        IsApproved = VacationDetail.IsApproved,
                        ApprovedDate = VacationDetail.ApprovedDate,
                        ApprovedBy = VacationDetail.ApprovedByNav != null ? new EmployeesCodesBLL().MapEmployeeCode(VacationDetail.ApprovedByNav) : new EmployeesCodesBLL() { Employee = new EmployeesBLL() },
                        VacationAction = new VacationsActionsBLL()
                        {
                            VacationActionID = VacationDetail.VacationActionID,
                            VacationActionName = VacationDetail.VacationsActions.VacationActionName
                        },
                        Notes = VacationDetail.Notes,
                        MainframeNo = VacationDetail.MainframeNo,
                        CreatedDate = VacationDetail.CreatedDate,
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(VacationDetail.CreatedByNav),
                        LastUpdatedDate = VacationDetail.LastUpdatedDate,
                        LastUpdatedBy = new EmployeesCodesBLL().MapEmployeeCode(VacationDetail.LastUpdatedByNav),
                    };
                }
                return VacationDetailBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}