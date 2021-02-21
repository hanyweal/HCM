using HCM.Classes.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class VacationsExtentionViewModel : CommonPrivilagesEntity
    {
        public int VacationID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("FromDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime FromDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("ToDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? ToDate { get; set; }


        [CustomDisplay("VacationTypeText")]
        public int VacationTypeID { get; set; }

        [CustomDisplay("SickVacationTypeText")]
        public int SickVacationTypeID { get; set; }

        [CustomDisplay("NotesText")]
        public string Notes { get; set; }

        [CustomDisplay("MainframeNoText")]
        public string MainframeNo { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("WorkDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime? WorkDate { get; set; }

        [Range(1, 999, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        [CustomDisplay("ExtentionPeriodText")]
        public int ExtentionPeriod { get; set; }

        public int EmployeeCodeID { get; set; }

        public int? SportsSeasonID { get; set; }
    }
}