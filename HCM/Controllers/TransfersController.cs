using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class TransfersController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTransfers()
        {
            BaseTransfersBLL BaseTransfersBLL = new BaseTransfersBLL();
            return Json(new { data = BaseTransfersBLL.GetTransfers() }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["TransferID"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Create(TransfersViewModel EmployeesTransferVM)
        {
            BaseTransfersBLL TransferEmployee = new BaseTransfersBLL();
            if (EmployeesTransferVM.TransfersTypes.TransferTypeID == Convert.ToInt16(TransfersTypesEnum.TransferEmployeeWithJob))
                TransferEmployee = GenericFactoryPattern<BaseTransfersBLL, TransferEmployeesWithJobBLL>.CreateInstance();
            else if (EmployeesTransferVM.TransfersTypes.TransferTypeID == Convert.ToInt16(TransfersTypesEnum.TransferEmployeeWithoutJob))
            {
                TransferEmployee = GenericFactoryPattern<BaseTransfersBLL, TransferEmployeesWithoutJobBLL>.CreateInstance();
                ((TransferEmployeesWithoutJobBLL)TransferEmployee).JobName = EmployeesTransferVM.JobName;
                ((TransferEmployeesWithoutJobBLL)TransferEmployee).RankName = EmployeesTransferVM.RankName;
                ((TransferEmployeesWithoutJobBLL)TransferEmployee).JobCode = EmployeesTransferVM.JobCode;
                ((TransferEmployeesWithoutJobBLL)TransferEmployee).CareerDegreeName = EmployeesTransferVM.CareerDegreeName;
                ((TransferEmployeesWithoutJobBLL)TransferEmployee).OrganizationName = EmployeesTransferVM.OrganizationName;
            }

            TransferEmployee.LoginIdentity = this.UserIdentity;
            TransferEmployee.TransferType = new TransfersTypesBLL() { TransferTypeID = EmployeesTransferVM.TransfersTypes.TransferTypeID };
            TransferEmployee.Destination = EmployeesTransferVM.Destination;
            TransferEmployee.KSACity = new KSACitiesBLL() { KSACityID = EmployeesTransferVM.KSACity.KSACityID };
            TransferEmployee.TransferDate = EmployeesTransferVM.TransferDate;
            TransferEmployee.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = EmployeesTransferVM.EmployeeCareerHistoryID };

            Result result = TransferEmployee.Add();

            if (result.EnumMember == TransfersValidationEnum.Done.ToString())
                Session["TransferID"] = TransferEmployee.TransferID;

            else if (result.EnumMember == TransfersValidationEnum.RejectedBecauseOfTransferDateIsLessThanHiringDateDate.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfTransferDateIsLessThanHiringDateText);

            return View(EmployeesTransferVM);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByTransferID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("TransferDate")]
        public ActionResult EditEmployeeTransfer(TransfersViewModel EmployeesTransferVM)
        {
            BaseTransfersBLL TransferEmployee = new BaseTransfersBLL();
            if (EmployeesTransferVM.TransfersTypes.TransferTypeID == Convert.ToInt16(TransfersTypesEnum.TransferEmployeeWithJob))
                TransferEmployee = GenericFactoryPattern<BaseTransfersBLL, TransferEmployeesWithJobBLL>.CreateInstance();
            else if (EmployeesTransferVM.TransfersTypes.TransferTypeID == Convert.ToInt16(TransfersTypesEnum.TransferEmployeeWithoutJob))
            {
                TransferEmployee = GenericFactoryPattern<BaseTransfersBLL, TransferEmployeesWithoutJobBLL>.CreateInstance();
                ((TransferEmployeesWithoutJobBLL)TransferEmployee).JobName = EmployeesTransferVM.JobName;
                ((TransferEmployeesWithoutJobBLL)TransferEmployee).RankName = EmployeesTransferVM.RankName;
                ((TransferEmployeesWithoutJobBLL)TransferEmployee).JobCode = EmployeesTransferVM.JobCode;
                ((TransferEmployeesWithoutJobBLL)TransferEmployee).CareerDegreeName = EmployeesTransferVM.CareerDegreeName;
                ((TransferEmployeesWithoutJobBLL)TransferEmployee).OrganizationName = EmployeesTransferVM.OrganizationName;
            }

            TransferEmployee.TransferID = EmployeesTransferVM.TransferID;
            TransferEmployee.LoginIdentity = this.UserIdentity;
            TransferEmployee.TransferType = new TransfersTypesBLL() { TransferTypeID = EmployeesTransferVM.TransfersTypes.TransferTypeID };
            TransferEmployee.Destination = EmployeesTransferVM.Destination;
            TransferEmployee.KSACity = new KSACitiesBLL() { KSACityID = EmployeesTransferVM.KSACity.KSACityID };
            TransferEmployee.TransferDate = EmployeesTransferVM.TransferDate;
            TransferEmployee.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = EmployeesTransferVM.EmployeeCareerHistoryID };

            Result result = TransferEmployee.Update();

            if (result.EnumMember == TransfersValidationEnum.Done.ToString())
                Session["TransferID"] = TransferEmployee.TransferID;

            else if (result.EnumMember == TransfersValidationEnum.RejectedBecauseOfTransferDateIsLessThanHiringDateDate.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfTransferDateIsLessThanHiringDateText);

            else if (result.EnumMember == TransfersValidationEnum.RejectedBecauseOfAlreadyProcessed.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfBecauseOfAlreadyProcessedText);

            return View(this.GetByTransferID(EmployeesTransferVM.TransferID));
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByTransferID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("EmployeeCodeNo,TransferDate,Destination")]
        public ActionResult Delete(TransfersViewModel EmployeeTransferVM)
        {
            BaseTransfersBLL BaseTransfers = new BaseTransfersBLL();
            BaseTransfers.LoginIdentity = UserIdentity;

            Result result = BaseTransfers.Remove(EmployeeTransferVM.TransferID);

            if (result.EnumMember == TransfersValidationEnum.RejectedBecauseOfAlreadyProcessed.ToString())
                throw new CustomException(Resources.Globalization.ValidationBecauseOfBecauseOfAlreadyProcessedText);

            return RedirectToAction("Index");
        }

        [NonAction]
        private TransfersViewModel GetByTransferID(int id)
        {
            BaseTransfersBLL EmployeeTransferBLL = new BaseTransfersBLL().GetByTransferID(id);
            TransfersViewModel EmployeeTransferVM = new TransfersViewModel();
            if (EmployeeTransferBLL != null)
            {
                switch (EmployeeTransferBLL.TransferType.TransferTypeID)
                {
                    case (int)TransfersTypesEnum.TransferEmployeeWithJob:
                        {
                            break;
                        }
                    case (int)TransfersTypesEnum.TransferEmployeeWithoutJob:
                        {
                            EmployeeTransferVM.JobCode = ((TransferEmployeesWithoutJobBLL)EmployeeTransferBLL).JobCode;
                            EmployeeTransferVM.CareerDegreeName = ((TransferEmployeesWithoutJobBLL)EmployeeTransferBLL).CareerDegreeName;
                            EmployeeTransferVM.JobName = ((TransferEmployeesWithoutJobBLL)EmployeeTransferBLL).JobName;
                            EmployeeTransferVM.OrganizationName = ((TransferEmployeesWithoutJobBLL)EmployeeTransferBLL).OrganizationName;
                            EmployeeTransferVM.RankName = ((TransferEmployeesWithoutJobBLL)EmployeeTransferBLL).RankName;
                            break;
                        }
                    default:
                        break;
                }

                EmployeeTransferVM.TransfersTypes = EmployeeTransferBLL.TransferType;
                EmployeeTransferVM.TransferID = EmployeeTransferBLL.TransferID;
                EmployeeTransferVM.TransferDate = EmployeeTransferBLL.TransferDate.Date;
                EmployeeTransferVM.EmployeeCodeID = EmployeeTransferBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID;
                EmployeeTransferVM.EmployeeCareerHistoryID = EmployeeTransferBLL.EmployeeCareerHistory.EmployeeCareerHistoryID;
                EmployeeTransferVM.KSACity = new KSACitiesBLL()
                {
                    KSACityID = EmployeeTransferBLL.KSACity.KSACityID,
                    KSACityName = EmployeeTransferBLL.KSACity.KSACityName
                };
                EmployeeTransferVM.Destination = EmployeeTransferBLL.Destination;
                //EmployeeTransferVM.EmployeesCareersHistory = EmployeeTransferBLL.EmployeeCareerHistory;
                EmployeeTransferVM.Employee = new EmployeesViewModel()
                {
                    EmployeeCodeID = EmployeeTransferBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                    //EmployeeCodeNo = EmployeeTransferBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    //EmployeeNameAr = EmployeeTransferBLL.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr
                };
            }
            return EmployeeTransferVM;
        }

        public JsonResult GetTransferPrint()
        {
            int? TransferID = Session["TransferID"] == null ? (int?)null : int.Parse(Session["TransferID"].ToString());
            return Json(new { data = TransferID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult PrintTransfer(int id)
        {
            //return View("~/Views/Shared/ReportViewerASPX.aspx");
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.Transfers.ToString(), id));
        }
    }
}