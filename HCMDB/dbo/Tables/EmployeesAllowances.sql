CREATE TABLE [dbo].[EmployeesAllowances] (
    [EmployeeAllowanceID]     INT      IDENTITY (1, 1) NOT NULL,
    [EmployeeCareerHistoryID] INT      NOT NULL,
    [AllowanceID]             INT      NOT NULL,
    [AllowanceStartDate]      DATE     NOT NULL,
    [IsActive]                BIT      NOT NULL,
    [CreatedDate]             DATETIME CONSTRAINT [DF_EmployeesAllowances_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]         DATETIME NULL,
    [CreatedBy]               INT      NULL,
    [LastUpdatedBy]           INT      NULL,
    CONSTRAINT [PK_EmployeesAllowances] PRIMARY KEY CLUSTERED ([EmployeeAllowanceID] ASC),
    CONSTRAINT [FK_EmployeesAllowances_Allowances] FOREIGN KEY ([AllowanceID]) REFERENCES [dbo].[Allowances] ([AllowanceID]),
    CONSTRAINT [FK_EmployeesAllowances_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_EmployeesAllowances_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_EmployeesAllowances_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_EmployeesAllowances]
    ON [dbo].[EmployeesAllowances]([AllowanceID] ASC, [EmployeeCareerHistoryID] ASC);

