CREATE PROC [dbo].[spGetOverTimesDetailsByOverTimeID]
(
	 @OverTimeID INT
)
AS
SELECT OverTimesDetails.OverTimeID, 
	   vwEmployeesCodes.EmployeeCodeNo, 
	   vwEmployeesCodes.EmployeeIDNo, 
	   vwEmployeesCodes.EmployeeNameAr
FROM OverTimesDetails
INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = OverTimesDetails.EmployeeCareerHistoryID
INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID = EmployeesCareersHistory.EmployeeCodeID
WHERE OverTimesDetails.OverTimeID = @OverTimeID
