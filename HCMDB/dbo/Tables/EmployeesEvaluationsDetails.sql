CREATE TABLE [dbo].[EmployeesEvaluationsDetails] (
    [EmployeeEvaluationDetailID] INT             IDENTITY (1, 1) NOT NULL,
    [EvaluationQuarterID]        INT             NOT NULL,
    [DirectManagerEvaluation]    DECIMAL (18, 2) NOT NULL,
    [TimeAttendanceEvaluation]   DECIMAL (18, 2) NOT NULL,
    [ViolationsEvaluation]       DECIMAL (18, 2) NOT NULL,
    [EmployeeEvaluationID]       INT             NOT NULL,
    CONSTRAINT [PK_EmployeesEvaluationsDetails] PRIMARY KEY CLUSTERED ([EmployeeEvaluationDetailID] ASC),
    CONSTRAINT [FK_EmployeesEvaluationsDetails_EmployeesEvaluations] FOREIGN KEY ([EmployeeEvaluationID]) REFERENCES [dbo].[EmployeesEvaluations] ([EmployeeEvaluationID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_EmployeesEvaluationsDetails_EvaluationsQuarters] FOREIGN KEY ([EvaluationQuarterID]) REFERENCES [dbo].[EvaluationsQuarters] ([EvaluationQuarterID])
);

