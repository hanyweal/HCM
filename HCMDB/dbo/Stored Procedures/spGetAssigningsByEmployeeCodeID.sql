-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetAssigningsByEmployeeCodeID] --93
(
	@EmployeeCodeID int
)
AS
BEGIN

	SELECT Assignings.*,
			AssigningsTypes.AssigningTypeName,
			Jobs.JobName,
			OrganizationsStructures.OrganizationName,
			KSACities.KSACityName
	FROM Assignings
	INNER JOIN AssigningsTypes ON Assignings.AssigningTypID = AssigningsTypes.AssigningTypID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = Assignings.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON EmployeesCareersHistory.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
	LEFT JOIN Jobs ON Assignings.JobID = Jobs.JobID
	LEFT JOIN OrganizationsStructures ON OrganizationsStructures.OrganizationID = Assignings.OrganizationID
	LEFT JOIN KSACities ON KSACities.KSACityID = Assignings.ExternalKSACityID
	WHERE EmployeesCareersHistory.EmployeeCodeID = @EmployeeCodeID
	
END
