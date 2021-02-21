CREATE TABLE [dbo].[InsteadDeportations] (
    [InsteadDeportationID]    INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeCareerHistoryID] INT            NOT NULL,
    [DeportationDate]         DATE           NOT NULL,
    [Note]                    NVARCHAR (MAX) NULL,
    [Amount]                  FLOAT (53)     NULL,
    [CreatedDate]             DATETIME       NOT NULL,
    [LastUpdatedDate]         DATETIME       NULL,
    [CreatedBy]               INT            NULL,
    [LastUpdatedBy]           INT            NULL,
    CONSTRAINT [PK_InsteadDeportations] PRIMARY KEY CLUSTERED ([InsteadDeportationID] ASC),
    CONSTRAINT [FK_InsteadDeportations_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_InsteadDeportations_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_InsteadDeportations_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
);

