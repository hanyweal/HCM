using HCM.Classes;
using HCM.Classes.CustomAttributes;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class EmployeesHolidaysSettingsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        //[CustomAuthorize]
        //public ActionResult Details(int id)
        //{
        //    return View(this.GetByEmployeeHolidaySettingID(id));
        //}

        //[CustomAuthorize]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Create(EmployeesHolidaysSettingsViewModel EmployeesHolidaysSettingsVM)
        //{
        //    EmployeesHolidaysSettingsBLL EmployeeHolidaySettingBLL = new EmployeesHolidaysSettingsBLL();
        //    EmployeeHolidaySettingBLL.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeesHolidaysSettingsVM.EmployeeCodeID };
        //    EmployeeHolidaySettingBLL.GovernmentDeductionType = EmployeesHolidaysSettingsVM.GovernmentDeductionType;
        //    EmployeeHolidaySettingBLL.EmployeeHolidaySettingType = EmployeesHolidaysSettingsVM.EmployeeHolidaySettingType;
        //    EmployeeHolidaySettingBLL.LoanNo = EmployeesHolidaysSettingsVM.LoanNo;
        //    EmployeeHolidaySettingBLL.LoanDate = EmployeesHolidaysSettingsVM.LoanDate;
        //    EmployeeHolidaySettingBLL.MonthlyDeductionAmount = EmployeesHolidaysSettingsVM.MonthlyDeductionAmount;
        //    EmployeeHolidaySettingBLL.TotalDeductionAmount = EmployeesHolidaysSettingsVM.TotalDeductionAmount;
        //    EmployeeHolidaySettingBLL.DeductionStartDate = EmployeesHolidaysSettingsVM.DeductionStartDate;
        //    EmployeeHolidaySettingBLL.ContractNo = EmployeesHolidaysSettingsVM.ContractNo;
        //    //EmployeeHolidaySettingBLL.ContractDate = EmployeesHolidaysSettingsVM.ContractDate;
        //    EmployeeHolidaySettingBLL.KSACity = new KSACitiesBLL() { KSACityID = EmployeesHolidaysSettingsVM.KSACity.KSACityID };
        //    EmployeeHolidaySettingBLL.SponserToIDNo = EmployeesHolidaysSettingsVM.SponserToIDNo;
        //    EmployeeHolidaySettingBLL.SponserToName = EmployeesHolidaysSettingsVM.SponserToName;
        //    EmployeeHolidaySettingBLL.LoginIdentity = UserIdentity;
        //    Result result = EmployeeHolidaySettingBLL.Add();
        //    if (result.EnumMember == EmployeesHolidaysSettingsValidationEnum.Done.ToString())
        //    {

        //    }

        //    return Json(new { EmployeeHolidaySettingID = EmployeeHolidaySettingBLL.EmployeeHolidaySettingID }, JsonRequestBehavior.AllowGet);
        //}

        //[CustomAuthorize]
        //public ActionResult Edit(int id)
        //{
        //    return View(this.GetByEmployeeHolidaySettingID(id));
        //}

        //[HttpPost]
        //public ActionResult Edit(EmployeesHolidaysSettingsViewModel EmployeesHolidaysSettingsVM)
        //{
        //    EmployeesHolidaysSettingsBLL EmployeeHolidaySettingBLL = new EmployeesHolidaysSettingsBLL();
        //    EmployeeHolidaySettingBLL.EmployeeHolidaySettingID = EmployeesHolidaysSettingsVM.EmployeeHolidaySettingID;
        //    EmployeeHolidaySettingBLL.EmployeeCode = new EmployeesCodesBLL() { EmployeeCodeID = EmployeesHolidaysSettingsVM.EmployeeCodeID };
        //    EmployeeHolidaySettingBLL.GovernmentDeductionType = EmployeesHolidaysSettingsVM.GovernmentDeductionType;
        //    EmployeeHolidaySettingBLL.EmployeeHolidaySettingType = EmployeesHolidaysSettingsVM.EmployeeHolidaySettingType;
        //    EmployeeHolidaySettingBLL.LoanNo = EmployeesHolidaysSettingsVM.LoanNo;
        //    EmployeeHolidaySettingBLL.LoanDate = EmployeesHolidaysSettingsVM.LoanDate;
        //    EmployeeHolidaySettingBLL.DeductionStartDate = EmployeesHolidaysSettingsVM.DeductionStartDate;
        //    EmployeeHolidaySettingBLL.MonthlyDeductionAmount = EmployeesHolidaysSettingsVM.MonthlyDeductionAmount;
        //    EmployeeHolidaySettingBLL.TotalDeductionAmount = EmployeesHolidaysSettingsVM.TotalDeductionAmount;
        //    EmployeeHolidaySettingBLL.ContractNo = EmployeesHolidaysSettingsVM.ContractNo;
        //    //EmployeeHolidaySettingBLL.ContractDate = EmployeesHolidaysSettingsVM.ContractDate;
        //    EmployeeHolidaySettingBLL.KSACity = new KSACitiesBLL() { KSACityID = EmployeesHolidaysSettingsVM.KSACity.KSACityID };
        //    EmployeeHolidaySettingBLL.SponserToIDNo = EmployeesHolidaysSettingsVM.SponserToIDNo;
        //    EmployeeHolidaySettingBLL.SponserToName = EmployeesHolidaysSettingsVM.SponserToName;
        //    EmployeeHolidaySettingBLL.LoginIdentity = UserIdentity;
        //    EmployeeHolidaySettingBLL.Update();

        //    return Json(new { EmployeeHolidaySettingID = EmployeeHolidaySettingBLL.EmployeeHolidaySettingID }, JsonRequestBehavior.AllowGet);
        //}

        //[CustomAuthorize]
        //public ActionResult Delete(int id)
        //{
        //    return View(this.GetByEmployeeHolidaySettingID(id));
        //}

        //[HttpDelete]
        //[IgnoreModelProperties("ContractDate,ContractNo,LoanNo,LoanDate,DeductionStartDate,MonthlyDeductionAmount,TotalDeductionAmount")]
        //public ActionResult Delete(EmployeesHolidaysSettingsViewModel EmployeesHolidaysSettingsVM)
        //{
        //    try
        //    {
        //        EmployeesHolidaysSettingsBLL EmployeeHolidaySettingBLL = new EmployeesHolidaysSettingsBLL();
        //        EmployeeHolidaySettingBLL.LoginIdentity = UserIdentity;
        //        EmployeeHolidaySettingBLL.Remove(EmployeesHolidaysSettingsVM.EmployeeHolidaySettingID);
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //[HttpGet]
        //public JsonResult GetEmployeesHolidaysSettings()
        //{
        //    EmployeesHolidaysSettingsBLL EmployeesHolidaysSettingsBLL = new EmployeesHolidaysSettingsBLL()
        //    {
        //        Search = Search,
        //        Order = Order,
        //        OrderDir = OrderDir,
        //        StartRec = StartRec,
        //        PageSize = PageSize
        //    };
        //    var data = EmployeesHolidaysSettingsBLL.GetEmployeesHolidaysSettings(out TotalRecordsOut, out RecFilterOut);
        //    return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        //}

        //[NonAction]
        //private EmployeesHolidaysSettingsViewModel GetByEmployeeHolidaySettingID(int id)
        //{
        //    EmployeesHolidaysSettingsBLL EmployeeHolidaySettingBLL = new EmployeesHolidaysSettingsBLL().GetByEmployeeHolidaySettingID(id);
        //    EmployeesHolidaysSettingsViewModel EmployeeHolidaySettingVM = new EmployeesHolidaysSettingsViewModel();
        //    if (EmployeeHolidaySettingBLL != null)
        //    {
        //        EmployeeHolidaySettingVM.EmployeeHolidaySettingID = EmployeeHolidaySettingBLL.EmployeeHolidaySettingID;
        //        EmployeeHolidaySettingVM.EmployeeCodeID = EmployeeHolidaySettingBLL.EmployeeCode.EmployeeCodeID;
        //        EmployeeHolidaySettingVM.LoanNo = EmployeeHolidaySettingBLL.LoanNo;
        //        EmployeeHolidaySettingVM.LoanDate = EmployeeHolidaySettingBLL.LoanDate;
        //        EmployeeHolidaySettingVM.DeductionStartDate = EmployeeHolidaySettingBLL.DeductionStartDate;
        //        EmployeeHolidaySettingVM.MonthlyDeductionAmount = EmployeeHolidaySettingBLL.MonthlyDeductionAmount;
        //        EmployeeHolidaySettingVM.TotalDeductionAmount = EmployeeHolidaySettingBLL.TotalDeductionAmount;
        //        EmployeeHolidaySettingVM.EmployeeCode = EmployeeHolidaySettingBLL.EmployeeCode;
        //        EmployeeHolidaySettingVM.EmployeeHolidaySettingType = EmployeeHolidaySettingBLL.EmployeeHolidaySettingType;
        //        EmployeeHolidaySettingVM.GovernmentDeductionType = EmployeeHolidaySettingBLL.GovernmentDeductionType;
        //        EmployeeHolidaySettingVM.ContractNo = EmployeeHolidaySettingBLL.ContractNo;
        //        //EmployeeHolidaySettingVM.ContractDate = EmployeeHolidaySettingBLL.ContractDate;
        //        if (EmployeeHolidaySettingBLL.KSACity != null)
        //        {
        //            EmployeeHolidaySettingVM.KSACity = new KSACitiesBLL() { KSACityID = EmployeeHolidaySettingBLL.KSACity.KSACityID };
        //        }
        //        EmployeeHolidaySettingVM.SponserToIDNo = EmployeeHolidaySettingBLL.SponserToIDNo;
        //        EmployeeHolidaySettingVM.SponserToName = EmployeeHolidaySettingBLL.SponserToName;
        //        EmployeeHolidaySettingVM.CreatedDate = EmployeeHolidaySettingBLL.CreatedDate;
        //        EmployeeHolidaySettingVM.CreatedBy = EmployeeHolidaySettingVM.GetCreatedByDisplayed(EmployeeHolidaySettingBLL.CreatedBy);

        //        EmployeeHolidaySettingVM.EmployeeVM = new EmployeesViewModel()
        //        {
        //            EmployeeCodeID = EmployeeHolidaySettingBLL.EmployeeCode.EmployeeCodeID,
        //            EmployeeCodeNo = EmployeeHolidaySettingBLL.EmployeeCode.EmployeeCodeNo,
        //            EmployeeNameAr = EmployeeHolidaySettingBLL.EmployeeCode.Employee.EmployeeNameAr
        //        };

        //    }
        //    return EmployeeHolidaySettingVM;
        //}

        //[CustomAuthorize]
        //public ActionResult PrintEmployeeHolidaySetting(int id)
        //{
        //    return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.EmployeesHolidaysSettings.ToString(), id));
        //}

        //[CustomAuthorize]
        //public ActionResult PrintAllEmployeeHolidaySettingByEmployeeCodeID(int id)
        //{
        //    return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.EmployeesHolidaysSettings.ToString(), id));
        //}
    }
}
