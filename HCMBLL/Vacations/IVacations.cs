using System.Collections.Generic;

namespace HCMBLL
{
    public interface IVacations : IEntity
    {
        //EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        //DateTime VacationStartDate { get; set; }

        //DateTime VacationEndDate { get; set; }

        //int VacationPeriod { get; set; }

        //DateTime WorkDate { get; set; }

        //bool IsApproved { get; set; }

        //int? ApprovedBy { get; set; }

        //DateTime? ApprovedDate { get; set; }

        //VacationsTypesEnum VacationType { get; }

        Result Add();

        Result Cancel();

        Result Extend();

        Result Break();

        Result Approve();

        Result ApproveCancel();

        List<BaseVacationsBLL> GetByEmployeeCodeVacationType(int EmployeeCodeID);
    }
}

