CREATE TABLE [dbo].[RanksCategories] (
    [RankCategoryID]   INT           NOT NULL,
    [RankCategoryName] NVARCHAR (50) NOT NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_RanksCategories_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]  DATETIME      NULL,
    CONSTRAINT [PK_RanksCategories] PRIMARY KEY CLUSTERED ([RankCategoryID] ASC)
);

