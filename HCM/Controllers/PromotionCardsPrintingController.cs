using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class PromotionCardsPrintingController : BaseController
    {
        [CustomAuthorize]
        public ActionResult Manage(int id)
        {
            return View(new PromotionCardsPrintingViewModel() { PromotionPeriodID = id });
        }

        [CustomAuthorize]
        [Route("PromotionCardsPrinting/GetPromotionCardsPrintingByEmployeeCodeID/{EmployeeCodeID}")]
        public JsonResult GetPromotionCardsPrintingByEmployeeCodeID(int EmployeeCodeID)
        {
            return Json(new
            {
                data = ((new PromotionCardsPrintingBLL().GetPromotionCardsPrintingByEmployeeCodeID(EmployeeCodeID))).Select(x => new
                {
                    PromotionCardPrintingID = x.PromotionCardPrintingID,
                    PeriodName = x.PromotionsPeriod.Period.PeriodName,
                    MaturityYear = x.PromotionsPeriod.Year.MaturityYear,
                    PromotionStartDate = x.PromotionsPeriod.PromotionStartDate,
                    PromotionEndDate = x.PromotionsPeriod.PromotionEndDate,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(PromotionCardsPrintingViewModel PromotionCardPrintingVM)
        {
            PromotionCardsPrintingBLL PromotionCardPrintingBLL = new PromotionCardsPrintingBLL()
            {
                EmployeesCodes = new EmployeesCodesBLL() { EmployeeCodeID = PromotionCardPrintingVM.EmployeeCodeID },
                PromotionsPeriod = new PromotionsPeriodsBLL() { PromotionPeriodID = PromotionCardPrintingVM.PromotionPeriodID },
                LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = UserIdentity.EmployeeCodeID },
            };
            Result result = PromotionCardPrintingBLL.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    PromotionCardPrintingVM.PromotionCardPrintingID = ((PromotionCardsPrintingBLL)result.Entity).PromotionCardPrintingID;
                }
            }
            return Json(new { PromotionCardPrintingVM.EmployeeCodeID, PromotionCardPrintingVM.PromotionPeriodID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        [Route("PromotionCardsPrinting/PrintPromotionCardByPromotionPeriod/{EmployeeCodeID}/{PromotionPeriodID}")]
        public ActionResult PrintPromotionCardByPromotionPeriod(int EmployeeCodeID, int PromotionPeriodID)
        {
            return Redirect(string.Format("~/WebForms/Reports/PromotionCard.aspx?EmployeeCodeID={0}&PromotionPeriodID={1}", EmployeeCodeID, PromotionPeriodID));
        } 
        
    }
}