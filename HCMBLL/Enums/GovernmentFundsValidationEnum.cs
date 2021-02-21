namespace HCMBLL.Enums
{
    public enum GovernmentFundsValidationEnum
    {
        Done = 1,
        RejectedBecauseOfMonthlyDeductionAmountShouldNotGreaterThanTotalDeductionAmount = 2,
        RejectedBecauseOfBeforeHiringDate = 3,
        RejectedBecauseOfAlreadyDeactivated = 4,
        RejectedBecauseOfDeactivationDateShouldNotBeLessThanDeductionStartDate = 5
    }
}
