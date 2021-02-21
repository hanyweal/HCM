using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class EVacationsRequestsController : BaseEServicesController
    {
        #region Employee ActionResults

        public ActionResult MyEVacationsRequests()
        {
            return View();
        }

        public ActionResult Create()
        {
            EmployeesCodesBLL AuthorizedPersonBLL = new EmployeesCodesBLL().GetEVacationAuthorizedPersonOfEmployee(WindowsUserIdentity, EServicesTypesEnum.Vacation);
            EmployeesViewModel AuthorizedPerson = AuthorizedPersonBLL != null ? new EmployeesViewModel() { EmployeeCodeNo = AuthorizedPersonBLL?.EmployeeCodeNo, EmployeeNameAr = AuthorizedPersonBLL?.Employee?.EmployeeNameAr } : null;
            VacationsViewModel VacationVM = new VacationsViewModel()
            {
                VacationType = new VacationsTypesBLL(),
                EmployeeCodeNo = WindowsUserIdentity,
                EVacationAuthorizedPerson = AuthorizedPerson
            };
            return View(VacationVM);
        }

        [HttpPost]
        [IgnoreModelProperties("HolidayTypeID,MaturityYearID,SportsSeasonID,SportsSeasonMaturityYearID")]
        public ActionResult Create(VacationsViewModel VacationVM)
        {
            BaseVacationsBLL Vacation = null;

            if (!VacationVM.VacationStartDate.HasValue || !VacationVM.VacationEndDate.HasValue)
                throw new CustomException(Resources.Globalization.RequiredStartDateAndEndDateText);

            if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Normal)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                if (VacationVM.NormalVacationTypeID.HasValue)
                    ((NormalVacationsBLL)Vacation).NormalVacationType = new NormalVacationsTypesBLL()
                    {
                        NormalVacationTypeID = VacationVM.NormalVacationTypeID.Value
                    };
            }

            Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(VacationVM.EmployeeCodeID.Value);
            Vacation.VacationType = (VacationsTypesEnum)VacationVM.VacationType.VacationTypeID;
            Vacation.VacationStartDate = VacationVM.VacationStartDate.Value.Date;
            Vacation.VacationEndDate = VacationVM.VacationEndDate.Value.Date;
            Vacation.Notes = VacationVM.Notes;
            Vacation.IsApprovedFromManager = false;
            Vacation.LoginIdentity = new EmployeesCodesBLL()
            {
                EmployeeCodeNo = this.WindowsUserIdentity,
                EmployeeCodeID = VacationVM.EmployeeCodeID.Value
            };

            Result result = Vacation.Add();

            if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfBeforeHiringDate.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationVacationBeforeHiringText, ((EmployeesCareersHistoryBLL)result.Entity).JoinDate.ToString(Classes.Helpers.CommonHelper.DateFormat)));

            else if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfEmployeeNotHasActualOrganization.ToString())
                throw new CustomException(Resources.Globalization.ValidationEmployeeNotHasActualOrganizationText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfDuringProbation.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationVacationDuringProbationText, ((EmployeesCareersHistoryBLL)result.Entity).ProbationEndDate.ToString(Classes.Helpers.CommonHelper.DateFormat)));

            else if (result.EnumMember == VacationsValidationEnum.SportsSeasonDoesNotExist.ToString())
                throw new CustomException(Resources.Globalization.SportsSeasonDoesNotExistText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidDates.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidDateText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidSportsDates.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidSportsDateText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString())
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheVacationLimitText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidStartDate.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidVacationStartDateText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfConsumedMaxLimit.ToString())
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheNormalVacationLimitText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNormalVacationBalanceExists.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfNormalVacationBalanceExistsText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDays.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDaysText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfVacationNotAllowedBetween1437and1438.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfVacationNotAllowedBetween1437and1438Text);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfVacationOutOfRange.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfVacationOutOfRangeText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseSickExceptionalVacationDatesOutOfRange.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfSickExceptionalVacationDatesOutOfRangeText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString())
                throw new CustomException(Resources.Globalization.ErrorInTimeAttendanceAppText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfMarriageVacationAcceptedOneTime.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfMarriageVacationAcceptedOneTimeText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNoActiveProxyCreatedToOtherPerson.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfNoActiveProxyCreatedToOtherPersonText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEmployeeHasProxyByOtherPerson.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationBecauseOfThereIsProxyByOtherPersonText, ((EServicesProxiesBLL)result.Entity).FromEmployee?.Employee?.EmployeeNameAr,
                                                                                                                                  ((EServicesProxiesBLL)result.Entity).StartDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat)));

            else if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfNotAllowedWeekEndBetweenTwoVacations.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationNotAllowedWeekEndBetweenTwoVacationsText, ((BaseVacationsBLL)result.Entity).VacationStartDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat),
                                                                                                                                    ((BaseVacationsBLL)result.Entity).VacationPeriod,
                                                                                                                                    ((BaseVacationsBLL)result.Entity).VacationEndDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat),
                                                                                                                                    ((BaseVacationsBLL)result.Entity).VacationTypeName));

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEVacationRequestPendingExist.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationBecauseOfEVacationRequestPendingExistText, ((BaseVacationsBLL)result.Entity).EVacationsRequest.EVacationRequestNo));

            Classes.Helpers.CommonHelper.ConflictValidationMessage(result);

            Session["EVacationRequestNo"] = Vacation.EVacationsRequest.EVacationRequestNo;
            return Json(new { EVacationRequestNo = Vacation.EVacationsRequest.EVacationRequestNo }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EVacationRequestCreated()
        {
            if (Session["EVacationRequestNo"] != null)
                ViewBag.EVacationRequestNo = Session["EVacationRequestNo"].ToString();
            else
                ViewBag.EVacationRequestNo = 0;

            return View();
        }

        [HttpGet]
        [Route("{controller}/GetMyEVacationsRequests")]
        public JsonResult GetMyEVacationsRequests()
        {
            return Json(new
            {
                data = new EVacationsRequestsBLL().GetEVacationsRequestsByEmployeeCodeNo(this.WindowsUserIdentity).Select(x => new
                {
                    EVacationRequestID = x.EVacationRequestID,
                    EVacationRequestNo = x.EVacationRequestNo,
                    VacationType = x.VacationType.VacationTypeName,
                    VacationStartDate = x.VacationStartDate,
                    VacationPeriod = x.VacationPeriod,
                    VacationEndDate = x.VacationEndDate,
                    WorkDate = x.WorkDate,
                    EVacationRequestStatusID = x.EVacationRequestStatus?.EVacationRequestStatusID,
                    EVacationRequestStatus = x.EVacationRequestStatus?.EVacationRequestStatusName,
                    CreatedDate = x.CreatedDate
                }).OrderByDescending(x => x.CreatedDate)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailsByEmployee(int id)
        {
            EVacationsRequestsViewModel EVacationRequestVM = GetByEServiceRequestID(id);
            return View(EVacationRequestVM);
        }

        [HttpPost]
        public ActionResult CancelByEmployee(EVacationsRequestsViewModel EVacationRequestVM)
        {
            Result result = new EVacationsRequestsBLL().CancelEVacationRequest(EVacationRequestVM.EVacationRequestID, EVacationRequestStatusEnum.CancelledByCreator);
            if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEVacationRequestStatusNotPending.ToString())
                throw new CustomException(Resources.Globalization.ValidationEVacationRequestStatusNotPendingText);

            return RedirectToAction("GetMyEVacationsRequests");
        }

        #endregion

        #region HR ActionResults
        [CustomAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("{controller}/GetAllEVacationsRequests")]
        public JsonResult GetAllEVacationsRequests()
        {
            // List<EVacationsRequestsBLL> EVacationsRequestsBLLList = new EVacationsRequestsBLL().GetAllEVacationsRequests();
            var data = new EVacationsRequestsBLL().GetAllEVacationsRequests().Select(x => new
            {
                EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                OrganizationName = x.ActualEmployeeOrganization != null ? x.ActualEmployeeOrganization.GetOrganizationNameTillLastParentExceptPresident(x.ActualEmployeeOrganization.OrganizationID) : string.Empty,
                EVacationRequestID = x.EVacationRequestID,
                EVacationRequestNo = x.EVacationRequestNo,
                VacationType = x.VacationType.VacationTypeName,
                VacationStartDate = x.VacationStartDate,
                VacationPeriod = x.VacationPeriod,
                VacationEndDate = x.VacationEndDate,
                WorkDate = x.WorkDate,
                EVacationRequestStatus = x.EVacationRequestStatus.EVacationRequestStatusName,
                CreatedDate = x.CreatedDate
            });

            return new Classes.Helpers.CommonHelper().SetJsonResultWithMaxJsonLength(data);
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            EVacationsRequestsViewModel EVacationRequestVM = GetByEServiceRequestID(id);
            return View(EVacationRequestVM);
        }

        [HttpPost]
        public ActionResult Cancel(EVacationsRequestsViewModel EVacationRequestVM)
        {
            if (string.IsNullOrEmpty(EVacationRequestVM.CancellationReasonByHR?.Trim()))
                throw new CustomException(Resources.Globalization.RequiredEVacationRequestCancellationText);

            Result result = new EVacationsRequestsBLL() { LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) } }.CancelEVacationRequest(EVacationRequestVM.EVacationRequestID, EVacationRequestStatusEnum.CancelledByHR, EVacationRequestVM.CancellationReasonByHR);
            if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEVacationRequestStatusNotPending.ToString())
                throw new CustomException(Resources.Globalization.ValidationEVacationRequestStatusNotPendingText);

            return RedirectToAction("Index");
        }

        #endregion

        #region AuthorizedPerson ActionResults
        public ActionResult RequestsUnderAuthorizedPerson()
        {
            return View();
        }

        public JsonResult GetPendingRequestsUnderAuthorizedPerson()
        {
            return Json(new
            {
                data = new EVacationsRequestsBLL().GetPendingEVacationsRequestsByAuthorizedPerson(this.WindowsUserIdentity).Select(x => new
                {
                    EVacationRequestID = x.EVacationRequestID,
                    EVacationRequestNo = x.EVacationRequestNo,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    OrganizationName = x.ActualEmployeeOrganization != null ? x.ActualEmployeeOrganization.GetOrganizationNameTillLastParentExceptPresident(x.ActualEmployeeOrganization.OrganizationID) : string.Empty,
                    JobName = x.ActualEmployeeJob != null ? x.ActualEmployeeJob.JobName : string.Empty,
                    VacationType = x.VacationType.VacationTypeName,
                    VacationStartDate = x.VacationStartDate,
                    VacationPeriod = x.VacationPeriod,
                    VacationEndDate = x.VacationEndDate,
                    CreatedDate = x.CreatedDate,
                }).OrderByDescending(x => x.CreatedDate)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDoneRequestsUnderAuthorizedPerson()
        {
            return Json(new
            {
                data = new EVacationsRequestsBLL().GetDoneEVacationsRequestsByApproverCodeNo(this.WindowsUserIdentity).Select(x => new
                {
                    EVacationRequestID = x.EVacationRequestID,
                    EVacationRequestNo = x.EVacationRequestNo,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    OrganizationName = x.ActualEmployeeOrganization != null ? x.ActualEmployeeOrganization.GetOrganizationNameTillLastParentExceptPresident(x.ActualEmployeeOrganization.OrganizationID) : string.Empty,
                    JobName = x.ActualEmployeeJob != null ? x.ActualEmployeeJob.JobName : string.Empty,
                    VacationType = x.VacationType.VacationTypeName,
                    VacationStartDate = x.VacationStartDate,
                    VacationPeriod = x.VacationPeriod,
                    VacationEndDate = x.VacationEndDate,
                    EVacationRequestStatus = x.EVacationRequestStatus.EVacationRequestStatusName,
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AuthorizedPersonDecision(int id)
        {
            EVacationsRequestsViewModel EVacationRequestVM = GetByEServiceRequestID(id);
            return View(EVacationRequestVM);
        }

        [HttpPost]
        public ActionResult AuthorizedPersonDecision(EVacationsRequestsViewModel EVacationRequestVM)
        {
            Session["UserID"] = 1;
            EVacationsRequestsBLL EVacationRequestBLL = new EVacationsRequestsBLL()
            {
                EVacationRequestID = EVacationRequestVM.EVacationRequestID,
                ApprovedBy = new EmployeesCodesBLL() { EmployeeCodeNo = WindowsUserIdentity },
                ApproverNotes = EVacationRequestVM.AuthorizedPersonNotes,
            };

            if (Request.Form["btnApprove"] != null)
                EVacationRequestVM.EVacationRequestStatusEnum = EVacationRequestStatusEnum.Approved;

            if (Request.Form["btnRefuse"] != null)
                EVacationRequestVM.EVacationRequestStatusEnum = EVacationRequestStatusEnum.Refused;

            //EVacationRequestVM.EVacationRequestStatusEnum = (EVacationRequestStatusEnum)EVacationRequestVM.EVacationRequestStatus.EVacationRequestStatusID;

            Result result = EVacationRequestBLL.MakeDecisionOfEVacationRequest(EVacationRequestBLL, EVacationRequestVM.EVacationRequestStatusEnum);

            if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfBeforeHiringDate.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationVacationBeforeHiringText, ((EmployeesCareersHistoryBLL)result.Entity).JoinDate.ToString(Classes.Helpers.CommonHelper.DateFormat)));

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfDuringProbation.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationVacationDuringProbationText, ((EmployeesCareersHistoryBLL)result.Entity).ProbationEndDate.ToString(Classes.Helpers.CommonHelper.DateFormat)));

            else if (result.EnumMember == VacationsValidationEnum.SportsSeasonDoesNotExist.ToString())
                throw new CustomException(Resources.Globalization.SportsSeasonDoesNotExistText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidDates.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidDateText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidSportsDates.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidSportsDateText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString())
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheVacationLimitText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidStartDate.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidVacationStartDateText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfConsumedMaxLimit.ToString())
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheNormalVacationLimitText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNormalVacationBalanceExists.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfNormalVacationBalanceExistsText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDays.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDaysText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfVacationNotAllowedBetween1437and1438.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfVacationNotAllowedBetween1437and1438Text);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfVacationOutOfRange.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfVacationOutOfRangeText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseSickExceptionalVacationDatesOutOfRange.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfSickExceptionalVacationDatesOutOfRangeText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString())
                throw new CustomException(Resources.Globalization.ErrorInTimeAttendanceAppText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfMarriageVacationAcceptedOneTime.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfMarriageVacationAcceptedOneTimeText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEVacationRequestStatusNotPending.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfEVacationRequestStatusNotPendingText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEVacationRequestPendingExist.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationBecauseOfEVacationRequestPendingExistText, ((BaseVacationsBLL)result.Entity).EVacationsRequest.EVacationRequestNo));

            else if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfApproverIsNotAuthorizedPerson.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationBecauseOfApproverIsNotAuthorizedPersonText, ((EmployeesCodesBLL)result.Entity).EmployeeCodeNo, ((EmployeesCodesBLL)result.Entity).Employee.EmployeeNameAr));

            Classes.Helpers.CommonHelper.ConflictValidationMessage(result);

            return RedirectToAction("RequestsUnderAuthorizedPerson");
        }

        public ActionResult DetailsByAuthorizedPerson(int id)
        {
            EVacationsRequestsViewModel EVacationRequestVM = GetByEServiceRequestID(id);
            return View(EVacationRequestVM);
        }
        #endregion

        [NonAction]
        private EVacationsRequestsViewModel GetByEServiceRequestID(int id)
        {
            EVacationsRequestsViewModel EVacationRequestVM = new EVacationsRequestsViewModel();
            EVacationsRequestsBLL EVacationRequestBLL = new EVacationsRequestsBLL().GetEVacationsRequestsByEVacationRequestID(id);
            if (EVacationRequestBLL != null)
            {
                EVacationRequestVM.EVacationRequestID = EVacationRequestBLL.EVacationRequestID;
                EVacationRequestVM.EmployeeNameAr = EVacationRequestBLL.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr;
                EVacationRequestVM.EmployeeCodeID = EVacationRequestBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID;
                EVacationRequestVM.EmployeeCodeNo = EVacationRequestBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo;
                EVacationRequestVM.OrganizationID = EVacationRequestBLL.ActualEmployeeOrganization.OrganizationID;
                EVacationRequestVM.OrganizationName = EVacationRequestBLL.ActualEmployeeOrganization.OrganizationName;
                EVacationRequestVM.EVacationRequestNo = EVacationRequestBLL.EVacationRequestNo;
                EVacationRequestVM.EVacationRequestStatusName = EVacationRequestBLL.EVacationRequestStatus.EVacationRequestStatusName;
                EVacationRequestVM.EVacationRequestStatusEnum = (EVacationRequestStatusEnum)EVacationRequestBLL.EVacationRequestStatus.EVacationRequestStatusID;
                EVacationRequestVM.VacationStartDate = EVacationRequestBLL.VacationStartDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat);
                EVacationRequestVM.VacationEndDate = EVacationRequestBLL.VacationEndDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat);
                EVacationRequestVM.VacationPeriod = EVacationRequestBLL.VacationPeriod;
                EVacationRequestVM.WorkDate = EVacationRequestBLL.WorkDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat);
                EVacationRequestVM.VacationType = EVacationRequestBLL.VacationType.VacationTypeName;
                EVacationRequestVM.CreatorNotes = !string.IsNullOrEmpty(EVacationRequestBLL.CreatorNotes) ? EVacationRequestBLL.CreatorNotes : Resources.Globalization.NoNotesText;
                EVacationRequestVM.CreatedDate = EVacationRequestBLL.CreatedDate;
                EVacationRequestVM.AuthorizedPersonCodeNo = EVacationRequestBLL.ApprovedBy?.EmployeeCodeNo;
                EVacationRequestVM.AuthorizedPersonNameAr = EVacationRequestBLL.ApprovedBy?.Employee?.EmployeeNameAr;
                EVacationRequestVM.AuthorizedPersonDecision = EVacationRequestBLL.EVacationRequestStatus?.EVacationRequestStatusName;
                EVacationRequestVM.AuthorizedPersonDecisionDateTime = EVacationRequestBLL.ApprovalDateTime.HasValue ? EVacationRequestBLL.ApprovalDateTime.Value : (DateTime?)null;
                EVacationRequestVM.AuthorizedPersonNotes = EVacationRequestBLL.ApproverNotes;
                EVacationRequestVM.CancellationReasonByHR = EVacationRequestBLL.CancellationReasonByHR;
                EVacationRequestVM.LastUpdatedDate = EVacationRequestBLL.LastUpdatedDate.HasValue ? EVacationRequestBLL.LastUpdatedDate.Value : (DateTime?)null;
                EVacationRequestVM.LastUpdatedBy = EVacationRequestBLL.LastUpdatedBy?.EmployeeCodeNo + " - " + EVacationRequestBLL.LastUpdatedBy?.Employee?.EmployeeNameAr;
            }

            return EVacationRequestVM;
        }
    }
}