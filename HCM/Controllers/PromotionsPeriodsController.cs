using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class PromotionsPeriodsController : BaseController
    {
        //
        // GET: /PromotionsPeriods/
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPromotionPeriodsByYear(int id)
        {
            return Json(new { data = new PromotionsPeriodsBLL().GetPeriodsByYear(id) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPromotionPeriodDetails(int id)
        {
            Session["PromotionRecordEmployees"] = null;
            return Json(new { data = new PromotionsPeriodsBLL().GetByPromotionPeriodID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPromotionsPeriods()
        {
            Session["PromotionRecordEmployees"] = null;
            var list = new PromotionsPeriodsBLL().GetPromotionsPeriods()
                .Select(x => new
                {
                    PromotionPeriodID = x.PromotionPeriodID,
                    Period = x.Period,
                    Year = x.Year,
                    PromotionStartDate = x.PromotionStartDate,
                    PromotionEndDate = x.PromotionEndDate,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                });

            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

      
        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PromotionsPeriodsViewModel PromotionPeriodVM)
        {
            PromotionsPeriodsBLL PromotionPeriod = new PromotionsPeriodsBLL();
            PromotionPeriod.Year = new MaturityYearsBalancesBLL() { MaturityYearID = PromotionPeriodVM.YearID };
            PromotionPeriod.Period = new PeriodsBLL() { PeriodID = PromotionPeriodVM.PeriodID };//QualificationsDegreesVM.QualificationDegreeName;
            PromotionPeriod.PromotionStartDate = PromotionPeriodVM.PromotionStartDate;
            PromotionPeriod.PromotionEndDate = PromotionPeriodVM.PromotionEndDate;
            PromotionPeriod.IsActive = PromotionPeriodVM.IsActive;
            PromotionPeriod.LoginIdentity = UserIdentity;
            Result result = PromotionPeriod.Add();
            if (result.EnumMember == PromotionsPeriodsValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == PromotionsPeriodsValidationEnum.RejectedBecauseOfPromotionStartDateIsGreaterThenPromotionEndDate.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationPromotionStartDateIsGreaterThenPromotionEndDateText);
            }
            else if (result.EnumMember == PromotionsPeriodsValidationEnum.RejectedBecauseOfAlreadyOnePromotionPeriodIsActive.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationAlreadyOnePromotionPeriodIsActiveText);
            }
            else if (result.EnumMember == PromotionsPeriodsValidationEnum.RejectedBecauseOfPromotionRecordExistWithThisPromotiosPeriodDates.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationPromotionRecordExistWithThesePromotiosPeriodDatesText);
            }
            return Json(new { PromotionPeriodID = PromotionPeriod.PromotionPeriodID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByPromotionPeriodID(id));
        }

        [HttpPost]

        public ActionResult Edit(PromotionsPeriodsViewModel PromotionPeriodVM)
        {
            PromotionsPeriodsBLL PromotionPeriod = new PromotionsPeriodsBLL();
            PromotionPeriod.PromotionPeriodID = PromotionPeriodVM.PromotionPeriodID;
            PromotionPeriod.Year = new MaturityYearsBalancesBLL() { MaturityYearID = PromotionPeriodVM.YearID };
            PromotionPeriod.Period = new PeriodsBLL() { PeriodID = PromotionPeriodVM.PeriodID };//QualificationsDegreesVM.QualificationDegreeName;
            PromotionPeriod.PromotionStartDate = PromotionPeriodVM.PromotionStartDate;
            PromotionPeriod.PromotionEndDate = PromotionPeriodVM.PromotionEndDate;
            PromotionPeriod.IsActive = PromotionPeriodVM.IsActive;
            PromotionPeriod.LoginIdentity = UserIdentity;
            Result result = PromotionPeriod.Update();
            if (result.EnumMember == PromotionsPeriodsValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == PromotionsPeriodsValidationEnum.RejectedBecauseOfPromotionStartDateIsGreaterThenPromotionEndDate.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationPromotionStartDateIsGreaterThenPromotionEndDateText);
            }
            else if (result.EnumMember == PromotionsPeriodsValidationEnum.RejectedBecauseOfAlreadyOnePromotionPeriodIsActive.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationAlreadyOnePromotionPeriodIsActiveText);
            }
            else if (result.EnumMember == PromotionsPeriodsValidationEnum.RejectedBecauseOfPromotionRecordExistWithThisPromotiosPeriodDates.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationPromotionRecordExistWithThesePromotiosPeriodDatesText);
            }
            return Json(new { PromotionPeriodID = PromotionPeriod.PromotionPeriodID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByPromotionPeriodID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("YearID,PeriodID,PromotionStartDateText,PromotionEndDateText")]
        public ActionResult Delete(PromotionsPeriodsViewModel PromotionPeriodVM)
        {
            PromotionsPeriodsBLL PromotionPeriod = new PromotionsPeriodsBLL();
            PromotionPeriod.PromotionPeriodID = PromotionPeriodVM.PromotionPeriodID;
            PromotionPeriod.LoginIdentity = UserIdentity;
            Result result = PromotionPeriod.Remove(PromotionPeriod.PromotionPeriodID);
            if (result.EnumMember == PromotionsPeriodsValidationEnum.Done.ToString())
            {

            }
            return Json(new { PromotionPeriodID = PromotionPeriod.PromotionPeriodID }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("PromotionsPeriods/GetInformationCardForPromotionConfirmed/{PromotionPeriodID}")]
        public JsonResult GetInformationCardForPromotionConfirmed(int PromotionPeriodID)
        {
            var list =  new PromotionCardsPrintingBLL().GetPromotionCardsPrintingByPromotionPeriodID(PromotionPeriodID)
                .Select(x => new
                {
                    PromotionCardPrintingID = x.PromotionCardPrintingID,
                    EmployeeCodeNo = x.EmployeesCodes.EmployeeCodeNo,
                    EmployeeNameAr = x.EmployeesCodes.Employee.EmployeeNameAr,
                    PromotionStartDate = x.PromotionsPeriod.PromotionStartDate,
                    PromotionEndDate = x.PromotionsPeriod.PromotionEndDate,
                    MaturityYear = x.PromotionsPeriod.Year.MaturityYear
                });
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult InformationCardForPromotionConfirmed(int id)
        {
            return View(this.GetByPromotionPeriodID(id));
        }

        [HttpPost]

        public ActionResult InformationCardForPromotionConfirmed(PromotionsPeriodsViewModel PromotionPeriodVM)
        {
            PromotionsPeriodsBLL PromotionPeriod = new PromotionsPeriodsBLL();
            PromotionPeriod.PromotionPeriodID = PromotionPeriodVM.PromotionPeriodID;
            PromotionPeriod.Year = new MaturityYearsBalancesBLL() { MaturityYearID = PromotionPeriodVM.YearID };
            PromotionPeriod.Period = new PeriodsBLL() { PeriodID = PromotionPeriodVM.PeriodID };//QualificationsDegreesVM.QualificationDegreeName;
            PromotionPeriod.PromotionStartDate = PromotionPeriodVM.PromotionStartDate;
            PromotionPeriod.PromotionEndDate = PromotionPeriodVM.PromotionEndDate;
            PromotionPeriod.IsActive = PromotionPeriodVM.IsActive;
            PromotionPeriod.LoginIdentity = UserIdentity;
            Result result = PromotionPeriod.Update();
            if (result.EnumMember == PromotionsPeriodsValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == PromotionsPeriodsValidationEnum.RejectedBecauseOfPromotionStartDateIsGreaterThenPromotionEndDate.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationPromotionStartDateIsGreaterThenPromotionEndDateText);
            }
            else if (result.EnumMember == PromotionsPeriodsValidationEnum.RejectedBecauseOfAlreadyOnePromotionPeriodIsActive.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationAlreadyOnePromotionPeriodIsActiveText);
            }
            else if (result.EnumMember == PromotionsPeriodsValidationEnum.RejectedBecauseOfPromotionRecordExistWithThisPromotiosPeriodDates.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationPromotionRecordExistWithThesePromotiosPeriodDatesText);
            }
            return Json(new { PromotionPeriodID = PromotionPeriod.PromotionPeriodID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private PromotionsPeriodsViewModel GetByPromotionPeriodID(int id)
        {
            PromotionsPeriodsBLL PromotionPeriod = new PromotionsPeriodsBLL().GetByPromotionPeriodID(id);
            PromotionsPeriodsViewModel PromotionPeriodVM = new PromotionsPeriodsViewModel();
            if (PromotionPeriod != null)
            {
                PromotionPeriodVM.PromotionPeriodID = PromotionPeriod.PromotionPeriodID;
                PromotionPeriodVM.YearID = PromotionPeriod.Year.MaturityYearID;
                PromotionPeriodVM.PeriodID = PromotionPeriod.Period.PeriodID;
                PromotionPeriodVM.MaturityYear = PromotionPeriod.Year.MaturityYear;
                PromotionPeriodVM.PeriodName = PromotionPeriod.Period.PeriodName;
                PromotionPeriodVM.PromotionStartDate = PromotionPeriod.PromotionStartDate;
                PromotionPeriodVM.PromotionEndDate = PromotionPeriod.PromotionEndDate;
                PromotionPeriodVM.IsActive = PromotionPeriod.IsActive.Value;
            }
            return PromotionPeriodVM;
        }
    }
}