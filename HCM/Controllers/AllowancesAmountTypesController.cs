using HCM.Classes;
using HCMBLL;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class AllowancesAmountTypesController : BaseController
    {
        //
        // GET: /GetAllowancesAmountTypes/
        [HttpGet]
        public JsonResult GetAllowancesAmountTypes()
        {
            return Json(new { data = new AllowancesAmountTypesBLL().GetAllowancesAmountTypes() }, JsonRequestBehavior.AllowGet);
        }

    }
}