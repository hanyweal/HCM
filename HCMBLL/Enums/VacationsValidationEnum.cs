namespace HCMBLL.Enums
{
    public enum VacationsValidationEnum
    {
        Done = 1,
        RejectedBeacuseOfPreviousNotApproved = 2,
        RejectedBecauseOfAlreadyCanceled = 3,
        RejectedBecauseOfNoBalance = 4,
        RejectedBecauseOfBeforeHiringDate = 5,
        RejectedBecauseOfDuringProbation = 6,
        RejectedBeacuseOfAlreadyApproved = 7,
        RejectedBecauseOfConsumedMaxLimit = 8,
        RejectedBecauseOfInvalidDates = 9,
        RejectedBeacuseOfNoChanceCancelCancelling = 10,
        RejectedBecauseOfNormalVacationBalanceExists = 11,
        RejectedBecauseOfNormalVacationReachedToMaxLimitOfSeparatedDays = 12,
        RejectedBecauseOfVacationNotAllowedBetween1437and1438 = 13,
        SportsSeasonDoesNotExist = 14,
        RejectedBecauseOfInvalidSportsDates = 15,
        RejectedBecauseOfVacationOutOfRange = 16,
        RejectedBecauseOfInvalidStartDate = 17,
        RejectedBecauseOfNewWorkDateGreaterThanPreviosWorkDate = 18,
        RejectedBecauseSickExceptionalVacationDatesOutOfRange = 19,
        RejectedBecauseOfErrorInTimeAttendanceApp = 20,
        RejectedBecauseOfMarriageVacationAcceptedOneTime = 21,
        RejectedBecauseOfEVacationRequestPendingExist = 22,
        RejectedBecauseOfEVacationRequestStatusNotPending = 23,
        RejectedBeacuseOfApproverIsNotAuthorizedPerson = 24,
        RejectedBeacuseOfEmployeeNotHasActualOrganization = 25,
        RejectedBeacuseOfNotAllowedWeekEndBetweenTwoVacations = 26,
        RejectedBecauseOfAlreadyBreak = 27,
        RejectedBecauseOfAlreadyExtend = 28,
        RejectedBecauseOfNoActiveProxyCreatedToOtherPerson = 29,
        RejectedBecauseOfEmployeeHasProxyByOtherPerson = 30
    }
}