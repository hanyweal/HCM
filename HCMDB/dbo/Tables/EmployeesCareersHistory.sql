CREATE TABLE [dbo].[EmployeesCareersHistory] (
    [EmployeeCareerHistoryID] INT      IDENTITY (1, 1) NOT NULL,
    [EmployeeCodeID]          INT      NOT NULL,
    [CareerHistoryTypeID]     INT      NOT NULL,
    [OrganizationJobID]       INT      NOT NULL,
    [CareerDegreeID]          INT      NOT NULL,
    [JoinDate]                DATE     NOT NULL,
    [TransactionStartDate]    DATE     NOT NULL,
    [TransactionEndDate]      DATE     NULL,
    [IsActive]                BIT      NOT NULL,
    [CreatedDate]             DATETIME CONSTRAINT [DF_EmployeesCareersHistory_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]         DATETIME NULL,
    [CreatedBy]               INT      NULL,
    [LastUpdatedBy]           INT      NULL,
    CONSTRAINT [PK_EmployeesCareersHistory] PRIMARY KEY CLUSTERED ([EmployeeCareerHistoryID] ASC),
    CONSTRAINT [FK_EmployeesCareersHistory_CareersDegrees] FOREIGN KEY ([CareerDegreeID]) REFERENCES [dbo].[CareersDegrees] ([CareerDegreeID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_EmployeesCareersHistory_CareersHistoryTypes] FOREIGN KEY ([CareerHistoryTypeID]) REFERENCES [dbo].[CareersHistoryTypes] ([CareerHistoryTypeID]),
    CONSTRAINT [FK_EmployeesCareersHistory_EmployeesCodes] FOREIGN KEY ([EmployeeCodeID]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmployeesCareersHistory_EmployeesCodes1] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_EmployeesCareersHistory_EmployeesCodes2] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_EmployeesCareersHistory_OrganizationsJobs] FOREIGN KEY ([OrganizationJobID]) REFERENCES [dbo].[OrganizationsJobs] ([OrganizationJobID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[EmployeesCareersHistory]([EmployeeCodeID] ASC, [OrganizationJobID] ASC);

