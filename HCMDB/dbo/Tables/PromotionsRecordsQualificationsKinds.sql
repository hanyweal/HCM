CREATE TABLE [dbo].[PromotionsRecordsQualificationsKinds] (
    [PromotionRecordQualificationKindID]   INT            IDENTITY (1, 1) NOT NULL,
    [PromotionRecordQualificationKindName] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_PromotionsRecordsQualificationsKinds] PRIMARY KEY CLUSTERED ([PromotionRecordQualificationKindID] ASC)
);

