namespace HCMBLL.Enums
{
    public enum ScholarshipsValidationEnum
    {
        Done = 1,
        RejectedBecauseOfAlreadyCanceled = 2,
        RejectedBecauseOfBeforeHiringDate = 3,
        RejectedBecauseOfDuringProbation = 4,
        RejectedBecauseOfInvalidDates = 5,
        RejectedBeacuseOfNoChanceCancelCancelling = 6,
        RejectedBecauseOfStatus = 7,
        RejectedBecauseOfActivePreviousAssigning = 8,
        RejectedBecauseOfAlreadyJoinedBefore = 9,
        RejectedBecauseOfScholarshipJoinDateBeforeScholarshipStartDate = 10,
        RejectedBecauseOfAlreadyPassed = 11
    }
}