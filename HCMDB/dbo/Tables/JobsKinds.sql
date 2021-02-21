CREATE TABLE [dbo].[JobsKinds] (
    [JobKindID]   INT           NOT NULL,
    [JobKindName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_JobsKinds] PRIMARY KEY CLUSTERED ([JobKindID] ASC)
);

