GO
ALTER  PROCEDURE [dbo].[spGetJobVacanciesByRankAndCategory]  
@RankID int=null,
@JobCategoryID int=null
AS
BEGIN
  SELECT RankName,
		 JobsCategories.JobCategoryName,		 
		 JobName,
		 OJobs.JobNo,
		 JobsCategories.JobCategoryNo,  
		 OrganizationName,     
		 OrganizationJobStatusName
  FROM   OrganizationsJobs as OJobs
  inner join Ranks  on Ranks.RankID = OJobs.RankID
  inner join Jobs   on Jobs.JobID=OJobs.JobID
  inner join OrganizationsJobsStatus   on OrganizationsJobsStatus.OrganizationJobStatusID=OJobs.OrganizationJobStatusID
  inner join JobsCategories on Jobs.JobCategoryID=JobsCategories.JobCategoryID
  inner join OrganizationsStructures on OrganizationsStructures.OrganizationID=OJobs.OrganizationID
  where OJobs.IsActive=1 and IsVacant=1 and OJobs.RankID=ISNULL(@RankID,OJobs.RankID) and JobsCategories.JobCategoryID=ISNULL(@JobCategoryID,JobsCategories.JobCategoryID)
  order by OJobs.RankID
END


GO
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
  WHERE IsVacant = 1 and Ranks.RankID = @RankID and OrganizationsJobs.IsActive=1
  ORDER BY CONVERT(int, JobNo) 
END