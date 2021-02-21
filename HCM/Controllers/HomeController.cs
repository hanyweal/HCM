using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorize]
        public ActionResult Index()
        {

            //new BaseTransfersBLL().StartProcess();

            //try
            //{
            //    System.Diagnostics.Process process = new System.Diagnostics.Process();
            //    process.StartInfo.FileName = Server.MapPath("~/Content/Adobe Reader 9.lnk");
            //    process.StartInfo.Arguments = @" /A ""zoom=100"" ""G:\a.pdf""";
            //    process.Start();
            //    process.Close();
            //    ViewBag.Result = "Done";
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Result = ex.Message;
            //    throw;
            //}

            //try
            //{
            //    new NormalVacationsBLL().GenerateBalancesForAllUsers();
            //}
            //catch (Exception ex)
            //{


            //    ViewBag.Class = "alert-danger";
            //    ViewBag.GlyphiconClass = "glyphicon glyphicon-remove";
            //    ViewBag.Message = ex.Message + " Inner Ex: " + (ex.InnerException != null ? ex.InnerException.Message : "");

            //    return PartialView("~/Views/Shared/_Alert.cshtml");
            //}

            //try
            //{
            //    new BasicSalariesBLL().GenerateSalaryForAllUsers();
            //}
            //catch (Exception ex)
            //{


            //    ViewBag.Class = "alert-danger";
            //    ViewBag.GlyphiconClass = "glyphicon glyphicon-remove";
            //    ViewBag.Message = ex.Message + " Inner Ex: " + (ex.InnerException != null ? ex.InnerException.Message : "");

            //    return PartialView("~/Views/Shared/_Alert.cshtml");
            //}

            return View();
        }
    }
}