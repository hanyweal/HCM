-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetEmployeesAllowancesByEmployeeCodeID] 
(
	@EmployeeCodeID int
)
AS
BEGIN

	SELECT	ea.EmployeeAllowanceID, 
			ea.AllowanceStartDate,
			mic_db.dbo.fn_GregToUmAlqura(ea.AllowanceStartDate) AS AllowanceStartDateUmAlQura, 
			ec.EmployeeCodeNo, 
			ec.EmployeeCodeID,
			ec.EmployeeIDNo, 
			ec.EmployeeNameAr,
			Allowances.AllowanceID, 
			Allowances.AllowanceName, 
			Allowances.AllowanceAmount, 
			AllowancesAmountTypes.AllowanceAmountTypeName,
			Jobs.JobName,
			ea.IsActive
	FROM EmployeesAllowances ea
	INNER JOIN EmployeesCareersHistory ech ON ea.EmployeeCareerHistoryID = ech.EmployeeCareerHistoryID
	INNER JOIN Allowances ON Allowances.AllowanceID = ea.AllowanceID
	INNER JOIN AllowancesAmountTypes ON Allowances.AllowanceAmountTypeID = AllowancesAmountTypes.AllowanceAmountTypeID
	INNER JOIN OrganizationsJobs ON OrganizationsJobs.OrganizationJobID = ech.OrganizationJobID
	INNER JOIN vwEmployeesCodes ec ON ec.EmployeeCodeID = ech.EmployeeCodeID
	INNER JOIN Employees e ON e.EmployeeID = ec.EmployeeID
	INNER JOIN Jobs on Jobs.JobID = OrganizationsJobs.JobID
	WHERE ech.EmployeeCodeID = @EmployeeCodeID
	
END

 