-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetSickVacationsPaidAmount] 

 
AS
BEGIN
 
	SELECT	Vacations.VacationID,
			VacationsDetails.IsApproved,
			VacationsTypes.VacationTypeName,
			VacationsActions.VacationActionName,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.FromDate) AS VacationStartDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(VacationsDetails.ToDate) AS VacationEndDateUmAlQura,
			dbo.fnGetPeriodBetweenTwoDates(VacationsDetails.FromDate ,VacationsDetails.ToDate) AS VacationPeriod,
			vwEmployeesCodes.EmployeeCodeNo, vwEmployeesCodes.EmployeeNameAr,
			Vacations.IsSickLeaveAmountPaid
	FROM Vacations
	INNER JOIN VacationsTypes ON VacationsTypes.VacationTypeID = Vacations.VacationTypeID
	INNER JOIN VacationsDetails ON Vacations.VacationID = VacationsDetails.VacationID
	INNER JOIN VacationsActions ON VacationsDetails.VacationActionID = VacationsActions.VacationActionID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = Vacations.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID = EmployeesCareersHistory.EmployeeCodeID
	WHERE Vacations.IsSickLeaveAmountPaid = 1
	AND Vacations.VacationTypeID = 4		-- sick leave
	ORDER BY Vacations.VacationID
	
END
