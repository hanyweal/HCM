
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetScholarshipByID]
(
	@ScholarshipID int
)
AS
BEGIN

	SELECT Scholarships.ScholarshipID, 
		Scholarships.EmployeeCodeID, 
		Scholarships.ScholarshipTypeID,
		KSACities.KSACityName, 
		Countries.CountryName, 
	    Scholarships.Location, 
	    Scholarships.ScholarshipStartDate,
	    Scholarships.ScholarshipEndDate,
	    mic_db.dbo.fn_GregToUmAlqura(Scholarships.ScholarshipStartDate) AS ScholarshipStartDateUmAlQura,
	    mic_db.dbo.fn_GregToUmAlqura(Scholarships.ScholarshipEndDate) AS ScholarshipEndDateUmAlQura,
	  Scholarships.QualificationID, 
	  Scholarships.CreatedDate, 
	  Scholarships.LastUpdatedDate, 
     ScholarshipsTypes.ScholarshipType, 
	  dbo.[fnGetCurrentQualification](Scholarships.EmployeeCodeID) AS CurrentQualification, 
	  QualificationsDegrees.QualificationDegreeName, 
	  Qualifications.QualificationName,
	  Scholarships.ScholarshipReason,
	   dbo.fnGetPeriodBetweenTwoDates(Scholarships.ScholarshipStartDate,Scholarships.ScholarshipEndDate) AS Period
	FROM QualificationsDegrees 
		INNER JOIN Qualifications ON QualificationsDegrees.QualificationDegreeID = Qualifications.QualificationDegreeID 
		RIGHT OUTER JOIN Scholarships 
		INNER JOIN ScholarshipsTypes ON Scholarships.ScholarshipTypeID = ScholarshipsTypes.ScholarshipTypeID ON  Qualifications.QualificationID = Scholarships.QualificationID
		LEFT JOIN KSACities on KSACities.KSACityID=Scholarships.KSACityID
		LEFT JOIN Countries on Countries.CountryID=Scholarships.CountryID
	WHERE ScholarshipID = @ScholarshipID

END
