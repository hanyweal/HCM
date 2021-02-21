CREATE TABLE [dbo].[PromotionsPeriods] (
    [PromotionPeriodID]  INT      IDENTITY (1, 1) NOT NULL,
    [PeriodID]           INT      NOT NULL,
    [YearID]             INT      NOT NULL,
    [PromotionStartDate] DATE     NOT NULL,
    [PromotionEndDate]   DATE     NOT NULL,
    [IsActive]           BIT      NULL,
    [CreatedBy]          INT      NULL,
    [CreatedDate]        DATETIME NULL,
    [LastUpdatedBy]      INT      NULL,
    [LastUpdatedDate]    DATETIME NULL,
    CONSTRAINT [PK_PromotionsPeriods] PRIMARY KEY CLUSTERED ([PromotionPeriodID] ASC),
    CONSTRAINT [FK_PromotionsPeriods_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsPeriods_MaturityYearsBalances] FOREIGN KEY ([YearID]) REFERENCES [dbo].[MaturityYearsBalances] ([MaturityYearID]),
    CONSTRAINT [FK_PromotionsPeriods_Periods] FOREIGN KEY ([PeriodID]) REFERENCES [dbo].[Periods] ([PeriodID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[PromotionsPeriods]([PeriodID] ASC, [YearID] ASC);

