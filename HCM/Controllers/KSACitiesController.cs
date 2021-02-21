using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class KSACitiesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        //
        // GET: /KSACities/
        [HttpGet]
        public JsonResult GetKSACitiesAll()
        {
            //int KSARegionID = 1;
            return Json(new { data = new KSACitiesBLL().GetKSACities() }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /KSACities/
        [HttpGet]
        public JsonResult GetKSACities(int id)
        {
            //int KSARegionID = 1;
            return Json(new { data = new KSACitiesBLL().GetByKSARegionID(id) }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["KSACityID"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByKSACityID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("KSACityName,KSARegion,KSARegionID,KSARegionName")]
        public ActionResult Delete(KSACitiesViewModel KSACityVM)
        {
            KSACitiesBLL.Remove(KSACityVM.KSACityID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        //[IgnoreModelProperties("KSACityDetailRequest")]
        public ActionResult Create(KSACitiesViewModel KSACityVM)
        {
            KSACitiesBLL KSACity = new KSACitiesBLL();
            KSACity.KSACityName = KSACityVM.KSACityName;
            KSACity.KSARegion = new KSARegionsBLL() { KSARegionID = KSACityVM.KSARegion.KSARegionID };
            Result result = KSACity.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                KSACitiesBLL KSACityEntity = (KSACitiesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["KSACityID"] = ((KSACitiesBLL)result.Entity).KSACityID;
                }
            }

            return View(KSACityVM);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByKSACityID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditKSACity(KSACitiesViewModel KSACityVM)
        {
            KSACitiesBLL KSACity = new KSACitiesBLL();
            KSACity.KSACityID = KSACityVM.KSACityID;
            KSACity.KSACityName = KSACityVM.KSACityName;
            KSACity.KSARegion = new KSARegionsBLL() { KSARegionID = KSACityVM.KSARegion.KSARegionID };
            Result result = KSACity.Update();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                KSACitiesBLL KSACityEntity = (KSACitiesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["KSACityID"] = ((KSACitiesBLL)result.Entity).KSACityID;
                }
            }

            return View(KSACityVM);
        }

        [HttpGet]
        public JsonResult GetKSACityID()
        {
            int? KSACityID = Session["KSACityID"] == null ? (int?)null : int.Parse(Session["KSACityID"].ToString());
            return Json(new { data = KSACityID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private KSACitiesViewModel GetByKSACityID(int id)
        {
            KSACitiesBLL KSACitiesBLL = (new KSACitiesBLL()).GetByKSACityID(id);
            KSACitiesViewModel KSACityVM = new KSACitiesViewModel();
            if (KSACitiesBLL != null)
            {
                KSACityVM.KSACityID = KSACitiesBLL.KSACityID;
                KSACityVM.KSACityName = KSACitiesBLL.KSACityName;
                KSACityVM.KSARegion = KSACitiesBLL.KSARegion;
            }
            return KSACityVM;
        }
    }
}