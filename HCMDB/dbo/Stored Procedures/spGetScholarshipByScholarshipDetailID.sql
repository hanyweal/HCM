-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetScholarshipByScholarshipDetailID] --93
(
	@ScholarshipDetailID INT = 23
)
AS
BEGIN

	SELECT  Scholarships.ScholarshipID,
			mic_db.dbo.fn_GregToUmAlqura(ScholarshipsDetails.FromDate) AS FromDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(ScholarshipsDetails.ToDate) AS ToDateUmAlQura,
			ScholarshipsDetails.FromDate,
			ScholarshipsDetails.ToDate,
			--mic_db.dbo.fn_GregToUmAlqura(DATEADD(DAY,1,ScholarshipsDetails.ToDate)) AS WorkDateUmAlQura,
			--DATEADD(DAY,1,ScholarshipsDetails.ToDate) AS WorkDate,
		
			ScholarshipsDetails.Notes,
			dbo.fnGetPeriodBetweenTwoDates(ScholarshipsDetails.FromDate,ScholarshipsDetails.ToDate) AS ScholarshipPeriod,
			ScholarshipsActions.ScholarshipActionName,
			ScholarshipsTypes.ScholarshipType,
			vwEmployeesCodes.EmployeeCodeNo, 
			vwEmployeesCodes.EmployeeCodeID,
			vwEmployeesCodes.EmployeeIDNo, 
			vwEmployeesCodes.EmployeeNameAr,
			OrganizationsStructures.OrganizationName,
			Ranks.RankName		
			
	FROM Scholarships
	INNER JOIN ScholarshipsTypes ON Scholarships.ScholarshipTypeID = ScholarshipsTypes.ScholarshipTypeID
	INNER JOIN ScholarshipsDetails ON Scholarships.ScholarshipID = ScholarshipsDetails.ScholarshipID 
	INNER JOIN ScholarshipsActions ON ScholarshipsDetails.ScholarshipActionID = ScholarshipsActions.ScholarshipActionID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCodeID = Scholarships.EmployeeCodeID
	INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID = Scholarships.EmployeeCodeID
	INNER JOIN OrganizationsJobs ON OrganizationsJobs.OrganizationJobID = EmployeesCareersHistory.OrganizationJobID
	INNER JOIN OrganizationsStructures ON OrganizationsJobs.OrganizationID = OrganizationsStructures.OrganizationID
	INNER JOIN Ranks ON Ranks.RankID = OrganizationsJobs.RankID
	WHERE ScholarshipsDetails.ScholarshipDetailID = @ScholarshipDetailID
END
