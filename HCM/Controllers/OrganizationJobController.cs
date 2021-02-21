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
using System.Net;
using System.Net.Http;
using System.Web.Mvc;


namespace HCM.Controllers
{
    public class OrganizationJobController : BaseController
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
                { "JobNo", Resources.Globalization.JobNoText },
                { "OrganizationName", Resources.Globalization.OrganizationNameText },
                { "JobName", Resources.Globalization.JobNameText },
                { "RankName", Resources.Globalization.RankNameText },
                { "JobCode", Resources.Globalization.JobCodeText },
                { "JobCategoryName", Resources.Globalization.JobCategoryNameText },
                { "IsVacant", Resources.Globalization.IsVacantText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetAllOrganizationsJobs(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllOrganizationsJobs()
        {
            OrganizationsJobsBLL OrganizationsJobs = new OrganizationsJobsBLL();

            var data = OrganizationsJobs.GetAllOrganizationsJobs().Select(c => new
            {
                JobName = c.Job.JobName,
                OrganizationName = c.OrganizationStructure.OrganizationName,
                RankName = c.Rank.RankName,
                JobNo = c.JobNo,
                JobCode = c.Job.JobCode,
                JobCategoryName = c.Job.JobCategory.JobCategoryName,
                IsVacant = c.IsVacant
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("OrganizationJob/GetOrganizationsJobs/{JobNo}/{OrganizationName}/{JobName}/{RankName}/{JobCode}/{JobCategoryName}")]
        public ActionResult GetOrganizationsJobs(string JobNo, string OrganizationName, string JobName, string RankName, string JobCode, string JobCategoryName)
        {
            OrganizationsJobsBLL OrganizationsJobs = new OrganizationsJobsBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            };

            var data = OrganizationsJobs.GetOrganizationsJobs(JobNo, OrganizationName, JobName, RankName, JobCode, JobCategoryName, out TotalRecordsOut, out RecFilterOut)
                .Select(c => new
                {
                    JobName = c.Job.JobName,
                    OrganizationName = c.OrganizationStructure.OrganizationName,
                    RankName = c.Rank.RankName,
                    JobNo = c.JobNo,
                    OrganizationJobID = c.OrganizationJobID,
                    JobCode = c.Job.JobCode,
                    JobCategoryName = c.Job.JobCategory.JobCategoryName,
                    IsVacant = c.IsVacant,
                    IsActive = c.IsActive
                });
            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        //[CustomAuthorize]
        //public ActionResult Delete(int id)
        //{
        //    return View(this.GetByAssigningID(id));
        //}

        //[CustomAuthorize]
        //public ActionResult Details(int id)
        //{
        //    return View(this.GetByAssigningID(id));
        //}

        //[HttpDelete]
        //[IgnoreModelProperties("EmployeeCodeNo,AssginingStartDate,AssginingEndDate,ExternalCity,ExternalOrganization,OrganizationName,JobName")]
        //public ActionResult Delete(AssigningsViewModel AssigningVM)
        //{
        //    BaseAssigningsBLL.Remove(AssigningVM.AssigningID);
        //    return View(AssigningVM);
        //}

        [HttpPost]
        public ActionResult Create(OrganizationsJobsViewModel OrganizationsJobsVM)
        {

            OrganizationsJobsBLL organizationJob = new OrganizationsJobsBLL();
            organizationJob.Rank = new RanksBLL() { RankID = (int)OrganizationsJobsVM.RankID };
            organizationJob.OrganizationStructure = new OrganizationsStructuresBLL() { OrganizationID = OrganizationsJobsVM.OrganizationID };
            organizationJob.Job = new JobsBLL() { JobID = OrganizationsJobsVM.JobID };
            organizationJob.JobNo = OrganizationsJobsVM.JobNo;
            organizationJob.LoginIdentity = UserIdentity;
            organizationJob.OrganizationJobStatus = new OrganizationsJobsStatusBLL() { OrganizationJobStatusID = OrganizationsJobsVM.OrganizationJobStatusID };
            Result result = organizationJob.Creation();
            if ((System.Type)result.EnumType == typeof(OrganizationJobValidationEnum))
            {
                if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOfExistsActiveJobWithJobNoAndRankID.ToString())
                    throw new CustomException(Resources.Globalization.ValidationJobOrganizationAlreadyExistsText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationThereIsNoActiveJobOrganizationText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoEmployeeCareerHistoryRelatedToThisOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationNoEmployeeCareerHistoryRelatedToThisOrganizationJobText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThisOrganizationJobIsNotVacant.ToString())
                    throw new CustomException(Resources.Globalization.ValidationIsNotVacantText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldBiggerThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldBiggerThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldLessThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldLessThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOperationDateLessThanOtherExists.ToString())
                    throw new CustomException(Resources.Globalization.ValidationRejectedBecauseOperationDateLessThanOtherExistsText);
            }


            return View(OrganizationsJobsVM);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByOrganizationJobID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("JobOperationDate")]
        public ActionResult EditOrganizationJob(OrganizationsJobsViewModel OrganizationsJobsVM)
        {
            OrganizationsJobsBLL organizationJob = new OrganizationsJobsBLL();
            organizationJob.OrganizationJobID = OrganizationsJobsVM.OrganizationJobID;
            organizationJob.Rank = new RanksBLL() { RankID = (int)OrganizationsJobsVM.RankID };
            organizationJob.OrganizationStructure = new OrganizationsStructuresBLL() { OrganizationID = OrganizationsJobsVM.OrganizationID };
            organizationJob.Job = new JobsBLL() { JobID = OrganizationsJobsVM.JobID };
            organizationJob.JobNo = OrganizationsJobsVM.JobNo;
            organizationJob.OrganizationJobStatus = new OrganizationsJobsStatusBLL() { OrganizationJobStatusID = OrganizationsJobsVM.OrganizationJobStatusID };
            organizationJob.LoginIdentity = UserIdentity;
            organizationJob.IsVacant = OrganizationsJobsVM.IsVacant;
            Result result = organizationJob.Modification();
         
            if ((System.Type)result.EnumType == typeof(OrganizationJobValidationEnum))
            {
                if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOfExistsActiveJobWithJobNoAndRankID.ToString())
                    throw new CustomException(Resources.Globalization.ValidationJobOrganizationAlreadyExistsText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationThereIsNoActiveJobOrganizationText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoEmployeeCareerHistoryRelatedToThisOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationNoEmployeeCareerHistoryRelatedToThisOrganizationJobText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThisOrganizationJobIsNotVacant.ToString())
                    throw new CustomException(Resources.Globalization.ValidationIsNotVacantText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldBiggerThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldBiggerThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldLessThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldLessThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOfExistsInPromotionsRecordsJobsVacancies.ToString())
                    throw new CustomException(Resources.Globalization.ValidationExistInPromotionsRecordsJobsVacanciesText);
            }

            return View(this.GetByOrganizationJobID(OrganizationsJobsVM.OrganizationJobID));
            //return View(AssigningVM);
        }


        [CustomAuthorize]
        public ActionResult JobOperation(int id)
        {
            return View(this.GetByOrganizationJobID(id));
        }

        //[HttpPost]
        //[ActionName("JobOperation")]
        //public ActionResult JobOperation(OrganizationsJobsViewModel OrganizationsJobsVM)
        //{
        //    OrganizationsJobsBLL organizationJob = new OrganizationsJobsBLL();
        //    organizationJob.OrganizationJobID = OrganizationsJobsVM.OrganizationJobID;
        //    organizationJob.Rank = new RanksBLL() { RankID = (int)OrganizationsJobsVM.RankID };
        //    organizationJob.OrganizationStructure = new OrganizationsStructuresBLL() { OrganizationID = OrganizationsJobsVM.OrganizationID };
        //    organizationJob.Job = new JobsBLL() { JobID = OrganizationsJobsVM.JobID };
        //    organizationJob.JobNo = OrganizationsJobsVM.JobNo;
        //    organizationJob.OrganizationJobStatus = new OrganizationsJobsStatusBLL() { OrganizationJobStatusID = OrganizationsJobsVM.OrganizationJobStatusID };
        //    organizationJob.LoginIdentity = UserIdentity;
        //    organizationJob.IsVacant = OrganizationsJobsVM.IsVacant;
        //    Result result = organizationJob.Modification();
        //    if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOfExistsInPromotionsRecordsJobsVacancies.ToString())
        //    {
        //        throw new CustomException(Resources.Globalization.ValidationExistInPromotionsRecordsJobsVacanciesText);
        //    }

        //    return View(this.GetByOrganizationJobID(OrganizationsJobsVM.OrganizationJobID));
        //    //return View(AssigningVM);
        //}

        //[HttpGet]
        //public JsonResult GetAssigningID()
        //{
        //    BaseAssigningsBLL AssigningBLL = AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
        //    AssigningBLL.AssigningID = int.Parse(Session["AssigningID"].ToString());
        //    return Json(new { data = AssigningBLL }, JsonRequestBehavior.AllowGet);
        //}

        //[CustomAuthorize]
        //public ActionResult PrintAssigning(int id)
        //{
        //    //return View("~/Views/Shared/ReportViewerASPX.aspx");
        //    return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.Assignings.ToString(), id));
        //}

        //[CustomAuthorize]
        //public ActionResult PrintAllAssigningsByEmployeeCodeID(int id)
        //{
        //    //return View("~/Views/Shared/ReportViewerASPX.aspx");
        //    return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.Assignings.ToString(), id));
        //}

        [NonAction]
        private OrganizationsJobsViewModel GetByOrganizationJobID(int id)
        {
            OrganizationsJobsBLL OrganizationsJobsBLL = new HCMBLL.OrganizationsJobsBLL().GetByOrganizationJobID(id);
            OrganizationsJobsViewModel OrganizationJobVM = new OrganizationsJobsViewModel();
            OrganizationJobVM.OrganizationStructure = new OrganizationStructureViewModel()
            {
                OrganizationID = OrganizationsJobsBLL.OrganizationStructure.OrganizationID,
                OrganizationName = OrganizationsJobsBLL.OrganizationStructure.OrganizationName,
            };
            
            OrganizationJobVM.RankID = OrganizationsJobsBLL.Rank.RankID; 
            OrganizationJobVM.OrganizationJobStatusID = OrganizationsJobsBLL.OrganizationJobStatus.OrganizationJobStatusID;
            OrganizationJobVM.Rank = new RanksViewModel()
            {
                RankID = OrganizationsJobsBLL.Rank.RankID,
                RankName = OrganizationsJobsBLL.Rank.RankName,
                RankCategory=new RanksCategoriesBLL() { 
                 RankCategoryID= OrganizationsJobsBLL.Rank.RankCategory.RankCategoryID
                }
            };
            OrganizationJobVM.Job = new JobsViewModel()
            {
                JobID = OrganizationsJobsBLL.Job.JobID,
                JobName = OrganizationsJobsBLL.Job.JobName
            };
            OrganizationJobVM.JobNo = OrganizationsJobsBLL.JobNo;
            OrganizationJobVM.OrganizationJobID = OrganizationsJobsBLL.OrganizationJobID;
            OrganizationJobVM.IsVacant = OrganizationsJobsBLL.IsVacant;

            return OrganizationJobVM;
        }

        [HttpGet]
        [Route("OrganizationJob/GetJobsVacancies/{JobCategoryID}/{RankID}")]
        public JsonResult GetJobsVacancies(int JobCategoryID, int RankID)
        {
            return Json(new
            {
                data = new OrganizationsJobsBLL().GetJobsVacancies(JobCategoryID, RankID).Select(item => new
                {
                    OrganizationJobID = item.OrganizationJobID,
                    JobGeneralGroupName = item.Job.JobCategory.JobGroup.JobGeneralGroup.JobGeneralGroupName,
                    JobGroupName = item.Job.JobCategory.JobGroup.JobGroupName,
                    JobCategoryName = item.Job.JobCategory.JobCategoryName,
                    JobCode = item.Job.JobCode,
                    JobName = item.Job.JobName,
                    JobNo = item.JobNo,
                    RankName = item.Rank.RankName,
                    OrganizationJobStatusName = item.OrganizationJobStatus.OrganizationJobStatusName,
                    OrganizationName = item.OrganizationStructure.OrganizationName,
                    IsReserved = item.IsReserved
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetEmployeesOfOrganizationJob/{OrganizationJobID}")]
        public JsonResult GetEmployeesOfOrganizationJob(int OrganizationJobID)
        {
            List<EmployeesCareersHistoryBLL> EmployeesCareersHistoryBLLList = new EmployeesCareersHistoryBLL().GetEmployeesByOrganizationJobID(OrganizationJobID);

            var data = EmployeesCareersHistoryBLLList.Select(x => new
            {
                EmployeeCodeNo = x.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.EmployeeCode.Employee.EmployeeNameAr,
                CareerDegreeName = x.CareerDegree.CareerDegreeName,
                JoinDate = x.JoinDate,
                JobName = x.OrganizationJob.Job.JobName,
                RankName = x.OrganizationJob.Rank.RankName,
                JobNo = x.OrganizationJob.JobNo,
                OrganizationName = x.OrganizationJob.OrganizationStructure.OrganizationName,
                IsActive = x.IsActive,
                CareerHistoryTypeName= x.CareerHistoryType.CareerHistoryTypeName
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetOrganizationJobHistory/{OrganizationJobID}")]
        public JsonResult GetOrganizationJobHistory(int OrganizationJobID)
        {
            List<OrganizationsJobsBLL> OrganizationsJobsBLLList = new OrganizationsJobsBLL().GetOrganizationJobHistoryOperationByOrganizationJobID(OrganizationJobID);

            var data = OrganizationsJobsBLLList.Select(x => new
            {
                JobNo = x.JobNo,
                OrganizationName = x.OrganizationStructure.OrganizationName,
                JobName = x.Job.JobName,
                RankName = x.Rank.RankName,
                JobOperationTypeName = x.JobOperationType.JobOperationTypeName,
                JobOperationDate = x.JobOperationDate,
                IsActive = x.IsActive,
                
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        #region JobOperation
        public HttpResponseMessage SaveModulation(JobOperationModulationViewModel jobOperationModulationVM)
        {
            Result result = null;
            OrganizationsJobsBLL modulationMrganizationJob = new OrganizationsJobsBLL().GetByOrganizationJobID(jobOperationModulationVM.OrganizationJobID);
            modulationMrganizationJob.OrganizationStructure = new OrganizationsStructuresBLL() { OrganizationID = jobOperationModulationVM.OrganizationID };
            modulationMrganizationJob.Job = new JobsBLL() { JobID = jobOperationModulationVM.JobID };
            modulationMrganizationJob.JobOperationDate = jobOperationModulationVM.JobOperationDate;
            modulationMrganizationJob.LoginIdentity = UserIdentity;
            result = modulationMrganizationJob.Modulation();
            if ((System.Type)result.EnumType == typeof(OrganizationJobValidationEnum))
            {
                if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOfExistsActiveJobWithJobNoAndRankID.ToString())
                    throw new CustomException(Resources.Globalization.ValidationJobOrganizationAlreadyExistsText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationThereIsNoActiveJobOrganizationText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoEmployeeCareerHistoryRelatedToThisOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationNoEmployeeCareerHistoryRelatedToThisOrganizationJobText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThisOrganizationJobIsNotVacant.ToString())
                    throw new CustomException(Resources.Globalization.ValidationIsNotVacantText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldBiggerThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldBiggerThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldLessThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldLessThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOperationDateLessThanOtherExists.ToString())
                    throw new CustomException(Resources.Globalization.ValidationRejectedBecauseOperationDateLessThanOtherExistsText);

            }

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
        public HttpResponseMessage SaveScalingDown(JobOperationScalingDownViewModel jobOperationScalingDownVM)
        {
            Result result = null;
            OrganizationsJobsBLL scalingDownOrganizationJob = new OrganizationsJobsBLL().GetByOrganizationJobID(jobOperationScalingDownVM.OrganizationJobID);
            scalingDownOrganizationJob.Rank = new RanksBLL() { RankID = jobOperationScalingDownVM.RankID };
            scalingDownOrganizationJob.JobNo = jobOperationScalingDownVM.JobNo;
            scalingDownOrganizationJob.JobOperationDate = jobOperationScalingDownVM.JobOperationDate;
            scalingDownOrganizationJob.LoginIdentity = UserIdentity;
            result = scalingDownOrganizationJob.ScalingDown();
            if ((System.Type)result.EnumType == typeof(OrganizationJobValidationEnum))
            {
                if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOfExistsActiveJobWithJobNoAndRankID.ToString())
                    throw new CustomException(Resources.Globalization.ValidationJobOrganizationAlreadyExistsText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationThereIsNoActiveJobOrganizationText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoEmployeeCareerHistoryRelatedToThisOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationNoEmployeeCareerHistoryRelatedToThisOrganizationJobText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThisOrganizationJobIsNotVacant.ToString())
                    throw new CustomException(Resources.Globalization.ValidationIsNotVacantText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldBiggerThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldBiggerThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldLessThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldLessThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOperationDateLessThanOtherExists.ToString())
                    throw new CustomException(Resources.Globalization.ValidationRejectedBecauseOperationDateLessThanOtherExistsText);

            }

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
        public HttpResponseMessage SavePushingUp(JobOperationPushingUpViewModel jobOperationPushingUpVM)
        {
            Result result = null;
            OrganizationsJobsBLL pushingUpOrganizationJob = new OrganizationsJobsBLL().GetByOrganizationJobID(jobOperationPushingUpVM.OrganizationJobID);
            pushingUpOrganizationJob.Rank = new RanksBLL() { RankID = jobOperationPushingUpVM.RankID };
            pushingUpOrganizationJob.JobNo = jobOperationPushingUpVM.JobNo;
            pushingUpOrganizationJob.JobOperationDate = jobOperationPushingUpVM.JobOperationDate;
            pushingUpOrganizationJob.LoginIdentity = UserIdentity;
            result = pushingUpOrganizationJob.PushingUp();
            if ((System.Type)result.EnumType == typeof(OrganizationJobValidationEnum))
            {
                if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOfExistsActiveJobWithJobNoAndRankID.ToString())
                    throw new CustomException(Resources.Globalization.ValidationJobOrganizationAlreadyExistsText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationThereIsNoActiveJobOrganizationText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoEmployeeCareerHistoryRelatedToThisOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationNoEmployeeCareerHistoryRelatedToThisOrganizationJobText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThisOrganizationJobIsNotVacant.ToString())
                    throw new CustomException(Resources.Globalization.ValidationIsNotVacantText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldBiggerThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldBiggerThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldLessThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldLessThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOperationDateLessThanOtherExists.ToString())
                    throw new CustomException(Resources.Globalization.ValidationRejectedBecauseOperationDateLessThanOtherExistsText);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
        public HttpResponseMessage SaveCancelation(JobOperationCancelationViewModel jobOperationCancelationVM)
        {
            Result result = null;
            OrganizationsJobsBLL pushingUpOrganizationJob = new OrganizationsJobsBLL().GetByOrganizationJobID(jobOperationCancelationVM.OrganizationJobID);
            pushingUpOrganizationJob.JobOperationDate = jobOperationCancelationVM.JobOperationDate;
            pushingUpOrganizationJob.LoginIdentity = UserIdentity;
            result = pushingUpOrganizationJob.Cancelation();
            if ((System.Type)result.EnumType == typeof(OrganizationJobValidationEnum))
            {
                if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOfExistsActiveJobWithJobNoAndRankID.ToString())
                    throw new CustomException(Resources.Globalization.ValidationJobOrganizationAlreadyExistsText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoActiveOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationThereIsNoActiveJobOrganizationText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThereIsNoEmployeeCareerHistoryRelatedToThisOrganizationJob.ToString())
                    throw new CustomException(Resources.Globalization.ValidationNoEmployeeCareerHistoryRelatedToThisOrganizationJobText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseThisOrganizationJobIsNotVacant.ToString())
                    throw new CustomException(Resources.Globalization.ValidationIsNotVacantText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldBiggerThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldBiggerThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseInPushingUpNextRankShouldLessThanCurrent.ToString())
                    throw new CustomException(Resources.Globalization.ValidationPushingUpNextRankShouldLessThanCurrentText);
                else if (result.EnumMember == OrganizationJobValidationEnum.RejectedBecauseOperationDateLessThanOtherExists.ToString())
                    throw new CustomException(Resources.Globalization.ValidationRejectedBecauseOperationDateLessThanOtherExistsText);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);

        }
        #endregion
    }
}