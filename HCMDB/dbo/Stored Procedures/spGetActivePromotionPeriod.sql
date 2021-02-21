-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetActivePromotionPeriod]

AS
BEGIN
	SELECT TOP(1) mic_db.dbo.fn_GregToUmAlqura(PromotionsPeriods.PromotionStartDate) AS  PromotionStartDateUmAlqura, 
	mic_db.dbo.fn_GregToUmAlqura(PromotionsPeriods.PromotionEndDate) AS  PromotionEndDateUmAlqura,
	MaturityYearsBalances.MaturityYear, 
	Periods.PeriodName 
	FROM PromotionsPeriods
	INNER JOIN Periods ON PromotionsPeriods.PeriodID = Periods.PeriodID
	INNER JOIN MaturityYearsBalances ON PromotionsPeriods.YearID = MaturityYearsBalances.MaturityYearID
	WHERE IsActive = 1
	ORDER BY PromotionsPeriods.YearID DESC
END
