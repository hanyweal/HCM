CREATE TABLE [dbo].[PromotionCardsPrinting] (
    [PromotionCardPrintingID] INT      IDENTITY (1, 1) NOT NULL,
    [PromotionPeriodID]       INT      NOT NULL,
    [EmployeeCodeID]          INT      NOT NULL,
    [CreatedDate]             DATETIME NOT NULL,
    [CreatedBy]               INT      NOT NULL,
    [LastUpdatedDate]         DATETIME NULL,
    [LastUpdatedBy]           INT      NULL,
    CONSTRAINT [PK_PromotionCardsPrinting] PRIMARY KEY CLUSTERED ([PromotionCardPrintingID] ASC),
    CONSTRAINT [FK_PromotionCardsPrinting_EmployeesCodes] FOREIGN KEY ([EmployeeCodeID]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionCardsPrinting_EmployeesCodes1] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_PromotionCardsPrinting_EmployeesCodes2] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionCardsPrinting_PromotionsPeriods] FOREIGN KEY ([PromotionPeriodID]) REFERENCES [dbo].[PromotionsPeriods] ([PromotionPeriodID])
);

