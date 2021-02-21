using HCM.Classes.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class VacationsCancelViewModel : CommonPrivilagesEntity
    {
        public int VacationID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("VacationStartDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime VacationStartDate { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("VacationEndDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime VacationEndDate { get; set; }

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
        public DateTime WorkDate { get; set; }

        [Range(1, 999, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        [CustomDisplay("VacationPeriodText")]
        public int VacationPeriod { get; set; }
    }
}