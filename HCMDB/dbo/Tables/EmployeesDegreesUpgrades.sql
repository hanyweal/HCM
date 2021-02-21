CREATE TABLE [dbo].[EmployeesDegreesUpgrades] (
    [EmployeeDegreeUpgradeID]   INT      IDENTITY (1, 1) NOT NULL,
    [EmployeeDegreeUpgradeYear] INT      NOT NULL,
    [CreatedBy]                 INT      NOT NULL,
    [CreatedDate]               DATETIME CONSTRAINT [DF_EmployeesDegreesUpgrades_CreatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_EmployeesDegreesUpgrades] PRIMARY KEY CLUSTERED ([EmployeeDegreeUpgradeID] ASC),
    CONSTRAINT [FK_EmployeesDegreesUpgrades_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_EmployeesDegreesUpgrades]
    ON [dbo].[EmployeesDegreesUpgrades]([EmployeeDegreeUpgradeYear] ASC);

