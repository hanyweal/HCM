
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters; 
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;  
using System; 
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class EmployeesOldJobsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

     

        public JsonResult GetAllEmployeesOldJobs()
        {
            var data = new EmployeesOldJobsBLL().GetEmployeesOldJobs()
               .Select(x => new
               {
                   EmployeeOldJobID = x.EmployeeOldJobID,
                   EmployeeCodeNo = x.EmployeeCode.EmployeeCodeNo,
                   EmployeeNameAr = x.EmployeeCode.Employee.EmployeeNameAr,
                   JobName = x.JobName,
                   OrganizationName=x.OrganizationName,
                   RankName=x.RankName,
                   CareerDegreeName= x.CareerDegreeName,
                   EmployeeOldJobStartDate = (DateTime)x.EmployeeOldJobStartDate,
                   EmployeeOldJobEndDate = (DateTime)x.EmployeeOldJobEndDate,
               });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["EmployeeOldJobID"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByEmployeeOldJobID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByEmployeeOldJobID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("")]
        public ActionResult Delete(EmployeesOldJobsViewModel EmployeeOldJobVM)
        {
            EmployeesOldJobsBLL EmployeesOldJobsBLL = new EmployeesOldJobsBLL();
            EmployeesOldJobsBLL.LoginIdentity = UserIdentity;
            EmployeesOldJobsBLL.Remove(EmployeeOldJobVM.EmployeeOldJobID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(EmployeesOldJobsViewModel EmployeeOldJobVM)
        {
            EmployeesOldJobsBLL EmployeeOldJob = new EmployeesOldJobsBLL();
            EmployeeOldJob.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeOldJobVM.EmployeeCodeID };
            EmployeeOldJob.JobName = EmployeeOldJobVM.JobName;
            EmployeeOldJob.OrganizationName = EmployeeOldJobVM.OrganizationName;
            EmployeeOldJob.RankName = EmployeeOldJobVM.RankName;
            EmployeeOldJob.CareerDegreeName = EmployeeOldJobVM.CareerDegreeName;
            EmployeeOldJob.EmployeeOldJobStartDate = EmployeeOldJobVM.EmployeeOldJobStartDate;
            EmployeeOldJob.EmployeeOldJobEndDate = EmployeeOldJobVM.EmployeeOldJobEndDate; 
            EmployeeOldJob.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) };
            Result result = EmployeeOldJob.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                EmployeesOldJobsBLL EmployeeOldJobEntity = (EmployeesOldJobsBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                { 
                    EmployeeOldJobVM.EmployeeOldJobID = ((EmployeesOldJobsBLL)result.Entity).EmployeeOldJobID;
                } 
            } 
            return Json(new { EmployeeOldJobID = EmployeeOldJobVM.EmployeeOldJobID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByEmployeeOldJobID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditEmployeeOldJob(EmployeesOldJobsViewModel EmployeeOldJobVM)
        {
            EmployeesOldJobsBLL EmployeeOldJob = new EmployeesOldJobsBLL();
            EmployeeOldJob.EmployeeOldJobID = EmployeeOldJobVM.EmployeeOldJobID;
            EmployeeOldJob.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeOldJobVM.EmployeeCodeID };
            EmployeeOldJob.JobName = EmployeeOldJobVM.JobName;
            EmployeeOldJob.OrganizationName = EmployeeOldJobVM.OrganizationName;
            EmployeeOldJob.RankName = EmployeeOldJobVM.RankName;
            EmployeeOldJob.CareerDegreeName = EmployeeOldJobVM.CareerDegreeName;
            EmployeeOldJob.EmployeeOldJobStartDate = EmployeeOldJobVM.EmployeeOldJobStartDate;
            EmployeeOldJob.EmployeeOldJobEndDate = EmployeeOldJobVM.EmployeeOldJobEndDate;
            EmployeeOldJob.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) };
            Result result = EmployeeOldJob.Update();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                EmployeesOldJobsBLL EmployeeOldJobEntity = (EmployeesOldJobsBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    EmployeeOldJobVM.EmployeeOldJobID = ((EmployeesOldJobsBLL)result.Entity).EmployeeOldJobID;
                }
            }

            return View(this.GetByEmployeeOldJobID(EmployeeOldJobVM.EmployeeOldJobID));     //View(EmployeeOldJobVM);
        }

        

        [HttpGet]
        public JsonResult GetEmployeeOldJobID()
        {
            int? EmployeeOldJobID = Session["EmployeeOldJobID"] == null ? (int?)null : int.Parse(Session["EmployeeOldJobID"].ToString());
            return Json(new { data = EmployeeOldJobID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private EmployeesOldJobsViewModel GetByEmployeeOldJobID(int id)
        {
            EmployeesOldJobsBLL EmployeeOldJobBLL = (new EmployeesOldJobsBLL()).GetByEmployeeOldJobID(id);
            EmployeesOldJobsViewModel EmployeeOldJobVM = new EmployeesOldJobsViewModel();
            if (EmployeeOldJobBLL != null)
            {
                EmployeeOldJobVM.EmployeeOldJobID = EmployeeOldJobBLL.EmployeeOldJobID;
                EmployeeOldJobVM.EmployeeCodeID = EmployeeOldJobBLL.EmployeeCode.EmployeeCodeID;
                EmployeeOldJobVM.EmployeeCode = EmployeeOldJobBLL.EmployeeCode;
                EmployeeOldJobVM.JobName = EmployeeOldJobBLL.JobName;
                EmployeeOldJobVM.OrganizationName = EmployeeOldJobBLL.OrganizationName;
                EmployeeOldJobVM.RankName = EmployeeOldJobBLL.RankName;
                EmployeeOldJobVM.CareerDegreeName = EmployeeOldJobBLL.CareerDegreeName;
                EmployeeOldJobVM.EmployeeOldJobStartDate = EmployeeOldJobBLL.EmployeeOldJobStartDate.Value.Date;
                EmployeeOldJobVM.EmployeeOldJobEndDate = EmployeeOldJobBLL.EmployeeOldJobEndDate.Value.Date;
                EmployeeOldJobVM.CreatedDate = EmployeeOldJobBLL.CreatedDate;
                EmployeeOldJobVM.CreatedBy = EmployeeOldJobVM.GetCreatedByDisplayed(EmployeeOldJobBLL.CreatedBy);
                EmployeeOldJobVM.LastUpdatedBy = EmployeeOldJobVM.GetCreatedByDisplayed(EmployeeOldJobBLL.CreatedBy);
            }
            return EmployeeOldJobVM;
        }
    }
}