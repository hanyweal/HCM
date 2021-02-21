using HCM.Classes.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class EmployeesEvaluationsViewModel : CommonPrivilagesEntity
    {
        public int EmployeeEvaluationID { get; set; }

        [CustomDisplay("MaturityYearBalanceText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int MaturityYearID { get; set; }

        [CustomDisplay("EvaluationPointsText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EvaluationPointID { get; set; }

        [CustomDisplay("EvaluationDegreeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public float EvaluationDegree { get; set; }
        public EmployeesViewModel EmployeeVM { get; set; }
        public int EvaluationQuarterID { get; set; }
        public decimal DirectManagerEvaluation { get; set; }
        public decimal TimeAttendanceEvaluation { get; set; }
        public decimal ViolationsEvaluation { get; set; }

    }
}