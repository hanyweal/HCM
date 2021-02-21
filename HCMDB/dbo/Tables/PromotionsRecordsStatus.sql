CREATE TABLE [dbo].[PromotionsRecordsStatus] (
    [PromotionRecordStatusID]   INT           NOT NULL,
    [PromotionRecordStatusName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PromotionsRecordsStatus] PRIMARY KEY CLUSTERED ([PromotionRecordStatusID] ASC)
);

