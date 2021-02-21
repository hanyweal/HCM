Alter table EVacationRequestsStatus drop column IsForAuthorizedPerson
GO

ALTER PROCEDURE [dbo].[spGetEmployeeByEmployeeCodeID]
(
	@EmployeeCodeID int
)
AS
BEGIN
	SELECT *
	FROM vwCurrentEmployeesCareer
	WHERE EmployeeCodeID = @EmployeeCodeID
END
GO