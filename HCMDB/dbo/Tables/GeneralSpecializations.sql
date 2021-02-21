CREATE TABLE [dbo].[GeneralSpecializations] (
    [GeneralSpecializationID]   INT            IDENTITY (1, 1) NOT NULL,
    [QualificationID]           INT            NULL,
    [GeneralSpecializationName] NVARCHAR (300) NULL,
    [DirectPoints]              DECIMAL (5, 2) NULL,
    [IndirectPoints]            DECIMAL (5, 2) NULL,
    [CreatedDate]               DATETIME       CONSTRAINT [DF__GeneralSp__Creat__092A4EB5] DEFAULT (getdate()) NULL,
    [CreatedBy]                 INT            NULL,
    [LastUpdatedDate]           DATETIME       NULL,
    [LastUpdatedBy]             INT            NULL,
    CONSTRAINT [PK_GeneralSpecializations] PRIMARY KEY CLUSTERED ([GeneralSpecializationID] ASC),
    CONSTRAINT [FK_GeneralSpecializations_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_GeneralSpecializations_Qualifications] FOREIGN KEY ([QualificationID]) REFERENCES [dbo].[Qualifications] ([QualificationID]) ON DELETE CASCADE,
    CONSTRAINT [IX_GeneralSpecializations] UNIQUE NONCLUSTERED ([QualificationID] ASC, [GeneralSpecializationName] ASC)
);

