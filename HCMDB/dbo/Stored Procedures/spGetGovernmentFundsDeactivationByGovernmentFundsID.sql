-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetGovernmentFundsDeactivationByGovernmentFundsID] 
(
	@GovernmentFundID int
)
AS
BEGIN
		
	SELECT GovernmentFunds.GovernmentFundID,
			GovernmentFunds.EmployeeCodeID,
			GovernmentFunds.GovernmentFundTypeID,
			GovernmentFunds.GovernmentDeductionTypeID,
			GovernmentFunds.LoanNo,
			GovernmentFunds.LoanDate,
			GovernmentFunds.DeductionStartDate,
			mic_db.dbo.fn_GregToUmAlqura(CONVERT(DATE,GovernmentFunds.LoanDate)) AS LoanDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(CONVERT(DATE,GovernmentFunds.DeductionStartDate)) AS DeductionStartDateUmAlQura,			
			GovernmentFunds.MonthlyDeductionAmount,
			GovernmentFunds.TotalDeductionAmount,
			--mic_db.dbo.fn_GregToUmAlqura(CONVERT(DATE,GovernmentFunds.ContractDate)) AS ContractDateUmAlQura,
			--GovernmentFunds.ContractDate,
			GovernmentFunds.ContractNo,
			GovernmentFunds.SponserToIDNo,
			GovernmentFunds.SponserToName,
			GovernmentFunds.CreatedDate,
			GovernmentFunds.LastUpdatedDate,
			GovernmentFunds.CreatedBy,
			GovernmentFunds.LastUpdatedBy,
			GovernmentDeductionsTypes.GovernmentDeductionTypeName,
			GovernmentFundsTypes.GovernmentFundTypeName,
			vwCurrentEmployeesCareer.EmployeeCodeNo, 
			vwCurrentEmployeesCareer.EmployeeIDNo, 
			vwCurrentEmployeesCareer.EmployeeNameAr,
			vwCurrentEmployeesCareer.RankName,
			vwCurrentEmployeesCareer.OrganizationName,
			vwCurrentEmployeesCareer.JobNo,
			KSACities.KSACityName,
			GovernmentFunds.LetterNo,
			mic_db.dbo.fn_GregToUmAlqura(CONVERT(DATE,GovernmentFunds.LetterDate)) AS LetterDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(CONVERT(DATE,GovernmentFunds.DeactiveDate)) AS DeactiveDateUmAlQura
	FROM GovernmentFunds
	INNER JOIN vwCurrentEmployeesCareer ON GovernmentFunds.EmployeeCodeID = vwCurrentEmployeesCareer.EmployeeCodeID
	LEFT JOIN GovernmentDeductionsTypes ON GovernmentFunds.GovernmentDeductionTypeID = GovernmentDeductionsTypes.GovernmentDeductionTypeID
	LEFT JOIN GovernmentFundsTypes ON GovernmentFunds.GovernmentFundTypeID = GovernmentFundsTypes.GovernmentFundTypeID
	LEFT JOIN KSACities ON GovernmentFunds.KSACityID = KSACities.KSACityID
	WHERE GovernmentFundID = @GovernmentFundID
END
