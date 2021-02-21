namespace HCMBLL.Enums
{
    public enum HolidaySettingValidationEnum
    {
        Done = 1,
        RejectedBecauseOfConflictWithHolidaySetting,
        RejectedBecauseOfHolidaysDurationShouldBeInTheSameHijriYear,
        RejectedBecauseOfEmployeeAssignToThisHolidaySetting
    }
}
