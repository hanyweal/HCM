CREATE TABLE [dbo].[PromotionsDecisions] (
    [PromotionDecisionID]   INT           NOT NULL,
    [PromotionDecisionName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PromotionsDecisions] PRIMARY KEY CLUSTERED ([PromotionDecisionID] ASC)
);

