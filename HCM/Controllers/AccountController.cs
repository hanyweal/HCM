using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ExceptionNameSpace;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using PSADTO;
using HCM.Models;
using HCMBLL;
using Globals;
using System.Collections.Generic;
using PSADTO.Enums;
using HCM.Classes;
using HCM.Classes.Exceptions;
using HCMBLL.Enums;

namespace HCM.Controllers
{
    public class AccountController : BaseController
    {
        int ProjectID = int.Parse(ConfigurationManager.AppSettings["ProjectID"].ToString());
        string AppEnvironment = ConfigurationManager.AppSettings["Environment"].ToString();

        [AllowAnonymous]
        [ChildActionOnly]
        public override ActionResult Index()
        {
            //UsersBLL userBLL = new UsersBLL();
            //if (Session["User"] != null)
            //    userBLL = (UsersBLL)Session["User"];

            EmployeesCodesBLL EmployeeCodeNo = new EmployeesCodesBLL().GetByEmployeeCodeNo(Session["EmployeeCodeNo"].ToString());
            AccountViewModel AccountVM = new AccountViewModel()
            {
                UserPic = EmployeeCodeNo.Employee.EmployeePicture,
            };

            ViewBag.Year = Globals.Calendar.GetUmAlQuraYear(DateTime.Now);
            return PartialView("~/Views/Account/_Index.cshtml", AccountVM);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();

            //var ff = Url.Action("UserProfile", "Account");
            //Response.RemoveOutputCacheItem(ff);

            var UserCookie = Request.Cookies["HCMUser"];
            AccountViewModel AccountVM = new AccountViewModel();
            if (UserCookie != null)
            {
                if (!string.IsNullOrEmpty(UserCookie.Value))
                {
                    AccountVM.UserName = UserCookie.Value.ToString();
                    AccountVM.RememberMe = true;
                }
            }
            return View(AccountVM);
        }

        [HttpPost]
        [AllowAnonymous]
        [IgnoreModelPropertiesAttribute("OldPassword,ConfirmPassword")]
        public ActionResult Login(AccountViewModel AccountVM)
        {
            AuthenticationResult result = new UsersBLL().AuthenticateUser(AccountVM.UserName, AccountVM.Password);

            if (result != null)
            {
                #region stop login if the app under Upgrading
                bool IsAppUnderUpgrading = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAppUnderUpgrading"].ToString());
                if (IsAppUnderUpgrading)
                {
                    List<int> AllowedEmpCodes = Utilities.ConvertToList(ConfigurationManager.AppSettings["AllowEmployeeCodesWhileUpgrading"], ',');
                    if (!AllowedEmpCodes.Contains(Convert.ToInt32((result.User.UserName))))
                    {
                        throw new CustomException(Resources.Globalization.AppUnderUpgradingMsgText);
                    }
                }
                #endregion

                EmployeesCodesBLL EmployeeCode = new EmployeesCodesBLL().GetByEmployeeCodeNo(result.User.UserName);
                //AuthenticationResult result = JsonConvert.DeserializeObject<AuthenticationResult>(response.Content.ReadAsStringAsync().Result);
                Session["Employee"] = EmployeeCode;
                Session["Authentication"] = result;
                Session["EmployeeCodeNo"] = result.User.UserName;
                Session["UserID"] = result.User.UserID;
                Session["IsAdmin"] = result.User.IsAdmin;
                Session["EmployeeName"] = EmployeeCode.Employee.EmployeeNameAr;
                Session["EmployeeCodeID"] = EmployeeCode.EmployeeCodeID;

                if (AccountVM.RememberMe)
                {
                    #region save the user name in cookies
                    var JsonUserName = JsonConvert.SerializeObject(int.Parse(result.User.UserName));
                    var UserCookie = new HttpCookie("HCMUser", JsonUserName);
                    UserCookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(UserCookie);
                    #endregion
                }
                else
                {
                    #region remove the user name in cookies
                    var UserCookie = Request.Cookies["HCMUser"];
                    if (UserCookie != null)
                    {
                        UserCookie.Value = null;
                        UserCookie.Expires.AddTicks(1);
                        Response.SetCookie(UserCookie);
                    }
                    #endregion
                }
            }
            else
            {
                throw new CustomException(Resources.Globalization.NotAuthenticatedText);
            }

            //return new HttpResponseMessage(HttpStatusCode.OK);
            return null;
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [CustomAuthorize]
        public ActionResult ChangePassword()
        {
            //return View("~/Views/Account/_ChangePassword.cshtml");
            return View(new AccountViewModel() { UserID = int.Parse(Session["UserID"].ToString()) });
        }

        [IgnoreModelPropertiesAttribute("UserName")]
        [HttpPost]
        public HttpResponseMessage ChangePassword(AccountViewModel AccountVM)
        {
            ChangePasswordResultEnum result = new UsersBLL().ChangePassword(AccountVM.UserID, AccountVM.OldPassword, AccountVM.Password, AccountVM.ConfirmPassword);
            if (result == ChangePasswordResultEnum.Done)
                return new HttpResponseMessage(HttpStatusCode.OK);

            else if (result == ChangePasswordResultEnum.RejectedBecauseOfNewPasswordNotMatchedWithConfirmPassword)
                throw new CustomException(Resources.Globalization.ValidatationPasswordNotMatchedWithConfirmPasswordText);

            else if (result == ChangePasswordResultEnum.RejectedBecauseOfNewPasswordNotMatchedWithPasswordPolicy)
                throw new CustomException(Resources.Globalization.ValidatationPasswordNotMatchedWithPasswordPolicyText);

            else if (result == ChangePasswordResultEnum.RejectedBecauseOfWrongOldPassword)
                throw new CustomException(Resources.Globalization.WrongOldPasswordText);

            else
                throw new CustomException(Resources.Globalization.GeneralErrorMessageText);
        }

        [CustomAuthorize]
        //[OutputCache(Duration = 40, VaryByCustom = "SessionID")]
        public ActionResult UserProfile()
        {
            EmployeesCodesBLL Employee = null;
            if (Session["Employee"] != null)
                Employee = (EmployeesCodesBLL)Session["Employee"];
            else
                Employee = new EmployeesCodesBLL().GetByEmployeeCodeNo(Session["EmployeeCodeNo"].ToString());

            AccountViewModel AccountVM = new AccountViewModel()
            {
                UserName = Employee.EmployeeCodeNo,
                EmployeeName = Employee.Employee.EmployeeNameAr,
                OrganizationName = Employee.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationName,
                JobName = Employee.EmployeeCurrentJob.OrganizationJob.Job.JobName,
                RankName = Employee.EmployeeCurrentJob.OrganizationJob.Rank.RankName,
                IsAdmin = Convert.ToBoolean(Session["IsAdmin"]),
                UserPic = Employee.Employee.EmployeePicture
            };
            //ViewBag.CurrentTime = DateTime.Now;
            return View(AccountVM);
        }

        [CustomAuthorize]
        public JsonResult GetUserGroups()
        {
            AuthenticationResult AuthenticationResult = (AuthenticationResult)System.Web.HttpContext.Current.Session["Authentication"];
            return Json(new { data = AuthenticationResult.Groups }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("Account/SendingOTPToValidateUser/{UserName}")]
        public JsonResult SendingOTPToValidateUser(string UserName)
        {
            string RandomOTP = "1234";
            UsersBLL UserBLL = new UsersBLL();
            AuthenticationResult result = new UsersBLL().GetUserInfo(UserName, this.ProjectID);
            if (result != null)
            {
                if (!AppEnvironment.Equals("dev"))
                {
                    RandomOTP = Globals.Utilities.CreateRandomNumbers(4);
                    string Message = string.Format(Resources.Globalization.OTPMessageText, RandomOTP);

                    UserBLL.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = result.User.Employee.EmployeeCodeID };
                    UserBLL.SendMessage(BusinessSubCategoriesEnum.ForgottenPassword, Message, result.User.Employee.EmployeeMobileNo);                    
                }

                Session["RandomOTP"] = RandomOTP;
            }
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("Account/ResetPassword/{UserID}/{NewPassword}/{ConfirmNewPassword}")]
        public HttpResponseMessage ResetPassword(int UserID, string NewPassword, string ConfirmNewPassword)
        {
            ChangePasswordResultEnum result = new UsersBLL().ResetPassword(UserID, NewPassword, ConfirmNewPassword);
            if (result == ChangePasswordResultEnum.Done)
                return new HttpResponseMessage(HttpStatusCode.OK);

            else if (result == ChangePasswordResultEnum.RejectedBecauseOfNewPasswordNotMatchedWithConfirmPassword)
                throw new CustomException(Resources.Globalization.ValidatationPasswordNotMatchedWithConfirmPasswordText);

            else if (result == ChangePasswordResultEnum.RejectedBecauseOfNewPasswordNotMatchedWithPasswordPolicy)
                throw new CustomException(Resources.Globalization.ValidatationPasswordNotMatchedWithPasswordPolicyText);

            else
                throw new CustomException(Resources.Globalization.GeneralErrorMessageText);
        }

        [HttpGet]
        [Route("Account/ValidateOTP/{OTP}")]
        public JsonResult ValidateOTP(string OTP)
        {
            bool Isvalidated = false;
            if (OTP == Session["RandomOTP"].ToString())
            {
                Isvalidated = true;
            }
            return Json(new { data = Isvalidated }, JsonRequestBehavior.AllowGet);
        }
    }
}