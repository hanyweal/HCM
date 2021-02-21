-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetDelegationDetailsByDelegationID] --93
(
	@DelegationID int
)
AS
BEGIN

	SELECT Delegations.DelegationID, 
			vwEmployeesCodes.EmployeeCodeNo, 
			vwEmployeesCodes.EmployeeCodeID,
			vwEmployeesCodes.EmployeeIDNo, 
			vwEmployeesCodes.EmployeeNameAr,
			dbo.[fnGetPreviousConsumedDelegationByEmployeeCodeID](Delegations.DelegationID,vwEmployeesCodes.EmployeeCodeID,
			DATEPART(YEAR,Delegations.DelegationStartDate)) AS PreviousConsumedDelegation,
			ranks.RankName
	FROM Delegations
	INNER JOIN DelegationsDetails ON Delegations.DelegationID = DelegationsDetails.DelegationID 
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = DelegationsDetails.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON EmployeesCareersHistory.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
	INNER JOIN OrganizationsJobs ON OrganizationsJobs.OrganizationJobID = EmployeesCareersHistory.OrganizationJobID
	INNER JOIN Ranks ON Ranks.RankID = OrganizationsJobs.RankID
	WHERE Delegations.DelegationID = @DelegationID
	ORDER BY 	Ranks.RankName DESC
END
