CREATE  PROCEDURE [dbo].[spGetJobVacanciesByRankAndCategory]  
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
  where IsVacant=1 and OJobs.RankID=ISNULL(@RankID,OJobs.RankID) and JobsCategories.JobCategoryID=ISNULL(@JobCategoryID,JobsCategories.JobCategoryID)
  order by OJobs.RankID
END
