using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 
namespace HCM.Models
{
    public class SectorsTypesViewModel : BaseViewModel
    {
        public int SectorTypeID { get; set; }
        public string SectorTypeName { get; set; }
    }
}