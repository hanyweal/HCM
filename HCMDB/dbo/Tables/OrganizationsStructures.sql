CREATE TABLE [dbo].[OrganizationsStructures] (
    [OrganizationID]       INT            IDENTITY (1, 1) NOT NULL,
    [OrganizationCode]     NVARCHAR (50)  NOT NULL,
    [OrganizationName]     NVARCHAR (150) NOT NULL,
    [ManagerCodeID]        INT            NULL,
    [OrganizationParentID] INT            NULL,
    [BranchID]             INT            NULL,
    [CreatedDate]          DATETIME       CONSTRAINT [DF_OrganizationsStructures_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]      DATETIME       NULL,
    [CreatedBy]            INT            NULL,
    [LastUpdatedBy]        INT            NULL,
    [ShowInECM]            BIT            NULL,
    CONSTRAINT [PK_OrganizationsStructures] PRIMARY KEY CLUSTERED ([OrganizationID] ASC),
    CONSTRAINT [FK_OrganizationsStructures_Branches] FOREIGN KEY ([BranchID]) REFERENCES [dbo].[Branches] ([BranchID]),
    CONSTRAINT [FK_OrganizationsStructures_EmployeesCodes] FOREIGN KEY ([ManagerCodeID]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_OrganizationsStructures_EmployeesCodes1] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_OrganizationsStructures_EmployeesCodes2] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_OrganizationsStructures_OrganizationsStructures] FOREIGN KEY ([OrganizationParentID]) REFERENCES [dbo].[OrganizationsStructures] ([OrganizationID])
);

