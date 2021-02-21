using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Web.Mvc;
using System.Data;
using System.Linq;

namespace HCM.Controllers
{
    public class StopWorksController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["StopWorkID"] = null;
            return View();
        }

        [HttpPost]
        [IgnoreModelProperties("StopWorkEndDate")]
        public ActionResult Create(StopWorksViewModel stopWorkVM)
        {
            //--== StopWork Master DataBind ===
            StopWorksBLL stopWork = new StopWorksBLL();
            stopWork.StopWorkStartDate = stopWorkVM.StopWorkStartDate;
            stopWork.StopWorkEndDate = stopWorkVM.StopWorkEndDate;
            stopWork.StartStopWorkDecisionNumber = stopWorkVM.StartStopWorkDecisionNumber;
            stopWork.StartStopWorkDecisionDate = stopWorkVM.StartStopWorkDecisionDate;
            stopWork.EndStopWorkDecisionNumber = stopWorkVM.EndStopWorkDecisionNumber;
            stopWork.EndStopWorkDecisionDate = stopWorkVM.EndStopWorkDecisionDate;
            stopWork.Note = stopWorkVM.Note;
            stopWork.StopPoint = stopWorkVM.StopPoint;
            stopWork.StopWorkType = stopWorkVM.StopWorkType;
            stopWork.IsConvicted = stopWorkVM.IsConvicted;
            stopWork.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(stopWorkVM.EmployeeCareerHistoryID);
            stopWork.LoginIdentity = UserIdentity;
            Result result = stopWork.Add();
            Session["StopWorkID"] = stopWork.StopWorkID;


            if ((System.Type)result.EnumType == typeof(StopWorkValidationEnum))
            {
                if (result.EnumMember == StopWorkValidationEnum.RejectedBecauseOfEndDateShouldBeMoreThanStartDate.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEndDateShouldBeMoreThanStartDateText);
                }
            }
            if ((System.Type)result.EnumType == typeof(StopWorkValidationEnum))
            {
                if (result.EnumMember == StopWorkValidationEnum.RejectedBecauseOfThereIsAnotherStopWorkNotEnding.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationThereIsAnotherStopWorkNotEndingText);
                }
            }


            if ((System.Type)result.EnumType == typeof(NoConflictWithOtherProcessValidationEnum))
            {
                if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithStopWork.ToString())
                {
                    //throw new CustomException(Resources.Globalization.ValidationConflictWithStopWorkText);
                    throw new CustomException(Resources.Globalization.ValidationConflictWithAssigningText);
                }
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithVacationText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithOverTimeText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithInternshipScholarshipText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithDelegationText);
                //}
            }
            //return View("Index");
            return Json(new { StopWorkID = stopWork.StopWorkID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByStopWorkID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("StopWorkEndDate")]
        public ActionResult EditStopWork(StopWorksViewModel StopWorkVM)
        {
            StopWorksBLL StopWork = new StopWorksBLL();
            StopWork.StopWorkID = StopWorkVM.StopWorkID;
            StopWork.StopWorkStartDate = StopWorkVM.StopWorkStartDate;
            StopWork.StopWorkEndDate = StopWorkVM.StopWorkEndDate;
            StopWork.StartStopWorkDecisionNumber = StopWorkVM.StartStopWorkDecisionNumber;
            StopWork.StartStopWorkDecisionDate = StopWorkVM.StartStopWorkDecisionDate;
            StopWork.EndStopWorkDecisionNumber = StopWorkVM.EndStopWorkDecisionNumber;
            StopWork.EndStopWorkDecisionDate = StopWorkVM.EndStopWorkDecisionDate;
            StopWork.Note = StopWorkVM.Note;
            StopWork.StopPoint = StopWorkVM.StopPoint;
            StopWork.StopWorkType = StopWorkVM.StopWorkType;
            StopWork.LoginIdentity = UserIdentity;
            StopWork.IsConvicted = StopWorkVM.IsConvicted;
            StopWork.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(StopWorkVM.EmployeeCareerHistoryID);
            Result result = StopWork.Update();

            StopWorksBLL StopWorkEntity = (StopWorksBLL)result.Entity;
            if ((System.Type)result.EnumType == typeof(StopWorkValidationEnum))
            {
                if (result.EnumMember == StopWorkValidationEnum.RejectedBecauseOfEndDateShouldBeMoreThanStartDate.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEndDateShouldBeMoreThanStartDateText);
                }
            }
            if ((System.Type)result.EnumType == typeof(StopWorkValidationEnum))
            {
                if (result.EnumMember == StopWorkValidationEnum.RejectedBecauseOfThereIsAnotherStopWorkNotEnding.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationThereIsAnotherStopWorkNotEndingText);
                }
            }
            if ((System.Type)result.EnumType == typeof(NoConflictWithOtherProcessValidationEnum))
            {
                if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.Done.ToString())
                {
                }
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithStopWork.ToString())
                //{
                //    //throw new CustomException(Resources.Globalization.ValidationConflictWithStopWorkText);
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithAssigningText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithVacationText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithOverTimeText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithInternshipScholarshipText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithDelegationText);
                //}
            }


            return View(GetByStopWorkID(StopWorkVM.StopWorkID));
        }

        [CustomAuthorize]
        public ActionResult End(int id)
        {
            StopWorksViewModel StopWorkVM = this.GetByStopWorkID(id);
            StopWorkVM.IsConvicted = StopWorkVM.IsConvicted ?? false;
            return View(StopWorkVM);
        }

        [HttpPost]
        [ActionName("End")]
        [IgnoreModelProperties("StopWorkType")]
        public ActionResult EndStopWork(StopWorksViewModel StopWorkVM)
        {
            StopWorksBLL StopWork = new StopWorksBLL().GetByStopWorkID(StopWorkVM.StopWorkID);
            StopWork.IsConvicted = StopWorkVM.IsConvicted;
            StopWork.StopWorkEndDate = StopWorkVM.StopWorkEndDate;
            StopWork.Note = StopWorkVM.Note;
            StopWork.LoginIdentity = UserIdentity;
            StopWork.EmployeeCareerHistory = StopWork.EmployeeCareerHistory;
            Result result = StopWork.EndStopWork();

            StopWorksBLL StopWorkEntity = (StopWorksBLL)result.Entity;
            if ((System.Type)result.EnumType == typeof(StopWorkValidationEnum))
            {
                if (result.EnumMember == StopWorkValidationEnum.RejectedBecauseOfEndDateShouldBeMoreThanStartDate.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEndDateShouldBeMoreThanStartDateText);
                }
            }
            if ((System.Type)result.EnumType == typeof(NoConflictWithOtherProcessValidationEnum))
            {
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithStopWork.ToString())
                //{
                //    //throw new CustomException(Resources.Globalization.ValidationConflictWithStopWorkText);
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithAssigningText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithVacationText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithOverTimeText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithInternshipScholarshipText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithDelegationText);
                //}
            }


            return View(GetByStopWorkID(StopWorkVM.StopWorkID));
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByStopWorkID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByStopWorkID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("StopWorkID,StopWorkStartDate,StopWorkEndDate,StopWorkType")]
        public ActionResult Delete(StopWorksViewModel StopWorkVM)
        {
            StopWorksBLL stopWorkBll = new StopWorksBLL();
            stopWorkBll.StopWorkID = StopWorkVM.StopWorkID;
            stopWorkBll.Remove();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[IgnoreModelProperties("StopWorkID,Task")] 
        //public HttpResponseMessage CreateStopWorkDetail(StopWorksViewModel StopWorkVM)
        //{
        //    //StopWorksViewModel StopWorkVM = new StopWorksViewModel();
        //    List<StopWorksDetailsBLL> StopWorkEmployeesList = GetStopWorkDetailsFromSession();

        //    if (string.IsNullOrEmpty(StopWorkVM.StopWorkDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo))
        //    {
        //        throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
        //    }
        //    else if (StopWorkEmployeesList.FindIndex(e => e.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo.Equals(StopWorkVM.StopWorkDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo)) > -1)
        //    {
        //        throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExist);
        //    }

        //    DateTime StartDate, EndDate;
        //    StartDate = StopWorkVM.StopWorkStartDate;
        //    EndDate = (DateTime)StopWorkVM.StopWorkEndDate;

        //    StopWorksBLL overtime = new StopWorksBLL()
        //    {
        //        StopWorkStartDate = StartDate,
        //        StopWorkEndDate = EndDate,
        //    };

        //    Result result = new StopWorksDetailsBLL()
        //    {
        //        StopWorks = overtime,
        //        EmployeeCareerHistory = new EmployeesCareersHistoryBLL()
        //        {
        //            EmployeeCareerHistoryID = StopWorkVM.StopWorkDetailRequest.EmployeeCareerHistory.EmployeeCareerHistoryID,
        //            EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = StopWorkVM.StopWorkDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID }
        //        }
        //    }.IsValid();


        //    if (result.EnumMember == StopWorkValidationEnum.Done.ToString())
        //    {
        //        StopWorkEmployeesList.Add(StopWorkVM.StopWorkDetailRequest);
        //        Session["StopWorksEmployees"] = StopWorkEmployeesList;
        //    }
        //    else if (result.EnumMember == StopWorkValidationEnum.RejectedBecauseOfStopWorkDatesMustBeInSameFinancialYear.ToString())
        //    {
        //        FinancialYearsBLL FinancialYearBLL = (FinancialYearsBLL)result.Entity;
        //        throw new CustomException(Resources.Globalization.ValidationStopWorkDatesMustBeInSameFinancialYearText + "NewLine" +
        //                                                        Resources.Globalization.FinancialYearInfoText + FinancialYearBLL.FinancialYear + ": NewLine" +
        //                                                        Resources.Globalization.FinancialYearStartDateText + FinancialYearBLL.FinancialYearStartDate.Date.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]) + "NewLine" +
        //                                                        Resources.Globalization.FinancialYearEndDateText + FinancialYearBLL.FinancialYearEndDate.Date.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]));
        //    }
        //    else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString())
        //    {
        //        throw new CustomException(Resources.Globalization.ValidationConflictWithDelegationText);
        //    }
        //    else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithStopWork.ToString())
        //    {
        //        throw new CustomException(Resources.Globalization.ValidationConflictWithStopWorkText);
        //    }
        //    else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
        //    {
        //        throw new CustomException(Resources.Globalization.ValidationConflictWithInternshipScholarshipText);
        //    }
        //    else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString())
        //    {
        //        throw new CustomException(Resources.Globalization.ValidationConflictWithVacationText);
        //    }


        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        //[HttpPost]
        //[IgnoreModelProperties("StopWorkID,StopWorkStartDate,StopWorkEndDate,StopWorkPeriod,Task,StopWorkDetailRequest")]
        //public HttpResponseMessage RemoveEmployeeFromStopWork(StopWorksViewModel StopWorkVM)
        //{
        //    List<StopWorksDetailsBLL> StopWorkEmployeesList = GetStopWorkDetailsFromSession();
        //    StopWorkEmployeesList.RemoveAt(StopWorkEmployeesList.FindIndex(e => e.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo.Equals(StopWorkVM.StopWorkDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo)));
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        //public JsonResult GetStopWorkEmployees()
        //{
        //    List<StopWorksDetailsBLL> StopWorkEmployeesList;
        //    if (Session["StopWorksEmployees"] != null)
        //        StopWorkEmployeesList = (List<StopWorksDetailsBLL>)Session["StopWorksEmployees"];
        //    else
        //        StopWorkEmployeesList = new List<StopWorksDetailsBLL>();

        //    return Json(new { data = StopWorkEmployeesList }, JsonRequestBehavior.AllowGet);
        //}


        //[HttpPost]
        //[IgnoreModelProperties("StopWorkID,StopWorkStartDate,StopWorkEndDate,StopWorkPeriod,Task,StopWorkDetailRequest")]
        //public HttpResponseMessage ResetEmployeeFromSession()
        //{
        //    ClearStopWorkDetailsFromSession();
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        public JsonResult GetStopWorks()
        {
            var data = new StopWorksBLL().GetStopWorks()
                .Select(x => new
                {
                    x.StopWorkID,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeIDNo = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeIDNo,
                    //DelegationStartDate = Globals.Calendar.GetUmAlQuraDate(x.DelegationStartDate),
                    x.StopWorkStartDate,
                    x.StopWorkEndDate,
                    IsApproved = x.IsConvicted,
                    x.IsConvicted
                });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private StopWorksViewModel GetByStopWorkID(int id)
        {
            StopWorksBLL StopWorkBLL = new StopWorksBLL().GetByStopWorkID(id);
            StopWorksViewModel StopWorkVM = new StopWorksViewModel();
            if (StopWorkBLL != null)
            {
                StopWorkVM.StopWorkID = StopWorkBLL.StopWorkID;
                StopWorkVM.StopWorkStartDate = StopWorkBLL.StopWorkStartDate.Date;
                StopWorkVM.StopWorkEndDate = StopWorkBLL.StopWorkEndDate;
                StopWorkVM.StartStopWorkDecisionNumber = StopWorkBLL.StartStopWorkDecisionNumber;
                StopWorkVM.StartStopWorkDecisionDate = StopWorkBLL.StartStopWorkDecisionDate;
                StopWorkVM.EndStopWorkDecisionNumber = StopWorkBLL.EndStopWorkDecisionNumber;
                StopWorkVM.EndStopWorkDecisionDate = StopWorkBLL.EndStopWorkDecisionDate;
                StopWorkVM.Note = StopWorkBLL.Note;
                //StopWorkVM.IsConvicted = StopWorkBLL.IsConvicted.HasValue ? StopWorkBLL.IsConvicted.Value : false;
                StopWorkVM.IsConvicted = StopWorkBLL.IsConvicted;
                StopWorkVM.StopPoint = StopWorkBLL.StopPoint;
                StopWorkVM.StopWorkCategory = StopWorkBLL.StopWorkType.StopWorkCategory;
                StopWorkVM.StopWorkType = StopWorkBLL.StopWorkType;
                StopWorkVM.CreatedDate = StopWorkBLL.CreatedDate;
                StopWorkVM.CreatedBy = StopWorkVM.GetCreatedByDisplayed(StopWorkBLL.CreatedBy);
                StopWorkVM.EmployeeCareerHistory = StopWorkBLL.EmployeeCareerHistory;
                StopWorkVM.Employee = new EmployeesViewModel()
                {
                    EmployeeCodeID = StopWorkBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                    EmployeeCodeNo = StopWorkBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = StopWorkBLL.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr
                };
            }
            return StopWorkVM;
        }

        //
        //// GET: /KSACities/
        //[HttpGet]
        //public JsonResult GetStopWorksTypesAll()
        //{
        //    //int KSARegionID = 1;
        //    return Json(new { data = new StopWorksTypesBLL().GetStopWorksTypes() }, JsonRequestBehavior.AllowGet);
        //}

        //
        // GET: /KSACities/
        [HttpGet]
        public JsonResult GetStopWorksTypes(int id)
        {
            //int KSARegionID = 1;
            return Json(new { data = new StopWorksTypesBLL().GetByStopWorkCategoryID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("{controller}/GetStopWorkEndDate/{StopWorkPeriod}/{StopWorkStartDate}")]
        public JsonResult GetStopWorkEndDate(int StopWorkPeriod, string StopWorkStartDate)
        {
            return Json(GetUmAlquraEndDate(StopWorkPeriod, StopWorkStartDate), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStopWorkByStopWorkID(int StopWorkID)
        {
            StopWorksViewModel vm = this.GetByStopWorkID(StopWorkID);
            if (vm == null) vm = new StopWorksViewModel();
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult PrintStopWork(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.StopWork.ToString(), id));
        }
    }
}
