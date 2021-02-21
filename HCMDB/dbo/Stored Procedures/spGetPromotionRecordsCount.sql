CREATE PROCEDURE [dbo].[spGetPromotionRecordsCount] 
	@PromotionPeriodID int 
AS
BEGIN
	
	SELECT RankName,
	dbo.fnGetPromotionRecordCountByPromotionPeriodID(@PromotionPeriodID,PromotionsRecords.RankID) AS PromotionRecordCount,
	dbo.fnGetCandidatesAlreadyPromotedCountByPromotionPeriodID(@PromotionPeriodID,PromotionsRecords.RankID) AS CandidatesAlreadyPromoted
	FROM PromotionsRecordsEmployees
	INNER JOIN PromotionsRecords on PromotionsRecords.PromotionRecordID=PromotionsRecordsEmployees.PromotionRecordID
	INNER JOIN Ranks on PromotionsRecords.RankID = Ranks.RankID	 
	WHERE  PromotionPeriodID = @PromotionPeriodID
	GROUP BY RankName,PromotionsRecords.RankID

END
