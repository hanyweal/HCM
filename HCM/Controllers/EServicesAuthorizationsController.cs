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
using System.Web;
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class EServicesAuthorizationsController : BaseEServicesController
    {
        // GET: EServicesAuthorizations
        public ActionResult Index()
        {
            EServicesAuthorizationsViewModel VM = new EServicesAuthorizationsViewModel();
            VM.EServiceTypes = this.GetEServiceTypes();
            VM.WindowsUserOrganizationStructures = this.WindowsUserOrganizationStructures;
            VM.Organization = new OrganizationStructureViewModel() { OrganizationID = this.WindowsUserOrganizationID };
            VM.WindowsEmployeeCode = this.WindowsEmployeeCode;            
            return View(VM);
        }

        [HttpGet]
        [Route("EServicesAuthorizations/GetFirstLevel/{EServiceTypeID}")]
        public JsonResult GetFirstLevel(int EServiceTypeID)
        {
            int OrganizationID = this.WindowsUserOrganizationID;
            string ManagerEmployeeCodeNo = WindowsUserIdentity;

            List<EServicesAuthorizationsBLL> Orgs = new EServicesAuthorizationsBLL().GetFirstLevel(OrganizationID, (EServicesTypesEnum)EServiceTypeID);            
            if (Orgs != null)
            {       
                return Json(new
                {
                    total = Orgs.Count,
                    data = Orgs.Select(x => MapToChartVM(x , Orgs.Count))
                }, JsonRequestBehavior.AllowGet);
            }
            else
                throw new Exception();
        }

        [HttpPost]
        [Route("EServicesAuthorizations/SetAuthorizations/{EServiceTypeID}/{OrganizationID}/{EmployeeCodeID}/{TotalNodes}")]
        public ActionResult SetAuthorizations(int EServiceTypeID, int OrganizationID, int EmployeeCodeID, int TotalNodes)
        {
            int LoginOrganizationID = WindowsUserOrganizationID;
            Result result = new EServicesAuthorizationsBLL() { LoginIdentity = this.WindowsEmployeeCode }
                                    .SetAuthorizations(EServiceTypeID, LoginOrganizationID, OrganizationID, EmployeeCodeID);
            EServicesAuthorizationsChartViewModel ChartVM = this.MapToChartVM(new EServicesAuthorizationsBLL().GetByOrganizationID(OrganizationID, (EServicesTypesEnum)EServiceTypeID), TotalNodes);

            if (result.EnumMember == EServicesAuthorizationsEnum.Done.ToString())
            {
                // do nothing
            }
            else if (result.EnumMember == EServicesAuthorizationsEnum.RejectedBecauseLoginOrganizationIsSameAsAuthorization.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEServicesAuthorizedLoginOrganizationIsSameAsAuthorizationText);
            }
            else if (result.EnumMember == EServicesAuthorizationsEnum.RejectedBecauseThereIsNoManagerForThisOrganization.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEServicesAuthorizedThereIsNoManagerForThisOrganizationText);
            }
            else if (result.EnumMember == EServicesAuthorizationsEnum.RejectedBecauseOrganizationManagerIsNotAuthorized.ToString())
            {
                throw new CustomException(Resources.Globalization.ValidationEServicesAuthorizedManagerIsNotAuthorizedText);
            }

            return Json(new { node = ChartVM }, JsonRequestBehavior.AllowGet);
        }

        private string[] GetTags(OrganizationsStructuresBLL org)
        {
            int ParentOrganizationID = org.ParentOrganization != null ? org.ParentOrganization.OrganizationID : 0;
            int OrganizationID = org.OrganizationID;
            int BranchID = org.Branch != null ? org.Branch.BranchID : 0;
            //if ( ParentOrganizationID == 1)
            //if(true)
            //{
            List<string> lst = new List<string>();
            if (OrganizationID % 2 == 0)
                lst.Add("subLevels0");
            else if (Math.Abs(OrganizationID - BranchID) % 2 == 0)
                lst.Add("subLevels1");
            else if (BranchID % 2 == 0)
                lst.Add("subLevels2");
            else
                lst.Add("subLevels3");

            return lst.ToArray();
            //}
            //else
            //{
            //    return new string[] { };
            //}
        }

        private List<EServicesTypesViewModel> GetEServiceTypes()
        {
            List<EServicesTypesBLL> lst = new EServicesTypesBLL().GetEServicesTypes();
            List<EServicesTypesViewModel> EServiceTypeLst = new List<EServicesTypesViewModel>();
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

        private EServicesAuthorizationsChartViewModel MapToChartVM(EServicesAuthorizationsBLL eServiceAuthorization, int totalNodes)
        {
            string ManagerEmployeeCodeNo = WindowsUserIdentity;
            return new EServicesAuthorizationsChartViewModel()
            {
                EServiceAuthorizationID = eServiceAuthorization.EServiceAuthorizationID,
                id = eServiceAuthorization.Organization.OrganizationID,
                pid = eServiceAuthorization.Organization.ParentOrganization != null ? eServiceAuthorization.Organization.ParentOrganization.OrganizationID : 0,
                OrganizationName = eServiceAuthorization.Organization.OrganizationName,
                FullOrganizationName = eServiceAuthorization.Organization.FullOrganizationName,
                ManagerIDNo = eServiceAuthorization.Organization.OrganizationManager != null ? eServiceAuthorization.Organization.OrganizationManager.Employee.EmployeeIDNo : string.Empty,
                ManagerCodeID = eServiceAuthorization.Organization.OrganizationManager != null ? eServiceAuthorization.Organization.OrganizationManager.EmployeeCodeID : 0,
                ManagerCodeNo = eServiceAuthorization.Organization.OrganizationManager != null ? eServiceAuthorization.Organization.OrganizationManager.EmployeeCodeNo : null,
                FullManagerName = eServiceAuthorization.Organization.OrganizationManager != null ? eServiceAuthorization.Organization.OrganizationManager.Employee.EmployeeNameAr : null,
                ManagerName = eServiceAuthorization.Organization.OrganizationManager != null ? eServiceAuthorization.Organization.OrganizationManager.EmployeeCodeNo + " - " + eServiceAuthorization.Organization.OrganizationManager.Employee.FirstNameAr + " " + eServiceAuthorization.Organization.OrganizationManager.Employee.LastNameAr : null,
                AuthorizedPersonCodeID = eServiceAuthorization.AuthorizedPerson.EmployeeCodeID,
                AuthorizedPersonCodeNo = eServiceAuthorization.AuthorizedPerson.EmployeeCodeNo,
                AuthorizedPersonFullName = eServiceAuthorization.AuthorizedPerson.Employee.EmployeeNameAr,
                AuthorizedPersonName = eServiceAuthorization.AuthorizedPerson.EmployeeCodeNo + " - " + eServiceAuthorization.AuthorizedPerson.Employee.FirstNameAr + " " + eServiceAuthorization.AuthorizedPerson.Employee.LastNameAr,
                Picture = "/Employees/RetrieveImage/" + (eServiceAuthorization.Organization.OrganizationManager != null ? eServiceAuthorization.Organization.OrganizationManager.EmployeeCodeNo : "0"),
                AuthPicture = "/Employees/RetrieveImage/" + eServiceAuthorization.AuthorizedPerson.EmployeeCodeNo,
                tags = totalNodes > 4 ? GetTags(eServiceAuthorization.Organization) : new string[] { },
                btnImg = "/BaseEServices/RetrieveImage/" + ((ManagerEmployeeCodeNo == eServiceAuthorization.AuthorizedPerson.EmployeeCodeNo) ? "plus.png" : "error2.png"),
                IsYourAuth = (ManagerEmployeeCodeNo == eServiceAuthorization.AuthorizedPerson.EmployeeCodeNo),
                IsYourAuthText = (ManagerEmployeeCodeNo == eServiceAuthorization.AuthorizedPerson.EmployeeCodeNo) ? Resources.Globalization.EServicesYouRAuthorizedText : Resources.Globalization.EServicesYouGiveAuthorityText,
                IsYourAuthImg = "/BaseEServices/RetrieveImage/" + ((ManagerEmployeeCodeNo == eServiceAuthorization.AuthorizedPerson.EmployeeCodeNo) ? "done.jpg" : "error2.png")
            };
        }

    }
}