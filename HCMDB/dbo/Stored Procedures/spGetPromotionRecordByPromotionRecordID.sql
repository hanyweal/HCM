-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetPromotionRecordByPromotionRecordID]
(
	@PromotionRecordID int	
)
AS
BEGIN
	SELECT  PromotionsRecords.PromotionRecordID, 
			PromotionsRecords.PromotionRecordNo,
			mic_db.dbo.fn_GregToUmAlqura(PromotionsRecords.PromotionRecordDate) AS PromotionRecordDateUmAlQura,
			MaturityYearsBalances.MaturityYear,
			Periods.PeriodName,
			mic_db.dbo.fn_GregToUmAlqura(PromotionsPeriods.PromotionStartDate) AS PromotionStartDateUmAlQura,
			mic_db.dbo.fn_GregToUmAlqura(PromotionsPeriods.PromotionEndDate) AS PromotionEndDateUmAlQura,
			--PromotionsPeriods.PromotionStartDate, 
			--PromotionsPeriods.PromotionEndDate,
			JobsCategories.JobCategoryNo,
			JobsCategories.JobCategoryName,
			JobsGroups.JobGroupName,
			Ranks.RankName,
			PromotionsRecordsStatus.PromotionRecordStatusName,
			vwEmployeesCodes.EmployeeCodeNo CreatedByCode,
			vwEmployeesCodes.EmployeeNameAr CreatedByName,
			PromotionsRecords.CreationDate 
	FROM PromotionsRecords
	INNER JOIN Ranks ON Ranks.RankID = PromotionsRecords.RankID
	INNER JOIN JobsCategories ON JobsCategories.JobCategoryID = PromotionsRecords.JobCategoryID
	INNER JOIN JobsGroups ON JobsGroups.JobGroupID = JobsCategories.JobGroupID
	INNER JOIN PromotionsPeriods ON PromotionsRecords.PromotionPeriodID = PromotionsPeriods.PromotionPeriodID
	INNER JOIN MaturityYearsBalances ON MaturityYearsBalances.MaturityYearID = PromotionsPeriods.YearID
	INNER JOIN PromotionsRecordsStatus ON PromotionsRecordsStatus.PromotionRecordStatusID = PromotionsRecords.PromotionRecordStatusID
	INNER JOIN Periods ON PromotionsPeriods.PeriodID = Periods.PeriodID
	INNER JOIN vwEmployeesCodes ON PromotionsRecords.CreatedBy = vwEmployeesCodes.EmployeeCodeID
	WHERE PromotionRecordID = @PromotionRecordID
END
