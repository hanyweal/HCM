using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL.DTO
{
    public class EmployeesOverTimesBasedOnAssigngingsDTO
    {
        public EmployeesOverTimesBasedOnAssigngingsDTO(string _EmployeeCodeNo,
            string _EmployeeNameAr,
            string _OrganizationName,
            string _JobName,
            string _RankCategoryName,
            string _RankName,
            DateTime _OverTimeStartDate,
            DateTime _OverTimeEndDate,
            int? _Sorting
            )
        {
            EmployeeCodeNo = _EmployeeCodeNo;
            EmployeeNameAr = _EmployeeNameAr;
            OrganizationName = _OrganizationName;
            JobName = _JobName;
            RankCategoryName = _RankCategoryName;
            RankName = _RankName;
            OverTimeStartDate = _OverTimeStartDate;
            OverTimeEndDate = _OverTimeEndDate;
            Sorting = _Sorting.HasValue ? _Sorting.Value : 0;
            
        }
        public string EmployeeCodeNo { get; set; }
        public string EmployeeNameAr { get; }
        public string OrganizationName { get; }
        public string JobName { get; }
        public string RankCategoryName { get; }
        public string RankName { get; }
        public DateTime OverTimeStartDate { get; }
        public DateTime OverTimeEndDate { get; }
        public int OverTimePeriod { get { return this.OverTimeEndDate.Subtract(this.OverTimeStartDate).Days + 1; } }
        public int? Sorting { get; set; }
    }
}
