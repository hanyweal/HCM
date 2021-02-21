using HCM.Classes.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCM.Models
{
    public class ReportEmployeesVacationsDuringPeriodViewModel
    {
        [CustomDisplay("VacationTypeText")]       
        public int? VacationTypeID { get; set; }

        [CustomDisplay("StartDateText")]
        public DateTime StartDate { get; set; }

        [CustomDisplay("EndDateText")]
        public DateTime EndDate { get; set; }

    }
}