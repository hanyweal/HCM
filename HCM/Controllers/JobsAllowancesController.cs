using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class JobsAllowancesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        //
        // GET: /JobsAllowances/
        [HttpGet]
        public JsonResult GetJobsAllowances()
        {
            return Json(new { data = new JobsAllowancesBLL().GetJobsAllowances() }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /JobsAllowances/
        [HttpGet]
        public JsonResult GetJobsAllowancesByAllowanceID(int id)
        {
            return Json(new { data = new JobsAllowancesBLL().GetByAllowanceID(id) }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["JobAllowanceID"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByJobAllowanceID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("Allowance,Job,IsActive,AllowanceID,JobID")]
        public ActionResult Delete(JobsAllowancesViewModel JobAllowanceVM)
        {
            JobsAllowancesBLL jobsAllowancesBLL = new JobsAllowancesBLL();
            jobsAllowancesBLL.LoginIdentity = UserIdentity;
            jobsAllowancesBLL.Remove(JobAllowanceVM.JobAllowanceID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        //[IgnoreModelProperties("JobAllowanceDetailRequest")]
        public ActionResult Create(JobsAllowancesViewModel JobAllowanceVM)
        {
            JobsAllowancesBLL JobAllowance = new JobsAllowancesBLL();
            JobAllowance.Allowance = new AllowancesBLL() { AllowanceID = JobAllowanceVM.AllowanceID };
            JobAllowance.Job = new JobsBLL() { JobID = JobAllowanceVM.JobID };
            JobAllowance.LoginIdentity = this.UserIdentity;
            Result result = JobAllowance.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsAllowancesBLL JobAllowanceEntity = (JobsAllowancesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["JobAllowanceID"] = ((JobsAllowancesBLL)result.Entity).JobAllowanceID;
                }
            }

            return View(JobAllowanceVM);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByJobAllowanceID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditJobAllowance(JobsAllowancesViewModel JobAllowanceVM)
        {
            JobsAllowancesBLL JobAllowance = new JobsAllowancesBLL();
            JobAllowance.JobAllowanceID = JobAllowanceVM.JobAllowanceID;
            JobAllowance.Allowance = new AllowancesBLL() { AllowanceID = JobAllowanceVM.AllowanceID };
            JobAllowance.Job = new JobsBLL() { JobID = JobAllowanceVM.JobID };
            JobAllowance.IsActive = JobAllowanceVM.IsActive;
            JobAllowance.LoginIdentity = this.UserIdentity;
            Result result = JobAllowance.Update();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                JobsAllowancesBLL JobAllowanceEntity = (JobsAllowancesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["JobAllowanceID"] = ((JobsAllowancesBLL)result.Entity).JobAllowanceID;
                }
            }

            return View(this.GetByJobAllowanceID(JobAllowanceVM.JobAllowanceID));
        }

        [HttpGet]
        public JsonResult GetJobAllowanceID()
        {
            int? JobAllowanceID = Session["JobAllowanceID"] == null ? (int?)null : int.Parse(Session["JobAllowanceID"].ToString());
            return Json(new { data = JobAllowanceID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private JobsAllowancesViewModel GetByJobAllowanceID(int id)
        {
            JobsAllowancesBLL JobsAllowancesBLL = (new JobsAllowancesBLL()).GetByJobAllowanceID(id);
            JobsAllowancesViewModel JobAllowanceVM = new JobsAllowancesViewModel();
            if (JobsAllowancesBLL != null)
            {
                JobAllowanceVM.JobAllowanceID = JobsAllowancesBLL.JobAllowanceID;
                JobAllowanceVM.Allowance = JobsAllowancesBLL.Allowance;
                JobAllowanceVM.Job = JobsAllowancesBLL.Job;
                JobAllowanceVM.IsActive = JobsAllowancesBLL.IsActive;
            }
            return JobAllowanceVM;
        }
    }
}