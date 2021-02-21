
CREATE PROCEDURE [dbo].[spGetEmployeeAllowancesByEndOfServiceID]
(
	@EndOfServiceID int
)
AS
BEGIN

	DECLARE @EmployeeCareerHistoryID int

	SELECT @EmployeeCareerHistoryID = EmployeeCareerHistoryID FROM EndOfServices EOS WHERE EndOfServiceID = @EndOfServiceID
	
	SELECT aat.AllowanceAmountTypeName, a.AllowanceName, SUM(a.AllowanceAmount) as TotalAllowanceAmount
	FROM EmployeesAllowances EA 
		INNER JOIN Allowances a ON a.AllowanceID = EA.AllowanceID
		INNER JOIN AllowancesAmountTypes aat ON aat.AllowanceAmountTypeID = a.AllowanceAmountTypeID
	WHERE EA.EmployeeCareerHistoryID = @EmployeeCareerHistoryID 
	GROUP BY aat.AllowanceAmountTypeName, a.AllowanceName 
	
END
