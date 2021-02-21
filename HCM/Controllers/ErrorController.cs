using HCM.Classes;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public override ActionResult Index()
        {
            return View();
        }

        public ActionResult General(HandleErrorInfo model)
        {
            return PartialView("~/Views/Error/General.cshtml", model);
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }

        public ActionResult Relogin()
        {
            return View();
        }

        public ActionResult UserIdentityNotKnown()
        {
            return View();
        }
    }
}