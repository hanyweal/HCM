namespace HCMBLL.Enums
{
    public enum OrganizationJobValidationEnum
    {
        Done = 1,
        RejectedBecauseOfExistsInPromotionsRecordsJobsVacancies = 2,
        RejectedBecauseOfExistsActiveJobWithJobNoAndRankID = 3,
        RejectedBecauseThereIsNoActiveOrganizationJob = 4,
        RejectedBecauseThereIsNoEmployeeCareerHistoryRelatedToThisOrganizationJob = 5,
        RejectedBecauseThisOrganizationJobIsNotVacant= 6,
        RejectedBecauseInPushingUpNextRankShouldBiggerThanCurrent = 7,
        RejectedBecauseInPushingUpNextRankShouldLessThanCurrent = 8,
        RejectedBecauseOperationDateLessThanOtherExists = 9,

    }
}
