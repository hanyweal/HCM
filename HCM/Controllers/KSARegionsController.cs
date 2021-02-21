using HCM.Classes;
using HCMBLL;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class KSARegionsController : BaseController
    {
        //
        // GET: /GetKSARegions/
        [HttpGet]
        public JsonResult GetKSARegions()
        {
            //int KSARegionID = 1;
            return Json(new { data = new KSARegionsBLL().GetKSARegions() }, JsonRequestBehavior.AllowGet);
        }

    }
}