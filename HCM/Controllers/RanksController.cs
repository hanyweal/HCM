using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCMBLL;
using HCMBLL.Enums;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class RanksController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }
        //
        // GET: /GetRanks/
        [HttpGet]
        public JsonResult GetRanks()
        {
            return Json(new { data = new RanksBLL().GetRanks() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("Ranks/UpdateIsPromotion/{RankIDs}")]
        public ActionResult UpdateIsPromotion(string RankIDs)
        {
            RanksBLL RankBLL = new RanksBLL();
            RankIDs = RankIDs == "0" ? "" : RankIDs;
            RankBLL.LoginIdentity = UserIdentity;
            Result result = RankBLL.UpdateIsPromotion(RankIDs);
            if ((System.Type)result.EnumType == typeof(RanksValidationEnum))
            {
                if (result.EnumMember == RanksValidationEnum.Done.ToString())
                {
                }
                //else if (result.EnumMember == RanksValidationEnum.RejectedBecauseOfNoCheckBoxSelected.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationRankNoCheckBoxSelectedText);
                //}
                else if (result.EnumMember == RanksValidationEnum.RejectedBecauseOfCheckBoxMultipleSelected.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationRankMultiCheckBoxSelectedText);
                }
            }

            return RedirectToAction("Index");// View();
        }

        [CustomAuthorize]
        public ActionResult PrintJobVacanciesByRankID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/JobVacancies.aspx?ID={0}", id));
        }



        //[CustomAuthorize]
        //public ActionResult Create()
        //{
        //    Session["RankID"] = null;
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Create(RanksViewModel RanksVM)
        //{
        //    RanksBLL RankBLL = new RanksBLL();
        //    RankBLL.RankName = RanksVM.RankName;
        //    RankBLL.RankCategory = new RanksCategoriesBLL() { RankCategoryID = RanksVM.RankCategory.RankCategoryID };
        //    Result result = RankBLL.Add();
        //    if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
        //    {
        //        RanksBLL RanksEntity = (RanksBLL)result.Entity;
        //        if (result.EnumMember == LookupsValidationEnum.Done.ToString())
        //        {
        //            Session["RankID"] = ((RanksBLL)result.Entity).RankID;
        //        }
        //    }
        //    return View(RanksVM);
        //}

        //[CustomAuthorize]
        //public ActionResult Edit(int id)
        //{
        //    return View(this.GetByRankID(id));
        //}

        //[HttpPost]
        //[ActionName("Edit")]
        //public ActionResult EditRanks(RanksViewModel RanksVM)
        //{
        //    RanksBLL RankBLL = new RanksBLL();
        //    RankBLL.RankID = RanksVM.RankID;
        //    RankBLL.RankName = RanksVM.RankName;
        //    RankBLL.RankCategory = new RanksCategoriesBLL() { RankCategoryID = RanksVM.RankCategory.RankCategoryID };
        //    Result result = RankBLL.Update();
        //    if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
        //    {
        //        KSACitiesBLL KSACityEntity = (KSACitiesBLL)result.Entity;
        //        if (result.EnumMember == LookupsValidationEnum.Done.ToString())
        //        {
        //            Session["KSACityID"] = ((KSACitiesBLL)result.Entity).KSACityID;
        //        }
        //    }

        //    return View(RanksVM);
        //}

        //[NonAction]
        //private RanksViewModel GetByRankID(int id)
        //{
        //    RanksBLL RankBLL = (new RanksBLL()).GetByRankID(id);
        //    RanksViewModel RanksVM = new RanksViewModel();
        //    if (RankBLL != null)
        //    {
        //        RanksVM.RankID = RankBLL.RankID;
        //        RanksVM.RankName = RankBLL.RankName;
        //        RanksVM.RankCategory = RankBLL.RankCategory;
        //    }
        //    return RanksVM;
        //}

    }
}