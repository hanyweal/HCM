-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetInternshipScholarshipDetailsByInternshipScholarshipID] --93
(
	@InternshipScholarshipID int
)
AS
BEGIN

	SELECT InternshipScholarships.InternshipScholarshipID, 
			vwEmployeesCodes.EmployeeCodeNo, 
			vwEmployeesCodes.EmployeeCodeID,
			vwEmployeesCodes.EmployeeIDNo, 
			vwEmployeesCodes.EmployeeNameAr
			,Ranks.RankName
	FROM InternshipScholarships
	INNER JOIN InternshipScholarshipsDetails ON InternshipScholarships.InternshipScholarshipID = InternshipScholarshipsDetails.InternshipScholarshipID 
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = InternshipScholarshipsDetails.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON EmployeesCareersHistory.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
	INNER JOIN OrganizationsJobs ON OrganizationsJobs.OrganizationJobID = EmployeesCareersHistory.OrganizationJobID
	INNER JOIN Ranks ON Ranks.RankID = OrganizationsJobs.RankID
	WHERE InternshipScholarships.InternshipScholarshipID = @InternshipScholarshipID
	ORDER BY 	Ranks.RankName DESC
	
END
