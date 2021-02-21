CREATE TABLE [dbo].[ExactSpecializations] (
    [ExactSpecializationID]   INT            IDENTITY (1, 1) NOT NULL,
    [GeneralSpecializationID] INT            NULL,
    [ExactSpecializationName] NVARCHAR (300) NULL,
    [CreatedDate]             DATETIME       CONSTRAINT [DF__ExactSpec__Creat__07420643] DEFAULT (getdate()) NULL,
    [CreatedBy]               INT            NULL,
    [LastUpdatedDate]         DATETIME       NULL,
    [LastUpdatedBy]           INT            NULL,
    CONSTRAINT [PK_ExactSpecializations] PRIMARY KEY CLUSTERED ([ExactSpecializationID] ASC),
    CONSTRAINT [FK_ExactSpecializations_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_ExactSpecializations_GeneralSpecializations] FOREIGN KEY ([GeneralSpecializationID]) REFERENCES [dbo].[GeneralSpecializations] ([GeneralSpecializationID]) ON DELETE CASCADE,
    CONSTRAINT [IX_ExactSpecializations] UNIQUE NONCLUSTERED ([ExactSpecializationName] ASC, [GeneralSpecializationID] ASC)
);

