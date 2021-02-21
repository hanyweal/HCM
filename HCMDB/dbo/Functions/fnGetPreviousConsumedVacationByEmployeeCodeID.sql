-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetPreviousConsumedVacationByEmployeeCodeID]
(
	 @VacationID int,
	 @EmployeeCodeID int,
	 @VacationTypeID int
)
RETURNS INT
AS
BEGIN
	DECLARE @Result INT
	SELECT @Result = ISNULL(SUM(DBO.fnGetPeriodBetweenTwoDates(Vacations.VacationStartDate,Vacations.VacationEndDate)),0)
	FROM Vacations
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = Vacations.EmployeeCareerHistoryID
	WHERE EmployeesCareersHistory.EmployeeCodeID = @EmployeeCodeID
	AND Vacations.VacationTypeID = @VacationTypeID
	AND Vacations.VacationID != @VacationID
	RETURN @Result
END
