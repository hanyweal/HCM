using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class ApprovalsController : BaseController
    {
        [CustomAuthorize]
        [HttpGet]
        public ActionResult DelegationApproval()
        {
            //return View(this.GetByDelegationID(1));
            return View();
        }

        [HttpPost]
        [IgnoreModelProperties("DelegationStartDate,DelegationEndDate,DelegationPeriod,DelegationReason,DelegationType,DelegationDistancePeriod,DelegationDetailRequest")]
        public ActionResult DelegationApproval(DelegationsViewModel DelegationVM, string BtnType)
        {
            Result result = null;
            ExpensesBLL expense = new BaseDelegationsBLL();
            expense.ApprovedBy = UserIdentity.EmployeeCodeID;
            if (BtnType == "Approve")
                result = expense.Approve(DelegationVM.DelegationID);
            else
                result = expense.ApproveCancel(DelegationVM.DelegationID);
            if ((System.Type)result.EnumType == typeof(DelegationsValidationEnum))
            {
                if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfAlreadyApprove.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationDelegationAlreadyApproveText);
                }
                if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfAlreadyApproveCancel.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationDelegationAlreadyApproveCancelText);
                }
            }
            return View();
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult OverTimeApproval()
        {
            return View();
        }

        [HttpPost]
        [IgnoreModelProperties("OverTimeStartDate,OverTimeEndDate,OverTimePeriod,OverTimeReason,OverTimeType,OverTimeDistancePeriod,OverTimeDetailRequest,Tasks,OverTimesDays")]
        public ActionResult OverTimeApproval(OverTimesViewModel OverTimeVM, string BtnType)
        {
            Result result = null;
            ExpensesBLL expense = new OverTimesBLL();
            expense.ApprovedBy = UserIdentity.EmployeeCodeID;
            if (BtnType == "Approve")
                result = expense.Approve(OverTimeVM.OverTimeID);
            else
                result = expense.ApproveCancel(OverTimeVM.OverTimeID);

            if (result.EnumMember == OverTimeValidationEnum.RejectedBecauseOfAlreadyApprove.ToString())
                throw new CustomException(Resources.Globalization.ValidationOverTimeAlreadyApproveText);

            if (result.EnumMember == OverTimeValidationEnum.RejectedBecauseOfAlreadyApproveCancel.ToString())
                throw new CustomException(Resources.Globalization.ValidationOverTimeAlreadyApproveCancelText);

            return View();
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult VacationApproval()
        {
            return View();
        }

        [HttpPost]
        [Route("{controller}/VacationApproval/{VacationID}/{ApprovalType}")]
        public ActionResult VacationApproval(string VacationID, string ApprovalType)
        {
            BaseVacationsBLL Vacation = new BaseVacationsBLL()
            {
                VacationID = int.Parse(VacationID),
                LoginIdentity = this.UserIdentity
            };
            Result result = new Result();
            if (ApprovalType == "Approve")
                result = Vacation.Approve();
            else
                result = Vacation.ApproveCancel();

            return View();
        }

        [NonAction]
        private DelegationsViewModel GetByDelegationID(int id)
        {
            BaseDelegationsBLL DelegationBLL = new BaseDelegationsBLL().GetByDelegationID(id);
            DelegationsViewModel DelegationVM = new DelegationsViewModel();
            if (DelegationBLL != null)
            {
                if (DelegationBLL.DelegationType.DelegationTypeID == (Int32)DelegationsTypesEnum.Internal)
                {
                    DelegationVM.KSARegion = ((InternalDelegationBLL)DelegationBLL).KSACity.KSARegion;
                    DelegationVM.KSACity = ((InternalDelegationBLL)DelegationBLL).KSACity;
                }
                else if (DelegationBLL.DelegationType.DelegationTypeID == (Int32)DelegationsTypesEnum.External)
                {
                    DelegationVM.Country = ((ExternalDelegationBLL)DelegationBLL).Country;
                }

                DelegationVM.DelegationKind = new DelegationsKindsBLL() { DelegationKindID = DelegationBLL.DelegationKind.DelegationKindID, DelegationKindName = DelegationBLL.DelegationKind.DelegationKindName };
                DelegationVM.DelegationType = new DelegationsTypesBLL() { DelegationTypeID = DelegationBLL.DelegationType.DelegationTypeID, DelegationTypeName = DelegationBLL.DelegationType.DelegationTypeName };
                DelegationVM.DelegationID = DelegationBLL.DelegationID;
                DelegationVM.DelegationStartDate = DelegationBLL.DelegationStartDate.Date;
                DelegationVM.DelegationEndDate = DelegationBLL.DelegationEndDate;
                DelegationVM.DelegationReason = DelegationBLL.DelegationReason;
                DelegationVM.DelegationPeriod = DelegationBLL.DelegationPeriod;
                DelegationVM.DelegationDistancePeriod = DelegationBLL.DelegationDistancePeriod;
                DelegationVM.CreatedDate = DelegationBLL.CreatedDate;
                DelegationVM.CreatedBy = DelegationVM.GetCreatedByDisplayed(DelegationBLL.CreatedBy);
                DelegationVM.IsApprove = DelegationBLL.IsApproved;
            }
            return DelegationVM;
        }

        [NonAction]
        private OverTimesViewModel GetByOverTimeID(int id)
        {
            OverTimesBLL OverTimeBLL = new OverTimesBLL().GetByOverTimeID(id);
            OverTimesViewModel OverTimeVM = new OverTimesViewModel();
            if (OverTimeBLL != null)
            {
                OverTimeVM.OverTimeID = OverTimeBLL.OverTimeID;
                OverTimeVM.OverTimeStartDate = OverTimeBLL.OverTimeStartDate.Date;
                OverTimeVM.OverTimeEndDate = OverTimeBLL.OverTimeEndDate;
                OverTimeVM.Tasks = OverTimeBLL.Tasks;
                OverTimeVM.WeekWorkHoursAvg = OverTimeBLL.WeekWorkHoursAvg;
                OverTimeVM.FridayHoursAvg = OverTimeBLL.FridayHoursAvg;
                OverTimeVM.SaturdayHoursAvg = OverTimeBLL.SaturdayHoursAvg;
                OverTimeVM.OverTimePeriod = OverTimeBLL.OverTimePeriod;
                OverTimeVM.Requester = OverTimeBLL.Requester;
                OverTimeVM.CreatedDate = OverTimeBLL.CreatedDate;
                OverTimeVM.CreatedBy = OverTimeVM.GetCreatedByDisplayed(OverTimeBLL.CreatedBy);
            }
            return OverTimeVM;
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult PromotionRecordEmployeeApproval()
        {
            return View();
        }

        [HttpPost]
        [Route("{controller}/PromotionRecordEmployeeApproval/{PromotionRecordEmployeeID}/{ApprovalType}")]
        public ActionResult PromotionRecordEmployeeApproval(string PromotionRecordEmployeeID, string ApprovalType)
        {
            PromotionsRecordsEmployeesBLL PromotionRecordEmployee = new PromotionsRecordsEmployeesBLL()
            {
                PromotionRecordEmployeeID = int.Parse(PromotionRecordEmployeeID),
                LoginIdentity = this.UserIdentity
            };
            Result result = new Result();
            if (ApprovalType == "Approve")
                result = PromotionRecordEmployee.Approve();
            else
                result = PromotionRecordEmployee.ApproveCancel();

            return View();
            //return Json(new { data = PromotionRecordEmployee.IsApproved }, JsonRequestBehavior.AllowGet);
        }
    }
}