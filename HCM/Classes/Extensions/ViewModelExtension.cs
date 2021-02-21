using HCM.Controllers;
using HCM.Models;
using HCMBLL;
using System;

namespace HCM.Classes.Extensions
{
    public static class ViewModelExtension
    {
        public static EmployeesViewModel GetEmployee(this EmployeesViewModel employeeVM)
        {
            EmployeesCodesBLL employeeCode = new EmployeesCodesBLL().GetByEmployeeCodeID(employeeVM.EmployeeCodeID.Value);
            EmployeesBLL employee = employeeCode.Employee;
            return new EmployeesViewModel()
            {
                EmployeeCodeID = employee.ActiveEmployeeCode != null ? employee.ActiveEmployeeCode.EmployeeCodeID : 0,
                EmployeeCodeNo = employee.ActiveEmployeeCode != null ? employee.ActiveEmployeeCode.EmployeeCodeNo : Resources.Globalization.EmptyEmployeeCodeAfterEndOfServiceText,
                EmployeeNameAr = employee.EmployeeNameAr,
                EmployeeIDNo = employee.EmployeeIDNo,
                EmployeeCareerHistoryID = employeeCode.EmployeeCurrentJob != null ? employeeCode.EmployeeCurrentJob.EmployeeCareerHistoryID : (int?)null,
                EmployeeJobName = employeeCode.EmployeeCurrentJob != null ? employeeCode.EmployeeCurrentJob.OrganizationJob.Job.JobName : null,
                EmployeeRankName = employeeCode.EmployeeCurrentJob != null ? employeeCode.EmployeeCurrentJob.OrganizationJob.Rank.RankName : null,
                EmployeeJobNo = employeeCode.EmployeeCurrentJob != null ? employeeCode.EmployeeCurrentJob.OrganizationJob.JobNo : null,
                EmployeeOrganizationName = employeeCode.EmployeeCurrentJob != null ? employeeCode.EmployeeCurrentJob.OrganizationJob.OrganizationStructure.OrganizationName : null,//"الجهة",
                EmployeeCurrentQualification = employeeCode.EmployeeCurrentQualification,
                EmployeeCurrentOrganization = new AssigningsController().GetOrganizationName(employeeCode.EmployeeCurrentJob != null ? employeeCode.EmployeeCurrentJob.EmployeeCareerHistoryID : 0),
                EmployeeBirthDate = employeeCode.Employee.EmployeeBirthDate != null ? employeeCode.Employee.EmployeeBirthDate : (DateTime?)null,
                CurrentJobJoinDate = employeeCode.EmployeeCurrentJob != null ? employeeCode.EmployeeCurrentJob.JoinDate : (DateTime?)null,
                HiringDate = employeeCode.HiringRecord != null ? employeeCode.HiringRecord.JoinDate : (DateTime?)null,
            };
        }
    }

    public static class DelegationsViewModelExtension
    {
        public static DelegationsViewModel GetDelegationDetail(this DelegationsDetailsViewModel DelagationVM)
        {
            DelegationsDetailsBLL DelegationDetailBLL = new DelegationsDetailsBLL().GetDelegationsDetailsByDelegationDetailID(DelagationVM.DelegationDetailID.Value);
            return new DelegationsViewModel()
            {
                DelegationStartDate = DelegationDetailBLL.Delegation.DelegationStartDate,
                DelegationEndDate = DelegationDetailBLL.Delegation.DelegationEndDate,
                DelegationKind = DelegationDetailBLL.Delegation.DelegationKind,
                DelegationType = DelegationDetailBLL.Delegation.DelegationType,
                DelegationDestination = DelegationDetailBLL.Delegation.DelegationDestination,
                DelegationReason = DelegationDetailBLL.Delegation.DelegationReason,
                DelegationDetailRequest = DelegationDetailBLL,
            };
        }
    }

    public static class InternshipScholarshipsViewModelExtension
    {
        public static InternshipScholarshipsViewModel GetInternshipScholarshipDetail(this InternshipScholarshipsDetailsViewModel InternshipScholarshipVM)
        {
            InternshipScholarshipsDetailsBLL InternshipScholarshipDetailBLL = new InternshipScholarshipsDetailsBLL().GetInternshipScholarshipsDetailsByInternshipScholarshipDetailID(InternshipScholarshipVM.InternshipScholarshipDetailID.Value);
            return new InternshipScholarshipsViewModel()
            {
                InternshipScholarshipStartDate = InternshipScholarshipDetailBLL.InternshipScholarship.InternshipScholarshipStartDate,
                InternshipScholarshipEndDate = InternshipScholarshipDetailBLL.InternshipScholarship.InternshipScholarshipEndDate,
                InternshipScholarshipType = InternshipScholarshipDetailBLL.InternshipScholarship.InternshipScholarshipType,
                InternshipScholarshipLocation = InternshipScholarshipDetailBLL.InternshipScholarship.InternshipScholarshipLocation,
                InternshipScholarshipReason = InternshipScholarshipDetailBLL.InternshipScholarship.InternshipScholarshipReason,
                InternshipScholarshipDetailRequest = InternshipScholarshipDetailBLL,
            };
        }
    }
}