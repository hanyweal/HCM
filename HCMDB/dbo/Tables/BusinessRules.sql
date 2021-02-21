CREATE TABLE [dbo].[BusinessRules] (
    [BusinessRuleID]        INT            NOT NULL,
    [BusinessRuleAr]        NVARCHAR (MAX) NOT NULL,
    [BusinessRuleEn]        NVARCHAR (MAX) NULL,
    [IsActive]              BIT            NOT NULL,
    [BusinessSubCategoryID] INT            NOT NULL,
    CONSTRAINT [PK_BusinessConditions] PRIMARY KEY CLUSTERED ([BusinessRuleID] ASC)
);

