GO
ALTER PROCEDURE [dbo].[spGetExperienceByEmployeeCodeID]22915
(
	@EmployeeCodeID int
)
AS
BEGIN
	select dbo.fnGetYearsFromTotalDays(DATEDIFF(DAY,FromDate,ToDate)  + 1) TotalYears,
			dbo.fnGetModMonthsFromTotalDays(DATEDIFF(DAY,FromDate,ToDate)  + 1) TotalMonths,
			dbo.fnGetModDaysFromTotalDays(DATEDIFF(DAY,FromDate,ToDate)  + 1) + 1 TotalDays 
   FROM [HCM].[dbo].[EmployeeExperiencesWithDetails]
	WHERE EmployeeCodeID = @EmployeeCodeID
END