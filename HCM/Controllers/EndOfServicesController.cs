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
    public class EndOfServicesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByEndOfServiceID(id));
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByEndOfServiceID(id));
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByEndOfServiceID(id));
        }
        [HttpGet]
        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "EndOfServiceID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "EndOfServiceDate", Resources.Globalization.EndOfServiceDateText },
                { "EndOfServiceDecisionNo", Resources.Globalization.EndOfServiceDecisionNoText },
                { "EndOfServiceDecisionDate", Resources.Globalization.EndOfServiceDecisionDateText },
                { "EndOfServiceCase", Resources.Globalization.EndOfServiceCaseText },
                { "EndOfServiceReason", Resources.Globalization.EndOfServiceReasonText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetAllEndOfServices(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllEndOfServices()
        {
            var data = new EndOfServicesBLL().GetEndOfServices().Select(item => new
            {
                EndOfServiceID = item.EndOfServiceID,
                EndOfServiceDate = Globals.Calendar.GetUmAlQuraDate(item.EndOfServiceDate),
                EndOfServiceDecisionDate = Globals.Calendar.GetUmAlQuraDate(item.EndOfServiceDecisionDate),
                EndOfServiceDecisionNo = item.EndOfServiceDecisionNo,
                EndOfServiceReason = item.EndOfServiceReason.EndOfServiceReason,
                EndOfServiceCase = item.EndOfServiceReason.EndOfServiceCase.EndOfServiceCase,
                EmployeeNameAr = item.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                EmployeeCodeNo = item.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
            });

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEndOfServices()
        {
            var data = new EndOfServicesBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            }.GetEndOfServices(out TotalRecordsOut, out RecFilterOut).Select(item => new
            {
                EndOfServiceID = item.EndOfServiceID,
                EndOfServiceDate = item.EndOfServiceDate,
                EndOfServiceDecisionDate = item.EndOfServiceDecisionDate,
                EndOfServiceDecisionNo = item.EndOfServiceDecisionNo,
                EndOfServiceReason = item.EndOfServiceReason.EndOfServiceReason,
                EndOfServiceCase = item.EndOfServiceReason.EndOfServiceCase.EndOfServiceCase,
                EmployeeNameAr = item.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                EmployeeCodeNo = item.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
            });

            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EndOfServiceVacationStartDate,EndOfServiceVacationEndDate")]
        public ActionResult Create(EndOfServicesViewModel EndOfServicesVM)
        {
            EndOfServicesBLL EndOfServiceBLL = new EndOfServicesBLL();
            EndOfServiceBLL.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = EndOfServicesVM.EmployeeCareerHistoryID };
            EndOfServiceBLL.EndOfServiceDate = EndOfServicesVM.EndOfServiceDate;
            EndOfServiceBLL.EndOfServiceDecisionNo = EndOfServicesVM.EndOfServiceDecisionNo;
            EndOfServiceBLL.EndOfServiceDecisionDate = EndOfServicesVM.EndOfServiceDecisionDate;
            EndOfServiceBLL.EndOfServiceReason = new EndOfServicesReasonsBLL() { EndOfServiceReasonID = EndOfServicesVM.EndOfServiceReasonID };
            EndOfServiceBLL.LoginIdentity = UserIdentity;
            Result result = EndOfServiceBLL.Add();
            if (result.EnumMember == EndOfServicesValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == EndOfServicesValidationEnum.RejectedBecauseOfConflictWithStopWorks.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationConflictWithStopWorkText);
            }
            //else if (result.EnumMember == EndOfServicesValidationEnum.RejectedBecauseOfMonthlyDeductionAmountShouldNotGreaterThanTotalDeductionAmount.ToString())
            //{
            //    throw new CustomException(Resources.Globalization.ValidationDeductionAmountShouldNotGreaterThenTotalDeductionAmountText);
            //}

            return Json(new { EndOfServiceID = EndOfServiceBLL.EndOfServiceID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeCareerHistoryID,EndOfServiceVacationStartDate,EndOfServiceVacationEndDate")]
        public ActionResult Edit(EndOfServicesViewModel EndOfServicesVM)
        {
            EndOfServicesBLL EndOfServiceBLL = new EndOfServicesBLL();
            EndOfServiceBLL.EndOfServiceID = EndOfServicesVM.EndOfServiceID;
            EndOfServiceBLL.EndOfServiceDate = EndOfServicesVM.EndOfServiceDate;
            EndOfServiceBLL.EndOfServiceDecisionNo = EndOfServicesVM.EndOfServiceDecisionNo;
            EndOfServiceBLL.EndOfServiceDecisionDate = EndOfServicesVM.EndOfServiceDecisionDate;
            EndOfServiceBLL.EndOfServiceReason = new EndOfServicesReasonsBLL() { EndOfServiceReasonID = EndOfServicesVM.EndOfServiceReasonID };
            EndOfServiceBLL.LoginIdentity = UserIdentity;

            Result result = EndOfServiceBLL.Update();
            if (result.EnumMember == EndOfServicesValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == EndOfServicesValidationEnum.RejectedBecauseOfConflictWithStopWorks.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationConflictWithStopWorkText);
            }
            else if (result.EnumMember == EndOfServicesValidationEnum.RejectedBecauseOfEndOfServicesDateIsPassedAway.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationBecauseOfEndOfServicesDateIsPassedAwayText);
            }

            return Json(new { EndOfServiceID = EndOfServiceBLL.EndOfServiceID }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [IgnoreModelProperties("EndOfServiceDate,EmployeeCareerHistoryID,EndOfServiceDecisionNo,EndOfServiceDecisionDate,EndOfServiceCaseID,EndOfServiceReasonID,EndOfServiceVacationStartDate,EndOfServiceVacationEndDate")]
        public ActionResult Delete(EndOfServicesViewModel EndOfServicesVM)
        {
            EndOfServicesBLL EndOfService = new EndOfServicesBLL();
            EndOfService.LoginIdentity = UserIdentity;
            Result result = EndOfService.Remove(EndOfServicesVM.EndOfServiceID);
            if (result.EnumMember == EndOfServicesValidationEnum.Done.ToString())
            {

            }
            else if (result.EnumMember == EndOfServicesValidationEnum.RejectedBecauseOfEndOfServicesDateIsPassedAway.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationBecauseOfEndOfServicesDateIsPassedAwayText);
            }
            return RedirectToAction("Index");
        }

        [NonAction]
        private EndOfServicesViewModel GetByEndOfServiceID(int id)
        {
            EndOfServicesBLL EndOfServiceBLL = new EndOfServicesBLL().GetByEndOfServiceID(id);
            EndOfServicesViewModel EndOfServiceVM = new EndOfServicesViewModel();
            if (EndOfServiceBLL != null)
            {
                EndOfServiceVM.EndOfServiceID = EndOfServiceBLL.EndOfServiceID;
                EndOfServiceVM.EmployeeCareerHistoryID = EndOfServiceBLL.EmployeeCareerHistory.EmployeeCareerHistoryID;
                EndOfServiceVM.EndOfServiceDate = EndOfServiceBLL.EndOfServiceDate.Date;
                EndOfServiceVM.EndOfServiceDecisionNo = EndOfServiceBLL.EndOfServiceDecisionNo;
                EndOfServiceVM.EndOfServiceDecisionDate = EndOfServiceBLL.EndOfServiceDecisionDate.Date;
                EndOfServiceVM.EndOfServiceReasonID = EndOfServiceBLL.EndOfServiceReason.EndOfServiceReasonID;
                EndOfServiceVM.EndOfServiceReason = EndOfServiceBLL.EndOfServiceReason;
                EndOfServiceVM.EndOfServiceCaseID = EndOfServiceBLL.EndOfServiceReason.EndOfServiceCase.EndOfServiceCaseID;

                EndOfServiceVM.EmployeeVM = new EmployeesViewModel()
                {
                    EmployeeCareerHistoryID = EndOfServiceBLL.EmployeeCareerHistory.EmployeeCareerHistoryID,
                    EmployeeCodeID = EndOfServiceBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,
                    EmployeeCodeNo = EndOfServiceBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = EndOfServiceBLL.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr
                };

                EndOfServiceVM.CreatedDate = EndOfServiceBLL.CreatedDate;
                EndOfServiceVM.CreatedBy = EndOfServiceVM.GetCreatedByDisplayed(EndOfServiceBLL.CreatedBy);
            }
            return EndOfServiceVM;
        }

        [HttpGet]
        public JsonResult GetEndOfServicesReasonsByCaseID(int id)
        {
            return Json(new { data = new EndOfServicesReasonsBLL().GetByEndOfServiceCaseID(id) }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult PrintVacationsByEndOfServiceID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.VacationsByEndOfService.ToString(), id));
        }

        [CustomAuthorize]
        public ActionResult PrintEmployeeCareerHistoryByEndOfServiceID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.EmployeeCareerHistoryByEndOfService.ToString(), id));
        }

        #region EndOfServiceVacation
        [CustomAuthorize]
        public ActionResult AddEndOfServiceVacations(int id)
        {
            return View(this.GetByEndOfServiceID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("EndOfServiceDate,EndOfServiceDecisionDate,EndOfServiceDecisionNo")]
        public HttpResponseMessage AddEndOfServiceVacations(EndOfServicesViewModel EndOfServiceVM)
        {
            EndOfServicesVacationsBLL EndOfServiceVacation = new EndOfServicesVacationsBLL()
            {
                EndOfService = new EndOfServicesBLL() { EndOfServiceID = EndOfServiceVM.EndOfServiceID },
                VacationStartDate = (DateTime)EndOfServiceVM.EndOfServiceVacationStartDate,
                VacationEndDate = (DateTime)EndOfServiceVM.EndOfServiceVacationEndDate,
                VacationType = EndOfServiceVM.EndOfServiceVacationType,
                LoginIdentity = UserIdentity

            };

            Result result = EndOfServiceVacation.Add();
            if ((System.Type)result.EnumType == typeof(EndOfServicesVacationsValidationEnum))
            {
                if (result.EnumMember == EndOfServicesVacationsValidationEnum.RejectedBecauseOfVacationEndDateBiggerThanEndOfServiceDate.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationVacationStartDateBiggerThanEndOfServiceDateText);
                }

            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public JsonResult GetEndOfServicesVacationsByEndOfServiceID(int id)
        {
            List<EndOfServicesVacationsBLL> EndOfServicesVacationsList = new EndOfServicesVacationsBLL().GetByEndOfServiceID(id);
            return Json(new { data = EndOfServicesVacationsList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpResponseMessage RemoveEndOfServiceVacation(int id)
        {
            new EndOfServicesVacationsBLL() { LoginIdentity = UserIdentity }.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        #endregion

        #region Print
        //[CustomAuthorize]
        [Route("{controller}/PrintEndOfService/{EndOfServiceID}")]
        public ActionResult PrintEndOfService(int EndOfServiceID)
        {
            return Redirect(string.Format("~/WebForms/Reports/EndOfService.aspx?EndOfServiceID={0}", EndOfServiceID));
        }
        #endregion

        #region ExpectedCompensationAfterEndOfServices
        [HttpGet]
        [CustomAuthorize]
        public ActionResult ExpectedCompensationAfterEndOfServices()
        {
            return View();
        }

        [HttpGet]
        [Route("{controller}/GetExpectedCompensationAfterEndOfServices/{EmployeeCodeNo}")]
        public JsonResult GetExpectedCompensationAfterEndOfServices(string EmployeeCodeNo = "")
        {
            if (EmployeeCodeNo == "0") EmployeeCodeNo = string.Empty;
            List<EmployeesBenefitsAfterEndOfServiceBLL> EmployeesBenefitsAfterEndOfServiceBLL = new EmployeesBenefitsAfterEndOfServiceBLL().GetEmployeesBenefits(EmployeeCodeNo);
            var Employees = EmployeesBenefitsAfterEndOfServiceBLL.Select(x => new
            {
                EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                RankCategoryName = x.EmployeeCareerHistory.OrganizationJob.Rank.RankCategory.RankCategoryName,
                RankName = x.EmployeeCareerHistory.OrganizationJob.Rank.RankName,
                OrganizationName = "المؤسسة",
                HiringDate = x.EmployeeHiringRecord.JoinDate,
                BasicSalary = x.SalaryDetails.Benefits != null ? x.SalaryDetails.Benefits.BasicSalary : 0,
                TotalSalary = x.SalaryDetails.TotalSalary.ToString(),
                RemainingNormalVacationBalance = x.TotalRemainingBalance.ToString(),
                NormalVacationCompensation = x.RemainingNormalVacationBalanceCompensation.ToString(),
                ServicePeriod = x.ServicePeriod.ToString() + " " + Resources.Globalization.UmAlquraYearText,
                EndOfService = x.EndOfServiceCompensation.ToString(),
                RemainingYearsCountInService = x.RemainingYearsCountInService.ToString(),
                AdditionalCompensation = x.AdditionalCompensation.ToString(),
                Age = "0" + " " + Resources.Globalization.UmAlquraYearText,
            });

            List<EmployeesPrivateCompaniesBenefitsAfterEndOfServiceBLL> EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL = new EmployeesPrivateCompaniesBenefitsAfterEndOfServiceBLL().GetEmployeesBenefits(EmployeeCodeNo);
            var PrivateEmployees = EmployeePrivateCompaniesBenefitsAfterEndOfServiceBLL.Select(x => new
            {
                EmployeeCodeNo = x.Employee.EmployeeCodeNo,
                EmployeeNameAr = x.Employee.EmployeeNameAr,
                RankCategoryName = x.Employee.RankCategoryName,
                RankName = x.Employee.RankName,
                OrganizationName = x.Employee.OrganizationName,
                HiringDate = x.Employee.HiringDate.Value.Date,
                BasicSalary = x.Employee.BasicSalary.ToString(),
                TotalSalary = x.Employee.TotalSalary.ToString(),
                RemainingNormalVacationBalance = x.Employee.RemainingNormalVacationBalance.ToString(),
                NormalVacationCompensation = x.RemainingNormalVacationBalanceCompensation.ToString(),
                ServicePeriod = x.ServicePeriod.ToString() + " " + Resources.Globalization.GregYearText,
                EndOfService = x.EndOfServiceCompensation.ToString(),
                RemainingYearsCountInService = x.RemainingYearsCountInService.ToString(),
                AdditionalCompensation = x.AdditionalCompensation.ToString(),
                Age = x.Employee.Age.ToString() + " " + Resources.Globalization.GregYearText,
            });

            var AllData = (from x in Employees
                           select new
                           {
                               EmployeeCodeNo = x.EmployeeCodeNo,
                               EmployeeNameAr = x.EmployeeNameAr,
                               RankCategoryName = x.RankCategoryName,
                               RankName = x.RankName,
                               OrganizationName = x.OrganizationName,
                               HiringDate = x.HiringDate.Date,
                               BasicSalary = x.BasicSalary.ToString(),
                               TotalSalary = x.TotalSalary.ToString(),
                               RemainingNormalVacationBalance = x.RemainingNormalVacationBalance,
                               NormalVacationCompensation = x.NormalVacationCompensation,
                               ServicePeriod = x.ServicePeriod,
                               EndOfService = x.EndOfService,
                               RemainingYearsCountInService = x.RemainingYearsCountInService,
                               AdditionalCompensation = x.AdditionalCompensation,
                               Age = x.Age
                           }).Union(from x in PrivateEmployees
                                    select new
                                    {
                                        EmployeeCodeNo = x.EmployeeCodeNo,
                                        EmployeeNameAr = x.EmployeeNameAr,
                                        RankCategoryName = x.RankCategoryName,
                                        RankName = x.RankName,
                                        OrganizationName = x.OrganizationName,
                                        HiringDate = x.HiringDate.Date,
                                        BasicSalary = x.BasicSalary.ToString(),
                                        TotalSalary = x.TotalSalary.ToString(),
                                        RemainingNormalVacationBalance = x.RemainingNormalVacationBalance,
                                        NormalVacationCompensation = x.NormalVacationCompensation,
                                        ServicePeriod = x.ServicePeriod,
                                        EndOfService = x.EndOfService,
                                        RemainingYearsCountInService = x.RemainingYearsCountInService,
                                        AdditionalCompensation = x.AdditionalCompensation,
                                        Age = x.Age
                                    });

            //var jsonResult = Json(new { data = AllData }, JsonRequestBehavior.AllowGet);
            //int len = jsonResult.MaxJsonLength.HasValue ? jsonResult.MaxJsonLength.Value : 0;
            //jsonResult.MaxJsonLength = int.MaxValue;

            return SetJsonResultWithMaxJsonLength(AllData);

            //return Json(new { data = AllData }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}