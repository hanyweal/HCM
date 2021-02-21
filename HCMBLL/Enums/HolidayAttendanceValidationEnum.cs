namespace HCMBLL.Enums
{
    public enum HolidayAttendanceValidationEnum
    {
        Done = 1,
        RejectedBecauseOfEmployeeStatus = 2,
        RejectedBecauseEmployeeRequired = 3,
        RejectedBecauseAlreadyExist = 4,
        RejectedBecauseAlreadyHasVacation = 5,
        //If employee has already holiday attendance on the same setting
        RejectedBecauseEmployeeAlreadyExistOnAnotherRecord = 6,
    }
}
