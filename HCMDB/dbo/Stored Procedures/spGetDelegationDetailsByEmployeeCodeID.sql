-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetDelegationDetailsByEmployeeCodeID]-- 22915
(
	@EmployeeCodeID int
)
AS
BEGIN

	SELECT Delegations.*,
			vwEmployeesCodes.EmployeeCodeNo, 
			vwEmployeesCodes.EmployeeCodeID,
			vwEmployeesCodes.EmployeeIDNo, 
			vwEmployeesCodes.EmployeeNameAr,
			DelegationsKinds.DelegationKindName,
			DelegationsTypes.DelegationTypeName,
			dbo.fnGetPeriodBetweenTwoDates(Delegations.DelegationStartDate , Delegations.DelegationEndDate) AS DelegationPeriod,
			dbo.[fnGetPreviousConsumedDelegationByEmployeeCodeID](Delegations.DelegationID,vwEmployeesCodes.EmployeeCodeID,YEAR(Delegations.DelegationStartDate)) AS PreviousConsumedDelegation
	FROM Delegations
	INNER JOIN DelegationsDetails ON Delegations.DelegationID = DelegationsDetails.DelegationID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = DelegationsDetails.EmployeeCareerHistoryID
	INNER JOIN DelegationsKinds ON Delegations.DelegationKindID = DelegationsKinds.DelegationKindID
	INNER JOIN DelegationsTypes ON Delegations.DelegationTypeID = DelegationsTypes.DelegationTypeID
	INNER JOIN vwEmployeesCodes ON EmployeesCareersHistory.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
	WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	ORDER BY DelegationStartDate
	
END
