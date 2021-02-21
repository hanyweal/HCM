-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetVacationsByVacationID]-- 22915
(
	@VacationID int
)
AS
BEGIN

	
	SELECT  vwEmployeesCodes.EmployeeCodeID,
			Vacations.VacationID,
			VacationsDetails.IsApproved,
			VacationsTypes.VacationTypeName,
			VacationsActions.VacationActionName,
			mic_db.dbo.fn_GregToUmAlqura(Vacations.VacationStartDate) AS VacationStartDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(Vacations.VacationEndDate) AS VacationEndDateUmAlQura,
			dbo.fnGetPeriodBetweenTwoDates(Vacations.VacationStartDate ,Vacations.VacationEndDate) AS VacationPeriod,
			
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.FromDate) AS FromDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.ToDate) AS ToDateUmAlQura,
			--mic_db.dbo.fn_GregToUmAlqura(DATEADD(DAY,1,Vacations.VacationEndDate)) AS WorkDateUmAlQura,
			dbo.fnGetPeriodBetweenTwoDates(VacationsDetails.FromDate ,VacationsDetails.ToDate) AS VacationActionPeriod
			
	FROM Vacations
	INNER JOIN VacationsTypes ON VacationsTypes.VacationTypeID = Vacations.VacationTypeID
	INNER JOIN VacationsDetails ON Vacations.VacationID = VacationsDetails.VacationID
	INNER JOIN VacationsActions ON VacationsDetails.VacationActionID = VacationsActions.VacationActionID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = Vacations.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID = EmployeesCareersHistory.EmployeeCodeID
	WHERE Vacations.VacationID = @VacationID
	--ORDER BY Vacations.VacationID
	
END
