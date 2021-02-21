-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetOverTimeDetailsByEmployeeCodeID] --24870
(
	@EmployeeCodeID int
)
AS
BEGIN

	SELECT OverTimes.*,
			vwEmployeesCodes.EmployeeCodeNo, 
			vwEmployeesCodes.EmployeeCodeID,
			vwEmployeesCodes.EmployeeIDNo, 
			vwEmployeesCodes.EmployeeNameAr,
			dbo.fnGetPeriodBetweenTwoDates(OverTimes.OverTimeStartDate , OverTimes.OverTimeEndDate) AS OverTimePeriod
	FROM OverTimes
	INNER JOIN OverTimesDetails ON OverTimes.OverTimeID = OverTimesDetails.OverTimeID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = OverTimesDetails.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON EmployeesCareersHistory.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
	WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	
END
