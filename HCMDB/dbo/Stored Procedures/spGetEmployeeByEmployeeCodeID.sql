-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetEmployeeByEmployeeCodeID]
(
	@EmployeeCodeID int
)
AS
BEGIN
	SELECT *
	FROM vwEmployeesCodes
	WHERE EmployeeCodeID = @EmployeeCodeID
END
