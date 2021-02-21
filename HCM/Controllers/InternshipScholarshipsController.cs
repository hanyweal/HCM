using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class InternshipScholarshipsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        public JsonResult GetInternshipScholarships()
        {
            BaseInternshipScholarshipsBLL InternshipScholarshipBLL = new InternalInternshipScholarshipsBLL();
            return Json(new { data = InternshipScholarshipBLL.GetInternshipScholarships() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInternshipScholarshipDetails()
        {
            return Json(new
            {
                data = new InternshipScholarshipsDetailsBLL().GetInternshipScholarshipsDetails().Select(x => new
                {
                    EmployeeCodeID = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeIDNo = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeIDNo,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    InternshipScholarshipID = x.InternshipScholarship.InternshipScholarshipID,
                    InternshipScholarshipLocation = x.InternshipScholarship.InternshipScholarshipLocation,
                    InternshipScholarshipReason = x.InternshipScholarship.InternshipScholarshipReason,
                    InternshipScholarshipTypeName = x.InternshipScholarship.InternshipScholarshipType.InternshipScholarshipTypeName,
                    InternshipScholarshipStartDate = x.InternshipScholarship.InternshipScholarshipStartDate,
                    InternshipScholarshipEndDate = x.InternshipScholarship.InternshipScholarshipEndDate,
                    InternshipScholarshipDetailID = x.InternshipScholarshipDetailID,
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["InternshipScholarshipID"] = null;
            Session["InternshipScholarshipsEmployees"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByInternshipScholarshipID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByInternshipScholarshipID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("InternshipScholarshipStartDate,InternshipScholarshipEndDate,InternshipScholarshipPeriod,InternshipScholarshipReason,InternshipScholarshipType,InternshipScholarshipDistancePeriod,InternshipScholarshipDetailRequest,InternshipScholarshipLocation")]
        public ActionResult Delete(InternshipScholarshipsViewModel InternshipScholarshipVM)
        {
            BaseInternshipScholarshipsBLL baseInternshipScholarshipsBLL = new BaseInternshipScholarshipsBLL();
            baseInternshipScholarshipsBLL.LoginIdentity = UserIdentity;
            baseInternshipScholarshipsBLL.Remove(InternshipScholarshipVM.InternshipScholarshipID);
            //return View(InternshipScholarshipVM); 
            return RedirectToAction("Index");
        }

        [HttpPost]
        [IgnoreModelProperties("InternshipScholarshipDetailRequest")]
        public ActionResult Create(InternshipScholarshipsViewModel InternshipScholarshipVM)
        {
            if (InternshipScholarshipVM.InternshipScholarshipType.InternshipScholarshipTypeID == (Int32)InternshipScholarshipsTypesEnum.Internal)
            {
                InternalInternshipScholarshipsBLL InternshipScholarship = new InternalInternshipScholarshipsBLL();
                InternshipScholarship.InternshipScholarshipType = new InternshipScholarshipsTypesBLL() { InternshipScholarshipTypeID = (int)InternshipScholarshipsTypesEnum.Internal };
                InternshipScholarship.InternshipScholarshipStartDate = InternshipScholarshipVM.InternshipScholarshipStartDate;
                InternshipScholarship.InternshipScholarshipEndDate = InternshipScholarshipVM.InternshipScholarshipEndDate;
                InternshipScholarship.InternshipScholarshipReason = InternshipScholarshipVM.InternshipScholarshipReason;
                InternshipScholarship.KSACity = new KSACitiesBLL() { KSACityID = InternshipScholarshipVM.KSACity.KSACityID };
                InternshipScholarship.InternshipScholarshipLocation = InternshipScholarshipVM.InternshipScholarshipLocation;
                InternshipScholarship.LoginIdentity = UserIdentity;
                InternshipScholarship.InternshipScholarshipsDetails = GetInternshipScholarshipDetailsFromSession();
                Result result = InternshipScholarship.Add();

                BaseInternshipScholarshipsBLL InternshipScholarshipEntity = (BaseInternshipScholarshipsBLL)result.Entity;
                if (result.EnumMember == InternshipScholarshipsValidationEnum.Done.ToString())
                {
                    InternshipScholarshipVM.InternshipScholarshipID = ((InternalInternshipScholarshipsBLL)result.Entity).InternshipScholarshipID;
                    //Session["InternshipScholarshipID"] = ((InternalInternshipScholarshipsBLL)result.Entity).InternshipScholarshipID;
                    ClearInternshipScholarshipDetailsFromSession();
                }
                else if (result.EnumMember == InternshipScholarshipsValidationEnum.RejectedBecauseEmployeeRequired.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
                }
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithOverTimeText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithDelegationText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithInternshipScholarshipText);
                //}

            }
            else if (InternshipScholarshipVM.InternshipScholarshipType.InternshipScholarshipTypeID == (Int32)InternshipScholarshipsTypesEnum.External)
            {
                ExternalInternshipScholarshipsBLL InternshipScholarship = new ExternalInternshipScholarshipsBLL();
                InternshipScholarship.InternshipScholarshipType = new InternshipScholarshipsTypesBLL() { InternshipScholarshipTypeID = (int)InternshipScholarshipsTypesEnum.External };
                InternshipScholarship.InternshipScholarshipStartDate = InternshipScholarshipVM.InternshipScholarshipStartDate;
                InternshipScholarship.InternshipScholarshipEndDate = InternshipScholarshipVM.InternshipScholarshipEndDate;
                InternshipScholarship.InternshipScholarshipReason = InternshipScholarshipVM.InternshipScholarshipReason;
                InternshipScholarship.InternshipScholarshipLocation = InternshipScholarshipVM.InternshipScholarshipLocation;
                InternshipScholarship.LoginIdentity = UserIdentity;
                InternshipScholarship.Country = new CountriesBLL() { CountryID = InternshipScholarshipVM.Country.CountryID };
                InternshipScholarship.InternshipScholarshipsDetails = GetInternshipScholarshipDetailsFromSession();
                Result result = InternshipScholarship.Add();

                BaseInternshipScholarshipsBLL InternshipScholarshipEntity = (BaseInternshipScholarshipsBLL)result.Entity;
                if (result.EnumMember == InternshipScholarshipsValidationEnum.Done.ToString())
                {
                    InternshipScholarshipVM.InternshipScholarshipID = ((ExternalInternshipScholarshipsBLL)result.Entity).InternshipScholarshipID;
                    //Session["InternshipScholarshipID"] = ((ExternalInternshipScholarshipsBLL)result.Entity).InternshipScholarshipID;
                    ClearInternshipScholarshipDetailsFromSession();
                }
                else if (result.EnumMember == InternshipScholarshipsValidationEnum.RejectedBecauseEmployeeRequired.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
                }
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithOverTimeText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithDelegationText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithInternshipScholarshipText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithVacationText);
                //}

            }

            //return View(InternshipScholarshipVM);
            return Json(new { InternshipScholarshipID = InternshipScholarshipVM.InternshipScholarshipID }, JsonRequestBehavior.AllowGet);
        }

        private List<InternshipScholarshipsDetailsBLL> GetInternshipScholarshipDetailsFromSession()
        {
            List<InternshipScholarshipsDetailsBLL> InternshipScholarshipEmployeesList;
            if (Session["InternshipScholarshipsEmployees"] != null)
                InternshipScholarshipEmployeesList = (List<InternshipScholarshipsDetailsBLL>)Session["InternshipScholarshipsEmployees"];
            else
                InternshipScholarshipEmployeesList = new List<InternshipScholarshipsDetailsBLL>();

            return InternshipScholarshipEmployeesList;
        }

        private void ClearInternshipScholarshipDetailsFromSession()
        {
            Session["InternshipScholarshipsEmployees"] = null;
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByInternshipScholarshipID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("InternshipScholarshipDetailRequest")]
        public ActionResult EditInternshipScholarship(InternshipScholarshipsViewModel InternshipScholarshipVM)
        {
            if (InternshipScholarshipVM.InternshipScholarshipType.InternshipScholarshipTypeID == (Int32)InternshipScholarshipsTypesEnum.Internal)
            {
                InternalInternshipScholarshipsBLL InternshipScholarship = new InternalInternshipScholarshipsBLL();
                InternshipScholarship.InternshipScholarshipID = InternshipScholarshipVM.InternshipScholarshipID;
                InternshipScholarship.InternshipScholarshipType = new InternshipScholarshipsTypesBLL() { InternshipScholarshipTypeID = (int)InternshipScholarshipsTypesEnum.Internal };
                InternshipScholarship.InternshipScholarshipStartDate = InternshipScholarshipVM.InternshipScholarshipStartDate;
                InternshipScholarship.InternshipScholarshipEndDate = InternshipScholarshipVM.InternshipScholarshipEndDate;
                InternshipScholarship.InternshipScholarshipReason = InternshipScholarshipVM.InternshipScholarshipReason;
                InternshipScholarship.InternshipScholarshipLocation = InternshipScholarshipVM.InternshipScholarshipLocation;
                InternshipScholarship.LoginIdentity = UserIdentity;
                InternshipScholarship.KSACity = new KSACitiesBLL() { KSACityID = InternshipScholarshipVM.KSACity.KSACityID };
                Result result = InternshipScholarship.Update();

                BaseInternshipScholarshipsBLL InternshipScholarshipEntity = (BaseInternshipScholarshipsBLL)result.Entity;
                if (result.EnumMember == InternshipScholarshipsValidationEnum.Done.ToString())
                {
                    Session["InternshipScholarshipID"] = ((InternalInternshipScholarshipsBLL)result.Entity).InternshipScholarshipID;
                    ClearInternshipScholarshipDetailsFromSession();
                }
                else if (result.EnumMember == InternshipScholarshipsValidationEnum.RejectedBecauseEmployeeRequired.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
                }
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithOverTimeText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithDelegationText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithInternshipScholarshipText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithVacationText);
                //}

            }
            else if (InternshipScholarshipVM.InternshipScholarshipType.InternshipScholarshipTypeID == (Int32)InternshipScholarshipsTypesEnum.External)
            {
                ExternalInternshipScholarshipsBLL InternshipScholarship = new ExternalInternshipScholarshipsBLL();
                InternshipScholarship.InternshipScholarshipID = InternshipScholarshipVM.InternshipScholarshipID;
                InternshipScholarship.InternshipScholarshipType = new InternshipScholarshipsTypesBLL() { InternshipScholarshipTypeID = (int)InternshipScholarshipsTypesEnum.External };
                InternshipScholarship.InternshipScholarshipStartDate = InternshipScholarshipVM.InternshipScholarshipStartDate;
                InternshipScholarship.InternshipScholarshipEndDate = InternshipScholarshipVM.InternshipScholarshipEndDate;
                InternshipScholarship.InternshipScholarshipReason = InternshipScholarshipVM.InternshipScholarshipReason;
                InternshipScholarship.InternshipScholarshipLocation = InternshipScholarshipVM.InternshipScholarshipLocation;
                InternshipScholarship.LoginIdentity = UserIdentity;
                InternshipScholarship.Country = new CountriesBLL() { CountryID = InternshipScholarshipVM.Country.CountryID };
                Result result = InternshipScholarship.Update();

                BaseInternshipScholarshipsBLL InternshipScholarshipEntity = (BaseInternshipScholarshipsBLL)result.Entity;
                if (result.EnumMember == InternshipScholarshipsValidationEnum.Done.ToString())
                {
                    Session["InternshipScholarshipID"] = ((ExternalInternshipScholarshipsBLL)result.Entity).InternshipScholarshipID;
                }
                else if (result.EnumMember == InternshipScholarshipsValidationEnum.RejectedBecauseEmployeeRequired.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
                }
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithOverTimeText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithDelegationText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithInternshipScholarshipText);
                //}
                //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString())
                //{
                //    throw new CustomException(Resources.Globalization.ValidationConflictWithVacationText);
                //}

            }

            return View(InternshipScholarshipVM);
        }

        [CustomAuthorize]
        public ActionResult PrintInternshipScholarship(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.InternshipScholarships.ToString(), id));
        }

        [CustomAuthorize]
        public ActionResult PrintAllInternshipScholarshipsByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.InternshipScholarships.ToString(), id));
        }

        //[HttpGet]
        //public JsonResult GetInternshipScholarshipID()
        //{
        //    int? InternshipScholarshipID = Session["InternshipScholarshipID"] == null ? (int?)null : int.Parse(Session["InternshipScholarshipID"].ToString());
        //    return Json(new { data = InternshipScholarshipID }, JsonRequestBehavior.AllowGet);
        //}

        [NonAction]
        private InternshipScholarshipsViewModel GetByInternshipScholarshipID(int id)
        {
            BaseInternshipScholarshipsBLL InternshipScholarshipBLL = new BaseInternshipScholarshipsBLL().GetByInternshipScholarshipID(id);
            InternshipScholarshipsViewModel InternshipScholarshipVM = new InternshipScholarshipsViewModel();
            if (InternshipScholarshipBLL != null)
            {
                if (InternshipScholarshipBLL.InternshipScholarshipType.InternshipScholarshipTypeID == (Int32)InternshipScholarshipsTypesEnum.Internal)
                {
                    InternshipScholarshipVM.KSARegion = ((InternalInternshipScholarshipsBLL)InternshipScholarshipBLL).KSACity.KSARegion;
                    InternshipScholarshipVM.KSACity = ((InternalInternshipScholarshipsBLL)InternshipScholarshipBLL).KSACity;
                }
                else if (InternshipScholarshipBLL.InternshipScholarshipType.InternshipScholarshipTypeID == (Int32)InternshipScholarshipsTypesEnum.External)
                {
                    InternshipScholarshipVM.Country = ((ExternalInternshipScholarshipsBLL)InternshipScholarshipBLL).Country;
                }

                InternshipScholarshipVM.InternshipScholarshipType = new InternshipScholarshipsTypesBLL() { InternshipScholarshipTypeID = InternshipScholarshipBLL.InternshipScholarshipType.InternshipScholarshipTypeID, InternshipScholarshipTypeName = InternshipScholarshipBLL.InternshipScholarshipType.InternshipScholarshipTypeName };
                InternshipScholarshipVM.InternshipScholarshipID = InternshipScholarshipBLL.InternshipScholarshipID;
                InternshipScholarshipVM.InternshipScholarshipStartDate = InternshipScholarshipBLL.InternshipScholarshipStartDate.Date;
                InternshipScholarshipVM.InternshipScholarshipEndDate = InternshipScholarshipBLL.InternshipScholarshipEndDate;
                InternshipScholarshipVM.InternshipScholarshipReason = InternshipScholarshipBLL.InternshipScholarshipReason;
                InternshipScholarshipVM.InternshipScholarshipPeriod = InternshipScholarshipBLL.InternshipScholarshipPeriod;
                InternshipScholarshipVM.InternshipScholarshipLocation = InternshipScholarshipBLL.InternshipScholarshipLocation;
                InternshipScholarshipVM.CreatedDate = InternshipScholarshipBLL.CreatedDate;
                InternshipScholarshipVM.CreatedBy = InternshipScholarshipVM.GetCreatedByDisplayed(InternshipScholarshipBLL.CreatedBy);
            }
            return InternshipScholarshipVM;
        }

        public JsonResult GetInternshipScholarshipEmployees()
        {
            List<InternshipScholarshipsDetailsBLL> InternshipScholarshipEmployeesList;
            if (Session["InternshipScholarshipsEmployees"] != null)
                InternshipScholarshipEmployeesList = (List<InternshipScholarshipsDetailsBLL>)Session["InternshipScholarshipsEmployees"];
            else
                InternshipScholarshipEmployeesList = new List<InternshipScholarshipsDetailsBLL>();

            return Json(new { data = InternshipScholarshipEmployeesList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInternshipScholarshipEmployeesByInternshipScholarshipID(int id)
        {
            return Json(new { data = new InternshipScholarshipsDetailsBLL().GetInternshipScholarshipsDetailsByInternshipScholarshipID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("InternshipScholarshipID,InternshipScholarshipPeriod,InternshipScholarshipReason,InternshipScholarshipType,InternshipScholarshipLocation")]
        public HttpResponseMessage CreateInternshipScholarshipDetail(InternshipScholarshipsViewModel InternshipScholarshipVM)
        {
            List<InternshipScholarshipsDetailsBLL> InternshipScholarshipEmployeesList = GetInternshipScholarshipDetailsFromSession();

            if (string.IsNullOrEmpty(InternshipScholarshipVM.InternshipScholarshipDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo))
            {
                throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
            }
            else if (InternshipScholarshipEmployeesList.FindIndex(e => e.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo.Equals(InternshipScholarshipVM.InternshipScholarshipDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo)) > -1)
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExistText);
            }

            DateTime StartDate, EndDate;
            StartDate = InternshipScholarshipVM.InternshipScholarshipStartDate;
            EndDate = InternshipScholarshipVM.InternshipScholarshipEndDate;

            Result result = CommonHelper.IsNoConflictWithOtherProcess(InternshipScholarshipVM.InternshipScholarshipDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID, StartDate, EndDate);

            if (result == null)
            {
                InternshipScholarshipEmployeesList.Add(InternshipScholarshipVM.InternshipScholarshipDetailRequest);
                Session["InternshipScholarshipsEmployees"] = InternshipScholarshipEmployeesList;
            }
            else
            {
                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
            }
            //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString())
            //{
            //    throw new CustomException(Resources.Globalization.ValidationConflictWithOverTimeText);
            //}
            //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithDelegation.ToString())
            //{
            //    throw new CustomException(Resources.Globalization.ValidationConflictWithDelegationText);
            //}
            //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
            //{
            //    throw new CustomException(Resources.Globalization.ValidationConflictWithInternshipScholarshipText);
            //}
            //else if (result.EnumMember == NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithVacation.ToString())
            //{
            //    throw new CustomException(Resources.Globalization.ValidationConflictWithVacationText);
            //}

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("InternshipScholarshipID,InternshipScholarshipPeriod,InternshipScholarshipReason,InternshipScholarshipType,InternshipScholarshipLocation")]
        public HttpResponseMessage CreateInternshipScholarshipDetailDB(InternshipScholarshipsViewModel InternshipScholarshipVM)
        {
            if (string.IsNullOrEmpty(InternshipScholarshipVM.InternshipScholarshipDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo))
            {
                throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
            }

            Result result = new InternshipScholarshipsDetailsBLL().Add(new InternshipScholarshipsDetailsBLL()
            {
                LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) },
                EmployeeCareerHistory = InternshipScholarshipVM.InternshipScholarshipDetailRequest.EmployeeCareerHistory,
                //EmployeeCode = InternshipScholarshipVM.InternshipScholarshipDetailRequest.EmployeeCareerHistory.EmployeeCode,
                InternshipScholarship = new BaseInternshipScholarshipsBLL()
                {
                    InternshipScholarshipID = InternshipScholarshipVM.InternshipScholarshipID,
                    InternshipScholarshipStartDate = InternshipScholarshipVM.InternshipScholarshipStartDate,
                    InternshipScholarshipEndDate = InternshipScholarshipVM.InternshipScholarshipEndDate,
                    InternshipScholarshipLocation = InternshipScholarshipVM.InternshipScholarshipLocation,
                    InternshipScholarshipReason = InternshipScholarshipVM.InternshipScholarshipReason,
                },
                IsPassengerOrderCompensation = InternshipScholarshipVM.InternshipScholarshipDetailRequest.IsPassengerOrderCompensation
            });

            if (result.EnumMember == InternshipScholarshipsValidationEnum.RejectedBecauseAlreadyExist.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExistText);
            }
            //else if (result.EnumMember == InternshipScholarshipsValidationEnum.RejectedBecauseOfMaxLimit.ToString())
            //{
            //    throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheInternshipScholarshipLimitText);
            //}
            //else if (result.EnumMember == InternshipScholarshipsValidationEnum.RejectedBecauseOfConflictWithOverTime.ToString())
            //{
            //    throw new CustomException(Resources.Globalization.ValidationInternshipScholarshipConflictWithOverTimeText);
            //}
            //else if (result.EnumMember == InternshipScholarshipsValidationEnum.RejectedBecauseOfConflictWithInternshipScholarship.ToString())
            //{
            //    throw new CustomException(Resources.Globalization.ValidationInternshipScholarshipConflictWithInternshipScholarshipText);
            //}

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("InternshipScholarshipID,InternshipScholarshipStartDate,InternshipScholarshipEndDate,InternshipScholarshipPeriod,InternshipScholarshipReason,InternshipScholarshipType,InternshipScholarshipLocation")]
        public HttpResponseMessage RemoveEmployeeFromInternshipScholarship(InternshipScholarshipsViewModel InternshipScholarshipVM)
        {
            List<InternshipScholarshipsDetailsBLL> InternshipScholarshipEmployeesList = GetInternshipScholarshipDetailsFromSession();
            InternshipScholarshipEmployeesList.RemoveAt(InternshipScholarshipEmployeesList.FindIndex(e => e.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo.Equals(InternshipScholarshipVM.InternshipScholarshipDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo)));
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("InternshipScholarshipID,InternshipScholarshipStartDate,InternshipScholarshipEndDate,InternshipScholarshipPeriod,InternshipScholarshipReason,InternshipScholarshipType,InternshipScholarshipLocation")]
        public HttpResponseMessage RemoveEmployeeFromDB(int id)
        {
            InternshipScholarshipsDetailsBLL internshipScholarshipsDetailsBLL = new InternshipScholarshipsDetailsBLL();
            internshipScholarshipsDetailsBLL.LoginIdentity = UserIdentity;
            internshipScholarshipsDetailsBLL.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("InternshipScholarshipID,InternshipScholarshipStartDate,InternshipScholarshipEndDate,InternshipScholarshipPeriod,InternshipScholarshipReason,InternshipScholarshipType,InternshipScholarshipDetailRequest,InternshipScholarshipLocation")]
        public HttpResponseMessage ResetEmployeeFromSession()
        {
            Session["InternshipScholarshipsEmployees"] = null;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("{controller}/GetInternshipScholarshipEndDate/{InternshipScholarshipPeriod}/{InternshipScholarshipStartDate}")]
        public JsonResult GetInternshipScholarshipEndDate(int InternshipScholarshipPeriod, string InternshipScholarshipStartDate)
        {
            //return GetUmAlquraEndDate(InternshipScholarshipPeriod, InternshipScholarshipStartDate);
            return Json(GetUmAlquraEndDate(InternshipScholarshipPeriod, InternshipScholarshipStartDate), JsonRequestBehavior.AllowGet);
        }
    }
}