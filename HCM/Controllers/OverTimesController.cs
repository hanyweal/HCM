using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Classes.Helpers;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class OverTimesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "OverTimeID", Resources.Globalization.SequenceNoText },
                { "OverTimeStartDate", Resources.Globalization.OverTimeStartDateText },
                { "OverTimeEndDate", Resources.Globalization.OverTimeEndDateText },
                { "OverTimePeriod", Resources.Globalization.OverTimePeriodText },
                { "Tasks", Resources.Globalization.OverTimeTaskText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetAllOverTimes(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllOverTimes()
        {
            var data = new OverTimesBLL().GetOverTimes()
                .Select(x => new
                {
                    OverTimeID = x.OverTimeID,
                    OverTimeStartDate = Globals.Calendar.GetUmAlQuraDate(x.OverTimeStartDate),
                    OverTimeEndDate = Globals.Calendar.GetUmAlQuraDate(x.OverTimeEndDate),
                    OverTimePeriod = x.OverTimePeriod,
                    Tasks = x.Tasks
                });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["OverTimeID"] = null;
            ClearOverTimeDetailsFromSession();
            return View();
        }

        [HttpPost]
        [IgnoreModelProperties("OverTimeDetailRequest")]
        public ActionResult Create(OverTimesViewModel overTimeVM)
        {
            //--== OverTime Master DataBind ===
            OverTimesBLL overTime = new OverTimesBLL();
            overTime.OverTimeEndDate = (DateTime)overTimeVM.OverTimeEndDate;
            overTime.OverTimeStartDate = overTimeVM.OverTimeStartDate;
            overTime.Tasks = overTimeVM.Tasks;
            overTime.FridayHoursAvg = overTimeVM.FridayHoursAvg != null ? (double)overTimeVM.FridayHoursAvg : Convert.ToDouble(null);
            overTime.SaturdayHoursAvg = overTimeVM.SaturdayHoursAvg != null ? (double)overTimeVM.SaturdayHoursAvg : Convert.ToDouble(null);
            overTime.WeekWorkHoursAvg = overTimeVM.WeekWorkHoursAvg;
            overTime.Requester = overTimeVM.Requester;
            overTime.LoginIdentity = UserIdentity;
            //--== OverTime Details DataBind ==
            overTime.OverTimesDetails = GetOverTimeDetailsFromSession();
            //--== OverTime Days DataBind ==
            overTime.OverTimesDays = GetOverTimeDays(overTimeVM.OverTimesDays);
            Result result = overTime.Add();
            Session["OverTimeID"] = overTime.OverTimeID;
            Session["OverTimesEmployees"] = null;
            if (result.EnumMember == OverTimeValidationEnum.RejectedBecauseEmployeeRequired.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
            }
            ResetEmployeeFromSession();
            //return View("Index");
            return Json(new { OverTimeID = overTime.OverTimeID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByOverTimeID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("OverTimeDetailRequest,OverTimesDays")]
        public ActionResult EditOverTime(OverTimesViewModel OverTimeVM)
        {
            OverTimesBLL OverTime = new OverTimesBLL();
            OverTime.OverTimeID = OverTimeVM.OverTimeID;
            OverTime.OverTimeStartDate = OverTimeVM.OverTimeStartDate;
            OverTime.OverTimeEndDate = (DateTime)OverTimeVM.OverTimeEndDate;
            OverTime.Tasks = OverTimeVM.Tasks;
            OverTime.FridayHoursAvg = OverTimeVM.FridayHoursAvg != null ? (double)OverTimeVM.FridayHoursAvg : Convert.ToDouble(null);
            OverTime.SaturdayHoursAvg = OverTimeVM.SaturdayHoursAvg != null ? (double)OverTimeVM.SaturdayHoursAvg : Convert.ToDouble(null);
            OverTime.WeekWorkHoursAvg = OverTimeVM.WeekWorkHoursAvg;
            OverTime.Requester = OverTimeVM.Requester;
            OverTime.LoginIdentity = UserIdentity;
            Result result = OverTime.Update();

            OverTimesBLL OverTimeEntity = (OverTimesBLL)result.Entity;
            if (result.EnumMember == OverTimeValidationEnum.Done.ToString())
            {
                Session["OverTimeID"] = ((OverTimesBLL)result.Entity).OverTimeID;
                ClearOverTimeDetailsFromSession();
            }
            else if (result.EnumMember == OverTimeValidationEnum.RejectedBecauseOfEmployeeStatus.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
            }


            return View(OverTimeVM);
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByOverTimeID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByOverTimeID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("OverTimeID,OverTimeStartDate,OverTimeEndDate,OverTimePeriod,Task,OverTimeDetailRequest,OverTimesDays")]
        public ActionResult Delete(OverTimesViewModel OverTimeVM)
        {
            OverTimesBLL overTimeBll = new OverTimesBLL();
            overTimeBll.OverTimeID = OverTimeVM.OverTimeID;
            overTimeBll.LoginIdentity = UserIdentity;
            overTimeBll.Remove();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [IgnoreModelProperties("OverTimeID,Task")]
        public HttpResponseMessage CreateOverTimeDetail(OverTimesViewModel OverTimeVM)
        {
            List<OverTimesDetailsBLL> OverTimeEmployeesList = GetOverTimeDetailsFromSession();

            if (string.IsNullOrEmpty(OverTimeVM.OverTimeDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo))
                throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
            else if (OverTimeEmployeesList.FindIndex(e => e.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo.Equals(OverTimeVM.OverTimeDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo)) > -1)
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExistText);

            DateTime StartDate, EndDate;
            StartDate = OverTimeVM.OverTimeStartDate;
            EndDate = (DateTime)OverTimeVM.OverTimeEndDate;

            OverTimesBLL overtime = new OverTimesBLL()
            {
                OverTimeStartDate = StartDate,
                OverTimeEndDate = EndDate,
                OverTimesDays = GetOverTimeDays(OverTimeVM.OverTimesDays)
            };

            Result result = new OverTimesDetailsBLL()
            {
                OverTime = overtime,
                EmployeeCareerHistory = new EmployeesCareersHistoryBLL()
                {
                    EmployeeCareerHistoryID = OverTimeVM.OverTimeDetailRequest.EmployeeCareerHistory.EmployeeCareerHistoryID,
                    EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = OverTimeVM.OverTimeDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID }
                }
            }.IsValid();

            if (result.EnumMember == OverTimeValidationEnum.Done.ToString())
            {
                OverTimeEmployeesList.Add(OverTimeVM.OverTimeDetailRequest);
                Session["OverTimesEmployees"] = OverTimeEmployeesList;
            }
            else if (result.EnumMember == OverTimeValidationEnum.RejectedBecauseOfOverTimeDatesMustBeInSameFinancialYear.ToString())
            {
                FinancialYearsBLL FinancialYearBLL = (FinancialYearsBLL)result.Entity;
                throw new CustomException(Resources.Globalization.ValidationOverTimeDatesMustBeInSameFinancialYearText + " NewLine " +
                                                                Resources.Globalization.FinancialYearInfoText + FinancialYearBLL.FinancialYear + " NewLine " +
                                                                Resources.Globalization.FinancialYearStartDateText + FinancialYearBLL.FinancialYearStartDate.Date.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]) + " NewLine " +
                                                                Resources.Globalization.FinancialYearEndDateText + FinancialYearBLL.FinancialYearEndDate.Date.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]));
            }
            Classes.Helpers.CommonHelper.ConflictValidationMessage(result);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("OverTimeID,OverTimeStartDate,OverTimeEndDate,OverTimePeriod,Task,OverTimeDetailRequest,OverTimesDays")]
        public HttpResponseMessage RemoveEmployeeFromOverTime(OverTimesViewModel OverTimeVM)
        {
            List<OverTimesDetailsBLL> OverTimeEmployeesList = GetOverTimeDetailsFromSession();
            OverTimeEmployeesList.RemoveAt(OverTimeEmployeesList.FindIndex(e => e.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo.Equals(OverTimeVM.OverTimeDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo)));
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public JsonResult GetOverTimeEmployees()
        {
            List<OverTimesDetailsBLL> OverTimeEmployeesList;
            if (Session["OverTimesEmployees"] != null)
                OverTimeEmployeesList = (List<OverTimesDetailsBLL>)Session["OverTimesEmployees"];
            else
                OverTimeEmployeesList = new List<OverTimesDetailsBLL>();

            return Json(new { data = OverTimeEmployeesList }, JsonRequestBehavior.AllowGet);
        }

        private List<OverTimesDetailsBLL> GetOverTimeDetailsFromSession()
        {
            List<OverTimesDetailsBLL> OverTimeEmployeesList;
            if (Session["OverTimesEmployees"] != null)
                OverTimeEmployeesList = (List<OverTimesDetailsBLL>)Session["OverTimesEmployees"];
            else
                OverTimeEmployeesList = new List<OverTimesDetailsBLL>();

            return OverTimeEmployeesList;
        }

        private List<OverTimesDaysBLL> GetOverTimeDays(string OverTimesDays)
        {
            List<OverTimesDaysBLL> OverTimeDaysList = new List<OverTimesDaysBLL>();
            OverTimesDaysBLL OverTimeDayBLL;
            string[] OverTimesDaysArray = OverTimesDays.Split(',');
            foreach (string day in OverTimesDaysArray)
            {
                DateTime OverTimesDay;
                if (DateTime.TryParse(day, out OverTimesDay))
                {
                    OverTimeDayBLL = new OverTimesDaysBLL()
                    {
                        OverTimeDay = OverTimesDay
                    };
                    if (!OverTimeDaysList.Exists(x => x.OverTimeDay == OverTimeDayBLL.OverTimeDay))
                    {
                        OverTimeDaysList.Add(OverTimeDayBLL);
                    }
                }

            }

            return OverTimeDaysList;
        }

        [HttpPost]
        [IgnoreModelProperties("OverTimeID,OverTimeStartDate,OverTimeEndDate,OverTimePeriod,Task,OverTimeDetailRequest")]
        public HttpResponseMessage ResetEmployeeFromSession()
        {
            ClearOverTimeDetailsFromSession();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public JsonResult GetOverTimes()
        {
            OverTimesBLL overTimeBLL = new OverTimesBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            };
            var data = overTimeBLL.GetOverTimes(out TotalRecordsOut, out RecFilterOut)
                .Select(x => new
                {
                    OverTimeID = x.OverTimeID,
                    OverTimeStartDate = x.OverTimeStartDate,
                    OverTimeStartDateGr = x.OverTimeStartDate.ToString(),
                    OverTimeEndDate = x.OverTimeEndDate,
                    OverTimeEndDateGr = x.OverTimeEndDate.ToString(),
                    OverTimePeriod = x.OverTimePeriod,
                    Tasks = x.Tasks
                });
            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);

            //return Json(new { data = overTimeBLL.GetOverTimes() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOverTimeEmployeesByOverTimeID(int id)
        {
            return Json(new { data = new OverTimesDetailsBLL().GetOverTimeDetailsByOverTimeID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("OverTimeID,OverTimeStartDate,OverTimeEndDate,OverTimePeriod,Task,OverTimesDays")]
        public HttpResponseMessage RemoveEmployeeFromDB(int id)
        {
            OverTimesDetailsBLL overTimeDetailBLL = new OverTimesDetailsBLL();
            overTimeDetailBLL.LoginIdentity = UserIdentity;
            overTimeDetailBLL.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("OverTimeID,OverTimePeriod,Task,OverTimesDays")]
        public HttpResponseMessage CreateOverTimeDetailDB(OverTimesViewModel OverTimeVM)
        {
            if (string.IsNullOrEmpty(OverTimeVM.OverTimeDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo))
            {
                throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
            }

            DateTime StartDate, EndDate;
            StartDate = OverTimeVM.OverTimeStartDate;
            EndDate = (DateTime)OverTimeVM.OverTimeEndDate;
            OverTimesBLL overtime = new OverTimesBLL() { OverTimeID = OverTimeVM.OverTimeID, OverTimeStartDate = OverTimeVM.OverTimeStartDate, OverTimeEndDate = (DateTime)OverTimeVM.OverTimeEndDate };

            OverTimesDetailsBLL overTimesDetail = new OverTimesDetailsBLL()
            {
                OverTime = overtime,
                EmployeeCareerHistory = new EmployeesCareersHistoryBLL()
                {
                    EmployeeCareerHistoryID = OverTimeVM.OverTimeDetailRequest.EmployeeCareerHistory.EmployeeCareerHistoryID,
                    EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = OverTimeVM.OverTimeDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID }
                },
                LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) }
            };
            Result result = overTimesDetail.IsValid();

            if (result.EnumMember == OverTimeValidationEnum.Done.ToString())
            {
                result = new OverTimesDetailsBLL().Add(overTimesDetail);
            }
            else if (result.EnumMember == OverTimeValidationEnum.RejectedBecauseOfOverTimeDatesMustBeInSameFinancialYear.ToString())
            {
                FinancialYearsBLL FinancialYearBLL = (FinancialYearsBLL)result.Entity;
                throw new CustomException(Resources.Globalization.ValidationOverTimeDatesMustBeInSameFinancialYearText + "NewLine" +
                                                                Resources.Globalization.FinancialYearInfoText + FinancialYearBLL.FinancialYear + ": NewLine" +
                                                                Resources.Globalization.FinancialYearStartDateText + FinancialYearBLL.FinancialYearStartDate.Date.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]) + "NewLine" +
                                                                Resources.Globalization.FinancialYearEndDateText + FinancialYearBLL.FinancialYearEndDate.Date.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]));
            }
            else if (result.EnumMember == OverTimeValidationEnum.RejectedBecauseAlreadyExist.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExistText);
            }
            Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
          
            return new HttpResponseMessage(HttpStatusCode.OK);
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
                OverTimeVM.IsApprove = OverTimeBLL.IsApproved;
                OverTimeVM.OverTimesDays = BindOverTimesDaysToViewModel(OverTimeBLL.OverTimesDays);
            }
            return OverTimeVM;
        }

        [CustomAuthorize]
        public ActionResult PrintOverTime(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.OverTimes.ToString(), id));
        }

        [CustomAuthorize]
        public ActionResult PrintAllOverTimesByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.OverTimes.ToString(), id));
        }

        private void ClearOverTimeDetailsFromSession()
        {
            Session["OverTimesEmployees"] = null;
        }

        [HttpPost]
        [Route("{controller}/GetOverTimeEndDate/{OverTimePeriod}/{OverTimeStartDate}")]
        public JsonResult GetOverTimeEndDate(int OverTimePeriod, string OverTimeStartDate)
        {
            return GetUmAlquraEndDate(OverTimePeriod, OverTimeStartDate);
        }

        public JsonResult GetOverTimeByOverTimeID(int OverTimeID)
        {
            OverTimesViewModel vm = this.GetByOverTimeID(OverTimeID);
            if (vm == null) vm = new OverTimesViewModel();
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        private string BindOverTimesDaysToViewModel(List<OverTimesDaysBLL> OverTimesDays)
        {
            string overTimesDays = string.Empty;
            for (int i = 0; i < OverTimesDays.Count; i++)
            {
                overTimesDays += OverTimesDays[i].OverTimeDay.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]) + ",";

            }
            overTimesDays.TrimEnd(',');
            return overTimesDays;


        }

        [HttpPost]
        [Route("{controller}/GetDaysBetweenStartAndEndDate/{OverTimePeriod}/{OverTimeStartDate}")]
        public JsonResult GetDaysBetweenStartAndEndDate(int OverTimePeriod, string OverTimeStartDate)
        {
            string UmAlquraEndDate = string.Empty;
            if (!string.IsNullOrEmpty(OverTimeStartDate) && OverTimePeriod != 0)
            {
                string GrStartDate = Globals.Calendar.UmAlquraToGreg(OverTimeStartDate);
                CultureInfo enCul = new CultureInfo("en-US");
                enCul.DateTimeFormat.Calendar = new GregorianCalendar(GregorianCalendarTypes.USEnglish);
                Thread.CurrentThread.CurrentUICulture = enCul;
                Thread.CurrentThread.CurrentCulture = enCul;
                DateTime GrEndDate = Convert.ToDateTime(GrStartDate).AddDays(OverTimePeriod - 1).Date;

                DateTime GrStartDateLoop = Convert.ToDateTime(GrStartDate);
                while (GrStartDateLoop <= GrEndDate)
                {

                    UmAlquraEndDate += Globals.Calendar.GetUmAlQuraDate(GrStartDateLoop);
                    if (GrStartDateLoop < GrEndDate)
                        UmAlquraEndDate += ",";

                    GrStartDateLoop = GrStartDateLoop.AddDays(1);
                }
            }

            return Json(new { data = UmAlquraEndDate }, JsonRequestBehavior.AllowGet);
        }
    }
}
