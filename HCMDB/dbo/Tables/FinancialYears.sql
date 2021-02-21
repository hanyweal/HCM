CREATE TABLE [dbo].[FinancialYears] (
    [FinancialYearID] INT  IDENTITY (1, 1) NOT NULL,
    [FinancialYear]   INT  NOT NULL,
    [StartDate]       DATE NOT NULL,
    [EndDate]         DATE NOT NULL,
    CONSTRAINT [PK_FinancialYears] PRIMARY KEY CLUSTERED ([FinancialYearID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[FinancialYears]([FinancialYear] ASC);

