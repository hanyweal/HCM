using HCMBLL;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HCMServices.Controllers
{
    public class OrganizationStructureController : ApiController
    {
        [AllowAnonymous]
        [HttpGet, Route("api/OrganizationStructure/Get")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new OrganizationsStructuresBLL().GetOrganizationStructure());
        }

        [AllowAnonymous]
        [HttpGet, Route("api/OrganizationStructure/GetWithFullName")]
        public HttpResponseMessage GetWithFullName()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new OrganizationsStructuresBLL().GetOrganizationStructureWithFullName());
        }

        [AllowAnonymous]
        [HttpGet, Route("api/OrganizationStructure/GetOrganizationStructureAsTree")]
        public HttpResponseMessage GetOrganizationStructureAsTree()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new OrganizationsStructuresBLL().GetOrganizationStructureAsTree());
        }

        [AllowAnonymous]
        [HttpGet, Route("api/OrganizationStructure/GetByOrganizationStructureID/{OrganizationID}")]
        public HttpResponseMessage GetByOrganizationStructureID(int OrganizationID)
        {
            if (OrganizationID == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            OrganizationsStructuresBLL Org = new OrganizationsStructuresBLL().GetByOrganizationID(OrganizationID);

            if (Org != null)
                return Request.CreateResponse(HttpStatusCode.OK, Org);
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Organization not found!");
        }

        [AllowAnonymous]
        [HttpPost, Route("api/OrganizationStructure/UpdateShowInECM/{OrganizationID}/{ShowInECM}/{EmployeeCodeID}")]
        public HttpResponseMessage UpdateShowInECM(int OrganizationID, bool ShowInECM, int EmployeeCodeID)
        {
            try
            {
                new OrganizationsStructuresBLL() { OrganizationID = OrganizationID, ShowInECM = ShowInECM, LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } }.UpdateShowInECM();
                return Request.CreateResponse(HttpStatusCode.OK, OrganizationID);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost, Route("api/OrganizationStructure/WorkingWithDigitalDecisionInECM/{OrganizationID}/{ShowInECM}/{EmployeeCodeID}")]
        public HttpResponseMessage WorkingWithDigitalDecisionInECM(int OrganizationID, bool WorkingWithDigitalDecisionInECM, int EmployeeCodeID)
        {
            try
            {
                new OrganizationsStructuresBLL() { OrganizationID = OrganizationID, WorkingWithDigitalDecisionInECM = WorkingWithDigitalDecisionInECM, LoginIdentity = new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID } }.UpdateWorkingWithDigitalDecisionInECM();
                return Request.CreateResponse(HttpStatusCode.OK, OrganizationID);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}
