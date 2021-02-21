-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetGovernmentFundsByEmployeeCodeID] 
(
	@EmployeeCodeID int
)
AS
BEGIN
	SELECT GovernmentFunds.*,
			GovernmentDeductionsTypes.GovernmentDeductionTypeName,
			GovernmentFundsTypes.GovernmentFundTypeName		
	FROM GovernmentFunds
	LEFT JOIN GovernmentDeductionsTypes ON GovernmentFunds.GovernmentDeductionTypeID = GovernmentDeductionsTypes.GovernmentDeductionTypeID
	LEFT JOIN GovernmentFundsTypes ON GovernmentFunds.GovernmentFundTypeID = GovernmentFundsTypes.GovernmentFundTypeID
	WHERE GovernmentFunds.EmployeeCodeID = @EmployeeCodeID
END
