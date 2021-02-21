using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Linq;

namespace HCM.Controllers
{
    public class HolidaysAttendanceController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["HolidayAttendanceID"] = null;
            ClearHolidayAttendanceDetailsFromSession();
            return View();
        }

        [HttpPost]
        [IgnoreModelProperties("HolidayAttendanceDetailRequest")]
        public ActionResult Create(HolidaysAttendanceViewModel HolidayAttendanceVM)
        {
            //--== HolidayAttendance Master DataBind ===
            HolidaysAttendanceBLL HolidayAttendance = new HolidaysAttendanceBLL();
            HolidayAttendance.HolidaySetting = new HolidaysSettingsBLL() { HolidaySettingID = HolidayAttendanceVM.HolidaySetting.HolidaySettingID };
            HolidayAttendance.Organization = new OrganizationsStructuresBLL() { OrganizationID = HolidayAttendanceVM.OrganizationID };
            HolidayAttendance.LoginIdentity = UserIdentity;
            //--== HolidayAttendance Details DataBind ==
            HolidayAttendance.HolidaysAttendanceDetails = GetHolidayAttendanceDetailsFromSession();
            Result result = HolidayAttendance.Add();
            Session["HolidayAttendanceID"] = HolidayAttendance.HolidayAttendanceID;
            if (result.EnumMember == HolidayAttendanceValidationEnum.RejectedBecauseEmployeeRequired.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
            }
            if (result.EnumMember == HolidayAttendanceValidationEnum.RejectedBecauseEmployeeAlreadyExistOnAnotherRecord.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExistOnAnotherRecordText +
                    "NewLine" + Resources.Globalization.EmployeeCodeNoText + " : " + ((EmployeesCareersHistoryBLL)result.Entity).EmployeeCode.EmployeeCodeNo +
                    "NewLine" + Resources.Globalization.EmployeeNameArText + " : " + ((EmployeesCareersHistoryBLL)result.Entity).EmployeeCode.Employee.EmployeeNameAr);
            }
            ResetEmployeeFromSession();
            return View("Index");
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByHolidayAttendanceID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("HolidayAttendanceDetailRequest")]
        public ActionResult EditHolidayAttendance(HolidaysAttendanceViewModel HolidayAttendanceVM)
        {
            HolidaysAttendanceBLL HolidayAttendance = new HolidaysAttendanceBLL();
            HolidayAttendance.HolidayAttendanceID = HolidayAttendanceVM.HolidayAttendanceID;
            HolidayAttendance.HolidaySetting = new HolidaysSettingsBLL() { HolidaySettingID = HolidayAttendanceVM.HolidaySetting.HolidaySettingID };
            HolidayAttendance.Organization = new OrganizationsStructuresBLL() { OrganizationID = HolidayAttendanceVM.OrganizationID };
            HolidayAttendance.LoginIdentity = UserIdentity;
            Result result = HolidayAttendance.Update();

            HolidaysAttendanceBLL HolidayAttendanceEntity = (HolidaysAttendanceBLL)result.Entity;
            if (result.EnumMember == HolidayAttendanceValidationEnum.Done.ToString())
            {
                Session["HolidayAttendanceID"] = ((HolidaysAttendanceBLL)result.Entity).HolidayAttendanceID;
                ClearHolidayAttendanceDetailsFromSession();
            }
            else if (result.EnumMember == HolidayAttendanceValidationEnum.RejectedBecauseOfEmployeeStatus.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
            }
            if (result.EnumMember == HolidayAttendanceValidationEnum.RejectedBecauseEmployeeAlreadyExistOnAnotherRecord.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExistText +
                    "NewLine" + Resources.Globalization.EmployeeCodeNoText + " : " + ((EmployeesCareersHistoryBLL)result.Entity).EmployeeCode.EmployeeCodeNo +
                    "NewLine" + Resources.Globalization.EmployeeNameArText + " : " + ((EmployeesCareersHistoryBLL)result.Entity).EmployeeCode.Employee.EmployeeNameAr);
            }

            return View(this.GetByHolidayAttendanceID(HolidayAttendanceVM.HolidayAttendanceID));
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByHolidayAttendanceID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByHolidayAttendanceID(id));
        }

        [HttpDelete]
        public ActionResult Delete(HolidaysAttendanceViewModel HolidayAttendanceVM)
        {
            HolidaysAttendanceBLL HolidayAttendanceBll = new HolidaysAttendanceBLL();
            HolidayAttendanceBll.HolidayAttendanceID = HolidayAttendanceVM.HolidayAttendanceID;
            HolidayAttendanceBll.LoginIdentity = UserIdentity;
            HolidayAttendanceBll.Remove();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [IgnoreModelProperties("HolidayAttendanceDetailRequest")]
        public HttpResponseMessage CreateHolidayAttendanceDetail(HolidaysAttendanceViewModel HolidayAttendanceVM)
        {
            List<HolidaysAttendanceDetailsBLL> HolidayAttendanceEmployeesList = GetHolidayAttendanceDetailsFromSession();
            if (HolidayAttendanceEmployeesList.Exists(x => x.EmployeeCareerHistory.EmployeeCareerHistoryID == HolidayAttendanceVM.HolidayAttendanceDetailRequest.EmployeeCareerHistory.EmployeeCareerHistoryID))
                throw new CustomException(Resources.Globalization.RecordAlreadyExistText);
            HolidayAttendanceEmployeesList.Add(HolidayAttendanceVM.HolidayAttendanceDetailRequest);
            Session["HolidaysAttendanceEmployees"] = HolidayAttendanceEmployeesList;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public JsonResult GetHolidayAttendanceEmployees()
        {
            List<HolidaysAttendanceDetailsBLL> HolidayAttendanceEmployeesList;
            if (Session["HolidaysAttendanceEmployees"] != null)
                HolidayAttendanceEmployeesList = (List<HolidaysAttendanceDetailsBLL>)Session["HolidaysAttendanceEmployees"];
            else
                HolidayAttendanceEmployeesList = new List<HolidaysAttendanceDetailsBLL>();

            return Json(new { data = HolidayAttendanceEmployeesList }, JsonRequestBehavior.AllowGet);
        }

        private List<HolidaysAttendanceDetailsBLL> GetHolidayAttendanceDetailsFromSession()
        {
            List<HolidaysAttendanceDetailsBLL> HolidayAttendanceEmployeesList;
            if (Session["HolidaysAttendanceEmployees"] != null)
                HolidayAttendanceEmployeesList = (List<HolidaysAttendanceDetailsBLL>)Session["HolidaysAttendanceEmployees"];
            else
                HolidayAttendanceEmployeesList = new List<HolidaysAttendanceDetailsBLL>();

            return HolidayAttendanceEmployeesList;
        }

        [HttpPost]
        [IgnoreModelProperties("HolidayAttendanceID,HolidaysAttendancetartDate,HolidayAttendanceEndDate,HolidayAttendancePeriod,Task,HolidayAttendanceDetailRequest")]
        public HttpResponseMessage ResetEmployeeFromSession()
        {
            ClearHolidayAttendanceDetailsFromSession();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public JsonResult GetHolidaysAttendance()
        {
            HolidaysAttendanceBLL HolidayAttendanceBLL = new HolidaysAttendanceBLL();
            List<HolidaysAttendanceBLL> list = HolidayAttendanceBLL.GetHolidaysAttendance();

            var lst = list.Select(d => new
            {
                d.HolidayAttendanceID,
                HolidaySettingStartDate = d.HolidaySetting.HolidaySettingStartDate,
                HolidaySettingEndDate = d.HolidaySetting.HolidaySettingEndDate,
                HolidaySettingPeriod = d.HolidaySetting.HolidaySettingPeriod,
                HolidayTypeName = d.HolidaySetting.HolidayType.HolidayTypeName,
                MaturityYear = d.HolidaySetting.MaturityYear.MaturityYear,
                OrganizationName = d.Organization.OrganizationName,
            });

            return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHolidayAttendanceEmployeesByHolidayAttendanceID(int id)
        {
            return Json(new { data = new HolidaysAttendanceDetailsBLL().GetHolidaysAttendanceDetailsByHolidayAttendanceID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpResponseMessage RemoveEmployeeFromDB(int id)
        {
            HolidaysAttendanceDetailsBLL HolidayAttendanceDetailBLL = new HolidaysAttendanceDetailsBLL();
            HolidayAttendanceDetailBLL.LoginIdentity = UserIdentity;
            HolidayAttendanceDetailBLL.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        //[IgnoreModelProperties("OverTimeID,OverTimeStartDate,OverTimeEndDate,OverTimePeriod,Task,OverTimeDetailRequest")]
        public HttpResponseMessage RemoveEmployeeFromHolidayAttendance(HolidaysAttendanceViewModel HolidayAttendanceVM)
        {
            List<HolidaysAttendanceDetailsBLL> OverTimeEmployeesList = GetHolidayAttendanceDetailsFromSession();
            OverTimeEmployeesList.RemoveAt(OverTimeEmployeesList.FindIndex(e => e.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo.Equals(HolidayAttendanceVM.HolidayAttendanceDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo)));
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage CreateHolidayAttendanceDetailDB(HolidaysAttendanceViewModel HolidayAttendanceVM)
        {
            if (string.IsNullOrEmpty(HolidayAttendanceVM.HolidayAttendanceDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo))
            {
                throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
            }


            HolidaysAttendanceBLL HolidayAttendanceBLL = new HolidaysAttendanceBLL() { HolidayAttendanceID = HolidayAttendanceVM.HolidayAttendanceID };

            HolidaysAttendanceDetailsBLL HolidaysAttendanceDetail = new HolidaysAttendanceDetailsBLL()
            {
                HolidaysAttendance = HolidayAttendanceBLL,
                EmployeeCareerHistory = new EmployeesCareersHistoryBLL()
                {
                    EmployeeCareerHistoryID = HolidayAttendanceVM.HolidayAttendanceDetailRequest.EmployeeCareerHistory.EmployeeCareerHistoryID,
                    EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = HolidayAttendanceVM.HolidayAttendanceDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID }
                },
                LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) }
            };


            Result result = new HolidaysAttendanceDetailsBLL().Add(HolidaysAttendanceDetail);
            if (result.EnumMember == HolidayAttendanceValidationEnum.RejectedBecauseEmployeeAlreadyExistOnAnotherRecord.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExistOnAnotherRecordText);
            }
            if (result.EnumMember == HolidayAttendanceValidationEnum.RejectedBecauseAlreadyExist.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExistText);
            }
            Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [NonAction]
        private HolidaysAttendanceViewModel GetByHolidayAttendanceID(int id)
        {
            HolidaysAttendanceBLL HolidayAttendanceBLL = new HolidaysAttendanceBLL().GetByHolidayAttendanceID(id);
            HolidaysAttendanceViewModel HolidayAttendanceVM = new HolidaysAttendanceViewModel();
            if (HolidayAttendanceBLL != null)
            {
                HolidayAttendanceVM.HolidayAttendanceID = HolidayAttendanceBLL.HolidayAttendanceID;
                HolidayAttendanceVM.HolidaySetting = HolidayAttendanceBLL.HolidaySetting;
                HolidayAttendanceVM.Organization = HolidayAttendanceBLL.Organization;
                HolidayAttendanceVM.CreatedDate = HolidayAttendanceBLL.CreatedDate;
                HolidayAttendanceVM.CreatedBy = HolidayAttendanceVM.GetCreatedByDisplayed(HolidayAttendanceBLL.CreatedBy);

            }
            return HolidayAttendanceVM;
        }

        private void ClearHolidayAttendanceDetailsFromSession()
        {
            Session["HolidaysAttendanceEmployees"] = null;
        }

        [HttpPost]
        [Route("{controller}/GetHolidayAttendanceDates/{MaturityYearID}/{HolidayTypeID}")]
        public JsonResult GetHolidayAttendanceDates(int MaturityYearID, int HolidayTypeID)
        {
            HolidaysSettingsBLL HolidaySetting = new HolidaysSettingsBLL().GetByMaturityYearIDAndHolidayTypeID(MaturityYearID, HolidayTypeID);
            bool isExist = HolidaySetting == null ? false : true;
            string startDate = "", endDate = "";
            if (isExist)
            {
                startDate = Globals.Calendar.GetUmAlQuraDate(HolidaySetting.HolidaySettingStartDate);
                endDate = Globals.Calendar.GetUmAlQuraDate(HolidaySetting.HolidaySettingEndDate);
            }
            return Json(new { HolidaySettingStartDate = startDate, HolidaySettingEndDate = endDate, IsExist = isExist, HolidaySettingID = HolidaySetting.HolidaySettingID }, JsonRequestBehavior.AllowGet);
        }
    }

}
