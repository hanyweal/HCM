
GO
/****** Object:  StoredProcedure [dbo].[spGetVacationsByVacationTypeID]    Script Date: 23/08/41 09:54:42 ص ******/
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
			VacationsTypes.VacationTypeName,
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
	WHERE VacationsTypes.VacationTypeID = ISNULL(@VacationTypeID, VacationsTypes.VacationTypeID)
	AND (@StartDate BETWEEN Vacations.VacationStartDate AND Vacations.VacationEndDate
		 OR @EndDate BETWEEN Vacations.VacationStartDate AND Vacations.VacationEndDate)
	ORDER BY Vacations.VacationID
	
END
 