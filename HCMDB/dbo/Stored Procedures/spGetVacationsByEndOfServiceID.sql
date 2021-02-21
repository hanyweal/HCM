
CREATE PROCEDURE [dbo].[spGetVacationsByEndOfServiceID] 
(
	@EndOfServiceID int
)
AS
BEGIN

	DECLARE @EmployeeCareerHistoryID int, @EmployeeCodeID int
	SELECT @EmployeeCareerHistoryID = EmployeeCareerHistoryID FROM EndOfServices EOS WHERE EndOfServiceID = @EndOfServiceID
	SELECT @EmployeeCodeID = EmployeeCodeID FROM EmployeesCareersHistory WHERE EmployeeCareerHistoryID = @EmployeeCareerHistoryID
	
	SELECT DATEDIFF(DAY, v.VacationStartDate, v.VacationEndDate) + 1 as Period, v.VacationID, v.VacationStartDate, v.VacationEndDate, vt.VacationTypeName 
	FROM Vacations v 
	INNER JOIN VacationsTypes vt ON vt.VacationTypeID = v.VacationTypeID
	WHERE v.VacationTypeID = 1 
	AND v.EmployeeCareerHistoryID IN (SELECT ECH.EmployeeCareerHistoryID FROM EmployeesCareersHistory ECH WHERE ECH.EmployeeCodeID = @EmployeeCodeID)
	UNION ALL
	SELECT DATEDIFF(DAY, EOSV.VacationStartDate, EOSV.VacationEndDate) + 1 AS Period, EOSV.EndOfServiceVacationID AS VacationID, EOSV.VacationStartDate AS VacationStartDate, EOSV.VacationEndDate AS VacationEndDate, vt.VacationTypeName  AS VacationTypeName
		FROM EndOfServicesVacations EOSV 
			INNER JOIN VacationsTypes vt ON vt.VacationTypeID = EOSV.VacationTypeID
		WHERE EOSV.EndOfServiceID=@EndOfServiceID
			AND EOSV.VacationTypeID = 1 
	ORDER BY v.VacationStartDate
END
