using ExceptionNameSpace;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Classes.Exceptions;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

namespace HCM.Classes
{
    public abstract class BaseController : Controller
    {
        public EmployeesCodesBLL UserIdentity
        {
            get
            {
                return new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()), EmployeeCodeNo = Session["EmployeeCodeNo"].ToString() };
            }
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult ExportExcel()
        {
            return View("Index");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            IEx iex = ExceptionFactory.CreateException(ExceptionEnum.DatabaseException);
            Exception ex = filterContext.Exception;

            //Log Exception ex
            Session["Exception"] = iex.Msg(ex);
            //return;

            // Redirect on error:
            //filterContext.ExceptionHandled = false;
            //return;
            //filterContext.Result = new RedirectToRouteResult(
            //                  new RouteValueDictionary(
            //                      new
            //                      {
            //                          controller = "Error",
            //                          action = "General"
            //                      })
            //                  );

            //filterContext.Result = new ViewResult
            //{
            //    ViewName = "~/Views/Shared/Error.cshtml"
            //};

            //// OR set the result without redirection:
            //filterContext.Result = new ViewResult
            //{
            //    ViewName = "~/Views/Error/Index.cshtml"
            //};
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var atrCurrent = System.Reflection.MethodBase.GetCurrentMethod().GetCustomAttributes(typeof(NoSecurtiyCheckingAttribute), true).FirstOrDefault() as NoSecurtiyCheckingAttribute;
            //var gg = filterContext.ActionDescriptor.GetCustomAttributes(true).FirstOrDefault();

            string ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string ActionName = filterContext.ActionDescriptor.ActionName;

            #region Check Allow Anonymous or not
            var AllowAnonymous = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
            if (AllowAnonymous.Length == 0)
            {
                if (!ControllerName.Equals("Error")
                    && !ControllerName.Equals("Account")
                    && System.Web.HttpContext.Current.Session["UserID"] == null)
                {
                    throw new CustomException(Resources.Globalization.ReloginAgainShouldBeText);
                }
            }
            #endregion

            object[] customsAttributes = filterContext.ActionDescriptor.GetCustomAttributes(true);
            //var IsAllowAnonymous = false;
            //foreach (object item in customsAttributes)
            //{
            //    if (item is NoSecurtiyCheckingAttribute)
            //        IsAllowAnonymous = true;
            //}

            //if (!IsAllowAnonymous)
            //{
            //    if (!ControllerName.Equals("Error") 
            //        && !ControllerName.Equals("Account") 
            //        && System.Web.HttpContext.Current.Session["UserID"] == null)
            //    {
            //        throw new CustomException(Resources.Globalization.ReloginAgainShouldBeText);
            //    }
            //}

            foreach (object item in customsAttributes)
            {
                if (item is IgnoreModelPropertiesAttribute)
                {
                    IgnoreModelPropertiesAttribute IgonreModelAttribute = (IgnoreModelPropertiesAttribute)item;
                    //IgonreModelAttribute.OnActionExecuting(filterContext);
                    IgonreModelAttribute.IgnoreModelProperties(filterContext);
                }
            }

            /* this is to get properties from model if this properties did not be bind with value from view to controller
            var model = filterContext.Controller.ViewData.Model as UsersViewModel;
            var modelState = filterContext.Controller.ViewData.ModelState;
            var valueProvider = filterContext.Controller.ValueProvider;
            List<string> keysWithNoIncomingValue = modelState.Keys.Where(x => !valueProvider.ContainsPrefix(x)).ToList();
            foreach (string key in keysWithNoIncomingValue)
                //modelState[key].Errors.Clear();
                modelState.Remove(key);*/

            ModelStateDictionary modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                string Message = string.Empty;
                foreach (ModelState item in modelState.Values)
                {
                    foreach (ModelError error in item.Errors)
                    {
                        Message += " - ";
                        Message += !string.IsNullOrEmpty(error.ErrorMessage) ? error.ErrorMessage : error.Exception.Message;
                        Message += " NewLine ";
                    }
                }
                throw new CustomException(Message);
            }

            base.OnActionExecuting(filterContext);
        }

        public JsonResult GetUmAlquraEndDate(int Period, string StartDate)
        {
            string UmAlquraEndDate = string.Empty;
            if (!string.IsNullOrEmpty(StartDate) && Period != 0)
            {
                string GrStartDate = Globals.Calendar.UmAlquraToGreg(StartDate);
                CultureInfo enCul = new CultureInfo("en-US");
                enCul.DateTimeFormat.Calendar = new GregorianCalendar(GregorianCalendarTypes.USEnglish);
                Thread.CurrentThread.CurrentUICulture = enCul;
                Thread.CurrentThread.CurrentCulture = enCul;
                DateTime GrEndDate = Convert.ToDateTime(GrStartDate).AddDays(Period - 1).Date;
                UmAlquraEndDate = Globals.Calendar.GetUmAlQuraDate(GrEndDate);
            }

            return Json(new { data = UmAlquraEndDate }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGregDateFromUmAlquraDate(string Date)
        {
            string GrStartDate = string.Empty;
            if (!string.IsNullOrEmpty(Date))
                GrStartDate = Globals.Calendar.UmAlquraToGreg(Date);

            return Json(new { data = GrStartDate }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUmAlquraDateFromGregDate(DateTime Date)
        {
            string UmAlquraDate = string.Empty;
            if (!string.IsNullOrEmpty(Date.ToString()))
                UmAlquraDate = Globals.Calendar.GregToUmAlQura(Date);

            return Json(new { data = UmAlquraDate }, JsonRequestBehavior.AllowGet);
        }

        public string GetUmAlquraPreviousDay(string StartDate)
        {
            string UmAlquraEndDate = string.Empty;
            if (!string.IsNullOrEmpty(StartDate))
            {
                string GrStartDate = Globals.Calendar.UmAlquraToGreg(StartDate);
                CultureInfo enCul = new CultureInfo("en-US");
                enCul.DateTimeFormat.Calendar = new GregorianCalendar(GregorianCalendarTypes.USEnglish);
                Thread.CurrentThread.CurrentUICulture = enCul;
                Thread.CurrentThread.CurrentCulture = enCul;
                DateTime GrEndDate = Convert.ToDateTime(GrStartDate).AddDays(-1).Date;
                UmAlquraEndDate = Globals.Calendar.GetUmAlQuraDate(GrEndDate);
            }

            return UmAlquraEndDate;
        }

        public JsonResult SetJsonResultWithMaxJsonLength(object DataToConvertToJson)
        {
            var jsonResult = Json(new { data = DataToConvertToJson }, JsonRequestBehavior.AllowGet);
            //int len = jsonResult.MaxJsonLength.HasValue ? jsonResult.MaxJsonLength.Value : 0;
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region DataTable Server Side Prop
        public string Search
        {
            get
            {
                if (Request.HttpMethod == "GET")
                    return Request.QueryString["search[value]"];
                else if (Request.HttpMethod == "GET")
                    return Request.Form.GetValues("search[value]")[0];
                return string.Empty;

            }
        }
        public string Draw
        {
            get
            {
                return Request.QueryString["draw"];
            }
        }
        public string Order
        {
            get
            {
                return Request.QueryString["order[0][column]"];
            }
        }

        public string OrderByColumnName
        {
            get
            {
                return Request.QueryString["columns[" + Order + "][data]"];
            }
        }

        public string OrderDir
        {
            get
            {
                return Request.QueryString["order[0][dir]"];
            }
        }
        public int StartRec
        {
            get
            {
                return Request.QueryString["start"] != null ? Convert.ToInt32(Request.QueryString["start"]) : 0;
            }
        }
        public int PageSize
        {
            get
            {
                return Request.QueryString["length"] != null ? Convert.ToInt32(Request.QueryString["length"]) : 10;
            }
        }
        public int TotalRecordsOut;
        public int RecFilterOut;

        #endregion//DataTable  Server Side Prop
    }
}