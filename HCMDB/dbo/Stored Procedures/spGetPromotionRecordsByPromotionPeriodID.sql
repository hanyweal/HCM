CREATE PROCEDURE [dbo].[spGetPromotionRecordsByPromotionPeriodID] 
	@PromotionPeriodID int 
AS
BEGIN
	
	SELECT 
	MaturityYear,
	PeriodName,
	mic_db.dbo.fn_GregToUmAlqura(PromotionStartDate) PromotionStartDate,
	mic_db.dbo.fn_GregToUmAlqura(PromotionEndDate) PromotionEndDate
	FROM PromotionsPeriods
	INNER JOIN MaturityYearsBalances on MaturityYearsBalances.MaturityYearID=PromotionsPeriods.YearID
	INNER JOIN Periods on Periods.PeriodID=PromotionsPeriods.PeriodID
	WHERE PromotionPeriodID=@PromotionPeriodID

END
