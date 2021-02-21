using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class GovernmentFundsTypesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /GovernmentFundsTypes/Create
        [HttpPost]
        public ActionResult Create(GovernmentFundsTypesViewModel GovernmentFundsTypesVM)
        {
            GovernmentFundsTypesBLL governmentFundsTypesBLL = new GovernmentFundsTypesBLL();
            governmentFundsTypesBLL.GovernmentFundTypeName = GovernmentFundsTypesVM.GovernmentFundTypeName;

            Result result = governmentFundsTypesBLL.Add();
            return View(GovernmentFundsTypesVM);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByGovernmentFundsTypesID(id));
        }

        //
        // POST: /GovernmentFundsTypes/Edit/5
        [HttpPost]
        public ActionResult Edit(GovernmentFundsTypesViewModel GovernmentFundsTypesVM)
        {
            GovernmentFundsTypesBLL governmentFundsTypesBLL = new GovernmentFundsTypesBLL();
            governmentFundsTypesBLL.GovernmentFundTypeID = GovernmentFundsTypesVM.GovernmentFundTypeID;
            governmentFundsTypesBLL.GovernmentFundTypeName = GovernmentFundsTypesVM.GovernmentFundTypeName;
            governmentFundsTypesBLL.Update();
            return View(this.GetByGovernmentFundsTypesID(GovernmentFundsTypesVM.GovernmentFundTypeID));
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByGovernmentFundsTypesID(id));
        }

        //
        // POST: /GovernmentFundsTypes/Delete/5
        [HttpDelete]
        [IgnoreModelProperties("GovernmentFundTypeName")]
        public ActionResult Delete(GovernmentFundsTypesViewModel GovernmentFundsTypesVM)
        {
            GovernmentFundsTypesBLL.Remove(GovernmentFundsTypesVM.GovernmentFundTypeID);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetGovernmentFundsTypes()
        {
            return Json(new { data = new GovernmentFundsTypesBLL().GetGovernmentFundsTypes() }, JsonRequestBehavior.AllowGet);
        }
        [NonAction]
        private GovernmentFundsTypesViewModel GetByGovernmentFundsTypesID(int id)
        {
            GovernmentFundsTypesBLL governmentFundsTypesBLL = new GovernmentFundsTypesBLL().GetByGovernmentFundTypeID(id);
            GovernmentFundsTypesViewModel GovernmentFundsTypesVM = new GovernmentFundsTypesViewModel();
            if (governmentFundsTypesBLL != null)
            {
                GovernmentFundsTypesVM.GovernmentFundTypeID = governmentFundsTypesBLL.GovernmentFundTypeID;
                GovernmentFundsTypesVM.GovernmentFundTypeName = governmentFundsTypesBLL.GovernmentFundTypeName;
            }
            return GovernmentFundsTypesVM;
        }
    }
}
