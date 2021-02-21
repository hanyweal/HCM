using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 
namespace HCM.Models
{
    public class EServicesTypesViewModel : BaseViewModel
    {
        public int EServiceTypeID { get; set; }
        public string EServiceTypeName { get; set; }
    }
}