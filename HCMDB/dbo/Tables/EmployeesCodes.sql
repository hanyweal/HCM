CREATE TABLE [dbo].[EmployeesCodes] (
    [EmployeeCodeID]  INT           IDENTITY (1, 1) NOT NULL,
    [EmployeeID]      INT           NOT NULL,
    [EmployeeCodeNo]  NVARCHAR (12) NOT NULL,
    [EmployeeTypeID]  INT           NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [CreatedDate]     DATETIME      CONSTRAINT [DF_EmployeesCodes_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]       INT           NULL,
    [LastUpdatedDate] DATETIME      NULL,
    [LastUpdatedBy]   INT           NULL,
    CONSTRAINT [PK_EmployeesCodes] PRIMARY KEY CLUSTERED ([EmployeeCodeID] ASC),
    CONSTRAINT [FK_EmployeesCodes_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmployeesCodes_EmployeesTypes] FOREIGN KEY ([EmployeeTypeID]) REFERENCES [dbo].[EmployeesTypes] ([EmployeeTypeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[EmployeesCodes]([EmployeeCodeNo] ASC);

