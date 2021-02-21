CREATE TABLE [dbo].[Ranks] (
    [RankID]               INT           IDENTITY (1, 1) NOT NULL,
    [RankCategoryID]       INT           NOT NULL,
    [RankName]             NVARCHAR (50) NOT NULL,
    [CreatedDate]          DATETIME      CONSTRAINT [DF_Ranks_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]      DATETIME      NULL,
    [NextRankID]           INT           NULL,
    [TransfareAllowance]   FLOAT (53)    NULL,
    [InternalDelegation]   FLOAT (53)    NULL,
    [ExternalDelegation]   FLOAT (53)    NULL,
    [IsActiveForPromotion] BIT           NULL,
    CONSTRAINT [PK_Ranks] PRIMARY KEY CLUSTERED ([RankID] ASC),
    CONSTRAINT [FK_Ranks_Ranks] FOREIGN KEY ([NextRankID]) REFERENCES [dbo].[Ranks] ([RankID]),
    CONSTRAINT [FK_Ranks_RanksCategories] FOREIGN KEY ([RankCategoryID]) REFERENCES [dbo].[RanksCategories] ([RankCategoryID])
);

