namespace HCMBLL.Enums
{
    public enum EmployeesEvaluationsValidationEnum
    {
        Done = 1,
        RejectedBecauseOfDirectManagerEvaluationIsNotBetweenZeroAndFifty = 2,
        RejectedBecauseOfTimeAttendanceEvaluationIsNotBetweenZeroAndThirtyFive = 3,
        RejectedBecauseOfViolationsEvaluationIsNotBetweenZeroAndFifteen = 4,
        RejectedBecauseOfEvaluationQuarterAlreadyExistsInCurrentYear = 5
    }
}


