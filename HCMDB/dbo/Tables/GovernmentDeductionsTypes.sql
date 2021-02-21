CREATE TABLE [dbo].[GovernmentDeductionsTypes] (
    [GovernmentDeductionTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [GovernmentDeductionTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GovernmentDeductionTypes] PRIMARY KEY CLUSTERED ([GovernmentDeductionTypeID] ASC)
);

