CREATE TABLE [dbo].[DelegationsDetails] (
    [DelegationDetailID]           INT      IDENTITY (1, 1) NOT NULL,
    [DelegationID]                 INT      NOT NULL,
    [EmployeeCareerHistoryID]      INT      NOT NULL,
    [CreatedDate]                  DATETIME CONSTRAINT [DF_DelegationsDetails_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]              DATETIME NULL,
    [CreatedBy]                    INT      NULL,
    [LastUpdatedBy]                INT      NULL,
    [IsPassengerOrderCompensation] BIT      CONSTRAINT [DF_DelegationsDetails_IsPassengerOrderCompensation] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DelegationsDetails] PRIMARY KEY CLUSTERED ([DelegationDetailID] ASC),
    CONSTRAINT [FK_DelegationsDetails_Delegations] FOREIGN KEY ([DelegationID]) REFERENCES [dbo].[Delegations] ([DelegationID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_DelegationsDetails_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID])
);

