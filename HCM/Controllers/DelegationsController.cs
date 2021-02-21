using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Classes.Helpers;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class DelegationsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>();
            ColumnNames.Add("DelegationID", Resources.Globalization.SequenceNoText);
            ColumnNames.Add("DelegationTypeName", Resources.Globalization.DelegationTypeText);
            ColumnNames.Add("DelegationKindName", Resources.Globalization.DelegationKindText);
            ColumnNames.Add("DelegationStartDate", Resources.Globalization.DelegationStartDateText);
            ColumnNames.Add("DelegationPeriod", Resources.Globalization.DelegationPeriodText);
            ColumnNames.Add("DelegationReason", Resources.Globalization.DelegationReasonText);
            ColumnNames.Add("IsApproved", Resources.Globalization.ApproveText);

            string fileName = ExcelHelper.ExportToExcel(GetAllDelegations(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllDelegations()
        {
            var data = new InternalDelegationBLL().GetDelegations()
                .Select(x => new
                {
                    DelegationID = x.DelegationID,
                    DelegationTypeName = x.DelegationType.DelegationTypeName,
                    DelegationKindName = x.DelegationKind.DelegationKindName,
                    DelegationStartDate = Globals.Calendar.GetUmAlQuraDate(x.DelegationStartDate),
                    x.DelegationPeriod,
                    x.DelegationReason,
                    IsApproved = x.IsApproved
                });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDelegations()
        {
            BaseDelegationsBLL DelegationBLL = new InternalDelegationBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize,
                OrderByColumnName = OrderByColumnName
            };
            var data = DelegationBLL.GetDelegations(out TotalRecordsOut, out RecFilterOut)
                .Select(x => new
                {
                    DelegationID = x.DelegationID,
                    DelegationTypeName = x.DelegationType.DelegationTypeName,
                    DelegationKindName = x.DelegationKind.DelegationKindName,
                    x.DelegationStartDate,
                    x.DelegationEndDate,
                    x.DelegationPeriod,
                    x.DelegationReason,
                    IsApproved = x.IsApproved
                });
            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        //[Route("{controller}/GetDelegationsTypes")]
        public JsonResult GetDelegationsTypes()
        {
            return Json(new
            {
                data = ((new DelegationsTypesBLL().GetDelegationsTypes())).Select(x => new
                {
                    DelegationTypeID = x.DelegationTypeID,
                    DelegationTypeName = x.DelegationTypeName,

                })
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDelegationDetails()
        {
            return Json(new
            {
                data = new DelegationsDetailsBLL().GetDelegationsDetails().Select(x => new
                {
                    EmployeeCodeID = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeIDNo = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeIDNo,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    DelegationID = x.Delegation.DelegationID,
                    DelegationReason = x.Delegation.DelegationReason,
                    Notes = x.Delegation.Notes,
                    DelegationTypeName = x.Delegation.DelegationType.DelegationTypeName,
                    DelegationKindName = x.Delegation.DelegationKind.DelegationKindName,
                    DelegationStartDate = x.Delegation.DelegationStartDate,
                    DelegationEndDate = x.Delegation.DelegationEndDate,
                    DelegationDestination = x.Delegation.DelegationDestination,
                    DelegationDetailID = x.DelegationDetailID

                })
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["DelegationID"] = null;
            Session["DelegationsEmployees"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByDelegationID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByDelegationID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("DelegationStartDate,DelegationEndDate,DelegationPeriod,DelegationReason,DelegationType,DelegationDistancePeriod,DelegationDetailRequest")]
        public ActionResult Delete(DelegationsViewModel DelegationVM)
        {
            BaseDelegationsBLL baseDelegationsBLL = new BaseDelegationsBLL();
            baseDelegationsBLL.LoginIdentity = UserIdentity;
            Result result = baseDelegationsBLL.Remove(DelegationVM.DelegationID);
            if ((System.Type)result.EnumType == typeof(DelegationsValidationEnum))
            {
                if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfAlreadyApprove.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationDelegationAlreadyApproveText);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [IgnoreModelProperties("DelegationDetailRequest")]
       
        public ActionResult Create(DelegationsViewModel DelegationVM)
        {
            if (DelegationVM.DelegationType.DelegationTypeID == (Int32)DelegationsTypesEnum.Internal)
            {
                InternalDelegationBLL Delegation = new InternalDelegationBLL();
                Delegation.DelegationKind = new DelegationsKindsBLL() { DelegationKindID = DelegationVM.DelegationKind.DelegationKindID };
                Delegation.DelegationType = new DelegationsTypesBLL() { DelegationTypeID = (int)DelegationsTypesEnum.Internal };
                Delegation.DelegationStartDate = DelegationVM.DelegationStartDate;
                Delegation.DelegationEndDate = DelegationVM.DelegationEndDate;
                Delegation.DelegationReason = DelegationVM.DelegationReason;
                Delegation.Notes = DelegationVM.Notes;
                Delegation.KSACity = new KSACitiesBLL() { KSACityID = DelegationVM.KSACity.KSACityID };
                Delegation.DelegationDistancePeriod = DelegationVM.DelegationDistancePeriod;
                Delegation.LoginIdentity = UserIdentity;
                Delegation.DelegationsDetails = GetDelegationDetailsFromSession();
                Result result = Delegation.Add();

                BaseDelegationsBLL DelegationEntity = (BaseDelegationsBLL)result.Entity;
                if (result.EnumMember == DelegationsValidationEnum.Done.ToString())
                {
                    DelegationVM.DelegationID = ((InternalDelegationBLL)result.Entity).DelegationID;
                    ClearDelegationDetailsFromSession();
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfDelegationDatesMustBeInSameFinancialYear.ToString())
                {
                    FinancialYearsBLL FinancialYearBLL = (FinancialYearsBLL)result.Entity;
                    throw new CustomException(Resources.Globalization.ValidationDelegationDatesMustBeInSameFinancialYearText + " NewLine" +
                                                                    Resources.Globalization.FinancialYearInfoText + FinancialYearBLL.FinancialYear + " NewLine" +
                                                                    Resources.Globalization.FinancialYearStartDateText + FinancialYearBLL.FinancialYearStartDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat) + " NewLine" +
                                                                    Resources.Globalization.FinancialYearEndDateText + FinancialYearBLL.FinancialYearEndDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat));
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfMaxLimit.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheDelegationLimitText);
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseEmployeeRequired.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
                }

                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
            }
            else if (DelegationVM.DelegationType.DelegationTypeID == (Int32)DelegationsTypesEnum.External)
            {
                ExternalDelegationBLL Delegation = new ExternalDelegationBLL();
                Delegation.DelegationKind = new DelegationsKindsBLL() { DelegationKindID = DelegationVM.DelegationKind.DelegationKindID };
                Delegation.DelegationType = new DelegationsTypesBLL() { DelegationTypeID = (int)DelegationsTypesEnum.External };
                Delegation.DelegationStartDate = DelegationVM.DelegationStartDate;
                Delegation.DelegationEndDate = DelegationVM.DelegationEndDate;
                Delegation.DelegationReason = DelegationVM.DelegationReason;
                Delegation.Notes = DelegationVM.Notes;
                Delegation.DelegationDistancePeriod = DelegationVM.DelegationDistancePeriod;
                Delegation.LoginIdentity = UserIdentity;
                Delegation.Country = new CountriesBLL() { CountryID = DelegationVM.Country.CountryID };
                Delegation.DelegationsDetails = GetDelegationDetailsFromSession();
                Result result = Delegation.Add();
                //if ((System.Type)result.EnumType == typeof(DelegationsValidationEnum))
                //{
                BaseDelegationsBLL DelegationEntity = (BaseDelegationsBLL)result.Entity;
                if (result.EnumMember == DelegationsValidationEnum.Done.ToString())
                {
                    DelegationVM.DelegationID = ((ExternalDelegationBLL)result.Entity).DelegationID;
                    ClearDelegationDetailsFromSession();
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfDelegationDatesMustBeInSameFinancialYear.ToString())
                {
                    FinancialYearsBLL FinancialYearBLL = (FinancialYearsBLL)result.Entity;
                    throw new CustomException(Resources.Globalization.ValidationDelegationDatesMustBeInSameFinancialYearText + " NewLine" +
                                                                    Resources.Globalization.FinancialYearInfoText + FinancialYearBLL.FinancialYear + " NewLine" +
                                                                    Resources.Globalization.FinancialYearStartDateText + FinancialYearBLL.FinancialYearStartDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat) + " NewLine" +
                                                                    Resources.Globalization.FinancialYearEndDateText + FinancialYearBLL.FinancialYearEndDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat));
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfMaxLimit.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheDelegationLimitText);
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseEmployeeRequired.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
                }

                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
            }

            return Json(new { DelegationID = DelegationVM.DelegationID }, JsonRequestBehavior.AllowGet);
        }

        private List<DelegationsDetailsBLL> GetDelegationDetailsFromSession()
        {
            List<DelegationsDetailsBLL> DelegationEmployeesList;
            if (Session["DelegationsEmployees"] != null)
                DelegationEmployeesList = (List<DelegationsDetailsBLL>)Session["DelegationsEmployees"];
            else
                DelegationEmployeesList = new List<DelegationsDetailsBLL>();

            return DelegationEmployeesList;
        }

        private void ClearDelegationDetailsFromSession()
        {
            Session["DelegationsEmployees"] = null;
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByDelegationID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("DelegationDetailRequest")]
        public ActionResult EditDelegation(DelegationsViewModel DelegationVM)
        {
            if (DelegationVM.DelegationType.DelegationTypeID == (Int32)DelegationsTypesEnum.Internal)
            {
                InternalDelegationBLL Delegation = new InternalDelegationBLL();
                Delegation.DelegationID = DelegationVM.DelegationID;
                Delegation.DelegationKind = new DelegationsKindsBLL() { DelegationKindID = DelegationVM.DelegationKind.DelegationKindID };
                Delegation.DelegationType = new DelegationsTypesBLL() { DelegationTypeID = (int)DelegationsTypesEnum.Internal };
                Delegation.DelegationStartDate = DelegationVM.DelegationStartDate;
                Delegation.DelegationEndDate = DelegationVM.DelegationEndDate;
                Delegation.DelegationReason = DelegationVM.DelegationReason;
                Delegation.Notes = DelegationVM.Notes;
                Delegation.DelegationDistancePeriod = DelegationVM.DelegationDistancePeriod;
                Delegation.LoginIdentity = UserIdentity;
                Delegation.KSACity = new KSACitiesBLL() { KSACityID = DelegationVM.KSACity.KSACityID };
                Result result = Delegation.Update();

                BaseDelegationsBLL DelegationEntity = (BaseDelegationsBLL)result.Entity;
                if (result.EnumMember == DelegationsValidationEnum.Done.ToString())
                {
                    Session["DelegationID"] = ((InternalDelegationBLL)result.Entity).DelegationID;
                    ClearDelegationDetailsFromSession();
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfMaxLimit.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheDelegationLimitText);
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseEmployeeRequired.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfAlreadyApprove.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationDelegationAlreadyApproveText);
                }

                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
            }
            else if (DelegationVM.DelegationType.DelegationTypeID == (Int32)DelegationsTypesEnum.External)
            {
                ExternalDelegationBLL Delegation = new ExternalDelegationBLL();
                Delegation.DelegationID = DelegationVM.DelegationID;
                Delegation.DelegationKind = new DelegationsKindsBLL() { DelegationKindID = DelegationVM.DelegationKind.DelegationKindID };
                Delegation.DelegationType = new DelegationsTypesBLL() { DelegationTypeID = (int)DelegationsTypesEnum.External };
                Delegation.DelegationStartDate = DelegationVM.DelegationStartDate;
                Delegation.DelegationEndDate = DelegationVM.DelegationEndDate;
                Delegation.DelegationReason = DelegationVM.DelegationReason;
                Delegation.Notes = DelegationVM.Notes;
                Delegation.DelegationDistancePeriod = DelegationVM.DelegationDistancePeriod;
                Delegation.LoginIdentity = UserIdentity;
                Delegation.Country = new CountriesBLL() { CountryID = DelegationVM.Country.CountryID };
                Result result = Delegation.Update();
                //if ((System.Type)result.EnumType == typeof(DelegationsValidationEnum))
                //{
                BaseDelegationsBLL DelegationEntity = (BaseDelegationsBLL)result.Entity;
                if (result.EnumMember == DelegationsValidationEnum.Done.ToString())
                {
                    Session["DelegationID"] = ((ExternalDelegationBLL)result.Entity).DelegationID;
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfMaxLimit.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheDelegationLimitText);
                }
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseEmployeeRequired.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationEmployeeRequiredText);
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
                else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfAlreadyApprove.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationDelegationAlreadyApproveText);
                }

                Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                //}
            }

            return View(DelegationVM);
        }

        [CustomAuthorize]
        public ActionResult PrintDelegation(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.Delegations.ToString(), id));
        }

        [CustomAuthorize]
        public ActionResult PrintAllDelegationsByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.Delegations.ToString(), id));
        }

        [NonAction]
        private DelegationsViewModel GetByDelegationID(int id)
        {
            BaseDelegationsBLL DelegationBLL = new BaseDelegationsBLL().GetByDelegationID(id);
            DelegationsViewModel DelegationVM = new DelegationsViewModel();
            if (DelegationBLL != null)
            {
                if (DelegationBLL.DelegationType.DelegationTypeID == (Int32)DelegationsTypesEnum.Internal)
                {
                    DelegationVM.KSARegion = ((InternalDelegationBLL)DelegationBLL).KSACity.KSARegion;
                    DelegationVM.KSACity = ((InternalDelegationBLL)DelegationBLL).KSACity;
                }
                else if (DelegationBLL.DelegationType.DelegationTypeID == (Int32)DelegationsTypesEnum.External)
                {
                    DelegationVM.KSACity = new KSACitiesBLL();
                    DelegationVM.Country = ((ExternalDelegationBLL)DelegationBLL).Country;
                }

                DelegationVM.DelegationKind = new DelegationsKindsBLL() { DelegationKindID = DelegationBLL.DelegationKind.DelegationKindID, DelegationKindName = DelegationBLL.DelegationKind.DelegationKindName };
                DelegationVM.DelegationType = new DelegationsTypesBLL() { DelegationTypeID = DelegationBLL.DelegationType.DelegationTypeID, DelegationTypeName = DelegationBLL.DelegationType.DelegationTypeName };
                DelegationVM.DelegationID = DelegationBLL.DelegationID;
                DelegationVM.DelegationStartDate = DelegationBLL.DelegationStartDate.Date;
                DelegationVM.DelegationEndDate = DelegationBLL.DelegationEndDate;
                DelegationVM.DelegationReason = DelegationBLL.DelegationReason;
                DelegationVM.Notes = DelegationBLL.Notes;
                DelegationVM.DelegationPeriod = DelegationBLL.DelegationPeriod;
                DelegationVM.DelegationDistancePeriod = DelegationBLL.DelegationDistancePeriod;
                DelegationVM.CreatedDate = DelegationBLL.CreatedDate;
                DelegationVM.CreatedBy = DelegationVM.GetCreatedByDisplayed(DelegationBLL.CreatedBy);
                DelegationVM.IsApprove = DelegationBLL.IsApproved;
            }
            return DelegationVM;
        }

        public JsonResult GetDelegationEmployees()
        {
            List<DelegationsDetailsBLL> DelegationEmployeesList;
            if (Session["DelegationsEmployees"] != null)
                DelegationEmployeesList = (List<DelegationsDetailsBLL>)Session["DelegationsEmployees"];
            else
                DelegationEmployeesList = new List<DelegationsDetailsBLL>();

            return Json(new { data = DelegationEmployeesList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDelegationEmployeesByDelegationID(int id)
        {
            return Json(new { data = new DelegationsDetailsBLL().GetDelegationsDetailsByDelegationID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("DelegationID,DelegationPeriod,DelegationReason,DelegationType")]
        public HttpResponseMessage CreateDelegationDetail(DelegationsViewModel DelegationVM)
        {
            List<DelegationsDetailsBLL> DelegationEmployeesList = GetDelegationDetailsFromSession();

            if (string.IsNullOrEmpty(DelegationVM.DelegationDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo))
            {
                throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
            }
            else if (DelegationEmployeesList.FindIndex(e => e.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo.Equals(DelegationVM.DelegationDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo)) > -1)
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExistText);
            }

            DateTime StartDate, EndDate;
            StartDate = DelegationVM.DelegationStartDate;
            EndDate = DelegationVM.DelegationEndDate;

            Result result = new EmployeeDelegationBLL(StartDate, EndDate, DelegationVM.DelegationKind.DelegationKindID, new EmployeesCodesBLL()
            {
                EmployeeCodeID = DelegationVM.DelegationDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID
            }).IsValid();

            //if ((System.Type)result.EnumType == typeof(DelegationsValidationEnum))
            //{
            if (result.EnumMember == DelegationsValidationEnum.Done.ToString())
            {
                DelegationEmployeesList.Add(DelegationVM.DelegationDetailRequest);
                Session["DelegationsEmployees"] = DelegationEmployeesList;
            }
            else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfDelegationDatesMustBeInSameFinancialYear.ToString())
            {
                FinancialYearsBLL FinancialYearBLL = (FinancialYearsBLL)result.Entity;
                throw new CustomException(Resources.Globalization.ValidationDelegationDatesMustBeInSameFinancialYearText + " NewLine" +
                                                                Resources.Globalization.FinancialYearInfoText + FinancialYearBLL.FinancialYear + " NewLine" +
                                                                Resources.Globalization.FinancialYearStartDateText + FinancialYearBLL.FinancialYearStartDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat) + " NewLine" +
                                                                Resources.Globalization.FinancialYearEndDateText + FinancialYearBLL.FinancialYearEndDate.Date.ToString(Classes.Helpers.CommonHelper.DateFormat));
            }
            else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfMaxLimit.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheDelegationLimitText);
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
            //}
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("DelegationID,DelegationPeriod,DelegationReason,DelegationType")]
        public HttpResponseMessage CreateDelegationDetailDB(DelegationsViewModel DelegationVM)
        {
            if (string.IsNullOrEmpty(DelegationVM.DelegationDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo))
            {
                throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
            }

            Result result = new DelegationsDetailsBLL().Add(new DelegationsDetailsBLL()
            {
                LoginIdentity = UserIdentity,
                EmployeeCareerHistory = DelegationVM.DelegationDetailRequest.EmployeeCareerHistory,
                Delegation = new BaseDelegationsBLL()
                {
                    DelegationID = DelegationVM.DelegationID,
                    DelegationStartDate = DelegationVM.DelegationStartDate,
                    DelegationEndDate = DelegationVM.DelegationEndDate,
                    DelegationKind = new DelegationsKindsBLL() { DelegationKindID = DelegationVM.DelegationKind.DelegationKindID }
                },
                IsPassengerOrderCompensation = DelegationVM.DelegationDetailRequest.IsPassengerOrderCompensation
            });

            //result = new EmployeeDelegationBLL(StartDate, EndDate, DelegationVM.DelegationKind.DelegationKindID, new EmployeesCodesBLL() { EmployeeCodeID = DelegationVM.DelegationDetailRequest.EmployeeCode.EmployeeCodeID }).IsValid();

            //if ((System.Type)result.EnumType == typeof(DelegationsValidationEnum))
            //{
            if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseAlreadyExist.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyExistText);
            }
            else if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfMaxLimit.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationAlreadyReachTheDelegationLimitText);
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
            // }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("DelegationID,DelegationStartDate,DelegationEndDate,DelegationPeriod,DelegationReason,DelegationType")]
        public HttpResponseMessage RemoveEmployeeFromDelegation(DelegationsViewModel DelegationVM)
        {
            List<DelegationsDetailsBLL> DelegationEmployeesList = GetDelegationDetailsFromSession();
            DelegationEmployeesList.RemoveAt(DelegationEmployeesList.FindIndex(e => e.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo.Equals(DelegationVM.DelegationDetailRequest.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo)));
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("DelegationID,DelegationStartDate,DelegationEndDate,DelegationPeriod,DelegationReason,DelegationType")]
        public HttpResponseMessage RemoveEmployeeFromDB(int id)
        {
            DelegationsDetailsBLL delegationsDetailsBLL = new DelegationsDetailsBLL();
            delegationsDetailsBLL.LoginIdentity = UserIdentity;
            delegationsDetailsBLL.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("DelegationID,DelegationStartDate,DelegationEndDate,DelegationPeriod,DelegationReason,DelegationType,DelegationDetailRequest")]
        public HttpResponseMessage ResetEmployeeFromSession()
        {
            Session["DelegationsEmployees"] = null;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("{controller}/GetDelegationBalanceByEmployeeCodeID/{id}/{DelegationStartDate}/{DelegationEndDate}")]
        public JsonResult GetDelegationBalanceByEmployeeCodeID(int id, string DelegationStartDate, string DelegationEndDate)
        {
            return Json(new { data = new EmployeesCodesBLL().GetDelegationBalanceByEmployeeCodeID(id, Convert.ToDateTime(DelegationStartDate), Convert.ToDateTime(DelegationEndDate)) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("{controller}/GetDelegationBalanceByEmployeeCodeNo/{EmployeeCodeNo}/{DelegationStartDate}/{DelegationEndDate}")]
        public JsonResult GetDelegationBalanceByEmployeeCodeNo(string EmployeeCodeNo, string DelegationStartDate, string DelegationEndDate)
        {
            EmployeesCodesBLL EmployeeCode = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);
            if (EmployeeCode != null)
                return Json(new { data = new EmployeesCodesBLL().GetDelegationBalanceByEmployeeCodeID(EmployeeCode.EmployeeCodeID, Convert.ToDateTime(DelegationStartDate), Convert.ToDateTime(DelegationEndDate)) }, JsonRequestBehavior.AllowGet);
            else
                return null;
        }

        [HttpPost]
        [Route("{controller}/GetDelegationEndDate/{DelegationPeriod}/{DelegationStartDate}")]
        public JsonResult GetDelegationEndDate(int DelegationPeriod, string DelegationStartDate)
        {
            return GetUmAlquraEndDate(DelegationPeriod, DelegationStartDate);
        }

        [CustomAuthorize]
        public ActionResult Approve(int id)
        {
            return View(this.GetByDelegationID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("DelegationStartDate,DelegationEndDate,DelegationPeriod,DelegationReason,DelegationType,DelegationDistancePeriod,DelegationDetailRequest")]
        public ActionResult Approve(DelegationsViewModel DelegationVM)
        {
            Result result = new BaseDelegationsBLL() { ApprovedBy = UserIdentity.EmployeeCodeID }.Approve(DelegationVM.DelegationID);
            if ((System.Type)result.EnumType == typeof(DelegationsValidationEnum))
            {
                if (result.EnumMember == DelegationsValidationEnum.RejectedBecauseOfAlreadyApprove.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationDelegationAlreadyApproveText);
                }
            }
            return RedirectToAction("Index");
        }

        public JsonResult GetDelegationByDelegationID(int DelegationID)
        {
            DelegationsViewModel vm = this.GetByDelegationID(DelegationID);
            if (vm == null) vm = new DelegationsViewModel();
            return Json(vm, JsonRequestBehavior.AllowGet);
        }
    }
}