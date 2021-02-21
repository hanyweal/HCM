using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL.DTO
{
    public class ActiveEmployeesWithFullDataDTO
    {
        public ActiveEmployeesWithFullDataDTO(string _EmployeeCodeNo,
            string _EmployeeIDNo,
            string _GenderName,
            string _NationalityName,
            string _FirstNameAr,
            string _MiddleNameAr,
            string _GrandFatherNameAr,
            string _LastNameAr,
            DateTime? _EmployeeBirthDate,
            string _OrganizationName,
            string _JoinDate,    //DateTime? _JoinDate,
            string _RankName,
            string _CareerDegreeName,
            string _JobName,
            string _JobNo,
            string _ActualJobName,
            string _ActualOrganizationName,
            double? _TransfareAllowance,
            double? _TotalAllowances,
            double? _TotalDeductions,
            double? _NetSalary,
            double? _BasicSalary,
            string _QualificationDegreeName,
            string _QualificationName,
            string _GeneralSpecializationName,
            string _MobileNo,
            string _HiringDate  //DateTime? _HiringDate

            )
        {
            EmployeeCodeNo = _EmployeeCodeNo; 
            EmployeeIDNo = _EmployeeIDNo;
            GenderName = _GenderName;
            NationalityName = _NationalityName;
            EmployeeBirthDate = _EmployeeBirthDate;
            EmployeeNameAr = _FirstNameAr + " " + _MiddleNameAr + " " + _GrandFatherNameAr + " " + _LastNameAr;
            OrganizationName = _OrganizationName;
            JoinDate = _JoinDate;
            RankName = _RankName;
            CareerDegreeName = _CareerDegreeName;
            JobName = _JobName;
            JobNo = _JobNo;
            ActualJobName = _ActualJobName;
            ActualOrganizationName = _ActualOrganizationName;
            TransfareAllowance = _TransfareAllowance;
            TotalAllowances = _TotalAllowances;
            TotalDeductions = _TotalDeductions;
            NetSalary = _NetSalary;
            BasicSalary = _BasicSalary;
            QualificationDegreeName = _QualificationDegreeName;
            QualificationName = _QualificationName;
            GeneralSpecializationName= _GeneralSpecializationName;
            MobileNo = _MobileNo;
            HiringDate = _HiringDate;
        }
        public ActiveEmployeesWithFullDataDTO()
        {

        }
        public string EmployeeCodeNo { get; set; }
        public string EmployeeIDNo { get; set; }
        public string GenderName { get; set; }
        public string NationalityName { get; set; }
        public string EmployeeNameAr { get; set; }
        public DateTime? EmployeeBirthDate { 
           private get;
            set; 
        }
        public string EmployeeBirthDateUmAlQura
        {
            get
            {

                return ((DateTime)this.EmployeeBirthDate).ToString("dd/MM/yyyy");
            }
        }
        public string EmployeeBirthDateGreg
        {
            get
            {

                return Globals.Calendar.UmAlquraToGreg(((DateTime)this.EmployeeBirthDate).ToString("dd/MM/yyyy"));
            }
        }
        public int EmployeeAge
        {
            get
            {
                DateTime today = DateTime.Today;
                var age= today.Year -((DateTime)this.EmployeeBirthDate).Year;
                if (((DateTime)this.EmployeeBirthDate).Date > today.AddYears(-age)) age--;
                return age;
            }
        }
        public string MobileNo { get; set; }
        public string HiringDate { get; set; } //public DateTime? HiringDate { get; set; }
        public string OrganizationName { get; set; }
        public string JoinDate { get; set; } //public DateTime? JoinDate { get; set; }
        public string RankName { get; set; }
        public string CareerDegreeName { get; set; }
        public string JobName { get; set; }
        public string JobNo { get; set; }
        public string ActualJobName { get; set; }
        public string ActualOrganizationName { get; set; }
        public double? TransfareAllowance { get; set; }
        public double? TotalAllowances { get; set; }
        public double? TotalDeductions { get; set; }
        public double? NetSalary { get; set; }
        public double? BasicSalary { get; set; }
        public string QualificationDegreeName { get; set; }
        public string QualificationName { get; set; }
        public string GeneralSpecializationName { get; set; }
        public string PreviosJobName { get; set; }
    }
}
