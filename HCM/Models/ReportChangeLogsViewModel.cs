using HCM.Classes.CustomAttributes;
using System;

namespace HCM.Models
{
    public class ReportChangeLogsViewModel
    {
        [CustomDisplay("EmployeesText")]
        public int? EmployeeCodeID { get; set; }

        [CustomDisplay("BusinessSubCategoriesText")]
        public int? BusinssSubCategoryID { get; set; }

        [CustomDisplay("StartDateText")]
        public DateTime StartDate { get; set; }

        [CustomDisplay("EndDateText")]
        public DateTime EndDate { get; set; }


    }
}