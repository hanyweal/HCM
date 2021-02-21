CREATE FUNCTION [dbo].[fnGetCandidatesAlreadyPromotedCountByPromotionPeriodID]
(
	@PromotionPeriodID int,
	@RankID int
)
RETURNS int
AS
BEGIN
	 DECLARE @CandidatesAlreadyPromotedCount int
	 SELECT @CandidatesAlreadyPromotedCount = COUNT(*)
	 FROM PromotionsRecordsEmployees
	 INNER JOIN PromotionsRecords on PromotionsRecords.PromotionRecordID=PromotionsRecordsEmployees.PromotionRecordID	
	 WHERE PromotionRecordJobVacancyID is not null
	 AND NewEmployeeCareerHistoryID is not null
	 AND PromotionPeriodID = @PromotionPeriodID
	 AND PromotionsRecords.RankID=@RankID
	 RETURN @CandidatesAlreadyPromotedCount
END
