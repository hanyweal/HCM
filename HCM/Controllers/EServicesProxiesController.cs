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
using System.Web;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class EServicesProxiesController : BaseEServicesController
    {
        // GET: EServicesProxies
        public ActionResult Index()
        {
            EServicesProxiesViewModel VM = new EServicesProxiesViewModel();
            VM.EServiceTypes = this.GetEServiceTypes();
            VM.WindowsUserOrganizationStructures = this.WindowsUserOrganizationStructures;
            VM.Organization = new OrganizationStructureViewModel() { OrganizationID = this.WindowsUserOrganizationID };
            VM.WindowsEmployeeCode = this.WindowsEmployeeCode;
            //VM.StartDate = DateTime.Now;
            return View(VM);
        }

        public ActionResult MyEServicesProxies()
        {
            EServicesProxiesViewModel VM = new EServicesProxiesViewModel();
            VM.EServiceTypes = this.GetEServiceTypes(true);
            VM.WindowsUserOrganizationStructures = this.WindowsUserOrganizationStructures;
            VM.Organization = new OrganizationStructureViewModel() { OrganizationID = this.WindowsUserOrganizationID };
            VM.WindowsEmployeeCode = this.WindowsEmployeeCode;
            return View(VM);
        }

        [HttpGet]
        [Route("EServicesProxies/GetAllEmployeesManagersByOrganizationID/{EServiceTypeID}")]
        public JsonResult GetAllEmployeesManagersByOrganizationID(int EServiceTypeID)
        {
            int OrganizationID = this.WindowsUserOrganizationID;
            string ManagerEmployeeCodeNo = this.WindowsUserIdentity;

            EServicesProxiesBLL EServiceProxy
                = new EServicesProxiesBLL().GetActiveByFromEmployeeCodeID(this.WindowsEmployeeCode.EmployeeCodeID, (EServicesTypesEnum)EServiceTypeID);
            int ToEmployeeProxyAssigned = EServiceProxy == null ? 0 : EServiceProxy.ToEmployee.EmployeeCodeID;
            List<EmployeesCodesBLL> Orgs = new EmployeesCodesBLL().GetAllEmployeesManagersByOrganizationID(OrganizationID, (EServicesTypesEnum)EServiceTypeID);
            List<EServicesProxiesChartViewModel> lst = new List<EServicesProxiesChartViewModel>();
            EServicesProxiesChartViewModel obj;

            foreach (var item in Orgs)
            {
                obj = MapToChartVM(item, ToEmployeeProxyAssigned);
                if (obj != null)
                    lst.Add(obj);
            }
            //lst.AddRange(Orgs.Select(x => MapToChartVM(x, ToEmployeeProxyAssigned)).ToList());

            if (Orgs != null)
            {
                return Json(new
                {
                    total = Orgs.Count,
                    data = lst
                }, JsonRequestBehavior.AllowGet);
            }
            else
                throw new Exception();
        }

        [HttpGet]
        [Route("EServicesProxies/GetProxiesByMe/{EServiceTypeID}")]
        public JsonResult GetProxiesByMe(int EServiceTypeID)
        {
            int OrganizationID = this.WindowsUserOrganizationID;
            string ManagerEmployeeCodeNo = this.WindowsUserIdentity;

            EServicesTypesEnum? EServiceTypeEnum = EServiceTypeID == 0 ? null : (EServicesTypesEnum?)EServiceTypeID;
            List<EServicesProxiesBLL> EServiceProxyList
                = new EServicesProxiesBLL().GetByFromEmployeeCodeID(this.WindowsEmployeeCode.EmployeeCodeID, EServiceTypeEnum);

            return Json(new
            {
                data = EServiceProxyList.Select(
                item => new
                {
                    item.EServiceProxyID,
                    EServiceTypeName = item.EServiceType.EServiceTypeName,
                    ToEmployeeCodeID = item.ToEmployee.EmployeeCodeID,
                    ToEmployeeCodeNo = item.ToEmployee.EmployeeCodeNo,
                    ToEmployeeNameAr = item.ToEmployee.Employee.EmployeeNameAr,
                    item.StartDate,
                    item.EndDate,
                    item.IsActive,
                    EServiceProxyStatus = item.EServiceProxyStatus.EServiceProxyStatus,
                    item.Notes
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("EServicesProxies/GetProxiesToMe/{EServiceTypeID}")]
        public JsonResult GetProxiesToMe(int EServiceTypeID)
        {
            int OrganizationID = this.WindowsUserOrganizationID;
            string ManagerEmployeeCodeNo = this.WindowsUserIdentity;

            EServicesTypesEnum? EServiceTypeEnum = EServiceTypeID == 0 ? null : (EServicesTypesEnum?)EServiceTypeID;
            List<EServicesProxiesBLL> EServiceProxyList
                = new EServicesProxiesBLL().GetByToEmployeeCodeID(this.WindowsEmployeeCode.EmployeeCodeID, EServiceTypeEnum);

            return Json(new
            {
                data = EServiceProxyList.Select(
                item => new
                {
                    item.EServiceProxyID,
                    EServiceTypeName = item.EServiceType.EServiceTypeName,
                    FromEmployeeCodeID = item.FromEmployee.EmployeeCodeID,
                    FromEmployeeCodeNo = item.FromEmployee.EmployeeCodeNo,
                    FromEmployeeNameAr = item.FromEmployee.Employee.EmployeeNameAr,
                    item.StartDate,
                    item.EndDate,
                    item.IsActive,
                    EServiceProxyStatus = item.EServiceProxyStatus.EServiceProxyStatus,
                    item.Notes
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("EServicesProxies/GetActiveEServiceProxy/{EServiceTypeID}")]
        public JsonResult GetActiveEServiceProxy(int EServiceTypeID)
        {
            int OrganizationID = this.WindowsUserOrganizationID;
            string ManagerEmployeeCodeNo = this.WindowsUserIdentity;

            EServicesProxiesBLL EServiceProxy
                = new EServicesProxiesBLL().GetActiveByFromEmployeeCodeID(this.WindowsEmployeeCode.EmployeeCodeID, (EServicesTypesEnum)EServiceTypeID);

            EServicesProxiesViewModel VM = new EServicesProxiesViewModel();
            if (EServiceProxy != null)
            {
                VM.EServiceProxyID = EServiceProxy.EServiceProxyID;
                VM.StartDate = EServiceProxy.StartDate;
                VM.EndDate = EServiceProxy.EndDate;
                VM.ToEmployee = new EmployeesViewModel()
                {
                    EmployeeCodeID = EServiceProxy.ToEmployee.EmployeeCodeID,
                    EmployeeCodeNo = EServiceProxy.ToEmployee.EmployeeCodeNo,
                    EmployeeNameAr = EServiceProxy.ToEmployee.Employee.EmployeeNameAr
                };
            }
            return Json(new
            {
                data = VM
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult CreateEServiceProxy(EServicesProxiesViewModel EServiceProxyVM)
        {
            int OrganizationID = this.WindowsUserOrganizationID;
            EServicesProxiesBLL EServiceProxy = new EServicesProxiesBLL();

            EServiceProxy.StartDate = EServiceProxyVM.StartDate;
            EServiceProxy.FromEmployee = new EmployeesCodesBLL() { EmployeeCodeID = EServiceProxyVM.FromEmployee.EmployeeCodeID.Value };
            EServiceProxy.ToEmployee = new EmployeesCodesBLL() { EmployeeCodeID = EServiceProxyVM.ToEmployee.EmployeeCodeID.Value };
            EServiceProxy.Notes = EServiceProxyVM.Notes;
            EServiceProxy.EServiceType = new EServicesTypesBLL() { EServiceTypeID = EServiceProxyVM.EServiceType.EServiceTypeID };
            EServiceProxy.LoginIdentity = this.WindowsEmployeeCode;
            Result result = EServiceProxy.Add();

            EServicesProxiesChartViewModel ChartVM =
                this.MapToChartVM(new EmployeesCodesBLL().GetAllEmployeesManagersByOrganizationID(OrganizationID,
                                    (EServicesTypesEnum)EServiceProxyVM.EServiceType.EServiceTypeID, EServiceProxyVM.ToEmployee.EmployeeCodeID.Value).FirstOrDefault(),
                EServiceProxyVM.ToEmployee.EmployeeCodeID.Value);

            if (result.EnumMember == EServicesProxiesEnum.Done.ToString())
            {
                // done
            }
            else if (result.EnumMember == EServicesProxiesEnum.RejectedBecauseLoginManagerIsSameAsProxyEmployee.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEServicesProxiesLoginManagerIsSameAsProxyEmployeeText);
            }
            else if (result.EnumMember == EServicesProxiesEnum.RejectedBecauseThereIsAlreadyActiveProxyExist.ToString())
            {
                string msg = string.Format(Resources.Globalization.ValidationEServicesProxiesThereIsAlreadyActiveProxyExistsText,
                                ((EServicesProxiesBLL)result.Entity).ToEmployee.Employee.EmployeeNameAr);
                throw new CustomException(msg);
            }
            else if (result.EnumMember == EServicesProxiesEnum.RejectedBecauseStartDateRequried.ToString())
            {
                throw new CustomException(string.Format(Resources.Globalization.RequiredFieldText, Resources.Globalization.StartDateText));
            }
            else if (result.EnumMember == EServicesProxiesEnum.RejectedBecauseThereIsPendingEVacationRequestExist.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEServicesProxiesThereIsPendingEVacationRequestExistText);
            }
            else if (result.EnumMember == EServicesProxiesEnum.RejectedBecauseThereIsVacationExist.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEServicesProxiesThereIsVacationExistText);
            }

            return Json(new { node = ChartVM }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("EServicesProxies/RevokeEServiceProxy/{EServiceProxyID}")]
        public ActionResult RevokeEServiceProxy(int EServiceProxyID)
        {
            Result result = new EServicesProxiesBLL() { LoginIdentity = this.WindowsEmployeeCode }
                                    .RevokeEServiceProxy(EServiceProxyID, EServicesProxiesStatusEnum.Expired);

            if (result.EnumMember == EServicesProxiesEnum.Done.ToString())
            {
                // do nothing
            }
            else if (result.EnumMember == EServicesProxiesEnum.RejectedBecauseEServiceProxyIsNotActive.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEServiceProxyIsNotActiveText);
            }

            return Json(new { EServiceProxyID = EServiceProxyID }, JsonRequestBehavior.AllowGet);
        }

        private EServicesProxiesChartViewModel MapToChartVM(EmployeesCodesBLL EmployeeCode, int ToEmployeeProxyAssigned)
        {
            int pid = EmployeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.ParentOrganization.OrganizationManager != null ? EmployeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationManager.EmployeeCodeID : 0;
            int id = EmployeeCode.EmployeeCodeID;
            //pid = pid == id ? 0 : pid;
            if (pid == id)
            { 
                pid = -1;id = 0;
            }
            string ManagerEmployeeCodeNo = WindowsUserIdentity;
            return new EServicesProxiesChartViewModel()
            {
                EmployeeCodeID = EmployeeCode.EmployeeCodeID,
                id = id,    //EmployeeCode.EmployeeCodeID,   //UniqueID++,//EmployeeCode.EmployeeCodeID + EmployeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationID,
                //pid = EmployeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.ParentOrganization != null ? EmployeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.ParentOrganization.OrganizationID : 0,
                pid = pid, //EmployeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.ParentOrganization.OrganizationManager != null ? EmployeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationManager.EmployeeCodeID : 0,
                OrganizationName = EmployeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationName,
                FullOrganizationName = EmployeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.FullOrganizationName,
                CurrentJobName = EmployeeCode.EmployeeCurrentJob.OrganizationJob.Job.JobName,
                EmployeeCodeNo = EmployeeCode.EmployeeCodeNo,
                FullEmployeeName = EmployeeCode.Employee.EmployeeNameAr,
                EmployeeName = EmployeeCode.EmployeeCodeNo + " - " + EmployeeCode.Employee.FirstNameAr + " " + EmployeeCode.Employee.LastNameAr,
                IsProxyAssignedImg = "/BaseEServices/RetrieveImage/" + (ToEmployeeProxyAssigned == EmployeeCode.EmployeeCodeID ? "done.jpg" : "blank.png"),
                Picture = "/Employees/RetrieveImage/" + EmployeeCode.EmployeeCodeNo,
                //tags = totalNodes > 4 ? GetTags(EmployeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure) : new string[] { },
                btnImg = "/BaseEServices/RetrieveImage/" + (ToEmployeeProxyAssigned == EmployeeCode.EmployeeCodeID ? "error2.png" : "plus.png"),
                nodeBg = (ToEmployeeProxyAssigned == EmployeeCode.EmployeeCodeID ? "#F57C00" : "#ffffff")
            };
        }

        private List<EServicesTypesViewModel> GetEServiceTypes(bool IsAll = false)
        {
            List<EServicesTypesBLL> lst = new EServicesTypesBLL().GetEServicesTypes();
            List<EServicesTypesViewModel> EServiceTypeLst = new List<EServicesTypesViewModel>();

            if (IsAll)
            {
                EServiceTypeLst.Add(new EServicesTypesViewModel()
                {
                    EServiceTypeID = 0,
                    EServiceTypeName = Resources.Globalization.AllText
                });
            }

            foreach (var item in lst)
            {
                EServiceTypeLst.Add(new EServicesTypesViewModel()
                {
                    EServiceTypeID = item.EServiceTypeID,
                    EServiceTypeName = item.EServiceTypeName
                });
            }

            return EServiceTypeLst;
        }
    }
}