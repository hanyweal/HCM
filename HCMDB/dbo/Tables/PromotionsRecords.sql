CREATE TABLE [dbo].[PromotionsRecords] (
    [PromotionRecordID]       INT      IDENTITY (1, 1) NOT NULL,
    [PromotionRecordNo]       INT      NOT NULL,
    [PromotionRecordDate]     DATE     NOT NULL,
    [RankID]                  INT      NOT NULL,
    [JobCategoryID]           INT      NOT NULL,
    [PromotionPeriodID]       INT      NOT NULL,
    [PromotionRecordStatusID] INT      NOT NULL,
    [OpeningTime]             DATETIME NOT NULL,
    [OpeningBy]               INT      NOT NULL,
    [PreferenceTime]          DATETIME NULL,
    [PreferenceBy]            INT      NULL,
    [InstallationTime]        DATETIME NULL,
    [InstallationBy]          INT      NULL,
    [ClosingTime]             DATETIME NULL,
    [ClosingBy]               INT      NULL,
    [CreationDate]            DATETIME NOT NULL,
    [CreatedBy]               INT      NOT NULL,
    [LastUpdatedDate]         DATETIME NULL,
    [LastUpdatedBy]           INT      NULL,
    [PromotionDate]           DATE     NULL,
    CONSTRAINT [PK_PromotionsRecords] PRIMARY KEY CLUSTERED ([PromotionRecordID] ASC),
    CONSTRAINT [FK_PromotionsRecords_EmployeesCodes] FOREIGN KEY ([PreferenceBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecords_EmployeesCodes1] FOREIGN KEY ([OpeningBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecords_EmployeesCodes2] FOREIGN KEY ([InstallationBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecords_EmployeesCodes3] FOREIGN KEY ([ClosingBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecords_EmployeesCodes4] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecords_EmployeesCodes5] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecords_JobsCategories] FOREIGN KEY ([JobCategoryID]) REFERENCES [dbo].[JobsCategories] ([JobCategoryID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PromotionsRecords_PromotionsPeriods] FOREIGN KEY ([PromotionPeriodID]) REFERENCES [dbo].[PromotionsPeriods] ([PromotionPeriodID]),
    CONSTRAINT [FK_PromotionsRecords_PromotionsRecordsStatus] FOREIGN KEY ([PromotionRecordStatusID]) REFERENCES [dbo].[PromotionsRecordsStatus] ([PromotionRecordStatusID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PromotionsRecords_Ranks] FOREIGN KEY ([RankID]) REFERENCES [dbo].[Ranks] ([RankID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[PromotionsRecords]([RankID] ASC, [JobCategoryID] ASC, [PromotionPeriodID] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique-No]
    ON [dbo].[PromotionsRecords]([PromotionRecordNo] ASC, [PromotionPeriodID] ASC);

