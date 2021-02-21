using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class AllowancesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Allowances/
        [HttpGet]
        public JsonResult GetAllowances()
        {
            return Json(new { data = new AllowancesBLL().GetAllowances() }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Allowances/
        [HttpGet]
        public JsonResult GetAllowancesByAmountTypeID(int id)
        {
            return Json(new { data = new AllowancesBLL().GetByAllowanceAmountTypeID(id) }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["AllowanceID"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByAllowanceID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("AllowanceName,AllowanceAmountType,AllowanceAmount,IsActive,AllowanceAmountTypeID,AllowanceAmountTypeName,AllowanceCalculationType,AllowanceCalculationTypeID,AllowanceCalculationTypeName")]
        public ActionResult Delete(AllowancesViewModel AllowanceVM)
        {
            AllowancesBLL allowancesBLL = new AllowancesBLL();
            allowancesBLL.LoginIdentity = UserIdentity;
            allowancesBLL.Remove(AllowanceVM.AllowanceID.Value);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(AllowancesViewModel AllowanceVM)
        {
            AllowancesBLL Allowance = new AllowancesBLL();
            Allowance.AllowanceName = AllowanceVM.AllowanceName;
            Allowance.AllowanceAmount = AllowanceVM.AllowanceAmount;
            Allowance.LoginIdentity = UserIdentity;
            Allowance.AllowanceAmountType = new AllowancesAmountTypesBLL() { AllowanceAmountTypeID = AllowanceVM.AllowanceAmountType.AllowanceAmountTypeID };
            Allowance.AllowanceCalculationType = new AllowancesCalculationTypesBLL() { AllowanceCalculationTypeID = AllowanceVM.AllowanceCalculationType.AllowanceCalculationTypeID };

            Result result = Allowance.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                AllowancesBLL AllowanceEntity = (AllowancesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["AllowanceID"] = ((AllowancesBLL)result.Entity).AllowanceID;
                }
            }

            return View(AllowanceVM);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByAllowanceID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditAllowance(AllowancesViewModel AllowanceVM)
        {
            AllowancesBLL Allowance = new AllowancesBLL();
            Allowance.AllowanceID = AllowanceVM.AllowanceID.Value;
            Allowance.AllowanceName = AllowanceVM.AllowanceName;
            Allowance.AllowanceAmount = AllowanceVM.AllowanceAmount;
            Allowance.IsActive = AllowanceVM.IsActive;
            Allowance.LoginIdentity = UserIdentity;
            Allowance.AllowanceAmountType = new AllowancesAmountTypesBLL() { AllowanceAmountTypeID = AllowanceVM.AllowanceAmountType.AllowanceAmountTypeID };
            Allowance.AllowanceCalculationType = new AllowancesCalculationTypesBLL() { AllowanceCalculationTypeID = AllowanceVM.AllowanceCalculationType.AllowanceCalculationTypeID };

            Result result = Allowance.Update();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                AllowancesBLL AllowanceEntity = (AllowancesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["AllowanceID"] = ((AllowancesBLL)result.Entity).AllowanceID;
                }
            }

            return View(AllowanceVM);
        }

        [HttpGet]
        public JsonResult GetAllowanceID()
        {
            int? AllowanceID = Session["AllowanceID"] == null ? (int?)null : int.Parse(Session["AllowanceID"].ToString());
            return Json(new { data = AllowanceID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private AllowancesViewModel GetByAllowanceID(int id)
        {
            AllowancesBLL AllowancesBLL = (new AllowancesBLL()).GetByAllowanceID(id);
            AllowancesViewModel AllowanceVM = new AllowancesViewModel();
            if (AllowancesBLL != null)
            {
                AllowanceVM.AllowanceID = AllowancesBLL.AllowanceID;
                AllowanceVM.AllowanceName = AllowancesBLL.AllowanceName;
                AllowanceVM.AllowanceAmount = AllowancesBLL.AllowanceAmount;
                AllowanceVM.IsActive = AllowancesBLL.IsActive;
                AllowanceVM.AllowanceAmountType = AllowancesBLL.AllowanceAmountType;
                AllowanceVM.AllowanceCalculationType = AllowancesBLL.AllowanceCalculationType;
            }
            return AllowanceVM;
        }
    }
}