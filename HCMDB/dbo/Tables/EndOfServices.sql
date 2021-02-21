CREATE TABLE [dbo].[EndOfServices] (
    [EndOfServiceID]           INT           IDENTITY (1, 1) NOT NULL,
    [EmployeeCareerHistoryID]  INT           NOT NULL,
    [EndOfServiceDate]         DATE          NOT NULL,
    [EndOfServiceDecisionNo]   NVARCHAR (50) NOT NULL,
    [EndOfServiceDecisionDate] DATE          NOT NULL,
    [EndOfServiceReasonID]     INT           NOT NULL,
    [isProcessed]              BIT           NULL,
    [CreatedDate]              DATETIME      CONSTRAINT [DF_EndOfServices_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]          DATETIME      NULL,
    [CreatedBy]                INT           NULL,
    [LastUpdatedBy]            INT           NULL,
    CONSTRAINT [PK_EndOfServices] PRIMARY KEY CLUSTERED ([EndOfServiceID] ASC),
    CONSTRAINT [FK_EndOfServices_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_EndOfServices_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_EndOfServices_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_EndOfServices_EndOfServicesReasons] FOREIGN KEY ([EndOfServiceReasonID]) REFERENCES [dbo].[EndOfServicesReasons] ([EndOfServiceReasonID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_EndOfServices]
    ON [dbo].[EndOfServices]([EmployeeCareerHistoryID] ASC);

