using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 
namespace HCM.Models
{
    public class EServicesProxiesStatusViewModel : BaseViewModel
    {
        public int EServiceProxyStatusID { get; set; }
        public string EServiceProxyStatus { get; set; }
    }
}