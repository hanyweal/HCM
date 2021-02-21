CREATE TABLE [dbo].[Branches] (
    [BranchID]   INT           IDENTITY (1, 1) NOT NULL,
    [BranchName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED ([BranchID] ASC)
);

