CREATE TABLE [dbo].[AssigningsTypes] (
    [AssigningTypID]    INT           IDENTITY (1, 1) NOT NULL,
    [AssigningTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AssigningTypes] PRIMARY KEY CLUSTERED ([AssigningTypID] ASC)
);

