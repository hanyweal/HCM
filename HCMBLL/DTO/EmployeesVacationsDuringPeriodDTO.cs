
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL.DTO
{
   public class EmployeesVacationsDuringPeriodDTO
    {
        public EmployeesVacationsDuringPeriodDTO(
            string _EmployeeCodeNo,
            string _EmployeeNameAr,
            string _EmployeeIDNo,
            string _OrganizationName,
           DateTime _VacationStartDate,
           DateTime _VacationEndDate,
           string _VacationTypeName
            )
        {
            EmployeeCodeNo = _EmployeeCodeNo;
            EmployeeNameAr = _EmployeeNameAr;
            EmployeeIDNo = _EmployeeIDNo;
            OrganizationName = _OrganizationName;
            VacationStartDate = _VacationStartDate;
            VacationEndDate = _VacationEndDate;
            VacationTypeName = _VacationTypeName;
        }
        public EmployeesVacationsDuringPeriodDTO()
        {

        }
        public string EmployeeCodeNo { get; }
        public string EmployeeNameAr { get; }
        public string EmployeeIDNo { get; }
        public string OrganizationName { get; }
        public DateTime VacationStartDate { get; }
        public DateTime VacationEndDate { get; }
        public int VacationPeriod { get { return this.VacationEndDate.Subtract(this.VacationStartDate).Days + 1; } }
        public string VacationTypeName { get; }
        








    }
}

