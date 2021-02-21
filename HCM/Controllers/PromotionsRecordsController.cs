using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public partial class PromotionsRecordsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();

        }

        [HttpGet]
        public JsonResult ResetCandidates()
        {
            Session["PromotionRecordEmployees"] = null;
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["PromotionRecordEmployees"] = null;
            RanksBLL rankBLL = new RanksBLL().GetActiveRankForPromotion();
            PromotionsRecordsViewModel PromotionRecordVM = new PromotionsRecordsViewModel()
            {
                PromotionRecordID = 0,
                RankID = rankBLL != null ? rankBLL.RankID : 0,
                RankName = rankBLL != null ? rankBLL.RankName : string.Empty,
                PromotionRecordDate = DateTime.Now.Date,
                PromotionRecordToolbarID = (int)HCM.Classes.Enums.PromotionsRecordsToolbarEnum.Open,
                PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
            };
            return View(PromotionRecordVM);
        }

        [HttpPost]
        public ActionResult Create(PromotionsRecordsViewModel PromotionsRecordsVM)
        {
            Result result = new Result();
            PromotionsRecordsBLL PromotionRecord = new PromotionsRecordsBLL()
            {
                PromotionRecordDate = PromotionsRecordsVM.PromotionRecordDate,
                PromotionPeriod = new PromotionsPeriodsBLL() { PromotionPeriodID = PromotionsRecordsVM.PromotionPeriodID },
                JobCategory = new JobsCategoriesBLL() { JobCategoryID = PromotionsRecordsVM.JobCategoryID },
                Rank = new RanksBLL() { RankID = PromotionsRecordsVM.RankID },
                PromotionsRecordsEmployees = Session["PromotionRecordEmployees"] != null ? (List<PromotionsRecordsEmployeesBLL>)Session["PromotionRecordEmployees"] : null,
                LoginIdentity = this.UserIdentity
            };
            result = PromotionRecord.Add();
            PromotionsRecordsBLL PromotionRecordObj = (PromotionsRecordsBLL)result.Entity;
            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsVM.PromotionRecordID = PromotionRecordObj.PromotionRecordID;
                PromotionsRecordsVM.PromotionRecordNo = PromotionRecordObj.PromotionRecordNo;
                PromotionsRecordsVM.PromotionRecordDate = PromotionRecordObj.PromotionRecordDate;
                PromotionsRecordsVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel() { PromotionRecordStatusName = PromotionRecordObj.PromotionRecordStatus.PromotionRecordStatusName };
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseThereArePormotionRecordsNotClosedInOtherRanks.ToString())
            {
                Session["PromotionRecordEmployees"] = null;
                throw new CustomException(string.Format(@Resources.Globalization.ValidationPormotionRecordsNotClosedInOtherRanksText.ToString(), PromotionRecordObj.PromotionRecordNo, PromotionRecordObj.JobCategory.JobCategoryName, PromotionRecordObj.Rank.RankName));
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseThereArePormotionRecordsNotInstalled.ToString())
            {
                Session["PromotionRecordEmployees"] = null;
                throw new CustomException(string.Format(@Resources.Globalization.ValidationPormotionRecordsNotInstalledText.ToString(), PromotionRecordObj.PromotionRecordNo, PromotionRecordObj.JobCategory.JobCategoryName));
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString())
            {
                Session["PromotionRecordEmployees"] = null;
                throw new CustomException(@Resources.Globalization.ValidationNoJobsVacanciesAvailableText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString())
            {
                Session["PromotionRecordEmployees"] = null;
                throw new CustomException(@Resources.Globalization.ValidationNoCandidatesEligibleInPromotionRecordText.ToString());
            }

            return Json(PromotionsRecordsVM);
        }

        public JsonResult Delete(int id)
        {
            Result result = new Result();
            result = new PromotionsRecordsBLL() { LoginIdentity = this.UserIdentity }.Remove(id);
            if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoPromotionRecordSelectedToDelete.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoPromotionRecordSelectedToDeleteText.ToString());
            }
            Session["PromotionRecordEmployees"] = null;
            return Json(new { data = id }, JsonRequestBehavior.AllowGet);
            //return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("{controller}/AddCandidateManually/{EmployeeCodeID}/{PromotionRecordID}/{ManualAddedReason}")]
        public JsonResult AddCandidateManually(int EmployeeCodeID, int PromotionRecordID, string ManualAddedReason)
        {
            Result result = new Result();
            result = new PromotionsRecordsEmployeesBLL()
            {
                CurrentEmployeeCareer = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } },
                ManualAddedReason = ManualAddedReason,
                LoginIdentity = this.UserIdentity,
                PromotionRecord = new PromotionsRecordsBLL()
                {
                    PromotionRecordID = PromotionRecordID,
                }
            }.AddCandidateManually();
            if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoPromotionRecordSelectedToDelete.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationNoPromotionRecordSelectedToDeleteText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateAndReasonAreRequiredToAdding.ToString())
            {
                throw new CustomException(Resources.Globalization.RequiredEmployeeAndReasonToAddingText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateRankNotDeserveToPromote.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationCandidateRankNotDeserveToPromoteText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfLastCandidateEvaluationWeak.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationLastCandidateEvaluationWeakText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfPromotionIsNotOpen.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationCandidateCanNotBeAddedPromotionIsNotOpenText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateHiringRecordNotExists.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeHiringRecordNotExistsText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateJobPeriodNotCompleted.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationCandidateJobPeriodNotCompletedText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateAlreadyInPromotionRecordNotInstalled.ToString())
            {
                throw new CustomException(string.Format(Resources.Globalization.ValidationCandidateAlreadyInPromotionRecordNotInstalledText.ToString(), ((PromotionsRecordsEmployeesBLL)result.Entity).PromotionRecord.PromotionRecordNo));
            }
            else if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfCandidateAlreadyReservedJobVacancy.ToString())
            {
                PromotionsRecordsEmployeesBLL pp = ((PromotionsRecordsEmployeesBLL)result.Entity);
                throw new CustomException(string.Format(Resources.Globalization.ValidationCandidateAlreadyAlreadyReservedJobVacancyText.ToString(), pp.PromotionRecord.PromotionRecordNo, pp.PromotionRecordJobVacancy.JobVacancy.JobNo, pp.PromotionRecordJobVacancy.JobVacancy.Job.JobName));
            }
            else
            {
                Session["PromotionRecordEmployees"] = null;
                return this.GetAllCandidatesByPromotionRecordID(PromotionRecordID);
            }
            //return Json(new { data = PromotionRecordID }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/RefreshPromotionRecord/{PromotionRecordID}")]
        public JsonResult RefreshPromotionRecord(int PromotionRecordID)
        {
            Result result = new PromotionsRecordsBLL()
            {
                LoginIdentity = this.UserIdentity
            }.RefreshPromotionRecord(PromotionRecordID);
            PromotionsRecordsBLL PromotionRecord = (PromotionsRecordsBLL)result.Entity;
            if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseThereArePormotionRecordsNotInstalled.ToString())
            {
                throw new CustomException(string.Format(@Resources.Globalization.ValidationPormotionRecordsNotInstalledText.ToString(), PromotionRecord.PromotionRecordNo, PromotionRecord.JobCategory.JobCategoryName));
            }
            return this.GetSpecificDataFromCandidates(PromotionRecord.PromotionsRecordsEmployees);
        }

        [HttpGet]
        public JsonResult GetPromotionsRecords()
        {
            Session["PromotionRecordEmployees"] = null;
            var list = new PromotionsRecordsBLL().GetPromotionsRecords()
                .Select(x => new
                {
                    PromotionRecordID = x.PromotionRecordID,
                    PromotionRecordNo = x.PromotionRecordNo,
                    PromotionRecordDate = x.PromotionRecordDate,
                    RankName = x.Rank.RankName,
                    JobCategoryName = x.JobCategory.JobCategoryName,
                    PromotionYear = x.PromotionPeriod.Year.MaturityYear,
                    PromotionPeriod = x.PromotionPeriod.Period.PeriodName,
                    PromotionRecordStatusName = x.PromotionRecordStatus.PromotionRecordStatusName,
                    TotalJobVacancies = x.TotalJobVacancies,
                    TotalIncludedCandidates = x.TotalIncludedCandidates,
                    TotalPromotedCandidates = x.TotalPromotedCandidates,
                    CreatedBy = x.CreatedBy != null ? x.CreatedBy.Employee.EmployeeNameAr : string.Empty,
                    CreatedDate = x.CreatedDate
                });

            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetPromotionCandidates/{RankID}/{JobCategoryID}/{PromotionPeriodID}")]
        public JsonResult GetPromotionCandidates(int RankID, int JobCategoryID, int PromotionPeriodID)
        {
            Session["PromotionRecordEmployees"] = null;
            if (RankID == 0 || JobCategoryID == 0 || PromotionPeriodID == 0)
                throw new CustomException(@Resources.Globalization.RankAndPromotionPeriodAndJobCategoryShouldBeSelectedText.ToString());

            Result result = new Result();
            result = new PromotionsRecordsBLL() { LoginIdentity = this.UserIdentity }.GetCandidatesShouldBeInPromotionRecord(JobCategoryID, PromotionPeriodID, RankID);
            PromotionsRecordsBLL PromotionRecord = (PromotionsRecordsBLL)result.Entity;

            //List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesBLLList = PromotionRecord.PromotionsRecordsEmployees;

            if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseThereArePormotionRecordsNotInstalled.ToString())
            {
                Session["PromotionRecordEmployees"] = null;
                throw new CustomException(string.Format(@Resources.Globalization.ValidationPormotionRecordsNotInstalledText.ToString(), PromotionRecord.PromotionRecordNo, PromotionRecord.JobCategory.JobCategoryName));
            }
            else if (PromotionRecord.PromotionsRecordsEmployees == null || PromotionRecord.PromotionsRecordsEmployees.Count == 0)
            {
                throw new CustomException(@Resources.Globalization.ValidationNoCandidatesEligibleInPromotionRecordText.ToString());
            }

            Session["PromotionRecordEmployees"] = PromotionRecord.PromotionsRecordsEmployees;
            return this.GetSpecificDataFromCandidates(PromotionRecord.PromotionsRecordsEmployees);
        }

        [HttpGet]
        [Route("{controller}/GetAllCandidatesByPromotionRecordID/{PromotionRecordID}")]
        public JsonResult GetAllCandidatesByPromotionRecordID(int PromotionRecordID)
        {
            Session["PromotionRecordEmployees"] = null;
            List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployees = new PromotionsRecordsEmployeesBLL().GetByPromotionRecordID(PromotionRecordID);
            return this.GetSpecificDataFromCandidates(PromotionsRecordsEmployees);
        }

        [HttpGet]
        [Route("{controller}/GetLenders/{EmployeeCarrerHistoryID}/{PromotionPeriodID}")]
        public ActionResult GetLenders(int EmployeeCarrerHistoryID, int PromotionPeriodID)
        {
            EmployeesCareersHistoryBLL emp = new EmployeesCareersHistoryBLL().GetByEmployeeCareerHistoryID(EmployeeCarrerHistoryID);
            PromotionsPeriodsBLL PromotionPeriodBLL = new PromotionsPeriodsBLL().GetByPromotionPeriodID(PromotionPeriodID);
            return Json(new { data = new LendersBLL().GetByEmployeeCodeID(emp.EmployeeCode.EmployeeCodeID, emp.JoinDate.Date, PromotionPeriodBLL.PromotionEndDate) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetStudiesVacations/{EmployeeCarrerHistoryID}/{PromotionPeriodID}")]
        public ActionResult GetStudiesVacations(int EmployeeCarrerHistoryID, int PromotionPeriodID)
        {
            EmployeesCareersHistoryBLL emp = new EmployeesCareersHistoryBLL().GetByEmployeeCareerHistoryID(EmployeeCarrerHistoryID);
            PromotionsPeriodsBLL PromotionPeriodBLL = new PromotionsPeriodsBLL().GetByPromotionPeriodID(PromotionPeriodID);
            var data = new StudiesVacationsBLL().GetStudiesVacationsByEmployeeCodeID(emp.EmployeeCode.EmployeeCodeID, emp.JoinDate.Date, PromotionPeriodBLL.PromotionEndDate).Select(x => new
            {
                VacationID = x.VacationID,
                VacationStartDate = x.VacationStartDate,
                VacationEndDate = x.VacationEndDate,
                VacationPeriod = x.VacationPeriod,
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

            //return Json(new { data = new EmployeesCodesBLL().GetVacationsWithDetailsByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetExceptionalVacations/{EmployeeCarrerHistoryID}/{PromotionPeriodID}")]
        public ActionResult GetExceptionalVacations(int EmployeeCarrerHistoryID, int PromotionPeriodID)
        {
            EmployeesCareersHistoryBLL emp = new EmployeesCareersHistoryBLL().GetByEmployeeCareerHistoryID(EmployeeCarrerHistoryID);
            PromotionsPeriodsBLL PromotionPeriodBLL = new PromotionsPeriodsBLL().GetByPromotionPeriodID(PromotionPeriodID);
            var data = new ExceptionalVacationsBLL().GetExceptionalVacationsByEmployeeCodeID(emp.EmployeeCode.EmployeeCodeID, emp.JoinDate.Date, PromotionPeriodBLL.PromotionEndDate).Select(x => new
            {
                VacationID = x.VacationID,
                VacationStartDate = x.VacationStartDate,
                VacationEndDate = x.VacationEndDate,
                VacationPeriod = x.VacationPeriod,
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

            //return Json(new { data = new EmployeesCodesBLL().GetVacationsWithDetailsByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetScholarships/{EmployeeCarrerHistoryID}/{PromotionPeriodID}")]
        public ActionResult GetScholarships(int EmployeeCarrerHistoryID, int PromotionPeriodID)
        {
            EmployeesCareersHistoryBLL emp = new EmployeesCareersHistoryBLL().GetByEmployeeCareerHistoryID(EmployeeCarrerHistoryID);
            PromotionsPeriodsBLL PromotionPeriodBLL = new PromotionsPeriodsBLL().GetByPromotionPeriodID(PromotionPeriodID);
            var data = new BaseScholarshipsBLL().GetByEmployeeCodeIDNotPassed(emp.EmployeeCode.EmployeeCodeID, emp.JoinDate.Date, PromotionPeriodBLL.PromotionEndDate).Select(x => new
            {
                ScholarshipID = x.ScholarshipID,
                ScholarshipStartDate = x.ScholarshipStartDate,
                ScholarshipPeriod = x.ScholarshipPeriod,
                ScholarshipEndDate = x.ScholarshipEndDate,
                IsPassed = x.IsPassed
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

            //return Json(new { data = new EmployeesCodesBLL().GetVacationsWithDetailsByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetStopWorks/{EmployeeCarrerHistoryID}/{PromotionPeriodID}")]
        public ActionResult GetStopWorks(int EmployeeCarrerHistoryID, int PromotionPeriodID)
        {
            EmployeesCareersHistoryBLL emp = new EmployeesCareersHistoryBLL().GetByEmployeeCareerHistoryID(EmployeeCarrerHistoryID);
            PromotionsPeriodsBLL PromotionPeriodBLL = new PromotionsPeriodsBLL().GetByPromotionPeriodID(PromotionPeriodID);
            var data = new StopWorksBLL().GetByEmployeeCodeIDConvicted(emp.EmployeeCode.EmployeeCodeID, emp.JoinDate.Date, PromotionPeriodBLL.PromotionEndDate).Select(x => new
            {
                StopWorkID = x.StopWorkID,
                StopWorkStartDate = x.StopWorkStartDate,
                StopWorkPeriod = x.StopWorkPeriod,
                StopWorkEndDate = x.StopWorkEndDate,
                IsConvicted = x.IsConvicted
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

            //return Json(new { data = new EmployeesCodesBLL().GetVacationsWithDetailsByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetAbsence/{EmployeeCarrerHistoryID}/{PromotionPeriodID}")]
        public ActionResult GetAbsence(int EmployeeCarrerHistoryID, int PromotionPeriodID)
        {
            EmployeesCareersHistoryBLL emp = new EmployeesCareersHistoryBLL().GetByEmployeeCareerHistoryID(EmployeeCarrerHistoryID);
            PromotionsPeriodsBLL PromotionPeriodBLL = new PromotionsPeriodsBLL().GetByPromotionPeriodID(PromotionPeriodID);
            List<TimeAttendanceBLL> EmployeesEvaluationsBLLList = new EmployeesCodesBLL().GetAbsenceByEmployeeCodeID(emp.EmployeeCode.EmployeeCodeID, emp.JoinDate.Date, PromotionPeriodBLL.PromotionEndDate);
            var data = EmployeesEvaluationsBLLList.Select(x => new
            {
                AbsenceDate = x.Date,
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //[CustomAuthorize]
        [Route("{controller}/PrintPromotionDecision/{PromotionRecordID}/{EmployeeCarrerHistoryID}")]
        public ActionResult PrintPromotionDecision(int PromotionRecordID, int? EmployeeCarrerHistoryID = null)
        {

            if (EmployeeCarrerHistoryID == 0)
                EmployeeCarrerHistoryID = null;

            return Redirect(string.Format("~/WebForms/Reports/PromotionDecision.aspx?PromotionRecordID={0}&EmployeeCarrerHistoryID={1}", PromotionRecordID, EmployeeCarrerHistoryID));
        }

        [HttpGet]
        [Route("{controller}/CheckIsDeserveExtraBonusNotSpecifiedValueForCandidatesAlreadyPromoted/{PromotionRecordID}")]
        public JsonResult CheckIsDeserveExtraBonusNotSpecifiedValueForCandidatesAlreadyPromoted(int PromotionRecordID)
        {
            List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesBLLList = new PromotionsRecordsEmployeesBLL().CheckIsDeserveExtraBonusNotSpecifiedValueForCandidatesAlreadyPromoted(PromotionRecordID);
            if (PromotionsRecordsEmployeesBLLList.Count > 0)
            {
                foreach (var PromotionRecordEmployee in PromotionsRecordsEmployeesBLLList)
                {
                    throw new CustomException(Resources.Globalization.ValidationOfIsDeserveExtraBonusNotSpecifiedValueText + "NewLine" + PromotionRecordEmployee.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeNo + ": " + PromotionRecordEmployee.CurrentEmployeeCareer.EmployeeCode.Employee.EmployeeNameAr);
                }
            }
            return Json(HttpStatusCode.OK, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetPromotionDate/{PromotionRecordID}")]
        public JsonResult GetPromotionDate(int PromotionRecordID)
        {
            PromotionsRecordsBLL PromotionsRecord = new PromotionsRecordsBLL().GetByPromotionRecordID(PromotionRecordID);

            return Json(PromotionsRecord.PromotionDate.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"]), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/SetPromotionDate/{PromotionRecordID}/{PromotionDate}")]
        public JsonResult SetPromotionDate(string PromotionRecordID, string PromotionDate)
        {
            PromotionsRecordsBLL PromotionRecord = new PromotionsRecordsBLL();
            PromotionRecord.PromotionRecordID = int.Parse(PromotionRecordID);
            PromotionRecord.PromotionDate = Convert.ToDateTime(Globals.Calendar.UmAlquraToGreg(PromotionDate), new CultureInfo("en-US"));
            PromotionRecord.LoginIdentity = UserIdentity;
            Result result = PromotionRecord.UpdatePromotionDate();

            if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionDateShouldBeLessThanPromotionRecordDate.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationPromotionDateShouldBeLessThanPromotionRecordDateText);
            }

            return Json(HttpStatusCode.OK, JsonRequestBehavior.AllowGet);
        }

        //[CustomAuthorize]
        [Route("{controller}/PrintPromotionRecord/{PromotionRecordID}")]
        public ActionResult PrintPromotionRecord(int PromotionRecordID)
        {
            return Redirect(string.Format("~/WebForms/Reports/PromotionRecord.aspx?PromotionRecordID={0}", PromotionRecordID));
        }

        public JsonResult GetSpecificDataFromCandidates(List<PromotionsRecordsEmployeesBLL> employees)
        {
            return Json(new
            {
                data = employees.Select(x => new
                {
                    EmployeeCareerHistoryID = x.CurrentEmployeeCareer.EmployeeCareerHistoryID,
                    EmployeeCodeID = x.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID,
                    EmployeeCodeNo = x.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = x.CurrentEmployeeCareer.EmployeeCode.Employee.EmployeeNameAr,
                    JobName = x.CurrentEmployeeCareer.OrganizationJob.Job.JobName,
                    JobNo = x.CurrentEmployeeCareer.OrganizationJob.JobNo,
                    JobCode = x.CurrentEmployeeCareer.OrganizationJob.Job.JobCode,
                    JobCategoryName = x.CurrentEmployeeCareer.OrganizationJob.Job.JobCategory.JobCategoryName,
                    OrganizationName = x.CurrentEmployeeCareer.OrganizationJob.OrganizationStructure.OrganizationName,
                    RankName = x.CurrentEmployeeCareer.OrganizationJob.Rank.RankName,
                    JoinDate = x.CurrentEmployeeCareer.JoinDate.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString()),
                    LastQualificationDegreeName = x.LastQualificationDegree != null ? x.LastQualificationDegree.QualificationDegreeName : string.Empty,
                    LastQualificationName = x.LastQualification != null ? x.LastQualification.QualificationName : string.Empty,
                    LastGeneralSpecializationName = x.LastGeneralSpecialization != null ? x.LastGeneralSpecialization.GeneralSpecializationName : string.Empty,
                    IsIncluded = x.IsIncluded,
                    AddedWayID = x.PromotionCandidateAddedWay.PromotionCandidateAddedWayID,
                    AddedWayName = x.PromotionCandidateAddedWay.PromotionCandidateAddedWayName,
                    ManualAddedReason = x.ManualAddedReason,
                    CreatedBy = x.CreatedBy.Employee != null ? x.CreatedBy.Employee.EmployeeNameAr : "",
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetCandidatesAlreadyPromotedByPromotionRecordID/{PromotionRecordID}")]
        public JsonResult GetCandidatesAlreadyPromotedByPromotionRecordID(int PromotionRecordID)
        {
            List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesList = new PromotionsRecordsEmployeesBLL().GetCandidatesAlreadyPromoted(PromotionRecordID);

            var data = PromotionsRecordsEmployeesList.Select(x => new
            {
                PromotionRecordEmployeeID = x.PromotionRecordEmployeeID,
                EmployeeCodeNo = x.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.CurrentEmployeeCareer.EmployeeCode.Employee.EmployeeNameAr,
                JobName = x.NewEmployeeCareer.OrganizationJob.Job.JobName,
                JobCode = x.NewEmployeeCareer.OrganizationJob.Job.JobCode,
                OrganizationName = x.NewEmployeeCareer.OrganizationJob.OrganizationStructure.OrganizationName,
                RankName = x.NewEmployeeCareer.OrganizationJob.Rank.RankName,
                IsApproved = x.IsApproved,
                IsDeserveExtraBonus = x.IsDeserveExtraBonus,
                AbsentDays = x.AbsentDays
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("RemovingReason")]
        public JsonResult UpdatePromotionRecordEmployeeIsDeserveExtraBonus(PromotionsRecordsEmployeesViewModel PromotionRecordEmployeeVM)
        {
            Result result = null;

            PromotionsRecordsEmployeesBLL PromotionRecordEmployeeBLL = new PromotionsRecordsEmployeesBLL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeVM.PromotionRecordEmployeeID);

            PromotionRecordEmployeeBLL.IsDeserveExtraBonus = PromotionRecordEmployeeVM.IsDeserveExtraBonus;
            PromotionRecordEmployeeBLL.LoginIdentity = this.UserIdentity;

            result = PromotionRecordEmployeeBLL.UpdateDeserveExtraBonus();

            if (result.EnumMember == CareersHistoryValidationEnum.RejectedBecauseOfCareerDegreeOutOfRange.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationOfCareerDegreeOutOfRangeText);
            }

            if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.Done.ToString())
            {
                return Json(HttpStatusCode.OK, JsonRequestBehavior.AllowGet);
            }
            if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfIsDeserveExtraBonusNotSpecifiedValue.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationOfIsDeserveExtraBonusNotSpecifiedValueText);
            }
            else if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfIsDeserveExtraBonusUpdateWithSameValue.ToString())
            {
                // throw new CustomException(Resources.Globalization.ValidationOfIsDeserveExtraBonusUpdateWithSameValueText);
                return Json(HttpStatusCode.OK, JsonRequestBehavior.AllowGet);
            }
            else if (result.EnumMember == PromotionsRecordsEmployeesValidationEnum.RejectedBecauseOfApproved.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationOfApprovedText);
            }
            else
            {
                return Json(HttpStatusCode.ExpectationFailed, JsonRequestBehavior.AllowGet);
                //return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("{controller}/Close/{PromotionRecordID}")]
        public JsonResult Close(int PromotionRecordID)
        {
            PromotionsRecordsBLL PromotionsRecordsBLL = new PromotionsRecordsBLL() { LoginIdentity = UserIdentity };
            Result result = PromotionsRecordsBLL.Close(PromotionRecordID);



            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsBLL PromotionRecordResult = (PromotionsRecordsBLL)result.Entity;
                var data = new
                {
                    PromotionRecordToolbarID = GetPromotionRecordToolbarID(6),
                    PromotionRecordStatus = PromotionRecordResult.PromotionRecordStatus
                };


                return Json(new { result = data }, JsonRequestBehavior.AllowGet);
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeInstalled.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationPromotionRecordStatusMustBeInstalledText);
            }
            else
            {
                return Json(HttpStatusCode.ExpectationFailed, JsonRequestBehavior.AllowGet);
            }
        }

    }
}