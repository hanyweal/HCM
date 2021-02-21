CREATE FUNCTION [dbo].[fnGetPromotionRecordCountByPromotionPeriodID]
(
	@PromotionPeriodID int,
	@RankID int
)
RETURNS int
AS
BEGIN
	 DECLARE @@PromotionsRecordsCount int
	 SELECT @@PromotionsRecordsCount = COUNT(*)
	 FROM PromotionsRecords	
	 WHERE PromotionPeriodID = @PromotionPeriodID and RankID=@RankID	
	 RETURN @@PromotionsRecordsCount
END
