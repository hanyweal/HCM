using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Classes.Extensions;
using HCM.Models;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using System.Data;
using HCM.Classes.Helpers;
using NPOI.HSSF.UserModel;
using System.Linq;
using System.Collections.Generic;

namespace HCM.Controllers
{
    public class AssigningsController : BaseController
    {
        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAssignings()
        {
            BaseAssigningsBLL AssigningBLL = AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
            AssigningBLL.Search = Search;
            AssigningBLL.Order = Order;
            AssigningBLL.OrderDir = OrderDir;
            AssigningBLL.StartRec = StartRec;
            AssigningBLL.PageSize = PageSize;

            var data = AssigningBLL.GetAssignings(out TotalRecordsOut, out RecFilterOut).Select(x => new
            {
                x.AssigningID,
                EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                AssigningStartDate = x.AssigningStartDate,
                AssigningEndDate = x.AssigningEndDate,
                IsFinished = x.IsFinished,
            });

            return Json(new { draw = Convert.ToInt32(Draw), recordsTotal = TotalRecordsOut, recordsFiltered = RecFilterOut, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportExcelAssignings()
        {
            BaseAssigningsBLL AssigningBLL = AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);

            var data = Json(new
            {
                data = AssigningBLL.GetAssignings().Select(x => new
                {
                    x.AssigningID,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    AssigningStartDate = Globals.Calendar.GetUmAlQuraDate(x.AssigningStartDate),
                    AssigningEndDate = x.AssigningEndDate.HasValue ? Globals.Calendar.GetUmAlQuraDate(x.AssigningEndDate.Value) : "",
                    OrganizationName = x.AssigningType.AssigningTypeID == (int)AssigningsTypesEnum.Internal ? ((InternalAssigningBLL)x).Organization.FullOrganizationName : ((ExternalAssigningBLL)x).ExternalOrganization,
                    JobName = x.AssigningType.AssigningTypeID == (int)AssigningsTypesEnum.Internal ? ((InternalAssigningBLL)x).Job.JobName : string.Empty,
                    IsFinished = x.IsFinished,
                })
            }, JsonRequestBehavior.AllowGet);

            Dictionary<string, string> ColumnNames = new Dictionary<string, string>
            {
                { "AssigningID", Resources.Globalization.SequenceNoText },
                { "EmployeeCodeNo", Resources.Globalization.EmployeeCodeNoText },
                { "EmployeeNameAr", Resources.Globalization.EmployeeNameArText },
                { "AssigningStartDate", Resources.Globalization.AssigningStartDateText },
                { "AssigningEndDate", Resources.Globalization.AssigningEndDateText },
                { "IsFinished", Resources.Globalization.AssigningIsFinishedText },
                { "OrganizationName", Resources.Globalization.OrganizationNameText },
                { "JobName", Resources.Globalization.JobNameText }
            };

            string fileName = ExcelHelper.ExportToExcel(Json(data), ColumnNames);
            return Json(new { DownLoadFile = fileName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAssigningsWillExpire()
        {
            BaseAssigningsBLL AssigningBLL = AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
            return Json(new
            {
                data = AssigningBLL.GetAssigningsWillExpire(DateTime.Now, 1).Select(x => new
                {
                    x.AssigningID,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    AssigningStartDate = x.AssigningStartDate,
                    AssigningEndDate = x.AssigningEndDate,
                    IsFinished = x.IsFinished,
                })
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAssigningsAlreadyExpired()
        {
            BaseAssigningsBLL AssigningBLL = AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
            return Json(new
            {
                data = AssigningBLL.GetAssigningsAlreadyExpiredNotFinished().Select(x => new
                {
                    x.AssigningID,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    AssigningStartDate = x.AssigningStartDate,
                    AssigningEndDate = x.AssigningEndDate,
                    IsFinished = x.IsFinished,
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult End(int id)
        {
            return View(this.GetByAssigningID(id));
        }

        [HttpPost]
        [IgnoreModelProperties("EmployeeCodeNo,AssginingStartDate,AssginingEndDate,ExternalCity,ExternalOrganization,OrganizationName,JobName")]
        public ActionResult End(AssigningsViewModel AssigningVM)
        {
            BaseAssigningsBLL assigning = new BaseAssigningsBLL();
            assigning.AssigningID = AssigningVM.AssigningID;
            assigning.LoginIdentity = UserIdentity;

            Result result = assigning.BreakLastAssigning(AssigningVM.EmployeeCodeID, DateTime.Now, (AssigningsReasonsEnum)AssigningVM.EndAssigningReasonID, assigning.Notes);

            if (result.EnumMember == OrganizationStructureValidationEnum.RejectedBecauseOfPlacementPeriodFinished.ToString())
                throw new CustomException(Resources.Globalization.EmployeesPlacementPeriodFinishedText);

            return View(AssigningVM);
        }

        [CustomAuthorize]
        public ActionResult Delete(int id)
        {
            return View(this.GetByAssigningID(id));
        }

        [CustomAuthorize]
        public ActionResult Details(int id)
        {
            return View(this.GetByAssigningID(id));
        }

        [HttpDelete]
        [IgnoreModelProperties("EmployeeCodeNo,AssginingStartDate,AssginingEndDate,ExternalCity,ExternalOrganization,OrganizationName,JobName")]
        public ActionResult Delete(AssigningsViewModel AssigningVM)
        {
            BaseAssigningsBLL assigningsBLL = new BaseAssigningsBLL();
            assigningsBLL.LoginIdentity = UserIdentity;
            Result result = assigningsBLL.Remove(AssigningVM.AssigningID);

            if ((System.Type)result.EnumType == typeof(AssigningsValidationEnum))
            {
                if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfAlreadyFinished.ToString())
                    throw new CustomException(Resources.Globalization.ValidationBecauseOfAssigningAlreadyFinishedText);
            }

            return View(AssigningVM);
        }

        [HttpPost]
        public ActionResult Create(AssigningsViewModel AssigningVM)
        {
            if (AssigningVM.AssigningType.AssigningTypeID == (Int32)AssigningsTypesEnum.Internal)
            {
                InternalAssigningBLL assigning = (InternalAssigningBLL)AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
                assigning.AssigningType = new AssigningsTypesBLL() { AssigningTypeID = (int)AssigningsTypesEnum.Internal };
                assigning.AssigningStartDate = AssigningVM.AssginingStartDate.Value.Date;
                assigning.AssigningEndDate = AssigningVM.AssginingEndDate.HasValue ? AssigningVM.AssginingEndDate.Value.Date : (DateTime?)null;
                assigning.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(AssigningVM.EmployeeCareerHistoryID);
                assigning.Manager = AssigningVM.ManagerCodeID.HasValue ? new EmployeesCodesBLL() { EmployeeCodeID = AssigningVM.ManagerCodeID.Value } : null;
                assigning.Job = new JobsBLL() { JobID = AssigningVM.JobID.HasValue ? AssigningVM.JobID.Value : 0 };
                if (AssigningVM.OrganizationID.HasValue)
                    assigning.Organization = new OrganizationsStructuresBLL() { OrganizationID = AssigningVM.OrganizationID.Value };
                assigning.AssigningReason = new AssigningsReasonsBLL() { AssigningReasonID = AssigningVM.AssigningReason.AssigningReasonID };
                assigning.AssigningReasonOther = AssigningVM.AssigningReasonOther;
                assigning.LoginIdentity = this.UserIdentity;

                Result result = assigning.Add();
                if ((System.Type)result.EnumType == typeof(NoConflictWithOtherProcessValidationEnum))
                {
                    Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                }
                if ((System.Type)result.EnumType == typeof(AssigningsValidationEnum))
                {
                    if (result.EnumMember == AssigningsValidationEnum.Done.ToString())
                    {
                        BaseAssigningsBLL assgining = (BaseAssigningsBLL)result.Entity;
                        Session["AssigningID"] = ((InternalAssigningBLL)result.Entity).AssigningID;
                    }
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfActivePreviousAssigning.ToString())
                    {
                        BaseAssigningsBLL assgining = (BaseAssigningsBLL)result.Entity;
                        throw new CustomException(Resources.Globalization.MustEndPreviousAssigningText + "NewLine" + "تاريخ اخر تكليف : " + assgining.AssigningStartDate.ToShortDateString());
                    }
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfConflictWithLenders.ToString())
                    {
                        LendersBLL Lender = ((LendersBLL)result.Entity);
                        throw new CustomException(string.Format(Resources.Globalization.ValidationConflictWithLenderText,
                            Lender.LenderStartDate.ToShortDateString(), Lender.LenderEndDate.ToShortDateString()));
                    }
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfEndDateIsLessThanCreationDate.ToString())
                    {
                        throw new CustomException(Resources.Globalization.ValidationBecauseOfEndDateIsLessThanCreationDateText);
                    }
                }
            }
            else if (AssigningVM.AssigningType.AssigningTypeID == (Int32)AssigningsTypesEnum.External)
            {
                ExternalAssigningBLL assigning = (ExternalAssigningBLL)AssigingsFactory.CreateAssigning(AssigningsTypesEnum.External);
                assigning.AssigningType = new AssigningsTypesBLL() { AssigningTypeID = (int)AssigningsTypesEnum.External };
                assigning.AssigningStartDate = AssigningVM.AssginingStartDate.Value.Date;
                assigning.AssigningEndDate = AssigningVM.AssginingEndDate.HasValue ? AssigningVM.AssginingEndDate.Value.Date : (DateTime?)null;
                assigning.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(AssigningVM.EmployeeCareerHistoryID);
                assigning.ExternalKSACity = new KSACitiesBLL() { KSACityID = AssigningVM.ExternalKSACity.KSACityID };
                assigning.ExternalOrganization = AssigningVM.ExternalOrganization;
                assigning.AssigningReason = new AssigningsReasonsBLL() { AssigningReasonID = AssigningVM.AssigningReason.AssigningReasonID };
                assigning.AssigningReasonOther = AssigningVM.AssigningReasonOther;
                assigning.LoginIdentity = this.UserIdentity;

                Result result = assigning.Add();
                if ((System.Type)result.EnumType == typeof(NoConflictWithOtherProcessValidationEnum))
                {
                    Classes.Helpers.CommonHelper.ConflictValidationMessage(result);
                }
                if ((System.Type)result.EnumType == typeof(AssigningsValidationEnum))
                {
                    if (result.EnumMember == AssigningsValidationEnum.Done.ToString())
                    {
                        BaseAssigningsBLL assgining = (BaseAssigningsBLL)result.Entity;
                        Session["AssigningID"] = ((ExternalAssigningBLL)result.Entity).AssigningID;
                    }
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfActivePreviousAssigning.ToString())
                    {
                        BaseAssigningsBLL assgining = (BaseAssigningsBLL)result.Entity;
                        throw new CustomException(Resources.Globalization.MustEndPreviousAssigningText + "NewLine" + "تاريخ اخر تكليف : " + assgining.AssigningStartDate.ToShortDateString());
                    }
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfConflictWithLenders.ToString())
                    {
                        LendersBLL Lender = ((LendersBLL)result.Entity);
                        throw new CustomException(string.Format(Resources.Globalization.ValidationConflictWithLenderText,
                            Lender.LenderStartDate.ToShortDateString(), Lender.LenderEndDate.ToShortDateString()));
                    }
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfEndDateIsLessThanCreationDate.ToString())
                    {
                        throw new CustomException(Resources.Globalization.ValidationBecauseOfEndDateIsLessThanCreationDateText);
                    }
                }
            }

            return View(AssigningVM);
        }

        [CustomAuthorize]
        public ActionResult Edit(int id)
        {
            return View(this.GetByAssigningID(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditAssigning(AssigningsViewModel AssigningVM)
        {
            if (AssigningVM.AssigningType.AssigningTypeID == (Int32)AssigningsTypesEnum.Internal)
            {
                InternalAssigningBLL assigning = (InternalAssigningBLL)AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
                assigning.AssigningID = AssigningVM.AssigningID;
                assigning.AssigningType = new AssigningsTypesBLL() { AssigningTypeID = (Int32)AssigningsTypesEnum.Internal };
                assigning.AssigningStartDate = AssigningVM.AssginingStartDate.Value.Date;
                //assigning.AssigningEndDate = AssigningVM.AssginingEndDate.HasValue ? AssigningVM.AssginingEndDate.Value.Date : (DateTime?)null;
                //assigning.IsFinished = AssigningVM.IsFinished;
                assigning.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = AssigningVM.EmployeeCareerHistoryID };
                assigning.Job = new JobsBLL() { JobID = AssigningVM.JobID.HasValue ? AssigningVM.JobID.Value : 0 };
                assigning.Organization = new OrganizationsStructuresBLL() { OrganizationID = AssigningVM.OrganizationID.Value };
                assigning.Manager = AssigningVM.ManagerCodeID.HasValue ? new EmployeesCodesBLL() { EmployeeCodeID = AssigningVM.ManagerCodeID.Value } : null;

                assigning.AssigningReason = new AssigningsReasonsBLL() { AssigningReasonID = AssigningVM.AssigningReason.AssigningReasonID };
                assigning.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) };
                assigning.AssigningReasonOther = AssigningVM.AssigningReasonOther;

                Result result = assigning.Update();
                if ((System.Type)result.EnumType == typeof(AssigningsValidationEnum))
                {
                    if (result.EnumMember == AssigningsValidationEnum.Done.ToString())
                    {
                        BaseAssigningsBLL assgining = (BaseAssigningsBLL)result.Entity;
                        Session["AssigningID"] = ((InternalAssigningBLL)result.Entity).AssigningID;
                    }

                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfAlreadyFinished.ToString())
                    {
                        throw new CustomException(Resources.Globalization.ValidationBecauseOfAssigningAlreadyFinishedText);
                    }
                    //else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfActivePreviousAssigning.ToString())
                    //{
                    //    BaseAssigningsBLL assgining = (BaseAssigningsBLL)result.Entity;
                    //    throw new CustomException(Resources.Globalization.MustEndPreviousAssigningText + "NewLine" + "تاريخ اخر تكليف : " + assgining.AssigningStartDate.ToShortDateString());
                    //}
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfConflictWithLenders.ToString())
                    {
                        LendersBLL Lender = ((LendersBLL)result.Entity);
                        throw new CustomException(string.Format(Resources.Globalization.ValidationConflictWithLenderText,
                            Lender.LenderStartDate.ToShortDateString(), Lender.LenderEndDate.ToShortDateString()));
                    }
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfEndDateIsLessThanCreationDate.ToString())
                    {
                        throw new CustomException(Resources.Globalization.ValidationBecauseOfEndDateIsLessThanCreationDateText);
                    }
                }
            }
            else if (AssigningVM.AssigningType.AssigningTypeID == (Int32)AssigningsTypesEnum.External)
            {
                ExternalAssigningBLL assigning = (ExternalAssigningBLL)AssigingsFactory.CreateAssigning(AssigningsTypesEnum.External);
                assigning.AssigningID = AssigningVM.AssigningID;
                assigning.AssigningType = new AssigningsTypesBLL() { AssigningTypeID = (Int32)AssigningsTypesEnum.External };
                assigning.AssigningStartDate = AssigningVM.AssginingStartDate.Value.Date;
                //assigning.AssigningEndDate = AssigningVM.AssginingEndDate.HasValue ? AssigningVM.AssginingEndDate.Value.Date : (DateTime?)null;
                //assigning.IsFinished = AssigningVM.IsFinished;
                //assigning.Employee = new EmployeesCodesBLL() { EmployeeCodeID = AssigningVM.EmployeeCodeID };
                assigning.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = AssigningVM.EmployeeCareerHistoryID };
                assigning.ExternalKSACity = new KSACitiesBLL() { KSACityID = AssigningVM.ExternalKSACity.KSACityID };
                assigning.ExternalOrganization = AssigningVM.ExternalOrganization;
                assigning.AssigningReason = new AssigningsReasonsBLL() { AssigningReasonID = AssigningVM.AssigningReason.AssigningReasonID };
                assigning.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = int.Parse(Session["EmployeeCodeID"].ToString()) };
                assigning.AssigningReasonOther = AssigningVM.AssigningReasonOther;

                Result result = assigning.Update();
                if ((System.Type)result.EnumType == typeof(AssigningsValidationEnum))
                {
                    if (result.EnumMember == AssigningsValidationEnum.Done.ToString())
                    {
                        BaseAssigningsBLL assgining = (BaseAssigningsBLL)result.Entity;
                        Session["AssigningID"] = ((ExternalAssigningBLL)result.Entity).AssigningID;
                    }
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfActivePreviousAssigning.ToString())
                    {
                        BaseAssigningsBLL assgining = (BaseAssigningsBLL)result.Entity;
                        throw new CustomException(Resources.Globalization.MustEndPreviousAssigningText + "NewLine" + Resources.Globalization.LastAssigningStartDateText + assgining.AssigningStartDate.ToShortDateString());
                    }
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfConflictWithLenders.ToString())
                    {
                        LendersBLL Lender = ((LendersBLL)result.Entity);
                        throw new CustomException(string.Format(Resources.Globalization.ValidationConflictWithLenderText,
                            Lender.LenderStartDate.ToShortDateString(), Lender.LenderEndDate.ToShortDateString()));
                    }
                    else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfEndDateIsLessThanCreationDate.ToString())
                    {
                        throw new CustomException(Resources.Globalization.ValidationBecauseOfEndDateIsLessThanCreationDateText);
                    }
                }
            }
            return View(this.GetByAssigningID(AssigningVM.AssigningID));
        }

        [HttpGet]
        public JsonResult GetAssigningID()
        {
            BaseAssigningsBLL AssigningBLL = AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
            AssigningBLL.AssigningID = int.Parse(Session["AssigningID"].ToString());
            return Json(new { data = AssigningBLL }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public ActionResult PrintAssigning(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/ReportViewerASPX.aspx?type={0}&ID={1}", BusinessSubCategoriesEnum.Assignings.ToString(), id));
        }

        [CustomAuthorize]
        public ActionResult PrintAllAssigningsByEmployeeCodeID(int id)
        {
            return Redirect(string.Format("~/WebForms/Reports/BusinessSubCategoryByEmployee.aspx?Type={0}&ID={1}", BusinessSubCategoriesEnum.Assignings.ToString(), id));
        }

        [AllowAnonymous]
        [HttpGet, Route("{Controller}/GetActiveAssigningByEmployeeCareerHistoryID/{EmployeeCareerHistoryID}")]
        public JsonResult GetActiveAssigningByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            return Json(new { data = GetOrganizationName(EmployeeCareerHistoryID) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Route("{Controller}/GetCurrentManagerOfEmployee/{EmployeeCareerHistoryID}")]
        public ActionResult GetCurrentManagerOfEmployee(int EmployeeCareerHistoryID)
        {
            return Json(new { data = new InternalAssigningBLL().GetCurrentManagerOfEmployee(EmployeeCareerHistoryID).OrganizationManager }, JsonRequestBehavior.AllowGet);
        }

        public string GetOrganizationName(int EmployeeCareerHistoryID)
        {
            string OrgName = "";
            BaseAssigningsBLL AssigningBLL = AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
            AssigningBLL = AssigningBLL.GetEmployeeActiveAssigning(EmployeeCareerHistoryID);

            if (AssigningBLL != null && AssigningBLL.AssigningID > 0)
            {
                if (AssigningBLL.AssigningType.AssigningTypeID == (int)AssigningsTypesEnum.Internal)
                    OrgName = ((InternalAssigningBLL)AssigningBLL).Organization.FullOrganizationName;
                else
                    OrgName = ((ExternalAssigningBLL)AssigningBLL).ExternalOrganization;
            }

            return OrgName;
        }

        [NonAction]
        private AssigningsViewModel GetByAssigningID(int id)
        {
            BaseAssigningsBLL AssigningBLL = BaseAssigningsBLL.GetByAssigningID(id);
            AssigningsViewModel AssigningVM = new AssigningsViewModel();
            if (AssigningBLL != null)
            {
                AssigningVM.ExternalKSACity = new KSACitiesBLL() { KSACityID = 0 }; // set default to avoid Object reference error.

                if (AssigningBLL.AssigningType.AssigningTypeID == (Int32)AssigningsTypesEnum.Internal)
                {
                    if (((InternalAssigningBLL)AssigningBLL).Job != null)
                    {
                        AssigningVM.Job = new JobsViewModel()
                        {
                            JobID = ((InternalAssigningBLL)AssigningBLL).Job.JobID,
                            JobName = ((InternalAssigningBLL)AssigningBLL).Job.JobName
                        };
                    }
                    AssigningVM.OrganizationStructure = new OrganizationStructureViewModel()
                    {
                        OrganizationID = ((InternalAssigningBLL)AssigningBLL).Organization.OrganizationID,
                        OrganizationName = ((InternalAssigningBLL)AssigningBLL).Organization.OrganizationName,
                        FullOrganizationName = ((InternalAssigningBLL)AssigningBLL).Organization.FullOrganizationName
                    };
                    if (((InternalAssigningBLL)AssigningBLL).Manager != null)
                    {
                        AssigningVM.Manager = new ParentOrganizationManagerViewModel()
                        {
                            ManagerCodeID = ((InternalAssigningBLL)AssigningBLL).Manager.EmployeeCodeID,
                            ManagerCodeNo = ((InternalAssigningBLL)AssigningBLL).Manager.EmployeeCodeNo,
                            ManagerNameAr = ((InternalAssigningBLL)AssigningBLL).Manager.Employee.EmployeeNameAr,
                        };
                    }
                }
                else if (AssigningBLL.AssigningType.AssigningTypeID == (Int32)AssigningsTypesEnum.External)
                {
                    AssigningVM.ExternalOrganization = ((ExternalAssigningBLL)AssigningBLL).ExternalOrganization;
                    AssigningVM.ExternalKSARegion = ((ExternalAssigningBLL)AssigningBLL).ExternalKSACity.KSARegion;
                    AssigningVM.ExternalKSACity = ((ExternalAssigningBLL)AssigningBLL).ExternalKSACity;
                    AssigningVM.Job = new JobsViewModel();
                    AssigningVM.OrganizationStructure = new OrganizationStructureViewModel();
                }
                AssigningVM.AssigningType = AssigningBLL.AssigningType;
                AssigningVM.AssigningID = AssigningBLL.AssigningID;
                AssigningVM.AssginingStartDate = AssigningBLL.AssigningStartDate;
                AssigningVM.AssginingEndDate = AssigningBLL.AssigningEndDate;
                AssigningVM.IsFinished = AssigningBLL.IsFinished;

                if (AssigningBLL.AssigningReason != null && AssigningBLL.AssigningReason.AssigningReasonID > 0)
                {
                    AssigningVM.AssigningReason = AssigningBLL.AssigningReason;
                    AssigningVM.AssigningReasonOther = AssigningBLL.AssigningReasonOther;
                }
                AssigningVM.EmployeeCodeID = AssigningBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID;
                AssigningVM.Employee = new EmployeesViewModel() { EmployeeCodeID = AssigningBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID };
                AssigningVM.Employee = AssigningVM.Employee.GetEmployee();
                AssigningVM.CreatedDate = AssigningBLL.CreatedDate;
                AssigningVM.CreatedBy = AssigningVM.GetCreatedByDisplayed(AssigningBLL.CreatedBy);
            }
            return AssigningVM;
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetAssigningReasons()
        {
            return Json(new { data = new AssigningsReasonsBLL().GetAssigningsReasons() }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet, Route("{Controller}/GetLastBrokenEmployeeAssigningByEmployeeCodeID/{EmployeeCodeID}")]
        public JsonResult GetLastBrokenEmployeeAssigningByEmployeeCodeID(int EmployeeCodeID)
        {
            return Json(new { data = new BaseAssigningsBLL().GetLastAssigningEndReasonsByEmployeeCodeID(EmployeeCodeID) }, JsonRequestBehavior.AllowGet);
        }
    }
}