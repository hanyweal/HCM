using HCM.Classes.Enums;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class ModalController : Controller
    {
        //
        // GET: /Modal/
        public ActionResult Index(ModalTypes ModalType, string Message, string CallBackFnName)
        {
            ViewBag.Message = (Message);
            ViewBag.CallBackFnName = CallBackFnName;
            if (ModalType == ModalTypes.Success)
            {
                ViewBag.Class = "alert-success";
                ViewBag.GlyphiconClass = "glyphicon glyphicon-ok";
            }
            else if (ModalType == ModalTypes.Failure)
            {
                ViewBag.Class = "alert-danger";
                ViewBag.GlyphiconClass = "glyphicon glyphicon-remove";
                ViewBag.Message = Session["Exception"] != null ? Server.UrlDecode(Session["Exception"].ToString()) : Message;
                Session["Exception"] = null;
            }
            else if (ModalType == ModalTypes.Warning)
            {
                ViewBag.Class = "alert-warning";
                ViewBag.GlyphiconClass = "glyphicon glyphicon-alert";
            }
            else if (ModalType == ModalTypes.Notice)
            {
                ViewBag.Class = "alert-info";
                ViewBag.GlyphiconClass = "glyphicon glyphicon-info-sign";
            }
            return PartialView("~/Views/Shared/_Alert.cshtml");
        }
    }
}