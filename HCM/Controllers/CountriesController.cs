using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class CountriesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCountries()
        {
            CountriesBLL CountriesBLL = new CountriesBLL();
            return Json(new { data = CountriesBLL.GetCountries() }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["CountryID"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByCountryID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("CountryName")]
        public ActionResult Delete(CountriesViewModel CountryVM)
        {
            CountriesBLL.Remove(CountryVM.CountryID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(CountriesViewModel CountryVM)
        {
            CountriesBLL Country = new CountriesBLL();
            Country.CountryName = CountryVM.CountryName;
            Result result = Country.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                CountriesBLL CountryEntity = (CountriesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["CountryID"] = ((CountriesBLL)result.Entity).CountryID;
                }
            }

            return View(CountryVM);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByCountryID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditCountry(CountriesViewModel CountryVM)
        {
            CountriesBLL Country = new CountriesBLL();
            Country.CountryID = CountryVM.CountryID;
            Country.CountryName = CountryVM.CountryName;
            Result result = Country.Update();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                CountriesBLL CountryEntity = (CountriesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["CountryID"] = ((CountriesBLL)result.Entity).CountryID;
                }
            }

            return View(CountryVM);
        }

        [HttpGet]
        public JsonResult GetCountryID()
        {
            int? CountryID = Session["CountryID"] == null ? (int?)null : int.Parse(Session["CountryID"].ToString());
            return Json(new { data = CountryID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private CountriesViewModel GetByCountryID(int id)
        {
            CountriesBLL CountriesBLL = new HCMBLL.CountriesBLL();
            CountriesBLL = CountriesBLL.GetByCountryID(id);
            CountriesViewModel CountryVM = new CountriesViewModel();
            if (CountriesBLL != null)
            {
                CountryVM.CountryID = CountriesBLL.CountryID;
                CountryVM.CountryName = CountriesBLL.CountryName;
            }
            return CountryVM;
        }
    }
}