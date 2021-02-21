using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Classes.Helpers;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class GovernmentFundsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "GovernmentFundID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "GovernmentDeductionTypeName", Resources.Globalization.GovernmentDeductionsTypesText },
                { "GovernmentFundTypeName", Resources.Globalization.GovernmentFundsTypesText },
                { "MonthlyDeductionAmount", Resources.Globalization.MonthlyDeductionAmountText },
                { "TotalDeductionAmount", Resources.Globalization.TotalDeductionAmountText },
                { "DeductionStartDate", Resources.Globalization.DeductionStartDateText },
                { "IsActive", Resources.Globalization.IsActiveText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetAllGovernmentFunds(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllGovernmentFunds()
        {
            var data = new GovernmentFundsBLL().GetGovernmentFunds().Select(x => new
            {
                GovernmentFundID = x.GovernmentFundID,
                EmployeeCodeNo = x.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.EmployeeCode.Employee.EmployeeNameAr,
                GovernmentDeductionTypeName = x.GovernmentDeductionType.GovernmentDeductionTypeName,
                GovernmentFundTypeName = x.GovernmentFundType.GovernmentFundTypeName,
                MonthlyDeductionAmount = x.MonthlyDeductionAmount,
                TotalDeductionAmount = x.TotalDeductionAmount,
                DeductionStartDate = Globals.Calendar.GetUmAlQuraDate(x.DeductionStartDate),
                IsActive = x.IsActive
            });

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByGovernmentFundID(id));
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [IgnoreModelProperties("DeactiveDate,LetterDate,LetterNo")]
        public ActionResult Create(GovernmentFundsViewModel GovernmentFundsVM)
        {
            GovernmentFundsBLL GovernmentFundBLL = new GovernmentFundsBLL();
            GovernmentFundBLL.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = GovernmentFundsVM.EmployeeCodeID };
            GovernmentFundBLL.GovernmentDeductionType = GovernmentFundsVM.GovernmentDeductionType;
            GovernmentFundBLL.GovernmentFundType = GovernmentFundsVM.GovernmentFundType;
            GovernmentFundBLL.LoanNo = GovernmentFundsVM.LoanNo;
            GovernmentFundBLL.LoanDate = GovernmentFundsVM.LoanDate;
            GovernmentFundBLL.MonthlyDeductionAmount = GovernmentFundsVM.MonthlyDeductionAmount;
            GovernmentFundBLL.TotalDeductionAmount = GovernmentFundsVM.TotalDeductionAmount;
            GovernmentFundBLL.DeductionStartDate = GovernmentFundsVM.DeductionStartDate;
            GovernmentFundBLL.ContractNo = GovernmentFundsVM.ContractNo;
            GovernmentFundBLL.BankIPAN = GovernmentFundsVM.BankIPAN;
            GovernmentFundBLL.KSACity = new KSACitiesBLL() { KSACityID = GovernmentFundsVM.KSACity.KSACityID };
            GovernmentFundBLL.SponserToIDNo = GovernmentFundsVM.SponserToIDNo;
            GovernmentFundBLL.SponserToName = GovernmentFundsVM.SponserToName;
            GovernmentFundBLL.LoginIdentity = UserIdentity;
            Result result = GovernmentFundBLL.Add();
            if (result.EnumMember == GovernmentFundsValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == GovernmentFundsValidationEnum.RejectedBecauseOfBeforeHiringDate.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationBeforeHiringText.ToString());
            }
            else if (result.EnumMember == GovernmentFundsValidationEnum.RejectedBecauseOfMonthlyDeductionAmountShouldNotGreaterThanTotalDeductionAmount.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationDeductionAmountShouldNotGreaterThenTotalDeductionAmountText);
            }

            return Json(new { GovernmentFundID = GovernmentFundBLL.GovernmentFundID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByGovernmentFundID(id));
        }

        [CustomAuthorize]
        public ActionResult Deactivation(int id)
        {
            return View(this.GetByGovernmentFundID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("DeactiveDate,LetterDate,LetterNo")]
        public ActionResult Edit(GovernmentFundsViewModel GovernmentFundsVM)
        {
            GovernmentFundsBLL GovernmentFundBLL = new GovernmentFundsBLL();
            GovernmentFundBLL.GovernmentFundID = GovernmentFundsVM.GovernmentFundID;
            GovernmentFundBLL.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = GovernmentFundsVM.EmployeeCodeID };
            GovernmentFundBLL.GovernmentDeductionType = GovernmentFundsVM.GovernmentDeductionType;
            GovernmentFundBLL.GovernmentFundType = GovernmentFundsVM.GovernmentFundType;
            GovernmentFundBLL.LoanNo = GovernmentFundsVM.LoanNo;
            GovernmentFundBLL.LoanDate = GovernmentFundsVM.LoanDate;
            GovernmentFundBLL.DeductionStartDate = GovernmentFundsVM.DeductionStartDate;
            GovernmentFundBLL.MonthlyDeductionAmount = GovernmentFundsVM.MonthlyDeductionAmount;
            GovernmentFundBLL.TotalDeductionAmount = GovernmentFundsVM.TotalDeductionAmount;
            GovernmentFundBLL.ContractNo = GovernmentFundsVM.ContractNo;
            GovernmentFundBLL.BankIPAN = GovernmentFundsVM.BankIPAN;
            GovernmentFundBLL.KSACity = new KSACitiesBLL() { KSACityID = GovernmentFundsVM.KSACity.KSACityID };
            GovernmentFundBLL.SponserToIDNo = GovernmentFundsVM.SponserToIDNo;
            GovernmentFundBLL.SponserToName = GovernmentFundsVM.SponserToName;
            GovernmentFundBLL.LoginIdentity = UserIdentity;


            Result result = GovernmentFundBLL.Update();
          
            if (result.EnumMember == GovernmentFundsValidationEnum.RejectedBecauseOfBeforeHiringDate.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationBeforeHiringText.ToString());
            }
            else if (result.EnumMember == GovernmentFundsValidationEnum.RejectedBecauseOfAlreadyDeactivated.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationAlreadyDeactivatedText.ToString());
            }
            else if (result.EnumMember == GovernmentFundsValidationEnum.RejectedBecauseOfMonthlyDeductionAmountShouldNotGreaterThanTotalDeductionAmount.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationDeductionAmountShouldNotGreaterThenTotalDeductionAmountText);
            }

            return Json(new { GovernmentFundID = GovernmentFundBLL.GovernmentFundID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeVM")]
        public ActionResult Deactivation(GovernmentFundsViewModel GovernmentFundsVM)
        {
            GovernmentFundsBLL GovernmentFundBLL = new GovernmentFundsBLL();
            GovernmentFundBLL.GovernmentFundID = GovernmentFundsVM.GovernmentFundID;
            GovernmentFundBLL.LetterDate = GovernmentFundsVM.LetterDate;
            GovernmentFundBLL.LetterNo = GovernmentFundsVM.LetterNo;
            GovernmentFundBLL.Notes = GovernmentFundsVM.Notes;
            GovernmentFundBLL.DeactiveDate = GovernmentFundsVM.DeactiveDate;
            GovernmentFundBLL.DeductionStartDate = GovernmentFundsVM.DeductionStartDate;
            GovernmentFundBLL.GovernmentFundDeactiveReason = GovernmentFundsVM.GovernmentFundDeactiveReason ;
            GovernmentFundBLL.LoginIdentity = UserIdentity;

            Result result = GovernmentFundBLL.Deactivate();
             if (result.EnumMember == GovernmentFundsValidationEnum.RejectedBecauseOfBeforeHiringDate.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationBeforeHiringText.ToString());
            }
            else if (result.EnumMember == GovernmentFundsValidationEnum.RejectedBecauseOfDeactivationDateShouldNotBeLessThanDeductionStartDate.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationBeforeDeductionStartDateText.ToString());
            }
            else if (result.EnumMember == GovernmentFundsValidationEnum.RejectedBecauseOfAlreadyDeactivated.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationAlreadyDeactivatedText.ToString());
            }
            else if (result.EnumMember == GovernmentFundsValidationEnum.RejectedBecauseOfMonthlyDeductionAmountShouldNotGreaterThanTotalDeductionAmount.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationDeductionAmountShouldNotGreaterThenTotalDeductionAmountText);
            }

            return Json(new { GovernmentFundID = GovernmentFundBLL.GovernmentFundID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByGovernmentFundID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("ContractDate,ContractNo,LoanNo,LoanDate,DeductionStartDate,MonthlyDeductionAmount,TotalDeductionAmount,DeactiveDate,LetterDate,LetterNo")]
        public ActionResult Delete(GovernmentFundsViewModel GovernmentFundsVM)
        {
            try
            {
                GovernmentFundsBLL governmentFundBLL = new GovernmentFundsBLL();
                governmentFundBLL.LoginIdentity = UserIdentity;
                governmentFundBLL.Remove(GovernmentFundsVM.GovernmentFundID);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public JsonResult GetGovernmentFunds()
        {
            GovernmentFundsBLL governmentFundsBLL = new GovernmentFundsBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize,
                OrderByColumnName = OrderByColumnName
            };
            var data = governmentFundsBLL.GetGovernmentFunds(out TotalRecordsOut, out RecFilterOut).Select(x => new
            {
                GovernmentFundID = x.GovernmentFundID,
                EmployeeCodeNo = x.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.EmployeeCode.Employee.EmployeeNameAr,
                GovernmentDeductionTypeName = x.GovernmentDeductionType.GovernmentDeductionTypeName,
                GovernmentFundTypeName = x.GovernmentFundType.GovernmentFundTypeName,
                MonthlyDeductionAmount = x.MonthlyDeductionAmount,
                TotalDeductionAmount = x.TotalDeductionAmount,
                DeductionStartDate = x.DeductionStartDate,
                GovernmentFundDeactiveReason = x.GovernmentFundDeactiveReason,
                IsActive = x.IsActive
            });
            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private GovernmentFundsViewModel GetByGovernmentFundID(int id)
        {
            GovernmentFundsBLL GovernmentFundBLL = new GovernmentFundsBLL().GetByGovernmentFundID(id);
            GovernmentFundsViewModel GovernmentFundVM = new GovernmentFundsViewModel();
            if (GovernmentFundBLL != null)
            {
                GovernmentFundVM.GovernmentFundID = GovernmentFundBLL.GovernmentFundID;
                GovernmentFundVM.EmployeeCodeID = GovernmentFundBLL.EmployeeCode.EmployeeCodeID;
                GovernmentFundVM.LoanNo = GovernmentFundBLL.LoanNo;
                GovernmentFundVM.LoanDate = GovernmentFundBLL.LoanDate;
                GovernmentFundVM.DeductionStartDate = GovernmentFundBLL.DeductionStartDate;
                GovernmentFundVM.MonthlyDeductionAmount = GovernmentFundBLL.MonthlyDeductionAmount;
                GovernmentFundVM.TotalDeductionAmount = GovernmentFundBLL.TotalDeductionAmount;
                GovernmentFundVM.RemainingDeductionAmount = GovernmentFundBLL.RemainingDeductionAmount;
                GovernmentFundVM.EmployeeCode = GovernmentFundBLL.EmployeeCode;
                GovernmentFundVM.GovernmentFundType = GovernmentFundBLL.GovernmentFundType;
                GovernmentFundVM.GovernmentDeductionType = GovernmentFundBLL.GovernmentDeductionType;
                GovernmentFundVM.ContractNo = GovernmentFundBLL.ContractNo;
                GovernmentFundVM.BankIPAN = GovernmentFundBLL.BankIPAN;
                GovernmentFundVM.GovernmentFundDeactiveReason = GovernmentFundBLL.GovernmentFundDeactiveReason;
                if (GovernmentFundBLL.KSACity != null)
                {
                    GovernmentFundVM.KSACity = new KSACitiesBLL() { KSACityID = GovernmentFundBLL.KSACity.KSACityID, KSACityName = GovernmentFundBLL.KSACity.KSACityName };
                }
                GovernmentFundVM.SponserToIDNo = GovernmentFundBLL.SponserToIDNo;
                GovernmentFundVM.SponserToName = GovernmentFundBLL.SponserToName;
                GovernmentFundVM.CreatedDate = GovernmentFundBLL.CreatedDate;
                GovernmentFundVM.CreatedBy = GovernmentFundVM.GetCreatedByDisplayed(GovernmentFundBLL.CreatedBy);

                GovernmentFundVM.IsActive = GovernmentFundBLL.IsActive;
                GovernmentFundVM.LetterDate = GovernmentFundBLL.LetterDate;
                GovernmentFundVM.LetterNo = GovernmentFundBLL.LetterNo;
                GovernmentFundVM.Notes = GovernmentFundBLL.Notes;
                GovernmentFundVM.DeactiveDate = GovernmentFundBLL.DeactiveDate;

                GovernmentFundVM.EmployeeVM = new EmployeesViewModel()
                {
                    EmployeeCodeID = GovernmentFundBLL.EmployeeCode.EmployeeCodeID,
                    EmployeeCodeNo = GovernmentFundBLL.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = GovernmentFundBLL.EmployeeCode.Employee.EmployeeNameAr,
                    EmployeeIDNo = GovernmentFundBLL.EmployeeCode.Employee.EmployeeIDNo,
                };

            }
            return GovernmentFundVM;
        }

        [CustomAuthorize]
        public ActionResult PrintGovernmentFund(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.GovernmentFunds.ToString(), id));
        }
        [CustomAuthorize]
        public ActionResult PrintGovernmentFundsDeactivation(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.GovernmentFundsDeactivation.ToString(), id));
        }

        [CustomAuthorize]
        public ActionResult PrintAllGovernmentFundByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.GovernmentFunds.ToString(), id));
        }
    }
}
