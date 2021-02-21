using ExceptionNameSpace;
using Globals;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Data;
using HCM.Classes.Helpers;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;

namespace HCM.Controllers
{
    public partial class EmployeesController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetEmployees()
        {
            return Json(new
            {
                data = new EmployeesBLL().GetEmployees().Select(
                item => new
                {
                    EmployeeID = item.EmployeeID,
                    EmployeeIDNo = item.EmployeeIDNo,
                    EmployeeNameAr = item.EmployeeNameAr,
                    EmployeeMobileNo = item.EmployeeMobileNo,
                    EmployeeBirthDate = item.EmployeeBirthDate,
                    EmployeeBirthPlace = item.EmployeeBirthPlace,
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("Employees/GetEmployeesByName/{EmployeeNameAr}")]
        public JsonResult GetEmployeesByName(string EmployeeNameAr)
        {
            return Json(new
            {
                data = new EmployeesBLL().GetEmployees().Where(e => e.EmployeeNameAr.Contains(EmployeeNameAr)).Select(item => new
                {
                    EmployeeID = item.EmployeeID,
                    EmployeeIDNo = item.EmployeeIDNo,
                    EmployeeNameAr = item.EmployeeNameAr,
                    EmployeeMobileNo = item.EmployeeMobileNo,
                    EmployeeBirthDate = item.EmployeeBirthDate,
                    EmployeeBirthPlace = item.EmployeeBirthPlace,
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetEmployeesCodes()
        {
            var data = new EmployeesCodesBLL()
            {
                Search = Search,
                Order = Order,
                OrderDir = OrderDir,
                StartRec = StartRec,
                PageSize = PageSize
            }.GetEmployeesCodes(out TotalRecordsOut, out RecFilterOut).Select(item => new
            {
                EmployeeCodeID = item.EmployeeCodeID,
                EmployeeCodeNo = item.EmployeeCodeNo,
                EmployeeIDNo = item.Employee.EmployeeIDNo,
                EmployeeNameAr = item.Employee.EmployeeNameAr,
            });

            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetByEmployeeCodeID(int id)
        {
            return Json(new { data = new EmployeesCodesBLL().GetByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{controller}/GetCurrentActualOrgAndActualJob/{EmployeeCodeNo}")]
        public JsonResult GetCurrentActualOrgAndActualJob(string EmployeeCodeNo)
        {
            return Json(new { data = new PlacementBLL().GetCurrentActualOrgAndActualJob(EmployeeCodeNo) }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmployeesViewModel EmployeeVM)
        {

            EmployeesBLL _employeeBll = new EmployeesBLL()
            {
                EmployeeIDNo = EmployeeVM.EmployeeIDNo,
                FirstNameAr = EmployeeVM.FirstNameAr,
                MiddleNameAr = EmployeeVM.MiddleNameAr,
                GrandFatherNameAr = EmployeeVM.GrandFatherNameAr,
                LastNameAr = EmployeeVM.LastNameAr,
                FirstNameEn = EmployeeVM.FirstNameEn,
                MiddleNameEn = EmployeeVM.MiddleNameEn,
                GrandFatherNameEn = EmployeeVM.GrandFatherNameEn,
                LastNameEn = EmployeeVM.LastNameEn,
                EmployeeBirthDate = EmployeeVM.EmployeeBirthDate,
                EmployeeBirthPlace = EmployeeVM.EmployeeBirthPlace,
                EmployeeMobileNo = EmployeeVM.EmployeeMobileNo,
                EmployeePassportNo = EmployeeVM.EmployeePassportNo,
                EmployeeEMail = EmployeeVM.EmployeeEMail,
                EmployeeIDIssueDate = EmployeeVM.EmployeeIDIssueDate,
                EmployeePassportSource = EmployeeVM.EmployeePassportSource,
                EmployeePassportIssueDate = EmployeeVM.EmployeePassportIssueDate,
                EmployeePassportEndDate = EmployeeVM.EmployeePassportEndDate,
                EmployeeIDExpiryDate = EmployeeVM.EmployeeIDExpiryDate,
                EmployeeIDCopyNo = EmployeeVM.EmployeeIDCopyNo,
                EmployeeIDIssuePlace = EmployeeVM.EmployeeIDIssuePlace,
                DependentCount = EmployeeVM.DependentCount,
                MaritalStatus = new MaritalStatusBLL() { MaritalStatusID = EmployeeVM.MaritalStatus.MaritalStatusID },
                Gender = new GendersBLL() { GenderID = EmployeeVM.GenderID },
                Nationality = new CountriesBLL { CountryID = EmployeeVM.CountryID },
                LoginIdentity = UserIdentity
            };
            _employeeBll.Add();
            return View(EmployeeVM);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            EmployeesBLL _employeeBll;
            EmployeesViewModel _employeeVM = new EmployeesViewModel();

            if (id != 0)
            {
                _employeeBll = new EmployeesBLL().GetByEmployeeID(id);
                _employeeVM = new EmployeesViewModel()
                {
                    EmployeeID = _employeeBll.EmployeeID,
                    EmployeeIDNo = _employeeBll.EmployeeIDNo,
                    FirstNameAr = _employeeBll.FirstNameAr,
                    MiddleNameAr = _employeeBll.MiddleNameAr,
                    GrandFatherNameAr = _employeeBll.GrandFatherNameAr,
                    LastNameAr = _employeeBll.LastNameAr,
                    FirstNameEn = _employeeBll.FirstNameEn,
                    MiddleNameEn = _employeeBll.MiddleNameEn,
                    GrandFatherNameEn = _employeeBll.GrandFatherNameEn,
                    LastNameEn = _employeeBll.LastNameEn,
                    EmployeeNameAr = _employeeBll.EmployeeNameAr,
                    EmployeeNameEn = _employeeBll.EmployeeNameEn,
                    EmployeeBirthDate = _employeeBll.EmployeeBirthDate,
                    EmployeeBirthPlace = _employeeBll.EmployeeBirthPlace,
                    EmployeeMobileNo = _employeeBll.EmployeeMobileNo,
                    EmployeePassportNo = _employeeBll.EmployeePassportNo,
                    EmployeeEMail = _employeeBll.EmployeeEMail,
                    EmployeeIDIssueDate = _employeeBll.EmployeeIDIssueDate,
                    EmployeePassportSource = _employeeBll.EmployeePassportSource,
                    EmployeePassportIssueDate = _employeeBll.EmployeePassportIssueDate,
                    EmployeePassportEndDate = _employeeBll.EmployeePassportEndDate
                };
            }

            return View(_employeeVM);
        }

        [HttpDelete]
        [IgnoreModelProperties("EmployeeIDNo,FirstNameAr,MiddleNameAr,FifthNameAr,GrandFatherNameAr,LastNameAr,FirstNameEn,MiddleNameEn,FifthNameEn,GrandFatherNameEn,LastNameEn,EmployeeMobileNo,EmployeeBirthDate")]
        public ActionResult Delete(EmployeesViewModel EmployeeVM)
        {
            EmployeesBLL _employeeBll;
            _employeeBll = new EmployeesBLL().GetByEmployeeID(EmployeeVM.EmployeeID);
            _employeeBll.LoginIdentity = UserIdentity;
            _employeeBll.Remove();
            return View("Employees/Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(this.GetByEmployeeID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        [IgnoreModelProperties("EmployeeMobileNo,FirstNameAr,MiddleNameAr,FifthNameAr,GrandFatherNameAr,LastNameAr,FirstNameEn,MiddleNameEn,FifthNameEn,GrandFatherNameEn,LastNameEn")]
        public ActionResult EditEmployee(EmployeesViewModel EmployeeVM)
        {
            EmployeesBLL EmployeeBLL = new EmployeesBLL()
            {
                EmployeeID = EmployeeVM.EmployeeID,
                EmployeeIDNo = EmployeeVM.EmployeeIDNo,
                EmployeeBirthDate = EmployeeVM.EmployeeBirthDate,
                EmployeeBirthPlace = EmployeeVM.EmployeeBirthPlace,
                EmployeePassportNo = EmployeeVM.EmployeePassportNo,
                EmployeeEMail = EmployeeVM.EmployeeEMail,
                EmployeeIDIssueDate = EmployeeVM.EmployeeIDIssueDate,
                EmployeePassportSource = EmployeeVM.EmployeePassportSource,
                EmployeePassportIssueDate = EmployeeVM.EmployeePassportIssueDate,
                EmployeePassportEndDate = EmployeeVM.EmployeePassportEndDate,
                EmployeeIDExpiryDate = EmployeeVM.EmployeeIDExpiryDate,
                EmployeeIDCopyNo = EmployeeVM.EmployeeIDCopyNo,
                EmployeeIDIssuePlace = EmployeeVM.EmployeeIDIssuePlace,
                DependentCount = EmployeeVM.DependentCount,
                MaritalStatus = new MaritalStatusBLL() { MaritalStatusID = EmployeeVM.MaritalStatus.MaritalStatusID },
                Gender = new GendersBLL() { GenderID = EmployeeVM.GenderID },
                Nationality = new CountriesBLL { CountryID = EmployeeVM.CountryID },
                LoginIdentity = UserIdentity
            };
            EmployeeBLL.Update();
            return View(EmployeeVM);
        }

        [CustomAuthorize]
        public ActionResult EmployeeProfile()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult CreateEmployeeCodeNo()
        {
            return View();
        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult CreateEmployeeCodeNo(CreateEmployeeCodeNoViewModel EmployeeVM)
        {
            EmployeesCodesBLL employee = new EmployeesCodesBLL();
            employee.EmployeeCodeNo = EmployeeVM.EmployeeCodeNo;
            employee.Employee = new EmployeesBLL() { EmployeeID = EmployeeVM.EmployeeID };
            employee.EmployeeType = EmployeeVM.EmployeeType;
            employee.LoginIdentity = this.UserIdentity;
            employee.AddOrEdit();
            return View();
        }

        [AllowAnonymous]
        [Route("Employees/GetEmployeeByEmployeeCodeNo/{EmployeeCodeNo}")]
        [NonAction]
        public JsonResult GetEmployeeByEmployeeCodeNo(string EmployeeCodeNo)
        {
            try
            {
                EmployeesCodesBLL Employee = new EmployeesCodesBLL().GetByEmployeeCodeNo(EmployeeCodeNo);
                if (Employee == null) Employee = new EmployeesCodesBLL();
                return Json(Employee, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        public JsonResult GetEmployeeByEmployeeIDNo(string EmployeeIDNo)
        {
            try
            {
                EmployeesBLL Employee = new EmployeesBLL().GetByEmployeeIDNo(EmployeeIDNo);
                if (Employee == null) Employee = new EmployeesBLL();
                return Json(Employee, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult GetEmployeeByEmployeeNameAr(string EmployeeNameAr)
        {
            try
            {
                EmployeesBLL Employee = new EmployeesBLL().GetByEmployeeNameAr(EmployeeNameAr);
                if (Employee == null) Employee = new EmployeesBLL();
                return Json(Employee, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        public EmployeesViewModel GetByEmployeeID(int id)
        {
            EmployeesBLL _employeeBll = new EmployeesBLL().GetByEmployeeID(id);
            EmployeesViewModel EmployeeVM = new EmployeesViewModel();
            if (_employeeBll != null)
            {
                EmployeeVM.EmployeeID = _employeeBll.EmployeeID;
                EmployeeVM.EmployeeIDNo = _employeeBll.EmployeeIDNo;
                EmployeeVM.FirstNameAr = _employeeBll.FirstNameAr;
                EmployeeVM.MiddleNameAr = _employeeBll.MiddleNameAr;
                EmployeeVM.GrandFatherNameAr = _employeeBll.GrandFatherNameAr;
                EmployeeVM.LastNameAr = _employeeBll.LastNameAr;
                EmployeeVM.FirstNameEn = _employeeBll.FirstNameEn;
                EmployeeVM.MiddleNameEn = _employeeBll.MiddleNameEn;
                EmployeeVM.GrandFatherNameEn = _employeeBll.GrandFatherNameEn;
                EmployeeVM.LastNameEn = _employeeBll.LastNameEn;
                EmployeeVM.EmployeeNameAr = _employeeBll.EmployeeNameAr;
                EmployeeVM.EmployeeNameEn = _employeeBll.EmployeeNameEn;
                EmployeeVM.EmployeeBirthDate = _employeeBll.EmployeeBirthDate;
                EmployeeVM.EmployeeBirthPlace = _employeeBll.EmployeeBirthPlace;
                EmployeeVM.EmployeeMobileNo = _employeeBll.EmployeeMobileNo;
                EmployeeVM.EmployeePassportNo = _employeeBll.EmployeePassportNo;
                EmployeeVM.EmployeeEMail = _employeeBll.EmployeeEMail;
                EmployeeVM.EmployeeIDIssueDate = _employeeBll.EmployeeIDIssueDate;
                EmployeeVM.EmployeePassportSource = _employeeBll.EmployeePassportSource;
                EmployeeVM.EmployeePassportIssueDate = _employeeBll.EmployeePassportIssueDate;
                EmployeeVM.EmployeePassportEndDate = _employeeBll.EmployeePassportEndDate;
                EmployeeVM.EmployeeIDExpiryDate = _employeeBll.EmployeeIDExpiryDate;
                EmployeeVM.EmployeeIDCopyNo = _employeeBll.EmployeeIDCopyNo;
                EmployeeVM.EmployeeIDIssuePlace = _employeeBll.EmployeeIDIssuePlace;
                EmployeeVM.DependentCount = _employeeBll.DependentCount;
                if (_employeeBll.MaritalStatus != null && _employeeBll.MaritalStatus.MaritalStatusID > 0)
                    EmployeeVM.MaritalStatus = new MaritalStatusBLL() { MaritalStatusID = _employeeBll.MaritalStatus.MaritalStatusID };
                if (_employeeBll.Gender != null && _employeeBll.Gender.GenderID > 0)
                    EmployeeVM.GenderID =  _employeeBll.Gender.GenderID ;
                if (_employeeBll.Nationality != null && _employeeBll.Nationality.CountryID > 0)
                    EmployeeVM.CountryID =  _employeeBll.Nationality.CountryID ;
            }
            return EmployeeVM;
        }

        [CustomAuthorize]
        public ActionResult PrintPromotionCardByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/PromotionCard.aspx?ID={0}", id));
        }

        [CustomAuthorize]
        public ActionResult PrintEmployeeQualificationByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.EmployeesQualifications.ToString(), id));
        }

        [HttpGet]
        public ActionResult ExportExcelActiveEmployeesWithFullData()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "EmployeeIDNo", Resources.Globalization.EmployeeIDNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "GenderName", Resources.Globalization.GenderText },
                { "NationalityName", Resources.Globalization.NationalityText },
                { "EmployeeBirthDateUmAlQura", Resources.Globalization.EmployeeBirthDateUmAlQuraText },
                { "EmployeeBirthDateGreg", Resources.Globalization.EmployeeBirthDateText },
                { "EmployeeAge", Resources.Globalization.EmployeeAgeText },
                { "EmployeeBirthDateGreg", Resources.Globalization.EmployeeBirthDateText },
                { "EmployeeAge", Resources.Globalization.EmployeeAgeText },
                { "MobileNo", Resources.Globalization.EmployeeMobileNoText },
                { "HiringDate", Resources.Globalization.HiringDateText },
                { "OrganizationName", Resources.Globalization.OrganizationNameText },
                { "JoinDate", Resources.Globalization.JoinDateText },
                { "RankName", Resources.Globalization.RankNameText },
                { "CareerDegreeName", Resources.Globalization.CareerDegreeNameText },
                { "JobName", Resources.Globalization.JobNameText },
                { "JobNo", Resources.Globalization.JobNoText },
                { "ActualJobName", Resources.Globalization.ActualJobNameText },
                { "ActualOrganizationName", Resources.Globalization.ActualOrganizationNameText },
                { "TransfareAllowance", Resources.Globalization.TransfareAllowanceText },
                { "TransfareAllowance", Resources.Globalization.TransfareAllowanceText },
                { "TotalAllowances", Resources.Globalization.TotalAllowancesText },
                { "TotalDeductions", Resources.Globalization.TotalDeductionsText },
                { "NetSalary", Resources.Globalization.NetSalaryText },
                { "BasicSalary", Resources.Globalization.BasicSalaryText },
                { "QualificationDegreeName", Resources.Globalization.QualificationDegreeNameText },
                { "QualificationName", Resources.Globalization.QualificationNameText },
                { "GeneralSpecializationName", Resources.Globalization.GeneralSpecializationNameText },
                { "PreviosJobNameAssignings", Resources.Globalization.PreviosJobNameAssigningsText }
            };

            string fileName = ExcelHelper.ExportToExcel(Json(new EmployeesCodesBLL().GetActiveEmployeesWithFullData()), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        [HttpGet, Route("Employees/RetrieveImage/{EmployeeCodeNo}")]
        public ActionResult RetrieveImage(string EmployeeCodeNo)
        {
            byte[] img = new EmployeesBLL().GetEmployeePictureBytes(EmployeeCodeNo);
            if (img != null)
            {
                return File(img, "image/jpg");
            }
            else
            {
                return null;
            }
        }


        #region HiringNewEmployee
        [CustomAuthorize]
        public ActionResult HiringNewEmployee()
        {
            HiringNewEmployeesViewModel HiringNewEmployeesViewModel = new HiringNewEmployeesViewModel();
            List<AllowancesBLL> allowances = new HCMBLL.AllowancesBLL().GetAllowances();
            HiringNewEmployeesViewModel.Allowances = new List<AllowancesBLL>();
            foreach (var allowance in allowances)
            {
                if (allowance.AllowanceID == (int)AllowancesEnum.AllowanceForTheNatureOfCadresWork || allowance.AllowanceID == (int)AllowancesEnum.AllowanceForWorkHoursDiffrence)
                {
                    HiringNewEmployeesViewModel.Allowances.Add(allowance);
                }
            }

            return View(HiringNewEmployeesViewModel);
        }

        [HttpPost]
        [CustomAuthorize]
        [Route("Employees/HiringNewEmployee")]
        public ActionResult HiringNewEmployeePost(HiringNewEmployeesViewModel HiringNewEmployeesViewModel)
        {
            EmployeesBLL _employeeBll = new EmployeesBLL()
            {
                EmployeeIDNo = HiringNewEmployeesViewModel.EmployeeIDNo,
                FirstNameAr = HiringNewEmployeesViewModel.FirstNameAr,
                MiddleNameAr = HiringNewEmployeesViewModel.MiddleNameAr,
                GrandFatherNameAr = HiringNewEmployeesViewModel.GrandFatherNameAr,
                FifthNameAr = HiringNewEmployeesViewModel.FifthNameAr,
                LastNameAr = HiringNewEmployeesViewModel.LastNameAr,
                FirstNameEn = HiringNewEmployeesViewModel.FirstNameEn,
                MiddleNameEn = HiringNewEmployeesViewModel.MiddleNameEn,
                GrandFatherNameEn = HiringNewEmployeesViewModel.GrandFatherNameEn,
                FifthNameEn = HiringNewEmployeesViewModel.FifthNameEn,
                LastNameEn = HiringNewEmployeesViewModel.LastNameEn,
                EmployeeBirthDate = HiringNewEmployeesViewModel.EmployeeBirthDate,
                EmployeeBirthPlace = HiringNewEmployeesViewModel.EmployeeBirthPlace,
                EmployeeMobileNo = HiringNewEmployeesViewModel.EmployeeMobileNo,
                EmployeePassportNo = HiringNewEmployeesViewModel.EmployeePassportNo,
                EmployeeEMail = HiringNewEmployeesViewModel.EmployeeEMail,
                EmployeeIDIssueDate = HiringNewEmployeesViewModel.EmployeeIDIssueDate,
                EmployeePassportSource = HiringNewEmployeesViewModel.EmployeePassportSource,
                EmployeePassportIssueDate = HiringNewEmployeesViewModel.EmployeePassportIssueDate,
                EmployeePassportEndDate = HiringNewEmployeesViewModel.EmployeePassportEndDate,
                EmployeeIDExpiryDate = HiringNewEmployeesViewModel.EmployeeIDExpiryDate,
                EmployeeIDCopyNo = HiringNewEmployeesViewModel.EmployeeIDCopyNo,
                EmployeeIDIssuePlace = HiringNewEmployeesViewModel.EmployeeIDIssuePlace,
                DependentCount = HiringNewEmployeesViewModel.DependentCount,
                MaritalStatus = new MaritalStatusBLL() { MaritalStatusID = HiringNewEmployeesViewModel.MaritalStatus.MaritalStatusID },
                Gender = new GendersBLL() { GenderID = HiringNewEmployeesViewModel.Gender.GenderID },
                Nationality = new CountriesBLL { CountryID = HiringNewEmployeesViewModel.CountryID },
                LoginIdentity = UserIdentity
            };
            EmployeesCodesBLL _employeesCode = new EmployeesCodesBLL();
            _employeesCode.EmployeeCodeNo = HiringNewEmployeesViewModel.EmployeeCodeNo;
            _employeesCode.Employee = new EmployeesBLL() { EmployeeID = HiringNewEmployeesViewModel.EmployeeID };
            _employeesCode.EmployeeType = new EmployeesTypesBLL() { EmployeeTypeID = 1 };
            _employeesCode.LoginIdentity = this.UserIdentity;
            EmployeesCareersHistoryBLL _employeesCareersHistory = new EmployeesCareersHistoryBLL()
            {
                JoinDate = HiringNewEmployeesViewModel.JoinDate,
                OrganizationJob = new OrganizationsJobsBLL() { OrganizationJobID = HiringNewEmployeesViewModel.OrganizationJobID },
                CareerDegree = new CareersDegreesBLL() { CareerDegreeID = HiringNewEmployeesViewModel.CareerDegreeID },
                CareerHistoryType = new CareersHistoryTypesBLL() { CareerHistoryTypeID = HiringNewEmployeesViewModel.CareerHistoryTypeID },
            };
            EmployeesQualificationsBLL _employeeQualification = new EmployeesQualificationsBLL();
            _employeeQualification.QualificationDegree = new QualificationsDegreesBLL() { QualificationDegreeID = HiringNewEmployeesViewModel.QualificationDegreeID };
            _employeeQualification.Qualification = new QualificationsBLL() { QualificationID = HiringNewEmployeesViewModel.QualificationID };
            _employeeQualification.GeneralSpecialization = new GeneralSpecializationsBLL() { GeneralSpecializationID = HiringNewEmployeesViewModel.GeneralSpecializationID };
            _employeeQualification.ExactSpecialization = new ExactSpecializationsBLL() { ExactSpecializationID = HiringNewEmployeesViewModel.ExactSpecializationID.HasValue ? (int)HiringNewEmployeesViewModel.ExactSpecializationID : 0 };
            _employeeQualification.UniSchName = HiringNewEmployeesViewModel.UniSchName;
            _employeeQualification.Department = HiringNewEmployeesViewModel.Department;
            _employeeQualification.FullGPA = HiringNewEmployeesViewModel.FullGPA;
            _employeeQualification.GPA = HiringNewEmployeesViewModel.GPA;
            _employeeQualification.StudyPlace = HiringNewEmployeesViewModel.StudyPlace;
            _employeeQualification.GraduationDate = HiringNewEmployeesViewModel.GraduationDate;
            _employeeQualification.GraduationYear = HiringNewEmployeesViewModel.GraduationYear;
            _employeeQualification.QualificationType = new QualificationsTypesBLL() { QualificationTypeID = HiringNewEmployeesViewModel.QualificationTypeID };
            _employeeQualification.LoginIdentity = UserIdentity;
            ContractorsBasicSalariesBLL _contractorBasicSalaryBLL = new ContractorsBasicSalariesBLL();
            _contractorBasicSalaryBLL.BasicSalary = HiringNewEmployeesViewModel.BasicSalary;
            _contractorBasicSalaryBLL.TransfareAllowance = HiringNewEmployeesViewModel.TransfareAllowance;
            _contractorBasicSalaryBLL.LoginIdentity = UserIdentity;
            List<EmployeesAllowancesBLL> _employeesAllowancesBLL = new List<EmployeesAllowancesBLL>();
            foreach (var item in HiringNewEmployeesViewModel.Allowances)
            {
                if (item.IsSelected)
                {
                    _employeesAllowancesBLL.Add(new EmployeesAllowancesBLL() { EmployeeCareerHistory = _employeesCareersHistory, Allowance = item, AllowanceStartDate = DateTime.Now, IsActive = true, LoginIdentity = UserIdentity });
                }
            }
            _employeeBll.AddHiringNewEmployee(_employeeBll, _employeesCode, _employeesCareersHistory, _employeeQualification, _contractorBasicSalaryBLL, _employeesAllowancesBLL);
            //return View("Employees/Index");

            return Json(new { ID = 0 }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CareerHistory

        public ActionResult GetCareerHistoryByEmployeeCodeID(int id)
        {
            return Json(new { data = new EmployeesCodesBLL().GetCareerHistoryByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delegations
        public ActionResult GetDelegationsByEmployeeCodeID(int id)
        {
            var data = new EmployeesCodesBLL().GetDelegationsByEmployeeCodeID(id)
               .Select(x => new
               {
                   x.DelegationID,
                   x.DelegationType.DelegationTypeName,
                   x.DelegationKind.DelegationKindName,
                   x.DelegationStartDate,
                   x.DelegationEndDate,
                   x.DelegationPeriod,
                   x.DelegationReason,
                   CountryOrCity = x.DelegationType.DelegationTypeID == (int)DelegationsTypesEnum.Internal ? ((InternalDelegationBLL)x).KSACity.KSACityName : ((ExternalDelegationBLL)x).Country.CountryName,
                   IsApproved = x.IsApproved
               });

            return SetJsonResultWithMaxJsonLength(data);
        }

        public ActionResult GetEmployeeDelegationsBalanceForYears(int id)
        {
            return Json(new { data = new EmployeeDelegationBLL().GetEmployeeDelegationsBalanceForYears(id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region OverTimes
        public JsonResult GetOverTimeByEmployeeCodeID(int id)
        {
            return Json(new { data = new EmployeesCodesBLL().GetOverTimesByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Old Jobs
        public JsonResult GetOldJobsByEmployeeCodeID(int id)
        {
            return Json(new { data = new EmployeesOldJobsBLL().GetByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PassengerOrders
        public ActionResult GetPassengerOrderByEmployeeCodeID(int id)
        {
           
            var data = new EmployeesCodesBLL().GetPassengerOrdersByEmployeeCodeID(id).Select(x => new
            {
                PassengerOrderID = x.PassengerOrderID,
                //??????????? needs to review
                //StartDate = (PassengerOrdersTypesEnum)x.PassengerOrderType.PassengerOrderTypeID == HCMBLL.Enums.PassengerOrdersTypesEnum.Delegation ? ((DelegationsDetailsBLL)x.EmployeeCareerHistory).Delegation.DelegationStartDate : ((InternshipScholarshipsDetailsBLL)x.EmployeeCareerHistory).InternshipScholarship.InternshipScholarshipStartDate,
                //EndDate = (PassengerOrdersTypesEnum)x.PassengerOrderType.PassengerOrderTypeID == HCMBLL.Enums.PassengerOrdersTypesEnum.Delegation ? ((DelegationsDetailsBLL)x.EmployeeCareerHistory).Delegation.DelegationEndDate : ((InternshipScholarshipsDetailsBLL)x.EmployeeCareerHistory).InternshipScholarship.InternshipScholarshipEndDate,
                //Reason = (PassengerOrdersTypesEnum)x.PassengerOrderType.PassengerOrderTypeID == HCMBLL.Enums.PassengerOrdersTypesEnum.Delegation ? ((DelegationsDetailsBLL)x.EmployeeCareerHistory).Delegation.DelegationReason : ((InternshipScholarshipsDetailsBLL)x.EmployeeCareerHistory).InternshipScholarship.InternshipScholarshipReason,
                TravellingDate = x.TravellingDate,
                Going = x.Going,
                Returning = x.Returning,
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Assignings
        public ActionResult GetAssigningByEmployeeCodeID(int id)
        {
            // get assigning from assignings module
            var AssigningAsEmployees = new EmployeesCodesBLL().GetAssigningsByEmployeeCodeID(id)
              .Select(x => new
              {
                  x.AssigningID,
                  //x.AssigningType.AssigningTypeName,
                  x.AssigningStartDate,
                  x.AssigningEndDate,
                  x.IsFinished,
                  Organization = x.AssigningType.AssigningTypeID == (int)AssigningsTypesEnum.Internal ? ((InternalAssigningBLL)x).Organization.FullOrganizationName : ((ExternalAssigningBLL)x).ExternalOrganization,
                  JobName = x.AssigningType.AssigningTypeID == (int)AssigningsTypesEnum.Internal ? ((InternalAssigningBLL)x).Job.JobName : string.Empty,
              });

            // get assigning from organization structure module
            var AssigningAsManagers = new OrganizationsManagersBLL().GetOrganizationsOfManager(id)
                 .Select(x => new
                 {
                     AssigningID = x.OrganizationMangerID,
                     //AssigningTypeName = "fsdf",
                     AssigningStartDate = x.FromDate,
                     AssigningEndDate = x.ToDate,
                     IsFinished = x.ToDate.HasValue && x.ToDate.Value < DateTime.Now.Date ? true : false,
                     Organization = x.Organization.GetOrganizationNameTillLastParentExceptPresident(x.Organization.OrganizationID),
                     JobName = Resources.Globalization.ManagerText + " " + x.Organization.OrganizationName
                 });

            var AllData = (from x in AssigningAsEmployees
                           select new
                           {
                               x.AssigningID,
                               //x.AssigningTypeName,
                               x.AssigningStartDate,
                               x.AssigningEndDate,
                               x.IsFinished,
                               x.Organization,
                               x.JobName
                           }).Union(from x in AssigningAsManagers
                                    select new
                                    {
                                        x.AssigningID,
                                        //x.AssigningTypeName,
                                        x.AssigningStartDate,
                                        x.AssigningEndDate,
                                        x.IsFinished,
                                        x.Organization,
                                       x.JobName
                                    });

            return SetJsonResultWithMaxJsonLength(AllData);
        }
        #endregion

        #region Lenders
        public ActionResult GetLenderByEmployeeCodeID(int id)
        {
            return Json(new { data = new EmployeesCodesBLL().GetLendersByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GovernmentFund
        public ActionResult GetGovernmentFundsByEmployeeCodeID(int id)
        {
            return Json(new { data = new EmployeesCodesBLL().GetGovernmentFundsByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Allowances
        public ActionResult GetAllowancesByEmployeeCodeID(int id)
        {
            return Json(new { data = new EmployeesCodesBLL().GetActiveAllownacessByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region InternshipScholarships
        public ActionResult GetInternshipScholarshipsByEmployeeCodeID(int id)
        {
            var data = new EmployeesCodesBLL().GetInternshipScholarshipsByEmployeeCodeID(id)
            .Select(x => new
            {
                x.InternshipScholarshipID,
                x.InternshipScholarshipType.InternshipScholarshipTypeName,
                x.InternshipScholarshipStartDate,
                x.InternshipScholarshipEndDate,
                x.InternshipScholarshipPeriod,
                x.InternshipScholarshipLocation,
                CityOrCountry = (x.InternshipScholarshipType.InternshipScholarshipTypeID == (int)InternshipScholarshipsTypesEnum.Internal ? ((InternalInternshipScholarshipsBLL)x).KSACity.KSACityName : ((ExternalInternshipScholarshipsBLL)x).Country.CountryName),
            });

            return SetJsonResultWithMaxJsonLength(data);

            //return Json(new { data = new EmployeesCodesBLL().GetInternshipScholarshipsByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Scholarships
        public ActionResult GetScholarshipsByEmployeeCodeID(int id)
        {
            var data = new EmployeesCodesBLL().GetScholarshipsByEmployeeCodeID(id)
               .Select(x => new
               {
                   ScholarshipID = x.ScholarshipID,
                   ScholarshipTypeName = x.ScholarshipType.ScholarshipType,
                   ScholarshipLocation = (x.ScholarshipType.ScholarshipTypeID == (int)ScholarshipsTypesEnum.Internal ? ((InternalScholarshipsBLL)x).KSACity.KSACityName : ((ExternalScholarshipsBLL)x).Country.CountryName),
                   ScholarshipStartDate = x.ScholarshipStartDate,
                   ScholarshipEndDate = x.ScholarshipEndDate,
                   ScholarshipPeriod = x.ScholarshipPeriod,
                   IsPassed = x.IsPassed,
                   CreatedDate = x.CreatedDate
               });
            return Json(new { data }, JsonRequestBehavior.AllowGet);

           // return Json(new { data = new EmployeesCodesBLL().GetScholarshipsByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetScholarshipsByEmployeeCodeID/{EmployeeCodeID}/{StartDate}/{EndDate}")]
        public ActionResult GetScholarshipsByEmployeeCodeID(int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            return Json(new
            {
                data = new BaseScholarshipsBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate)
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmployeesExperiences
        public ActionResult GetEmployeeExperiencesByEmployeeCodeID(int id)
        {
            return Json(new { data = new EmployeesCodesBLL().GetExperiencesByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Qualification
        [HttpGet]
        public JsonResult GetEmployeeCurrentQualification(int id)
        {
            return Json(new { data = new EmployeesCodesBLL().GetEmployeeCurrentQualificationByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeQualificationsByEmployeeCodeID(int id)
        {
            return Json(new { data = new EmployeesCodesBLL().GetQualificationsByEmployeeCodeID(id, 0) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Vacations
        public ActionResult GetVacationsByEmployeeCodeID(int id)
        {
            var data = new EmployeesCodesBLL().GetVacationsDetailsByEmployeeCodeID(id).Select(x => new
            {
                VacationID = x.Vacation.VacationID,
                VacationDetailID = x.VacationDetailID,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                VacationPeriod = x.VacationPeriod,
                VacationTypeName = x.Vacation.VacationTypeName,
                VacationActionName = x.VacationAction.VacationActionName,
            }).OrderBy(x => x.FromDate);
            return Json(new { data = data.OrderBy(x => x.FromDate.Date) }, JsonRequestBehavior.AllowGet);

            //return Json(new { data = new EmployeesCodesBLL().GetVacationsWithDetailsByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Update Mobile Number

        [CustomAuthorize]
        public ActionResult EditMobileNo()
        {
            Session["UpdateMobileNoOTP"] = null;
            return View();
        }

        [HttpPost]
        [ActionName("SendOTP")]
        [IgnoreModelProperties("MobileOTP,NewFirstNameAr,NewMiddleNameAr,NewGrandFatherNameAr,NewLastNameAr,NewFirstNameEn,NewMiddleNameEn,NewGrandFatherNameEn,NewLastNameEn")]
        public HttpResponseMessage SendOTP(EmployeesUpdateViewModel EmployeesUpdateVM)
        {
            Session["UpdateMobileNoOTP"] = Utilities.CreateRandomNumbers(5);
            string Msg = string.Format(Resources.Globalization.UpdateMobileNoMessageFormatText, "" + Session["UpdateMobileNoOTP"]);

            if (!new SMSBLL().SendSMS(new SMSLogsBLL() { Message = Msg, MobileNo = EmployeesUpdateVM.NewMobileNo }, false))
                throw new CustomException(Resources.Globalization.ValidationErrorWhileSendingSMSText);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [ActionName("UpdateMobileNo")]
        [IgnoreModelProperties("NewFirstNameAr,NewMiddleNameAr,NewGrandFatherNameAr,NewLastNameAr,NewFirstNameEn,NewMiddleNameEn,NewGrandFatherNameEn,NewLastNameEn")]
        public HttpResponseMessage UpdateMobileNo(EmployeesUpdateViewModel EmployeesUpdateVM)
        {
            string OTP = "" + Session["UpdateMobileNoOTP"];
            if (OTP.Equals(EmployeesUpdateVM.MobileOTP))
            {
                EmployeesBLL _employeeBll = new EmployeesBLL()
                {
                    EmployeeIDNo = EmployeesUpdateVM.EmployeeIDNo,
                    EmployeeMobileNo = EmployeesUpdateVM.NewMobileNo,
                    LoginIdentity = UserIdentity
                };
                _employeeBll.UpdateMobileNo();
                Session["UpdateMobileNoOTP"] = null;
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                throw new CustomException(Resources.Globalization.MobileOTPNotMatchedText);
            }
        }

        #endregion

        #region Update Employee Name AR
        [CustomAuthorize]
        public ActionResult EditEmployeeName()
        {
            return View();
        }


        [HttpPost]
        [IgnoreModelProperties("MobileOTP,MobileNo")]
        public HttpResponseMessage EditEmployeeName(EmployeesUpdateViewModel EmployeesUpdateVM)
        {
            EmployeesBLL _employeeBll = new EmployeesBLL();
            _employeeBll.EmployeeID = new EmployeesCodesBLL().GetByEmployeeCodeID(EmployeesUpdateVM.EmployeeCodeID).Employee.EmployeeID;
            _employeeBll.FirstNameAr = EmployeesUpdateVM.NewFirstNameAr;
            _employeeBll.MiddleNameAr = EmployeesUpdateVM.NewMiddleNameAr;
            _employeeBll.GrandFatherNameAr = EmployeesUpdateVM.NewGrandFatherNameAr;
            _employeeBll.FifthNameAr = EmployeesUpdateVM.NewFifthNameAr;
            _employeeBll.LastNameAr = EmployeesUpdateVM.NewLastNameAr;
            _employeeBll.FirstNameEn = EmployeesUpdateVM.NewFirstNameEn;
            _employeeBll.MiddleNameEn = EmployeesUpdateVM.NewMiddleNameEn;
            _employeeBll.GrandFatherNameEn = EmployeesUpdateVM.NewGrandFatherNameEn;
            _employeeBll.FifthNameEn = EmployeesUpdateVM.NewFifthNameEn;
            _employeeBll.LastNameEn = EmployeesUpdateVM.NewLastNameEn;
            _employeeBll.LoginIdentity = UserIdentity;
            _employeeBll.UpdateEmployeeName();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        #endregion

        #region PromotionRecord
        public ActionResult GetPromotionRecordsByEmployeeCodeID(int id)
        {
            var data = new EmployeesCodesBLL().GetPromotionRecordsByEmployeeCodeID(id).Select(x => new
            {
                Year = x.PromotionRecord.PromotionPeriod.Year.MaturityYear,
                PeriodName = x.PromotionRecord.PromotionPeriod.Period.PeriodName,
                PromotionRecordNo = x.PromotionRecord.PromotionRecordNo,
                PromotionRecordDate = x.PromotionRecord.PromotionRecordDate,
                PromotionRecordStatusName = x.PromotionRecord.PromotionRecordStatus.PromotionRecordStatusName,
                PromotionRecordRankName = x.PromotionRecord.Rank.RankName,
                PromotionRecordJobCategoryName = x.PromotionRecord.JobCategory.JobCategoryName,
                // JobNo = x.CurrentEmployeeCareer.OrganizationJob.JobNo,
                IsPromoted = x.NewEmployeeCareer,
                IsRemoval = x.IsRemovedAfterIncluding,
                PromotionCandidateAddedWayName = x.PromotionCandidateAddedWay.PromotionCandidateAddedWayName,
            });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Stopwork
        public ActionResult GetEmployeeStopWorksByEmployeeCodeID(int id)
        {
            return Json(new { data = new EmployeesCodesBLL().GetStopWorksByEmployeeCodeID(id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmployeeEvaluation
        public ActionResult GetEmployeeEvaluationByEmployeeCodeID(int id)
        {
            return Json(new
            {
                data = ((new EmployeesCodesBLL().GetEmployeeEvaluationByEmployeeCodeID(id))).Select(x => new
                {
                    EmployeeEvaluationID = x.EmployeeEvaluationID,
                    MaturityYearsBalances = x.MaturityYearsBalances,
                    EvaluationPoints = x.EvaluationPoints,
                    EvaluationDegree = x.EvaluationDegree,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmployeesExperiences With Details
        public JsonResult GetEmployeesExperiencesWithDetailByEmployeeCodeID(int id)
        {
            return Json(new
            {
                data = ((new EmployeeExperiencesWithDetailsBLL().GetEmployeesExperiencesWithDetailByEmployeeCodeID(id)))
                .Select(x => new
                {
                    EmployeeExperienceWithDetailID = x.EmployeeExperienceWithDetailID,
                    JobName = x.JobName,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    SectorName = x.SectorName,
                    SectorTypeName = x.SectorsTypes.SectorTypeName,
                    x.ExperienceYears,
                    x.ExperienceMonths,
                    x.ExperienceDays,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult PrintEmployeeExperienceDetailByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.EmployeesExperiencesDetails.ToString(), id));
        }
        #endregion
    }

}