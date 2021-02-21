using HCM.Classes;
using HCM.Classes.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HCMBLL;
using HCMBLL.Enums;
using HCM.Models;
using ExceptionNameSpace;
using HCM.Classes.CustomFilters;
using PSADTO;
using PSADTO.Enums;

namespace HCM.Controllers
{
    public class VacationsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            VacationsViewModel VacationVM = new VacationsViewModel() { VacationType = new VacationsTypesBLL() };
            SetAuthentications(VacationVM);

            return View(VacationVM);
        }

        [CustomAuthorize]
        public ActionResult InquiryBalances()
        {
            VacationsViewModel VacationVM = new VacationsViewModel() { VacationType = new VacationsTypesBLL() };
            SetAuthentications(VacationVM);

            return View(VacationVM);
        }


        [Route("{controller}/GetVacationsType/{GenderID}")]
        public JsonResult GetVacationsType(int GenderID)
        {
            return Json(new
            {
                data = ((new VacationsTypesBLL().GetVacationsTypes(GenderID))).Select(x => new
                {
                    VacationTypeID = x.VacationTypeID,
                    VacationTypeName = x.VacationTypeName,

                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetVacationsType/{GenderID}/{IsPossibleTobeCreatedFromEVacationRequest}")]
        public JsonResult GetVacationsType(int GenderID, bool IsPossibleTobeCreatedFromEVacationRequest)
        {
            return Json(new
            {
                data = ((new VacationsTypesBLL().GetVacationsTypes(GenderID, IsPossibleTobeCreatedFromEVacationRequest))).Select(x => new
                {
                    VacationTypeID = x.VacationTypeID,
                    VacationTypeName = x.VacationTypeName,

                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetSportsSeasonDescription/{SportsSeasonMaturityYearID}")]
        public JsonResult GetSportsSeasonDescription(int SportsSeasonMaturityYearID)
        {
            return Json(new
            {
                data = ((new SportsSeasonsBLL().GetByMaturityYearID(SportsSeasonMaturityYearID))).Select(x => new
                {
                    SportsSeasonID = x.SportsSeasonID,
                    SportsSeasonDescription = x.SportsSeasonDescription,

                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetBySportsSeasonID/{SportsSeasonID}")]
        public JsonResult GetBySportsSeasonID(int SportsSeasonID)
        {
            SportsSeasonsBLL SportsSeasons = new SportsSeasonsBLL().GetBySportsSeasonID(SportsSeasonID);
            return Json(new
            {
                data = SportsSeasons
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetEmployeeCanceledVacations/{EmployeeCodeID}/{VacationTypeID}")]
        public JsonResult GetEmployeeCanceledVacations(int EmployeeCodeID, int VacationTypeID)
        {
            return Json(new
            {
                data = ((new BaseVacationsBLL().GetCanceledVacations(EmployeeCodeID, (VacationsTypesEnum)VacationTypeID))).Select(x => new
                {
                    VacationID = x.VacationID,
                    VacationTypeName = x.VacationType.ToString(),
                    VacationStartDate = x.VacationStartDate,
                    VacationEndDate = x.VacationEndDate,
                    VacationPeriod = x.VacationPeriod,
                    WorkDate = x.WorkDate,
                    IsCanceled = x.IsCanceled,
                    HasDetailsWithoutCreation = x.HasDetailsWithoutCreationPermission,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                }).OrderByDescending(x => x.VacationStartDate)
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetEmployeeFinishedVacations/{EmployeeCodeID}/{VacationTypeID}")]
        public JsonResult GetEmployeeFinishedVacations(int EmployeeCodeID, int VacationTypeID)
        {
            return Json(new
            {
                data = ((new BaseVacationsBLL().GetFinishedVacations(EmployeeCodeID, (VacationsTypesEnum)VacationTypeID))).Select(x => new
                {
                    VacationID = x.VacationID,
                    VacationTypeName = x.VacationType.ToString(),
                    VacationStartDate = x.VacationStartDate,
                    VacationEndDate = x.VacationEndDate,
                    VacationPeriod = x.VacationPeriod,
                    WorkDate = x.WorkDate,
                    IsCanceled = x.IsCanceled,
                    HasDetailsWithoutCreation = x.HasDetailsWithoutCreationPermission,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate,
                    NormalVacationTypeName = x.NormalVacationsType != null ? x.NormalVacationsType.NormalVacationTypeName : ""
                }).OrderByDescending(x => x.VacationStartDate)
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetEmployeeValidVacations/{EmployeeCodeID}/{VacationTypeID}")]
        public JsonResult GetEmployeeValidVacations(int EmployeeCodeID, int VacationTypeID)
        {
            return Json(new
            {
                data = ((new BaseVacationsBLL().GetValidVacations(EmployeeCodeID, (VacationsTypesEnum)VacationTypeID))).Select(x => new
                {
                    VacationID = x.VacationID,
                    VacationTypeName = x.VacationType.ToString(),
                    VacationStartDate = x.VacationStartDate,
                    VacationEndDate = x.VacationEndDate,
                    VacationPeriod = x.VacationPeriod,
                    WorkDate = x.WorkDate,
                    IsCanceled = x.IsCanceled,
                    HasDetailsWithoutCreation = x.HasDetailsWithoutCreationPermission,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                }).OrderByDescending(x => x.VacationStartDate)
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetVacationByVacationID/{VacationID}")]
        public JsonResult GetVacationByVacationID(int VacationID)
        {
            BaseVacationsBLL Vacation = new BaseVacationsBLL().GetByVacationID(VacationID);
            return Json(new
            {
                data = Vacation
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetEmployeeVacationsDetailsByVacationID/{VacationID}")]
        public JsonResult GetEmployeeVacationsDetailsByVacationID(int VacationID)
        {
            List<VacationsDetailsBLL> VacationsDetailsList = new VacationsDetailsBLL().GetVacationsDetailsByVacationID(VacationID);
            return Json(new
            {
                data = VacationsDetailsList.Select(x => new
                {
                    VacationDetailID = x.VacationDetailID,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    VacationPeriod = x.VacationPeriod,
                    IsApproved = x.IsApproved,
                    VacationActionName = x.VacationAction.VacationActionName,
                    WorkDate = x.Vacation.WorkDate,
                    Notes = x.Notes,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate,
                    ApprovedBy = x.ApprovedBy.Employee.EmployeeNameAr,
                    ApprovedDate = x.ApprovedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("{controller}/GetVacationEndDate/{VacationPeriod}/{VacationStartDate}")]
        public JsonResult GetVacationEndDate(int VacationPeriod, string VacationStartDate)
        {
            ////Expected format of VacationStartDate = "1440-05-15"
            //int year, day, month;
            //if (int.TryParse(VacationStartDate.Substring(0, 4), out year) &&
            //    int.TryParse(VacationStartDate.Substring(8, 2), out day) &&
            //    int.TryParse(VacationStartDate.Substring(5, 2), out month))
            //{
            //    DateTime StartDate = Convert.ToDateTime(Globals.Calendar.UmAlquraToGreg(string.Format("{0}/{1}/{2}", day, month, year)), new CultureInfo("en-US"));

            //    NormalVacationsBLL NormalVacationBLL = new NormalVacationsBLL();
            //    NormalVacationBLL.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetHiringRecordByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID); 
            //    DateTime MaturityEndDate = Convert.ToDateTime(this.GetMaturityEndDate(year, month, day), new CultureInfo("en-US"));

            //}

            return GetUmAlquraEndDate(VacationPeriod, VacationStartDate);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("{controller}/GetVacationPeriodAndEndDate/{WorkDate}/{VacationStartDate}")]
        public JsonResult GetVacationPeriodAndEndDate(string WorkDate, string VacationStartDate)
        {
            // Format expected: YYYY-MM-DD
            string VacationEndDate = GetUmAlquraPreviousDay(WorkDate);
            double daysCount = Globals.Calendar.GetUmAlquraDateDiff(VacationStartDate, VacationEndDate);

            return Json(new { VacationEndDate = VacationEndDate, VacationPeriod = daysCount }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("{controller}/GetWorkDateAfterVacation/{VacationEndDate}")]
        public JsonResult GetWorkDateAfterVacation(string VacationEndDate)
        {
            return GetUmAlquraEndDate(2, VacationEndDate);
        }

        public ActionResult Extend(int id)
        {
            VacationsExtentionViewModel VacationVM = new VacationsExtentionViewModel();
            BaseVacationsBLL Vacation = this.GetByVacationID(id);
            if (Vacation != null && Vacation.VacationID > 0)
            {
                Vacation = Vacation.GetByVacationID(id);
                VacationVM.VacationTypeID = (int)Vacation.VacationType;
                if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Sick)
                {
                    VacationVM.SickVacationTypeID = (int)((SickVacationsBLL)Vacation).SickVacationType;
                }
                if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.AccompanimentSick)
                {
                    VacationVM.SickVacationTypeID = (int)((AccompanimentSickVacationsBLL)Vacation).SickVacationType;
                }
                if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Accompaniment)
                {
                    VacationVM.SickVacationTypeID = (int)((AccompanimentVacationsBLL)Vacation).SickVacationType;
                }
                if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Sports)
                {
                    VacationVM.SportsSeasonID = ((SportsVacationsBLL)Vacation).SportsSeason.SportsSeasonID;
                }
                VacationVM.VacationID = Vacation.VacationID;
                VacationVM.FromDate = Vacation.WorkDate.Date;
                VacationVM.ToDate = null;
            }
            SetAuthentications(VacationVM);
            return View(VacationVM);
        }

        public ActionResult Edit(int id)
        {
            VacationsViewModel VacationVM = new VacationsViewModel();
            BaseVacationsBLL Vacation = new BaseVacationsBLL().GetByVacationID(id);
            VacationVM.VacationID = Vacation.VacationID;
            VacationVM.EmployeeCodeID = Vacation.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID;
            VacationVM.VacationType = new VacationsTypesBLL() { VacationTypeID = Convert.ToInt32(Vacation.VacationType) };
            VacationVM.VacationStartDate = Vacation.VacationStartDate.Date;
            VacationVM.VacationEndDate = Vacation.VacationEndDate.Date;
            VacationVM.WorkDate = Vacation.WorkDate;
            VacationVM.VacationPeriod = Vacation.VacationPeriod;

            if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Normal)
            {
                if (((NormalVacationsBLL)Vacation).NormalVacationType != null)
                    VacationVM.NormalVacationTypeID = ((NormalVacationsBLL)Vacation).NormalVacationType.NormalVacationTypeID;
            }
            if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Sick)
            {
                VacationVM.SickVacationType = new SickVacationsTypesBLL() { SickVacationTypeID = Convert.ToInt32(((SickVacationsBLL)Vacation).SickVacationType) };
            }
            if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Sports)
            {
                VacationVM.SportsSeasonID = ((SportsVacationsBLL)Vacation).SportsSeason.SportsSeasonID;
                VacationVM.SportsSeasonMaturityYearID = ((SportsVacationsBLL)Vacation).SportsSeason.MaturityYear.MaturityYearID;
            }
            if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.AccompanimentSick)
            {
                VacationVM.SickVacationType = new SickVacationsTypesBLL() { SickVacationTypeID = Convert.ToInt32(((AccompanimentSickVacationsBLL)Vacation).SickVacationType) };
            }
            if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Accompaniment)
            {
                VacationVM.SickVacationType = new SickVacationsTypesBLL() { SickVacationTypeID = Convert.ToInt32(((AccompanimentVacationsBLL)Vacation).SickVacationType) };
            }
            if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Studies)
            {
                VacationVM.StudiesVacationStartDate = ((StudiesVacationsBLL)Vacation).StudiesVacationStartDate;
            }
            if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Compensatory)
            {
                VacationVM.MaturityYearID = ((CompensatoryVacationsBLL)Vacation).HolidayAttendanceDetail.HolidaysAttendance.HolidaySetting.MaturityYear.MaturityYearID;
                VacationVM.HolidayTypeID = ((CompensatoryVacationsBLL)Vacation).HolidayAttendanceDetail.HolidaysAttendance.HolidaySetting.HolidayType.HolidayTypeID;
            }

            SetAuthentications(VacationVM);
            return View(VacationVM);
        }

        public ActionResult Break(int id)
        {
            VacationsBreakViewModel VacationVM = new VacationsBreakViewModel();
            BaseVacationsBLL Vacation = this.GetByVacationID(id);
            if (Vacation != null && Vacation.VacationID > 0)
            {
                VacationVM.VacationID = Vacation.VacationID;
                VacationVM.VacationTypeID = (int)Vacation.VacationType;
                if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Sick)
                {
                    VacationVM.SickVacationTypeID = (int)((SickVacationsBLL)Vacation).SickVacationType;
                }
                if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.AccompanimentSick)
                {
                    VacationVM.SickVacationTypeID = (int)((AccompanimentSickVacationsBLL)Vacation).SickVacationType;
                }
                if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Accompaniment)
                {
                    VacationVM.SickVacationTypeID = (int)((AccompanimentVacationsBLL)Vacation).SickVacationType;
                }
                VacationVM.VacationStartDate = Vacation.VacationStartDate.Date;
                VacationVM.VacationEndDate = Vacation.VacationEndDate.Date;
                VacationVM.WorkDate = Vacation.WorkDate;
                VacationVM.VacationPeriod = Vacation.VacationPeriod;
            }
            SetAuthentications(VacationVM);

            return View(VacationVM);
        }

        public ActionResult Cancel(int id)
        {
            VacationsCancelViewModel VacationVM = new VacationsCancelViewModel();
            BaseVacationsBLL Vacation = this.GetByVacationID(id);
            if (Vacation != null && Vacation.VacationID > 0)
            {
                VacationVM.VacationID = Vacation.VacationID;
                VacationVM.VacationTypeID = (int)Vacation.VacationType;
                if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Sick)
                {
                    VacationVM.SickVacationTypeID = (int)((SickVacationsBLL)Vacation).SickVacationType;
                }
                if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.AccompanimentSick)
                {
                    VacationVM.SickVacationTypeID = (int)((AccompanimentSickVacationsBLL)Vacation).SickVacationType;
                }
                if ((int)Vacation.VacationType == (Int16)VacationsTypesEnum.Accompaniment)
                {
                    VacationVM.SickVacationTypeID = (int)((AccompanimentVacationsBLL)Vacation).SickVacationType;
                }
                VacationVM.VacationStartDate = Vacation.VacationStartDate.Date;
                VacationVM.VacationEndDate = Vacation.VacationEndDate.Date;
                VacationVM.WorkDate = Vacation.WorkDate;
                VacationVM.VacationPeriod = Vacation.VacationPeriod;
            }
            SetAuthentications(VacationVM);
            return View(VacationVM);
        }

        [HttpPost]
        [IgnoreModelProperties("HolidayTypeID,MaturityYearID,SportsSeasonID,SportsSeasonMaturityYearID")]
        public ActionResult Create(VacationsViewModel VacationVM)
        {
            BaseVacationsBLL Vacation = null;
            if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Exceptional)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Normal)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                if (VacationVM.NormalVacationTypeID.HasValue)
                    ((NormalVacationsBLL)Vacation).NormalVacationType = new NormalVacationsTypesBLL()
                    {
                        NormalVacationTypeID = VacationVM.NormalVacationTypeID.Value
                    };
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Sick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SickVacationsBLL>.CreateInstance();
                ((SickVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationType.SickVacationTypeID;
                ((SickVacationsBLL)Vacation).IsSickLeaveAmountPaid = VacationVM.IsSickLeaveAmountPaid;
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.AccompanimentSick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentSickVacationsBLL>.CreateInstance();
                ((AccompanimentSickVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationType.SickVacationTypeID;
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Accompaniment)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentVacationsBLL>.CreateInstance();
                ((AccompanimentVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationType.SickVacationTypeID;
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Sports)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SportsVacationsBLL>.CreateInstance();
                ((SportsVacationsBLL)Vacation).SportsSeason = new SportsSeasonsBLL()
                {
                    SportsSeasonID = VacationVM.SportsSeasonID.Value
                };
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Born)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BornVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Emergency)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, EmergencyVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Dead)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, DeadVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.MotherHood)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MotherHoodVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Birth)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BirthVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Studies)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, StudiesVacationsBLL>.CreateInstance();
                ((StudiesVacationsBLL)Vacation).StudiesVacationStartDate = VacationVM.StudiesVacationStartDate;
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Compensatory)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, CompensatoryVacationsBLL>.CreateInstance();
                ((CompensatoryVacationsBLL)Vacation).HolidayAttendanceDetail = new CompensatoryVacationsBLL().GetHolidayAttendanceDetailByEmployeeCodeIDAndHolidayTypeIDAndHolidaySettingID((int)VacationVM.EmployeeCodeID, VacationVM.MaturityYearID, VacationVM.HolidayTypeID);
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Marriage)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MarriageVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Exam)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExamVacationsBLL>.CreateInstance();
            Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(VacationVM.EmployeeCodeID.Value);

            Vacation.VacationType = (VacationsTypesEnum)VacationVM.VacationType.VacationTypeID;
            Vacation.VacationStartDate = VacationVM.VacationStartDate.Value.Date;
            Vacation.VacationEndDate = VacationVM.VacationEndDate.Value.Date;
            Vacation.Notes = VacationVM.Notes;
            Vacation.LoginIdentity = this.UserIdentity;

            Result result = Vacation.Add();

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

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfMarriageVacationAcceptedOneTime.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfMarriageVacationAcceptedOneTimeText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEVacationRequestPendingExist.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationBecauseOfEVacationRequestPendingExistText, ((EVacationsRequestsBLL)result.Entity).EVacationRequestNo));

            else if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfNotAllowedWeekEndBetweenTwoVacations.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationNotAllowedWeekEndBetweenTwoVacationsText, ((BaseVacationsBLL)result.Entity).VacationStartDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat),
                                                                                                                                    ((BaseVacationsBLL)result.Entity).VacationPeriod,
                                                                                                                                    ((BaseVacationsBLL)result.Entity).VacationEndDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat),
                                                                                                                                    ((BaseVacationsBLL)result.Entity).VacationTypeName));

            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString())
                throw new CustomException(Resources.Globalization.ErrorInTimeAttendanceAppText);

            //else
            Classes.Helpers.CommonHelper.ConflictValidationMessage(result);

            // we get VacationDetailID and return to view to show report.
            int VacationDetailID = 0;
            VacationsDetailsBLL VacationDetail = new VacationsDetailsBLL().GetVacationDetailByVacationIDActionID(Vacation.VacationID, (int)VacationsActionsEnum.Add);
            if (VacationDetail != null && VacationDetail.VacationDetailID > 0)
                VacationDetailID = VacationDetail.VacationDetailID;

            return Json(new { VacationID = Vacation.VacationID, VacationDetailID = VacationDetailID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("HolidayTypeID,MaturityYearID,SportsSeasonID,SportsSeasonMaturityYearID")]
        public ActionResult Extend(VacationsExtentionViewModel VacationVM)
        {
            BaseVacationsBLL Vacation = null;
            if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Exceptional)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Normal)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Sick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SickVacationsBLL>.CreateInstance();
                ((SickVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationTypeID;
            }
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.AccompanimentSick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentSickVacationsBLL>.CreateInstance();
                ((AccompanimentSickVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationTypeID;
            }
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Accompaniment)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentVacationsBLL>.CreateInstance();
                ((AccompanimentVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationTypeID;
            }
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Sports)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SportsVacationsBLL>.CreateInstance();
                ((SportsVacationsBLL)Vacation).SportsSeason = new SportsSeasonsBLL()
                {
                    SportsSeasonID = VacationVM.SportsSeasonID.Value
                };
            }

            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Born)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BornVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Emergency)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, EmergencyVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Dead)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, DeadVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.MotherHood)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MotherHoodVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Birth)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BirthVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Studies)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, StudiesVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Compensatory)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, CompensatoryVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Marriage)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MarriageVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Exam)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExamVacationsBLL>.CreateInstance();
            Vacation.VacationID = VacationVM.VacationID;
            Vacation.VacationStartDate = VacationVM.FromDate.Date;
            Vacation.VacationEndDate = VacationVM.ToDate.Value.Date;
            Vacation.Notes = VacationVM.Notes;
            Vacation.MainframeNo = VacationVM.MainframeNo;
            Vacation.LoginIdentity = this.UserIdentity;
            Result result = Vacation.Extend();

            if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfPreviousNotApproved.ToString())
                throw new CustomException(Resources.Globalization.ValidationPreviousVacationActionNotApprovedText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfAlreadyCanceled.ToString())
                throw new CustomException(Resources.Globalization.ValidationVacationAlreadyCanceledText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString())
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheVacationLimitText);
            else if (result.EnumMember == VacationsValidationEnum.SportsSeasonDoesNotExist.ToString())
                throw new CustomException(Resources.Globalization.SportsSeasonDoesNotExistText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidSportsDates.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidSportsDateText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfConsumedMaxLimit.ToString())
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheNormalVacationLimitText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNormalVacationBalanceExists.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfNormalVacationBalanceExistsText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDays.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDaysText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfConsumedMaxLimit.ToString())
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheNormalVacationLimitText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidStartDate.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidVacationStartDateText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseSickExceptionalVacationDatesOutOfRange.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfSickExceptionalVacationDatesOutOfRangeText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEVacationRequestPendingExist.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationBecauseOfEVacationRequestPendingExistText, ((EVacationsRequestsBLL)result.Entity).EVacationRequestNo));
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString())
                throw new CustomException(Resources.Globalization.ErrorInTimeAttendanceAppText);

            Classes.Helpers.CommonHelper.ConflictValidationMessage(result);

            return Json(new { VacationID = Vacation.VacationID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Break(VacationsBreakViewModel VacationVM)
        {
            //BaseVacationsBLL Vacation = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
            BaseVacationsBLL Vacation = null;
            if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Exceptional)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Normal)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Sick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SickVacationsBLL>.CreateInstance();
                ((SickVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationTypeID;
            }
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.AccompanimentSick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentSickVacationsBLL>.CreateInstance();
                ((AccompanimentSickVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationTypeID;
            }
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Accompaniment)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentVacationsBLL>.CreateInstance();
                ((AccompanimentVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationTypeID;
            }
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Sports)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SportsVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Emergency)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, EmergencyVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Born)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BornVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Dead)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, DeadVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.MotherHood)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MotherHoodVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Birth)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BirthVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Studies)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, StudiesVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Compensatory)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, CompensatoryVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Marriage)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MarriageVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Exam)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExamVacationsBLL>.CreateInstance();
            Vacation.VacationID = VacationVM.VacationID;
            Vacation.VacationStartDate = VacationVM.VacationStartDate.Date;
            Vacation.VacationEndDate = VacationVM.VacationEndDate.Date;
            Vacation.Notes = VacationVM.Notes;
            Vacation.MainframeNo = VacationVM.MainframeNo;
            Vacation.LoginIdentity = this.UserIdentity;
            Result result = Vacation.Break();

            if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfPreviousNotApproved.ToString())
                throw new CustomException(Resources.Globalization.ValidationPreviousVacationActionNotApprovedText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDays.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDaysText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNewWorkDateGreaterThanPreviosWorkDate.ToString())
                throw new CustomException(Resources.Globalization.ValidationVacationPeriodText.ToString());
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfAlreadyCanceled.ToString())
                throw new CustomException(Resources.Globalization.ValidationVacationAlreadyCanceledText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEVacationRequestPendingExist.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationBecauseOfEVacationRequestPendingExistText, ((EVacationsRequestsBLL)result.Entity).EVacationRequestNo));
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString())
                throw new CustomException(Resources.Globalization.ErrorInTimeAttendanceAppText);
            else
                return Json(new { VacationID = Vacation.VacationID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeCareerHistoryID,HolidayTypeID,MaturityYearID,SportsSeasonID,SportsSeasonMaturityYearID")]
        public ActionResult Edit(VacationsViewModel VacationVM)
        {
            BaseVacationsBLL Vacation = null;
            if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Exceptional)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Normal)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                if (VacationVM.NormalVacationTypeID.HasValue)
                    ((NormalVacationsBLL)Vacation).NormalVacationType = new NormalVacationsTypesBLL()
                    {
                        NormalVacationTypeID = VacationVM.NormalVacationTypeID.Value
                    };
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Sick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SickVacationsBLL>.CreateInstance();
                ((SickVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationType.SickVacationTypeID;
                ((SickVacationsBLL)Vacation).IsSickLeaveAmountPaid = VacationVM.IsSickLeaveAmountPaid;
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.AccompanimentSick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentSickVacationsBLL>.CreateInstance();
                ((AccompanimentSickVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationType.SickVacationTypeID;
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Accompaniment)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentVacationsBLL>.CreateInstance();
                ((AccompanimentVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationType.SickVacationTypeID;
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Sports)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SportsVacationsBLL>.CreateInstance();
                ((SportsVacationsBLL)Vacation).SportsSeason = new SportsSeasonsBLL()
                {
                    SportsSeasonID = VacationVM.SportsSeasonID.Value
                };
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Born)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BornVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Emergency)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, EmergencyVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Dead)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, DeadVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.MotherHood)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MotherHoodVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Birth)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BirthVacationsBLL>.CreateInstance();

            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Studies)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, StudiesVacationsBLL>.CreateInstance();
                ((StudiesVacationsBLL)Vacation).StudiesVacationStartDate = VacationVM.StudiesVacationStartDate;
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Compensatory)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, CompensatoryVacationsBLL>.CreateInstance();
                ((CompensatoryVacationsBLL)Vacation).HolidayAttendanceDetail = new CompensatoryVacationsBLL().GetHolidayAttendanceDetailByEmployeeCodeIDAndHolidayTypeIDAndHolidaySettingID((int)VacationVM.EmployeeCodeID, VacationVM.MaturityYearID, VacationVM.HolidayTypeID);
            }
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Marriage)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MarriageVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationType.VacationTypeID == (int)VacationsTypesEnum.Exam)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExamVacationsBLL>.CreateInstance();
            Vacation.VacationID = VacationVM.VacationID;
            Vacation.VacationStartDate = VacationVM.VacationStartDate.Value.Date;
            Vacation.VacationEndDate = VacationVM.VacationEndDate.Value.Date;
            Vacation.Notes = VacationVM.Notes;
            Vacation.LoginIdentity = this.UserIdentity;
            Result result = Vacation.Edit();

            if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfAlreadyApproved.ToString())
                throw new CustomException(Resources.Globalization.ValidationVacationActionAlreadyApprovedText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNoBalance.ToString())
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheVacationLimitText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfConsumedMaxLimit.ToString())
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheNormalVacationLimitText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNormalVacationBalanceExists.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfNormalVacationBalanceExistsText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDays.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDaysText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfVacationNotAllowedBetween1437and1438.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfVacationNotAllowedBetween1437and1438Text);
            else if (result.EnumMember == VacationsValidationEnum.SportsSeasonDoesNotExist.ToString())
                throw new CustomException(Resources.Globalization.SportsSeasonDoesNotExistText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidSportsDates.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidSportsDateText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidDates.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidDateText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfInvalidStartDate.ToString())
                throw new CustomException(Resources.Globalization.ValidationInvalidVacationStartDateText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfVacationOutOfRange.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfVacationOutOfRangeText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseSickExceptionalVacationDatesOutOfRange.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfSickExceptionalVacationDatesOutOfRangeText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEVacationRequestPendingExist.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationBecauseOfEVacationRequestPendingExistText, ((EVacationsRequestsBLL)result.Entity).EVacationRequestNo));
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString())
                throw new CustomException(Resources.Globalization.ErrorInTimeAttendanceAppText);

            else if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfPreviousNotApproved.ToString())
                throw new CustomException(Resources.Globalization.ValidationPreviousVacationActionNotApprovedText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfAlreadyCanceled.ToString())
                throw new CustomException(Resources.Globalization.ValidationVacationAlreadyCanceledText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfAlreadyBreak.ToString())
                throw new CustomException(Resources.Globalization.ValidationVacationAlreadyBreakText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfAlreadyExtend.ToString())
                throw new CustomException(Resources.Globalization.ValidationVacationAlreadyExtendedText);

            Classes.Helpers.CommonHelper.ConflictValidationMessage(result);

            return Json(new { VacationID = Vacation.VacationID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Cancel(VacationsCancelViewModel VacationVM)
        {
            //BaseVacationsBLL Vacation = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
            BaseVacationsBLL Vacation = null;
            if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Exceptional)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Normal)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Sick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SickVacationsBLL>.CreateInstance();
                ((SickVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationTypeID;
            }
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.AccompanimentSick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentSickVacationsBLL>.CreateInstance();
                ((AccompanimentSickVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationTypeID;
            }
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Accompaniment)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentVacationsBLL>.CreateInstance();
                ((AccompanimentVacationsBLL)Vacation).SickVacationType = (SickVacationsTypesEnum)VacationVM.SickVacationTypeID;
            }
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Sports)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SportsVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Emergency)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, EmergencyVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Born)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BornVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Dead)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, DeadVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.MotherHood)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MotherHoodVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Birth)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BirthVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Studies)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, StudiesVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Compensatory)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, CompensatoryVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Marriage)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MarriageVacationsBLL>.CreateInstance();
            else if (VacationVM.VacationTypeID == (int)VacationsTypesEnum.Exam)
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExamVacationsBLL>.CreateInstance();
            Vacation.VacationID = VacationVM.VacationID;
            Vacation.VacationStartDate = VacationVM.VacationStartDate.Date;
            Vacation.VacationEndDate = VacationVM.VacationEndDate.Date;
            Vacation.Notes = VacationVM.Notes;
            Vacation.MainframeNo = VacationVM.MainframeNo;
            Vacation.LoginIdentity = this.UserIdentity;
            Result result = Vacation.Cancel();

            if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfPreviousNotApproved.ToString())
                throw new CustomException(Resources.Globalization.ValidationPreviousVacationActionNotApprovedText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfAlreadyCanceled.ToString())
                throw new CustomException(Resources.Globalization.ValidationVacationAlreadyCanceledText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfErrorInTimeAttendanceApp.ToString())
                throw new CustomException(Resources.Globalization.ErrorInTimeAttendanceAppText);
            else if (result.EnumMember == VacationsValidationEnum.RejectedBecauseOfEVacationRequestPendingExist.ToString())
                throw new CustomException(string.Format(Resources.Globalization.ValidationBecauseOfEVacationRequestPendingExistText, ((EVacationsRequestsBLL)result.Entity).EVacationRequestNo));
            else
                return Json(new { VacationID = Vacation.VacationID }, JsonRequestBehavior.AllowGet);
        }

        //[CustomAuthorize]
        public ActionResult PrintVacationAction(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.Vacations.ToString(), id));
        }

        //[CustomAuthorize]
        public ActionResult PrintVacationByVacationDetailsID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.Vacations.ToString(), id));
        }

        //[CustomAuthorize]
        public ActionResult PrintAllVacationsByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.Vacations.ToString(), id));
        }

        [Route("{controller}/GetVacationBalance/{EmployeeCodeID}/{VacationTypeID}/{VacationStartDate}")]
        public JsonResult GetVacationBalance(int EmployeeCodeID, int VacationTypeID, DateTime? VacationStartDate)
        {
            return GetAllVacationBalance(EmployeeCodeID, VacationTypeID, VacationStartDate, null, null, null);
        }

        [Route("{controller}/GetCompensatoryVacationBalance/{EmployeeCodeID}/{VacationTypeID}/{MaturityYearID}/{HolidayTypeID}")]
        public JsonResult GetCompensatoryVacationBalance(int EmployeeCodeID, int VacationTypeID, int? MaturityYearID, int? HolidayTypeID)
        {
            return GetAllVacationBalance(EmployeeCodeID, VacationTypeID, null, MaturityYearID, HolidayTypeID, null);
        }

        [Route("{controller}/GetSportsVacationBalance/{EmployeeCodeID}/{VacationTypeID}/{SportsSeasonID}")]
        public JsonResult GetSportsVacationBalance(int EmployeeCodeID, int VacationTypeID, int? SportsSeasonID)
        {
            return GetAllVacationBalance(EmployeeCodeID, VacationTypeID, null, null, null, SportsSeasonID);
        }

        public JsonResult GetBalanceExpiry(int id)
        {
            try
            {
                BaseVacationsBLL Vacation = null;

                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                Vacation.VacationStartDate = DateTime.Now;
                List<NormalVacationsBreakupDetails> CheckExpiry = ((NormalVacationsBLL)Vacation).GetBreakupDetails(id, Vacation.VacationStartDate.Year, Vacation.VacationStartDate.Month, Vacation.VacationStartDate.Day);

                return Json(new
                {
                    data = CheckExpiry.Select(x => new
                    {
                        ExpiryDate = x.ExpiryDate,
                        Days = x.Days,
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        public JsonResult DeleteVacationByVacationDetailID(int id)
        {
            VacationsDetailsBLL Vacation = new VacationsDetailsBLL();
            Vacation.LoginIdentity = this.UserIdentity;
            Result result = Vacation.Remove(id);

            if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfAlreadyApproved.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationActionAlreadyApprovedText);
            }
            else if (result.EnumMember == VacationsValidationEnum.RejectedBeacuseOfNoChanceCancelCancelling.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationActionNoChanceCancelCancellingText);
            }
            else
            {
                return Json(new
                {
                    data = Vacation
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDeservedOldBalanceDetails(int id)
        {
            try
            {
                BaseVacationsBLL Vacation = null;
                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                List<NormalDeservedOldBalanceDetails> NormalDeservedOldBalanceDetailsList = ((NormalVacationsBLL)Vacation).GetDeservedOldBalanceDetails(id);
                return Json(new
                {
                    data = NormalDeservedOldBalanceDetailsList.Select(x => new
                    {
                        ServiceYear = x.ServiceYear,
                        MaturityYearBalance = x.MaturityYearBalance,
                        UnDeservedBalance = x.UnDeservedBalance,
                        DeservedBalance = x.DeservedBalance
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        public JsonResult GetConsumedOldBalanceDetails(int id)
        {
            try
            {
                BaseVacationsBLL Vacation = null;
                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                List<NormalConsumedOldBalanceDetails> NormalConsumedOldBalanceDetailsList = ((NormalVacationsBLL)Vacation).GetConsumedOldBalanceDetails(id);
                return Json(new
                {
                    data = NormalConsumedOldBalanceDetailsList.Select(x => new
                    {
                        VacationStartDate = x.VacationStartDate,
                        VacationPeriod = x.VacationPeriod
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        // [Route("{controller}/GetDeservedCurrentBalanceDetails/{id}")]
        public JsonResult GetDeservedCurrentBalanceDetails(int id)
        {
            try
            {
                BaseVacationsBLL Vacation = null;

                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                Vacation.VacationStartDate = DateTime.Now;
                List<NormalDeservedCurrentBalanceDetails> NormalDeservedCurrentBalanceDetailsList = ((NormalVacationsBLL)Vacation).GetDeservedCurrentBalanceDetails(id);
                return Json(new
                {
                    data = NormalDeservedCurrentBalanceDetailsList.Select(x => new
                    {
                        FromDate = x.FromDate,
                        ToDate = x.ToDate,
                        MaturityYearBalance = x.MaturityYearBalance,
                        UnDeservedBalance = x.UnDeservedBalance,
                        DeservedBalance = x.DeservedBalance
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        //[Route("{controller}/GetConsumedCurrentBalanceDetails/{id}")]
        public JsonResult GetConsumedCurrentBalanceDetails(int id)
        {
            try
            {
                BaseVacationsBLL Vacation = null;

                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                Vacation.VacationStartDate = DateTime.Now;
                //Vacation.VacationStartDate = Convert.ToDateTime(Globals.Calendar.UmAlquraToGreg(string.Format("{0}/{1}/{2}", Vacation.VacationStartDate.Date.Day, Vacation.VacationStartDate.Date.Month, Vacation.VacationStartDate.Date.Year)), new CultureInfo("en-US"));
                List<NormalConsumedCurrentBalanceDetails> NormalConsumedCurrentBalanceDetailsList = ((NormalVacationsBLL)Vacation).GetConsumedCurrentBalanceDetails(id);
                return Json(new
                {
                    data = NormalConsumedCurrentBalanceDetailsList.Select(x => new
                    {
                        VacationStartDate = x.VacationStartDate,
                        VacationPeriod = x.VacationPeriod
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        private BaseVacationsBLL GetByVacationID(int id)
        {
            BaseVacationsBLL Vacation = null;
            Vacation = new BaseVacationsBLL().GetByVacationID(id);
            if (Vacation != null && Vacation.VacationID > 0)
            {
                if (Vacation.VacationType == VacationsTypesEnum.Exceptional)
                    Vacation = (ExceptionalVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.Normal)
                    Vacation = (NormalVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.Sick)
                    Vacation = (SickVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.AccompanimentSick)
                    Vacation = (AccompanimentSickVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.Accompaniment)
                    Vacation = (AccompanimentVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.Sports)
                    Vacation = (SportsVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.Born)
                    Vacation = (BornVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.Dead)
                    Vacation = (DeadVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.MotherHood)
                    Vacation = (MotherHoodVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.Birth)
                    Vacation = (BirthVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.Studies)
                    Vacation = (StudiesVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.Marriage)
                    Vacation = (MarriageVacationsBLL)Vacation;
                else if (Vacation.VacationType == VacationsTypesEnum.Exam)
                    Vacation = (ExamVacationsBLL)Vacation;
            }
            return Vacation;
        }

        private JsonResult GetAllVacationBalance(int EmployeeCodeID, int VacationTypeID, DateTime? VacationStartDate, int? MaturityYearID, int? HolidayTypeID, int? SportsSeasonID)
        {
            BaseVacationsBLL Vacation = null;
            if (VacationTypeID == (int)VacationsTypesEnum.Exceptional)
            {
                /* we are showing balance in balance table  */
                //Vacation = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                //((ExceptionalVacationsBLL)Vacation).GetBalanceTable(EmployeeCodeID, null, 0);

                //return Json(new
                //{
                //    data = new
                //    {
                //        VacationBalance = (Vacation as ExceptionalVacationsBLL).VacationBalance,
                //        VacationConsumedBalance = (Vacation as ExceptionalVacationsBLL).VacationConsumedBalance,
                //        VacationRemainingBalance = (Vacation as ExceptionalVacationsBLL).VacationRemainingBalance
                //    }
                //}, JsonRequestBehavior.AllowGet);
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Sick)
            {
                /* we are showing balance in balance table  */
                //Vacation = GenericFactoryPattern<BaseVacationsBLL, SickVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                //((SickVacationsBLL)Vacation).GetBalanceTable(EmployeeCodeID, null, 0);

                //return Json(new
                //{
                //    data = new
                //    {
                //        VacationBalance = (Vacation as SickVacationsBLL).VacationBalance,
                //        VacationConsumedBalance = (Vacation as SickVacationsBLL).VacationConsumedBalance,
                //        VacationRemainingBalance = (Vacation as SickVacationsBLL).VacationRemainingBalance
                //    }
                //}, JsonRequestBehavior.AllowGet);

            }
            else if (VacationTypeID == (int)VacationsTypesEnum.AccompanimentSick)
            {
                /* we are showing balance in balance table  */
                //Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                //((AccompanimentVacationsBLL)Vacation).GetBalanceTable(EmployeeCodeID, null, 0);

                //return Json(new
                //{
                //    data = new
                //    {
                //        VacationBalance = (Vacation as AccompanimentVacationsBLL).VacationBalance,
                //        VacationConsumedBalance = (Vacation as AccompanimentVacationsBLL).VacationConsumedBalance,
                //        VacationRemainingBalance = (Vacation as AccompanimentVacationsBLL).VacationRemainingBalance
                //    }
                //}, JsonRequestBehavior.AllowGet);
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Accompaniment)
            {
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Sports)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SportsVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
                ((SportsVacationsBLL)Vacation).SportsSeason = new SportsSeasonsBLL().GetBySportsSeasonID(SportsSeasonID.Value);
                ((SportsVacationsBLL)Vacation).GetVacationBalance();

            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Born)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BornVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
                ((BornVacationsBLL)Vacation).GetVacationBalance();
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Emergency)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, EmergencyVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
                ((EmergencyVacationsBLL)Vacation).GetVacationBalance();
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Dead)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, DeadVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
                ((DeadVacationsBLL)Vacation).GetVacationBalance();
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.MotherHood)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MotherHoodVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
                ((MotherHoodVacationsBLL)Vacation).GetVacationBalance();
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Birth)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, BirthVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
                ((BirthVacationsBLL)Vacation).GetVacationBalance();
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Studies)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, StudiesVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Compensatory)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, CompensatoryVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL()
                //{
                //    EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID },
                //};
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
                ((CompensatoryVacationsBLL)Vacation).HolidayAttendanceDetail = new CompensatoryVacationsBLL().GetHolidayAttendanceDetailByEmployeeCodeIDAndHolidayTypeIDAndHolidaySettingID(EmployeeCodeID, (int)MaturityYearID, (int)HolidayTypeID);
                if (((CompensatoryVacationsBLL)Vacation).HolidayAttendanceDetail != null)
                    ((CompensatoryVacationsBLL)Vacation).HolidayAttendanceDetail.HolidaysAttendance = new HolidaysAttendanceBLL().GetByHolidayAttendanceID(((CompensatoryVacationsBLL)Vacation).HolidayAttendanceDetail.HolidaysAttendance.HolidayAttendanceID);
                else
                {

                    HolidaysAttendanceBLL HolidaysAttendanceBLL = new HolidaysAttendanceBLL();
                    HolidaysAttendanceBLL.HolidaySetting = new HolidaysSettingsBLL() { HolidayType = new HolidaysTypesBLL(), MaturityYear = new MaturityYearsBalancesBLL() };
                    ((CompensatoryVacationsBLL)Vacation).HolidayAttendanceDetail = new HolidaysAttendanceDetailsBLL() { HolidaysAttendance = HolidaysAttendanceBLL };

                }
                ((CompensatoryVacationsBLL)Vacation).GetVacationBalance();
                //(Vacation as CompensatoryVacationsBLL).MaturityYear = new MaturityYearsBalancesBLL() { MaturityYearID = (int)MaturityYearID };
                //(Vacation as CompensatoryVacationsBLL).HolidayType = new HolidaysTypesBLL() { HolidayTypeID = (int)HolidayTypeID };
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Normal)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, NormalVacationsBLL>.CreateInstance();
                Vacation.VacationStartDate = VacationStartDate.HasValue ? VacationStartDate.Value : DateTime.Now;
                ((NormalVacationsBLL)Vacation).GetVacationBalances(EmployeeCodeID, Vacation.VacationStartDate.Year, Vacation.VacationStartDate.Month, Vacation.VacationStartDate.Day);

                return Json(new
                {
                    data = new
                    {
                        DeservedOldBalance = (Vacation as NormalVacationsBLL).DeservedOldBalance,
                        ConsumedOldBalance = (Vacation as NormalVacationsBLL).ConsumedOldBalance,
                        RemainingOldBalance = (Vacation as NormalVacationsBLL).RemainingOldBalance,

                        DeservedCurrentBalance = (Vacation as NormalVacationsBLL).DeservedCurrentBalance,
                        ConsumedCurrentBalance = (Vacation as NormalVacationsBLL).ConsumedCurrentBalance,
                        ExpiredCurrentBalance = (Vacation as NormalVacationsBLL).ExpiredCurrentBalance,
                        RemainingCurrentBalance = (Vacation as NormalVacationsBLL).RemainingCurrentBalance,
                        //NetRemainingCurrentBalance = (Vacation as NormalVacationsBLL).NetRemainingCurrentBalance,

                        InAdvanceBalance = (Vacation as NormalVacationsBLL).InAdvanceBalance,
                        TotalAvailableVacationBalance = (Vacation as NormalVacationsBLL).TotalAvailableVacationBalance,

                        TotalDeservedBalance = (Vacation as NormalVacationsBLL).TotalDeservedBalance,
                        TotalConsumedBalance = (Vacation as NormalVacationsBLL).TotalConsumedBalance,
                        TotalRemainingBalance = (Vacation as NormalVacationsBLL).TotalRemainingBalance,

                        TotalConsumedSeparatedDays = (Vacation as NormalVacationsBLL).TotalConsumedSeparatedDays,
                    }
                }, JsonRequestBehavior.AllowGet);

            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Marriage)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, MarriageVacationsBLL>.CreateInstance();
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
                ((MarriageVacationsBLL)Vacation).GetVacationBalance();
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Exam)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExamVacationsBLL>.CreateInstance();
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
                ((ExamVacationsBLL)Vacation).GetVacationBalance();
            }
            else
                Vacation = new BaseVacationsBLL();

            return Json(new
            {
                data = Vacation
            }, JsonRequestBehavior.AllowGet);

        }

        [AllowAnonymous]
        [Route("{controller}/GetLastNVacations/{EmployeeCodeID}")]
        public JsonResult GetLastNVacations(int EmployeeCodeID)
        {
            return Json(new
            {
                data = (new BaseVacationsBLL().GetLastNVacationsEmployeeCodeID(EmployeeCodeID)).Select(x => new
                {
                    VacationID = x.VacationID,
                    VacationTypeName = x.VacationTypeName.ToString(),
                    VacationStartDate = x.VacationStartDate,
                    VacationEndDate = x.VacationEndDate,
                    VacationPeriod = x.VacationPeriod,
                    WorkDate = x.WorkDate,
                    IsCanceled = x.IsCanceled,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                }).OrderByDescending(x => x.VacationStartDate)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VacationDetails(int id)
        {
            return View("VacationDetails", new VacationsViewModel() { VacationID = id });
        }

        [Route("{controller}/GetBalanceTableByVacationType/{EmployeeCodeID}/{VacationTypeID}")]
        public JsonResult GetBalanceTableByVacationType(int EmployeeCodeID, int VacationTypeID)
        {
            //DateTime? VacationStartDate = null;
            BaseVacationsBLL Vacation = null;

            if (VacationTypeID == (int)VacationsTypesEnum.Exceptional)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);

                return Json(new
                {
                    data = (((ExceptionalVacationsBLL)Vacation).GetBalanceTable(EmployeeCodeID, null, 0)).Select(x => new
                    {
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        ConsumedBalance = x.ConsumedBalance,
                        x.RemainingBalance
                    }).OrderBy(x => x.StartDate)
                }, JsonRequestBehavior.AllowGet);
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Sick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, SickVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);

                return Json(new
                {
                    data = (((SickVacationsBLL)Vacation).GetBalanceTable(EmployeeCodeID, null, 0)).Select(x => new
                    {
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        ConsumedBalance = x.ConsumedBalance,
                        x.RemainingBalance
                    }).OrderBy(x => x.StartDate)
                }, JsonRequestBehavior.AllowGet);
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.AccompanimentSick)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentSickVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);

                return Json(new
                {
                    data = (((AccompanimentSickVacationsBLL)Vacation).GetBalanceTable(EmployeeCodeID, null, 0)).Select(x => new
                    {
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        ConsumedBalance = x.ConsumedBalance,
                        x.RemainingBalance
                    }).OrderBy(x => x.StartDate)
                }, JsonRequestBehavior.AllowGet);
            }
            else if (VacationTypeID == (int)VacationsTypesEnum.Accompaniment)
            {
                Vacation = GenericFactoryPattern<BaseVacationsBLL, AccompanimentVacationsBLL>.CreateInstance();
                //Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } };
                Vacation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);

                return Json(new
                {
                    data = (((AccompanimentVacationsBLL)Vacation).GetBalanceTable(EmployeeCodeID, null, 0)).Select(x => new
                    {
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        ConsumedBalance = x.ConsumedBalance,
                        x.RemainingBalance
                    }).OrderBy(x => x.StartDate)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = new List<BaseVacationsBLL>() }, JsonRequestBehavior.AllowGet);
            }

        }

        [AllowAnonymous]
        [Route("{controller}/GetEmployeesHaveVacationsBetweenTwoDates/{OrganizationID}/{VacationStartDate}/{VacationEndDate}")]
        public JsonResult GetEmployeesHaveVacationsBetweenTwoDates(int OrganizationID, DateTime VacationStartDate, DateTime VacationEndDate)
        {
            return Json(new
            {
                data = (new BaseVacationsBLL().GetEmployeesHaveVacationsBetweenTwoDates(OrganizationID, VacationStartDate, VacationEndDate)).Select(x => new
                {
                    x.VacationID,
                    x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    x.VacationTypeName,
                    x.VacationStartDate,
                    x.VacationEndDate,
                    x.VacationPeriod,
                    x.WorkDate,
                }).OrderByDescending(x => x.VacationStartDate)
            }, JsonRequestBehavior.AllowGet);

            //return Json(new { data = new BaseVacationsBLL().GetEmployeesHaveVacationsBetweenTwoDates(OrganizationID, VacationStartDate, VacationEndDate) }, JsonRequestBehavior.AllowGet);
        }

        private void SetAuthentications(VacationsViewModel VacationVM)
        {
            AuthenticationResult Authentication = (AuthenticationResult)Session["Authentication"];
            if (Authentication != null && Authentication.User != null && Authentication.User.IsAdmin)
            {
                VacationVM.HasCreatingAccess
                = VacationVM.HasDeletingAccess
                = VacationVM.HasUpdatingAccess = true;
            }
            else
            {
                GroupsObjects Privilage = Authentication.Privilages.FirstOrDefault(e => e.Object.ObjectID == (int)ObjectsEnum.HCMVacations);

                if (Privilage != null)
                {
                    VacationVM.HasCreatingAccess = Privilage.Creating;
                    VacationVM.HasDeletingAccess = Privilage.Deleting;
                    VacationVM.HasUpdatingAccess = Privilage.Updating;
                }
                else
                {
                    VacationVM.HasCreatingAccess
                    = VacationVM.HasDeletingAccess
                    = VacationVM.HasUpdatingAccess = false;
                }
            }
        }

        private void SetAuthentications(VacationsExtentionViewModel VacationVM)
        {
            AuthenticationResult Authentication = (AuthenticationResult)Session["Authentication"];
            if (Authentication != null && Authentication.User != null && Authentication.User.IsAdmin)
            {
                VacationVM.HasCreatingAccess
                = VacationVM.HasDeletingAccess
                = VacationVM.HasUpdatingAccess = true;
            }
            else
            {
                GroupsObjects Privilage = Authentication.Privilages.FirstOrDefault(e => e.Object.ObjectID == (int)ObjectsEnum.HCMVacations);

                if (Privilage != null)
                {
                    VacationVM.HasCreatingAccess = Privilage.Creating;
                    VacationVM.HasDeletingAccess = Privilage.Deleting;
                    VacationVM.HasUpdatingAccess = Privilage.Updating;
                }
                else
                {
                    VacationVM.HasCreatingAccess
                    = VacationVM.HasDeletingAccess
                    = VacationVM.HasUpdatingAccess = false;
                }
            }
        }

        private void SetAuthentications(VacationsBreakViewModel VacationVM)
        {
            AuthenticationResult Authentication = (AuthenticationResult)Session["Authentication"];
            if (Authentication != null && Authentication.User != null && Authentication.User.IsAdmin)
            {
                VacationVM.HasCreatingAccess
                = VacationVM.HasDeletingAccess
                = VacationVM.HasUpdatingAccess = true;
            }
            else
            {
                GroupsObjects Privilage = Authentication.Privilages.FirstOrDefault(e => e.Object.ObjectID == (int)ObjectsEnum.HCMVacations);

                if (Privilage != null)
                {
                    VacationVM.HasCreatingAccess = Privilage.Creating;
                    VacationVM.HasDeletingAccess = Privilage.Deleting;
                    VacationVM.HasUpdatingAccess = Privilage.Updating;
                }
                else
                {
                    VacationVM.HasCreatingAccess
                    = VacationVM.HasDeletingAccess
                    = VacationVM.HasUpdatingAccess = false;
                }
            }
        }

        private void SetAuthentications(VacationsCancelViewModel VacationVM)
        {
            AuthenticationResult Authentication = (AuthenticationResult)Session["Authentication"];
            if (Authentication != null && Authentication.User != null && Authentication.User.IsAdmin)
            {
                VacationVM.HasCreatingAccess
                = VacationVM.HasDeletingAccess
                = VacationVM.HasUpdatingAccess = true;
            }
            else
            {
                GroupsObjects Privilage = Authentication.Privilages.FirstOrDefault(e => e.Object.ObjectID == (int)ObjectsEnum.HCMVacations);

                if (Privilage != null)
                {
                    VacationVM.HasCreatingAccess = Privilage.Creating;
                    VacationVM.HasDeletingAccess = Privilage.Deleting;
                    VacationVM.HasUpdatingAccess = Privilage.Updating;
                }
                else
                {
                    VacationVM.HasCreatingAccess
                    = VacationVM.HasDeletingAccess
                    = VacationVM.HasUpdatingAccess = false;
                }
            }
        }
    }
}