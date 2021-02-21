using System;
using System.Linq;
using System.Web.Mvc;
using HCMBLL;
using HCM.Models;
using HCM.Classes;
using HCM.Classes.CustomFilters;
using HCM.Classes.CustomAttributes;
using HCMBLL.Enums;
using System.Data;
using PSADTO;
using PSADTO.Enums;
using System.Collections.Generic;
using HCM.Classes.Helpers;
using System.Globalization;
using System.Configuration;

namespace HCM.Controllers
{
    public class EmployeeExperiencesWithDetailsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Manage(int id)
        {
            EmployeeExperiencesWithDetailsViewModel VM = this.GetByEmployeeCodeID(id);
            //int total = new EmployeeExperiencesWithDetailsBLL().GetTotalDaysByEmployeeExperience(22915);
            SetAuthentications(VM);
            return View(VM);
        }

        [HttpGet]
        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "EmployeeExperienceWithDetailID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "SectorTypeName", Resources.Globalization.SectorTypeText },
                { "SectorName", Resources.Globalization.SectorNameText },
                { "JobName", Resources.Globalization.JobNameText },
                { "FromDate", Resources.Globalization.FromDateHijriText },
                { "ToDate", Resources.Globalization.ToDateHijriText },
                { "FromDateGr", Resources.Globalization.FromDateText },
                { "ToDateGr", Resources.Globalization.ToDateText },
                { "CreatedBy", Resources.Globalization.CreatedByText },
                { "CreatedDate", Resources.Globalization.CreatedDateText },
            };

            string fileName = ExcelHelper.ExportToExcel(GetEmployeeExperiencesWithDetails(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeExperiencesWithDetails()
        {
            EmployeeExperiencesWithDetailsViewModel VW = new EmployeeExperiencesWithDetailsViewModel();
            CultureInfo enCul = new CultureInfo("en-US");

            var data = new EmployeeExperiencesWithDetailsBLL().GetEmployeesExperiences()
               .Select(x => new
               {
                   EmployeeExperienceWithDetailID = x.EmployeeExperienceWithDetailID,
                   EmployeeCodeNo = x.EmployeesCodes.EmployeeCodeNo,
                   EmployeeNameAr = x.EmployeesCodes.Employee.EmployeeNameAr,
                   x.SectorsTypes.SectorTypeName,
                   x.SectorName,
                   x.JobName,
                   FromDate = Globals.Calendar.GetUmAlQuraDate(x.FromDate),
                   ToDate = Globals.Calendar.GetUmAlQuraDate(x.ToDate),
                   FromDateGr = x.FromDate.ToString(ConfigurationManager.AppSettings["DateFormat"], enCul.DateTimeFormat),
                   ToDateGr = x.ToDate.ToString(ConfigurationManager.AppSettings["DateFormat"], enCul.DateTimeFormat),
                   CreatedBy = VW.GetCreatedByDisplayed(x.CreatedBy),
                   CreatedDate = Globals.Calendar.GetUmAlQuraDate(x.CreatedDate)
               });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private EmployeeExperiencesWithDetailsViewModel GetByEmployeeCodeID(int id)
        {
            EmployeesCodesBLL EmployeesCodesBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(id);
            EmployeeExperiencesWithDetailsViewModel EmployeeExperienceVM = new EmployeeExperiencesWithDetailsViewModel();
            int year, month, days;
            new EmployeeExperiencesWithDetailsBLL().GetTotalEmployeeExperienceInYMD(id, out year, out month, out days);

            EmployeeExperienceVM.TotalYears = year;
            EmployeeExperienceVM.TotalMonths = month;
            EmployeeExperienceVM.TotalDays = days;
            EmployeeExperienceVM.EmployeeVM = new EmployeesViewModel()
            {
                EmployeeCodeID = EmployeesCodesBLL.EmployeeCodeID,
                EmployeeCodeNo = EmployeesCodesBLL.EmployeeCodeNo,
                EmployeeNameAr = EmployeesCodesBLL.Employee.EmployeeNameAr,
                EmployeeJobName = EmployeesCodesBLL.EmployeeCurrentJob != null ? EmployeesCodesBLL.EmployeeCurrentJob.OrganizationJob.Job.JobName : null,
                EmployeeRankName = EmployeesCodesBLL.EmployeeCurrentJob != null ? EmployeesCodesBLL.EmployeeCurrentJob.OrganizationJob.Rank.RankName : null,
                EmployeeOrganizationName = EmployeesCodesBLL.EmployeeCurrentJob != null ? EmployeesCodesBLL.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationName : null,
                HiringDate = EmployeesCodesBLL.HiringRecord != null ? EmployeesCodesBLL.HiringRecord.JoinDate : (DateTime?)null,
                EmployeeIDNo = EmployeesCodesBLL.Employee.EmployeeIDNo,
                EmployeeJobNo = EmployeesCodesBLL.EmployeeCurrentJob != null ? EmployeesCodesBLL.EmployeeCurrentJob.OrganizationJob.JobNo : null
            };
            return EmployeeExperienceVM;
        }

        [Route("EmployeeExperiencesWithDetails/GetEmployeesExperiencesWithDetailByEmployeeCodeID/{EmployeeCodeID}")]
        public JsonResult GetEmployeesExperiencesWithDetailByEmployeeCodeID(int EmployeeCodeID)
        {
            return Json(new
            {
                data = ((new EmployeeExperiencesWithDetailsBLL().GetEmployeesExperiencesWithDetailByEmployeeCodeID(EmployeeCodeID))).Select(x => new
                {
                    EmployeeExperienceWithDetailID = x.EmployeeExperienceWithDetailID,
                    JobName = x.JobName,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    SectorName = x.SectorName,
                    SectorTypeName = x.SectorsTypes.SectorTypeName,
                    x.ExperienceYears,
                    x.ExperienceMonths,
                    x.ExperienceDays,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetByID(int id)
        {
            EmployeeExperiencesWithDetailsBLL exp = new EmployeeExperiencesWithDetailsBLL().GetByEmployeesExperiencesWithDetailID(id);
            return Json(new
            {
                EmployeeExperienceWithDetailID = exp.EmployeeExperienceWithDetailID,
                JobName = exp.JobName,
                FromDate = exp.FromDate,
                ToDate = exp.ToDate,
                SectorName = exp.SectorName,
                SectorTypeID = exp.SectorsTypes.SectorTypeID,
                SectorTypeName = exp.SectorsTypes.SectorTypeName,
                CreatedBy = exp.CreatedBy.Employee.EmployeeNameAr,
                CreatedDate = exp.CreatedDate
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public JsonResult Delete(int id)
        {
            EmployeeExperiencesWithDetailsBLL EmployeeExperiencesWithDetail = new EmployeeExperiencesWithDetailsBLL();
            EmployeeExperiencesWithDetail.LoginIdentity = UserIdentity;
            Result result = EmployeeExperiencesWithDetail.Remove(id);

            return Json(new
            {
                data = EmployeeExperiencesWithDetail
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeVM")]
        public JsonResult Create(EmployeeExperiencesWithDetailsViewModel EmployeeExperienceVM)
        {
            EmployeeExperiencesWithDetailsBLL EmployeeExperiencesWithDetails = new EmployeeExperiencesWithDetailsBLL();
            EmployeeExperiencesWithDetails.JobName = EmployeeExperienceVM.JobName;
            EmployeeExperiencesWithDetails.FromDate = EmployeeExperienceVM.FromDate;
            EmployeeExperiencesWithDetails.ToDate = EmployeeExperienceVM.ToDate;
            EmployeeExperiencesWithDetails.SectorName = EmployeeExperienceVM.SectorName;
            EmployeeExperiencesWithDetails.SectorsTypes = new SectorsTypesBLL() { SectorTypeID = EmployeeExperienceVM.SectorTypeID };
            EmployeeExperiencesWithDetails.EmployeesCodes = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeExperienceVM.EmployeeVM.EmployeeCodeID.Value };
            EmployeeExperiencesWithDetails.LoginIdentity = UserIdentity;
            Result result = EmployeeExperiencesWithDetails.Add();

            return Json(new { data = EmployeeExperiencesWithDetails.EmployeeExperienceWithDetailID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeVM")]
        public JsonResult Edit(EmployeeExperiencesWithDetailsViewModel EmployeeExperienceVM)
        {
            EmployeeExperiencesWithDetailsBLL EmployeeExperiencesWithDetails = new EmployeeExperiencesWithDetailsBLL();
            EmployeeExperiencesWithDetails.EmployeeExperienceWithDetailID = EmployeeExperienceVM.EmployeeExperienceWithDetailID;
            EmployeeExperiencesWithDetails.JobName = EmployeeExperienceVM.JobName;
            EmployeeExperiencesWithDetails.FromDate = EmployeeExperienceVM.FromDate;
            EmployeeExperiencesWithDetails.ToDate = EmployeeExperienceVM.ToDate;
            EmployeeExperiencesWithDetails.SectorName = EmployeeExperienceVM.SectorName;
            EmployeeExperiencesWithDetails.SectorsTypes = new SectorsTypesBLL() { SectorTypeID = EmployeeExperienceVM.SectorTypeID };
            EmployeeExperiencesWithDetails.EmployeesCodes = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeExperienceVM.EmployeeVM.EmployeeCodeID.Value };
            EmployeeExperiencesWithDetails.LoginIdentity = UserIdentity;
            Result result = EmployeeExperiencesWithDetails.Update();

            return Json(new { data = EmployeeExperiencesWithDetails.EmployeeExperienceWithDetailID }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{controller}/GetGregDate/{hijriDt}")]
        public JsonResult GetGregDate(string hijriDt)
        {
            return Json(GetGregDateFromUmAlquraDate(hijriDt), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{controller}/GetHijriDate/{gregDt}")]
        public JsonResult GetHijriDate(string gregDt)
        {
            return Json(GetUmAlquraDateFromGregDate(DateTime.Parse(gregDt)), JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("EmployeeExperiencesWithDetails/GetTotalEmployeeExperienceInYMD/{EmployeeCodeID}")]
        public JsonResult GetTotalEmployeeExperienceInYMD(int EmployeeCodeID)
        {
            int years, months, days;
            new EmployeeExperiencesWithDetailsBLL().GetTotalEmployeeExperienceInYMD(EmployeeCodeID, out years, out months, out days);

            return Json(new
            {
                years,
                months,
                days
            }, JsonRequestBehavior.AllowGet);
        }

        private void SetAuthentications(EmployeeExperiencesWithDetailsViewModel VM)
        {
            AuthenticationResult Authentication = (AuthenticationResult)Session["Authentication"];
            if (Authentication != null && Authentication.User != null && Authentication.User.IsAdmin)
            {
                VM.HasCreatingAccess
                = VM.HasDeletingAccess
                = VM.HasUpdatingAccess = true;
            }
            else
            {
                GroupsObjects Privilage = Authentication.Privilages.FirstOrDefault(e => e.Object.ObjectID == (int)ObjectsEnum.EmployeesExperiencesManage);

                if (Privilage != null)
                {
                    VM.HasCreatingAccess = Privilage.Creating;
                    VM.HasDeletingAccess = Privilage.Deleting;
                    VM.HasUpdatingAccess = Privilage.Updating;
                }
                else
                {
                    VM.HasCreatingAccess
                    = VM.HasDeletingAccess
                    = VM.HasUpdatingAccess = false;
                }
            }
        }
    }
}