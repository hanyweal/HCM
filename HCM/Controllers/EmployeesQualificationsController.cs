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
    public class EmployeesQualificationsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Manage(int id)
        {
            EmployeesQualificationsViewModel VM = this.GetByEmployeeCodeID(id);
            SetAuthentications(VM);
            return View(VM);
        }

        [NonAction]
        private EmployeesQualificationsViewModel GetByEmployeeCodeID(int id)
        {
            EmployeesCodesBLL EmployeesCodesBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(id);
            EmployeesQualificationsViewModel EmployeeQualificationVM = new EmployeesQualificationsViewModel();
            EmployeeQualificationVM.EmployeeVM = new EmployeesViewModel()
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
            //EmployeeQualificationVM.QualificationDegreeID = new QualificationsDegreesBLL().QualificationDegreeID;
            //EmployeeQualificationVM.QualificationID = new QualificationsBLL().QualificationID;
            //EmployeeQualificationVM.GeneralSpecializationID = new GeneralSpecializationsBLL().GeneralSpecializationID;
            return EmployeeQualificationVM;
        }

        [Route("EmployeesQualifications/GetEmployeeQualificationByEmployeeCodeID/{EmployeeCodeID}")]
        public JsonResult GetEmployeeQualificationByEmployeeCodeID(int EmployeeCodeID)
        {
            return Json(new
            {
                data = ((new EmployeesQualificationsBLL().GetEmployeeQualificationByEmployeeCodeID(EmployeeCodeID))).Select(x => new
                {
                    EmployeeQualificationID = x.EmployeeQualificationID,
                    QualificationDegree = x.QualificationDegree,
                    Qualification = x.Qualification,
                    GeneralSpecialization = x.GeneralSpecialization,
                    ExactSpecialization = x.ExactSpecialization,
                    UniSchName = x.UniSchName,
                    FullGPA = x.FullGPA,
                    Department = x.Department,
                    GPA = x.GPA,
                    StudyPlace = x.StudyPlace,
                    GraduationDate = x.GraduationDate,
                    GraduationYear = x.GraduationYear,
                    QualificationType = x.QualificationType,
                    CreatedBy = x.CreatedBy != null ? x.CreatedBy.Employee.EmployeeNameAr : null,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public JsonResult Delete(int id)
        {
            EmployeesQualificationsBLL EmployeeQualification = new EmployeesQualificationsBLL();
            EmployeeQualification.LoginIdentity = UserIdentity;
            Result result = EmployeeQualification.Remove(id);
            if (result.EnumMember == EmployeesQualificationsValidationEnum.Done.ToString())
            {

            }
            return Json(new
            {
                data = EmployeeQualification
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeVM,ExactSpecialization,QualificationType")]
        public ActionResult Create(EmployeesQualificationsViewModel EmployeeQualificationVM)
        {
            EmployeesQualificationsBLL EmployeeQualification = new EmployeesQualificationsBLL();
            EmployeeQualification.QualificationDegree = new QualificationsDegreesBLL() { QualificationDegreeID = EmployeeQualificationVM.QualificationDegreeID };
            EmployeeQualification.Qualification = new QualificationsBLL() { QualificationID = EmployeeQualificationVM.QualificationID };
            EmployeeQualification.GeneralSpecialization = new GeneralSpecializationsBLL() { GeneralSpecializationID = EmployeeQualificationVM.GeneralSpecializationID };
            EmployeeQualification.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeQualificationVM.EmployeeVM.EmployeeCodeID.Value };
            EmployeeQualification.ExactSpecialization = new ExactSpecializationsBLL() { ExactSpecializationID = EmployeeQualificationVM.ExactSpecializationID };
            EmployeeQualification.UniSchName = EmployeeQualificationVM.UniSchName;
            EmployeeQualification.Department = EmployeeQualificationVM.Department;
            EmployeeQualification.FullGPA = EmployeeQualificationVM.FullGPA;
            EmployeeQualification.GPA = EmployeeQualificationVM.GPA;
            EmployeeQualification.StudyPlace = EmployeeQualificationVM.StudyPlace;
            EmployeeQualification.GraduationDate = EmployeeQualificationVM.GraduationDate;
            EmployeeQualification.GraduationYear = EmployeeQualificationVM.GraduationYear;
            EmployeeQualification.QualificationType = new QualificationsTypesBLL() { QualificationTypeID = EmployeeQualificationVM.QualificationType.QualificationTypeID };
            EmployeeQualification.LoginIdentity = UserIdentity;
            Result result = EmployeeQualification.Add();
            if (result.EnumMember == EmployeesQualificationsValidationEnum.Done.ToString())
            {

            }
            return Json(new { data = EmployeeQualification.EmployeeQualificationID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeVM,ExactSpecialization,QualificationType")]
        public ActionResult Edit(EmployeesQualificationsViewModel EmployeeQualificationVM)
        {
            EmployeesQualificationsBLL EmployeeQualification = new EmployeesQualificationsBLL();
            EmployeeQualification.EmployeeQualificationID = EmployeeQualificationVM.EmployeeQualificationID;
            EmployeeQualification.QualificationDegree = new QualificationsDegreesBLL() { QualificationDegreeID = EmployeeQualificationVM.QualificationDegreeID };
            EmployeeQualification.Qualification = new QualificationsBLL() { QualificationID = EmployeeQualificationVM.QualificationID };
            EmployeeQualification.GeneralSpecialization = new GeneralSpecializationsBLL() { GeneralSpecializationID = EmployeeQualificationVM.GeneralSpecializationID };
            EmployeeQualification.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeQualificationVM.EmployeeVM.EmployeeCodeID.Value };
            EmployeeQualification.ExactSpecialization = new ExactSpecializationsBLL() { ExactSpecializationID = EmployeeQualificationVM.ExactSpecializationID };
            EmployeeQualification.UniSchName = EmployeeQualificationVM.UniSchName;
            EmployeeQualification.Department = EmployeeQualificationVM.Department;
            EmployeeQualification.FullGPA = EmployeeQualificationVM.FullGPA;
            EmployeeQualification.GPA = EmployeeQualificationVM.GPA;
            EmployeeQualification.StudyPlace = EmployeeQualificationVM.StudyPlace;
            EmployeeQualification.GraduationDate = EmployeeQualificationVM.GraduationDate;
            EmployeeQualification.GraduationYear = EmployeeQualificationVM.GraduationYear;
            EmployeeQualification.QualificationType = new QualificationsTypesBLL() { QualificationTypeID = EmployeeQualificationVM.QualificationType.QualificationTypeID };
            EmployeeQualification.LoginIdentity = UserIdentity;
            Result result = EmployeeQualification.Update();

            return Json(new { data = EmployeeQualification.EmployeeQualificationID }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "EmployeeQualificationID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "QualificationDegree", Resources.Globalization.QualificationDegreeNameText },
                { "Qualification", Resources.Globalization.QualificationNameText },
                { "GeneralSpecialization", Resources.Globalization.GeneralSpecializationNameText },
                { "ExactSpecialization", Resources.Globalization.ExactSpecializationNameText },
                { "JobName", Resources.Globalization.JobNameText },
                { "UniSchName", Resources.Globalization.UniSchNameText },
                { "FullGPA", Resources.Globalization.FullGPAText },
                { "Department", Resources.Globalization.DepartmentText },
                { "GPA", Resources.Globalization.GPAText },
                { "StudyPlace", Resources.Globalization.StudyPlaceText },
                { "GraduationDate", Resources.Globalization.GraduationDateText },
                { "QualificationType", Resources.Globalization.QualificationTypeText },
                { "GraduationDate", Resources.Globalization.GraduationDateText },
                { "StudyPlace", Resources.Globalization.StudyPlaceText },
                { "CreatedDate", Resources.Globalization.CreatedDateText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetEmployeesQualifications(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeesQualifications()
        {
            var data = new EmployeesQualificationsBLL().GetEmployeesQualifications()
               .Select(x => new
               {
                   EmployeeQualificationID = x.EmployeeQualificationID,
                   QualificationDegree = x.QualificationDegree != null ? x.QualificationDegree.QualificationDegreeName : null,
                   Qualification = x.Qualification != null ? x.Qualification.QualificationName : null,
                   GeneralSpecialization = x.GeneralSpecialization != null ? x.GeneralSpecialization.GeneralSpecializationName : null,
                   ExactSpecialization = x.ExactSpecialization != null ? x.ExactSpecialization.ExactSpecializationName : null,
                   UniSchName = x.UniSchName,
                   FullGPA = x.FullGPA,
                   Department = x.Department,
                   GPA = x.GPA,
                   StudyPlace = x.StudyPlace,
                   GraduationDate = x.GraduationDate != null ? Globals.Calendar.GetUmAlQuraDate(x.GraduationDate.Value) : null,
                   GraduationYear = x.GraduationYear,
                   QualificationType = x.QualificationType != null ? x.QualificationType.QualificationTypeName : null,
                   CreatedDate = Globals.Calendar.GetUmAlQuraDate(x.CreatedDate),
                   EmployeeCodeNo = x.EmployeeCode.EmployeeCodeNo,
                   EmployeeNameAr = x.EmployeeCode.Employee.EmployeeNameAr,
               });
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        private void SetAuthentications(EmployeesQualificationsViewModel VM)
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
                GroupsObjects Privilage = Authentication.Privilages.FirstOrDefault(e => e.Object.ObjectID == (int)ObjectsEnum.EmployeesQualificationsManage);

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