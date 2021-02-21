using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class HelperController : Controller
    {
        [HttpGet]
        [Route("{controller}/GetDays/{Day}")]
        public JsonResult GetDays(string Day)
        {
            return Json(new { data = Globals.Utilities.GetNumAsList(1, 30).Where(x => x.StartsWith(Day)) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetMonths/{Month}")]
        public JsonResult GetMonths(string Month)
        {
            return Json(new { data = Globals.Utilities.GetNumAsList(1, 12).Where(x => x.StartsWith(Month)) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetYears/{Year}")]
        public JsonResult GetYears(string Year)
        {
            return Json(new { data = Globals.Utilities.GetNumAsList(1350, 1450).Where(x => x.StartsWith(Year)) }, JsonRequestBehavior.AllowGet);
        }
    }
}