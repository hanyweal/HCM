CREATE TABLE [dbo].[OverTimesDetails] (
    [OverTimeDetailID]        INT      IDENTITY (1, 1) NOT NULL,
    [OverTimeID]              INT      NOT NULL,
    [CreateDate]              DATETIME NOT NULL,
    [LastUpdatedDate]         DATETIME NULL,
    [CreatedBy]               INT      NULL,
    [LastUpdatedBy]           INT      NULL,
    [EmployeeCareerHistoryID] INT      NOT NULL,
    CONSTRAINT [PK_OverTimesDetails] PRIMARY KEY CLUSTERED ([OverTimeDetailID] ASC),
    CONSTRAINT [FK_OverTimesDetails_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_OverTimesDetails_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_OverTimesDetails_EmployeesCodes1] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_OverTimesDetails_EmployeesCodes2] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_OverTimesDetails_OverTimes] FOREIGN KEY ([OverTimeID]) REFERENCES [dbo].[OverTimes] ([OverTimeID])
);

