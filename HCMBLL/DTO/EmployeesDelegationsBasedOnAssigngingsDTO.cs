using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL.DTO
{
    public class EmployeesDelegationsBasedOnAssigngingsDTO
    {
        public EmployeesDelegationsBasedOnAssigngingsDTO(string _EmployeeCodeNo,
            string _EmployeeNameAr,
            string _OrganizationName,
            string _JobName,
            string _RankCategoryName,
            string _RankName,
            DateTime _DelegationStartDate,
            DateTime _DelegationEndDate,
            string _DelegationTypeName,
            int? _Sorting
            )
        {
            EmployeeCodeNo = _EmployeeCodeNo;
            EmployeeNameAr = _EmployeeNameAr;
            OrganizationName = _OrganizationName;
            JobName = _JobName;
            RankCategoryName = _RankCategoryName;
            RankName = _RankName;
            DelegationStartDate = _DelegationStartDate;
            DelegationEndDate = _DelegationEndDate;
            DelegationTypeName = _DelegationTypeName;
            Sorting = _Sorting.HasValue ? _Sorting.Value : 0;
        }
        public string EmployeeCodeNo { get; set; }
        public string EmployeeNameAr { get; }
        public string OrganizationName { get; }
        public string JobName { get; }
        public string RankCategoryName { get; }
        public string RankName { get; }
        public DateTime DelegationStartDate { get; }
        public DateTime DelegationEndDate { get; }
        public int DelegationPeriod { get { return this.DelegationEndDate.Subtract(this.DelegationStartDate).Days + 1; } }
        public string DelegationTypeName { get; }
        public int? Sorting { get; set; }
    }
}
