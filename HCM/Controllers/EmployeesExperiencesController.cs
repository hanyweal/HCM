using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Classes.Helpers;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using NPOI.HSSF.UserModel;
using PSADTO;
using PSADTO.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class EmployeesExperiencesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Manage(int id)
        {
            EmployeesExperiencesViewModel VM = this.GetByEmployeeCodeID(id);
            SetAuthentications(VM);
            return View(VM);
        }

        [NonAction]
        private EmployeesExperiencesViewModel GetByEmployeeCodeID(int id)
        {
            EmployeesCodesBLL EmployeesCodesBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(id);
            EmployeesExperiencesViewModel EmployeeExperienceVM = new EmployeesExperiencesViewModel();
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

        [Route("EmployeesExperiences/GetEmployeeExperiencesByEmployeeCodeID/{EmployeeCodeID}")]
        public JsonResult GetEmployeeExperiencesByEmployeeCodeID(int EmployeeCodeID)
        {
            return Json(new
            {
                data = ((new EmployeesExperiencesBLL().GetEmployeeExperiencesByEmployeeCodeID(EmployeeCodeID))).Select(x => new
                {
                    EmployeeExperienceID = x.EmployeeExperienceID,
                    TotalYears = x.TotalYears,
                    TotalMonths = x.TotalMonths,
                    TotalDays = x.TotalDays,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public JsonResult Delete(int id)
        {
            EmployeesExperiencesBLL EmployeeExperience = new EmployeesExperiencesBLL();
            EmployeeExperience.LoginIdentity = UserIdentity;
            Result result = EmployeeExperience.Remove(id);
            if (result.EnumMember == EmployeesExperiencesValidationEnum.Done.ToString())
            {

            }
            return Json(new
            {
                data = EmployeeExperience
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeVM")]
        public JsonResult Create(EmployeesExperiencesViewModel EmployeeExperienceVM)
        {
            EmployeesExperiencesBLL EmployeeExperiences = new EmployeesExperiencesBLL();
            EmployeeExperiences.TotalYears = EmployeeExperienceVM.TotalYears;
            EmployeeExperiences.TotalMonths = EmployeeExperienceVM.TotalMonths;
            EmployeeExperiences.TotalDays = EmployeeExperienceVM.TotalDays;
            EmployeeExperiences.EmployeesCodes = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeExperienceVM.EmployeeVM.EmployeeCodeID.Value };
            EmployeeExperiences.LoginIdentity = UserIdentity;
            Result result = EmployeeExperiences.Add();
            if (result.EnumMember == EmployeesExperiencesValidationEnum.Done.ToString())
            {

            }
            return Json(new { data = EmployeeExperiences.EmployeeExperienceID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeVM")]
        public JsonResult Edit(EmployeesExperiencesViewModel EmployeeExperienceVM)
        {
            EmployeesExperiencesBLL EmployeeExperiences = new EmployeesExperiencesBLL();
            EmployeeExperiences.EmployeeExperienceID = EmployeeExperienceVM.EmployeeExperienceID;
            EmployeeExperiences.TotalYears = EmployeeExperienceVM.TotalYears;
            EmployeeExperiences.TotalMonths = EmployeeExperienceVM.TotalMonths;
            EmployeeExperiences.TotalDays = EmployeeExperienceVM.TotalDays;
            EmployeeExperiences.EmployeesCodes = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeExperienceVM.EmployeeVM.EmployeeCodeID.Value };
            EmployeeExperiences.LoginIdentity = UserIdentity;
            Result result = EmployeeExperiences.Update();
            if (result.EnumMember == EmployeesExperiencesValidationEnum.Done.ToString())
            {

            }
            return Json(new { data = EmployeeExperiences.EmployeeExperienceID }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "EmployeeExperienceID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "TotalYears", Resources.Globalization.TotalYearsText },
                { "TotalMonths", Resources.Globalization.TotalMonthsText },
                { "TotalDays", Resources.Globalization.TotalDaysText },
                { "CreatedDate", Resources.Globalization.CreatedDateText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetEmployeesExperiences(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeesExperiences()
        {
            var data = new EmployeesExperiencesBLL().GetEmployeesExperiences()
               .Select(x => new
               {
                   EmployeeExperienceID = x.EmployeeExperienceID,
                   TotalYears = x.TotalYears,
                   TotalMonths = x.TotalMonths,
                   TotalDays = x.TotalDays,
                   EmployeeCodeNo = x.EmployeesCodes.EmployeeCodeNo,
                   EmployeeNameAr = x.EmployeesCodes.Employee.EmployeeNameAr,
                   CreatedDate = Globals.Calendar.GetUmAlQuraDate(x.CreatedDate),
               });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        private void SetAuthentications(EmployeesExperiencesViewModel VM)
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