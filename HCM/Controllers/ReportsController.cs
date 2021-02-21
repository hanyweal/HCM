using System;
using System.Configuration;
using System.Web.Mvc;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using Resources;
using System.Linq;
using System.Data;
using HCM.Classes.Helpers;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;
using Ninject.Infrastructure.Language;
using MvcSiteMapProvider.Linq;
using System.Data.Entity;
using NPOI.SS.Formula.Functions;

namespace HCM.Controllers
{
    public class ReportsController : BaseController
    {
        [CustomAuthorize]
        public ActionResult ReportJobVacanciesByRankByCategory()
        {
            return View();
        }

        public ActionResult JobVacanciesByRankByCategory()
        {
            int? RankID = Request.QueryString["RankID"].ToString() != string.Empty ? int.Parse(Request.QueryString["RankID"].ToString()) : (int?)null;
            int? JobCategoryID = Request.QueryString["JobCategoryID"].ToString() != string.Empty ? int.Parse(Request.QueryString["JobCategoryID"].ToString()) : (int?)null;
            return Redirect(string.Format("~/WebForms/Reports/JobVacanciesByRankAndCategory.aspx?RankID={0}&JobCategoryID={1}", RankID, JobCategoryID));
        }

        [CustomAuthorize]
        public ActionResult ReportPromotionRecordsByPromotionPeriod()
        {
            return View();
        }

        public ActionResult PromotionRecordsByPromotionPeriod()
        {
            int PromotionPeriodID = int.Parse(Request.QueryString["PromotionPeriodID"].ToString());
            return Redirect(string.Format("~/WebForms/Reports/PromotionRecordsByPromotionPeriod.aspx?PromotionPeriodID={0}", PromotionPeriodID));
        }

        [CustomAuthorize]
        public ActionResult ReportStopWorksByEndedStatus()
        {
            return View();
        }
        public ActionResult StopWorksByEndedStatus()
        {
            int StopWorkEnded = int.Parse(Request.QueryString["StopWorkEnded"].ToString());
            int? EmployeeCodeID = Request.QueryString["EmployeeCodeID"].ToString() == "" ? null : (int?)int.Parse(Request.QueryString["EmployeeCodeID"].ToString());
            string ReportTitle = "";
            switch (StopWorkEnded)
            {
                case 0:
                    ReportTitle = Globalization.StopWorkCurrentlyReport;
                    break;
                case 1:
                    ReportTitle = Globalization.StopWorkEndedReport;
                    break;
                default:
                    ReportTitle = Globalization.StopWorkAllStatusReport;
                    break;
            }
            return Redirect(string.Format("~/WebForms/Reports/StopWorksByEndedStatus.aspx?StopWorkEnded={0}&EmployeeCodeID={1}&ReportTitle={2}", StopWorkEnded, EmployeeCodeID, ReportTitle));
        }

        public ActionResult ReportSickVacationsPaidAmount()
        {
            return View();
        }

        public ActionResult SickVacationsPaidAmount()
        {
            return Redirect("~/WebForms/Reports/SickVacationsPaidAmount.aspx");
        }

        [CustomAuthorize]
        public ActionResult ReportChangeLogs()
        {
            ReportChangeLogsViewModel VM = new ReportChangeLogsViewModel()
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult ChangeLogs(ReportChangeLogsViewModel VM)
        {
            int EmployeeCodeID = VM.EmployeeCodeID.HasValue ? VM.EmployeeCodeID.Value : -1;
            int BusinssSubCategoryID = VM.BusinssSubCategoryID.HasValue ? VM.BusinssSubCategoryID.Value : -1;
            string url = string.Format("Reports/ReportViewerASPX.aspx?type={0}&ID={1}&BusinssSubCategoryID={2}&StartDate={3}&EndDate={4}",
                BusinessSubCategoriesEnum.ChangeLogs.ToString(), EmployeeCodeID, BusinssSubCategoryID,
                    VM.StartDate.ToString(ConfigurationManager.AppSettings["DateFormat"]),
                    VM.EndDate.ToString(ConfigurationManager.AppSettings["DateFormat"]));

            return Json(new { url = url }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult ReportVacationTodayByVacationType()
        {
            ReportVacationTodayByVacationTypeViewModel VM = new ReportVacationTodayByVacationTypeViewModel()
            {
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now,
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult VacationTodayByVacationType(ReportVacationTodayByVacationTypeViewModel VM)
        {
            int VacationTypeID = VM.VacationTypeID.HasValue ? VM.VacationTypeID.Value : 0;
            string url = string.Format("Reports/VacationTodayByVacationType.aspx?VacationTypeID={0}&StartDate={1}&EndDate={2}",
                    VacationTypeID,
                    VM.StartDate.ToString(ConfigurationManager.AppSettings["DateFormat"]),
                    VM.EndDate.ToString(ConfigurationManager.AppSettings["DateFormat"]));

            return Json(new { url = url }, JsonRequestBehavior.AllowGet);
            //int VacationTypeID = int.Parse(Request.QueryString["VacationTypeID"].ToString());
            //return Redirect(string.Format("~/WebForms/Reports/VacationTodayByVacationType.aspx?VacationTypeID={0}", VacationTypeID));
        }

        [CustomAuthorize]
        public ActionResult ReportEmployeesVacationsDuringPeriod()
        {
            ReportEmployeesVacationsDuringPeriodViewModel VM = new ReportEmployeesVacationsDuringPeriodViewModel()
            {
                StartDate = DateTime.Now.AddMonths(-1),
                EndDate = DateTime.Now,
            };
            return View(VM);
        }

        [HttpPost]
        public ActionResult ReportEmployeesVacationsDuringPeriod(ReportEmployeesVacationsDuringPeriodViewModel VM)
        {
            HCMBLL.BaseVacationsBLL vacationsBLL = new BaseVacationsBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            };
            VacationsTypesEnum? vacationsTypesEnum = null;
            if (VM.VacationTypeID != null)
                vacationsTypesEnum = (VacationsTypesEnum)VM.VacationTypeID;
            var data = vacationsBLL.GetValidEmployeesVacationsDuringPeriod(vacationsTypesEnum, VM.StartDate, VM.EndDate);          
            Session["EmployeesVacationsDuringPeriod"] = data.ToList();
            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);

        }
        //[HttpGet]
        //public ActionResult ExportReportEmployeesVacationsDuringPeriod()
        //{
        //    DataSet dataSet = new DataSet();
        //    DataTable dt = new DataTable();
        //    dataSet.Tables.Add(dt);

        //    List<EmployeesVacationsDuringPeriodViewModel> data = Session["EmployeesVacationsDuringPeriod"] as List<EmployeesVacationsDuringPeriodViewModel>;
        //    var workbook = new HSSFWorkbook();
        //    var sheet = workbook.CreateSheet(Resources.Globalization.DelegationsText);
        //    var headerRow = sheet.CreateRow(0);
        //    var titleFont = workbook.CreateFont();
        //    titleFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
        //    titleFont.FontHeightInPoints = 12;
        //    var titleStyle = workbook.CreateCellStyle();
        //    titleStyle.SetFont(titleFont);

        //    var DataFont = workbook.CreateFont();
        //    DataFont.FontHeightInPoints = 12;
        //    var DataStyle = workbook.CreateCellStyle();
        //    DataStyle.SetFont(DataFont);

        //    int j = 0;

        //    var cell = headerRow.CreateCell(j);
        //    cell.CellStyle = titleStyle;
        //    cell.SetCellValue(Resources.Globalization.EmployeeCodeNoText);

        //    cell = headerRow.CreateCell(++j);
        //    cell.CellStyle = titleStyle;
        //    cell.SetCellValue(Resources.Globalization.EmployeeNameArText);

        //    cell = headerRow.CreateCell(++j);
        //    cell.CellStyle = titleStyle;
        //    cell.SetCellValue(Resources.Globalization.EmployeeIDNoText);

        //    cell = headerRow.CreateCell(++j);
        //    cell.CellStyle = titleStyle;
        //    cell.SetCellValue(Resources.Globalization.OrganizationNameText);

        //    cell = headerRow.CreateCell(++j);
        //    cell.CellStyle = titleStyle;
        //    cell.SetCellValue(Resources.Globalization.VacationStartDateText);

        //    cell = headerRow.CreateCell(++j);
        //    cell.CellStyle = titleStyle;
        //    cell.SetCellValue(Resources.Globalization.VacationEndDateText);

        //    cell = headerRow.CreateCell(++j);
        //    cell.CellStyle = titleStyle;
        //    cell.SetCellValue(Resources.Globalization.PeriodText);

        //    cell = headerRow.CreateCell(++j);
        //    cell.CellStyle = titleStyle;
        //    cell.SetCellValue(Resources.Globalization.VacationTypeText);

        //    for (int i = 0; i < data.Count; i++)
        //    {

        //        var row = sheet.CreateRow(i+1);
        //        var o = data[i];

        //        var cell0 = row.CreateCell(0);
        //        cell0.CellStyle = DataStyle;
        //        cell0.SetCellValue(o.EmployeeCodeNo.ToString());

        //        var cell1 = row.CreateCell(1);
        //        cell1.CellStyle = DataStyle;
        //        cell1.SetCellValue(o.EmployeeNameAr.ToString());

        //        var cell2 = row.CreateCell(2);
        //        cell2.CellStyle = DataStyle;
        //        cell2.SetCellValue(o.EmployeeIDNo.ToString());

        //        var cell3 = row.CreateCell(3);
        //        cell3.CellStyle = DataStyle;
        //        cell3.SetCellValue(o.OrganizationName.ToString());

        //        var cell4 = row.CreateCell(4);
        //        cell4.CellStyle = DataStyle;
        //        cell4.SetCellValue(o.VacationStartDate.ToString());

        //        var cell5 = row.CreateCell(5);
        //        cell5.CellStyle = DataStyle;
        //        cell5.SetCellValue(o.VacationEndDate.ToString());

        //        var cell6 = row.CreateCell(6);
        //        cell6.CellStyle = DataStyle;
        //        cell6.SetCellValue(o.VacationPeriod.ToString());

        //        var cell7 = row.CreateCell(7);
        //        cell7.CellStyle = DataStyle;
        //        cell7.SetCellValue(o.VacationTypeName.ToString());

        //    }

        //    string fileName = ExcelHelper.ExportToExcel(workbook);
        //    return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        //}

        [CustomAuthorize]
        public ActionResult ReportOrphanAssignments()
        {
            return View();
        }

        public ActionResult OrphanAssignments()
        {
            return Redirect("~/WebForms/Reports/ReportOrphanAssignments.aspx");
        }
    }
}