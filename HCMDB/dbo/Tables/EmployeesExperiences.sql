CREATE TABLE [dbo].[EmployeesExperiences] (
    [EmployeeExperienceID] INT      IDENTITY (1, 1) NOT NULL,
    [EmployeeCodeID]       INT      NOT NULL,
    [TotalYears]           INT      NOT NULL,
    [TotalMonths]          INT      NOT NULL,
    [TotalDays]            INT      NOT NULL,
    [CreatedBy]            INT      NOT NULL,
    [CreatedDate]          DATETIME NULL,
    [LastUpdatedBy]        INT      NULL,
    [LastUpdatedDate]      DATETIME NULL,
    CONSTRAINT [PK_EmployeesExperiences] PRIMARY KEY CLUSTERED ([EmployeeExperienceID] ASC),
    CONSTRAINT [FK_EmployeesExperiences_EmployeesCodes] FOREIGN KEY ([EmployeeCodeID]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmployeesExperiences_EmployeesCodes1] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_EmployeesExperiences]
    ON [dbo].[EmployeesExperiences]([EmployeeCodeID] ASC);

