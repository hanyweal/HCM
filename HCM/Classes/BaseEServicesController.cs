using HCM.Classes.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HCMBLL;
using HCM.Models;
using System.Web.Routing;

namespace HCM.Classes
{
    public class BaseEServicesController : Controller
    {
        private string _WindowsUserIdentity;
        public string WindowsUserIdentity
        {
            get
            {
                //_UserIdentity = "90025365";
                _WindowsUserIdentity = Request.LogonUserIdentity.Name.Substring(Request.LogonUserIdentity.Name.IndexOf('\\') + 1);
                if (_WindowsUserIdentity.ToLower() == "hany" || // hany aldesouky                              
                    _WindowsUserIdentity.ToLower() == "mshaik" || // mohie //      
                    _WindowsUserIdentity.ToLower() == "Nawaf" || // Nawaf // 
                    _WindowsUserIdentity.ToLower() == "zeshan" ||
                    _WindowsUserIdentity.ToLower() == "zeeshan")// || // zeshan // 
                    _WindowsUserIdentity = System.Configuration.ConfigurationManager.AppSettings["WindowsUserIdentity"].ToString();

                return _WindowsUserIdentity;
            }
            set
            {
                _WindowsUserIdentity = value;
            }
        }

        public EmployeesCodesBLL WindowsEmployeeCode
        {
            get
            {
                EmployeesCodesBLL EmployeeCodeBLL = new EmployeesCodesBLL();
                if (Session["WindowsEmployeeCode"] == null)
                    EmployeeCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeNo(this.WindowsUserIdentity);

                else
                    EmployeeCodeBLL = (EmployeesCodesBLL)Session["WindowsEmployeeCode"];

                return EmployeeCodeBLL;
            }
        }

        public List<OrganizationsStructuresBLL> WindowsUserOrganizationStructures
        {
            get
            {
                List<OrganizationsStructuresBLL> lst = new List<OrganizationsStructuresBLL>();
                if (Session["WindowsUserOrganizationStructures"] == null)
                {
                    lst = new OrganizationsStructuresBLL().GetAllOrganizationsForManagerByManagerCodeNo(this.WindowsUserIdentity);
                    if (lst == null)
                        lst = new List<OrganizationsStructuresBLL>();
                }
                else
                    lst = (List<OrganizationsStructuresBLL>)Session["WindowsUserOrganizationStructures"];

                return lst;
            }
        }

        public int WindowsUserOrganizationID
        {
            get
            {
                int OrgID = 0;
                if (Session["OrganizationID"] == null)
                {
                    OrganizationsStructuresBLL org = WindowsUserOrganizationStructures.FirstOrDefault();
                    if (org != null && org.OrganizationID > 0)
                        OrgID = org.OrganizationID;

                    Session["OrganizationID"] = OrgID;
                }
                else
                {
                    OrgID = Convert.ToInt32(Session["OrganizationID"]);
                }

                return OrgID;//OrgID;//2094; //       // 2092;// 
            }
            set
            {
                Session["OrganizationID"] = value;
            }
        }

        //public bool IsUserKnown
        //{
        //    get
        //    {
        //        return Session["IsUserKnown"] != null ? Convert.ToBoolean(Session["IsUserKnown"].ToString()) : false;
        //    }
        //}

        //public string DateFormat
        //{
        //    get
        //    {
        //        return System.Configuration.ConfigurationManager.AppSettings["DateFormat"].ToString();
        //    }
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (Session["UserIdentity"] == null)
            //    Session["UserIdentity"] = new EmployeesCodesBLL().GetByEmployeeCodeNo(this.WindowsUserIdentity);

            EmployeesCodesBLL UserIdentity = WindowsEmployeeCode;
            
            if (UserIdentity == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                                          new RouteValueDictionary(
                                              new
                                              {
                                                  controller = "Error",
                                                  action = "UserIdentityNotKnown"
                                              })
                                          );
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            IEx iex = ExceptionFactory.CreateException(ExceptionEnum.DatabaseException);
            Exception ex = filterContext.Exception;

            //Log Exception ex
            Session["Exception"] = iex.Msg(ex);
        }

        [HttpGet]
        [Route("BaseEServices/RetrieveImage/{ImageFileName}")]
        public ActionResult RetrieveImage(string ImageFileName)
        {
            string ImagePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ImagesPath"]) + ImageFileName;

            if (!System.IO.File.Exists(ImagePath))
                ImagePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ImagesPath"]) + "anonymous.png";

            byte[] img = System.IO.File.ReadAllBytes(ImagePath);

            if (img != null)
            {
                return File(img, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        [ChildActionOnly]
        public PartialViewResult GetWindowsUserOrganizationStructures()
        {
            ViewBag.OrganizationStructures = WindowsUserOrganizationStructures;
            ViewBag.OrganizationID = WindowsUserOrganizationID;
            return PartialView("~/Views/Shared/_EServicesOrganizations.cshtml");
        }

        [HttpPost]
        public ActionResult ChangeWindowsUserOrganization(int id)
        {
            this.WindowsUserOrganizationID = id;
            return Json(new { WindowsUserOrganizationID = id }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetUserIdentityInfo")]
        public JsonResult GetUserIdentityInfo()
        {
            //EmployeesCodesBLL UserIdentity = new EmployeesCodesBLL().GetByEmployeeCodeNo(this.WindowsUserIdentity);
            //Session["IsUserKnown"] = UserIdentity != null ? true : false;

            //EmployeesCodesBLL UserIdentity = Session["UserIdentity"] != null ? (EmployeesCodesBLL)Session["UserIdentity"] : null;

            EmployeesCodesBLL UserIdentity = WindowsEmployeeCode;

            PlacementBLL CurrentPlacement = new PlacementBLL().GetCurrentActualOrgAndActualJob(this.WindowsUserIdentity);

            var data = new
            {
                UserCodeID = UserIdentity?.EmployeeCodeID,
                UserCodeNo = UserIdentity?.EmployeeCodeNo,
                UserName = UserIdentity?.Employee?.EmployeeNameAr,
                UserIDNo = UserIdentity?.Employee?.EmployeeIDNo,
                //OrganizationName = UserIdentity != null ? UserIdentity.EmployeeCurrentJob != null ? UserIdentity.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationName : string.Empty : string.Empty,
                //OrganizationName = ((InternalAssigningBLL)new InternalAssigningBLL().GetEmployeeActiveAssigning(this.WindowsUserIdentity))?.Organization?.FullOrganizationName,
                OrganizationName = CurrentPlacement?.Organization?.FullOrganizationName,
                JobName = CurrentPlacement?.Job?.JobName,
                UserPicture = UserIdentity?.Employee?.EmployeePicture
            };
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
    }
}