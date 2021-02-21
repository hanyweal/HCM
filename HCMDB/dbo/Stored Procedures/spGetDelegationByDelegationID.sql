
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetDelegationByDelegationID]
(
	@DelegationID int
)
AS
BEGIN

	SELECT Delegations.DelegationID,
		 Delegations.DelegationDistancePeriod,
		 Delegations.DelegationKindID, 
		 Delegations.DelegationTypeID, 
		 Delegations.DelegationStartDate, 
		 mic_db.dbo.fn_GregToUmAlqura(Delegations.DelegationStartDate) AS DelegationStartDateUmAlQura,
		 Delegations.DelegationEndDate, 
		 mic_db.dbo.fn_GregToUmAlqura(Delegations.DelegationEndDate) AS DelegationEndDateUmAlQura,
		 Delegations.DelegationReason,
		 Delegations.Notes,
		 Delegations.CountryID, 
		 Delegations.KSACityID, 
		 Delegations.IsActive, 
		 Delegations.CreatedDate, 
		 Delegations.LastUpdatedDate, 
		 DelegationsTypes.DelegationTypeName, 
		 DelegationsKinds.DelegationKindName, 
		 Countries.CountryName, 
		 KSARegions.KSARegionName, 
		 KSACities.KSACityName, 
		 dbo.fnGetPeriodBetweenTwoDates(Delegations.DelegationStartDate,Delegations.DelegationEndDate) AS DelegationPeriod
	FROM KSARegions 
		INNER JOIN KSACities ON KSARegions.KSARegionID = KSACities.KSARegionID 
		RIGHT OUTER JOIN Delegations 
		INNER JOIN DelegationsKinds ON Delegations.DelegationKindID = DelegationsKinds.DelegationKindID 
		INNER JOIN DelegationsTypes ON Delegations.DelegationTypeID = DelegationsTypes.DelegationTypeID ON KSACities.KSACityID = Delegations.KSACityID 
		LEFT OUTER JOIN Countries ON Delegations.CountryID = Countries.CountryID
	WHERE DelegationID = @DelegationID
	
END
