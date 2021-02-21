namespace HCMBLL.Enums
{
    public enum DelegationsValidationEnum
    {
        Done = 1,
        RejectedBecauseOfMaxLimit = 2,
        RejectedBecauseEmployeeRequired = 3,
        //RejectedBecauseOfConflictWithOverTime = 4,
        //RejectedBecauseOfConflictWithDelegation = 5,
        RejectedBecauseAlreadyExist = 6,
        RejectedBecauseOfDelegationDatesMustBeInSameFinancialYear = 7,
        //RejectedBecauseOfConflictWithInternshipScholarship = 8,
        RejectedBecauseOfAlreadyApprove = 9,
        RejectedBecauseOfAlreadyApproveCancel = 10
    }
}
