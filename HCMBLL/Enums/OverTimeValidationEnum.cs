namespace HCMBLL.Enums
{
    public enum OverTimeValidationEnum
    {
        Done = 1,
        RejectedBecauseOfEmployeeStatus = 2,
        RejectedBecauseEmployeeRequired = 3,
        //RejectedBecauseOfConflictWithOverTime = 4,
        //RejectedBecauseOfConflictWithDelegation = 5,
        RejectedBecauseAlreadyExist = 6,
        RejectedBecauseOfOverTimeDatesMustBeInSameFinancialYear = 7,
        //RejectedBecauseOfConflictWithInternshipScholarship = 8,
        RejectedBecauseOfAlreadyApprove = 9,
        RejectedBecauseOfAlreadyApproveCancel = 10
    }
}
