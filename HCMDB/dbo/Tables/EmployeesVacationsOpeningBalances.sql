CREATE TABLE [dbo].[EmployeesVacationsOpeningBalances] (
    [EmployeeVacationOpeningBalanceID] INT IDENTITY (1, 1) NOT NULL,
    [EmployeeCodeID]                   INT NOT NULL,
    [VacationTypeID]                   INT NOT NULL,
    [OpeningBalance]                   INT NOT NULL,
    CONSTRAINT [PK_EmployeesVacationsOpeningBalances] PRIMARY KEY CLUSTERED ([EmployeeVacationOpeningBalanceID] ASC),
    CONSTRAINT [FK_EmployeesVacationsOpeningBalances_EmployeesCodes] FOREIGN KEY ([EmployeeCodeID]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmployeesVacationsOpeningBalances_VacationsTypes] FOREIGN KEY ([VacationTypeID]) REFERENCES [dbo].[VacationsTypes] ([VacationTypeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-EmployeeCode-VacationType]
    ON [dbo].[EmployeesVacationsOpeningBalances]([EmployeeCodeID] ASC, [VacationTypeID] ASC);

