using ExceptionNameSpace;
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
    public class QualificationsController : BaseController
    {
        [HttpGet]
        [Route("Qualifications/GetQualifications/{id}")]
        public JsonResult GetQualifications(int id)
        {
            return Json(new { data = new QualificationsBLL().GetByQualificationDegreeID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("Qualifications/GetGeneralSpecializations/{id}")]
        public JsonResult GetGeneralSpecializations(int id)
        {
            return Json(new { data = new GeneralSpecializationsBLL().GetByQualificationID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("Qualifications/GetExactSpecializations/{id}")]
        public JsonResult GetExactSpecializations(int id)
        {
            return Json(new { data = new ExactSpecializationsBLL().GetByGeneralSpecializationID(id) }, JsonRequestBehavior.AllowGet);
        }

        [Route("Qualifications/GetQualificationsDegrees")]
        public JsonResult GetQualificationsDegrees()
        {
            return Json(new
            {
                data = ((new QualificationsDegreesBLL().GetQualificationsDegrees())).Select(x => new
                {
                    QualificationDegreeID = x.QualificationDegreeID,
                    QualificationDegreeName = x.QualificationDegreeName,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("Qualifications/GetQualifications")]
        public JsonResult GetQualifications()
        {
            return Json(new
            {
                data = ((new QualificationsBLL().GetQualifications())).Select(x => new
                {
                    QualificationID = x.QualificationID,
                    QualificationName = x.QualificationName,
                    DirectPoints = x.DirectPoints,
                    IndirectPoints = x.IndirectPoints,
                    QualificationDegreeName = x.QualificationDegree.QualificationDegreeName,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("Qualifications/GetGeneralSpecializations")]
        public JsonResult GetGeneralSpecializations()
        {
            return Json(new
            {
                data = ((new GeneralSpecializationsBLL().GetGeneralSpecializations())).Select(x => new
                {
                    GeneralSpecializationID = x.GeneralSpecializationID,
                    GeneralSpecializationName = x.GeneralSpecializationName,
                    DirectPoints = x.DirectPoints,
                    IndirectPoints = x.IndirectPoints,
                    QualificationName = x.Qualification.QualificationName,
                    QualificationDegreeName = x.Qualification.QualificationDegree.QualificationDegreeName,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("Qualifications/GetExactSpecializations")]
        public JsonResult GetExactSpecializations()
        {
            return Json(new
            {
                data = ((new ExactSpecializationsBLL().GetExactSpecializations())).Select(x => new
                {
                    ExactSpecializationID = x.ExactSpecializationID,
                    ExactSpecializationName = x.ExactSpecializationName,
                    GeneralSpecializationName = x.GeneralSpecialization.GeneralSpecializationName,
                    QualificationName = x.GeneralSpecialization.Qualification.QualificationName,
                    QualificationDegreeName = x.GeneralSpecialization.Qualification.QualificationDegree.QualificationDegreeName,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [NonAction]
        private QualificationsDegreesViewModel GetByQualificationDegreeID(int id)
        {
            QualificationsDegreesBLL QualificationDegreeBLL = new QualificationsDegreesBLL().GetByQualificationDegreeID(id);
            QualificationsDegreesViewModel QualificationsDegreesVM = new QualificationsDegreesViewModel();
            if (QualificationDegreeBLL != null)
            {
                QualificationsDegreesVM.QualificationDegreeID = QualificationDegreeBLL.QualificationDegreeID;
                QualificationsDegreesVM.QualificationDegreeName = QualificationDegreeBLL.QualificationDegreeName;
            }
            return QualificationsDegreesVM;
        }

        [CustomAuthorize]
        public ActionResult CreateQualificationDegree()
        {
            return View(new QualificationsDegreesViewModel());
        }

        [HttpPost]
        public ActionResult CreateQualificationDegree(QualificationsDegreesViewModel QualificationsDegreesVM)
        {
            QualificationsDegreesBLL QualificationDegrees = new QualificationsDegreesBLL();
            QualificationDegrees.QualificationDegreeName = QualificationsDegreesVM.QualificationDegreeName;
            QualificationDegrees.LoginIdentity = UserIdentity;
            Result result = QualificationDegrees.Add();
            if (result.EnumMember == QualificationsDegreesValidationEnum.Done.ToString())
            {

            }
            return Json(new { QualificationDegreeID = QualificationDegrees.QualificationDegreeID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult EditQualificationDegree(int id)
        {
            return View(this.GetByQualificationDegreeID(id));
        }

        [HttpPost]
        public ActionResult EditQualificationDegree(QualificationsDegreesViewModel QualificationsDegreesVM)
        {
            QualificationsDegreesBLL QualificationDegrees = new QualificationsDegreesBLL();
            QualificationDegrees.QualificationDegreeID = QualificationsDegreesVM.QualificationDegreeID;
            QualificationDegrees.QualificationDegreeName = QualificationsDegreesVM.QualificationDegreeName;
            QualificationDegrees.LoginIdentity = UserIdentity;
            Result result = QualificationDegrees.Update();
            if (result.EnumMember == QualificationsDegreesValidationEnum.Done.ToString())
            {

            }
            return Json(new { QualificationDegreeID = QualificationDegrees.QualificationDegreeID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult DeleteQualificationDegree(int id)
        {
            return View(this.GetByQualificationDegreeID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("QualificationDegreeName")]
        public ActionResult DeleteQualificationDegree(QualificationsDegreesViewModel QualificationsDegreesVM)
        {
            QualificationsDegreesBLL QualificationDegrees = new QualificationsDegreesBLL();
            QualificationDegrees.QualificationDegreeID = QualificationsDegreesVM.QualificationDegreeID;
            QualificationDegrees.LoginIdentity = UserIdentity;
            Result result = QualificationDegrees.Remove(QualificationDegrees.QualificationDegreeID);
            if (result.EnumMember == QualificationsDegreesValidationEnum.Done.ToString())
            {

            }
            return Json(new { QualificationDegreeID = QualificationDegrees.QualificationDegreeID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private QualificationsViewModel GetByQualificationID(int id)
        {
            QualificationsBLL QualificationBLL = new QualificationsBLL().GetByQualificationID(id);
            QualificationsViewModel QualificationVM = new QualificationsViewModel();
            if (QualificationBLL != null)
            {
                QualificationVM.QualificationID = QualificationBLL.QualificationID;
                QualificationVM.QualificationName = QualificationBLL.QualificationName;
                QualificationVM.DirectPoints = QualificationBLL.DirectPoints;
                QualificationVM.IndirectPoints = QualificationBLL.IndirectPoints;
                QualificationVM.QualificationDegreeID = QualificationBLL.QualificationDegree.QualificationDegreeID; //new QualificationsDegreesBLL() { QualificationDegreeID = QualificationBLL.QualificationDegree.QualificationDegreeID };
                QualificationVM.QualificationDegreeName = QualificationBLL.QualificationDegree.QualificationDegreeName;
            }
            return QualificationVM;
        }

        [CustomAuthorize]
        public ActionResult CreateQualification()
        {
            return View(new QualificationsViewModel());
        }

        [HttpPost]
        public ActionResult CreateQualification(QualificationsViewModel QualificationVM)
        {
            QualificationsBLL QualificationBLL = new QualificationsBLL();
            QualificationBLL.QualificationName = QualificationVM.QualificationName;
            QualificationBLL.DirectPoints = QualificationVM.DirectPoints;
            QualificationBLL.IndirectPoints = QualificationVM.IndirectPoints;
            QualificationBLL.QualificationDegree = new QualificationsDegreesBLL() { QualificationDegreeID = QualificationVM.QualificationDegreeID };
            QualificationBLL.LoginIdentity = UserIdentity;
            Result result = QualificationBLL.Add();
            if (result.EnumMember == QualificationsValidationEnum.Done.ToString())
            {

            }
            return Json(new { QualificationID = QualificationBLL.QualificationID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult EditQualification(int id)
        {
            return View(this.GetByQualificationID(id));
        }

        [HttpPost]
        public ActionResult EditQualification(QualificationsViewModel QualificationVM)
        {
            QualificationsBLL QualificationBLL = new QualificationsBLL();
            QualificationBLL.QualificationID = QualificationVM.QualificationID;
            QualificationBLL.QualificationName = QualificationVM.QualificationName;
            QualificationBLL.DirectPoints = QualificationVM.DirectPoints;
            QualificationBLL.IndirectPoints = QualificationVM.IndirectPoints;
            QualificationBLL.QualificationDegree = new QualificationsDegreesBLL() { QualificationDegreeID = QualificationVM.QualificationDegreeID };
            QualificationBLL.LoginIdentity = UserIdentity;
            Result result = QualificationBLL.Update();
            if (result.EnumMember == QualificationsValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == QualificationsValidationEnum.RejectedBecauseOfThisQualificationExistsInEmployeesQualifications.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationThisQualificationExistsInEmployeesQualificationsText);
            }
            else if (result.EnumMember == QualificationsValidationEnum.RejectedBecauseOfThisQualificationExistsInJobsCategoriesQualifications.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationThisQualificationExistsInJobsCategoriesQualificationsText);
            }
            return Json(new { QualificationID = QualificationBLL.QualificationID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult DeleteQualification(int id)
        {
            return View(this.GetByQualificationID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("QualificationName,QualificationDegree,DirectPoints,IndirectPoints")]
        public ActionResult DeleteQualification(QualificationsViewModel QualificationVM)
        {
            QualificationsBLL QualificationBLL = new QualificationsBLL();
            QualificationBLL.QualificationID = QualificationVM.QualificationID;
            QualificationBLL.LoginIdentity = UserIdentity;
            Result result = QualificationBLL.Remove(QualificationBLL.QualificationID);
            if (result.EnumMember == QualificationsValidationEnum.Done.ToString())
            {

            }
            return Json(new { QualificationID = QualificationBLL.QualificationID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private GeneralSpecializationsViewModel GetByGeneralSpecializationID(int id)
        {
            GeneralSpecializationsBLL GeneralSpecializationBLL = new GeneralSpecializationsBLL().GetByGeneralSpecializationID(id);
            GeneralSpecializationsViewModel GeneralSpecializationVM = new GeneralSpecializationsViewModel();
            if (GeneralSpecializationBLL != null)
            {
                GeneralSpecializationVM.GeneralSpecializationID = GeneralSpecializationBLL.GeneralSpecializationID;
                GeneralSpecializationVM.GeneralSpecializationName = GeneralSpecializationBLL.GeneralSpecializationName;
                GeneralSpecializationVM.DirectPoints = GeneralSpecializationBLL.DirectPoints;
                GeneralSpecializationVM.IndirectPoints = GeneralSpecializationBLL.IndirectPoints;
                GeneralSpecializationVM.QualificationID = GeneralSpecializationBLL.Qualification.QualificationID;
                GeneralSpecializationVM.QualificationDegreeID = GeneralSpecializationBLL.Qualification.QualificationDegree.QualificationDegreeID; //new QualificationsDegreesBLL() { QualificationDegreeID = GeneralSpecializationBLL.Qualification.QualificationID };
                GeneralSpecializationVM.QualificationName = GeneralSpecializationBLL.Qualification.QualificationName;
                GeneralSpecializationVM.QualificationDegreeName = GeneralSpecializationBLL.Qualification.QualificationDegree.QualificationDegreeName;
            }
            return GeneralSpecializationVM;
        }

        [CustomAuthorize]
        public ActionResult CreateGeneralSpecialization()
        {
            return View(new GeneralSpecializationsViewModel());
        }

        [HttpPost]
        public ActionResult CreateGeneralSpecialization(GeneralSpecializationsViewModel GeneralSpecializationVM)
        {
            GeneralSpecializationsBLL GeneralSpecializationBLL = new GeneralSpecializationsBLL();
            GeneralSpecializationBLL.GeneralSpecializationName = GeneralSpecializationVM.GeneralSpecializationName;
            GeneralSpecializationBLL.DirectPoints = GeneralSpecializationVM.DirectPoints;
            GeneralSpecializationBLL.IndirectPoints = GeneralSpecializationVM.IndirectPoints;
            GeneralSpecializationBLL.Qualification = new QualificationsBLL() { QualificationID = GeneralSpecializationVM.QualificationID };
            GeneralSpecializationBLL.LoginIdentity = UserIdentity;
            Result result = GeneralSpecializationBLL.Add();
            if (result.EnumMember == GeneralSpecializationsValidationEnum.Done.ToString())
            {

            }
            return Json(new { GeneralSpecializationID = GeneralSpecializationBLL.GeneralSpecializationID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult EditGeneralSpecialization(int id)
        {
            return View(this.GetByGeneralSpecializationID(id));
        }

        [HttpPost]
        public ActionResult EditGeneralSpecialization(GeneralSpecializationsViewModel GeneralSpecializationVM)
        {
            GeneralSpecializationsBLL GeneralSpecializationBLL = new GeneralSpecializationsBLL();
            GeneralSpecializationBLL.GeneralSpecializationID = GeneralSpecializationVM.GeneralSpecializationID;
            GeneralSpecializationBLL.GeneralSpecializationName = GeneralSpecializationVM.GeneralSpecializationName;
            GeneralSpecializationBLL.DirectPoints = GeneralSpecializationVM.DirectPoints;
            GeneralSpecializationBLL.IndirectPoints = GeneralSpecializationVM.IndirectPoints;
            GeneralSpecializationBLL.Qualification = new QualificationsBLL() { QualificationID = GeneralSpecializationVM.QualificationID };
            GeneralSpecializationBLL.LoginIdentity = UserIdentity;
            Result result = GeneralSpecializationBLL.Update();
            if (result.EnumMember == GeneralSpecializationsValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == GeneralSpecializationsValidationEnum.RejectedBecauseOfThisGeneralSpecializationExistsInEmployeesQualifications.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationThisGeneralSpecializationExistsInEmployeesQualificationsText);
            }
            else if (result.EnumMember == GeneralSpecializationsValidationEnum.RejectedBecauseOfThisGeneralSpecializationExistsInJobsCategoriesQualifications.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationThisGeneralSpecializationExistsInJobsCategoriesQualificationsText);
            }
            return Json(new { GeneralSpecializationID = GeneralSpecializationBLL.GeneralSpecializationID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult DeleteGeneralSpecialization(int id)
        {
            return View(this.GetByGeneralSpecializationID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("GeneralSpecializationName,Qualification,DirectPoints,IndirectPoints")]
        public ActionResult DeleteGeneralSpecialization(GeneralSpecializationsViewModel GeneralSpecializationVM)
        {
            GeneralSpecializationsBLL GeneralSpecializationBLL = new GeneralSpecializationsBLL();
            GeneralSpecializationBLL.GeneralSpecializationID = GeneralSpecializationVM.GeneralSpecializationID;
            GeneralSpecializationBLL.LoginIdentity = UserIdentity;
            Result result = GeneralSpecializationBLL.Remove(GeneralSpecializationBLL.GeneralSpecializationID);
            if (result.EnumMember == GeneralSpecializationsValidationEnum.Done.ToString())
            {

            }
            return Json(new { GeneralSpecializationID = GeneralSpecializationBLL.GeneralSpecializationID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult CreateExactSpecialization()
        {
            return View(new ExactSpecializationsViewModel());
        }

        [HttpPost]
        public ActionResult CreateExactSpecialization(ExactSpecializationsViewModel ExactSpecializationsViewModel)
        {
            ExactSpecializationsBLL ExactSpecializationBLL = new ExactSpecializationsBLL();
            ExactSpecializationBLL.ExactSpecializationName = ExactSpecializationsViewModel.ExactSpecializationName;
            ExactSpecializationBLL.GeneralSpecialization = new GeneralSpecializationsBLL() { GeneralSpecializationID = ExactSpecializationsViewModel.GeneralSpecializationID };
            ExactSpecializationBLL.LoginIdentity = UserIdentity;
            Result result = ExactSpecializationBLL.Add();
            if (result.EnumMember == ExactSpecializationsValidationEnum.Done.ToString())
            {

            }
            return Json(new { ExactSpecializationID = ExactSpecializationBLL.ExactSpecializationID }, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize]
        public ActionResult EditExactSpecialization(int id)
        {
            return View(this.GetByExactSpecializationID(id));
        }

        [HttpPost]
        public ActionResult EditExactSpecialization(ExactSpecializationsViewModel ExactSpecializationsVM)
        {
            ExactSpecializationsBLL ExactSpecializationBLL = new ExactSpecializationsBLL();
            ExactSpecializationBLL.ExactSpecializationID = ExactSpecializationsVM.ExactSpecializationID;
            ExactSpecializationBLL.ExactSpecializationName = ExactSpecializationsVM.ExactSpecializationName;
            ExactSpecializationBLL.GeneralSpecialization = new GeneralSpecializationsBLL() { GeneralSpecializationID = ExactSpecializationsVM.GeneralSpecializationID };
            ExactSpecializationBLL.LoginIdentity = UserIdentity;
            Result result = ExactSpecializationBLL.Update();
            if (result.EnumMember == ExactSpecializationsValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == ExactSpecializationsValidationEnum.RejectedBecauseOfThisExactSpecializationExistsInEmployeesQualifications.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationThisExactSpecializationExistsInEmployeesQualificationsText);
            }
            return Json(new { ExactSpecializationID = ExactSpecializationBLL.ExactSpecializationID }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private ExactSpecializationsViewModel GetByExactSpecializationID(int id)//ExactSpecializationsViewModel ExactSpecializationsViewModel
        {
            ExactSpecializationsBLL ExactSpecializationBLL = new ExactSpecializationsBLL().GetByExactSpecializationID(id);
            ExactSpecializationsViewModel ExactSpecializationsVM = new ExactSpecializationsViewModel();
            if (ExactSpecializationBLL != null)
            {
                ExactSpecializationsVM.ExactSpecializationID = ExactSpecializationBLL.ExactSpecializationID;
                ExactSpecializationsVM.ExactSpecializationName = ExactSpecializationBLL.ExactSpecializationName;
                ExactSpecializationsVM.QualificationDegreeID = ExactSpecializationBLL.GeneralSpecialization.Qualification.QualificationDegree.QualificationDegreeID;
                ExactSpecializationsVM.QualificationID = ExactSpecializationBLL.GeneralSpecialization.Qualification.QualificationID;
                ExactSpecializationsVM.GeneralSpecializationID = ExactSpecializationBLL.GeneralSpecialization.GeneralSpecializationID;
                ExactSpecializationsVM.QualificationDegreeName = ExactSpecializationBLL.GeneralSpecialization.Qualification.QualificationDegree.QualificationDegreeName;
                ExactSpecializationsVM.QualificationName = ExactSpecializationBLL.GeneralSpecialization.Qualification.QualificationName;
                ExactSpecializationsVM.GeneralSpecializationName = ExactSpecializationBLL.GeneralSpecialization.GeneralSpecializationName;
            }
            return ExactSpecializationsVM;
        }

        [CustomAuthorize]
        public ActionResult DeleteExactSpecialization(int id)
        {
            return View(this.GetByExactSpecializationID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("ExactSpecializationName,Qualification,GeneralSpecialization")]
        public ActionResult DeleteExactSpecialization(ExactSpecializationsViewModel ExactSpecializationsVM)
        {
            ExactSpecializationsBLL ExactSpecializationBLL = new ExactSpecializationsBLL();
            ExactSpecializationBLL.ExactSpecializationID = ExactSpecializationsVM.ExactSpecializationID;
            ExactSpecializationBLL.LoginIdentity = UserIdentity;
            Result result = ExactSpecializationBLL.Remove(ExactSpecializationBLL.ExactSpecializationID);
            if (result.EnumMember == ExactSpecializationsValidationEnum.Done.ToString())
            {

            }
            return Json(new { ExactSpecializationID = ExactSpecializationBLL.ExactSpecializationID }, JsonRequestBehavior.AllowGet);
        }

    }
}