-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetInternshipScholarshipByInternshipScholarshipID]
(
	@InternshipScholarshipID int
)
AS
BEGIN

	SELECT InternshipScholarships.InternshipScholarshipID, 
		 InternshipScholarships.InternshipScholarshipTypeID, 
		 InternshipScholarships.InternshipScholarshipStartDate, 
		 mic_db.dbo.fn_GregToUmAlqura(InternshipScholarships.InternshipScholarshipStartDate) AS InternshipScholarshipStartDateUmAlQura,
		 InternshipScholarships.InternshipScholarshipEndDate, 
		 mic_db.dbo.fn_GregToUmAlqura(InternshipScholarships.InternshipScholarshipEndDate) AS InternshipScholarshipEndDateUmAlQura,
		 InternshipScholarships.InternshipScholarshipLocation,
		 InternshipScholarships.InternshipScholarshipReason,
		 InternshipScholarships.CountryID, 
		 InternshipScholarships.KSACityID, 
		 InternshipScholarships.IsActive, 
		 InternshipScholarships.CreatedDate, 
		 InternshipScholarships.LastUpdatedDate, 
		 InternshipScholarshipsTypes.InternshipScholarshipTypeName,  
		 Countries.CountryName, 
		 KSARegions.KSARegionName, 
		 KSACities.KSACityName, 
		 dbo.fnGetPeriodBetweenTwoDates(InternshipScholarships.InternshipScholarshipStartDate,InternshipScholarships.InternshipScholarshipEndDate) AS InternshipScholarshipPeriod
	FROM KSARegions 
		INNER JOIN KSACities ON KSARegions.KSARegionID = KSACities.KSARegionID 
		RIGHT OUTER JOIN InternshipScholarships 
		INNER JOIN InternshipScholarshipsTypes ON InternshipScholarships.InternshipScholarshipTypeID = InternshipScholarshipsTypes.InternshipScholarshipTypeID ON KSACities.KSACityID = InternshipScholarships.KSACityID 
		LEFT OUTER JOIN Countries ON InternshipScholarships.CountryID = Countries.CountryID
	WHERE InternshipScholarshipID = @InternshipScholarshipID
	
END
