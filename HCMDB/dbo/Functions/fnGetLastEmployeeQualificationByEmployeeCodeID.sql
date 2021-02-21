-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetLastEmployeeQualificationByEmployeeCodeID]
(
	@EmployeeCodeID int
)

RETURNS TABLE 
AS
RETURN 
(
	SELECT TOP(1) *
	FROM vwEmployeesQualifications
	WHERE EmployeeCodeID = @EmployeeCodeID
	ORDER BY [WEIGHT] DESC
	, GraduationDate DESC
	, CONVERT(INT, GraduationYear) DESC
)
