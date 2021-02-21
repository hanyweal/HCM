using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class PayrollsController : BaseController
    {
        // GET: Payrolls
        [CustomAuthorize]
        public ActionResult VacationsDuringOvertimeDays()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult EmployeesSalariesDetails()
        {
            return View();
        }

        [HttpGet]
        [Route("{controller}/GetEmployeesSalaryDetails/{EmployeeCodeNo}")]
        public JsonResult GetEmployeesSalaryDetails(string EmployeeCodeNo = "")
        {
            if (EmployeeCodeNo == "0") EmployeeCodeNo = string.Empty;
            List<SalaryDetailsBLL> SalaryDetailBLL = new SalaryDetailsBLL().GetSalaryDetails(EmployeeCodeNo);
            var f = SalaryDetailBLL.Select(x => new 
            {
                EmployeeCodeNo = x.Employee.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.Employee.EmployeeCode.Employee.EmployeeNameAr,
                RankCategoryName = x.Employee.OrganizationJob.Rank.RankCategory.RankCategoryName,
                RankName = x.Employee.OrganizationJob.Rank.RankName,
                CareerDegreeName = x.Employee.CareerDegree.CareerDegreeName,
                BasicSalary = x.Benefits != null ? x.Benefits.BasicSalary : 0,
                TransfareAllowance = x.Benefits != null ? x.Benefits.TransfareAllowance : 0,
                TotalAllowances = x.Benefits != null ? x.Benefits.TotalAllowances : 0,
                TotalDeductions = x.Benefits != null ? x.Deductions.TotalDeductions : 0,
                NetSalary = x.NetSalary
            });
            return Json(new { data = f }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{controller}/GetVacationDaysDuringOvertime/{OverTimeID}/{OverTimeDetailID}")]
        public JsonResult GetDaysBetweenStartAndEndDate(int OverTimeID, int OverTimeDetailID)
        {
            return Json(new { data = new PayrollsBLL().GetDaysBetweenStartAndEndDate(OverTimeID, OverTimeDetailID) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOverTimeAndVacationDaysByOverTimeID(int id)
        {
            return Json(new { data = new PayrollsBLL().GetOverTimeAndVacationDaysByOverTimeID(id) }, JsonRequestBehavior.AllowGet);
        }
    }
}