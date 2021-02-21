using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class EmployeesDegreesUpgradesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        //
        // GET: /EmployeesDegreesUpgrades/
        [HttpGet]
        public JsonResult GetEmployeesDegreesUpgrades()
        {
            return Json(new { data = new EmployeesDegreesUpgradesBLL().GetEmployeesDegreesUpgrades() }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["EmployeeDegreeUpgradeID"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmployeesDegreesUpgradesViewModel EmployeeDegreeUpgradeVM)
        {
            EmployeesDegreesUpgradesBLL EmployeeDegreeUpgrade = new EmployeesDegreesUpgradesBLL();
            EmployeeDegreeUpgrade.EmployeeDegreeUpgradeYear = EmployeeDegreeUpgradeVM.EmployeeDegreeUpgradeYear;
            EmployeeDegreeUpgrade.LoginIdentity = UserIdentity;

            Result result = EmployeeDegreeUpgrade.Add();
            if ((System.Type)result.EnumType == typeof(EmployeesDegreesUpgradesValidationEnum))
            {
                EmployeesDegreesUpgradesBLL EmployeeDegreeUpgradeEntity = (EmployeesDegreesUpgradesBLL)result.Entity;
                if (result.EnumMember == EmployeesDegreesUpgradesValidationEnum.Done.ToString())
                {
                    Session["EmployeeDegreeUpgradeID"] = ((EmployeesDegreesUpgradesBLL)result.Entity).EmployeeDegreeUpgradeID;
                }
                else if (result.EnumMember == EmployeesDegreesUpgradesValidationEnum.RejectedAlreadyExists.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEmployeesDegreesUpgradesAlreadyExistsText);
                }
            }

            return View(EmployeeDegreeUpgradeVM);
        }

        [NonAction]
        public List<int> GetYears()
        {
            List<int> Years = new List<int>();
            for (int i = DateTime.Now.Year - 2; i < DateTime.Now.Year + 5; i++)
                Years.Add(i);

            return Years;
        }
    }
}