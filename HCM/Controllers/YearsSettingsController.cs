using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Models;
using HCMBLL;
using System.Web.Mvc;


namespace HCM.Controllers
{
    public class YearsSettingsController : BaseController
    {

        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetMaturityYears()
        {
            return Json(new { data = new MaturityYearsBalancesBLL().GetMaturityYearsBalances() }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByYearSettingID(id));
        }

        [NonAction]
        private YearsSettingsViewModel GetByYearSettingID(int id)
        {
            MaturityYearsBalancesBLL MaturityYearsBalance = new MaturityYearsBalancesBLL().GetByMaturityYearID(id);
            YearsSettingsViewModel YearSettingVM = new YearsSettingsViewModel();
            if (MaturityYearsBalance != null)
            {
                YearSettingVM.MaturityYearBalance = new MaturityYearsBalancesBLL() { MaturityYear = MaturityYearsBalance.MaturityYear, MaturityYearID = MaturityYearsBalance.MaturityYearID };
            }
            return YearSettingVM;
        }
    }
}