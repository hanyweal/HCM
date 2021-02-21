using HCM.Classes.CustomAttributes;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HCM.Models
{
    public class EServicesAuthorizationsViewModel
    {
        public int EServiceAuthorizationID { get; set; }

        public EmployeesViewModel AuthorizedPerson { get; set; }

        public OrganizationStructureViewModel Organization { get; set; }

        public EServicesTypesViewModel EServiceType { get; set; }

        public List<EServicesTypesViewModel> EServiceTypes { get; set; }

        public EmployeesCodesBLL WindowsEmployeeCode { get; set; }

        public List<OrganizationsStructuresBLL> WindowsUserOrganizationStructures { get; set; }
    }

    public class EServicesAuthorizationsChartViewModel
    {
        public int EServiceAuthorizationID { get; set; }
        public int id { get; set; }
        public int pid { get; set; }
        public string OrganizationName { get; set; }
        public string FullOrganizationName { get; set; }
        public string ManagerIDNo { get; set; }
        public int ManagerCodeID { get; set; }
        public string ManagerCodeNo { get; set; }
        public string FullManagerName { get; set; }
        public string ManagerName { get; set; }
        public int AuthorizedPersonCodeID { get; set; }
        public string AuthorizedPersonCodeNo { get; set; }
        public string AuthorizedPersonFullName { get; set; }
        public string AuthorizedPersonName { get; set; }
        public string Picture { get; set; }
        public string AuthPicture { get; set; }
        public string[] tags { get; set; }
        public string btnImg { get; set; }
        public bool IsYourAuth { get; set; }
        public string IsYourAuthText { get; set; }
        public string IsYourAuthImg { get; set; }
    }
}