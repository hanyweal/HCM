
CREATE PROCEDURE [dbo].[spGetEmployeeDetailsByEndOfServiceID]
(
	@EndOfServiceID int
)
AS
BEGIN

	SELECT EC.EmployeeCodeNo, EC.EmployeeIDNo, EC.EmployeeNameAr, 
		OS.OrganizationCode, OS.OrganizationName, dbo.Ranks.RankName, dbo.Jobs.JobName, 
		dbo.OrganizationsJobs.JobNo, EC.EmployeeCodeID, ECH.EmployeeCareerHistoryID,
		(SELECT ECH1.JoinDate FROM dbo.EmployeesCareersHistory ECH1 WHERE ECH1.EmployeeCodeID=EC.EmployeeCodeID AND ECH1.CareerHistoryTypeID=1) as JoiningDate,
		(SELECT TOP 1 BasicSalary FROM BasicSalaries BS WHERE BS.CareerDegreeID=ECH.CareerDegreeID AND BS.RankID=OrganizationsJobs.RankID) as BasicSalary,
		EOS.EndOfServiceID,
		EOS.EndOfServiceDate,
		EOS.EndOfServiceDecisionNo,
		EOS.EndOfServiceDecisionDate,
		r.EndOfServiceReasonID,
		r.EndOfServiceReason,
		c.EndOfServiceCaseID,
		c.EndOfServiceCase
	FROM dbo.OrganizationsJobs 
		INNER JOIN dbo.EmployeesCareersHistory ECH ON dbo.OrganizationsJobs.OrganizationJobID = ECH.OrganizationJobID 
		INNER JOIN dbo.OrganizationsStructures OS ON dbo.OrganizationsJobs.OrganizationID = OS.OrganizationID 
		INNER JOIN dbo.Ranks ON dbo.OrganizationsJobs.RankID = dbo.Ranks.RankID 
		INNER JOIN dbo.vwEmployeesCodes EC ON ECH.EmployeeCodeID = EC.EmployeeCodeID 
		INNER JOIN dbo.Jobs ON dbo.OrganizationsJobs.JobID = dbo.Jobs.JobID
		INNER JOIN dbo.EndOfServices EOS ON EOS.EmployeeCareerHistoryID = ECH.EmployeeCareerHistoryID
		INNER JOIN dbo.EndOfServicesReasons r ON r.EndOfServiceReasonID = EOS.EndOfServiceReasonID
		INNER JOIN dbo.EndOfServicesCases c ON c.EndOfServiceCaseID = r.EndOfServiceCaseID
	WHERE EOS.EndOfServiceID = @EndOfServiceID	

END
