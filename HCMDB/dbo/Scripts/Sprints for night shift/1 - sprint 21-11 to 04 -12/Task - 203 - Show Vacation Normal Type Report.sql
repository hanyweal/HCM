GO
/****** Object:  StoredProcedure [dbo].[spGetVacationsByVacationTypeID]    Script Date: 22/04/42 07:06:00 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spGetVacationsByVacationTypeID] 
(
	@VacationTypeID int,
	@StartDate date,
	@EndDate date
)
AS
BEGIN

	IF @VacationTypeID = 0 
		SET @VacationTypeID = null

	SELECT Vacations.VacationID,
			VacationsDetails.IsApproved,
			--VacationsTypes.VacationTypeName,
			VacationTypeName = VacationsTypes.VacationTypeName + '' + CASE WHEN LEN(NormalVacationsTypes.NormalVacationTypeName) > 0 THEN ' - ' + NormalVacationsTypes.NormalVacationTypeName ELSE '' END,			
			VacationsActions.VacationActionName,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.FromDate) AS VacationStartDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.ToDate) AS VacationEndDateUmAlQura,
			--mic_db.dbo.fn_GregToUmAlqura(DATEADD(DAY,1,Vacations.VacationEndDate)) AS WorkDateUmAlQura,
			dbo.fnGetPeriodBetweenTwoDates(VacationsDetails.FromDate, VacationsDetails.ToDate) AS VacationPeriod,
			vwEmployeesCodes.EmployeeCodeNo, vwEmployeesCodes.EmployeeIDNo, vwEmployeesCodes.EmployeeNameAr, 
			EmployeesCareersHistory.OrganizationCode, EmployeesCareersHistory.OrganizationName,
			EmployeesCareersHistory.JobName,
			mic_db.dbo.fn_GregToUmAlqura(@StartDate) AS StartDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(@EndDate) AS EndDateUmAlQura
	FROM Vacations
	INNER JOIN VacationsTypes ON VacationsTypes.VacationTypeID = Vacations.VacationTypeID
	INNER JOIN VacationsDetails ON Vacations.VacationID = VacationsDetails.VacationID
	INNER JOIN VacationsActions ON VacationsDetails.VacationActionID = VacationsActions.VacationActionID
	INNER JOIN vwEmployeesCareersHistory as EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = Vacations.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID = EmployeesCareersHistory.EmployeeCodeID
	LEFT OUTER JOIN NormalVacationsTypes ON NormalVacationsTypes.NormalVacationTypeID= Vacations.NormalVacationTypeID
	WHERE VacationsTypes.VacationTypeID = ISNULL(@VacationTypeID, VacationsTypes.VacationTypeID)
	AND (@StartDate BETWEEN Vacations.VacationStartDate AND Vacations.VacationEndDate
		 OR @EndDate BETWEEN Vacations.VacationStartDate AND Vacations.VacationEndDate)
	ORDER BY Vacations.VacationID
	
END
 

GO
/****** Object:  StoredProcedure [dbo].[spGetVacationsByEmployeeCodeID]    Script Date: 22/04/42 07:03:39 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spGetVacationsByEmployeeCodeID]-- 22915
(
	@EmployeeCodeID int
)
AS
BEGIN

	--SELECT Vacations.*,
	--		VacationsTypes.VacationTypeName,
	--		mic_db.dbo.fn_GregToUmAlqura(Vacations.VacationStartDate) AS VacationStartDateUmAlQura,
	--		mic_db.dbo.fn_GregToUmAlqura(Vacations.VacationEndDate) AS VacationEndDateUmAlQura,
	--		mic_db.dbo.fn_GregToUmAlqura(DATEADD(DAY,1,Vacations.VacationEndDate)) AS WorkDateUmAlQura,
	--		dbo.fnGetPeriodBetweenTwoDates(Vacations.VacationStartDate , Vacations.VacationEndDate) AS VacationPeriod
	--FROM Vacations
	--INNER JOIN VacationsTypes ON VacationsTypes.VacationTypeID = Vacations.VacationTypeID
	--INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = Vacations.EmployeeCareerHistoryID
	--INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID = EmployeesCareersHistory.EmployeeCodeID
	--WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	--ORDER BY Vacations.VacationStartDate
	
	SELECT Vacations.VacationID,
			VacationsDetails.IsApproved,
			--VacationsTypes.VacationTypeName,
			VacationTypeName = VacationsTypes.VacationTypeName + '' + CASE WHEN LEN(NormalVacationsTypes.NormalVacationTypeName) > 0 THEN ' - ' + NormalVacationsTypes.NormalVacationTypeName ELSE '' END,			
			VacationsActions.VacationActionName,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.FromDate) AS VacationStartDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.ToDate) AS VacationEndDateUmAlQura,
			--mic_db.dbo.fn_GregToUmAlqura(DATEADD(DAY,1,Vacations.VacationEndDate)) AS WorkDateUmAlQura,
			dbo.fnGetPeriodBetweenTwoDates(VacationsDetails.FromDate ,VacationsDetails.ToDate) AS VacationPeriod
	FROM Vacations
	INNER JOIN VacationsTypes ON VacationsTypes.VacationTypeID = Vacations.VacationTypeID
	INNER JOIN VacationsDetails ON Vacations.VacationID = VacationsDetails.VacationID
	INNER JOIN VacationsActions ON VacationsDetails.VacationActionID = VacationsActions.VacationActionID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = Vacations.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID = EmployeesCareersHistory.EmployeeCodeID
	LEFT OUTER JOIN NormalVacationsTypes ON NormalVacationsTypes.NormalVacationTypeID= Vacations.NormalVacationTypeID
	WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	ORDER BY Vacations.VacationID
	
END


GO
/****** Object:  StoredProcedure [dbo].[spGetVacationByVacationDetailID]    Script Date: 22/04/42 06:58:37 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetVacationByVacationDetailID] --305091
(
	@VacationDetailID INT = 2084
)
AS
BEGIN
DECLARE @VacationID INT
SELECT @VacationID=VD.VacationID FROM VacationsDetails VD WHERE VD.VacationDetailID=@VacationDetailID

SELECT
			VacationsDetails.VacationDetailID as VacationDetailID,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.FromDate) AS FromDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.ToDate) AS ToDateUmAlQura,
			VacationsDetails.FromDate,
			VacationsDetails.ToDate,
			mic_db.dbo.fn_GregToUmAlqura(DATEADD(DAY,1,VacationsDetails.ToDate)) AS WorkDateUmAlQura,
			DATEADD(DAY,1,VacationsDetails.ToDate) AS WorkDate,
			VacationsDetails.MainframeNo,
			VacationsDetails.Notes,
			dbo.fnGetPeriodBetweenTwoDates(VacationsDetails.FromDate,VacationsDetails.ToDate) AS VacationPeriod,
			VacationsActions.VacationActionID,
			VacationsActions.VacationActionName,
			VacationsTypes.VacationTypeID,
			--VacationsTypes.VacationTypeName,
			VacationTypeName = VacationsTypes.VacationTypeName + '' + CASE WHEN LEN(NormalVacationsTypes.NormalVacationTypeName) > 0 THEN ' - ' + NormalVacationsTypes.NormalVacationTypeName ELSE '' END,
			vwEmployeesCodes.EmployeeCodeNo, 
			vwEmployeesCodes.EmployeeCodeID,
			vwEmployeesCodes.EmployeeIDNo, 
			vwEmployeesCodes.EmployeeNameAr,
			jobs.JobName,
			OrganizationsStructures.OrganizationName,
			Ranks.RankName,
			--VacationsTypes.VacationBalance,
			dbo.[fnGetPreviousConsumedVacationByEmployeeCodeID](Vacations.VacationID,vwEmployeesCodes.EmployeeCodeID,Vacations.VacationTypeID) AS PreviousConsumedVacation,
			 vwEmployeesCodes.NationalityID as NationalityID,
			NormalVacationsTypes.NormalVacationTypeID,
			NormalVacationsTypes.NormalVacationTypeName,
		PR.*,
		OrganizationsJobs.JobNo,
		Cast(vacations.CreatedDate as date) as createdDate,
		mic_db.dbo.fn_GregToUmAlqura(Cast(vacations.CreatedDate as date)) as CreatedDateUmAlqura
		
			--DATEPART(YEAR,Delegations.DelegationStartDate)) AS PreviousConsumedDelegation,
	FROM Vacations
	INNER JOIN VacationsTypes ON Vacations.VacationTypeID = VacationsTypes.VacationTypeID
	INNER JOIN VacationsDetails ON Vacations.VacationID = VacationsDetails.VacationID 
	INNER JOIN VacationsActions ON VacationsDetails.VacationActionID = VacationsActions.VacationActionID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = Vacations.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON EmployeesCareersHistory.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
	INNER JOIN OrganizationsJobs ON OrganizationsJobs.OrganizationJobID = EmployeesCareersHistory.OrganizationJobID
	INNER JOIN Jobs ON Jobs.JobID = OrganizationsJobs.JobID
	INNER JOIN OrganizationsStructures ON OrganizationsJobs.OrganizationID = OrganizationsStructures.OrganizationID
	INNER JOIN Ranks ON Ranks.RankID = OrganizationsJobs.RankID
	LEFT OUTER JOIN NormalVacationsTypes ON NormalVacationsTypes.NormalVacationTypeID= Vacations.NormalVacationTypeID
	LEFT OUTER JOIN Countries ON Countries.CountryID=vwEmployeesCodes.NationalityID
	INNER JOIN  (SELECT TOP 1 
	VD.VacationID,
	VD.VacationDetailID AS VacationIDCreationRecord ,
	mic_db.dbo.fn_GregToUmAlqura(VD.FromDate) AS FromDateUmAlQuraCreationRecord,
	mic_db.dbo.fn_GregToUmAlqura(VD.ToDate) AS ToDateUmAlQuraCreationRecord,
	VD.FromDate AS FromDateCreationRecord,
	VD.ToDate AS ToDateCreationRecord,
	mic_db.dbo.fn_GregToUmAlqura(DATEADD(DAY,1,VD.ToDate)) AS WorkDateUmAlQuraCreationRecord,
	DATEADD(DAY,1,VD.ToDate) AS WorkDateCreationRecord,
	VD.MainframeNo AS MainframeNoCreationRecord,
	VD.Notes AS NotesCreationRecord,
	dbo.fnGetPeriodBetweenTwoDates(VD.FromDate,VD.ToDate) AS VacationPeriodCreationRecord,
	VA.VacationActionID AS VacationActionIDCreationRecord,
	VA.VacationActionName AS VacationActionNameCreationRecord
	FROM VacationsDetails VD 
		INNER JOIN VacationsActions VA ON VD.VacationActionID = VA.VacationActionID
	WHERE VD.VacationID=@VacationID
		AND (VD.VacationDetailID < @VacationDetailID OR vd.VacationActionID=1)
	ORDER BY VD.VacationDetailID DESC) PR ON PR.VacationID=VacationsDetails.VacationID
	WHERE VacationsDetails.VacationDetailID = @VacationDetailID
END


