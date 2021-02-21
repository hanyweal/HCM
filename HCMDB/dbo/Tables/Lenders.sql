CREATE TABLE [dbo].[Lenders] (
    [LenderID]           INT            IDENTITY (1, 1) NOT NULL,
    [LenderStartDate]    DATE           NOT NULL,
    [LenderEndDate]      DATE           NOT NULL,
    [KSACityID]          INT            NULL,
    [LenderOrganization] NVARCHAR (MAX) NOT NULL,
    [EmployeeCodeID]     INT            NOT NULL,
    [IsFinished]         BIT            NOT NULL,
    [CreatedDate]        DATETIME       NOT NULL,
    [CreatedBy]          INT            NULL,
    [LastUpdatedDate]    DATETIME       NULL,
    [LastUpdatedBy]      INT            NULL,
    CONSTRAINT [PK_Lenders] PRIMARY KEY CLUSTERED ([LenderID] ASC),
    CONSTRAINT [FK_Lenders_EmployeesCodes] FOREIGN KEY ([EmployeeCodeID]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Lenders_EmployeesCodes1] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_Lenders_EmployeesCodes2] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_Lenders_KSACities] FOREIGN KEY ([KSACityID]) REFERENCES [dbo].[KSACities] ([KSACityID])
);

