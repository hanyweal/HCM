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
using System.Web.Mvc;

namespace HCM.Controllers
{
    public class OrganizationStructureController : BaseController
    {
        public OrganizationStructureController()
        {

        }

        [CustomAuthorize]
        public override ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult IndexOrgChart()
        {
            return View();
        }

        public ActionResult OrganizationStructure()
        {
            return PartialView("~/Views/Sahred/_OrganizationStructure.cshtml");
        }

        public JsonResult GetByOrganizationID(int id)
        {
            OrganizationsStructuresBLL org = new OrganizationsStructuresBLL().GetByOrganizationID(id);

            return Json(new
            {
                data = new
                {
                    OrganizationCode = org.OrganizationCode,
                    OrganizationName = org.OrganizationName,
                    BranchID = org.Branch.BranchID,
                    OrganizationNameWithBranch = org.ParentOrganization != null ? org.ParentOrganization.OrganizationNameWithBranch : "",
                    EmployeeCodeNo = org.OrganizationManager != null ? org.OrganizationManager.EmployeeCodeNo : "",
                    EmployeeNameAr = org.OrganizationManager != null ? org.OrganizationManager.Employee.EmployeeNameAr : "",
                    EmployeeNameEn = org.OrganizationManager != null ? org.OrganizationManager.Employee.EmployeeNameEn : "",
                    EmployeeCodeID = org.OrganizationManager != null ? org.OrganizationManager.EmployeeCodeID : 0,
                    EmployeeOrganizationName = org.OrganizationManager != null && org.OrganizationManager.EmployeeCurrentJob != null ?
                                                org.OrganizationManager.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationName : "",

                    EmployeeIDNo = org.OrganizationManager != null ? org.OrganizationManager.Employee.EmployeeIDNo : "",
                    JoinDate = org.OrganizationManager != null && org.OrganizationManager.HiringRecord != null ? org.OrganizationManager.HiringRecord.JoinDate : (DateTime?)null,

                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetOrganizationStructure()
        {
            //var oo = new OrganizationsStructuresBLL().GetOrganizationStructure();
            //List<OrganizationsStructuresBLL> orgList = new OrganizationsStructuresBLL().GetOrganizationStructureAsTree();
            OrganizationsStructuresBLL orgList = new OrganizationsStructuresBLL().GetOrganizationStructureAsTree();
            return Json(new { data = orgList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(OrganizationStructureViewModel OrganizationStructureVM)
        {
            OrganizationsStructuresBLL OrganizationsStructureBLL = new OrganizationsStructuresBLL()
            {
                OrganizationCode = OrganizationStructureVM.OrganizationCode,
                OrganizationName = OrganizationStructureVM.OrganizationName,
                OrganizationManager = OrganizationStructureVM.ManagerCodeID.HasValue ? new EmployeesCodesBLL() { EmployeeCodeID = (int)OrganizationStructureVM.ManagerCodeID } : null,
                ParentOrganization = new OrganizationsStructuresBLL() { OrganizationID = OrganizationStructureVM.ParentOrganizationID },
                Branch = new BranchesBLL() { BranchID = OrganizationStructureVM.BranchID },
                LoginIdentity = UserIdentity
            };
            Result result = OrganizationsStructureBLL.Add();
            OrganizationsStructuresBLL organizationStructure = (OrganizationsStructuresBLL)result.Entity;
            if (result.EnumMember == DelegationsValidationEnum.Done.ToString())
            {
                OrganizationStructureVM.OrganizationID = organizationStructure.OrganizationID;
            }
            //return View("Index");
            OrganizationsStructuresBLL organizationStructureBLL = new OrganizationsStructuresBLL().GetByOrganizationID((int)OrganizationStructureVM.OrganizationID);
            var data = new
            {
                id = OrganizationStructureVM.OrganizationID,
                pid = OrganizationStructureVM.ParentOrganizationID,
                OrganizationName = OrganizationStructureVM.OrganizationName,
                FullOrganizationName = OrganizationStructureVM.OrganizationName,
                ManagerIDNo = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.Employee.EmployeeIDNo : string.Empty,
                ManagerCodeID = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeID : (int?)null,
                ManagerCodeNo = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo : null,
                FullManagerName = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.Employee.EmployeeNameAr : null,
                ManagerName = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo + " - " + organizationStructureBLL.OrganizationManager.Employee.FirstNameAr + " " + organizationStructureBLL.OrganizationManager.Employee.LastNameAr : null,
                ManagePic = "/Employees/RetrieveImage/" + (organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo : "0"),
                tags = GetTagsForOrganizationChart(organizationStructureBLL)
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(OrganizationStructureViewModel OrganizationStructureVM)
        {
            OrganizationsStructuresBLL organizationStructureBLL = new OrganizationsStructuresBLL().GetByOrganizationID((int)OrganizationStructureVM.OrganizationID);
            organizationStructureBLL.OrganizationCode = OrganizationStructureVM.OrganizationCode;
            organizationStructureBLL.OrganizationName = OrganizationStructureVM.OrganizationName;
            organizationStructureBLL.Branch = new BranchesBLL() { BranchID = OrganizationStructureVM.BranchID };
            organizationStructureBLL.OrganizationManager = OrganizationStructureVM.ManagerCodeID.HasValue ? new EmployeesCodesBLL() { EmployeeCodeID = (int)OrganizationStructureVM.ManagerCodeID } : null;
            organizationStructureBLL.LoginIdentity = UserIdentity;
            Result result = organizationStructureBLL.Update();

            organizationStructureBLL = new OrganizationsStructuresBLL().GetByOrganizationID((int)OrganizationStructureVM.OrganizationID);
            //return View("Index");
            var data = new
            {
                id = organizationStructureBLL.OrganizationID,
                pid = organizationStructureBLL.ParentOrganization != null ? organizationStructureBLL.ParentOrganization.OrganizationID : 0,
                OrganizationName = organizationStructureBLL.OrganizationName,
                FullOrganizationName = organizationStructureBLL.OrganizationName,
                ManagerIDNo = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.Employee.EmployeeIDNo : string.Empty,
                ManagerCodeID = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeID : (int?)null,
                ManagerCodeNo = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo : null,
                FullManagerName = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.Employee.EmployeeNameAr : null,
                ManagerName = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo + " - " + organizationStructureBLL.OrganizationManager.Employee.FirstNameAr + " " + organizationStructureBLL.OrganizationManager.Employee.LastNameAr : null,
                ManagePic = "/Employees/RetrieveImage/" + (organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo : "0"),
                tags = GetTagsForOrganizationChart(organizationStructureBLL)
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [IgnoreModelProperties("OrganizationCode,OrganizationName")]
        public ActionResult EditParent(OrganizationStructureViewModel OrganizationStructureVM)
        {
            OrganizationsStructuresBLL organizationStructureBLL = new OrganizationsStructuresBLL().GetByOrganizationID((int)OrganizationStructureVM.OrganizationID);
            organizationStructureBLL.ParentOrganization.OrganizationID = OrganizationStructureVM.ParentOrganizationID;
            organizationStructureBLL.LoginIdentity = UserIdentity;
            Result result = organizationStructureBLL.Update();

            organizationStructureBLL = new OrganizationsStructuresBLL().GetByOrganizationID((int)OrganizationStructureVM.OrganizationID);
            //return View("Index");
            var data = new
            {
                id = organizationStructureBLL.OrganizationID,
                pid = organizationStructureBLL.ParentOrganization != null ? organizationStructureBLL.ParentOrganization.OrganizationID : 0,
                OrganizationName = organizationStructureBLL.OrganizationName,
                FullOrganizationName = organizationStructureBLL.OrganizationName,
                ManagerIDNo = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.Employee.EmployeeIDNo : string.Empty,
                ManagerCodeID = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeID : (int?)null,
                ManagerCodeNo = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo : null,
                FullManagerName = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.Employee.EmployeeNameAr : null,
                ManagerName = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo + " - " + organizationStructureBLL.OrganizationManager.Employee.FirstNameAr + " " + organizationStructureBLL.OrganizationManager.Employee.LastNameAr : null,
                ManagePic = "/Employees/RetrieveImage/" + (organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo : "0"),
                tags = GetTagsForOrganizationChart(organizationStructureBLL)
            };

            return Json(data, JsonRequestBehavior.AllowGet);
            //return View("Index");
        }

        [HttpPost]
        [Route("{controller}/ChangeOrganizationManager")]
        [IgnoreModelProperties("OrganizationCode,OrganizationName,ParentOrganizationID")]
        public ActionResult ChangeOrganizationManager(OrganizationStructureViewModel OrganizationStructureVM)
        {
            OrganizationsStructuresBLL organizationStructureBLL = new OrganizationsStructuresBLL()
            {
                LoginIdentity = this.UserIdentity,
                OrganizationID = OrganizationStructureVM.OrganizationID.Value,
                OrganizationManager = OrganizationStructureVM.ManagerCodeID.HasValue ? new EmployeesCodesBLL() { EmployeeCodeID = OrganizationStructureVM.ManagerCodeID.Value } : null,
            };

            Result result = organizationStructureBLL.ChangeOrganizationManager(organizationStructureBLL);

            organizationStructureBLL = new OrganizationsStructuresBLL().GetByOrganizationID((int)OrganizationStructureVM.OrganizationID);
            //return View("Index");
            var data = new
            {
                id = organizationStructureBLL.OrganizationID,
                pid = organizationStructureBLL.ParentOrganization != null ? organizationStructureBLL.ParentOrganization.OrganizationID : 0,
                OrganizationName = organizationStructureBLL.OrganizationName,
                FullOrganizationName = organizationStructureBLL.OrganizationName,
                ManagerIDNo = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.Employee.EmployeeIDNo : string.Empty,
                ManagerCodeID = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeID : (int?)null,
                ManagerCodeNo = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo : null,
                FullManagerName = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.Employee.EmployeeNameAr : null,
                ManagerName = organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo + " - " + organizationStructureBLL.OrganizationManager.Employee.FirstNameAr + " " + organizationStructureBLL.OrganizationManager.Employee.LastNameAr : null,
                ManagePic = "/Employees/RetrieveImage/" + (organizationStructureBLL.OrganizationManager != null ? organizationStructureBLL.OrganizationManager.EmployeeCodeNo : "0"),
                tags = GetTagsForOrganizationChart(organizationStructureBLL)
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            OrganizationsStructuresBLL organizationStructureBLL = new OrganizationsStructuresBLL();
            organizationStructureBLL.LoginIdentity = UserIdentity;
            Result result = organizationStructureBLL.Remove(id);
            if ((System.Type)result.EnumType == typeof(OrganizationStructureValidationEnum))
            {
                if (result.EnumMember == OrganizationStructureValidationEnum.RejectedBecauseOfRelatedData.ToString())
                {
                    throw new CustomException(Resources.Globalization.ValidationAlreadyHasChildOrgText);
                }
            }

            return View("Index");
        }

        [HttpGet]
        [Route("{controller}/GetOrganizationsByName/{OrganizationsName}")]
        public JsonResult GetOrganizationsByName(string OrganizationsName)
        {
            var oData = new OrganizationsStructuresBLL().GetOrganizationStructure().Where(e => e.OrganizationNameWithBranch.Contains(OrganizationsName)).Select(item => new
            {
                OrganizationID = item.OrganizationID,
                OrganizationNameWithBranch = item.OrganizationNameWithBranch
            });
            return Json(new
            {
                data = oData
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("{controller}/GetParentOrganizationsManagers/{id}")]
        public JsonResult GetParentOrganizationsManagers(int id)
        {
            return Json(new
            {
                data = new OrganizationsStructuresBLL().GetParentOrganizationsManagers(id).Select(x => new
                {
                    //OrganizationName = x.OrganizationName,
                    FullOrganizationName = x.FullOrganizationName,
                    ManagerCodeID = x.OrganizationManager.EmployeeCodeID,
                    ManagerCodeNo = x.OrganizationManager.EmployeeCodeNo,
                    ManagerNameAr = x.OrganizationManager.Employee.EmployeeNameAr
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("OrganizationStructure/GetOrganizationChart")]
        public JsonResult GetOrganizationChart()
        {
            List<OrganizationsStructuresBLL> Orgs = new OrganizationsStructuresBLL().GetOrganizationStructureWithManagers();
            if (Orgs != null)
            {
                return Json(new
                {
                    data = Orgs
                            //.Where(x=> new OrganizationsStructuresBLL().GetByOrganizationIDsWithhAllChilds(1803).Contains(x.OrganizationID))
                            .Select(x => new
                            {
                                id = x.OrganizationID,
                                pid = x.ParentOrganization != null ? x.ParentOrganization.OrganizationID : 0,
                                OrganizationName = x.OrganizationName,
                                FullOrganizationName = x.FullOrganizationName,
                                ManagerIDNo = x.OrganizationManager != null ? x.OrganizationManager.Employee.EmployeeIDNo : string.Empty,
                                ManagerCodeID = x.OrganizationManager != null ? x.OrganizationManager.EmployeeCodeID : (int?)null,
                                ManagerCodeNo = x.OrganizationManager != null ? x.OrganizationManager.EmployeeCodeNo : null,
                                FullManagerName = x.OrganizationManager != null ? x.OrganizationManager.Employee.EmployeeNameAr : null,
                                ManagerName = x.OrganizationManager != null ? x.OrganizationManager.EmployeeCodeNo + " - " + x.OrganizationManager.Employee.FirstNameAr + " " + x.OrganizationManager.Employee.LastNameAr : null,
                                ManagePic = "/Employees/RetrieveImage/" + (x.OrganizationManager != null ? x.OrganizationManager.EmployeeCodeNo : "0"),
                                tags = GetTagsForOrganizationChart(x),
                            })
                }, JsonRequestBehavior.AllowGet);
            }
            else
                throw new Exception();
        }

        private string[] GetTagsForOrganizationChart(OrganizationsStructuresBLL org)
        {
            int ParentOrganizationID = org.ParentOrganization != null ? org.ParentOrganization.OrganizationID : 0;
            int OrganizationID = org.OrganizationID;
            int BranchID = org.Branch != null ? org.Branch.BranchID : 0;
            if (ParentOrganizationID == 1)
            {
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
            }
            else
            {
                return new string[] { };
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{controller}/GetOrganizationManagers/{id}")]
        public JsonResult GetOrganizationManagers(int id)
        {
            return Json(new
            {
                data = new OrganizationsManagersBLL().GetOrganizationsManagers(id).Select(x => new
                {  
                    ManagerCodeNo = x.ManagerCode.EmployeeCodeNo,
                    ManagerName = x.ManagerCode.Employee.EmployeeNameAr,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                })
            }, JsonRequestBehavior.AllowGet);
        }
    }
}