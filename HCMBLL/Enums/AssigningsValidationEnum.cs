namespace HCMBLL.Enums
{
    public enum AssigningsValidationEnum
    {
        Done = 1,
        RejectedBecauseOfStatus = 2,
        RejectedBecauseOfActivePreviousAssigning = 3,
        RejectedBecauseOfConflictWithLenders = 4,
        RejectedBecauseOfEndDateIsLessThanCreationDate = 5,
        RejectedBecauseAssigningNotFound = 6,
        RejectedBecauseOfAlreadyFinished = 7
    }
}
