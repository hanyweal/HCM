CREATE TABLE [dbo].[Transfers] (
    [TransferID]              INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeCareerHistoryID] INT            NOT NULL,
    [TransferTypeID]          INT            NOT NULL,
    [TransferDate]            DATE           NOT NULL,
    [Destination]             NVARCHAR (150) NOT NULL,
    [KSACityID]               INT            NOT NULL,
    [JobName]                 NVARCHAR (150) NULL,
    [RankName]                NVARCHAR (150) NULL,
    [JobCode]                 NVARCHAR (150) NULL,
    [OrganizationName]        NVARCHAR (150) NULL,
    [CareerDegreeName]        NVARCHAR (150) NULL,
    [CreatedDate]             DATETIME       CONSTRAINT [DF_EmployeesTransfers_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]         DATETIME       NULL,
    CONSTRAINT [PK_EmployeesTransfers_1] PRIMARY KEY CLUSTERED ([TransferID] ASC),
    CONSTRAINT [FK_EmployeesTransfers_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_EmployeesTransfers_TransfersTypes] FOREIGN KEY ([TransferTypeID]) REFERENCES [dbo].[TransfersTypes] ([TransferTypeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[Transfers]([EmployeeCareerHistoryID] ASC);

