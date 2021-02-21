using ExceptionNameSpace;
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
    public class EmployeesEvaluationsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Manage(int id)
        {
            EmployeesEvaluationsViewModel VM = this.GetByEmployeeCodeID(id);
            SetAuthentications(VM);
            return View(VM);
        }

        [Route("EmployeesEvaluations/GetEmployeeEvaluationByEmployeeCodeID/{EmployeeCodeID}")]
        public JsonResult GetEmployeeEvaluationByEmployeeCodeID(int EmployeeCodeID)
        {
            return Json(new
            {
                data = ((new EmployeesEvaluationsBLL().GetEmployeeEvaluationsByEmployeeCodeID(EmployeeCodeID))).Select(x => new
                {
                    EmployeeEvaluationID = x.EmployeeEvaluationID,
                    MaturityYearsBalances = x.MaturityYearsBalances,
                    EvaluationPoints = x.EvaluationPoints,
                    EvaluationDegree = x.EvaluationDegree,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("EmployeesEvaluations/GetEmployeeEvaluationDetailsByEmployeeEvaluationID/{EmployeeEvaluationID}")]
        public JsonResult GetEmployeeEvaluationDetailsByEmployeeEvaluationID(int EmployeeEvaluationID)
        {
            return Json(new
            {
                data = ((new EmployeesEvaluationsDetailsBLL().GetEmployeeEvaluationDetailsByEmployeeEvaluationID(EmployeeEvaluationID))).Select(x => new
                {
                    EmployeeEvaluationDetailID = x.EmployeeEvaluationDetailID,
                    EvaluationQuarterName = x.EvaluationQuarter.EvaluationQuarterName,
                    DirectManagerEvaluation = x.DirectManagerEvaluation,
                    TimeAttendanceEvaluation = x.TimeAttendanceEvaluation,
                    ViolationsEvaluation = x.ViolationsEvaluation
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private EmployeesEvaluationsViewModel GetByEmployeeCodeID(int id)
        {
            EmployeesCodesBLL EmployeesCodesBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(id);
            EmployeesEvaluationsViewModel EmployeeEvaluationVM = new EmployeesEvaluationsViewModel();
            EmployeeEvaluationVM.EmployeeVM = new EmployeesViewModel()
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
            return EmployeeEvaluationVM;
        }
        [CustomAuthorize]
        public JsonResult Delete(int id)
        {
            EmployeesEvaluationsBLL EmployeeEvaluation = new EmployeesEvaluationsBLL();
            EmployeeEvaluation.LoginIdentity = UserIdentity;
            Result result = EmployeeEvaluation.Remove(id);
            
            return Json(new
            {
                data = EmployeeEvaluation
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeVM")]
        public JsonResult Create(EmployeesEvaluationsViewModel EmployeeEvaluationVM)
        {
            EmployeesEvaluationsBLL EmployeeEvaluation = new EmployeesEvaluationsBLL();
            EmployeeEvaluation.MaturityYearsBalances = new MaturityYearsBalancesBLL() { MaturityYearID = EmployeeEvaluationVM.MaturityYearID };
            EmployeeEvaluation.EvaluationPoints = new EvaluationPointsBLL() { EvaluationPointID = EmployeeEvaluationVM.EvaluationPointID };
            EmployeeEvaluation.EvaluationDegree = EmployeeEvaluationVM.EvaluationDegree;
            EmployeeEvaluation.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeEvaluationVM.EmployeeVM.EmployeeCodeID.Value };
            EmployeeEvaluation.LoginIdentity = UserIdentity;
            Result result = EmployeeEvaluation.Add();
            
            return Json(new { data = EmployeeEvaluation.EmployeeEvaluationID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeVM")]
        public JsonResult Edit(EmployeesEvaluationsViewModel EmployeeEvaluationVM)
        {
            EmployeesEvaluationsBLL EmployeeEvaluation = new EmployeesEvaluationsBLL();
            EmployeeEvaluation.EmployeeEvaluationID = EmployeeEvaluationVM.EmployeeEvaluationID;
            EmployeeEvaluation.MaturityYearsBalances = new MaturityYearsBalancesBLL() { MaturityYearID = EmployeeEvaluationVM.MaturityYearID };
            EmployeeEvaluation.EvaluationPoints = new EvaluationPointsBLL() { EvaluationPointID = EmployeeEvaluationVM.EvaluationPointID };
            EmployeeEvaluation.EvaluationDegree = EmployeeEvaluationVM.EvaluationDegree;
            EmployeeEvaluation.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeEvaluationVM.EmployeeVM.EmployeeCodeID.Value };
            EmployeeEvaluation.LoginIdentity = UserIdentity;
            Result result = EmployeeEvaluation.Update();
           
            return Json(new { data = EmployeeEvaluation.EmployeeEvaluationID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("{controller}/UpdateDirectManagerEvaluation/{DirectManagerEvaluation}/{TimeAttendanceEvaluation}/{ViolationsEvaluation}/{EmployeeEvaluationDetailID}")]
        public ActionResult UpdateDirectManagerEvaluation(decimal DirectManagerEvaluation, decimal TimeAttendanceEvaluation, decimal ViolationsEvaluation, int EmployeeEvaluationDetailID)
        {
            EmployeesEvaluationsDetailsBLL EmployeeEvaluationDetails = new EmployeesEvaluationsDetailsBLL()
            {
                LoginIdentity = this.UserIdentity,
                EmployeeEvaluationDetailID = EmployeeEvaluationDetailID,
                DirectManagerEvaluation = DirectManagerEvaluation,
                TimeAttendanceEvaluation = TimeAttendanceEvaluation,
                ViolationsEvaluation = ViolationsEvaluation
            };
            Result result = EmployeeEvaluationDetails.Update();
            return Json(new { EmployeeEvaluationDetailID = EmployeeEvaluationDetails.EmployeeEvaluationDetailID }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "EmployeeEvaluationID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "MaturityYearsBalances", Resources.Globalization.MaturityYearBalanceText },
                { "EvaluationPoints", Resources.Globalization.EvaluationPointsText },
                { "Evaluation", Resources.Globalization.EvaluationText },
                { "EvaluationDegree", Resources.Globalization.EvaluationDegreeText },
                { "CreatedDate", Resources.Globalization.CreatedDateText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetEmployeesEvaluations(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeesEvaluations()
        {
            var data = new EmployeesEvaluationsBLL().GetEmployeesEvaluations()
               .Select(x => new
               {
                   EmployeeEvaluationID = x.EmployeeEvaluationID,
                   MaturityYearsBalances = x.MaturityYearsBalances.MaturityYear, //x.MaturityYearsBalances,
                   EvaluationPoints = x.EvaluationPoints.EvaluationPoint,
                   Evaluation = x.EvaluationPoints.Evaluation,
                   EvaluationDegree = x.EvaluationDegree,
                   EmployeeCodeNo = x.EmployeeCode.EmployeeCodeNo,
                   EmployeeNameAr = x.EmployeeCode.Employee.EmployeeNameAr,
                   CreatedDate = Globals.Calendar.GetUmAlQuraDate(x.CreatedDate),
               });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeVM")]
        public JsonResult CreateEmpEvaluationDetails(EmployeesEvaluationsViewModel EmployeeEvaluationVM)
        {
            EmployeesEvaluationsDetailsBLL EmployeesEvaluationsDetails = new EmployeesEvaluationsDetailsBLL();
            EmployeesEvaluationsDetails.DirectManagerEvaluation = EmployeeEvaluationVM.DirectManagerEvaluation;
            EmployeesEvaluationsDetails.TimeAttendanceEvaluation = EmployeeEvaluationVM.TimeAttendanceEvaluation;
            EmployeesEvaluationsDetails.ViolationsEvaluation = EmployeeEvaluationVM.ViolationsEvaluation;
            EmployeesEvaluationsDetails.EvaluationQuarter = new EvaluationsQuartersBLL() { EvaluationQuarterID = EmployeeEvaluationVM.EvaluationQuarterID };
            EmployeesEvaluationsDetails.EmployeeEvaluation = new EmployeesEvaluationsBLL() { EmployeeEvaluationID = EmployeeEvaluationVM.EmployeeEvaluationID, MaturityYearsBalances = new MaturityYearsBalancesBLL() { MaturityYearID = EmployeeEvaluationVM.MaturityYearID } };
            EmployeesEvaluationsDetails.LoginIdentity = UserIdentity;

            Result result = EmployeesEvaluationsDetails.Add();
            if (result.EnumMember == EmployeesEvaluationsValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == EmployeesEvaluationsValidationEnum.RejectedBecauseOfDirectManagerEvaluationIsNotBetweenZeroAndFifty.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationDirectManagerEvaluationShouldBeBetweenZeroAndFiftyText);

            }
            else if (result.EnumMember == EmployeesEvaluationsValidationEnum.RejectedBecauseOfTimeAttendanceEvaluationIsNotBetweenZeroAndThirtyFive.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationTimeAttendanceEvaluationShouldBeBetweenZeroAndThirtyFiveText);
            }
            else if (result.EnumMember == EmployeesEvaluationsValidationEnum.RejectedBecauseOfViolationsEvaluationIsNotBetweenZeroAndFifteen.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationViolationsEvaluationShouldBeBetweenZeroAndFifteenText);

            }
            else if (result.EnumMember == EmployeesEvaluationsValidationEnum.RejectedBecauseOfEvaluationQuarterAlreadyExistsInCurrentYear.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEvaluationQuarterAlreadyExistsInCurrentYearText);
            }

            return Json(new { data = EmployeesEvaluationsDetails.EmployeeEvaluationDetailID }, JsonRequestBehavior.AllowGet);
        }

        private void SetAuthentications(EmployeesEvaluationsViewModel VM)
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
                GroupsObjects Privilage = Authentication.Privilages.FirstOrDefault(e => e.Object.ObjectID == (int)ObjectsEnum.EmployeesEvaluationsManage);

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