namespace HCMBLL.Enums
{
    public enum PromotionsPeriodsValidationEnum
    {
        Done = 1,
        RejectedBecauseOfPromotionStartDateIsGreaterThenPromotionEndDate = 2,
        RejectedBecauseOfAlreadyOnePromotionPeriodIsActive = 3,
        RejectedBecauseOfPromotionRecordExistWithThisPromotiosPeriodDates = 4
    }
}
