CREATE TABLE [dbo].[EmployeesQualifications] (
    [EmployeeQualificationID] INT             IDENTITY (1, 1) NOT NULL,
    [QualificationDegreeID]   INT             NULL,
    [QualificationID]         INT             NULL,
    [GeneralSpecializationID] INT             NULL,
    [ExactSpecializationID]   INT             NULL,
    [EmployeeCodeID]          INT             NULL,
    [UniSchName]              NVARCHAR (150)  NULL,
    [Department]              NVARCHAR (200)  NULL,
    [FullGPA]                 DECIMAL (18, 2) NULL,
    [GPA]                     DECIMAL (18, 2) NULL,
    [StudyPlace]              NVARCHAR (100)  NULL,
    [GraduationDate]          DATE            NULL,
    [AttachFile]              IMAGE           NULL,
    [Percentage]              DECIMAL (5, 2)  NULL,
    [QualificationTypeID]     INT             NULL,
    [GraduationYear]          NVARCHAR (4)    NULL,
    [CreatedDate]             DATETIME        CONSTRAINT [DF__Employees__Creat__0559BDD1] DEFAULT (getdate()) NULL,
    [CreatedBy]               INT             NULL,
    [LastUpdatedDate]         DATETIME        NULL,
    [LastUpdatedBy]           INT             NULL,
    CONSTRAINT [PK_EmployeesQualifications] PRIMARY KEY CLUSTERED ([EmployeeQualificationID] ASC),
    CONSTRAINT [FK_EmployeesQualifications_EmployeesCodes] FOREIGN KEY ([EmployeeCodeID]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmployeesQualifications_EmployeesCodes1] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_EmployeesQualifications_ExactSpecializations] FOREIGN KEY ([ExactSpecializationID]) REFERENCES [dbo].[ExactSpecializations] ([ExactSpecializationID]),
    CONSTRAINT [FK_EmployeesQualifications_GeneralSpecializations] FOREIGN KEY ([GeneralSpecializationID]) REFERENCES [dbo].[GeneralSpecializations] ([GeneralSpecializationID]),
    CONSTRAINT [FK_EmployeesQualifications_Qualifications] FOREIGN KEY ([QualificationID]) REFERENCES [dbo].[Qualifications] ([QualificationID]),
    CONSTRAINT [FK_EmployeesQualifications_QualificationsDegrees] FOREIGN KEY ([QualificationDegreeID]) REFERENCES [dbo].[QualificationsDegrees] ([QualificationDegreeID]),
    CONSTRAINT [FK_EmployeesQualifications_QualificationsTypes] FOREIGN KEY ([QualificationTypeID]) REFERENCES [dbo].[QualificationsTypes] ([QualificationTypeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[EmployeesQualifications]([QualificationDegreeID] ASC, [QualificationID] ASC, [GeneralSpecializationID] ASC, [EmployeeCodeID] ASC);

