CREATE   PROCEDURE [dbo].[spGetVacationByVacationDetailID] --256648
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
			VacationsTypes.VacationTypeName,
			vwEmployeesCodes.EmployeeCodeNo, 
			vwEmployeesCodes.EmployeeCodeID,
			vwEmployeesCodes.EmployeeIDNo, 
			vwEmployeesCodes.EmployeeNameAr,
			jobs.JobName,
			OrganizationsStructures.OrganizationName,
			Ranks.RankName,
			--VacationsTypes.VacationBalance,
			(SELECT rcvb.VacationBalance FROM RanksCategoriesVacationsBalances rcvb WHERE rcvb.VacationTypeID=Vacations.VacationTypeID AND rcvb.RankCategoryID=Ranks.RankCategoryID) AS VacationBalance,
			dbo.[fnGetPreviousConsumedVacationByEmployeeCodeID](Vacations.VacationID,vwEmployeesCodes.EmployeeCodeID,Vacations.VacationTypeID) AS PreviousConsumedVacation,
			 vwEmployeesCodes.NationalityID as NationalityID,
			NormalVacationsTypes.NormalVacationTypeID,
			NormalVacationsTypes.NormalVacationTypeName,
		PR.*,
		OrganizationsJobs.JobNo,
		Cast(vacations.CreatedDate as date) as CreatedDate,
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

