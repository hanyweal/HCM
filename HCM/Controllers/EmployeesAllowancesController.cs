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
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class EmployeesAllowancesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "EmployeeAllowanceID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "AllowanceName", Resources.Globalization.AllowanceNameText },
                { "AllowanceStartDate", Resources.Globalization.AllowanceStartDateText },
                { "JobName", Resources.Globalization.AssigningIsFinishedText },
                { "IsActive", Resources.Globalization.IsActiveText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetAllEmployeesAllowances(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllEmployeesAllowances()
        {
            var data = new EmployeesAllowancesBLL().GetEmployeesAllowances()
               .Select(x => new
               {
                   EmployeeAllowanceID = x.EmployeeAllowanceID,
                   AllowanceName = x.Allowance.AllowanceName,
                   EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                   EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                   JobName = x.EmployeeCareerHistory.OrganizationJob.Job.JobName,
                   //EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCareerHistoryByEmployeeCareerHistoryID(item.EmployeeCareerHistoryID),                        
                   AllowanceStartDate = Globals.Calendar.GetUmAlQuraDate(x.AllowanceStartDate),
                   IsActive = x.IsActive
               });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeesAllowances()
        {
            EmployeesAllowancesBLL EmployeesAllowancesBLL = new EmployeesAllowancesBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            };
            var data = EmployeesAllowancesBLL.GetEmployeesAllowances(out TotalRecordsOut, out RecFilterOut)
                .Select(x => new
                {
                    EmployeeAllowanceID = x.EmployeeAllowanceID,
                    Allowance = x.Allowance,
                    EmployeeCareerHistory = x.EmployeeCareerHistory,
                    //EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCareerHistoryByEmployeeCareerHistoryID(item.EmployeeCareerHistoryID),                        
                    AllowanceStartDate = x.AllowanceStartDate,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                });
            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["EmployeeAllowanceID"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByEmployeeAllowanceID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByEmployeeAllowanceID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("AllowanceStartDate,AllowanceID,EmployeesCareersHistoryID")]
        public ActionResult Delete(EmployeesAllowancesViewModel EmployeeAllowanceVM)
        {
            EmployeesAllowancesBLL employeesAllowancesBLL = new EmployeesAllowancesBLL();
            employeesAllowancesBLL.LoginIdentity = UserIdentity;
            employeesAllowancesBLL.Remove(EmployeeAllowanceVM.EmployeeAllowanceID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(EmployeesAllowancesViewModel EmployeeAllowanceVM)
        {
            EmployeesAllowancesBLL EmployeeAllowance = new EmployeesAllowancesBLL();
            EmployeeAllowance.Allowance = new AllowancesBLL() { AllowanceID = EmployeeAllowanceVM.AllowanceID };
            EmployeeAllowance.AllowanceStartDate = EmployeeAllowanceVM.AllowanceStartDate;
            EmployeeAllowance.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = EmployeeAllowanceVM.EmployeeCareerHistoryID };
            EmployeeAllowance.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) };
            Result result = EmployeeAllowance.Add();
            if ((System.Type)result.EnumType == typeof(AllowanceValidationEnum))
            {
                EmployeesAllowancesBLL EmployeeAllowanceEntity = (EmployeesAllowancesBLL)result.Entity;
                if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
                {
                    // Session["EmployeeAllowanceID"] = ((EmployeesAllowancesBLL)result.Entity).EmployeeAllowanceID;
                    EmployeeAllowanceVM.EmployeeAllowanceID = ((EmployeesAllowancesBLL)result.Entity).EmployeeAllowanceID;
                }
                else if (result.EnumMember == AllowanceValidationEnum.RejectedBecauseOfNotAllowedForJob.ToString())     //not in jobAllowance table
                {
                    throw new CustomException(Resources.Globalization.ValidationAllowanceNotAllowedForJobText);
                }
                else if (result.EnumMember == AllowanceValidationEnum.RejectedBecauseOfStoped.ToString())               //allowance inactive
                {
                    throw new CustomException(Resources.Globalization.ValidationAllowanceStopedText);
                }
                else if (result.EnumMember == AllowanceValidationEnum.RejectedBecauseOfStopedForJob.ToString())     // job inactive
                {
                    throw new CustomException(Resources.Globalization.ValidationAllowanceStopedForJobText);
                }
            }

            //return View(EmployeeAllowanceVM);
            return Json(new { EmployeeAllowanceID = EmployeeAllowanceVM.EmployeeAllowanceID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByEmployeeAllowanceID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditEmployeeAllowance(EmployeesAllowancesViewModel EmployeeAllowanceVM)
        {
            EmployeesAllowancesBLL EmployeeAllowance = new EmployeesAllowancesBLL();
            EmployeeAllowance.EmployeeAllowanceID = EmployeeAllowanceVM.EmployeeAllowanceID;
            EmployeeAllowance.Allowance = new AllowancesBLL() { AllowanceID = EmployeeAllowanceVM.AllowanceID };
            EmployeeAllowance.AllowanceStartDate = EmployeeAllowanceVM.AllowanceStartDate;
            EmployeeAllowance.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = EmployeeAllowanceVM.EmployeeCareerHistoryID };
            EmployeeAllowance.IsActive = EmployeeAllowanceVM.IsActive;
            EmployeeAllowance.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) };
            Result result = EmployeeAllowance.Update();
            if ((System.Type)result.EnumType == typeof(AllowanceValidationEnum))
            {
                EmployeesAllowancesBLL EmployeeAllowanceEntity = (EmployeesAllowancesBLL)result.Entity;
                if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
                {
                    Session["EmployeeAllowanceID"] = ((EmployeesAllowancesBLL)result.Entity).EmployeeAllowanceID;
                }
                else if (result.EnumMember == AllowanceValidationEnum.RejectedBecauseOfNotAllowedForJob.ToString())     //not in jobAllowance table
                {
                    throw new CustomException(Resources.Globalization.ValidationAllowanceNotAllowedForJobText);
                }
                else if (result.EnumMember == AllowanceValidationEnum.RejectedBecauseOfStoped.ToString())               //allowance inactive
                {
                    throw new CustomException(Resources.Globalization.ValidationAllowanceStopedText);
                }
                else if (result.EnumMember == AllowanceValidationEnum.RejectedBecauseOfStopedForJob.ToString())     // job inactive
                {
                    throw new CustomException(Resources.Globalization.ValidationAllowanceStopedForJobText);
                }
            }

            return View(this.GetByEmployeeAllowanceID(EmployeeAllowanceVM.EmployeeAllowanceID));     //View(EmployeeAllowanceVM);
        }

        [CustomAuthorize]
        public ActionResult PrintEmployeeAllowance(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.EmployeesAllowances.ToString(), id));
        }

        [CustomAuthorize]
        public ActionResult PrintAllAllowancesByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.EmployeesAllowances.ToString(), id));
        }

        [HttpGet]
        public JsonResult GetEmployeeAllowanceID()
        {
            int? EmployeeAllowanceID = Session["EmployeeAllowanceID"] == null ? (int?)null : int.Parse(Session["EmployeeAllowanceID"].ToString());
            return Json(new { data = EmployeeAllowanceID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private EmployeesAllowancesViewModel GetByEmployeeAllowanceID(int id)
        {
            EmployeesAllowancesBLL EmployeeAllowanceBLL = (new EmployeesAllowancesBLL()).GetByEmployeeAllowanceID(id);
            EmployeesAllowancesViewModel EmployeeAllowanceVM = new EmployeesAllowancesViewModel();
            if (EmployeeAllowanceBLL != null)
            {
                EmployeeAllowanceVM.EmployeeAllowanceID = EmployeeAllowanceBLL.EmployeeAllowanceID;
                EmployeeAllowanceVM.AllowanceStartDate = EmployeeAllowanceBLL.AllowanceStartDate.Date;
                EmployeeAllowanceVM.Allowance = EmployeeAllowanceBLL.Allowance;
                EmployeeAllowanceVM.EmployeeCareersHistory = EmployeeAllowanceBLL.EmployeeCareerHistory;
                EmployeeAllowanceVM.IsActive = EmployeeAllowanceBLL.IsActive;
                EmployeeAllowanceVM.CreatedDate = EmployeeAllowanceBLL.CreatedDate;
                EmployeeAllowanceVM.CreatedBy = EmployeeAllowanceVM.GetCreatedByDisplayed(EmployeeAllowanceBLL.CreatedBy);
            }
            return EmployeeAllowanceVM;
        }
    }
}