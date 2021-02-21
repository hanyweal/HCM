-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetInternshipScholarshipDetailsByEmployeeCodeID]-- 22915
(
	@EmployeeCodeID int
)
AS
BEGIN

	SELECT InternshipScholarships.*,
			vwEmployeesCodes.EmployeeCodeNo, 
			vwEmployeesCodes.EmployeeCodeID,
			vwEmployeesCodes.EmployeeIDNo, 
			vwEmployeesCodes.EmployeeNameAr, 
			InternshipScholarshipsTypes.InternshipScholarshipTypeName,
			dbo.fnGetPeriodBetweenTwoDates(InternshipScholarships.InternshipScholarshipStartDate , InternshipScholarships.InternshipScholarshipEndDate) AS InternshipScholarshipPeriod
	FROM InternshipScholarships
	INNER JOIN InternshipScholarshipsDetails ON InternshipScholarships.InternshipScholarshipID = InternshipScholarshipsDetails.InternshipScholarshipID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = InternshipScholarshipsDetails.EmployeeCareerHistoryID
	INNER JOIN InternshipScholarshipsTypes ON InternshipScholarships.InternshipScholarshipTypeID = InternshipScholarshipsTypes.InternshipScholarshipTypeID
	INNER JOIN vwEmployeesCodes ON EmployeesCareersHistory.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
	WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	ORDER BY InternshipScholarshipStartDate
	
END
