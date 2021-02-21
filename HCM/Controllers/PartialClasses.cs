using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.Enums;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public partial class PromotionsRecordsController : BaseController
    {
        [CustomAuthorize]
        public ActionResult Manage(int id)
        {
            PromotionsRecordsViewModel PromotionRecordVM = this.GetByPromotionRecordID(id);

            #region Adding Log
            new PromotionsRecordsLogsBLL()
            {
                PromotionRecord = new PromotionsRecordsBLL() { PromotionRecordID = PromotionRecordVM.PromotionRecordID.HasValue ? PromotionRecordVM.PromotionRecordID.Value : 0 },
                PromotionRecordNo = PromotionRecordVM.PromotionRecordNo.HasValue ? PromotionRecordVM.PromotionRecordNo.Value : 0,
                PromotionRecordActionType = new PromotionsRecordsActionsTypesBLL() { PromotionActionTypeID = (int)PromotionsRecordsActionsTypesEnum.View },
                ActionDescription = string.Empty,
                LoginIdentity = this.UserIdentity,
            }.Add();
            #endregion

            ViewBag.PromotionRecordQualificationKindList = new PromotionsRecordsQualificationsKindsBLL().GetPromotionsRecordsQualificationsKinds();
            return View(PromotionRecordVM);
        }

        [HttpGet]
        public JsonResult GetPromotionsRecordsEmployeesByPromotionRecordID(int id)
        {
            //int i = 1;
            bool IsIncluded = true;
            var list = new PromotionsRecordsEmployeesBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            }.GetByPromotionRecordID(id, IsIncluded, out TotalRecordsOut, out RecFilterOut).Select(p => new
            {
                //CandidateSerialNo = i++,
                p.PromotionRecordEmployeeID,
                EmployeeCareerHistoryID = p.CurrentEmployeeCareer.EmployeeCareerHistoryID,
                EmployeeCodeID = p.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeID,
                EmployeeCodeNo = p.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeNo,
                //EmployeeIDNo = p.CurrentEmployeeCareer.EmployeeCode.Employee.EmployeeIDNo,
                EmployeeNameAr = p.CurrentEmployeeCareer.EmployeeCode.Employee.EmployeeNameAr,
                RankName = p.CurrentEmployeeCareer.OrganizationJob.Rank.RankName,
                JobCategoryName = p.CurrentEmployeeCareer.OrganizationJob.Job.JobCategory.JobCategoryName,
                JobNo = p.CurrentEmployeeCareer.OrganizationJob.JobNo,
                JobCode = p.CurrentEmployeeCareer.OrganizationJob.Job.JobCode,
                JobName = p.CurrentEmployeeCareer.OrganizationJob.Job.JobName,
                //JobStatus = p.CurrentEmployeeCareer.OrganizationJob.IsActive,     // ??????????
                OrganizationName = p.CurrentEmployeeCareer.OrganizationJob.OrganizationStructure.OrganizationNameWithBranch,
                JoinDate = p.CurrentEmployeeCareer.JoinDate,//.ToString(System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString()),
                AddedWayID = p.PromotionCandidateAddedWay.PromotionCandidateAddedWayID,
                AddedWayName = p.PromotionCandidateAddedWay.PromotionCandidateAddedWayName,
                ManualAddedReason = p.ManualAddedReason,
                CreatedBy = p.CreatedBy.Employee != null ? p.CreatedBy.Employee.EmployeeNameAr : "",
                CreatedDate = p.CreatedDate,

                LastQualificationDegreeName = p.LastQualificationDegree != null ? p.LastQualificationDegree.QualificationDegreeName : "",
                LastQualificationName = p.LastQualification != null ? p.LastQualification.QualificationName : "",
                LastGeneralSpecializationName = p.LastGeneralSpecialization != null ? p.LastGeneralSpecialization.GeneralSpecializationName : "",

                NewRankName = p.PromotionRecordJobVacancy != null ? p.PromotionRecordJobVacancy.JobVacancy.Rank.RankName : "",
                NewJobCategoryName = p.PromotionRecordJobVacancy != null ? p.PromotionRecordJobVacancy.JobVacancy.Job.JobCategory.JobCategoryName : "",
                NewJobNo = p.PromotionRecordJobVacancy != null ? p.PromotionRecordJobVacancy.JobVacancy.JobNo : "",
                NewJobCode = p.PromotionRecordJobVacancy != null ? p.PromotionRecordJobVacancy.JobVacancy.Job.JobCode : "",
                NewJobName = p.PromotionRecordJobVacancy != null ? p.PromotionRecordJobVacancy.JobVacancy.Job.JobName : "",
                NewOrganizationName = p.PromotionRecordJobVacancy != null ? p.PromotionRecordJobVacancy.JobVacancy.OrganizationStructure.OrganizationNameWithBranch : "",

                IsRemovedAfterIncluding = p.IsRemovedAfterIncluding,

                TotalEducationPoints = p.EducationPoints,
                TotalSeniorityPoints = p.SeniorityPoints,
                TotalEvaluationPoints = p.EvaluationPoints,
                ActualTaskPerformancePoints = p.ActualTaskPerformancePoints,
                TotalPoints = p.TotalPoints,
                PromotionDecisionName = p.PromotionDecision != null ? p.PromotionDecision.PromotionDecisionName : "",
                LastQualificationWeight = p.LastQualificationDegree != null ? p.LastQualificationDegree.Weight : 0,

                PromotionDecisionSupportTitle = GetPromotionDecisionSupportTitle(p.PromotionDecision),
                PromotionDecisionSupportValue = GetPromotionDecisionSupportValue(p),

                PromotionRecordStatusEnum = p.PromotionRecord.PromotionRecordStatusEnum,

                RemovingReason = p.RemovingReason,
                RemovedByName = p.RemovingBy != null ? p.RemovingBy.Employee.EmployeeNameAr : "",

                PromotionRecordJobVacancyID = p.PromotionRecordJobVacancy != null ? p.PromotionRecordJobVacancy.PromotionRecordJobVacancyID : 0,
                PromotionPeriodID = p.PromotionRecord.PromotionPeriod.PromotionPeriodID,

                IsByExamResult = p.ByExamResult.HasValue ? 1 : 0,
                ByExamResult = p.ByExamResult.HasValue ? p.ByExamResult.Value : 0


            });

            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPromotionsRecordsStatus()
        {
            return Json(new { data = new PromotionsRecordsStatusBLL().GetPromotionsRecordsStatus() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Preference(PromotionsRecordsViewModel PromotionsRecordsVM)
        {
            Result result = new Result();
            PromotionsRecordsBLL pp = new PromotionsRecordsBLL()
            {
                PromotionRecordID = PromotionsRecordsVM.PromotionRecordID.Value,
                JobCategory = new JobsCategoriesBLL() { JobCategoryID = PromotionsRecordsVM.JobCategoryID },
                Rank = new RanksBLL() { RankID = PromotionsRecordsVM.RankID },
                LoginIdentity = this.UserIdentity
            };
            result = pp.Preference();

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsBLL oo = (PromotionsRecordsBLL)result.Entity;
                PromotionsRecordsVM.PromotionRecordID = oo.PromotionRecordID;
                //PromotionsRecordsVM.PromotionRecordNo = oo.PromotionRecordNo;
                //PromotionsRecordsVM.PromotionRecordDate = oo.PromotionRecordDate;
                PromotionsRecordsVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
                {
                    PromotionRecordStatusID = oo.PromotionRecordStatus.PromotionRecordStatusID,
                    PromotionRecordStatusName = oo.PromotionRecordStatus.PromotionRecordStatusName
                };
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoJobsVacanciesAvailableText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoCandidatesEligibleInPromotionRecordText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfSomeQualifitionPointsAreNull.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationSomeQualifitionPointsAreNullInPromotionRecordText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpen.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBeOpenText.ToString());
            }
            //PromotionRecordVM.PromotionRecordToolbarID = GetPromotionRecordToolbarID(PromotionRecordBLL.PromotionRecordStatus.PromotionRecordStatusID);
            return Json(new
            {
                PromotionRecordStatus = PromotionsRecordsVM.PromotionRecordStatus,
                PromotionRecordToolbarID = GetPromotionRecordToolbarID(PromotionsRecordsVM.PromotionRecordStatus.PromotionRecordStatusID)
            },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UndoPreference(PromotionsRecordsViewModel PromotionsRecordsVM)
        {
            Result result = new Result();
            PromotionsRecordsBLL pp = new PromotionsRecordsBLL()
            {
                PromotionRecordID = PromotionsRecordsVM.PromotionRecordID.Value,
                JobCategory = new JobsCategoriesBLL() { JobCategoryID = PromotionsRecordsVM.JobCategoryID },
                Rank = new RanksBLL() { RankID = PromotionsRecordsVM.RankID },
                LoginIdentity = this.UserIdentity
            };
            result = pp.UndoPreference();

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsBLL oo = (PromotionsRecordsBLL)result.Entity;
                PromotionsRecordsVM.PromotionRecordID = oo.PromotionRecordID;
                //PromotionsRecordsVM.PromotionRecordNo = oo.PromotionRecordNo;
                //PromotionsRecordsVM.PromotionRecordDate = oo.PromotionRecordDate;
                PromotionsRecordsVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
                {
                    PromotionRecordStatusID = oo.PromotionRecordStatus.PromotionRecordStatusID,
                    PromotionRecordStatusName = oo.PromotionRecordStatus.PromotionRecordStatusName
                };
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoJobsVacanciesAvailableText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoCandidatesEligibleInPromotionRecordText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBePreferenced.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBePreferencedText.ToString());
            }

            //return Json(PromotionsRecordsVM.PromotionRecordStatus, JsonRequestBehavior.AllowGet);
            return Json(new
            {
                PromotionRecordStatus = PromotionsRecordsVM.PromotionRecordStatus,
                PromotionRecordToolbarID = GetPromotionRecordToolbarID(PromotionsRecordsVM.PromotionRecordStatus.PromotionRecordStatusID)
            },
               JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Install(PromotionsRecordsViewModel PromotionsRecordsVM)
        {
            Result result = new Result();
            PromotionsRecordsBLL pp = new PromotionsRecordsBLL()
            {
                PromotionRecordID = PromotionsRecordsVM.PromotionRecordID.Value,
                JobCategory = new JobsCategoriesBLL() { JobCategoryID = PromotionsRecordsVM.JobCategoryID },
                Rank = new RanksBLL() { RankID = PromotionsRecordsVM.RankID },
                LoginIdentity = this.UserIdentity
            };
            result = pp.Install();

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsBLL oo = (PromotionsRecordsBLL)result.Entity;
                PromotionsRecordsVM.PromotionRecordID = oo.PromotionRecordID;
                //PromotionsRecordsVM.PromotionRecordNo = oo.PromotionRecordNo;
                //PromotionsRecordsVM.PromotionRecordDate = oo.PromotionRecordDate;
                PromotionsRecordsVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
                {
                    PromotionRecordStatusID = oo.PromotionRecordStatus.PromotionRecordStatusID,
                    PromotionRecordStatusName = oo.PromotionRecordStatus.PromotionRecordStatusName
                };
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoJobsVacanciesAvailableText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoCandidatesEligibleInPromotionRecordText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBePreferenced.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBePreferencedText.ToString());
            }

            return Json(new
            {
                PromotionRecordStatus = PromotionsRecordsVM.PromotionRecordStatus,
                PromotionRecordToolbarID = GetPromotionRecordToolbarID(PromotionsRecordsVM.PromotionRecordStatus.PromotionRecordStatusID)
            },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UndoInstall(PromotionsRecordsViewModel PromotionsRecordsVM)
        {
            Result result = new Result();
            PromotionsRecordsBLL pp = new PromotionsRecordsBLL()
            {
                PromotionRecordID = PromotionsRecordsVM.PromotionRecordID.Value,
                JobCategory = new JobsCategoriesBLL() { JobCategoryID = PromotionsRecordsVM.JobCategoryID },
                Rank = new RanksBLL() { RankID = PromotionsRecordsVM.RankID },
                LoginIdentity = this.UserIdentity
            };
            result = pp.UndoInstall();

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsBLL oo = (PromotionsRecordsBLL)result.Entity;
                PromotionsRecordsVM.PromotionRecordID = oo.PromotionRecordID;
                //PromotionsRecordsVM.PromotionRecordNo = oo.PromotionRecordNo;
                //PromotionsRecordsVM.PromotionRecordDate = oo.PromotionRecordDate;
                PromotionsRecordsVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
                {
                    PromotionRecordStatusID = oo.PromotionRecordStatus.PromotionRecordStatusID,
                    PromotionRecordStatusName = oo.PromotionRecordStatus.PromotionRecordStatusName
                };
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoJobsVacanciesAvailableText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoCandidatesEligibleInPromotionRecordText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeInstalled.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBeInstalledText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfAssignedJobCategoryIsInInstalledStatus.ToString())
            {
                PromotionsRecordsBLL ooo = (PromotionsRecordsBLL)result.Entity;
                //PromotionsRecordsVM.PromotionRecordID = ooo.PromotionRecordID; 
                //PromotionsRecordsVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
                //{
                //    PromotionRecordStatusID = ooo.PromotionRecordStatus.PromotionRecordStatusID,
                //    PromotionRecordStatusName = ooo.PromotionRecordStatus.PromotionRecordStatusName
                //};
                throw new CustomException(string.Format(@Resources.Globalization.ValidationPromotionRecordCannotUndoInsalledBecauseAssignedJobCategoryHasInstalledStatusText.ToString(), ooo.PromotionRecordNo, ooo.JobCategory.JobCategoryName));
                //throw new CustomException(@Resources.Globalization.ValidationPromotionRecordCannotUndoInsalledBecauseAssignedJobCategoryHasInstalledStatusText.ToString());
            }

            return Json(new
            {
                PromotionRecordStatus = PromotionsRecordsVM.PromotionRecordStatus,
                PromotionRecordToolbarID = GetPromotionRecordToolbarID(PromotionsRecordsVM.PromotionRecordStatus.PromotionRecordStatusID)
            },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveByPromotionRecordEmployeeID(PromotionsRecordsEmployeesViewModel PromotionRecordEmployeeVM)
        {
            PromotionsRecordsViewModel PromotionsRecordsVM = new PromotionsRecordsViewModel();
            Result result = new Result();
            PromotionsRecordsEmployeesBLL emp = new PromotionsRecordsEmployeesBLL()
            {
                PromotionRecordEmployeeID = PromotionRecordEmployeeVM.PromotionRecordEmployeeID,
                RemovingReason = PromotionRecordEmployeeVM.RemovingReason,
                RemovingBy = this.UserIdentity,
                LoginIdentity = this.UserIdentity
            };
            result = emp.RemoveAndRedistributeJobs();

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsEmployeesBLL oo = (PromotionsRecordsEmployeesBLL)result.Entity;
                PromotionsRecordsVM.PromotionRecordID = oo.PromotionRecord.PromotionRecordID;
                //PromotionsRecordsVM.PromotionRecordNo = oo.PromotionRecordNo;
                //PromotionsRecordsVM.PromotionRecordDate = oo.PromotionRecordDate;
                PromotionsRecordsVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
                {
                    PromotionRecordStatusID = oo.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusID,
                    PromotionRecordStatusName = oo.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusName
                };
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpenOrPreferenced.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBeOpenOrPreferencedText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoJobsVacanciesAvailableText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoCandidatesEligibleInPromotionRecordText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfSomeQualifitionPointsAreNull.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationSomeQualifitionPointsAreNullInPromotionRecordText.ToString());
            }

            return Json(new
            {
                PromotionRecordStatus = PromotionsRecordsVM.PromotionRecordStatus,
                PromotionRecordToolbarID = GetPromotionRecordToolbarID(PromotionsRecordsVM.PromotionRecordStatus.PromotionRecordStatusID)
            },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IncludeAgainAfterRemovedByPromotionRecordEmployeeID(int id)
        {
            PromotionsRecordsViewModel PromotionsRecordsVM = new PromotionsRecordsViewModel();
            Result result = new Result();
            PromotionsRecordsEmployeesBLL emp = new PromotionsRecordsEmployeesBLL()
            {
                PromotionRecordEmployeeID = id,
                LoginIdentity = this.UserIdentity
            };
            result = emp.IncludeAgainAfterRemoved();

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsEmployeesBLL oo = (PromotionsRecordsEmployeesBLL)result.Entity;
                PromotionsRecordsVM.PromotionRecordID = oo.PromotionRecord.PromotionRecordID;
                PromotionsRecordsVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
                {
                    PromotionRecordStatusID = oo.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusID,
                    PromotionRecordStatusName = oo.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusName
                };
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpenOrPreferenced.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBeOpenOrPreferencedText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoJobsVacanciesAvailableText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoCandidatesEligibleInPromotionRecordText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfSomeQualifitionPointsAreNull.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationSomeQualifitionPointsAreNullInPromotionRecordText.ToString());
            }

            return Json(new
            {
                PromotionRecordStatus = PromotionsRecordsVM.PromotionRecordStatus,
                PromotionRecordToolbarID = GetPromotionRecordToolbarID(PromotionsRecordsVM.PromotionRecordStatus.PromotionRecordStatusID)
            },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddActualTaskPerformancePointsByPromotionRecordEmployeeID(int id)
        {
            PromotionsRecordsViewModel PromotionsRecordsVM = new PromotionsRecordsViewModel();
            Result result = new Result();
            PromotionsRecordsEmployeesBLL emp = new PromotionsRecordsEmployeesBLL()
            {
                PromotionRecordEmployeeID = id,
                ActualTaskPerformancePoints = Convert.ToDecimal(ConfigurationManager.AppSettings["ActualTaskPerformancePoints"]),
                LoginIdentity = this.UserIdentity
            };
            result = emp.UpdateActualTaskPerformancePoints();

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsEmployeesBLL oo = (PromotionsRecordsEmployeesBLL)result.Entity;
                PromotionsRecordsVM.PromotionRecordID = oo.PromotionRecord.PromotionRecordID;
                PromotionsRecordsVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
                {
                    PromotionRecordStatusID = oo.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusID,
                    PromotionRecordStatusName = oo.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusName
                };
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpenOrPreferenced.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBeOpenOrPreferencedText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoJobsVacanciesAvailableText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoCandidatesEligibleInPromotionRecordText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfSomeQualifitionPointsAreNull.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationSomeQualifitionPointsAreNullInPromotionRecordText.ToString());
            }

            return Json(new
            {
                PromotionRecordStatus = PromotionsRecordsVM.PromotionRecordStatus,
                PromotionRecordToolbarID = GetPromotionRecordToolbarID(PromotionsRecordsVM.PromotionRecordStatus.PromotionRecordStatusID)
            },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveActualTaskPerformancePointsByPromotionRecordEmployeeID(int id)
        {
            PromotionsRecordsViewModel PromotionsRecordsVM = new PromotionsRecordsViewModel();
            Result result = new Result();
            PromotionsRecordsEmployeesBLL emp = new PromotionsRecordsEmployeesBLL()
            {
                PromotionRecordEmployeeID = id,
                ActualTaskPerformancePoints = 0,
                LoginIdentity = this.UserIdentity
            };
            result = emp.UpdateActualTaskPerformancePoints();

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsEmployeesBLL oo = (PromotionsRecordsEmployeesBLL)result.Entity;
                PromotionsRecordsVM.PromotionRecordID = oo.PromotionRecord.PromotionRecordID;
                PromotionsRecordsVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
                {
                    PromotionRecordStatusID = oo.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusID,
                    PromotionRecordStatusName = oo.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusName
                };
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpenOrPreferenced.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBeOpenOrPreferencedText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoJobsVacanciesAvailable.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoJobsVacanciesAvailableText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNoCandidatesEligible.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNoCandidatesEligibleInPromotionRecordText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfSomeQualifitionPointsAreNull.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationSomeQualifitionPointsAreNullInPromotionRecordText.ToString());
            }

            return Json(new
            {
                PromotionRecordStatus = PromotionsRecordsVM.PromotionRecordStatus,
                PromotionRecordToolbarID = GetPromotionRecordToolbarID(PromotionsRecordsVM.PromotionRecordStatus.PromotionRecordStatusID)
            },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("{controller}/ShuffleJob/{PromotionRecordEmployeeID}/{NewEmployeeCodeNo}")]
        public ActionResult ShuffleJob(int PromotionRecordEmployeeID, string NewEmployeeCodeNo)
        {
            Result result = new Result();
            PromotionsRecordsEmployeesViewModel PromotionsRecordsEmployeesVM = new PromotionsRecordsEmployeesViewModel();

            PromotionsRecordsEmployeesBLL pre = new PromotionsRecordsEmployeesBLL()
            {
                LoginIdentity = this.UserIdentity
            };
            result = pre.ShuffleJob(PromotionRecordEmployeeID, NewEmployeeCodeNo);

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsEmployeesBLL oo = (PromotionsRecordsEmployeesBLL)result.Entity;
                PromotionsRecordsEmployeesVM.PromotionRecordEmployeeID = oo.PromotionRecordEmployeeID;
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBePreferencedOrInstalled.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBePreferencedOrInstalledText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfNewEmployeeCodeNoNotExitsInPromotionRecordEmployeesOrNotPromoted.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationNewEmployeeCodeNoNotExitsInPromotionRecordEmployeesOrNotPromotedText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecauseOfOneOfEmployeeInRedistributeJobIsApproved.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordOneOfEmployeeInRedistributeJobIsApprovedText.ToString());
            }


            return Json(new
            {
                PromotionRecordEmployeeID = PromotionsRecordsEmployeesVM.PromotionRecordEmployeeID,
            },
                JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetPromotionRecordEmployeeByPromotionRecordEmployeeID/{PromotionRecordEmployeeID}")]
        public JsonResult GetPromotionRecordEmployeeByPromotionRecordEmployeeID(int PromotionRecordEmployeeID)
        {
            PromotionsRecordsEmployeesBLL PromotionRecordEmployee = new PromotionsRecordsEmployeesBLL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID);
            return Json(new
            {
                data = PromotionRecordEmployee
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("PromotionsRecords/RenderToolBar/{PromotionRecordToolbar}")]
        public ActionResult RenderToolBar(PromotionsRecordsToolbarEnum PromotionRecordToolbar)
        {
            if (PromotionRecordToolbar == PromotionsRecordsToolbarEnum.Open)
                return PartialView("~/Views/PromotionsRecords/_OpenToolbar.cshtml");
            else if (PromotionRecordToolbar == PromotionsRecordsToolbarEnum.Opened)
                return PartialView("~/Views/PromotionsRecords/_OpenedToolbar.cshtml");
            else if (PromotionRecordToolbar == PromotionsRecordsToolbarEnum.Preference)
                return PartialView("~/Views/PromotionsRecords/_PreferenceToolbar.cshtml");
            else if (PromotionRecordToolbar == PromotionsRecordsToolbarEnum.Install)
                return PartialView("~/Views/PromotionsRecords/_InstallToolbar.cshtml");
            else if (PromotionRecordToolbar == PromotionsRecordsToolbarEnum.Close)
                return PartialView("~/Views/PromotionsRecords/_CloseToolbar.cshtml");
            else if (PromotionRecordToolbar == PromotionsRecordsToolbarEnum.Closed)
                return PartialView("~/Views/PromotionsRecords/_ClosedToolbar.cshtml");

            return null;
        }

        [HttpGet]
        public JsonResult GetJobsVacanciesByPromotionRecordID(int id)
        {
            return Json(new
            {
                data = new PromotionsRecordsJobsVacanciesBLL().GetByPromotionRecordID(id).Select(item => new
                {
                    OrganizationJobID = item.JobVacancy.OrganizationJobID,
                    JobGeneralGroupName = item.JobVacancy.Job.JobCategory.JobGroup.JobGeneralGroup.JobGeneralGroupName,
                    JobGroupName = item.JobVacancy.Job.JobCategory.JobGroup.JobGroupName,
                    JobCategoryName = item.JobVacancy.Job.JobCategory.JobCategoryName,
                    JobCode = item.JobVacancy.Job.JobCode,
                    JobName = item.JobVacancy.Job.JobName,
                    JobNo = item.JobVacancy.JobNo,
                    RankName = item.JobVacancy.Rank.RankName,
                    OrganizationName = item.JobVacancy.OrganizationStructure.OrganizationName,
                    IsReserved = item.JobVacancy.IsReserved
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{controller}/GetCandidatesForRedistributeJobsByPromotionRecordID/{PromotionRecordID}")]
        public JsonResult GetCandidatesForRedistributeJobsByPromotionRecordID(int PromotionRecordID)
        {
            List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesList = new PromotionsRecordsEmployeesBLL().GetCandidatesForRedistributeJobsByPromotionRecordID(PromotionRecordID);

            var data = PromotionsRecordsEmployeesList.Select(x => new
            {
                PromotionRecordEmployeeID = x.PromotionRecordEmployeeID,
                EmployeeCodeNo = x.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.CurrentEmployeeCareer.EmployeeCode.Employee.EmployeeNameAr,
                PromotionRecordJobVacancyID = x.PromotionRecordJobVacancy.PromotionRecordJobVacancyID,
                JobCode = x.PromotionRecordJobVacancy.JobVacancy.Job.JobCode,
                JobName = x.PromotionRecordJobVacancy.JobVacancy.Job.JobName,
                JobNo = x.PromotionRecordJobVacancy.JobVacancy.JobNo,
                RankName = x.PromotionRecordJobVacancy.JobVacancy.Rank.RankName,
                OrganizationName = x.PromotionRecordJobVacancy.JobVacancy.OrganizationStructure.OrganizationName,
            });

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{controller}/GetQualifications/{EmployeeCodeID}/{PromotionRecordEmployeeID}")]
        public ActionResult GetQualifications(int EmployeeCodeID, int PromotionRecordEmployeeID)
        {
            List<EmployeesQualificationsBLL> EmployeesQualificationsBLLList = new EmployeesCodesBLL().GetQualificationsByEmployeeCodeID(EmployeeCodeID, PromotionRecordEmployeeID);

            var data = EmployeesQualificationsBLLList.Select(x => new
            {
                EmployeeQualificationID = x.EmployeeQualificationID,
                QualificationDegreeName = x.QualificationDegree.QualificationDegreeName,
                QualificationName = x.Qualification != null ? x.Qualification.QualificationName : string.Empty,
                GeneralSpecializationName = x.GeneralSpecialization != null ? x.GeneralSpecialization.GeneralSpecializationName : string.Empty,
                GraduationDate = x.GraduationDate.HasValue ? x.GraduationDate.Value.Date : (DateTime?)null,
                Points = x.Points,
                Weight = x.Weight > 0 ? x.Weight.ToString() : "",
                IsIncluded = x.IsIncluded,
            });

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{controller}/GetEvaluations/{EmployeeCodeID}/{PromotionRecordEmployeeID}")]
        public ActionResult GetEvaluations(int EmployeeCodeID, int PromotionRecordEmployeeID)
        {
            List<EmployeesEvaluationsBLL> EmployeesEvaluationsBLLList = new EmployeesCodesBLL().GetEvaluationsByEmployeeCodeID(EmployeeCodeID, PromotionRecordEmployeeID);

            var data = EmployeesEvaluationsBLLList.Select(x => new
            {
                EvaluationDegree = x.EvaluationDegree,
                EvaluationYear = x.MaturityYearsBalances.MaturityYear,
                Evaluation = x.EvaluationPoints.Evaluation,
                Points = x.Points
            });

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{controller}/GetSeniority/{PromotionRecordEmployeeID}")]
        public ActionResult GetSeniority(int PromotionRecordEmployeeID)
        {
            List<PromotionsRecordsEmployeesSeniorityDetailsBLL> List = new PromotionsRecordsEmployeesSeniorityDetailsBLL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID);
            var data = List.Select(x => new
            {
                CurrentJobSeniorityMonths = x.PromotionRecordEmployee.CurrentJobSeniorityMonths,
                x.Months,
                x.Points,
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{controller}/GetPromotionRecordLogsByPromotionRecordID/{PromotionRecordID}")]
        public JsonResult GetPromotionRecordLogsByPromotionRecordID(int PromotionRecordID)
        {
            var List = new PromotionsRecordsLogsBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            }.GetByPromotionRecordID(PromotionRecordID, out TotalRecordsOut, out RecFilterOut).Select(p => new
            {
                p.PromotionRecordLogID,
                p.PromotionRecordNo,
                p.ActionDescription,
                PromotionActionTypeName = p.PromotionRecordActionType.PromotionActionTypeName,
                p.ActionDate,
                ActionBy = p.ActionBy.Employee.EmployeeNameAr
            });

            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = List }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{controller}/GetCandidatesByExamByPromotionRecordID/{PromotionRecordID}")]
        public JsonResult GetCandidatesByExamByPromotionRecordID(int PromotionRecordID)
        {
            List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesList = new PromotionsRecordsEmployeesBLL().GetCandidatesByExamByPromotionRecordID(PromotionRecordID);

            var data = PromotionsRecordsEmployeesList.Select(x => new
            {
                PromotionRecordEmployeeID = x.PromotionRecordEmployeeID,
                EmployeeCodeNo = x.CurrentEmployeeCareer.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.CurrentEmployeeCareer.EmployeeCode.Employee.EmployeeNameAr,
                ByExamResult = x.ByExamResult.HasValue ? x.ByExamResult.Value : 0
            });

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("{controller}/UpdateExamResult/{PromotionRecordEmployeeID}/{ExamResult}")]
        public ActionResult UpdateExamResult(int PromotionRecordEmployeeID, decimal ExamResult)
        {
            Result result = new Result();
            PromotionsRecordsEmployeesViewModel PromotionsRecordsEmployeesVM = new PromotionsRecordsEmployeesViewModel();

            PromotionsRecordsEmployeesBLL pre = new PromotionsRecordsEmployeesBLL()
            {
                LoginIdentity = this.UserIdentity
            };
            result = pre.UpdateExamResult(PromotionRecordEmployeeID, ExamResult);

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsEmployeesBLL oo = (PromotionsRecordsEmployeesBLL)result.Entity;
                PromotionsRecordsEmployeesVM.PromotionRecordEmployeeID = oo.PromotionRecordEmployeeID;
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBePreferenced.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBePreferencedOrInstalledText.ToString());
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionDecisionMustBeShouldBePromotedByExam.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionDecisionMustBeShouldBePromotedByExamText.ToString());
            }

            return Json(new
            {
                PromotionRecordEmployeeID = PromotionsRecordsEmployeesVM.PromotionRecordEmployeeID,
            },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Route("{controller}/ResetByExamResults/{PromotionRecordID}")]
        public ActionResult ResetByExamResults(int PromotionRecordID)
        {
            Result result = new Result();
            PromotionsRecordsViewModel PromotionsRecordsVM = new PromotionsRecordsViewModel();

            result = new PromotionsRecordsEmployeesBLL() { LoginIdentity = this.UserIdentity }.ResetByExamResults(PromotionRecordID);

            if (result.EnumMember == PromotionsRecordsValidationEnum.Done.ToString())
            {
                PromotionsRecordsVM.PromotionRecordID = PromotionRecordID;
            }
            else if (result.EnumMember == PromotionsRecordsValidationEnum.RejectedBecausePromotionRecordStatusMustBeOpen.ToString())
            {
                throw new CustomException(@Resources.Globalization.ValidationPromotionRecordStatusMustBeOpenText.ToString());
            }


            return Json(new
            {
                PromotionRecordID = PromotionsRecordsVM.PromotionRecordID,
            },
                JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private PromotionsRecordsViewModel GetByPromotionRecordID(int id)
        {
            PromotionsRecordsBLL PromotionRecordBLL = new PromotionsRecordsBLL().GetByPromotionRecordID(id);
            PromotionsRecordsViewModel PromotionRecordVM = new PromotionsRecordsViewModel();
            if (PromotionRecordBLL != null)
            {
                PromotionRecordVM.PromotionRecordID = PromotionRecordBLL.PromotionRecordID;
                PromotionRecordVM.PromotionRecordDate = PromotionRecordBLL.PromotionRecordDate.Date;
                PromotionRecordVM.PromotionRecordNo = PromotionRecordBLL.PromotionRecordNo;
                PromotionRecordVM.JobCategoryID = PromotionRecordBLL.JobCategory.JobCategoryID;
                PromotionRecordVM.RankID = PromotionRecordBLL.Rank.RankID;
                PromotionRecordVM.RankName = PromotionRecordBLL.Rank.RankName;
                PromotionRecordVM.YearID = PromotionRecordBLL.PromotionPeriod.Year.MaturityYearID;
                PromotionRecordVM.PromotionPeriodID = PromotionRecordBLL.PromotionPeriod.PromotionPeriodID;
                PromotionRecordVM.PromotionPeriod = PromotionRecordBLL.PromotionPeriod;
                PromotionRecordVM.PromotionRecordStatus = new PromotionsRecordsStatusViewModel()
                {
                    PromotionRecordStatusID = PromotionRecordBLL.PromotionRecordStatus.PromotionRecordStatusID,
                    PromotionRecordStatusName = PromotionRecordBLL.PromotionRecordStatus.PromotionRecordStatusName
                };

                PromotionRecordVM.PromotionRecordToolbarID = GetPromotionRecordToolbarID(PromotionRecordBLL.PromotionRecordStatus.PromotionRecordStatusID);
                //PromotionRecordVM.CreatedDate = PromotionRecordBLL.CreatedDate;
                //PromotionRecordVM.CreatedBy = PromotionRecordVM.GetCreatedByDisplayed(PromotionRecordBLL.CreatedBy);
            }
            return PromotionRecordVM;
        }

        [NonAction]
        private int GetPromotionRecordToolbarID(int PromotionRecordStatusID)
        {
            if (PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Open)
                return (int)PromotionsRecordsToolbarEnum.Preference;
            else if (PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Preferenced)
                return (int)PromotionsRecordsToolbarEnum.Install;
            else if (PromotionRecordStatusID == (int)PromotionsRecordsStatusEnum.Installed)
                return (int)PromotionsRecordsToolbarEnum.Close;
            else
                return (int)PromotionsRecordsToolbarEnum.Closed;
        }

        [NonAction]
        private string GetPromotionDecisionSupportTitle(PromotionsDecisionsBLL promotionsDecisionsBLL)
        {
            string title = "";
            if (promotionsDecisionsBLL != null && promotionsDecisionsBLL.PromotionDecisionID > 0)
            {
                switch ((PromotionsDecisionsEnum)promotionsDecisionsBLL.PromotionDecisionID)
                {
                    case PromotionsDecisionsEnum.ByTotalPoints:
                        break;
                    case PromotionsDecisionsEnum.ByLastQualifications:
                        break;
                    case PromotionsDecisionsEnum.ByCurrentJobSeniority:
                        title = @Resources.Globalization.CurrentJobSeniorityPeriodText;
                        break;
                    case PromotionsDecisionsEnum.ByLastJobSeniority:
                        title = @Resources.Globalization.LastJobSeniorityPeriodText;
                        break;
                    case PromotionsDecisionsEnum.ByTotalExperience:
                        title = @Resources.Globalization.TotalExperienceText;
                        break;
                    case PromotionsDecisionsEnum.ByExam:
                        break;
                    default:
                        break;
                }
            }

            return title;
        }

        [NonAction]
        private string GetPromotionDecisionSupportValue(PromotionsRecordsEmployeesBLL p)
        {
            string value = "";
            if (p.PromotionDecision != null && p.PromotionDecision.PromotionDecisionID > 0)
            {
                switch ((PromotionsDecisionsEnum)p.PromotionDecision.PromotionDecisionID)
                {
                    case PromotionsDecisionsEnum.ByTotalPoints:
                        break;
                    case PromotionsDecisionsEnum.ByLastQualifications:
                        break;
                    case PromotionsDecisionsEnum.ByCurrentJobSeniority:
                        value = Globals.Utilities.GetYearMonthDaysFromTotalDays(p.CurrentJobSeniorityDays, p.DaysCountInUmAlquraYear, p.DaysCountInUmAlquraMonth);
                        break;
                    case PromotionsDecisionsEnum.ByLastJobSeniority:
                        value = Globals.Utilities.GetYearMonthDaysFromTotalDays(p.LastJobSeniorityDays, p.DaysCountInUmAlquraYear, p.DaysCountInUmAlquraMonth);
                        break;
                    case PromotionsDecisionsEnum.ByTotalExperience:
                        value = Globals.Utilities.GetYearMonthDaysFromTotalDays(p.TotalExperience, p.DaysCountInUmAlquraYear, p.DaysCountInUmAlquraMonth);
                        break;
                    case PromotionsDecisionsEnum.ByExam:
                        break;
                    default:
                        break;
                }
            }
            return value;
        }
    }
}