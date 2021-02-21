using ExceptionNameSpace;
using HCM.Classes;
using HCM.Classes.CustomAttributes;
using HCM.Classes.CustomFilters;
using HCM.Classes.Exceptions;
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
    public class EmployeesPlacementController : BaseEServicesController
    {
        // GET: EmployeesPlacement
        [AllowAnonymous]
        public ActionResult Index()
        {
            var LastDayUmAlqura = System.Configuration.ConfigurationManager.AppSettings["LastDayOfEmployeesPlacement"].ToString();
            string LastDayUmAlquraGreg = Globals.Calendar.UmAlquraToGreg(LastDayUmAlqura);
            ViewBag.LastDayOfEmployeesPlacement = LastDayUmAlqura;
            ViewBag.LastDayOfEmployeesPlacementGreg = LastDayUmAlquraGreg;
            return View();
        }

        [CustomAuthorize]
        public ActionResult EmployeesInPlaced()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult OrganizationsPlacement()
        {
            return View();
        }

        [HttpGet]
        [Route("{controller}/GetAllOrganizationsForManager")]
        public JsonResult GetAllOrganizationsForManager()
        {
            List<OrganizationsStructuresBLL> AllOrganizations = new OrganizationsStructuresBLL().GetAllOrganizationsForManagerByManagerCodeNo(this.WindowsUserIdentity);
            if (AllOrganizations != null)
            {
                return Json(new
                {
                    data = AllOrganizations.Select(x => new
                    {
                        OrganizationID = x.OrganizationID,
                        OrganizationName = x.OrganizationName,
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    data = string.Empty
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("{controller}/GetOrganizationsUnderManager/{OrganizationID}")]
        public JsonResult GetOrganizationsUnderManager(int OrganizationID)
        {
            List<OrganizationsStructuresBLL> ChildOrganizations = new OrganizationsStructuresBLL().GetChildByParentOrganizationID(OrganizationID);
            if (ChildOrganizations != null)
            {
                return Json(new
                {
                    data = ChildOrganizations.Select(x => new
                    {
                        // this id for OrgChart
                        //id = x.OrganizationID,
                        //pid = x.ParentOrganization != null ? x.ParentOrganization.OrganizationID : 0,
                        //OrganizationName = x.OrganizationName,
                        //FullOrganizationName = x.FullOrganizationName,
                        //ManagerIDNo = x.OrganizationManager != null ? x.OrganizationManager.Employee.EmployeeIDNo : string.Empty,
                        //ManagerCodeID = x.OrganizationManager != null ? x.OrganizationManager.EmployeeCodeID : (int?)null,
                        //ManagerCodeNo = x.OrganizationManager != null ? x.OrganizationManager.EmployeeCodeNo : null,
                        //FullManagerName = x.OrganizationManager != null ? x.OrganizationManager.Employee.EmployeeNameAr : null,
                        //ManagerName = x.OrganizationManager != null ? x.OrganizationManager.Employee.FirstNameAr + " " + x.OrganizationManager.Employee.LastNameAr : null,
                        //tags = x.OrganizationManager != null ? x.OrganizationManager.EmployeeCodeNo == this.ADLoginIdentity ? "[FirstLevel]" : "SecondLevel" : "SecondLevel",
                        ////ManagerImage = 
                        ////ManagerImage = ConfigurationManager.AppSettings["EmployeesPicsPath"].ToString() + "anonymous.png"
                        ////ManagerImage = RazorHelper
                        //ManagerImage = x.OrganizationManager != null ? "http://localhost:32788/Content/Images/90025159.jpg" : "http://localhost:32788/Content/Images/anonymous.png"


                        // this for dataTable
                        OrganizationID = x.OrganizationID,
                        OrganizationName = x.OrganizationName,
                        FullOrganizationName = x.FullOrganizationName,
                        ManagerIDNo = x.OrganizationManager != null ? x.OrganizationManager.Employee.EmployeeIDNo : string.Empty,
                        ManagerCodeID = x.OrganizationManager != null ? x.OrganizationManager.EmployeeCodeID : (int?)null,
                        ManagerCodeNo = x.OrganizationManager != null ? x.OrganizationManager.EmployeeCodeNo : string.Empty,
                        FullManagerName = x.OrganizationManager != null ? x.OrganizationManager.Employee.EmployeeNameAr : string.Empty,
                        ManagerName = x.OrganizationManager != null ? x.OrganizationManager.Employee.FirstNameAr + " " + x.OrganizationManager.Employee.LastNameAr : string.Empty,
                        ManagerImage = x.OrganizationManager != null ? "http://localhost:32788/Content/Images/90025159.jpg" : "http://localhost:32788/Content/Images/anonymous.png",
                        LastUpdatedDate = x.OrganizationManager != null ? x.LastUpdatedDate : null
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            else
                throw new Exception();
        }

        [HttpPost]
        [Route("{controller}/AssignManagerToOrganization")]
        [IgnoreModelProperties("OrganizationCode,OrganizationName")]
        public HttpResponseMessage AssignManagerToOrganization(OrganizationStructureViewModel OrganizationStructureVM)
        {
            OrganizationsStructuresBLL organizationStructureBLL = new OrganizationsStructuresBLL()
            {
                LoginIdentity = this.WindowsEmployeeCode,
                OrganizationID = OrganizationStructureVM.OrganizationID.Value,
                OrganizationManager = OrganizationStructureVM.ManagerCodeID.HasValue ? new EmployeesCodesBLL() { EmployeeCodeID = OrganizationStructureVM.ManagerCodeID.Value } : null,
            };

            Result result = organizationStructureBLL.ChangeOrganizationManager(organizationStructureBLL);

            //if (result.EnumMember == OrganizationStructureValidationEnum.RejectedBecauseOfPlacementPeriodFinished.ToString())
            //    throw new CustomException(Resources.Globalization.EmployeesPlacementPeriodFinishedText);
            //else

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{controller}/GetEmployeesUnderManagerByOrganization/{OrganizationID}")]
        public JsonResult GetEmployeesUnderManagerByOrganization(int OrganizationID)
        {
            BaseAssigningsBLL AssigningBLL = AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
            List<InternalAssigningBLL> Employees = new InternalAssigningBLL()
            {
                Organization = new OrganizationsStructuresBLL()
                {
                    OrganizationID = OrganizationID
                },
                Manager = new EmployeesCodesBLL() { EmployeeCodeID = this.WindowsEmployeeCode.EmployeeCodeID }
            }.GetEmployeesUnderManagerByOrganization();

            if (Employees != null)
            {
                return Json(new
                {
                    data = Employees.Select(x => new
                    {
                        AssigningID = x.AssigningID,
                        EmployeeCodeID = x.EmployeeCareerHistory?.EmployeeCode?.EmployeeCodeID,
                        EmployeeCodeNo = x.EmployeeCareerHistory?.EmployeeCode?.EmployeeCodeNo,
                        EmployeeIDNo = x.EmployeeCareerHistory?.EmployeeCode?.Employee?.EmployeeIDNo,
                        FullEmployeeName = x.EmployeeCareerHistory?.EmployeeCode?.Employee?.EmployeeNameAr,
                        OrganizationName = x.EmployeeCareerHistory != null ? x.Organization?.OrganizationName : string.Empty,
                        JobName = x.EmployeeCareerHistory != null ? x.Job?.JobName : string.Empty,
                        AssigningReason = x.AssigningReason?.AssigningReasonName,
                        CreatedDate = x.CreatedDate
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            else
                throw new Exception();
        }

        [HttpPost]
        [Route("{controller}/AssignEmployeeUnderOrganizationManager/{EmployeeCodeID}/{OrganizationID}/{JobID}")]
        public HttpResponseMessage AssignEmployeeUnderOrganizationManager(int EmployeeCodeID, int OrganizationID, int JobID)
        {
            InternalAssigningBLL assigning = (InternalAssigningBLL)AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
            assigning.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(EmployeeCodeID);
            assigning.Manager = new EmployeesCodesBLL() { EmployeeCodeID = this.WindowsEmployeeCode.EmployeeCodeID };
            assigning.Organization = new OrganizationsStructuresBLL() { OrganizationID = OrganizationID };
            assigning.Job = new JobsBLL() { JobID = JobID };
            assigning.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = this.WindowsEmployeeCode.EmployeeCodeID };

            if (assigning.EmployeeCareerHistory.EmployeeCareerHistoryID == 0)
                throw new CustomException(Resources.Globalization.NoCareerHistoryExistsToEmployeeText);

            Result result = assigning.AssignEmployeeUnderManager();

            if (result.EnumMember == OrganizationStructureValidationEnum.RejectedBecauseOfPlacementPeriodFinished.ToString())
                throw new CustomException(Resources.Globalization.EmployeesPlacementPeriodFinishedText);

            else if (result.EnumMember == AssigningsValidationEnum.RejectedBecauseOfActivePreviousAssigning.ToString())
            {
                InternalAssigningBLL assgining = (InternalAssigningBLL)result.Entity;
                throw new CustomException(Resources.Globalization.ValidationEmployeeAlreadyAssignedToOrgText
                                                            + " NewLine" + Resources.Globalization.ManagerNameArText + " : " + (assgining.Manager != null ? assgining.Manager.Employee.EmployeeNameAr : string.Empty)
                                                            + " NewLine" + Resources.Globalization.OrganizationNameText + " : " + assgining.Organization.GetOrganizationNameTillLastParentExceptPresident(assgining.Organization.OrganizationID)
                                                            );
            }

            else
                return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("{controller}/RemoveEmployeeFromOrganizationManager/{AssigningID}")]
        public HttpResponseMessage RemoveEmployeeFromOrganizationManager(int AssigningID)
        {
            InternalAssigningBLL assigningBLL = (InternalAssigningBLL)AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
            assigningBLL.AssigningID = AssigningID;
            assigningBLL.LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = this.WindowsEmployeeCode.EmployeeCodeID };
            Result result = assigningBLL.RemoveEmployeeFromManager();

            if (result.EnumMember == OrganizationStructureValidationEnum.RejectedBecauseOfPlacementPeriodFinished.ToString())
                throw new CustomException(Resources.Globalization.EmployeesPlacementPeriodFinishedText);
            else
                return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        public JsonResult GetEmployeesInPlaced()
        {
            return Json(new
            {
                data = new PlacementBLL().GetEmployeesInPlaced().Select(x => new
                {
                    EmployeeCareerHistoryID = x.EmployeeCareerHistory.EmployeeCareerHistoryID,
                    EmployeeCodeNo = x.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo,
                    EmployeeNameAr = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeNameAr,
                    EmployeeMobileNo = x.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeMobileNo,
                    JobName = x.EmployeeCareerHistory.OrganizationJob.Job.JobName,
                    Rank = x.EmployeeCareerHistory.OrganizationJob.Rank.RankName,
                    FullOrganizationName = x.EmployeeCareerHistory.OrganizationJob.OrganizationStructure.FullOrganizationName,
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetOrganizationsPlacement/{OrganizationID}")]
        public JsonResult GetOrganizationsPlacement(int OrganizationID)
        {
            return Json(new
            {
                data = new PlacementBLL().GetOrganizationsPlacement(OrganizationID).Select(x => new
                {
                    OrganizationName = x.Organization.FullOrganizationName,
                    ManagerName = x.Organization.OrganizationManager != null ? (x.Organization.OrganizationManager.EmployeeCodeNo + " - " + x.Organization.OrganizationManager.Employee.EmployeeNameAr) : string.Empty,
                    ManagerMobileNo = x.Organization.OrganizationManager != null ? x.Organization.OrganizationManager.Employee.EmployeeMobileNo : string.Empty,
                    EmployeesPlacedCount = x.EmployeesPlacedCount
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpResponseMessage EndingEmployeeAssigning(AssigningsViewModel AssigningVM)
        {
            BaseAssigningsBLL assigning = (InternalAssigningBLL)AssigingsFactory.CreateAssigning(AssigningsTypesEnum.Internal);
            assigning.AssigningID = AssigningVM.AssigningID;
            assigning.LoginIdentity = WindowsEmployeeCode;

            //assigning.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCode = new EmployeesCodesBLL() { emplo } }
            //assigning.AssigningEndDate = DateTime.Now;
            //assigning.AssigningEndReason = new AssigningsReasonsBLL() { AssigningReasonID = AssigningVM.EndAssigningReasonID };
            //assigning.Notes = AssigningVM.Notes;

            Result result = assigning.BreakLastAssigning(AssigningVM.EmployeeCodeID, DateTime.Now, (AssigningsReasonsEnum)AssigningVM.EndAssigningReasonID, assigning.Notes);

            if (result.EnumMember == OrganizationStructureValidationEnum.RejectedBecauseOfPlacementPeriodFinished.ToString())
                throw new CustomException(Resources.Globalization.EmployeesPlacementPeriodFinishedText);
            else
                return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}