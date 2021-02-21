using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class HolidaysSettingsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        public JsonResult GetHolidaysSettings()
        {
            HolidaysSettingsBLL HolidaySettingBLL = new HolidaysSettingsBLL();
            return Json(new { data = HolidaySettingBLL.GetHolidaysSettings() }, JsonRequestBehavior.AllowGet);
        }

        [Route("HolidaysSettings/GetByMaturityYearID/{MaturityYearID}")]
        public JsonResult GetByMaturityYearID(int MaturityYearID)
        {
            return Json(new
            {
                data = ((new HolidaysSettingsBLL().GetByMaturityYearID(MaturityYearID))).Select(x => new
                {
                    MaturityYear = x.MaturityYear,
                    HolidayType = x.HolidayType,
                    HolidaySettingStartDate = x.HolidaySettingStartDate,
                    HolidaySettingID = x.HolidaySettingID,
                    HolidaySettingEndDate = x.HolidaySettingEndDate,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate,

                    HolidaySettingPeriod = x.HolidaySettingPeriod
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create(int id)
        {
            return View(new HolidaysSettingsViewModel() { MaturityYearBalance = new MaturityYearsBalancesBLL().GetByMaturityYearID(id) });
        }

        [HttpPost]
        public ActionResult Create(HolidaysSettingsViewModel HolidaySettingVM)
        {
            //--== HolidaySetting Master DataBind ===
            HolidaysSettingsBLL HolidaySetting = new HolidaysSettingsBLL();
            HolidaySetting.HolidaySettingEndDate = (DateTime)HolidaySettingVM.HolidaySettingEndDate;
            HolidaySetting.HolidaySettingStartDate = (DateTime)HolidaySettingVM.HolidaySettingStartDate;
            HolidaySetting.MaturityYear = HolidaySettingVM.MaturityYearBalance;
            HolidaySetting.HolidayType = HolidaySettingVM.HolidayType;
            HolidaySetting.LoginIdentity = UserIdentity;
            Result result = HolidaySetting.Add();
            if (result.EnumMember == HolidaySettingValidationEnum.RejectedBecauseOfHolidaysDurationShouldBeInTheSameHijriYear.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationHolidaysDurationShouldBeInTheSameHijriYearText);
            }

            if (result.EnumMember == HolidaySettingValidationEnum.RejectedBecauseOfConflictWithHolidaySetting.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationConflictWithHolidaySetting);
            }
            return Json(new { HolidaySettingID = HolidaySetting.HolidaySettingID, MaturityYearID = HolidaySetting.MaturityYear.MaturityYearID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByHolidaySettingID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("HolidaySettingDetailRequest")]
        public ActionResult EditHolidaySetting(HolidaysSettingsViewModel HolidaySettingVM)
        {
            HolidaysSettingsBLL HolidaySetting = new HolidaysSettingsBLL();
            HolidaySetting.HolidaySettingID = (int)HolidaySettingVM.HolidaySettingID;
            HolidaySetting.HolidaySettingStartDate = (DateTime)HolidaySettingVM.HolidaySettingStartDate;
            HolidaySetting.HolidaySettingEndDate = (DateTime)HolidaySettingVM.HolidaySettingEndDate;
            HolidaySetting.HolidayType = HolidaySettingVM.HolidayType;
            HolidaySetting.MaturityYear = HolidaySettingVM.MaturityYearBalance;
            HolidaySetting.LoginIdentity = UserIdentity;
            Result result = HolidaySetting.Update();

            if (result.EnumMember == HolidaySettingValidationEnum.RejectedBecauseOfHolidaysDurationShouldBeInTheSameHijriYear.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationHolidaysDurationShouldBeInTheSameHijriYearText);
            }
            if (result.EnumMember == HolidaySettingValidationEnum.RejectedBecauseOfEmployeeAssignToThisHolidaySetting.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeAssignToThisHolidaySettingText);
            }
            if (result.EnumMember == HolidaySettingValidationEnum.RejectedBecauseOfConflictWithHolidaySetting.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationConflictWithHolidaySetting);
            }
            HolidaysSettingsBLL HolidaySettingEntity = (HolidaysSettingsBLL)result.Entity;

            return View(HolidaySettingVM);
        }

        [HttpPost]
        [Route("{controller}/GetHolidaySettingEndDate/{HolidaySettingPeriod}/{HolidaySettingStartDate}")]
        public JsonResult GetHolidaySettingEndDate(int HolidaySettingPeriod, string HolidaySettingStartDate)
        {
            return Json(GetUmAlquraEndDate(HolidaySettingPeriod, HolidaySettingStartDate), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        [IgnoreModelProperties("HolidaySettingStartDate")]
        public ActionResult Delete(int id)
        {
            return View(this.GetByHolidaySettingID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByHolidaySettingID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("HolidayType,MaturityYearBalance,HolidaySettingStartDate")]
        public ActionResult Delete(HolidaysSettingsViewModel HolidaySettingVM)
        {
            HolidaysSettingsBLL HolidaySettingBll = new HolidaysSettingsBLL();
            HolidaySettingBll.LoginIdentity = UserIdentity;
            HolidaySettingBll.Remove((int)HolidaySettingVM.HolidaySettingID);
            return Json(new { HolidaySettingID = HolidaySettingVM.HolidaySettingID, MaturityYearID = HolidaySettingVM.MaturityYearBalance.MaturityYearID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private HolidaysSettingsViewModel GetByHolidaySettingID(int id)
        {
            HolidaysSettingsBLL HolidaySettingBLL = new HolidaysSettingsBLL().GetByHolidaySettingID(id);
            HolidaysSettingsViewModel HolidaySettingVM = new HolidaysSettingsViewModel();
            if (HolidaySettingBLL != null)
            {
                HolidaySettingVM.HolidaySettingID = HolidaySettingBLL.HolidaySettingID;
                HolidaySettingVM.HolidaySettingStartDate = HolidaySettingBLL.HolidaySettingStartDate.Date;
                HolidaySettingVM.HolidaySettingEndDate = HolidaySettingBLL.HolidaySettingEndDate;
                HolidaySettingVM.HolidaySettingPeriod = HolidaySettingBLL.HolidaySettingPeriod;
                HolidaySettingVM.MaturityYearBalance = HolidaySettingBLL.MaturityYear;
                HolidaySettingVM.HolidayType = HolidaySettingBLL.HolidayType;
                HolidaySettingVM.CreatedDate = HolidaySettingBLL.CreatedDate;
                HolidaySettingVM.CreatedBy = HolidaySettingVM.GetCreatedByDisplayed(HolidaySettingBLL.CreatedBy);
            }
            return HolidaySettingVM;
        }
    }
}
