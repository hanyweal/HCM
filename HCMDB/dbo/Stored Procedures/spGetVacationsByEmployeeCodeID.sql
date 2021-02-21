-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetVacationsByEmployeeCodeID]-- 22915
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
			VacationsTypes.VacationTypeName,
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
	WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	ORDER BY Vacations.VacationID
	
END
