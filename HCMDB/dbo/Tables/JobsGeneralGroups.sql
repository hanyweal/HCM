CREATE TABLE [dbo].[JobsGeneralGroups] (
    [JobGeneralGroupID]   INT           IDENTITY (1, 1) NOT NULL,
    [JobGeneralGroupName] NVARCHAR (50) NOT NULL,
    [CreatedDate]         DATETIME      CONSTRAINT [DF_JobsGeneralGroups_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]     DATETIME      NULL,
    [CreatedBy]           INT           NULL,
    [LastUpdatedBy]       INT           NULL,
    CONSTRAINT [PK_JobsGeneralGroups] PRIMARY KEY CLUSTERED ([JobGeneralGroupID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-JobGeneralGroupName]
    ON [dbo].[JobsGeneralGroups]([JobGeneralGroupName] ASC);

