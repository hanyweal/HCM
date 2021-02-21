CREATE TABLE [dbo].[RanksCategoriesVacationsBalances] (
    [RankCategoryVacationBalanceID] INT        IDENTITY (1, 1) NOT NULL,
    [RankCategoryID]                INT        NOT NULL,
    [VacationTypeID]                INT        NOT NULL,
    [VacationBalance]               FLOAT (53) NOT NULL,
    CONSTRAINT [PK_RanksCategoriesVacationsBalances] PRIMARY KEY CLUSTERED ([RankCategoryVacationBalanceID] ASC),
    CONSTRAINT [FK_RanksCategoriesVacationsBalances_RanksCategories] FOREIGN KEY ([RankCategoryID]) REFERENCES [dbo].[RanksCategories] ([RankCategoryID]),
    CONSTRAINT [FK_RanksCategoriesVacationsBalances_VacationsTypes] FOREIGN KEY ([VacationTypeID]) REFERENCES [dbo].[VacationsTypes] ([VacationTypeID])
);

