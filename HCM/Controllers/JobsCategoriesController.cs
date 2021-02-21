using HCM.Classes;
using HCM.Models;
using HCMBLL;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class JobsCategoriesController : BaseController
    {
        //[CustomAuthorize]
        //public override ActionResult Index()
        //{
        //    return View();
        //}

        public JsonResult GetJobsCategories()
        {
            var data = new JobsCategoriesBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            }.GetJobsCategories(out TotalRecordsOut, out RecFilterOut).Select(item => new
            {
                JobCategoryID = item.JobCategoryID,
                JobCategoryName = item.JobCategoryName,
                JobGroupID = item.JobGroup.JobGroupID,
                JobGroupName = item.JobGroup.JobGroupName,
                JobGeneralGroupID = item.JobGroup.JobGeneralGroup.JobGeneralGroupID,
                JobGeneralGroupName = item.JobGroup.JobGeneralGroup.JobGeneralGroupName,
                QualificationDegreeName = item.MinQualificationDegree != null ? item.MinQualificationDegree.QualificationDegreeName : "",
                QualificationName = item.MinQualification != null ? item.MinQualification.QualificationName : "",
                GeneralSpecializationName = item.MinGeneralSpecialization != null ? item.MinGeneralSpecialization.GeneralSpecializationName : ""
            });

            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetByJobCategoryID(int id)
        {
            return Json(new { data = new JobsCategoriesBLL().GetByJobCategoryID(id) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssignedJobCategoriesByJobCategoryID(int id)
        {
            return Json(new { data = new PromotionsJobsCategoriesBLL().GetAssignedJobCategoriesByJobCategoryID(id) }, JsonRequestBehavior.AllowGet);
        }

        public JobsCategoriesViewModel GetJobsCategoryByJobCategoryID(int id)
        {
            JobsCategoriesViewModel JobsCategoriesVM = new JobsCategoriesViewModel();
            JobsCategoriesBLL JobCategory = new JobsCategoriesBLL().GetByJobCategoryID(id);

            if (JobCategory != null && JobCategory.JobCategoryID > 0)
            {
                JobsCategoriesVM.JobCategoryID = JobCategory.JobCategoryID;
                JobsCategoriesVM.JobCategoryName = JobCategory.JobCategoryName;
                JobsCategoriesVM.JobGroup = JobCategory.JobGroup;
                JobsCategoriesVM.JobGeneralGroup = JobCategory.JobGroup.JobGeneralGroup;
            }

            return JobsCategoriesVM;
        }
    }
}