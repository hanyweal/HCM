namespace HCMBLL.Enums
{
    public enum CareersHistoryValidationEnum
    {
        Done = 1,
        RejectedBecauseOfAlreadyHiringBefore = 2,
        RejectedHiringDateUpdatingBecauseOfAlreadyActionsBefore = 3,
        RejectedBecauseOfCareerDegreeOutOfRange = 4,
        RejectedBecauseOfExistsInPromotionsRecordsEmployees = 5,
        RejectedBecauseOfJoinDateMustBeLessThanHiringRecordJoinDate = 6,
    }
}
