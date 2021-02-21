using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExceptionNameSpace;
using System.Data;
using Microsoft.Office.Interop.Excel;
using HCM.Classes.Helpers;
using NPOI.HSSF.UserModel;

namespace HCM.Controllers
{
    public class BasicSalariesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetContractorsBasicSalaries()
        {
            ContractorsBasicSalariesBLL ContractorsBasicSalariesBLL = new ContractorsBasicSalariesBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            };
            var data = ContractorsBasicSalariesBLL.GetContractorsBasicSalaries(out TotalRecordsOut, out RecFilterOut).Select(x => new
            {
                ContractorBasicSalaryID = x.ContractorBasicSalaryID,
                EmployeeCodeNo = x.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.EmployeeCode.Employee.EmployeeNameAr,
                RankCategoryName = x.EmployeeCode.EmployeeCurrentJob.OrganizationJob.Rank.RankCategory.RankCategoryName,
                BasicSalary = x.BasicSalary,
                TransfareAllowance = x.TransfareAllowance,
            });
            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBasicSalaries()
        {
            var data = new BasicSalariesBLL().GetBasicSalaries().Select(x => new
            {
                BasicSalaryID = x.BasicSalaryID,
                RankCategoryName = x.Rank.RankCategory.RankCategoryName,
                RankName = x.Rank.RankName,
                CareerDegreeName = x.CareerDegree.CareerDegreeName,
                BasicSalary = x.BasicSalary,
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportExcelContractorsBasicSalaries()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "ContractorBasicSalaryID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "BasicSalary", Resources.Globalization.BasicSalaryText },
                { "TransfareAllowance", Resources.Globalization.TransfareAllowanceText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetAllContractorsBasicSalaries(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllContractorsBasicSalaries()
        {
            var data = new ContractorsBasicSalariesBLL().GetContractorsBasicSalaries().Select(x => new
            {
                ContractorBasicSalaryID = x.ContractorBasicSalaryID,
                EmployeeCodeNo = x.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.EmployeeCode.Employee.EmployeeNameAr,
                BasicSalary = x.BasicSalary,
                TransfareAllowance = x.TransfareAllowance
            });

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult DetailsContractorsBasicSalaries(int id)
        {
            return View(this.GetByContractorBasicSalaryID(id));
        }

        [CustomAuthorize]
        public ActionResult CreateContractorsBasicSalaries()
        {
            return View(new ContractorsBasicSalariesViewModel() { BasicSalary = 0, TransfareAllowance = 0 });
        }

        [HttpPost]
        public ActionResult CreateContractorsBasicSalaries(ContractorsBasicSalariesViewModel ContractorBasicSalaryVM)
        {
            ContractorsBasicSalariesBLL ContractorBasicSalaryBLL = new ContractorsBasicSalariesBLL();
            ContractorBasicSalaryBLL.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = ContractorBasicSalaryVM.EmployeeCodeID };
            ContractorBasicSalaryBLL.BasicSalary= ContractorBasicSalaryVM.BasicSalary;
            ContractorBasicSalaryBLL.TransfareAllowance= ContractorBasicSalaryVM.TransfareAllowance;
            ContractorBasicSalaryBLL.LoginIdentity = UserIdentity;
            Result result = ContractorBasicSalaryBLL.Add();
           
            if (result.EnumMember == ContractorsBasicSalariesValidationEnum.RejectedBecauseOfEmployeeMustBeContractualEmployee.ToString())
                throw new CustomException(Resources.Globalization.ValidationEmployeeMustBeContractualEmployeeText);

            return Json(new { ContractorBasicSalaryBLL.ContractorBasicSalaryID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult EditContractorsBasicSalaries(int id)
        {
            return View(this.GetByContractorBasicSalaryID(id));
        }

        [HttpPost]
        public ActionResult EditContractorsBasicSalaries(ContractorsBasicSalariesViewModel ContractorBasicSalaryVM)
        {
            ContractorsBasicSalariesBLL ContractorBasicSalaryBLL = new ContractorsBasicSalariesBLL();
            ContractorBasicSalaryBLL.ContractorBasicSalaryID = ContractorBasicSalaryVM.ContractorBasicSalaryID;
            ContractorBasicSalaryBLL.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = ContractorBasicSalaryVM.EmployeeCodeID };
            ContractorBasicSalaryBLL.BasicSalary = ContractorBasicSalaryVM.BasicSalary;
            ContractorBasicSalaryBLL.TransfareAllowance = ContractorBasicSalaryVM.TransfareAllowance;
            ContractorBasicSalaryBLL.LoginIdentity = UserIdentity;

            Result result = ContractorBasicSalaryBLL.Update();
            if (result.EnumMember == GovernmentFundsValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == ContractorsBasicSalariesValidationEnum.RejectedBecauseOfEmployeeMustBeContractualEmployee.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeMustBeContractualEmployeeText);
            }

            return Json(new { ContractorBasicSalaryID = ContractorBasicSalaryBLL.ContractorBasicSalaryID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult DeleteContractorsBasicSalaries(int id)
        {
            return View(this.GetByContractorBasicSalaryID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("BasicSalary,TransfareAllowance")]
        public ActionResult DeleteContractorsBasicSalaries(ContractorsBasicSalariesViewModel ContractorBasicSalaryVM)
        {
            try
            {
                ContractorsBasicSalariesBLL ContractorBasicSalaryBLL = new ContractorsBasicSalariesBLL();
                ContractorBasicSalaryBLL.LoginIdentity = UserIdentity;
                ContractorBasicSalaryBLL.Remove(ContractorBasicSalaryVM.ContractorBasicSalaryID);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [NonAction]
        private ContractorsBasicSalariesViewModel GetByContractorBasicSalaryID(int id)
        {
            ContractorsBasicSalariesBLL ContractorBasicSalaryBLL = new ContractorsBasicSalariesBLL().GetByContractorBasicSalaryID(id);
            ContractorsBasicSalariesViewModel ContractorBasicSalaryVM = new ContractorsBasicSalariesViewModel();
            if (ContractorBasicSalaryBLL != null)
            {
                ContractorBasicSalaryVM.ContractorBasicSalaryID = ContractorBasicSalaryBLL.ContractorBasicSalaryID;
                ContractorBasicSalaryVM.EmployeeCodeID = ContractorBasicSalaryBLL.EmployeeCode.EmployeeCodeID;
                ContractorBasicSalaryVM.BasicSalary = ContractorBasicSalaryBLL.BasicSalary;
                ContractorBasicSalaryVM.TransfareAllowance = ContractorBasicSalaryBLL.TransfareAllowance;
                ContractorBasicSalaryVM.EmployeeCode = ContractorBasicSalaryBLL.EmployeeCode;
                //ContractorBasicSalaryVM.CreatedDate = ContractorBasicSalaryBLL.CreatedDate;
                //ContractorBasicSalaryVM.CreatedBy = ContractorBasicSalaryVM.GetCreatedByDisplayed(ContractorBasicSalaryBLL.CreatedBy);

                ContractorBasicSalaryVM.EmployeeVM = new EmployeesViewModel()
                {
                    EmployeeCodeID = ContractorBasicSalaryBLL.EmployeeCode.EmployeeCodeID,
                    EmployeeCodeNo = ContractorBasicSalaryBLL.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = ContractorBasicSalaryBLL.EmployeeCode.Employee.EmployeeNameAr,
                    EmployeeIDNo = ContractorBasicSalaryBLL.EmployeeCode.Employee.EmployeeIDNo,
                };

            }
            return ContractorBasicSalaryVM;
        }
    }
}