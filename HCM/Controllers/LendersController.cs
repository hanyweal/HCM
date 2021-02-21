using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System.Web.Mvc;
using System.Linq;

namespace HCM.Controllers
{
    public class LendersController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLenders()
        {
            var data = new LendersBLL().GetLenders()
                .Select(x => new
                {
                    x.LenderID,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeIDNo = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeIDNo,
                    LenderOrganization = x.LenderOrganization,
                    KSACity = x.KSACity.KSACityName,
                    x.LenderStartDate,
                    x.LenderEndDate,
                    x.IsFinished
                });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            //return Json(new { data = new LendersBLL().GetLenders() }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["LenderID"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Create(LendersViewModel LendersVM)
        {
            LendersBLL LenderBLL = new LendersBLL()
            {
                LenderStartDate = LendersVM.LenderStartDate.Date,
                LenderEndDate = LendersVM.LenderEndDate.Date,
                LenderOrganization = LendersVM.LenderOrganization,
                KSACity = new KSACitiesBLL() { KSACityID = LendersVM.KSACity.KSACityID },
                EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = LendersVM.EmployeeCareerHistoryID },
                LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) },
            };
            Result result = LenderBLL.Add();
            if ((System.Type)result.EnumType == typeof(NoConflictWithOtherProcessValidationEnum))
            {
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
            }
            if ((System.Type)result.EnumType == typeof(LendersValidationEnum))
            {
                if (result.EnumMember == LendersValidationEnum.Done.ToString())
                {
                    LendersVM.LenderID = ((LendersBLL)result.Entity).LenderID;
                    //Session["LenderID"] = ((LendersBLL)result.Entity).LenderID;
                }
                //else if (result.EnumMember == LendersValidationEnum.RejectedBecauseOfActivePreviousLender.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.MustEndPreviousLenderText + "NewLine" + "تاريخ اخر تكليف : " + assgining.LenderStartDate.ToShortDateString());
                //}
                else if (result.EnumMember == LendersValidationEnum.RejectedBecauseOfConflictWithAssigning.ToString())
                {
                    BaseAssigningsBLL Assigning = ((BaseAssigningsBLL)result.Entity);
                    //throw new CustomException(string.Format(Resources.Globalization.ValidationConflictWithAssigningText,
                    //    Assigning.AssigningStartDate.ToShortDateString(), Assigning.AssigningEndDate.Value.ToShortDateString())); 
                    throw new CustomException(Resources.Globalization.ValidationConflictWithAssigningText);
                }
            }

            return Json(new { LenderID = LendersVM.LenderID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            LendersViewModel LenderVM = GetLenderByLenderID(id);
            return View(LenderVM);
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            LendersViewModel LenderVM = GetLenderByLenderID(id);
            return View(LenderVM);
        }

        [HttpDelete]
        [IgnoreModelProperties("LenderStartDate,LenderEndDate,LenderOrganization,EmployeeCodeNo")]
        public ActionResult Delete(LendersViewModel LendersVM)
        {
            LendersBLL LenderBLL = new LendersBLL()
            {
                LenderID = LendersVM.LenderID,
                LoginIdentity = UserIdentity
            };
            Result result = LenderBLL.Remove();
            if ((System.Type)result.EnumType == typeof(LendersValidationEnum))
            {
                // if (result.EnumMember == LendersValidationEnum.RejectedBecauseOfActivePreviousLender.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.MustEndPreviousLenderText + "NewLine" + "تاريخ اخر تكليف : " + assgining.LenderStartDate.ToShortDateString());
                //}
            }
            return View();
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            LendersViewModel LenderVM = GetLenderByLenderID(id);
            return View(LenderVM);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditLender(LendersViewModel LenderVM)
        {
            LendersBLL LenderBLL = new LendersBLL()
            {
                LenderID = LenderVM.LenderID,
                LenderStartDate = LenderVM.LenderStartDate.Date,
                LenderEndDate = LenderVM.LenderEndDate.Date,
                LenderOrganization = LenderVM.LenderOrganization,
                IsFinished = LenderVM.IsFinished,
                KSACity = new KSACitiesBLL() { KSACityID = LenderVM.KSACity.KSACityID },
                EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = LenderVM.EmployeeCareerHistoryID },
                LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) },
            };
            Result result = LenderBLL.Update();
            if ((System.Type)result.EnumType == typeof(NoConflictWithOtherProcessValidationEnum))
            {
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
            }
            if ((System.Type)result.EnumType == typeof(LendersValidationEnum))
            {
                if (result.EnumMember == LendersValidationEnum.Done.ToString())
                {
                    Session["LenderID"] = ((LendersBLL)result.Entity).LenderID;
                }
                else if (result.EnumMember == LendersValidationEnum.RejectedBecauseAlreadyFinished.ToString())
                {
                    throw new CustomException(string.Format(Resources.Globalization.ValidationLenderAlreadyFinishedEditNotAllowedText ));
                }                
                else if (result.EnumMember == LendersValidationEnum.RejectedBecauseOfConflictWithAssigning.ToString())
                {
                    BaseAssigningsBLL Assigning = ((BaseAssigningsBLL)result.Entity);
                    throw new CustomException(string.Format(Resources.Globalization.ValidationConflictWithAssigningText,
                        Assigning.AssigningStartDate.ToShortDateString(), Assigning.AssigningEndDate.Value.ToShortDateString()));
                }
                //else if (result.EnumMember == LendersValidationEnum.RejectedBecauseOfActivePreviousLender.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.MustEndPreviousLenderText + "NewLine" + "تاريخ اخر تكليف : " + assgining.LenderStartDate.ToShortDateString());
                //}
            }
            return View(this.GetLenderByLenderID(LenderVM.LenderID));
            //return View(LenderVM);
        }

        [CustomAuthorize]
        public ActionResult End(int id)
        {
            LendersViewModel LenderVM = GetLenderByLenderID(id);
            LenderVM.IsFinished = true;
            return View(LenderVM);
        }

        [HttpPost]
        [ActionName("End")]
        [IgnoreModelProperties("LenderOrganization")]
        public ActionResult EndLender(LendersViewModel LenderVM)
        {
            LendersBLL Lender = new LendersBLL().GetByLenderID(LenderVM.LenderID);
            Lender.IsFinished = true;
            Lender.LenderEndDate = LenderVM.LenderEndDate;
            Lender.LenderEndReason = "" +LenderVM.LenderEndReason;
            Lender.LoginIdentity = UserIdentity;
            Lender.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = LenderVM.EmployeeCareerHistoryID };
            Result result = Lender.EndLender();

            LendersBLL LenderEntity = (LendersBLL)result.Entity;
            if ((System.Type)result.EnumType == typeof(LendersValidationEnum))
            {
                if (result.EnumMember == LendersValidationEnum.Done.ToString())
                {
                    Session["LenderID"] = LenderVM.LenderID;
                }
                else if (result.EnumMember == LendersValidationEnum.RejectedBecauseEndReasonRequired.ToString())
                { 
                    throw new CustomException( Resources.Globalization.RequiredLenderEndReasonText);
                }
            }

            return View(GetLenderByLenderID(LenderVM.LenderID));
        }

        private LendersViewModel GetLenderByLenderID(int id)
        {
            LendersBLL LenderBLL = new LendersBLL().GetByLenderID(id);
            LendersViewModel LenderVM = new LendersViewModel();
            if (LenderBLL != null)
            {
                LenderVM.LenderID = LenderBLL.LenderID;
                LenderVM.LenderStartDate = LenderBLL.LenderStartDate;
                LenderVM.LenderEndDate = LenderBLL.LenderEndDate;
                LenderVM.LenderOrganization = LenderBLL.LenderOrganization;
                LenderVM.KSACity = LenderBLL.KSACity;
                LenderVM.IsFinished = LenderBLL.IsFinished;
                LenderVM.EmployeeCareerHistoryID = LenderBLL.EmployeeCareerHistory.EmployeeCareerHistoryID;
                LenderVM.EmployeeCareerHistory = LenderBLL.EmployeeCareerHistory;
                LenderVM.Employee = new EmployeesViewModel()
                {
                    EmployeeCodeID = LenderBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                    EmployeeCodeNo = LenderBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = LenderBLL.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr
                };
                LenderVM.CreatedDate = LenderBLL.CreatedDate;
                LenderVM.CreatedBy = LenderVM.GetCreatedByDisplayed(LenderBLL.CreatedBy);
            }
            return LenderVM;
        }

        [CustomAuthorize]
        public ActionResult PrintLender(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.Lenders.ToString(), id));
        }

        [CustomAuthorize]
        public ActionResult PrintAllLendersByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.Lenders.ToString(), id));
        }
    }
}