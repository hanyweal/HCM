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
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class ScholarshipsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View(new ScholarshipsViewModel() { ScholarshipType = new ScholarshipsTypesBLL() });
        }

        [HttpGet]
        public override ActionResult ExportExcel()
        {
            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "ScholarshipID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "ScholarshipStartDate", Resources.Globalization.ScholarshipStartDateText },
                { "ScholarshipEndDate", Resources.Globalization.ScholarshipEndDateText },
                { "ScholarshipType", Resources.Globalization.ScholarshipTypeText },
                { "Qualification", Resources.Globalization.QualificationNameText },
                { "ScholarshipReason", Resources.Globalization.ScholarshipReasonText },
                { "ScholarshipJoinDate", Resources.Globalization.ScholarshipJoinDateText },
                { "IsPassed", Resources.Globalization.IsPassedText },
                { "KSACityName", Resources.Globalization.KSACityNameText },
                { "Location", Resources.Globalization.ScholarshipLocationText },
                { "CountryName", Resources.Globalization.CountryNameText },
                { "CreatedDate", Resources.Globalization.CreatedDateText }
            };

            string fileName = ExcelHelper.ExportToExcel(GetScholarships(), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetScholarships()
        {
            var data = new BaseScholarshipsBLL().GetScholarships()
              .Select(x => new
              {
                  ScholarshipID = x.ScholarshipID,
                  ScholarshipStartDate = Globals.Calendar.GetUmAlQuraDate(x.ScholarshipStartDate),
                  ScholarshipEndDate = Globals.Calendar.GetUmAlQuraDate(x.ScholarshipEndDate),
                  ScholarshipType = x.ScholarshipType.ScholarshipType,
                  EmployeeCodeNo = x.Employee.EmployeeCodeNo,
                  EmployeeNameAr = x.Employee.Employee.EmployeeNameAr,
                  Qualification = x.Qualification != null ? x.Qualification.QualificationName : null,
                  ScholarshipReason = x.ScholarshipReason,
                  ScholarshipJoinDate = x.ScholarshipJoinDate,
                  IsPassed = x.IsPassed,
                  KSACityName = x.ScholarshipType.ScholarshipTypeID == (int)ScholarshipsTypesEnum.Internal ? ((InternalScholarshipsBLL)x).KSACity.KSACityName : null,
                  Location = x.ScholarshipType.ScholarshipTypeID == (int)ScholarshipsTypesEnum.Internal ? ((InternalScholarshipsBLL)x).Location : null,
                  CountryName = x.ScholarshipType.ScholarshipTypeID == (int)ScholarshipsTypesEnum.External ? ((ExternalScholarshipsBLL)x).Country.CountryName : null,
                  CreatedDate = Globals.Calendar.GetUmAlQuraDate(x.CreatedDate)
              });
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            Session["ScholarshipID"] = null;
            return View();
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByScholarshipID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("EmployeeCodeNo,ScholarshipStartDate,ScholarshipEndDate,Location")]
        public ActionResult Delete(ScholarshipsViewModel ScholarshipVM)
        {
            BaseScholarshipsBLL baseScholarshipsBLL = new HCMBLL.BaseScholarshipsBLL();
            baseScholarshipsBLL.LoginIdentity = UserIdentity;
            baseScholarshipsBLL.Remove(ScholarshipVM.ScholarshipID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(ScholarshipsViewModel ScholarshipVM)
        {
            BaseScholarshipsBLL EmployeeScholarship = null;

            switch (ScholarshipVM.ScholarshipType.ScholarshipTypeID)
            {
                case (int)ScholarshipsTypesEnum.Internal:
                    {
                        EmployeeScholarship = GenericFactoryPattern<BaseScholarshipsBLL, InternalScholarshipsBLL>.CreateInstance();
                        EmployeeScholarship.ScholarshipTypeEnum = (ScholarshipsTypesEnum)ScholarshipVM.ScholarshipType.ScholarshipTypeID;
                        EmployeeScholarship.ScholarshipStartDate = ScholarshipVM.ScholarshipStartDate.Value;
                        EmployeeScholarship.ScholarshipEndDate = ScholarshipVM.ScholarshipEndDate.Value;
                        EmployeeScholarship.Employee = new EmployeesCodesBLL() { EmployeeCodeID = ScholarshipVM.EmployeeCodeID };
                        ((InternalScholarshipsBLL)EmployeeScholarship).Location = ScholarshipVM.Location;
                        ((InternalScholarshipsBLL)EmployeeScholarship).KSACity = new KSACitiesBLL() { KSACityID = ScholarshipVM.KSACity.KSACityID };
                        EmployeeScholarship.ScholarshipReason = ScholarshipVM.ScholarshipReason;
                        EmployeeScholarship.Qualification = new QualificationsBLL() { QualificationID = ScholarshipVM.Qualification.QualificationID };
                        EmployeeScholarship.LoginIdentity = UserIdentity;
                        Result result = EmployeeScholarship.Add();

                        if (result.EnumMember == ScholarshipsValidationEnum.Done.ToString())
                        {
                            ScholarshipVM.ScholarshipID = ((InternalScholarshipsBLL)result.Entity).ScholarshipID;
                        }

                        Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                        break;
                    }
                case (int)ScholarshipsTypesEnum.External:
                    {
                        EmployeeScholarship = GenericFactoryPattern<BaseScholarshipsBLL, ExternalScholarshipsBLL>.CreateInstance();
                        EmployeeScholarship.ScholarshipTypeEnum = (ScholarshipsTypesEnum)ScholarshipVM.ScholarshipType.ScholarshipTypeID;
                        EmployeeScholarship.ScholarshipStartDate = ScholarshipVM.ScholarshipStartDate.Value;
                        EmployeeScholarship.ScholarshipEndDate = ScholarshipVM.ScholarshipEndDate.Value;
                        EmployeeScholarship.Employee = new EmployeesCodesBLL() { EmployeeCodeID = ScholarshipVM.EmployeeCodeID };
                        EmployeeScholarship.ScholarshipReason = ScholarshipVM.ScholarshipReason;
                        ((ExternalScholarshipsBLL)EmployeeScholarship).Country = new CountriesBLL() { CountryID = ScholarshipVM.Country.CountryID };
                        EmployeeScholarship.Qualification = new QualificationsBLL() { QualificationID = ScholarshipVM.Qualification.QualificationID };
                        EmployeeScholarship.LoginIdentity = UserIdentity;
                        Result result = EmployeeScholarship.Add();

                        if (result.EnumMember == ScholarshipsValidationEnum.Done.ToString())
                        {
                            ScholarshipVM.ScholarshipID = ((ExternalScholarshipsBLL)result.Entity).ScholarshipID;
                        }
                        Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                        break;
                    }
                default:
                    break;
            }

            return Json(new { ScholarshipID = ScholarshipVM.ScholarshipID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]

        public ActionResult Edit(int id)
        {
            ScholarshipsViewModel ScholarshipVM = new ScholarshipsViewModel();
            BaseScholarshipsBLL Scholarship = this.GetByScholarshipID(id);
            if (Scholarship != null && Scholarship.ScholarshipID > 0)
            {
                Scholarship = Scholarship.GetByScholarshipID(id);
                //ScholarshipVM.ScholarshipTypeID = (int)Scholarship.ScholarshipType;
                ScholarshipVM.ScholarshipType = new ScholarshipsTypesBLL() { ScholarshipTypeID = Convert.ToInt32(Scholarship.ScholarshipTypeEnum) };
                if ((int)Scholarship.ScholarshipTypeEnum == (Int16)ScholarshipsTypesEnum.External)
                {
                    ScholarshipVM.Country = ((ExternalScholarshipsBLL)Scholarship).Country;
                }
                if ((int)Scholarship.ScholarshipTypeEnum == (Int16)ScholarshipsTypesEnum.Internal)
                {
                    ScholarshipVM.KSACity = ((InternalScholarshipsBLL)Scholarship).KSACity;
                    ScholarshipVM.Location = ((InternalScholarshipsBLL)Scholarship).Location;
                }
                ScholarshipVM.ScholarshipID = Scholarship.ScholarshipID;
                ScholarshipVM.ScholarshipReason = Scholarship.ScholarshipReason;
                ScholarshipVM.ScholarshipStartDate = Scholarship.ScholarshipStartDate;
                ScholarshipVM.ScholarshipEndDate = Scholarship.ScholarshipEndDate;
                ScholarshipVM.ScholarshipPeriod = Scholarship.ScholarshipPeriod;
            }

            return View(ScholarshipVM);
        }

        [HttpPost]
        public ActionResult Edit(ScholarshipsViewModel ScholarshipVM)
        {
            BaseScholarshipsBLL Scholarship = null;
            if (ScholarshipVM.ScholarshipType.ScholarshipTypeID == (int)ScholarshipsTypesEnum.Internal)
            {
                Scholarship = GenericFactoryPattern<BaseScholarshipsBLL, InternalScholarshipsBLL>.CreateInstance();
                ((InternalScholarshipsBLL)Scholarship).Location = ScholarshipVM.Location;
            }
            else if (ScholarshipVM.ScholarshipType.ScholarshipTypeID == (int)ScholarshipsTypesEnum.External)
                Scholarship = GenericFactoryPattern<BaseScholarshipsBLL, ExternalScholarshipsBLL>.CreateInstance();

            Scholarship.ScholarshipID = ScholarshipVM.ScholarshipID;
            Scholarship.ScholarshipStartDate = ScholarshipVM.ScholarshipStartDate.Value.Date;
            Scholarship.ScholarshipEndDate = ScholarshipVM.ScholarshipEndDate.Value.Date;
            Scholarship.ScholarshipReason = ScholarshipVM.ScholarshipReason;
            Scholarship.LoginIdentity = this.UserIdentity;
            Result result = Scholarship.Edit();

            if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyCanceled.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyCanceledText);
            }
            else if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyPassed.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyPassedText);
            }

            Classes.Helpers.CommonHelper.ConflictValidationMessage(result);

            return Json(new { ScholarshipID = Scholarship.ScholarshipID }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]

        public ActionResult PrintAllScholarshipByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.Scholarships.ToString(), id));
        }

        [CustomAuthorize]
        public ActionResult PrintScholarshipAction(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.Scholarships.ToString(), id));
        }

        [CustomAuthorize]
        public ActionResult PrintScholarship(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", "EmployeeScholarship", id));
        }

        [HttpPost]
        [Route("{controller}/GetScholarshipEndDate/{ScholarshipPeriod}/{ScholarshipStartDate}")]
        public JsonResult GetScholarshipEndDate(int ScholarshipPeriod, string ScholarshipStartDate)
        {
            return Json(GetUmAlquraEndDate(ScholarshipPeriod, ScholarshipStartDate), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("{controller}/GetScholarshipFromEndDate/{ScholarshipStartDate}/{ScholarshipEndDate}")]
        public JsonResult GetScholarshipFromEndDate(string ScholarshipStartDate, string ScholarshipEndDate)
        {
            double daysCount = Globals.Calendar.GetUmAlquraDateDiff(ScholarshipStartDate, ScholarshipEndDate);
            return Json(new { ScholarshipPeriod = daysCount }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetEmployeeScholarshipsDetailsByScholarshipID/{ScholarshipID}")]
        public JsonResult GetEmployeeScholarshipsDetailsByScholarshipID(int ScholarshipID)
        {
            List<ScholarshipsDetailsBLL> ScholarshipsDetailsList = new ScholarshipsDetailsBLL().GetScholarshipDetailsByScholarshipID(ScholarshipID);//     GetVacationsDetailsByVacationID(VacationID);
            return Json(new
            {
                data = ScholarshipsDetailsList.Select(x => new
                {
                    ScholarshipDetailID = x.ScholarshipDetailID,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    Notes = x.Notes,
                    ScholarshipPeriod = x.ScholarshipPeriod,
                    ScholarshipActionName = x.ScholarshipAction.ScholarshipActionName,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate,
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetEmployeeValidScholarships/{EmployeeCodeID}/{ScholarshipTypeID}")]
        public JsonResult GetEmployeeValidScholarships(int EmployeeCodeID, int ScholarshipTypeID)
        {
            return Json(new
            {
                data = ((new BaseScholarshipsBLL().GetValidScholarships(EmployeeCodeID, (ScholarshipsTypesEnum)ScholarshipTypeID))).Select(x => new
                {
                    ScholarshipID = x.ScholarshipID,
                    ScholarshipType = x.ScholarshipTypeEnum.ToString(),
                    ScholarshipStartDate = x.ScholarshipStartDate,
                    ScholarshipEndDate = x.ScholarshipEndDate,
                    ScholarshipPeriod = x.ScholarshipPeriod,
                    IsCanceled = x.IsCanceled,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate

                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetEmployeeFinishedScholarships/{EmployeeCodeID}/{ScholarshipTypeID}")]
        public JsonResult GetEmployeeFinishedScholarships(int EmployeeCodeID, int ScholarshipTypeID)
        {
            return Json(new
            {
                data = ((new BaseScholarshipsBLL().GetFinishedScholarships(EmployeeCodeID, (ScholarshipsTypesEnum)ScholarshipTypeID))).Select(x => new
                {
                    ScholarshipID = x.ScholarshipID,
                    ScholarshipType = x.ScholarshipTypeEnum.ToString(),
                    ScholarshipStartDate = x.ScholarshipStartDate,
                    ScholarshipEndDate = x.ScholarshipEndDate,
                    ScholarshipPeriod = x.ScholarshipPeriod,
                    IsCanceled = x.IsCanceled,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("{controller}/GetEmployeeCanceledScholarships/{EmployeeCodeID}/{ScholarshipTypeID}")]
        public JsonResult GetEmployeeCanceledScholarships(int EmployeeCodeID, int ScholarshipTypeID)
        {
            return Json(new
            {
                data = ((new BaseScholarshipsBLL().GetCanceledScholarships(EmployeeCodeID, (ScholarshipsTypesEnum)ScholarshipTypeID))).Select(x => new
                {
                    ScholarshipID = x.ScholarshipID,
                    ScholarshipType = x.ScholarshipTypeEnum.ToString(),
                    ScholarshipStartDate = x.ScholarshipStartDate,
                    ScholarshipEndDate = x.ScholarshipEndDate,
                    ScholarshipPeriod = x.ScholarshipPeriod,
                    IsCanceled = x.IsCanceled,
                    CreatedBy = x.CreatedBy.Employee.EmployeeNameAr,
                    CreatedDate = x.CreatedDate

                })
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteScholarshipByScholarshipDetailID(int id)
        {
            //BaseVacationsBLL Vacation = GenericFactoryPattern<BaseVacationsBLL, ExceptionalVacationsBLL>.CreateInstance();
            ScholarshipsDetailsBLL Scholarship = new ScholarshipsDetailsBLL();
            Scholarship.LoginIdentity = this.UserIdentity;
            Result result = Scholarship.Remove(id);
            //if (result.EnumMember == ScholarshipsValidationEnum.RejectedBeacuseOfNoChanceCancelCancelling.ToString())
            //{
            //    throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyCanceledText);
            //}
            return Json(new
            {
                data = Scholarship
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Extend(int id)
        {
            ScholarshipsExtentionViewModel ScholarshipVM = new ScholarshipsExtentionViewModel();
            BaseScholarshipsBLL Scholarship = this.GetByScholarshipID(id);
            if (Scholarship != null && Scholarship.ScholarshipID > 0)
            {
                Scholarship = Scholarship.GetByScholarshipID(id);
                ScholarshipVM.ScholarshipTypeID = (int)Scholarship.ScholarshipTypeEnum;
                if ((int)Scholarship.ScholarshipTypeEnum == (Int16)ScholarshipsTypesEnum.External)
                {
                    ScholarshipVM.Country = ((ExternalScholarshipsBLL)Scholarship).Country;
                }
                if ((int)Scholarship.ScholarshipTypeEnum == (Int16)ScholarshipsTypesEnum.Internal)
                {
                    ScholarshipVM.KSACity = ((InternalScholarshipsBLL)Scholarship).KSACity;
                }
                ScholarshipVM.ScholarshipID = Scholarship.ScholarshipID;
                ScholarshipVM.FromDate = Scholarship.ScholarshipEndDate.AddDays(1);
                ScholarshipVM.ToDate = null;
            }

            return View(ScholarshipVM);
        }

        [HttpPost]
        public ActionResult Extend(ScholarshipsExtentionViewModel ScholarshipVM)
        {
            BaseScholarshipsBLL Scholarship = null;
            if (ScholarshipVM.ScholarshipTypeID == (int)ScholarshipsTypesEnum.Internal)
                Scholarship = GenericFactoryPattern<BaseScholarshipsBLL, InternalScholarshipsBLL>.CreateInstance();
            else if (ScholarshipVM.ScholarshipTypeID == (int)ScholarshipsTypesEnum.External)
                Scholarship = GenericFactoryPattern<BaseScholarshipsBLL, ExternalScholarshipsBLL>.CreateInstance();

            Scholarship.ScholarshipID = ScholarshipVM.ScholarshipID;
            Scholarship.ScholarshipStartDate = ScholarshipVM.FromDate.Value.Date;
            Scholarship.ScholarshipEndDate = ScholarshipVM.ToDate.Value.Date;
            Scholarship.Notes = ScholarshipVM.Notes;
            Scholarship.LoginIdentity = this.UserIdentity;
            Result result = Scholarship.Extend();

            if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyCanceled.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyCanceledText);
            }
            else if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyPassed.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyPassedText);
            }
            else if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyJoinedBefore.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyJoinedDateText);
            }

            Classes.Helpers.CommonHelper.ConflictValidationMessage(result);

            return Json(new { ScholarshipID = Scholarship.ScholarshipID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Break(int id)
        {
            ScholarshipsBreakViewModel ScholarshipVM = new ScholarshipsBreakViewModel();
            BaseScholarshipsBLL Scholarship = this.GetByScholarshipID(id);

            if (Scholarship != null && Scholarship.ScholarshipID > 0)
            {
                Scholarship = Scholarship.GetByScholarshipID(id);
                ScholarshipVM.ScholarshipTypeID = (int)Scholarship.ScholarshipTypeEnum;
                if ((int)Scholarship.ScholarshipTypeEnum == (Int16)ScholarshipsTypesEnum.External)
                {
                    ScholarshipVM.Country = ((ExternalScholarshipsBLL)Scholarship).Country;
                }
                if ((int)Scholarship.ScholarshipTypeEnum == (Int16)ScholarshipsTypesEnum.Internal)
                {
                    ScholarshipVM.KSACity = ((InternalScholarshipsBLL)Scholarship).KSACity;
                }
                ScholarshipVM.ScholarshipID = Scholarship.ScholarshipID;
                //ScholarshipVM.Qualification = Scholarship.Qualification;
                ScholarshipVM.ScholarshipStartDate = Scholarship.ScholarshipStartDate.Date;
                ScholarshipVM.ScholarshipEndDate = Scholarship.ScholarshipEndDate.Date;
                ScholarshipVM.ScholarshipPeriod = Scholarship.ScholarshipPeriod;
                //ScholarshipVM.ScholarshipReason = Scholarship.ScholarshipReason;
            }
            return View(ScholarshipVM);
        }

        [HttpPost]
        public ActionResult Break(ScholarshipsBreakViewModel ScholarshipVM)
        {
            BaseScholarshipsBLL Scholarship = null;
            if (ScholarshipVM.ScholarshipTypeID == (int)ScholarshipsTypesEnum.Internal)
                Scholarship = GenericFactoryPattern<BaseScholarshipsBLL, InternalScholarshipsBLL>.CreateInstance();
            else if (ScholarshipVM.ScholarshipTypeID == (int)ScholarshipsTypesEnum.External)
                Scholarship = GenericFactoryPattern<BaseScholarshipsBLL, ExternalScholarshipsBLL>.CreateInstance();

            Scholarship.ScholarshipID = ScholarshipVM.ScholarshipID;
            Scholarship.ScholarshipStartDate = ScholarshipVM.ScholarshipStartDate.Value.Date;
            Scholarship.ScholarshipEndDate = ScholarshipVM.ScholarshipEndDate.Value.Date;
            Scholarship.Notes = ScholarshipVM.Notes;
            Scholarship.LoginIdentity = this.UserIdentity;
            Result result = Scholarship.Break();

            if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyCanceled.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyCanceledText);
            }
            else if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyPassed.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyPassedText);
            }
            else if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyJoinedBefore.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyJoinedDateText);
            }
            else
                return Json(new { ScholarshipID = Scholarship.ScholarshipID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cancel(int id)
        {
            ScholarshipsCancelViewModel ScholarshipVM = new ScholarshipsCancelViewModel();
            BaseScholarshipsBLL Scholarship = this.GetByScholarshipID(id);

            if (Scholarship != null && Scholarship.ScholarshipID > 0)
            {
                Scholarship = Scholarship.GetByScholarshipID(id);
                ScholarshipVM.ScholarshipTypeID = (int)Scholarship.ScholarshipTypeEnum;
                if ((int)Scholarship.ScholarshipTypeEnum == (Int16)ScholarshipsTypesEnum.External)
                {
                    ScholarshipVM.Country = ((ExternalScholarshipsBLL)Scholarship).Country;
                }
                if ((int)Scholarship.ScholarshipTypeEnum == (Int16)ScholarshipsTypesEnum.Internal)
                {
                    ScholarshipVM.KSACity = ((InternalScholarshipsBLL)Scholarship).KSACity;
                }
                ScholarshipVM.ScholarshipID = Scholarship.ScholarshipID;
                //ScholarshipVM.Qualification = Scholarship.Qualification;
                ScholarshipVM.ScholarshipStartDate = Scholarship.ScholarshipStartDate.Date;
                ScholarshipVM.ScholarshipEndDate = Scholarship.ScholarshipEndDate.Date;
                ScholarshipVM.ScholarshipPeriod = Scholarship.ScholarshipPeriod;
                //ScholarshipVM.ScholarshipReason = Scholarship.ScholarshipReason;
            }
            return View(ScholarshipVM);
        }

        [HttpPost]
        public ActionResult Cancel(ScholarshipsCancelViewModel ScholarshipVM)
        {
            BaseScholarshipsBLL Scholarship = null;
            if (ScholarshipVM.ScholarshipTypeID == (int)ScholarshipsTypesEnum.Internal)
                Scholarship = GenericFactoryPattern<BaseScholarshipsBLL, InternalScholarshipsBLL>.CreateInstance();
            else if (ScholarshipVM.ScholarshipTypeID == (int)ScholarshipsTypesEnum.External)
                Scholarship = GenericFactoryPattern<BaseScholarshipsBLL, ExternalScholarshipsBLL>.CreateInstance();

            Scholarship.ScholarshipID = ScholarshipVM.ScholarshipID;
            Scholarship.ScholarshipStartDate = ScholarshipVM.ScholarshipStartDate.Value.Date;
            Scholarship.ScholarshipEndDate = ScholarshipVM.ScholarshipEndDate.Value.Date;
            Scholarship.Notes = ScholarshipVM.Notes;
            Scholarship.LoginIdentity = this.UserIdentity;
            Result result = Scholarship.Cancel();

            if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyCanceled.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyCanceledText);
            }
            else if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyPassed.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyPassedText);
            }
            else if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyJoinedBefore.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyJoinedDateText);
            }
            else
                return Json(new { ScholarshipID = Scholarship.ScholarshipID }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Joining(int id)
        {
            ScholarshipsJoiningViewModel ScholarshipVM = new ScholarshipsJoiningViewModel();
            BaseScholarshipsBLL Scholarship = this.GetByScholarshipID(id);

            if (Scholarship != null && Scholarship.ScholarshipID > 0)
            {
                Scholarship = Scholarship.GetByScholarshipID(id);
                ScholarshipVM.ScholarshipTypeID = (int)Scholarship.ScholarshipTypeEnum;
                if ((int)Scholarship.ScholarshipTypeEnum == (Int16)ScholarshipsTypesEnum.External)
                {
                    ScholarshipVM.Country = ((ExternalScholarshipsBLL)Scholarship).Country;
                }
                if ((int)Scholarship.ScholarshipTypeEnum == (Int16)ScholarshipsTypesEnum.Internal)
                {
                    ScholarshipVM.KSACity = ((InternalScholarshipsBLL)Scholarship).KSACity;
                }
                ScholarshipVM.ScholarshipID = Scholarship.ScholarshipID;
                //ScholarshipVM.Qualification = Scholarship.Qualification;
                ScholarshipVM.ScholarshipStartDate = Scholarship.ScholarshipStartDate.Date;
                ScholarshipVM.ScholarshipEndDate = Scholarship.ScholarshipEndDate.Date;
                ScholarshipVM.ScholarshipPeriod = Scholarship.ScholarshipPeriod;
                //ScholarshipVM.ScholarshipReason = Scholarship.ScholarshipReason;
            }
            return View(ScholarshipVM);
        }

        [HttpPost]
        public ActionResult Joining(ScholarshipsJoiningViewModel ScholarshipVM)
        {
            BaseScholarshipsBLL Scholarship = null;
            if (ScholarshipVM.ScholarshipTypeID == (int)ScholarshipsTypesEnum.Internal)
                Scholarship = GenericFactoryPattern<BaseScholarshipsBLL, InternalScholarshipsBLL>.CreateInstance();
            else if (ScholarshipVM.ScholarshipTypeID == (int)ScholarshipsTypesEnum.External)
                Scholarship = GenericFactoryPattern<BaseScholarshipsBLL, ExternalScholarshipsBLL>.CreateInstance();

            Scholarship.ScholarshipID = ScholarshipVM.ScholarshipID;
            Scholarship.ScholarshipStartDate = ScholarshipVM.ScholarshipStartDate.Value.Date;
            Scholarship.ScholarshipEndDate = ScholarshipVM.ScholarshipEndDate.Value.Date;
            Scholarship.ScholarshipJoinDate = ScholarshipVM.ScholarshipJoinDate.Value.Date;
            Scholarship.IsPassed = ScholarshipVM.IsPassed;
            Scholarship.Notes = ScholarshipVM.Notes;

            Scholarship.LoginIdentity = this.UserIdentity;
            Result result = Scholarship.Joining();

            if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyCanceled.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyCanceledText);
            }
            else if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyPassed.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyPassedText);
            }
            else if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfAlreadyJoinedBefore.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipsAlreadyJoinedDateText);
            }
            else if (result.EnumMember == ScholarshipsValidationEnum.RejectedBecauseOfScholarshipJoinDateBeforeScholarshipStartDate.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationScholarshipJoinDateBeforeScholarshipStartDateText);
            }
            else
                return Json(new { ScholarshipID = Scholarship.ScholarshipID }, JsonRequestBehavior.AllowGet);
        }

        private BaseScholarshipsBLL GetByScholarshipID(int id)
        {
            BaseScholarshipsBLL Scholarship = null;
            Scholarship = new BaseScholarshipsBLL().GetByScholarshipID(id);
            if (Scholarship != null && Scholarship.ScholarshipID > 0)
            {
                if (Scholarship.ScholarshipTypeEnum == ScholarshipsTypesEnum.Internal)
                    Scholarship = (InternalScholarshipsBLL)Scholarship;
                else if (Scholarship.ScholarshipTypeEnum == ScholarshipsTypesEnum.External)
                    Scholarship = (ExternalScholarshipsBLL)Scholarship;

            }
            return Scholarship;
        }

    }
}