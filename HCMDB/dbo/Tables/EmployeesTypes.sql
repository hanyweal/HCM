CREATE TABLE [dbo].[EmployeesTypes] (
    [EmployeeTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [EmployeeTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EmployeesTypes] PRIMARY KEY CLUSTERED ([EmployeeTypeID] ASC)
);

