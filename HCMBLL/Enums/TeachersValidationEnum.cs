namespace HCMBLL.Enums
{
    public enum TeachersValidationEnum
    {
        Done = 1,
        Rejected = 2,
        RejectedBecauseOfEndDateShouldNotBeLessThanStartDate = 3,
        RejectedBecauseOfBeforeHiringDate = 4
    }
}
