-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetPreviousConsumedDelegationByEmployeeCodeID]
(
	 @DelegationID int,
	 @EmployeeCodeID int,
	 @FinancialYear int
)
RETURNS INT
AS
BEGIN
	DECLARE @Result INT
	SELECT @Result = ISNULL(SUM(DBO.fnGetPeriodBetweenTwoDates(Delegations.DelegationstartDate,Delegations.DelegationEndDate)),0)
	FROM Delegations
	INNER JOIN DelegationsDetails ON Delegations.DelegationID = DelegationsDetails.DelegationID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = DelegationsDetails.EmployeeCareerHistoryID
	WHERE EmployeesCareersHistory.EmployeeCodeID = @EmployeeCodeID
	--AND Delegations.DelegationStartDate < (SELECT DelegationStartDate 
	--									   FROM Delegations 
	--									   INNER JOIN DelegationsDetails ON Delegations.DelegationID = DelegationsDetails.DelegationID
	--									   WHERE Delegations.DelegationID = @DelegationID 
	--									   AND DelegationsDetails.EmployeeCodeID=@EmployeeCodeID
	--									   AND DelegationKindID = 1) -- Tasks مهمة عمل
	AND DATEPART(YEAR,Delegations.DelegationStartDate) = @FinancialYear
	AND Delegations.DelegationKindID = 1 -- Tasks مهمة عمل
	AND Delegations.DelegationID != @DelegationID
	RETURN @Result
END
