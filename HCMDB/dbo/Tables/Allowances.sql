CREATE TABLE [dbo].[Allowances] (
    [AllowanceID]                INT            IDENTITY (1, 1) NOT NULL,
    [AllowanceName]              NVARCHAR (150) NOT NULL,
    [AllowanceAmountTypeID]      INT            NOT NULL,
    [AllowanceCalculationTypeID] INT            NULL,
    [AllowanceAmount]            FLOAT (53)     NOT NULL,
    [IsActive]                   BIT            NOT NULL,
    [CreatedDate]                DATETIME       CONSTRAINT [DF_Allowances_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]            DATETIME       NULL,
    CONSTRAINT [PK_Allowances] PRIMARY KEY CLUSTERED ([AllowanceID] ASC),
    CONSTRAINT [FK_Allowances_AllowancesAmountTypes] FOREIGN KEY ([AllowanceAmountTypeID]) REFERENCES [dbo].[AllowancesAmountTypes] ([AllowanceAmountTypeID]),
    CONSTRAINT [FK_Allowances_AllwoancesCalcualtionTypes] FOREIGN KEY ([AllowanceCalculationTypeID]) REFERENCES [dbo].[AllowancesCalculationTypes] ([AllowanceCalculationTypeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Allowances]
    ON [dbo].[Allowances]([AllowanceName] ASC);

