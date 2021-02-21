using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL.DTO
{
    public class EmployeesVacationsBasedOnAssigngingsDTO
    {
        public EmployeesVacationsBasedOnAssigngingsDTO(string _EmployeeCodeNo,
            string _EmployeeNameAr,
            string _OrganizationName,
            string _JobName,
            string _RankCategoryName,
            string _RankName,
            DateTime _VacationStartDate,
            DateTime _VacationEndDate,
            string _VacationTypeName,
            int? _Sorting
            )
        {
            EmployeeCodeNo = _EmployeeCodeNo;
            EmployeeNameAr = _EmployeeNameAr;
            OrganizationName = _OrganizationName;
            JobName = _JobName;
            RankCategoryName = _RankCategoryName;
            RankName = _RankName;
            VacationStartDate = _VacationStartDate;
            VacationEndDate = _VacationEndDate;
            VacationTypeName = _VacationTypeName;
            _Sorting = _Sorting.HasValue ? _Sorting.Value : 0;
        }
        public string EmployeeCodeNo { get; set; }
        public string EmployeeNameAr { get; }
        public string OrganizationName { get; }
        public string JobName { get; }
        public string RankCategoryName { get; }
        public string RankName { get; }
        public DateTime VacationStartDate { get; }
        public DateTime VacationEndDate { get; }
        public int VacationPeriod { get { return this.VacationEndDate.Subtract(this.VacationStartDate).Days + 1; } }
        public string VacationTypeName { get; }
        public int? Sorting { get; set; }
    }
}
