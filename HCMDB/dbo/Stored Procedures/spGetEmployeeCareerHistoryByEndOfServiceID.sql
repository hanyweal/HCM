
CREATE PROCEDURE [dbo].[spGetEmployeeCareerHistoryByEndOfServiceID]
(
	@EndOfServiceID int
)
AS
BEGIN

	DECLARE @EmployeeCareerHistoryID int, @EmployeeCodeID int
	SELECT @EmployeeCareerHistoryID = EmployeeCareerHistoryID FROM EndOfServices EOS WHERE EndOfServiceID = @EndOfServiceID
	SELECT @EmployeeCodeID = EmployeeCodeID FROM EmployeesCareersHistory WHERE EmployeeCareerHistoryID = @EmployeeCareerHistoryID
	
	SELECT EC.EmployeeCodeID, ECH.EmployeeCareerHistoryID, N'المؤسسة العامة للصناعات العسكرية' as OrganizationName,
		--OS.OrganizationCode, OS.OrganizationName, 
		dbo.Ranks.RankName, dbo.Jobs.JobName, dbo.OrganizationsJobs.JobNo, 
		CHT.CareerHistoryTypeID, CHT.CareerHistoryTypeName,
		ECH.JoinDate,
		BS.BasicSalary 
	FROM dbo.OrganizationsJobs 
	INNER JOIN dbo.EmployeesCareersHistory ECH ON dbo.OrganizationsJobs.OrganizationJobID = ECH.OrganizationJobID 
	--INNER JOIN dbo.OrganizationsStructures OS ON dbo.OrganizationsJobs.OrganizationID = OS.OrganizationID 
	INNER JOIN dbo.Ranks ON dbo.OrganizationsJobs.RankID = dbo.Ranks.RankID 
	INNER JOIN dbo.vwEmployeesCodes EC ON ECH.EmployeeCodeID = EC.EmployeeCodeID 
	INNER JOIN dbo.Jobs ON dbo.OrganizationsJobs.JobID = dbo.Jobs.JobID
	INNER JOIN dbo.CareersHistoryTypes CHT ON CHT.CareerHistoryTypeID = ECH.CareerHistoryTypeID
	LEFT OUTER JOIN dbo.BasicSalaries BS ON BS.CareerDegreeID = ECH.CareerDegreeID AND BS.RankID = ranks.RankID 
	WHERE EC.EmployeeCodeID = @EmployeeCodeID
	ORDER BY ECH.JoinDate
	
END
