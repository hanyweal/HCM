using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Classes.Extensions;
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
    public class PassengerOrdersController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "PassengerOrderID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "TravellingDate", Resources.Globalization.TravellingDateText },
                { "Returning", Resources.Globalization.ReturningText },
                { "Going", Resources.Globalization.GoingText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetAllPassengerOrders(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllPassengerOrders()
        {
            var data = new PassengerOrdersBLL().GetPassengerOrders()
                .Select(x => new
                {
                    PassengerOrderID = x.PassengerOrderID,
                    TravellingDate = Globals.Calendar.GetUmAlQuraDate(x.TravellingDate),
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    Going = x.Going,
                    Returning = x.Returning
                });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPassengerOrders()
        {
            PassengerOrdersBLL PassengerOrderBLL = new PassengerOrdersBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            };
            var data = PassengerOrderBLL.GetPassengerOrders(out TotalRecordsOut, out RecFilterOut)
                .Select(x => new
                {
                    PassengerOrderID = x.PassengerOrderID,
                    TravellingDate = x.TravellingDate,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    Going = x.Going,
                    Returning = x.Returning
                });
            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["PassengerOrderID"] = null;
            Session["PassengerOrdersItineraries"] = null;
            Session["PassengerOrdersEscorts"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByPassengerOrderID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByPassengerOrderID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("TravellingDate,DelegationDetailRequest,PassengerOrdersItineraryRequest")]
        public ActionResult Delete(PassengerOrdersViewModel PassengerOrderVM)
        {
            PassengerOrdersBLL passengerOrdersBLL = new PassengerOrdersBLL();
            passengerOrdersBLL.LoginIdentity = UserIdentity;
            passengerOrdersBLL.Remove(PassengerOrderVM.PassengerOrderID);
            //return View(PassengerOrderVM); 
            return RedirectToAction("Index");
        }

        [HttpPost]
        [IgnoreModelProperties("PassengerOrdersItineraryRequest")]
        public ActionResult Create(PassengerOrdersViewModel PassengerOrderVM)
        {
            PassengerOrdersBLL PassengerOrder = new PassengerOrdersBLL();
            PassengerOrder.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = PassengerOrderVM.EmployeeCareerHistoryID };

            if (PassengerOrderVM.RankID > 0)
                PassengerOrder.Rank = new RanksBLL() { RankID = PassengerOrderVM.RankID };

            if (PassengerOrderVM.TicketClassID > 0)
                PassengerOrder.TicketClass = new TicketsClassesBLL() { TicketClassID = PassengerOrderVM.TicketClassID };

            PassengerOrder.TravellingDate = PassengerOrderVM.TravellingDate;
            PassengerOrder.Going = PassengerOrderVM.Going;
            PassengerOrder.Returning = PassengerOrderVM.Returning;
            PassengerOrder.Reason = PassengerOrderVM.Reason;
            PassengerOrder.LoginIdentity = UserIdentity;
            PassengerOrder.PassengerOrdersItineraries = GetPassengerOrderItinerarysFromSession();
            PassengerOrder.PassengerOrdersEscorts = GetPassengerOrderEscortsFromSession();

            Result result = PassengerOrder.Add();
            if ((System.Type)result.EnumType == typeof(PassengerOrdersValidationEnum))
            {
                PassengerOrdersBLL PassengerOrderEntity = (PassengerOrdersBLL)result.Entity;
                if (result.EnumMember == PassengerOrdersValidationEnum.Done.ToString())
                {
                    PassengerOrderVM.PassengerOrderID = ((PassengerOrdersBLL)result.Entity).PassengerOrderID;
                    ClearPassengerOrderItinerarysFromSession();
                }
                else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecauseItineraryRequired.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationItineraryRequiredText);
                }
                else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecausePassengerOrderAlreadyTook.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationPassengerOrderAlreadyTookText);
                }
                else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecausePassengerOrderCompensation.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationPassengerOrderCompensationText);
                }
                else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecauseMaxEscortMemberInOrdersIsFour.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationMaxPassengerOrderEscortIsFour);
                }
            }

            return Json(new { PassengerOrderID = PassengerOrderVM.PassengerOrderID }, JsonRequestBehavior.AllowGet);
        }

        private List<PassengerOrdersItinerariesBLL> GetPassengerOrderItinerarysFromSession()
        {
            List<PassengerOrdersItinerariesBLL> PassengerOrdersItinerariesList;
            if (Session["PassengerOrdersItineraries"] != null)
                PassengerOrdersItinerariesList = (List<PassengerOrdersItinerariesBLL>)Session["PassengerOrdersItineraries"];
            else
                PassengerOrdersItinerariesList = new List<PassengerOrdersItinerariesBLL>();

            return PassengerOrdersItinerariesList;
        }

        private List<PassengerOrdersEscortsBLL> GetPassengerOrderEscortsFromSession()
        {
            List<PassengerOrdersEscortsBLL> PassengerOrdersEscortsList;
            if (Session["PassengerOrdersEscorts"] != null)
                PassengerOrdersEscortsList = (List<PassengerOrdersEscortsBLL>)Session["PassengerOrdersEscorts"];
            else
                PassengerOrdersEscortsList = new List<PassengerOrdersEscortsBLL>();

            return PassengerOrdersEscortsList;
        }

        private void ClearPassengerOrderItinerarysFromSession()
        {
            Session["PassengerOrdersItineraries"] = null;
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByPassengerOrderID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("PassengerOrdersItineraryRequest,DelegationStartDate,DelegationEndDate")]
        public ActionResult EditPassengerOrder(PassengerOrdersViewModel PassengerOrderVM)
        {
            PassengerOrdersBLL PassengerOrder = new PassengerOrdersBLL();
            PassengerOrder.PassengerOrderID = PassengerOrderVM.PassengerOrderID;
            PassengerOrder.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = PassengerOrderVM.EmployeeCareerHistoryID };

            if (PassengerOrderVM.RankID > 0)
                PassengerOrder.Rank = new RanksBLL() { RankID = PassengerOrderVM.RankID };

            if (PassengerOrderVM.TicketClassID > 0)
                PassengerOrder.TicketClass = new TicketsClassesBLL() { TicketClassID = PassengerOrderVM.TicketClassID };

            PassengerOrder.TravellingDate = PassengerOrderVM.TravellingDate;
            PassengerOrder.Going = PassengerOrderVM.Going;
            PassengerOrder.Returning = PassengerOrderVM.Returning;
            PassengerOrder.Reason = PassengerOrderVM.Reason;
            PassengerOrder.LoginIdentity = UserIdentity;
            Result result = PassengerOrder.Update();
            if ((System.Type)result.EnumType == typeof(PassengerOrdersValidationEnum))
            {
                PassengerOrdersBLL PassengerOrderEntity = (PassengerOrdersBLL)result.Entity;
                if (result.EnumMember == PassengerOrdersValidationEnum.Done.ToString())
                {
                    Session["PassengerOrderID"] = ((PassengerOrdersBLL)result.Entity).PassengerOrderID;
                    ClearPassengerOrderItinerarysFromSession();
                }
                else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecauseItineraryRequired.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationItineraryRequiredText);
                }
                else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecausePassengerOrderAlreadyTook.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationPassengerOrderAlreadyTookText);
                }

            }

            return View(this.GetByPassengerOrderID(PassengerOrderVM.PassengerOrderID));
        }

        [CustomAuthorize]
        public ActionResult PrintPassengerOrder(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.PassengerOrders.ToString(), id));
        }

        [NonAction]
        private PassengerOrdersViewModel GetByPassengerOrderID(int id)
        {
            PassengerOrdersBLL PassengerOrderBLL = new PassengerOrdersBLL().GetByPassengerOrderID(id);
            PassengerOrdersViewModel PassengerOrderVM = new PassengerOrdersViewModel();
            if (PassengerOrderBLL != null)
            {
                PassengerOrderVM.PassengerOrderID = PassengerOrderBLL.PassengerOrderID;
                PassengerOrderVM.RankID = PassengerOrderBLL.Rank.RankID;
                PassengerOrderVM.RankName = PassengerOrderBLL.Rank.RankName;
                PassengerOrderVM.TicketClassName = PassengerOrderBLL.TicketClass.TicketClassName;
                PassengerOrderVM.TicketClassID = PassengerOrderBLL.TicketClass.TicketClassID;
                PassengerOrderVM.TravellingDate = PassengerOrderBLL.TravellingDate;
                PassengerOrderVM.Going = PassengerOrderBLL.Going;
                PassengerOrderVM.Returning = PassengerOrderBLL.Returning;
                PassengerOrderVM.Reason = PassengerOrderBLL.Reason;
                PassengerOrderVM.CreatedDate = PassengerOrderBLL.CreatedDate;
                PassengerOrderVM.CreatedBy = PassengerOrderVM.GetCreatedByDisplayed(PassengerOrderBLL.CreatedBy);

                PassengerOrderVM.Employee = new EmployeesViewModel() { EmployeeCodeID = PassengerOrderBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID };
                PassengerOrderVM.Employee = PassengerOrderVM.Employee.GetEmployee();
            }

            return PassengerOrderVM;
        }

        public JsonResult GetPassengerOrdersItineraries()
        {
            List<PassengerOrdersItinerariesBLL> PassengerOrdersItinerariesList;
            if (Session["PassengerOrdersItineraries"] != null)
                PassengerOrdersItinerariesList = (List<PassengerOrdersItinerariesBLL>)Session["PassengerOrdersItineraries"];
            else
                PassengerOrdersItinerariesList = new List<PassengerOrdersItinerariesBLL>();

            return Json(new { data = PassengerOrdersItinerariesList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPassengerOrdersEscorts()
        {
            List<PassengerOrdersEscortsBLL> PassengerOrdersEscortsList;
            if (Session["PassengerOrdersEscorts"] != null)
                PassengerOrdersEscortsList = (List<PassengerOrdersEscortsBLL>)Session["PassengerOrdersEscorts"];
            else
                PassengerOrdersEscortsList = new List<PassengerOrdersEscortsBLL>();

            return Json(new { data = PassengerOrdersEscortsList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPassengerOrdersItinerariesByPassengerOrderID(int id)
        {
            return Json(new { data = new PassengerOrdersItinerariesBLL().GetPassengerOrdersItinerariesByPassengerOrderID(id) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPassengerOrdersEscortsByPassengerOrderID(int id)
        {
            return Json(new { data = new PassengerOrdersEscortsBLL().GetPassengerOrdersEscortsByPassengerOrderID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest")]
        public HttpResponseMessage CreatePassengerOrderItinerary(PassengerOrdersViewModel PassengerOrderVM)
        {
            List<PassengerOrdersItinerariesBLL> PassengerOrdersItinerariesList = GetPassengerOrderItinerarysFromSession();

            //if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.EmployeeCode.EmployeeCodeNo))
            //{
            //    throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
            //}
            //else 
            if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity))
            {
                throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityRequiredText);
            }
            else if (PassengerOrdersItinerariesList.FindIndex(e => e.FromCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity.ToUpper())
                                                                && e.ToCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity.ToUpper())) > -1)
            {
                throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityAlreadyExistText);
            }

            PassengerOrdersItinerariesList.Add(PassengerOrderVM.PassengerOrdersItineraryRequest);
            Session["PassengerOrdersItineraries"] = PassengerOrdersItinerariesList;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest,PassengerOrdersItineraryRequest")]
        public HttpResponseMessage CreatePassengerOrderEscort(PassengerOrdersViewModel PassengerOrderVM)
        {
            List<PassengerOrdersEscortsBLL> PassengerOrdersEscortsList = GetPassengerOrderEscortsFromSession();

            //if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.EmployeeCode.EmployeeCodeNo))
            //{
            //    throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
            //}
            //else 
            if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersEscortRequest.EscortName) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersEscortRequest.EscortIDNo) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersEscortRequest.EscortAge) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersEscortRequest.EscortRelativeRelation))
            {
                throw new CustomException(Resources.Globalization.ValidationPassengerOrderEscortRequiredText);
            }
            else if (PassengerOrdersEscortsList.FindIndex(e => e.EscortName.ToUpper().Equals(PassengerOrderVM.PassengerOrdersEscortRequest.EscortName.ToUpper())
                                                                && e.EscortIDNo.ToUpper().Equals(PassengerOrderVM.PassengerOrdersEscortRequest.EscortIDNo.ToUpper())
                                                                && e.EscortAge.ToUpper().Equals(PassengerOrderVM.PassengerOrdersEscortRequest.EscortAge.ToUpper())
                                                                && e.EscortRelativeRelation.ToUpper().Equals(PassengerOrderVM.PassengerOrdersEscortRequest.EscortRelativeRelation.ToUpper())
                                                                ) > -1)
            {
                throw new CustomException(Resources.Globalization.ValidationPassengerOrderEscortAlreadyExistText);
            }

            PassengerOrdersEscortsList.Add(new PassengerOrdersEscortsBLL() { EscortAge = PassengerOrderVM.PassengerOrdersEscortRequest.EscortAge, EscortIDNo = PassengerOrderVM.PassengerOrdersEscortRequest.EscortIDNo, EscortRelativeRelation = PassengerOrderVM.PassengerOrdersEscortRequest.EscortRelativeRelation, EscortName = PassengerOrderVM.PassengerOrdersEscortRequest.EscortName, LoginIdentity = UserIdentity });
            Session["PassengerOrdersEscorts"] = PassengerOrdersEscortsList;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost]
        [IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest")]
        public HttpResponseMessage RemoveItineraryFromPassengerOrder(PassengerOrdersViewModel PassengerOrderVM)
        {
            List<PassengerOrdersItinerariesBLL> PassengerOrdersItinerariesList = GetPassengerOrderItinerarysFromSession();
            PassengerOrdersItinerariesList.RemoveAt(PassengerOrdersItinerariesList
                                                        .FindIndex(e => e.FromCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity.ToUpper())
                                                                    && e.ToCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity.ToUpper())));
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest,PassengerOrdersItineraryRequest")]
        public HttpResponseMessage RemoveEscortFromPassengerOrder(PassengerOrdersViewModel PassengerOrderVM)
        {
            List<PassengerOrdersEscortsBLL> PassengerOrdersEscortsList = GetPassengerOrderEscortsFromSession();
            PassengerOrdersEscortsList.RemoveAt(PassengerOrdersEscortsList
                                                        .FindIndex(e => e.EscortName.ToUpper().Equals(PassengerOrderVM.PassengerOrdersEscortRequest.EscortName.ToUpper())
                                                                    && e.EscortIDNo.ToUpper().Equals(PassengerOrderVM.PassengerOrdersEscortRequest.EscortIDNo.ToUpper())
                                                                     && e.EscortAge.ToUpper().Equals(PassengerOrderVM.PassengerOrdersEscortRequest.EscortAge.ToUpper())
                                                                      && e.EscortRelativeRelation.ToUpper().Equals(PassengerOrderVM.PassengerOrdersEscortRequest.EscortRelativeRelation.ToUpper())
                                                                    ));
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("PassengerOrderID,TravellingDateText,DelegationDetailRequest,PassengerOrdersItineraryRequest")]
        public HttpResponseMessage ResetItineraryFromSession()
        {
            Session["PassengerOrdersItineraries"] = null;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest")]
        public HttpResponseMessage CreatePassengerOrderItineraryDB(PassengerOrdersViewModel PassengerOrderVM)
        {
            if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity))
            {
                throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityRequiredText);
            }
            //else if (PassengerOrdersItinerariesList.FindIndex(e => e.FromCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity.ToUpper())
            //                                                    && e.ToCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity.ToUpper())) > -1)
            //{
            //    throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityAlreadyExist);
            //}
            PassengerOrderVM.PassengerOrdersItineraryRequest.LoginIdentity = UserIdentity;
            Result result = new PassengerOrdersItinerariesBLL().Add(PassengerOrderVM.PassengerOrdersItineraryRequest);

            if ((System.Type)result.EnumType == typeof(PassengerOrdersValidationEnum))
            {
                if (result.EnumMember == PassengerOrdersValidationEnum.Done.ToString())
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecausePassengerOrderItineraryCityAlreadyExist.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityAlreadyExistText);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest,PassengerOrdersItineraryRequest")]
        public HttpResponseMessage CreatePassengerOrderEscortDB(PassengerOrdersViewModel PassengerOrderVM)
        {
            if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersEscortRequest.EscortName) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersEscortRequest.EscortIDNo) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersEscortRequest.EscortAge) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersEscortRequest.EscortRelativeRelation))
            {
                throw new CustomException(Resources.Globalization.ValidationPassengerOrderEscortRequiredText);
            }
            //else if (PassengerOrdersItinerariesList.FindIndex(e => e.FromCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity.ToUpper())
            //                                                    && e.ToCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity.ToUpper())) > -1)
            //{
            //    throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityAlreadyExist);
            //}
           // PassengerOrderVM.PassengerOrdersEscortRequest.CreatedBy = UserIdentity;
            Result result = new PassengerOrdersEscortsBLL().Add(new PassengerOrdersEscortsBLL() { EscortAge = PassengerOrderVM.PassengerOrdersEscortRequest.EscortAge, EscortIDNo = PassengerOrderVM.PassengerOrdersEscortRequest.EscortIDNo, EscortRelativeRelation = PassengerOrderVM.PassengerOrdersEscortRequest.EscortRelativeRelation, EscortName = PassengerOrderVM.PassengerOrdersEscortRequest.EscortName,LoginIdentity = UserIdentity,PassengerOrder=new PassengerOrdersBLL() { PassengerOrderID= PassengerOrderVM .PassengerOrderID} });

            if ((System.Type)result.EnumType == typeof(PassengerOrdersValidationEnum))
            {
                if (result.EnumMember == PassengerOrdersValidationEnum.Done.ToString())
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecausePassengerOrderEscortAlreadyExist.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationPassengerOrderEscortRequiredText);
                }
                else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecauseMaxEscortMemberInOrdersIsFour.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationMaxPassengerOrderEscortIsFour);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [HttpPost]
        [IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest")]
        public HttpResponseMessage RemovePassengerOrderItineraryDB(int id)
        {
            PassengerOrdersItinerariesBLL passengerOrdersItinerariesBLL = new PassengerOrdersItinerariesBLL();
            passengerOrdersItinerariesBLL.LoginIdentity = UserIdentity;
            passengerOrdersItinerariesBLL.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        [HttpPost]
        [IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest,PassengerOrdersItineraryRequest")]
        public HttpResponseMessage RemovePassengerOrderEscortDB(int id)
        {
            PassengerOrdersEscortsBLL passengerOrdersEscortBLL = new PassengerOrdersEscortsBLL();
            passengerOrdersEscortBLL.LoginIdentity = UserIdentity;
            passengerOrdersEscortBLL.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        [CustomAuthorize]
        public ActionResult PrintAllPassengerOrdersByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.PassengerOrders.ToString(), id));
        }
    }
}