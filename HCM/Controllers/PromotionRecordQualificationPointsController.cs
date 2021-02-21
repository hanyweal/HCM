using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Linq;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public partial class PromotionRecordQualificationPointsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [IgnoreModelProperties("PromotionRecord,PromotionRecordQualificationKindList")]
        public ActionResult UpdateQualificationKind(PromotionsRecordsQualificationPointsViewModel PromotionRecordQualificationPointVM)
        {
            Result result = new Result();
            PromotionsRecordsQualificationsPointsBLL pp = new PromotionsRecordsQualificationsPointsBLL()
            {
                PromotionRecordQualificationPointID = PromotionRecordQualificationPointVM.PromotionRecordQualificationPointID,
                QualificationDegree = PromotionRecordQualificationPointVM.QualificationDegree,
                Qualification = PromotionRecordQualificationPointVM.Qualification,
                GeneralSpecialization = PromotionRecordQualificationPointVM.GeneralSpecialization,
                PromotionRecordQualificationKind = PromotionRecordQualificationPointVM.PromotionRecordQualificationKind,
                LoginIdentity = this.UserIdentity
            };
            result = pp.UpdateQualificationKindAndPoints();

            if (result.EnumMember == PromotionsRecordsQualificationsPointsValidationEnum.Done.ToString())
            {
                pp.Points = ((PromotionsRecordsQualificationsPointsBLL)result.Entity).Points;
            }
            //else if (result.EnumMember == PromotionsRecordsQualificationsPointsValidationEnum.RejectedBecauseOf .ToString())
            //{
            //    throw new CustomException(@Resources.Globalization.Validation..Text.ToString());
            //} 
            //return Json(PromotionRecordQualificationPointVM);
            return Json(new { PromotionRecordQualificationPointID = pp.PromotionRecordQualificationPointID, Points = pp.Points }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPromotionsRecordsQualificationsKinds()
        {
            return Json(new { data = new PromotionsRecordsQualificationsKindsBLL().GetPromotionsRecordsQualificationsKinds() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetByPromotionRecordID(int id)
        {
            var list = new PromotionsRecordsQualificationsPointsBLL().GetByPromotionRecordID(id)
                .Select(item => new
                {
                    item.PromotionRecordQualificationPointID,
                    PromotionRecordStatusID = item.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusID,
                    QualificationDegreeID = item.QualificationDegree != null ? item.QualificationDegree.QualificationDegreeID : 0,
                    QualificationID = item.Qualification != null ? item.Qualification.QualificationID : 0,
                    GeneralSpecializationID = item.GeneralSpecialization != null ? item.GeneralSpecialization.GeneralSpecializationID : 0,
                    QualificationDegreeName = item.QualificationDegree != null ? item.QualificationDegree.QualificationDegreeName : "",
                    QualificationName = item.Qualification != null ? item.Qualification.QualificationName : "",
                    GeneralSpecializationName = item.GeneralSpecialization != null ? item.GeneralSpecialization.GeneralSpecializationName : "",
                    Points = item.Points,
                    PromotionRecordQualificationKindID = item.PromotionRecordQualificationKind != null ? item.PromotionRecordQualificationKind.PromotionRecordQualificationKindID : 0,
                    PromotionRecordQualificationKindName = item.PromotionRecordQualificationKind != null ? item.PromotionRecordQualificationKind.PromotionRecordQualificationKindName : "",
                    //PromotionRecordQualificationKindList = new PromotionsRecordsQualificationsKindsBLL().GetPromotionsRecordsQualificationsKinds()
                });
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
            /*
             * With Server side paging
            var list = new PromotionsRecordsQualificationsPointsBLL()
            {
                Search = Search,
                Order = Order,
                OrderByColumnName = OrderByColumnName,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            }.GetByPromotionRecordID(id, out TotalRecordsOut, out RecFilterOut)
            .Select(item => new PromotionsRecordsQualificationPointsViewModel()
            {                
                PromotionRecordQualificationPointID = item.PromotionRecordQualificationPointID,
                PromotionRecord = item.PromotionRecord,
                QualificationDegree = item.QualificationDegree,
                Qualification = item.Qualification,
                GeneralSpecialization = item.GeneralSpecialization,
                Points = item.Points,
                PromotionRecordQualificationKind = item.PromotionRecordQualificationKind,
                //PromotionRecordQualificationKindList = new PromotionsRecordsQualificationsKindsBLL().GetPromotionsRecordsQualificationsKinds()
            });
            //.Select(item => new
            // {
            //     item.PromotionRecordQualificationPointID,
            //     PromotionRecordStatusID = item.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusID,
            //     QualificationDegreeID = item.QualificationDegree != null ? item.QualificationDegree.QualificationDegreeID : 0,
            //     QualificationID = item.Qualification != null ? item.Qualification.QualificationID : 0,
            //     GeneralSpecializationID = item.GeneralSpecialization != null ? item.GeneralSpecialization.GeneralSpecializationID : 0,
            //     QualificationDegreeName = item.QualificationDegree != null ? item.QualificationDegree.QualificationDegreeName : "",
            //     QualificationName = item.Qualification != null ? item.Qualification.QualificationName : "",
            //     GeneralSpecializationName = item.GeneralSpecialization != null ? item.GeneralSpecialization.GeneralSpecializationName : "",
            //     Points = item.Points,
            //     PromotionRecordQualificationKindID = item.PromotionRecordQualificationKind != null ? item.PromotionRecordQualificationKind.PromotionRecordQualificationKindID : 0,
            //     PromotionRecordQualificationKindName = item.PromotionRecordQualificationKind != null ? item.PromotionRecordQualificationKind.PromotionRecordQualificationKindName : "",
            //     PromotionRecordQualificationKindList = new PromotionsRecordsQualificationsKindsBLL().GetPromotionsRecordsQualificationsKinds()
            // });
            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = list }, JsonRequestBehavior.AllowGet);
            */
        }
    }
}