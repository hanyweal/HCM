
CREATE   PROCEDURE [dbo].[spGetVacationCreationRecordByVacationID]
(
	@VacationID INT
)
AS
BEGIN
	SELECT  Vacations.VacationID AS VacationIDCreationRecord ,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.FromDate) AS FromDateUmAlQuraCreationRecord,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.ToDate) AS ToDateUmAlQuraCreationRecord,
			VacationsDetails.FromDate AS FromDateCreationRecord,
			VacationsDetails.ToDate AS ToDateCreationRecord,
			mic_db.dbo.fn_GregToUmAlqura(DATEADD(DAY,1,VacationsDetails.ToDate)) AS WorkDateUmAlQuraCreationRecord,
			DATEADD(DAY,1,VacationsDetails.ToDate) AS WorkDateCreationRecord,
			VacationsDetails.MainframeNo AS MainframeNoCreationRecord,
			VacationsDetails.Notes AS NotesCreationRecord,
			dbo.fnGetPeriodBetweenTwoDates(VacationsDetails.FromDate,VacationsDetails.ToDate) AS VacationPeriodCreationRecord,
			VacationsActions.VacationActionID AS VacationActionIDCreationRecord,
			VacationsActions.VacationActionName AS VacationActionNameCreationRecord,
			VacationsTypes.VacationTypeName AS VacationTypeNameCreationRecord ,
			vwEmployeesCodes.EmployeeCodeNo EmployeeCodeNoCreationRecord, 
			vwEmployeesCodes.EmployeeCodeID AS EmployeeCodeIDCreationRecord ,
			vwEmployeesCodes.EmployeeIDNo AS EmployeeIDNoCreationRecord, 
			vwEmployeesCodes.EmployeeNameAr AS EmployeeNameArCreationRecord ,
			jobs.JobName AS JobNameCreationRecord,
			OrganizationsStructures.OrganizationName AS OrganizationNameCreationRecord ,
			Ranks.RankName AS RankNameCreationRecord ,
			--VacationsTypes.VacationBalance AS VacationBalanceCreationRecord,			
			(SELECT rcvb.VacationBalance FROM RanksCategoriesVacationsBalances rcvb WHERE rcvb.VacationTypeID=Vacations.VacationTypeID AND rcvb.RankCategoryID=Ranks.RankCategoryID) AS VacationBalanceCreationRecord,
			dbo.[fnGetPreviousConsumedVacationByEmployeeCodeID](Vacations.VacationID,vwEmployeesCodes.EmployeeCodeID,Vacations.VacationTypeID) AS PreviousConsumedVacationCreationRecord
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
	WHERE VacationsDetails.VacationID = @VacationID and VacationsActions.VacationActionID = 1
END
