CREATE TABLE [dbo].[StopWorks] (
    [StopWorkID]                  INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeCareerHistoryID]     INT            NOT NULL,
    [StopWorkTypeID]              INT            NOT NULL,
    [StopWorkStartDate]           DATE           NOT NULL,
    [StopWorkEndDate]             DATE           NULL,
    [StopPoint]                   NVARCHAR (500) NULL,
    [IsConvicted]                 BIT            NULL,
    [Note]                        NVARCHAR (MAX) NULL,
    [CreatedDate]                 DATETIME       NOT NULL,
    [LastUpdatedDate]             DATETIME       NULL,
    [CreatedBy]                   INT            NULL,
    [LastUpdatedBy]               INT            NULL,
    [StartStopWorkDecisionNumber] NVARCHAR (50)  NULL,
    [StartStopWorkDecisionDate]   DATETIME       NULL,
    [EndStopWorkDecisionNumber]   NVARCHAR (50)  NULL,
    [EndStopWorkDecisionDate]     DATETIME       NULL,
    CONSTRAINT [PK_StopWorks] PRIMARY KEY CLUSTERED ([StopWorkID] ASC),
    CONSTRAINT [FK_StopWorks_CreatedByNav] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_StopWorks_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_StopWorks_LastUpdatedByNav] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_StopWorks_StopWorksTypes] FOREIGN KEY ([StopWorkTypeID]) REFERENCES [dbo].[StopWorksTypes] ([StopWorkTypeID])
);

