namespace HCMBLL.Enums
{
    public enum LendersValidationEnum
    {
        Done = 1,
        RejectedBecauseOfTotalPeriod = 2,
        RejectedBecauseOfConflictWithAssigning = 3,
        RejectedBecauseAlreadyFinished = 4,
        RejectedBecauseEndReasonRequired = 5,
    }
}
