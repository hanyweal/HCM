CREATE PROCEDURE [dbo].[spGetExperienceByEmployeeCodeID]
(
	@EmployeeCodeID int
)
AS
BEGIN
	SELECT TotalYears,TotalMonths,TotalDays 
	FROM EmployeesExperiences
	WHERE EmployeeCodeID = @EmployeeCodeID
END
