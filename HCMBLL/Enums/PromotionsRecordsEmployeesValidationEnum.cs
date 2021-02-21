namespace HCMBLL.Enums
{
    public enum PromotionsRecordsEmployeesValidationEnum
    {
        Done = 1,
        RejectedBecauseOfCandidateRankNotDeserveToPromote = 2,
        RejectedBecauseOfCandidateJobPeriodNotCompleted = 3,
        RejectedBecauseOfCandidateAndReasonAreRequiredToAdding = 4,
        RejectedBecauseOfIsDeserveExtraBonusNotSpecifiedValue = 5,
        RejectedBecauseOfIsDeserveExtraBonusUpdateWithSameValue = 6,
        RejectedBecauseOfApproved = 7,
        RejectedBecauseOfCandidateHiringRecordNotExists = 8,
        RejectedBecauseOfCandidateAlreadyInPromotionRecordNotInstalled = 9,
        RejectedBecauseOfCandidateAlreadyReservedJobVacancy = 10,
        RejectedBecauseOfLastCandidateEvaluationWeak = 11
    }
}