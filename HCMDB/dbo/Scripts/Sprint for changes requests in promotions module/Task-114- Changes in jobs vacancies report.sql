ALTER PROCEDURE [dbo].[spGetJobVacanciesByRankID]
(
	@RankID int 
)
AS
BEGIN
SELECT RankName ,JobNo ,JobName ,OrganizationName ,JobsCategories.JobCategoryName ,Jobs.JobCode
  FROM OrganizationsJobs 
  INNER JOIN Ranks ON [OrganizationsJobs].RankID = Ranks.RankID
  INNER JOIN Jobs ON [OrganizationsJobs].JobID = Jobs.JobID 
  INNER JOIN JobsCategories ON Jobs.JobCategoryID = JobsCategories.JobCategoryID
  INNER JOIN OrganizationsStructures ON OrganizationsStructures.OrganizationID = OrganizationsJobs.OrganizationID
  WHERE IsVacant = 1 and Ranks.RankID = @RankID
  ORDER BY CONVERT(int, JobNo) 
END