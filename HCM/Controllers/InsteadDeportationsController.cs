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
    public class InsteadDeportationsController : BaseController
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
                { "InsteadDeportationID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "JobName", Resources.Globalization.JobNameText },
                { "DeportationDate", Resources.Globalization.DeportationDateText },
                { "Note", Resources.Globalization.NotesText },
                { "Amount", Resources.Globalization.InsteadDeportationAmountText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetAllInsteadDeportations(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllInsteadDeportations()
        {
            var data = new InsteadDeportationsBLL().GetInsteadDeportations()
               .Select(x => new
               {
                   InsteadDeportationID = x.InsteadDeportationID,
                   EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                   EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                   JobName = x.EmployeeCareerHistory.OrganizationJob.Job.JobName,
                   DeportationDate = Globals.Calendar.GetUmAlQuraDate(x.DeportationDate),
                   Note = x.Note,
                   Amount = x.Amount
               });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInsteadDeportations()
        {
            InsteadDeportationsBLL InsteadDeportationsBLL = new InsteadDeportationsBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            };
            var data = InsteadDeportationsBLL.GetInsteadDeportations(out TotalRecordsOut, out RecFilterOut)
                .Select(x => new
                {
                    InsteadDeportationID = x.InsteadDeportationID,
                    EmployeeCareerHistory = x.EmployeeCareerHistory,
                    //EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCareerHistoryByEmployeeCareerHistoryID(item.EmployeeCareerHistoryID),                        
                    DeportationDate = x.DeportationDate,
                    Amount = x.Amount,
                    Note = x.Note,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                });
            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["InsteadDeportationID"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByInsteadDeportationID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByInsteadDeportationID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("AllowanceStartDate,AllowanceID,EmployeesCareersHistoryID")]
        public ActionResult Delete(InsteadDeportationsViewModel InsteadDeportationVM)
        {
            InsteadDeportationsBLL InsteadDeportationsBLL = new InsteadDeportationsBLL();
            InsteadDeportationsBLL.LoginIdentity = UserIdentity;
            InsteadDeportationsBLL.Remove(InsteadDeportationVM.InsteadDeportationID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(InsteadDeportationsViewModel InsteadDeportationVM)
        {
            InsteadDeportationsBLL InsteadDeportation = new InsteadDeportationsBLL();
            InsteadDeportation.DeportationDate = InsteadDeportationVM.DeportationDate;
            InsteadDeportation.Note = InsteadDeportationVM.Note;
            InsteadDeportation.Amount = InsteadDeportationVM.Amount;
            InsteadDeportation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = (int)InsteadDeportationVM.EmployeeCareerHistoryID };
            InsteadDeportation.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) };
            Result result = InsteadDeportation.Add();
            if ((System.Type)result.EnumType == typeof(AllowanceValidationEnum))
            {
                InsteadDeportationsBLL InsteadDeportationEntity = (InsteadDeportationsBLL)result.Entity;
                if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
                {
                    // Session["InsteadDeportationID"] = ((InsteadDeportationsBLL)result.Entity).InsteadDeportationID;
                    InsteadDeportationVM.InsteadDeportationID = ((InsteadDeportationsBLL)result.Entity).InsteadDeportationID;
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

            //return View(InsteadDeportationVM);
            return Json(new { InsteadDeportationID = InsteadDeportationVM.InsteadDeportationID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByInsteadDeportationID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditInsteadDeportation(InsteadDeportationsViewModel InsteadDeportationVM)
        {
            InsteadDeportationsBLL InsteadDeportation = new InsteadDeportationsBLL();
            InsteadDeportation.InsteadDeportationID = InsteadDeportationVM.InsteadDeportationID;
            InsteadDeportation.DeportationDate = InsteadDeportationVM.DeportationDate;
            InsteadDeportation.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = (int)InsteadDeportationVM.EmployeeCareerHistoryID };
            InsteadDeportation.Note = InsteadDeportationVM.Note;
            InsteadDeportation.Amount = InsteadDeportationVM.Amount;
            InsteadDeportation.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) };
            Result result = InsteadDeportation.Update();
            if ((System.Type)result.EnumType == typeof(AllowanceValidationEnum))
            {
                InsteadDeportationsBLL InsteadDeportationEntity = (InsteadDeportationsBLL)result.Entity;
                if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
                {
                    Session["InsteadDeportationID"] = ((InsteadDeportationsBLL)result.Entity).InsteadDeportationID;
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

            return View(this.GetByInsteadDeportationID(InsteadDeportationVM.InsteadDeportationID));     //View(InsteadDeportationVM);
        }

        [HttpGet]
        public JsonResult GetInsteadDeportationID()
        {
            int? InsteadDeportationID = Session["InsteadDeportationID"] == null ? (int?)null : int.Parse(Session["InsteadDeportationID"].ToString());
            return Json(new { data = InsteadDeportationID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private InsteadDeportationsViewModel GetByInsteadDeportationID(int id)
        {
            InsteadDeportationsBLL InsteadDeportationBLL = (new InsteadDeportationsBLL()).GetByInsteadDeportationID(id);
            InsteadDeportationsViewModel InsteadDeportationVM = new InsteadDeportationsViewModel();
            if (InsteadDeportationBLL != null)
            {
                InsteadDeportationVM.InsteadDeportationID = InsteadDeportationBLL.InsteadDeportationID;
                InsteadDeportationVM.DeportationDate = InsteadDeportationBLL.DeportationDate.Date;
                InsteadDeportationVM.EmployeeCareersHistory = InsteadDeportationBLL.EmployeeCareerHistory;
                InsteadDeportationVM.Note = InsteadDeportationBLL.Note;
                InsteadDeportationVM.Amount = InsteadDeportationBLL.Amount;
                InsteadDeportationVM.CreatedDate = InsteadDeportationBLL.CreatedDate;
                InsteadDeportationVM.CreatedBy = InsteadDeportationVM.GetCreatedByDisplayed(InsteadDeportationBLL.CreatedBy);
            }
            return InsteadDeportationVM;
        }

        [CustomAuthorize]
        public ActionResult PrintInsteadDeportation(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.InsteadDeportations.ToString(), id));
        }

    }
}