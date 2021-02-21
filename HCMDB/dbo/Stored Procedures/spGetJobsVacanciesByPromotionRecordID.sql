-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetJobsVacanciesByPromotionRecordID]
(
	@PromotionRecordID int	
)
AS
BEGIN
	SELECT  vwOrganizationsJobs.JobNo,
			vwOrganizationsJobs.OrganizationCode,
			vwOrganizationsJobs.OrganizationName,
			vwOrganizationsJobs.JobName,
			vwOrganizationsJobs.JobCode,
			vwOrganizationsJobs.JobCategoryName,
			vwOrganizationsJobs.RankName
	FROM PromotionsRecordsJobsVacancies
	INNER JOIN vwOrganizationsJobs ON PromotionsRecordsJobsVacancies.OrganizationJobID = vwOrganizationsJobs.OrganizationJobID
	WHERE PromotionRecordID = @PromotionRecordID
END
