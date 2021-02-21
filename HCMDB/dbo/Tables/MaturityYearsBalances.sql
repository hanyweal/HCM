CREATE TABLE [dbo].[MaturityYearsBalances] (
    [MaturityYearID] INT IDENTITY (1, 1) NOT NULL,
    [MaturityYear]   INT NOT NULL,
    [Balance]        INT NOT NULL,
    CONSTRAINT [PK_MaturityYearsBalances] PRIMARY KEY CLUSTERED ([MaturityYearID] ASC)
);

