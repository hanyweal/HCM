using ExceptionNameSpace;
using HCM.Classes;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using PSADTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class InformationCardForPromotionController : BaseEServicesController
    {
        int ProjectID = int.Parse(ConfigurationManager.AppSettings["ProjectID"].ToString());
        string AppEnvironment = ConfigurationManager.AppSettings["Environment"].ToString();
        string EmployeeCodeNo {
            get
            {
                return Session["WindowsUserIdentity"].ToString();
            }
            set {
                Session["WindowsUserIdentity"] = value;
            }
        }
        // GET: InformationCardForPromotion
        public ActionResult Index()
        {
            EmployeeCodeNo = WindowsUserIdentity;// WindowsUserIdentity;//"90044011";// "90044140"; //
            EmployeesCodesBLL employeesCodes= new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);


            Result result = new PromotionCardsPrintingBLL().CheckPromotionCardsPrintingForEmployeeByEmployeeCodeID(employeesCodes.EmployeeCodeID);
            PromotionCardsPrintingBLL ResultPromotionCardsPrintingBLL = (PromotionCardsPrintingBLL)result.Entity;
            if (result.EnumMember == PromotionCardsPrintingValidationEnum.RejectedBecauseOfThereIsNoActivePromotionPeriod.ToString())
            {
                return View("NoActivePromotionsPeriods");
            }
            else if (result.EnumMember == PromotionCardsPrintingValidationEnum.RejectedBecauseOfEmployeeHaveRecordWithSamePeriod.ToString())
            {
                ViewBag.PrintedBefor = true.ToString();
                return View(new PromotionCardsPrintingDetailsViewModel() { PromotionCardPrintingID= ResultPromotionCardsPrintingBLL.PromotionCardPrintingID, EmployeeCodeID = ResultPromotionCardsPrintingBLL.EmployeesCodes.EmployeeCodeID });
            }
            else
            {
                ViewBag.PrintedBefor = false.ToString();
                return View(new PromotionCardsPrintingDetailsViewModel() { PromotionPeriodID = ResultPromotionCardsPrintingBLL.PromotionsPeriod.PromotionPeriodID, EmployeeCodeID = ResultPromotionCardsPrintingBLL.EmployeesCodes.EmployeeCodeID });
            }
        }
  
       
        [Route("InformationCardForPromotion/Approved")]
        [HttpPost]
        public ActionResult Approved(PromotionCardsPrintingDetailsViewModel PromotionCardsPrintingDetailsViewModel)
        {
            #region  Validate ViewModel
            bool IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllowncesDetails = false;
            if(PromotionCardsPrintingDetailsViewModel.IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllowncesDetails==null)
                throw new CustomException(Resources.Globalization.ValidateIsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllowncesDetailsText);
            
            if (!PromotionCardsPrintingDetailsViewModel.IsApprovedDetails)
                throw new CustomException(Resources.Globalization.ValidationRatificationText);

            IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllowncesDetails = (bool)PromotionCardsPrintingDetailsViewModel.IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllowncesDetails;
            #endregion
            var IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllowncesText = IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllowncesDetails ? Resources.Globalization.YesText : Resources.Globalization.NoText;

            Result result = new PromotionCardsPrintingBLL()
            {
                LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = PromotionCardsPrintingDetailsViewModel.EmployeeCodeID },
                PromotionsPeriod =new PromotionsPeriodsBLL() { PromotionPeriodID= PromotionCardsPrintingDetailsViewModel.PromotionPeriodID },
                EmployeesCodes=new EmployeesCodesBLL() { EmployeeCodeID= PromotionCardsPrintingDetailsViewModel.EmployeeCodeID }
            }.Add();
            PromotionCardsPrintingBLL PromotionCardsPrinting;
            if (result.EnumMember == PromotionCardsPrintingValidationEnum.Done.ToString())
            {
                PromotionCardsPrinting = (PromotionCardsPrintingBLL)result.Entity;
                return Redirect(string.Format("~/WebForms/Reports/AfterRatificationPromotionCard.aspx?EmployeeCodeID={0}&PromotionPeriodID={1}&PromotionCardPrintingID={2}&IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllownces={3}", PromotionCardsPrinting.EmployeesCodes.EmployeeCodeID, PromotionCardsPrinting.PromotionsPeriod.PromotionPeriodID, PromotionCardsPrinting.PromotionCardPrintingID, IsApprovedYouHaveJobWithAllowncesAndGotJobWithoutAllowncesText));

            }
            else if (result.EnumMember == PromotionCardsPrintingValidationEnum.RejectedBecauseOfEmployeeHaveRecordWithSamePeriod.ToString())
            {

                throw new CustomException(Resources.Globalization.ValidationEmployeeHasRecordWithSamePeriodText);
            }
            return Json(new { PromotionPeriodID = PromotionCardsPrintingDetailsViewModel.PromotionPeriodID }, JsonRequestBehavior.AllowGet);
        }
        #region OTP
        [HttpGet]
        [Route("InformationCardForPromotion/SendingOTPToValidateUser")]
        public JsonResult SendingOTPToValidateUser()
        {
            
            string RandomOTP = "1234";
            UsersBLL UserBLL = new UsersBLL();
            EmployeesCodesBLL employeesCodes = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);
            string Message="";
                if (!AppEnvironment.Equals("dev"))
                {
                    RandomOTP = Globals.Utilities.CreateRandomNumbers(4);
                    Message = string.Format(Resources.Globalization.OTPMessageText, RandomOTP);

                  //  UserBLL.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = result.User.Employee.EmployeeCodeID };
                    UserBLL.SendMessage(BusinessSubCategoriesEnum.ForgottenPassword, Message, employeesCodes.Employee.EmployeeMobileNo);
                }

                Session["RandomOTP"] = RandomOTP;
           
            return Json(new { data = Message }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [Route("InformationCardForPromotion/ValidateOTP/{OTP}")]
        public JsonResult ValidateOTP(string OTP)
        {
            bool Isvalidated = false;
            if (OTP == Session["RandomOTP"].ToString())
            {
                Isvalidated = true;
            }
            return Json(new { data = Isvalidated }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PDF
        [Route("InformationCardForPromotion/PrintPromotionCardBeforRatificationByPromotionPeriod/{EmployeeCodeID}/{PromotionPeriodID}")]
        public ActionResult PrintPromotionCardBeforRatificationByPromotionPeriod(int EmployeeCodeID, int PromotionPeriodID)
        {
            return Redirect(string.Format("~/WebForms/Reports/BeforRatificationPromotionCard.aspx?EmployeeCodeID={0}&PromotionPeriodID={1}", EmployeeCodeID, PromotionPeriodID));
        }
        [Route("InformationCardForPromotion/PrintPromotionCardExist/{PromotionCardPrintingID}")]
        public ActionResult PrintPromotionCardExist(int PromotionCardPrintingID)
        {

            return Redirect(string.Format("~/WebForms/Reports/ExistPromotionCard.aspx?PromotionCardPrintingID={0}", PromotionCardPrintingID));
        }
        #endregion

    }
}