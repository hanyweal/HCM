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
    public class EServicesProxiesViewModel
    {
        public int EServiceProxyID { get; set; }

        public EmployeesViewModel FromEmployee { get; set; }

        public EmployeesViewModel ToEmployee { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("StartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public OrganizationStructureViewModel Organization { get; set; }

        public EServicesProxiesStatusViewModel EServiceProxyStatus { get; set; }

        public EServicesTypesViewModel EServiceType { get; set; }

        public List<EServicesTypesViewModel> EServiceTypes { get; set; }

        public EmployeesCodesBLL WindowsEmployeeCode { get; set; }

        public List<OrganizationsStructuresBLL> WindowsUserOrganizationStructures { get; set; }

        public bool IsActive { get; set; }

        [CustomDisplay("NotesText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string Notes { get; set; }
    }

    public class EServicesProxiesChartViewModel
    {
        public int id { get; set; }
        public int pid { get; set; }
        public int EmployeeCodeID { get; internal set; }
        public string EmployeeCodeNo { get; internal set; }
        public string FullEmployeeName { get; set; }
        public string EmployeeName { get; set; }
        public string OrganizationName { get; set; }
        public string FullOrganizationName { get; set; }
        public string Picture { get; set; }
        //public string[] tags { get; set; }
        public string btnImg { get; set; } 
        public string IsProxyAssignedImg { get; internal set; }
        public string nodeBg { get; set; }
        public string CurrentJobName { get; internal set; }
    }
}