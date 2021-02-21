using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class RanksTicketsClassesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        //
        // GET: /RanksTicketsClasses/
        [HttpGet]
        public JsonResult GetRanksTicketsClasses()
        {
            return Json(new { data = new RanksTicketsClassesBLL().GetRanksTicketsClasses() }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /RanksTicketsClasses/
        [HttpGet]
        public JsonResult GetRanksTicketsClassesByRankID(int id)
        {
            // id = RankID
            return Json(new { data = new RanksTicketsClassesBLL().GetRanksTicketsClassesByRankID(id) }, JsonRequestBehavior.AllowGet);
        }

        ////
        //// GET: /RanksTicketsClasses/
        //[HttpGet]
        //public JsonResult GetRanksTicketsClasses(int id)
        //{
        //    //int KSARegionID = 1;
        //    return Json(new { data = new RanksTicketsClassesBLL().GetByKSARegionID(id) }, JsonRequestBehavior.AllowGet);
        //}

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["RankTicketClassID"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByRankTicketClassID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("RankID,RankName,TicketClassID,TicketClassName")]
        public ActionResult Delete(RanksTicketsClassesViewModel RankTicketClassVM)
        {
            RanksTicketsClassesBLL.Remove(RankTicketClassVM.RankTicketClassID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(RanksTicketsClassesViewModel RankTicketClassVM)
        {
            RanksTicketsClassesBLL RankTicketClass = new RanksTicketsClassesBLL();
            RankTicketClass.Rank = RankTicketClassVM.Rank;
            RankTicketClass.TicketClass = RankTicketClassVM.TicketClass;
            Result result = RankTicketClass.Add();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                RanksTicketsClassesBLL RankTicketClassEntity = (RanksTicketsClassesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["RankTicketClassID"] = ((RanksTicketsClassesBLL)result.Entity).RankTicketClassID;
                }
            }

            return View(RankTicketClassVM);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByRankTicketClassID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditRankTicketClass(RanksTicketsClassesViewModel RankTicketClassVM)
        {
            RanksTicketsClassesBLL RankTicketClass = new RanksTicketsClassesBLL();
            RankTicketClass.RankTicketClassID = RankTicketClassVM.RankTicketClassID;
            RankTicketClass.Rank = RankTicketClassVM.Rank;
            RankTicketClass.TicketClass = RankTicketClassVM.TicketClass;
            Result result = RankTicketClass.Update();
            if ((System.Type)result.EnumType == typeof(LookupsValidationEnum))
            {
                RanksTicketsClassesBLL RankTicketClassEntity = (RanksTicketsClassesBLL)result.Entity;
                if (result.EnumMember == LookupsValidationEnum.Done.ToString())
                {
                    Session["RankTicketClassID"] = ((RanksTicketsClassesBLL)result.Entity).RankTicketClassID;
                }
            }

            return View(RankTicketClassVM);
        }

        [HttpGet]
        public JsonResult GetRankTicketClassID()
        {
            int? RankTicketClassID = Session["RankTicketClassID"] == null ? (int?)null : int.Parse(Session["RankTicketClassID"].ToString());
            return Json(new { data = RankTicketClassID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private RanksTicketsClassesViewModel GetByRankTicketClassID(int id)
        {
            RanksTicketsClassesBLL RanksTicketsClassesBLL = (new RanksTicketsClassesBLL()).GetByRankTicketClassID(id);
            RanksTicketsClassesViewModel RankTicketClassVM = new RanksTicketsClassesViewModel();
            if (RanksTicketsClassesBLL != null)
            {
                RankTicketClassVM.RankTicketClassID = RanksTicketsClassesBLL.RankTicketClassID;
                RankTicketClassVM.Rank = RanksTicketsClassesBLL.Rank;
                RankTicketClassVM.TicketClass = RanksTicketsClassesBLL.TicketClass;
            }
            return RankTicketClassVM;
        }
    }
}