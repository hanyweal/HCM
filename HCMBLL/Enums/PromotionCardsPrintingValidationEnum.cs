namespace HCMBLL.Enums
{
    public enum PromotionCardsPrintingValidationEnum
    {
        Done = 1,
        RejectedBecauseOfEmployeeHaveRecordWithSamePeriod = 2,
        RejectedBecauseOfThereIsNoActivePromotionPeriod= 3,
    }
}