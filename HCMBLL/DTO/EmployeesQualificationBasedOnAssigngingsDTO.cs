using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL.DTO
{
    public class EmployeesQualificationBasedOnAssigngingsDTO
    {
        public EmployeesQualificationBasedOnAssigngingsDTO(string _EmployeeCodeNo,
            string _EmployeeNameAr,
            string _OrganizationName,
            string _JobName,
            string _RankCategoryName,
            string _RankName,
            string _QualificationDegreeName,
            string _QualificationName,
            string _GeneralSpecializationName,
            int? _Sorting
            )
        {
            EmployeeCodeNo = _EmployeeCodeNo;
            EmployeeNameAr = _EmployeeNameAr;
            OrganizationName = _OrganizationName;
            JobName = _JobName;
            RankCategoryName = _RankCategoryName;
            RankName = _RankName;
            QualificationDegreeName = _QualificationDegreeName;
            QualificationName = _QualificationName;
            GeneralSpecializationName = _GeneralSpecializationName;
            Sorting = _Sorting.HasValue ? _Sorting.Value : 0;
        }
        public string EmployeeCodeNo { get; set; }
        public string EmployeeNameAr { get; }
        public string OrganizationName { get; }
        public string JobName { get; }
        public string RankCategoryName { get; }
        public string RankName { get; }
        public string QualificationDegreeName { get; }
        public string QualificationName { get; }
        public string GeneralSpecializationName { get; }
        public int? Sorting { get; }
    }
}
