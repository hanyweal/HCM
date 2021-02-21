USE [HCM]
GO

/****** Object:  View [dbo].[vwActualEmployeesBasedOnAssignings]    Script Date: 8/23/2020 08:36:50 ص ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwActualEmployeesBasedOnAssignings]
AS
SELECT        dbo.vwEmployeesCareersHistory.EmployeeCodeNo, dbo.vwEmployeesCareersHistory.EmployeeIDNo, dbo.vwEmployeesCareersHistory.EmployeeNameAr, dbo.vwEmployeesCareersHistory.RankName, dbo.Jobs.JobName, 
                         dbo.Assignings.OrganizationID, dbo.OrganizationsStructures.OrganizationName, dbo.vwEmployeesCareersHistory.RankCategoryID, dbo.vwEmployeesCareersHistory.RankCategoryName, 
                         dbo.vwEmployeesCareersHistory.RankID, dbo.vwEmployeesCareersHistory.RankName AS Expr1, dbo.Assignings.AssigningStartDate, dbo.Assignings.AssigningEndDate, dbo.Assignings.IsFinished, 
                         dbo.Assignings.EmployeeCareerHistoryID , dbo.vwEmployeesCareersHistory.EmployeeCodeID
FROM            dbo.Assignings INNER JOIN
                         dbo.Jobs ON dbo.Jobs.JobID = dbo.Assignings.JobID INNER JOIN
                         dbo.vwEmployeesCareersHistory ON dbo.Assignings.EmployeeCareerHistoryID = dbo.vwEmployeesCareersHistory.EmployeeCareerHistoryID INNER JOIN
                         dbo.OrganizationsStructures ON dbo.Assignings.OrganizationID = dbo.OrganizationsStructures.OrganizationID INNER JOIN
                         dbo.OrganizationsJobs ON dbo.vwEmployeesCareersHistory.OrganizationJobID = dbo.OrganizationsJobs.OrganizationJobID
UNION
SELECT        dbo.vwCurrentEmployeesCareer.EmployeeCodeNo, dbo.vwCurrentEmployeesCareer.EmployeeIDNo, dbo.vwCurrentEmployeesCareer.EmployeeNameAr, vwCurrentEmployeesCareer.RankName, 
                         N'مدير ' + OrganizationsStructures.OrganizationName AS JobName, OrganizationsStructures.OrganizationID, OrganizationsStructures.OrganizationName, vwCurrentEmployeesCareer.RankCategoryID, 
                         vwCurrentEmployeesCareer.RankCategoryName, vwCurrentEmployeesCareer.RankID, vwCurrentEmployeesCareer.RankName AS Expr1, DATEADD(year, - 10, CONVERT(date, getdate())), CONVERT(date, getdate()), 
                         'FALSE' AS IsFinshed, vwCurrentEmployeesCareer.EmployeeCareerHistoryID ,  dbo.vwCurrentEmployeesCareer.EmployeeCodeID
FROM            OrganizationsStructures INNER JOIN
                         vwCurrentEmployeesCareer ON OrganizationsStructures.ManagerCodeID = vwCurrentEmployeesCareer.EmployeeCodeID
GO
