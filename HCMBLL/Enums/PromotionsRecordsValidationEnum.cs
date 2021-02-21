namespace HCMBLL.Enums
{
    public enum PromotionsRecordsValidationEnum
    {
        Done = 1,
        RejectedBecauseOfNoJobsVacanciesAvailable = 2,
        RejectedBecauseOfNoCandidatesEligible = 3,
        RejectedBecauseOfNoPromotionRecordSelectedToDelete = 4,
        RejectedBecauseOfPromotionIsNotOpen = 5,
        RejectedBecauseOfSomeQualifitionPointsAreNull = 6,
        RejectedBecausePromotionRecordStatusMustBeOpen = 7,
        RejectedBecausePromotionRecordStatusMustBePreferenced = 8,
        RejectedBecausePromotionRecordStatusMustBeInstalled = 9,
        RejectedBecausePromotionRecordStatusMustBeOpenOrPreferenced = 10,
        RejectedBecauseThereArePormotionRecordsHasRelationNotInstalled = 11,
        RejectedBecauseOfAssignedJobCategoryIsInInstalledStatus = 12,
        RejectedBecauseThereArePormotionRecordsNotInstalled = 13,
        RejectedBecauseOfNewEmployeeCodeNoNotExitsInPromotionRecordEmployeesOrNotPromoted = 14,
        RejectedBecauseThereArePormotionRecordsNotClosedInOtherRanks = 15,
        RejectedBecausePromotionDateShouldBeLessThanPromotionRecordDate = 16,
        RejectedBecausePromotionRecordStatusMustBePreferencedOrInstalled = 17,
        RejectedBecauseOfOneOfEmployeeInRedistributeJobIsApproved = 18,
        RejectedBecausePromotionDecisionMustBeShouldBePromotedByExam = 19
    }
}