CREATE TABLE [dbo].[EmployeesEvaluations] (
    [EmployeeEvaluationID] INT        IDENTITY (1, 1) NOT NULL,
    [MaturityYearID]       INT        NOT NULL,
    [EvaluationPointID]    INT        NOT NULL,
    [EvaluationDegree]     FLOAT (53) NOT NULL,
    [EmployeeCodeID]       INT        NOT NULL,
    [CreatedBy]            INT        NULL,
    [CreatedDate]          DATETIME   NULL,
    [LastUpdatedBy]        INT        NULL,
    [LastUpdatedDate]      DATETIME   NULL,
    CONSTRAINT [PK_EmployeesEvaluations] PRIMARY KEY CLUSTERED ([EmployeeEvaluationID] ASC),
    CONSTRAINT [FK_EmployeesEvaluations_EmployeesCodes] FOREIGN KEY ([EmployeeCodeID]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmployeesEvaluations_EmployeesCodes1] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_EmployeesEvaluations_EvaluationPoints] FOREIGN KEY ([EvaluationPointID]) REFERENCES [dbo].[EvaluationPoints] ([EvaluationPointID]),
    CONSTRAINT [FK_EmployeesEvaluations_MaturityYearsBalances] FOREIGN KEY ([MaturityYearID]) REFERENCES [dbo].[MaturityYearsBalances] ([MaturityYearID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[EmployeesEvaluations]([MaturityYearID] ASC, [EmployeeCodeID] ASC);

