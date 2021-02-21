CREATE TABLE [dbo].[Qualifications] (
    [QualificationID]       INT            IDENTITY (1, 1) NOT NULL,
    [QualificationDegreeID] INT            NULL,
    [QualificationName]     NVARCHAR (400) NULL,
    [DirectPoints]          DECIMAL (5, 2) NULL,
    [IndirectPoints]        DECIMAL (5, 2) NULL,
    [CreatedDate]           DATETIME       NULL,
    [CreatedBy]             INT            NULL,
    [LastUpdatedDate]       DATETIME       NULL,
    [LastUpdatedBy]         INT            NULL,
    CONSTRAINT [PK_Qualifications] PRIMARY KEY CLUSTERED ([QualificationID] ASC),
    CONSTRAINT [FK_Qualifications_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_Qualifications_QualificationsDegrees] FOREIGN KEY ([QualificationDegreeID]) REFERENCES [dbo].[QualificationsDegrees] ([QualificationDegreeID]) ON DELETE CASCADE,
    CONSTRAINT [IX_Qualifications] UNIQUE NONCLUSTERED ([QualificationDegreeID] ASC, [QualificationName] ASC)
);

