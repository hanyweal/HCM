CREATE VIEW [dbo].[vwEmployeesCareersHistory]
AS
SELECT     dbo.vwEmployeesCodes.EmployeeCodeNo, dbo.vwEmployeesCodes.EmployeeIDNo, dbo.vwEmployeesCodes.EmployeeNameAr, 
                      dbo.OrganizationsStructures.OrganizationCode, dbo.OrganizationsStructures.OrganizationName, dbo.Ranks.RankName, dbo.Jobs.JobName, 
                      dbo.OrganizationsJobs.JobNo, dbo.vwEmployeesCodes.EmployeeCodeID, dbo.EmployeesCareersHistory.EmployeeCareerHistoryID, 
                      dbo.EmployeesCareersHistory.JoinDate, dbo.Jobs.JobCode, dbo.Ranks.RankID, dbo.OrganizationsJobs.IsReserved, dbo.OrganizationsJobs.IsVacant, 
                      dbo.JobsCategories.JobCategoryID, dbo.JobsCategories.JobCategoryName, dbo.vwEmployeesCodes.IsActive, 
                      dbo.EmployeesCareersHistory.IsActive AS IsCurrentJob
FROM         dbo.OrganizationsJobs INNER JOIN
                      dbo.EmployeesCareersHistory ON dbo.OrganizationsJobs.OrganizationJobID = dbo.EmployeesCareersHistory.OrganizationJobID INNER JOIN
                      dbo.OrganizationsStructures ON dbo.OrganizationsJobs.OrganizationID = dbo.OrganizationsStructures.OrganizationID INNER JOIN
                      dbo.Ranks ON dbo.OrganizationsJobs.RankID = dbo.Ranks.RankID INNER JOIN
                      dbo.vwEmployeesCodes ON dbo.EmployeesCareersHistory.EmployeeCodeID = dbo.vwEmployeesCodes.EmployeeCodeID INNER JOIN
                      dbo.Jobs ON dbo.OrganizationsJobs.JobID = dbo.Jobs.JobID INNER JOIN
                      dbo.JobsCategories ON dbo.Jobs.JobCategoryID = dbo.JobsCategories.JobCategoryID
