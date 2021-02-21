namespace HCMBLL.Enums
{
    public enum TransfersValidationEnum
    {
        Done = 1,
        Rejected = 2,
        RejectedBecauseOfTransferDateIsLessThanHiringDateDate = 3,
        RejectedBecauseOfAlreadyProcessed = 4
    }
}
