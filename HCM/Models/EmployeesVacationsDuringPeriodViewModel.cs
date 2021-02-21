using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCM.Models
{
    public class EmployeesVacationsDuringPeriodViewModel
    {

        public string EmployeeCodeNo { get; set; }
        public string EmployeeNameAr { get; set; }
        public string EmployeeIDNo { get; set; }
        public string OrganizationName { get; set; }
        public DateTime VacationStartDate { get; set; }
        public DateTime VacationEndDate { get; set; }
        public int VacationPeriod { get; set; }
        public string VacationTypeName { get; set; }

    }
}