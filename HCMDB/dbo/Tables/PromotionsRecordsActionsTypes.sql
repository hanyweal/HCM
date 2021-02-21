CREATE TABLE [dbo].[PromotionsRecordsActionsTypes] (
    [PromotionActionTypeID]   INT           NOT NULL,
    [PromotionActionTypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_PromotionsActionsTypes] PRIMARY KEY CLUSTERED ([PromotionActionTypeID] ASC)
);

