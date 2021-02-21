CREATE TABLE [dbo].[DelegationsKinds] (
    [DelegationKindID]   INT           IDENTITY (1, 1) NOT NULL,
    [DelegationKindName] NVARCHAR (50) NOT NULL,
    [IsIncludeInventory] BIT           NULL,
    CONSTRAINT [PK_DelegationsKinds] PRIMARY KEY CLUSTERED ([DelegationKindID] ASC)
);

