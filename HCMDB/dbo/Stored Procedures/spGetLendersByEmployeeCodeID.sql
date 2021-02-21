-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetLendersByEmployeeCodeID] --93
(
	@EmployeeCodeID int
)
AS
BEGIN

	SELECT Lenders.*,
			KSACities.KSACityName
	FROM Lenders
	INNER JOIN vwEmployeesCodes ON Lenders.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
	LEFT JOIN KSACities ON Lenders.KSACityID = KSACities.KSACityID
	WHERE Lenders.EmployeeCodeID = @EmployeeCodeID
	
END

 