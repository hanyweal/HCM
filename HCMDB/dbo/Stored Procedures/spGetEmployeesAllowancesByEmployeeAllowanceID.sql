-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetEmployeesAllowancesByEmployeeAllowanceID]
(
	@EmployeeAllowanceID int
)
AS
BEGIN 

	SELECT ea.EmployeeAllowanceID, 
		mic_db.dbo.fn_GregToUmAlqura(ea.AllowanceStartDate) AS AllowanceStartDateUmAlQura, 
			ea.AllowanceStartDate, 
			ec.EmployeeCodeID , 
			e.EmployeeIDNo, 
			e.FirstNameAr, 
			e.MiddleNameAr, 
			e.GrandFatherNameAr, 
			e.LastNameAr, 
			Allowances.AllowanceID, 
			Allowances.AllowanceName, 
			Allowances.AllowanceAmount, 
			AllowancesAmountTypes.AllowanceAmountTypeName,
			Jobs.JobName, 
			ranks.RankName
	FROM EmployeesAllowances ea
	INNER JOIN EmployeesCareersHistory ech ON ea.EmployeeCareerHistoryID = ech.EmployeeCareerHistoryID
	INNER JOIN Allowances ON Allowances.AllowanceID = ea.AllowanceID
	INNER JOIN AllowancesAmountTypes ON Allowances.AllowanceAmountTypeID = AllowancesAmountTypes.AllowanceAmountTypeID
	INNER JOIN OrganizationsJobs ON OrganizationsJobs.OrganizationJobID = ech.OrganizationJobID	
	INNER JOIN EmployeesCodes ec ON ec.EmployeeCodeID = ech.EmployeeCodeID
	INNER JOIN Employees e ON e.EmployeeID = ec.EmployeeID
	INNER JOIN Jobs on Jobs.JobID = OrganizationsJobs.JobID
	INNER JOIN Ranks ON Ranks.RankID = OrganizationsJobs.RankID
	WHERE ea.EmployeeAllowanceID = @EmployeeAllowanceID
	ORDER BY ranks.RankName DESC
END
