-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetScholarshipsByEmployeeCodeID]-- 22915
(
	@EmployeeCodeID int
)
AS
BEGIN

	
	
	--SELECT Scholarships.ScholarshipID,			
	--		ScholarshipsTypes.ScholarshipType,
	--		ScholarshipsActions.ScholarshipActionName,
	--		mic_db.dbo.fn_GregToUmAlqura(ScholarshipsDetails.FromDate) AS ScholarshipStartDateUmAlQura,
	--		mic_db.dbo.fn_GregToUmAlqura(ScholarshipsDetails.ToDate) AS ScholarshipEndDateUmAlQura,
	--		--mic_db.dbo.fn_GregToUmAlqura(DATEADD(DAY,1,Vacations.VacationEndDate)) AS WorkDateUmAlQura,
	--		dbo.fnGetPeriodBetweenTwoDates(ScholarshipsDetails.FromDate ,ScholarshipsDetails.ToDate) AS ScholarshipPeriod
	--FROM Scholarships
	--INNER JOIN ScholarshipsTypes ON ScholarshipsTypes.ScholarshipTypeID = Scholarships.ScholarshipTypeID
	--INNER JOIN ScholarshipsDetails ON Scholarships.ScholarshipID = ScholarshipsDetails.ScholarshipID
	--INNER JOIN ScholarshipsActions ON ScholarshipsDetails.ScholarshipActionID = ScholarshipsActions.ScholarshipActionID
	----INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCodeID = Scholarships.EmployeeCodeID
	--INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID = Scholarships.EmployeeCodeID
	--WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	--ORDER BY Scholarships.ScholarshipID

	SELECT Scholarships.ScholarshipID,			
			ScholarshipsTypes.ScholarshipType,
			--ScholarshipsActions.ScholarshipActionName,
			mic_db.dbo.fn_GregToUmAlqura(Scholarships.ScholarshipStartDate) AS ScholarshipStartDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(Scholarships.ScholarshipEndDate) AS ScholarshipEndDateUmAlQura,
			--mic_db.dbo.fn_GregToUmAlqura(DATEADD(DAY,1,Vacations.VacationEndDate)) AS WorkDateUmAlQura,
			dbo.fnGetPeriodBetweenTwoDates(Scholarships.ScholarshipStartDate ,Scholarships.ScholarshipEndDate) AS ScholarshipPeriod
	FROM Scholarships
	INNER JOIN ScholarshipsTypes ON ScholarshipsTypes.ScholarshipTypeID = Scholarships.ScholarshipTypeID
	--INNER JOIN ScholarshipsDetails ON Scholarships.ScholarshipID = ScholarshipsDetails.ScholarshipID
	--INNER JOIN ScholarshipsActions ON ScholarshipsDetails.ScholarshipActionID = ScholarshipsActions.ScholarshipActionID
	--INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCodeID = Scholarships.EmployeeCodeID
	INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID = Scholarships.EmployeeCodeID
	WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	ORDER BY Scholarships.ScholarshipID

	
	
END
