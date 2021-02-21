using HCM.Classes;
using HCM.Classes.CustomAttributes;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class PassengerOrdersOldController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public override ActionResult ExportExcel()
        //{
        //    DataSet dataSet = new DataSet();
        //    dataSet.Tables.Add(ExcelHelper.ConvertJsonToDatatable(GetAllPassengerOrders()));

        //    var excelApp = new Application();
        //    excelApp.Visible = true;
        //    excelApp.Workbooks.Add();

        //    Worksheet workSheet = (Worksheet)excelApp.ActiveSheet;
        //    workSheet.Name = Resources.Globalization.PassengerOrdersText;

        //    var columnName = dataSet.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

        //    int i = 0;
        //    string column = "";
        //    foreach (var col in columnName)
        //    {
        //        i++;
        //        if (col == "PassengerOrderID")
        //            column = Resources.Globalization.SequenceNoText;
        //        else if (col == "TravellingDate")
        //            column = Resources.Globalization.TravellingDateText;
        //        else if (col == "EmployeeCodeNo")
        //            column = Resources.Globalization.EmployeeCodeNoText;
        //        else if (col == "EmployeeNameAr")
        //            column = Resources.Globalization.EmployeeNameArText;
        //        else if (col == "Returning")
        //            column = Resources.Globalization.ReturningText;
        //        else if (col == "Going")
        //            column = Resources.Globalization.GoingText;


        //        workSheet.Cells[1, i] = column;
        //        workSheet.Cells[1, i].Font.Bold = true;
        //    }

        //    int j;
        //    for (i = 0; i < dataSet.Tables[0].Rows.Count; i++)
        //    {
        //        for (j = 0; j < dataSet.Tables[0].Columns.Count; j++)
        //        {
        //            workSheet.Cells[i + 2, j + 1] = Convert.ToString(dataSet.Tables[0].Rows[i][j]);
        //        }
        //    }

        //    return RedirectToAction("Index");
        //}

        //public JsonResult GetAllPassengerOrders()
        //{
        //    var data = new PassengerOrdersBLL().GetPassengerOrders()
        //        .Select(x => new
        //        {
        //            PassengerOrderID = x.PassengerOrderID,
        //            TravellingDate = Globals.Calendar.GetUmAlQuraDate(x.TravellingDate),                   
        //            EmployeeCodeNo = x.EmployeeCode.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
        //            EmployeeNameAr = x.EmployeeCode.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
        //            Going = x.Going,
        //            Returning = x.Returning
        //        });
        //    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetPassengerOrders()
        //{
        //    PassengerOrdersBLL PassengerOrderBLL = new PassengerOrdersBLL()
        //    {
        //        Search = Search,
        //        Order = Order,
        //        OrderDir = OrderDir,
        //        StartRec = StartRec,
        //        PageSize = PageSize
        //    };
        //    var data = PassengerOrderBLL.GetPassengerOrders(out TotalRecordsOut, out RecFilterOut)
        //        .Select(x => new
        //        {
        //            PassengerOrderID = x.PassengerOrderID,
        //            TravellingDate = x.TravellingDate,
        //            EmployeeCodeNo = x.EmployeeCode.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
        //            EmployeeNameAr = x.EmployeeCode.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
        //            Going = x.Going,
        //            Returning = x.Returning
        //        });
        //    return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        //}

        //[CustomAuthorize]
        //public ActionResult Create()
        //{
        //    Session["PassengerOrderID"] = null;
        //    Session["PassengerOrdersItineraries"] = null;
        //    return View();
        //}

        //[CustomAuthorize]
        //public ActionResult Delete(int id)
        //{
        //    return View(this.GetByPassengerOrderID(id));
        //}

        //[CustomAuthorize]
        //public ActionResult Details(int id)
        //{
        //    return View(this.GetByPassengerOrderID(id));
        //}

        //[HttpDelete]
        //[IgnoreModelProperties("TravellingDate,DelegationDetailRequest,PassengerOrdersItineraryRequest")]
        //public ActionResult Delete(PassengerOrdersViewModel PassengerOrderVM)
        //{
        //    PassengerOrdersBLL passengerOrdersBLL = new PassengerOrdersBLL();
        //    passengerOrdersBLL.LoginIdentity = UserIdentity;
        //    passengerOrdersBLL.Remove(PassengerOrderVM.PassengerOrderID);
        //    //return View(PassengerOrderVM); 
        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //[IgnoreModelProperties("PassengerOrdersItineraryRequest,DelegationStartDate,DelegationEndDate,InternshipScholarshipStartDate,InternshipScholarshipEndDate")]
        //public ActionResult Create(PassengerOrdersViewModel PassengerOrderVM)
        //{
        //    PassengerOrdersBLL PassengerOrder = new PassengerOrdersBLL();
        //    PassengerOrder.PassengerOrderType = new PassengerOrdersTypesBLL() { PassengerOrderTypeID = PassengerOrderVM.PassengerOrderTypeID };

        //    if (PassengerOrderVM.PassengerOrderTypeID == Convert.ToInt32(PassengerOrdersTypesEnum.Delegation))
        //        PassengerOrder.EmployeeCode = new DelegationPassengerOrderBLL() { DetailID = PassengerOrderVM.DelegationDetailRequest.DelegationDetailID.Value };
        //    else if (PassengerOrderVM.PassengerOrderTypeID == Convert.ToInt32(PassengerOrdersTypesEnum.InternshipScholarship))
        //        PassengerOrder.EmployeeCode = new InternshipsScholarshipPassengerOrderBLL() { DetailID = PassengerOrderVM.InternshipScholarshipDetailRequest.InternshipScholarshipDetailID.Value };

        //    if (PassengerOrderVM.RankID > 0)
        //        PassengerOrder.Rank = new RanksBLL() { RankID = PassengerOrderVM.RankID };

        //    if (PassengerOrderVM.TicketClassID > 0)
        //        PassengerOrder.TicketClass = new TicketsClassesBLL() { TicketClassID = PassengerOrderVM.TicketClassID };
        //    PassengerOrder.TravellingDate = PassengerOrderVM.TravellingDate;
        //    PassengerOrder.Going = PassengerOrderVM.Going;
        //    PassengerOrder.Returning = PassengerOrderVM.Returning;
        //    PassengerOrder.LoginIdentity = UserIdentity;
        //    PassengerOrder.PassengerOrdersItineraries = GetPassengerOrderItinerarysFromSession();
        //    Result result = PassengerOrder.Add();
        //    if ((System.Type)result.EnumType == typeof(PassengerOrdersValidationEnum))
        //    {
        //        PassengerOrdersBLL PassengerOrderEntity = (PassengerOrdersBLL)result.Entity;
        //        if (result.EnumMember == PassengerOrdersValidationEnum.Done.ToString())
        //        {
        //            PassengerOrderVM.PassengerOrderID = ((PassengerOrdersBLL)result.Entity).PassengerOrderID;
        //            ClearPassengerOrderItinerarysFromSession();
        //        }
        //        else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecauseItineraryRequired.ToString())
        //        {
        //            throw new CustomException(Resources.Globalization.ValidationItineraryRequiredText);
        //        }
        //        else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecausePassengerOrderAlreadyTook.ToString())
        //        {
        //            throw new CustomException(Resources.Globalization.ValidationPassengerOrderAlreadyTookText);
        //        }
        //        else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecausePassengerOrderCompensation.ToString())
        //        {
        //            throw new CustomException(Resources.Globalization.ValidationPassengerOrderCompensationText);
        //        }
        //    }

        //    return Json(new { PassengerOrderID = PassengerOrderVM.PassengerOrderID }, JsonRequestBehavior.AllowGet);
        //}

        //private List<PassengerOrdersItinerariesBLL> GetPassengerOrderItinerarysFromSession()
        //{
        //    List<PassengerOrdersItinerariesBLL> PassengerOrdersItinerariesList;
        //    if (Session["PassengerOrdersItineraries"] != null)
        //        PassengerOrdersItinerariesList = (List<PassengerOrdersItinerariesBLL>)Session["PassengerOrdersItineraries"];
        //    else
        //        PassengerOrdersItinerariesList = new List<PassengerOrdersItinerariesBLL>();

        //    return PassengerOrdersItinerariesList;
        //}

        //private void ClearPassengerOrderItinerarysFromSession()
        //{
        //    Session["PassengerOrdersItineraries"] = null;
        //}

        //[CustomAuthorize]
        //public ActionResult Edit(int id)
        //{
        //    return View(this.GetByPassengerOrderID(id));
        //}

        //[HttpPost]
        //[ActionName("Edit")]
        //[IgnoreModelProperties("PassengerOrdersItineraryRequest,DelegationStartDate,DelegationEndDate")]
        //public ActionResult EditPassengerOrder(PassengerOrdersViewModel PassengerOrderVM)
        //{
        //    PassengerOrdersBLL PassengerOrder = new PassengerOrdersBLL();
        //    PassengerOrder.PassengerOrderID = PassengerOrderVM.PassengerOrderID;
        //    PassengerOrder.PassengerOrderType = new PassengerOrdersTypesBLL() { PassengerOrderTypeID = PassengerOrderVM.PassengerOrderTypeID };

        //    if (PassengerOrderVM.PassengerOrderTypeID == (int)PassengerOrdersTypesEnum.Delegation)
        //        PassengerOrder.EmployeeCode = new DelegationPassengerOrderBLL() { DetailID = PassengerOrderVM.DelegationDetailRequest.DelegationDetailID.Value };
        //    else if (PassengerOrderVM.PassengerOrderTypeID == (int)PassengerOrdersTypesEnum.InternshipScholarship)
        //        PassengerOrder.EmployeeCode = new InternshipsScholarshipPassengerOrderBLL() { DetailID = PassengerOrderVM.InternshipScholarshipDetailRequest.InternshipScholarshipDetailID.Value };

        //    if (PassengerOrderVM.RankID > 0)
        //        PassengerOrder.Rank = new RanksBLL() { RankID = PassengerOrderVM.RankID };

        //    if (PassengerOrderVM.TicketClassID > 0)
        //        PassengerOrder.TicketClass = new TicketsClassesBLL() { TicketClassID = PassengerOrderVM.TicketClassID };

        //    PassengerOrder.TravellingDate = PassengerOrderVM.TravellingDate;
        //    PassengerOrder.Going = PassengerOrderVM.Going;
        //    PassengerOrder.Returning = PassengerOrderVM.Returning;
        //    PassengerOrder.LoginIdentity = UserIdentity;
        //    Result result = PassengerOrder.Update();
        //    if ((System.Type)result.EnumType == typeof(PassengerOrdersValidationEnum))
        //    {
        //        PassengerOrdersBLL PassengerOrderEntity = (PassengerOrdersBLL)result.Entity;
        //        if (result.EnumMember == PassengerOrdersValidationEnum.Done.ToString())
        //        {
        //            Session["PassengerOrderID"] = ((PassengerOrdersBLL)result.Entity).PassengerOrderID;
        //            ClearPassengerOrderItinerarysFromSession();
        //        }
        //        else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecauseItineraryRequired.ToString())
        //        {
        //            throw new CustomException(Resources.Globalization.ValidationItineraryRequiredText);
        //        }
        //        else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecausePassengerOrderAlreadyTook.ToString())
        //        {
        //            throw new CustomException(Resources.Globalization.ValidationPassengerOrderAlreadyTookText);
        //        }

        //    }

        //    return View(PassengerOrderVM);
        //}

        //[CustomAuthorize]
        //public ActionResult PrintPassengerOrder(int id)
        //{
        //    return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.PassengerOrders.ToString(), id));
        //}

        //[NonAction]
        //private PassengerOrdersViewModel GetByPassengerOrderID(int id)
        //{
        //    PassengerOrdersBLL PassengerOrderBLL = new PassengerOrdersBLL().GetByPassengerOrderID(id);
        //    PassengerOrdersViewModel PassengerOrderVM = new PassengerOrdersViewModel();
        //    if (PassengerOrderBLL != null)
        //    {
        //        PassengerOrderVM.PassengerOrderID = PassengerOrderBLL.PassengerOrderID;
        //        PassengerOrderVM.RankID = PassengerOrderBLL.Rank.RankID;
        //        PassengerOrderVM.RankName = PassengerOrderBLL.Rank.RankName;
        //        PassengerOrderVM.TicketClassName = PassengerOrderBLL.TicketClass.TicketClassName;
        //        PassengerOrderVM.TicketClassID = PassengerOrderBLL.TicketClass.TicketClassID;
        //        PassengerOrderVM.TravellingDate = PassengerOrderBLL.TravellingDate;
        //        PassengerOrderVM.Going = PassengerOrderBLL.Going;
        //        PassengerOrderVM.Returning = PassengerOrderBLL.Returning;
        //        PassengerOrderVM.CreatedDate = PassengerOrderBLL.CreatedDate;
        //        PassengerOrderVM.CreatedBy = PassengerOrderVM.GetCreatedByDisplayed(PassengerOrderBLL.CreatedBy);
        //        if ((PassengerOrdersTypesEnum)PassengerOrderBLL.PassengerOrderType.PassengerOrderTypeID == PassengerOrdersTypesEnum.Delegation)
        //        {
        //            PassengerOrderVM.PassengerOrderTypeID = (int)PassengerOrdersTypesEnum.Delegation;
        //            PassengerOrderVM.DelegationDetailRequest = new DelegationsDetailsViewModel()
        //            {
        //                Delegation = ((DelegationsDetailsBLL)PassengerOrderBLL.EmployeeCode).Delegation,
        //                DelegationDetailID = PassengerOrderBLL.EmployeeCode.DetailID,
        //                EmployeeCareerHistory = ((DelegationsDetailsBLL)PassengerOrderBLL.EmployeeCode).EmployeeCareerHistory
        //            };
        //        }
        //        else if ((PassengerOrdersTypesEnum)PassengerOrderBLL.PassengerOrderType.PassengerOrderTypeID == PassengerOrdersTypesEnum.InternshipScholarship)
        //        {
        //            PassengerOrderVM.PassengerOrderTypeID = (int)PassengerOrdersTypesEnum.InternshipScholarship;
        //            PassengerOrderVM.InternshipScholarshipDetailRequest = new InternshipScholarshipsDetailsViewModel()
        //            {
        //                InternshipScholarship = ((InternshipScholarshipsDetailsBLL)PassengerOrderBLL.EmployeeCode).InternshipScholarship,
        //                InternshipScholarshipDetailID = PassengerOrderBLL.EmployeeCode.DetailID,
        //                EmployeeCareerHistory = ((InternshipScholarshipsDetailsBLL)PassengerOrderBLL.EmployeeCode).EmployeeCareerHistory,
        //            };
        //        }
        //    }
        //    return PassengerOrderVM;
        //}

        //public JsonResult GetPassengerOrdersItineraries()
        //{
        //    List<PassengerOrdersItinerariesBLL> PassengerOrdersItinerariesList;
        //    if (Session["PassengerOrdersItineraries"] != null)
        //        PassengerOrdersItinerariesList = (List<PassengerOrdersItinerariesBLL>)Session["PassengerOrdersItineraries"];
        //    else
        //        PassengerOrdersItinerariesList = new List<PassengerOrdersItinerariesBLL>();

        //    return Json(new { data = PassengerOrdersItinerariesList }, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetPassengerOrdersItinerariesByPassengerOrderID(int id)
        //{
        //    return Json(new { data = new PassengerOrdersItinerariesBLL().GetPassengerOrdersItinerariesByPassengerOrderID(id) }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //[IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest")]
        //public HttpResponseMessage CreatePassengerOrderItinerary(PassengerOrdersViewModel PassengerOrderVM)
        //{
        //    List<PassengerOrdersItinerariesBLL> PassengerOrdersItinerariesList = GetPassengerOrderItinerarysFromSession();

        //    //if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.EmployeeCode.EmployeeCodeNo))
        //    //{
        //    //    throw new CustomException(Resources.Globalization.RequiredEmployeeCodeNoText);
        //    //}
        //    //else 
        //    if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity))
        //    {
        //        throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityRequiredText);
        //    }
        //    else if (PassengerOrdersItinerariesList.FindIndex(e => e.FromCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity.ToUpper())
        //                                                        && e.ToCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity.ToUpper())) > -1)
        //    {
        //        throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityAlreadyExistText);
        //    }

        //    PassengerOrdersItinerariesList.Add(PassengerOrderVM.PassengerOrdersItineraryRequest);
        //    Session["PassengerOrdersItineraries"] = PassengerOrdersItinerariesList;

        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        //[HttpPost]
        //[IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest")]
        //public HttpResponseMessage RemoveItineraryFromPassengerOrder(PassengerOrdersViewModel PassengerOrderVM)
        //{
        //    List<PassengerOrdersItinerariesBLL> PassengerOrdersItinerariesList = GetPassengerOrderItinerarysFromSession();
        //    PassengerOrdersItinerariesList.RemoveAt(PassengerOrdersItinerariesList
        //                                                .FindIndex(e => e.FromCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity.ToUpper())
        //                                                            && e.ToCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity.ToUpper())));
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        //[HttpPost]
        //[IgnoreModelProperties("PassengerOrderID,TravellingDateText,DelegationDetailRequest,PassengerOrdersItineraryRequest")]
        //public HttpResponseMessage ResetItineraryFromSession()
        //{
        //    Session["PassengerOrdersItineraries"] = null;
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        //[HttpPost]
        //[IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest")]
        //public HttpResponseMessage CreatePassengerOrderItineraryDB(PassengerOrdersViewModel PassengerOrderVM)
        //{
        //    if (string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity) || string.IsNullOrEmpty(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity))
        //    {
        //        throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityRequiredText);
        //    }
        //    //else if (PassengerOrdersItinerariesList.FindIndex(e => e.FromCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.FromCity.ToUpper())
        //    //                                                    && e.ToCity.ToUpper().Equals(PassengerOrderVM.PassengerOrdersItineraryRequest.ToCity.ToUpper())) > -1)
        //    //{
        //    //    throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityAlreadyExist);
        //    //}
        //    PassengerOrderVM.PassengerOrdersItineraryRequest.LoginIdentity = UserIdentity;
        //    Result result = new PassengerOrdersItinerariesBLL().Add(PassengerOrderVM.PassengerOrdersItineraryRequest);

        //    if ((System.Type)result.EnumType == typeof(PassengerOrdersValidationEnum))
        //    {
        //        if (result.EnumMember == PassengerOrdersValidationEnum.Done.ToString())
        //        {
        //            return new HttpResponseMessage(HttpStatusCode.OK);
        //        }
        //        else if (result.EnumMember == PassengerOrdersValidationEnum.RejectedBecausePassengerOrderItineraryCityAlreadyExist.ToString())
        //        {
        //            throw new CustomException(Resources.Globalization.ValidationPassengerOrderItineraryCityAlreadyExistText);
        //        }
        //    }
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        //[HttpPost]
        //[IgnoreModelProperties("PassengerOrderID,TravellingDate,DelegationDetailRequest")]
        //public HttpResponseMessage RemovePassengerOrderItineraryDB(int id)
        //{
        //    PassengerOrdersItinerariesBLL passengerOrdersItinerariesBLL = new PassengerOrdersItinerariesBLL();
        //    passengerOrdersItinerariesBLL.LoginIdentity = UserIdentity;
        //    passengerOrdersItinerariesBLL.Remove(id);
        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}

        //[CustomAuthorize]
        //public ActionResult PrintAllPassengerOrdersByEmployeeCodeID(int id)
        //{
        //    return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.PassengerOrders.ToString(), id));
        //}
    }
}