using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class JobsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        #region Jobs
        public ActionResult GetJobs()
        {
            return Json(new { data = new JobsBLL().GetJobs() }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetJobsToPlacement()
        {
            return Json(new { data = new JobsBLL().GetJobsToPlacement() }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetJobsByJobID(int id)
        {
            return Json(new { data = new JobsBLL().GetByJobID(id) }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(JobsViewModel JobVM)
        {
            JobsBLL JobBLL = new JobsBLL()
            {
                JobCategory = new JobsCategoriesBLL() { JobCategoryID = (int)JobVM.JobCategoryID },
                JobName = JobVM.JobName,
                JobCode = JobVM.JobCode,
                LoginIdentity = UserIdentity
            };
            Result result = JobBLL.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsBLL JobEntity = (JobsBLL)result.Entity;
                if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
                {
                    JobVM.JobID = ((JobsBLL)result.Entity).JobID;
                }
            }
            return Json(new { JobID = JobVM.JobID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByJobID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditJob(JobsViewModel JobVM)
        {
            JobsBLL Job = new JobsBLL();
            Job.JobID = JobVM.JobID.Value;
            Job.JobCategory = new JobsCategoriesBLL() { JobCategoryID = (int)JobVM.JobCategoryID };
            Job.JobName = JobVM.JobName;
            Job.JobCode = JobVM.JobCode;
            Job.LoginIdentity = UserIdentity;
            Result result = Job.Update();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsBLL JobAllowanceEntity = (JobsBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["JobID"] = ((JobsBLL)result.Entity).JobID;
                }
            }
            return View(this.GetByJobID(JobVM.JobID.Value));
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByJobID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("JobName,JobCategoryID,JobCode")]
        public ActionResult Delete(JobsViewModel JobVM)
        {
            JobsBLL jobBLL = new JobsBLL();
            jobBLL.LoginIdentity = UserIdentity;
            jobBLL.Remove(JobVM.JobID.Value);
            return RedirectToAction("Index");
        }

        [NonAction]
        private JobsViewModel GetByJobID(int id)
        {
            JobsBLL JobBLL = new JobsBLL();
            JobBLL = JobBLL.GetByJobID(id);
            JobsViewModel JobVM = new JobsViewModel();
            if (JobBLL != null)
            {
                JobVM.JobID = JobBLL.JobID;
                JobVM.JobCode = JobBLL.JobCode;
                JobVM.JobName = JobBLL.JobName;
                JobVM.JobCategoryID = JobBLL.JobCategory.JobCategoryID;
                JobVM.JobCategoryName = "" + JobBLL.JobCategory.JobCategoryName;
                JobVM.JobGroup = JobBLL.JobCategory.JobGroup;
                JobVM.JobGeneralGroup = JobBLL.JobCategory.JobGroup.JobGeneralGroup;
            }
            return JobVM;
        }
        #endregion

        #region JobsCategories
        public ActionResult GetJobsCategories()
        {
            return Json(new { data = new JobsCategoriesBLL().GetJobsCategories() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobsCategoriesByJobID(int id)
        {
            return Json(new { data = new JobsCategoriesBLL().GetByJobCategoryID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetJobsCategoriesByJobGroupID(int id)
        {
            //int KSARegionID = 1;
            return Json(new { data = new JobsCategoriesBLL().GetByJobGroupID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetJobCategoryByJobCategoryID(int id)
        {
            //int KSARegionID = 1;
            return Json(new { data = new JobsCategoriesBLL().GetByJobCategoryID(id) }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult CreateJobCategory()
        {
            return View();
        }

        [HttpPost]
        [IgnoreModelProperties("QualificationDegree,Qualification,GeneralSpecialization,PromotionPeriodID,AssignedJobCategoryJobCategoryID")]
        public ActionResult CreateJobCategory(JobsCategoriesViewModel JobCategoryVM)
        {
            JobsCategoriesBLL JobsCategoriesBLL = new JobsCategoriesBLL()
            {
                JobGroup = new JobsGroupsBLL() { JobGroupID = JobCategoryVM.JobGroupID },
                JobCategoryName = JobCategoryVM.JobCategoryName,
                JobCategoryNo = JobCategoryVM.JobCategoryNo,
                LoginIdentity = UserIdentity

            };

            //if (JobCategoryVM.GeneralSpecialization.GeneralSpecializationID != 0)
            //    JobsCategoriesBLL.MinGeneralSpecialization = JobCategoryVM.GeneralSpecialization;

            //if (JobCategoryVM.Qualification.QualificationID != 0)
            //    JobsCategoriesBLL.MinQualification = JobCategoryVM.Qualification;

            //if (JobCategoryVM.QualificationDegree.QualificationDegreeID != 0)
            //    JobsCategoriesBLL.MinQualificationDegree = JobCategoryVM.QualificationDegree;

            Result result = JobsCategoriesBLL.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsCategoriesBLL JobEntity = (JobsCategoriesBLL)result.Entity;
                if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
                {
                    JobCategoryVM.JobCategoryID = ((JobsCategoriesBLL)result.Entity).JobCategoryID;
                }

            }

            return Json(new { JobCategoryID = JobCategoryVM.JobCategoryID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult EditJobCategory(int id)
        {
            return View(this.GetByJobCategoryID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("QualificationDegree,Qualification,GeneralSpecialization,PromotionPeriodID,AssignedJobCategoryJobCategoryID")]
        public ActionResult EditJobCategory(JobsCategoriesViewModel JobCategoryVM)
        {
            JobsCategoriesBLL JobsCategoriesBLL = new JobsCategoriesBLL();
            JobsCategoriesBLL.JobCategoryID = JobCategoryVM.JobCategoryID.Value;
            JobsCategoriesBLL.JobGroup = new JobsGroupsBLL() { JobGroupID = JobCategoryVM.JobGroupID };
            JobsCategoriesBLL.JobCategoryName = JobCategoryVM.JobCategoryName;
            JobsCategoriesBLL.JobCategoryNo = JobCategoryVM.JobCategoryNo;
            //if (JobCategoryVM.GeneralSpecialization.GeneralSpecializationID != 0)
            //    JobsCategoriesBLL.MinGeneralSpecialization = JobCategoryVM.GeneralSpecialization;

            //if (JobCategoryVM.Qualification.QualificationID != 0)
            //    JobsCategoriesBLL.MinQualification = JobCategoryVM.Qualification;

            //if (JobCategoryVM.QualificationDegree.QualificationDegreeID != 0)
            //    JobsCategoriesBLL.MinQualificationDegree = JobCategoryVM.QualificationDegree;
            JobsCategoriesBLL.LoginIdentity = UserIdentity;
            Result result = JobsCategoriesBLL.Update();


            if ((System.Type)result.EnumType == typeof(JobsCategoriesValidationEnum))
            {
                if (result.EnumMember == JobsCategoriesValidationEnum.RejectedBecauseOfItHasPromotionRecord.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationJobCategoryHasPromotionRecordText);
                }
            }
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsCategoriesBLL JobAllowanceEntity = (JobsCategoriesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["JobCategoryID"] = ((JobsCategoriesBLL)result.Entity).JobCategoryID;
                }
            }

            return View(this.GetByJobCategoryID(JobCategoryVM.JobCategoryID.Value));
        }

        [CustomAuthorize]
        public ActionResult AddJobCategoryQualification(int id)
        {
            return View(this.GetByJobCategoryID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("QualificationDegree,Qualification,GeneralSpecialization,JobCategoryName,JobGroupID,AssignedJobCategoryJobCategoryID,JobCategoryNo")]
        public HttpResponseMessage AddJobCategoryQualification(JobsCategoriesViewModel JobCategoryVM)
        {
            //List<JobsCategoriesQualificationsBLL> JobsCategoriesQualificationsList = GetJobsCategoriesQualificationsFromSession();
            JobsCategoriesQualificationsBLL JobCategoryQualification = new JobsCategoriesQualificationsBLL()
            {
                JobCategory = new JobsCategoriesBLL() { JobCategoryID = JobCategoryVM.JobCategoryQualification.JobCategory.JobCategoryID },
                QualificationDegree = new QualificationsDegreesBLL() { QualificationDegreeID = JobCategoryVM.JobCategoryQualification.QualificationDegree.QualificationDegreeID },
                IsMinQualification = JobCategoryVM.JobCategoryQualification.IsMinQualification,
                PromotionPeriod = new PromotionsPeriodsBLL() { PromotionPeriodID = (int)JobCategoryVM.PromotionPeriodID },
                LoginIdentity = UserIdentity
            };
            if (JobCategoryVM.JobCategoryQualification.Qualification != null)
            {
                JobCategoryQualification.Qualification = new QualificationsBLL() { QualificationID = JobCategoryVM.JobCategoryQualification.Qualification.QualificationID };
            }
            if (JobCategoryVM.JobCategoryQualification.GeneralSpecialization != null)
            {
                JobCategoryQualification.GeneralSpecialization = new GeneralSpecializationsBLL() { GeneralSpecializationID = JobCategoryVM.JobCategoryQualification.GeneralSpecialization.GeneralSpecializationID };
            }

            //if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.EmployeeCode.EmployeeCodeNo))
            //{
            //    throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
            //}
            //else 
            //if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity))
            //{
            //    throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityRequired);
            //}
            //else if (PassengerOrdersItinerariesList.FindIndex(e => e.FromCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity.ToUpper())
            //                                                    && e.ToCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity.ToUpper())) > -1)
            //{
            //    throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityAlreadyExist);
            //}

            Result result = JobCategoryQualification.Add();
            if ((System.Type)result.EnumType == typeof(JobsCategoriesValidationEnum))
            {
                if (result.EnumMember == JobsCategoriesValidationEnum.RejectedBecauseOfItHasPromotionRecord.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationJobCategoryHasPromotionRecordText);
                }
            }
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsCategoriesQualificationsBLL JobEntity = (JobsCategoriesQualificationsBLL)result.Entity;
                if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
                {
                    JobCategoryVM.JobCategoryQualification.JobCategoryQualificationID = ((JobsCategoriesQualificationsBLL)result.Entity).JobCategoryQualificationID;
                }

            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [CustomAuthorize]
        public ActionResult AddPromotionJobCategory(int id)
        {
            return View(this.GetByJobCategoryID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("QualificationDegree,Qualification,GeneralSpecialization,JobCategoryName,JobGroupID,JobCategoryNo")]
        public HttpResponseMessage AddPromotionJobCategory(JobsCategoriesViewModel JobCategoryVM)
        {
            PromotionsJobsCategoriesBLL PromotionJobCategory = new PromotionsJobsCategoriesBLL()
            {
                JobCategory = new JobsCategoriesBLL() { JobCategoryID = JobCategoryVM.PromotionJobCategory.JobCategory.JobCategoryID },
                PromotionPeriod = new PromotionsPeriodsBLL() { PromotionPeriodID = (int)JobCategoryVM.PromotionPeriodID },
                AssignedJobCategory = new JobsCategoriesBLL() { JobCategoryID = (int)JobCategoryVM.AssignedJobCategoryJobCategoryID },
                LoginIdentity = UserIdentity
            };
            Result result = PromotionJobCategory.Add();
            if ((System.Type)result.EnumType == typeof(JobsCategoriesValidationEnum))
            {
                if (result.EnumMember == JobsCategoriesValidationEnum.RejectedBecauseOfItHasPromotionRecord.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationJobCategoryHasPromotionRecordText);
                }
            }
            if ((System.Type)result.EnumType == typeof(PromotionsJobsCategoriesValidationEnum))
            {
                PromotionsJobsCategoriesBLL JobEntity = (PromotionsJobsCategoriesBLL)result.Entity;
                if (result.EnumMember == PromotionsJobsCategoriesValidationEnum.RejectedBecauseAssignedJobCategorySameJobCategory.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationAssignedJobCategorySameJobCategoryText);
                }

            }
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                PromotionsJobsCategoriesBLL JobEntity = (PromotionsJobsCategoriesBLL)result.Entity;
                if (result.EnumMember == PromotionsJobsCategoriesValidationEnum.Done.ToString())
                {
                    JobCategoryVM.PromotionJobCategory.PromotionJobCategoryID = ((PromotionsJobsCategoriesBLL)result.Entity).PromotionJobCategoryID;
                }

            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private List<JobsCategoriesQualificationsBLL> GetJobsCategoriesQualificationsFromSession()
        {
            List<JobsCategoriesQualificationsBLL> JobsCategoriesQualificationsList;
            if (Session["JobsCategoriesQualifications"] != null)
                JobsCategoriesQualificationsList = (List<JobsCategoriesQualificationsBLL>)Session["JobsCategoriesQualifications"];
            else
                JobsCategoriesQualificationsList = new List<JobsCategoriesQualificationsBLL>();

            return JobsCategoriesQualificationsList;
        }

        [CustomAuthorize]
        public ActionResult DeleteJobCategory(int id)
        {
            return View(this.GetByJobCategoryID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("JobCategoryName,JobGroupID,PromotionPeriodID,AssignedJobCategoryJobCategoryID,JobCategoryNo")]
        public ActionResult DeleteJobCategory(JobsCategoriesViewModel JobCategoryVM)
        {
            JobsCategoriesBLL jobCategoryBLL = new JobsCategoriesBLL();
            jobCategoryBLL.LoginIdentity = UserIdentity;
            jobCategoryBLL.Remove(JobCategoryVM.JobCategoryID.Value);
            return RedirectToAction("Index");
        }

        [NonAction]
        private JobsCategoriesViewModel GetByJobCategoryID(int id)
        {
            JobsCategoriesBLL JobCategoryBLL = new JobsCategoriesBLL();
            JobCategoryBLL = JobCategoryBLL.GetByJobCategoryID(id);
            JobsCategoriesViewModel JobCategoryVM = new JobsCategoriesViewModel();
            if (JobCategoryBLL != null)
            {
                JobCategoryVM.JobCategoryID = JobCategoryBLL.JobCategoryID;
                JobCategoryVM.JobCategoryName = JobCategoryBLL.JobCategoryName;
                JobCategoryVM.JobGroupID = JobCategoryBLL.JobGroup.JobGroupID;
                JobCategoryVM.JobGroupName = "" + JobCategoryBLL.JobGroup.JobGroupName;
                JobCategoryVM.JobGeneralGroup = JobCategoryBLL.JobGroup.JobGeneralGroup;
                JobCategoryVM.GeneralSpecialization = JobCategoryBLL.MinGeneralSpecialization;
                JobCategoryVM.QualificationDegree = JobCategoryBLL.MinQualificationDegree;
                JobCategoryVM.Qualification = JobCategoryBLL.MinQualification;
                JobCategoryVM.JobCategoryNo = JobCategoryBLL.JobCategoryNo;
            }
            return JobCategoryVM;
        }
        #endregion

        #region JobsGroups
        public ActionResult GetJobsGroups()
        {
            return Json(new { data = new JobsGroupsBLL().GetJobsGroups() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobsGroupsByJobID(int id)
        {
            return Json(new { data = new JobsGroupsBLL().GetByJobGroupID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetJobsGroupsByJobGeneralGroupID(int id)
        {
            //int KSARegionID = 1;
            return Json(new { data = new JobsGroupsBLL().GetByJobGeneralGroupID(id) }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult CreateJobGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateJobGroup(JobsGroupsViewModel JobGroupVM)
        {
            JobsGroupsBLL JobsGroupsBLL = new JobsGroupsBLL()
            {
                JobGeneralGroup = new JobsGeneralGroupsBLL() { JobGeneralGroupID = JobGroupVM.JobGeneralGroupID },
                JobGroupName = JobGroupVM.JobGroupName,
                LoginIdentity = UserIdentity
            };
            Result result = JobsGroupsBLL.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsGroupsBLL JobEntity = (JobsGroupsBLL)result.Entity;
                if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
                {
                    JobGroupVM.JobGroupID = ((JobsGroupsBLL)result.Entity).JobGroupID;
                }

            }

            return Json(new { JobGroupID = JobGroupVM.JobGroupID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult EditJobGroup(int id)
        {
            return View(this.GetByJobGroupID(id));
        }

        [HttpPost]
        public ActionResult EditJobGroup(JobsGroupsViewModel JobGroupVM)
        {
            JobsGroupsBLL Job = new JobsGroupsBLL();
            Job.JobGroupID = JobGroupVM.JobGroupID.Value;
            Job.JobGeneralGroup = new JobsGeneralGroupsBLL() { JobGeneralGroupID = JobGroupVM.JobGeneralGroupID };
            Job.JobGroupName = JobGroupVM.JobGroupName;
            Job.LoginIdentity = UserIdentity;
            Result result = Job.Update();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsGroupsBLL JobAllowanceEntity = (JobsGroupsBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["JobGroupID"] = ((JobsGroupsBLL)result.Entity).JobGroupID;
                }
            }

            return View(this.GetByJobGroupID(JobGroupVM.JobGroupID.Value));
        }

        [CustomAuthorize]
        public ActionResult DeleteJobGroup(int id)
        {
            return View(this.GetByJobGroupID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("JobGroupName,JobGroupID")]
        public ActionResult DeleteJobGroup(JobsGroupsViewModel JobGroupVM)
        {
            JobsGroupsBLL jobGroupBLL = new JobsGroupsBLL();
            jobGroupBLL.LoginIdentity = UserIdentity;
            jobGroupBLL.Remove(JobGroupVM.JobGroupID.Value);
            return RedirectToAction("Index");
        }

        [NonAction]
        private JobsGroupsViewModel GetByJobGroupID(int id)
        {
            JobsGroupsBLL JobGroupBLL = new JobsGroupsBLL();
            JobGroupBLL = JobGroupBLL.GetByJobGroupID(id);
            JobsGroupsViewModel JobGroupVM = new JobsGroupsViewModel();
            if (JobGroupBLL != null)
            {
                JobGroupVM.JobGroupID = JobGroupBLL.JobGroupID;
                JobGroupVM.JobGroupName = JobGroupBLL.JobGroupName;
                JobGroupVM.JobGeneralGroupID = JobGroupBLL.JobGeneralGroup.JobGeneralGroupID;
                JobGroupVM.JobGeneralGroupName = "" + JobGroupBLL.JobGeneralGroup.JobGeneralGroupName;
            }
            return JobGroupVM;
        }
        #endregion

        #region JobsGeneralGroups
        public ActionResult GetJobsGeneralGroups()
        {
            return Json(new { data = new JobsGeneralGroupsBLL().GetJobsGeneralGroups() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobsGeneralGroupsByJobID(int id)
        {
            return Json(new { data = new JobsGeneralGroupsBLL().GetByJobGeneralGroupID(id) }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult CreateJobGeneralGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateJobGeneralGroup(JobsGeneralGroupsViewModel JobGeneralGroupVM)
        {
            JobsGeneralGroupsBLL JobsGeneralGroupsBLL = new JobsGeneralGroupsBLL()
            {
                JobGeneralGroupName = JobGeneralGroupVM.JobGeneralGroupName,
                LoginIdentity = UserIdentity
            };
            Result result = JobsGeneralGroupsBLL.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsGeneralGroupsBLL JobEntity = (JobsGeneralGroupsBLL)result.Entity;
                if (result.EnumMember == AllowanceValidationEnum.Done.ToString())
                {
                    JobGeneralGroupVM.JobGeneralGroupID = ((JobsGeneralGroupsBLL)result.Entity).JobGeneralGroupID;
                }

            }

            return Json(new { JobGeneralGroupID = JobGeneralGroupVM.JobGeneralGroupID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult EditJobGeneralGroup(int id)
        {
            return View(this.GetByJobGeneralGroupID(id));
        }

        [HttpPost]
        public ActionResult EditJobGeneralGroup(JobsGeneralGroupsViewModel JobGeneralGroupVM)
        {
            JobsGeneralGroupsBLL JobGeneralGroup = new JobsGeneralGroupsBLL();
            JobGeneralGroup.JobGeneralGroupID = JobGeneralGroupVM.JobGeneralGroupID.Value;
            JobGeneralGroup.JobGeneralGroupName = JobGeneralGroupVM.JobGeneralGroupName;
            JobGeneralGroup.LoginIdentity = UserIdentity;
            Result result = JobGeneralGroup.Update();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsGeneralGroupsBLL JobAllowanceEntity = (JobsGeneralGroupsBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["JobGeneralGroupID"] = ((JobsGeneralGroupsBLL)result.Entity).JobGeneralGroupID;
                }
            }

            return View(this.GetByJobGeneralGroupID(JobGeneralGroupVM.JobGeneralGroupID.Value));
        }

        [CustomAuthorize]
        public ActionResult DeleteJobGeneralGroup(int id)
        {
            return View(this.GetByJobGeneralGroupID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("JobGeneralGroupName,JobGeneralGroupID")]
        public ActionResult DeleteJobGeneralGroup(JobsGeneralGroupsViewModel JobGeneralGroupVM)
        {
            JobsGeneralGroupsBLL jobBLL = new JobsGeneralGroupsBLL();
            jobBLL.LoginIdentity = UserIdentity;
            jobBLL.Remove(JobGeneralGroupVM.JobGeneralGroupID.Value);
            return RedirectToAction("Index");
        }

        [NonAction]
        private JobsGeneralGroupsViewModel GetByJobGeneralGroupID(int id)
        {
            JobsGeneralGroupsBLL JobGeneralGroupBLL = new JobsGeneralGroupsBLL();
            JobGeneralGroupBLL = JobGeneralGroupBLL.GetByJobGeneralGroupID(id);
            JobsGeneralGroupsViewModel JobGeneralGroupVM = new JobsGeneralGroupsViewModel();
            if (JobGeneralGroupBLL != null)
            {
                JobGeneralGroupVM.JobGeneralGroupID = JobGeneralGroupBLL.JobGeneralGroupID;
                JobGeneralGroupVM.JobGeneralGroupName = JobGeneralGroupBLL.JobGeneralGroupName;
            }
            return JobGeneralGroupVM;
        }
        #endregion

        #region JobsCategoriesQualifications
        [HttpPost]
        public JsonResult GetJobsCategoriesQualificationsByJobCategoryID(int id)
        {
            List<JobsCategoriesQualificationsBLL> JobsCategoriesQualificationsList = new JobsCategoriesQualificationsBLL().GetByJobCategoryID(id);
            return Json(new { data = JobsCategoriesQualificationsList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public HttpResponseMessage RemoveJobCategorieQualification(int id)
        {
            Result result = null;
            result = new JobsCategoriesQualificationsBLL() { LoginIdentity = UserIdentity }.Remove(id);
            if ((System.Type)result.EnumType == typeof(JobsCategoriesValidationEnum))
            {
                if (result.EnumMember == JobsCategoriesValidationEnum.RejectedBecauseOfItHasPromotionRecord.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationJobCategoryHasPromotionRecordText);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        #endregion

        #region PromotionsJobsCategories
        [HttpPost]
        public JsonResult GetPromotionsJobsCategoriesByJobCategoryID(int id)
        {
            List<PromotionsJobsCategoriesBLL> PromotionsJobsCategoriesList = new PromotionsJobsCategoriesBLL().GetPromotionsJobsCategoriesByJobCategoryID(id);
            return Json(new { data = PromotionsJobsCategoriesList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public HttpResponseMessage RemovePromotionJobCategory(int id)
        {
            Result result = null;
            result = new PromotionsJobsCategoriesBLL() { LoginIdentity = UserIdentity }.Remove(id);
            if ((System.Type)result.EnumType == typeof(JobsCategoriesValidationEnum))
            {
                if (result.EnumMember == JobsCategoriesValidationEnum.RejectedBecauseOfItHasPromotionRecord.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationJobCategoryHasPromotionRecordText);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        #endregion
    }
}