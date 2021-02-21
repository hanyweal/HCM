CREATE TABLE [dbo].[AllowancesAmountTypes] (
    [AllowanceAmountTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [AllowanceAmountTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AllowancesAmountTypes] PRIMARY KEY CLUSTERED ([AllowanceAmountTypeID] ASC)
);

