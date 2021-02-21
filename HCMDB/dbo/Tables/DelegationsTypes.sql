CREATE TABLE [dbo].[DelegationsTypes] (
    [DelegationTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [DelegationTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DelegationsTypes] PRIMARY KEY CLUSTERED ([DelegationTypeID] ASC)
);

