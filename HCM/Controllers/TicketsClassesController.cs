using HCM.Classes;
using HCMBLL;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class TicketsClassesController : BaseController
    {
        //
        // GET: /GetTicketsClasses/
        [HttpGet]
        public JsonResult GetTicketsClasses()
        {
            return Json(new { data = new TicketsClassesBLL().GetTicketsClasses() }, JsonRequestBehavior.AllowGet);
        }

    }
}