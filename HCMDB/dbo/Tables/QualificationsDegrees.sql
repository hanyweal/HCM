CREATE TABLE [dbo].[QualificationsDegrees] (
    [QualificationDegreeID]   INT            IDENTITY (1, 1) NOT NULL,
    [QualificationDegreeName] NVARCHAR (100) NULL,
    [Weight]                  INT            NULL,
    [DirectPoints]            DECIMAL (5, 2) NULL,
    [IndirectPoints]          DECIMAL (5, 2) NULL,
    [CreatedDate]             DATETIME       NULL,
    [CreatedBy]               INT            NULL,
    [LastUpdatedDate]         DATETIME       NULL,
    [LastUpdatedBy]           INT            NULL,
    CONSTRAINT [PK_QualificationsDegrees] PRIMARY KEY CLUSTERED ([QualificationDegreeID] ASC),
    CONSTRAINT [FK_QualificationsDegrees_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [IX_QualificationsDegrees] UNIQUE NONCLUSTERED ([QualificationDegreeName] ASC)
);

