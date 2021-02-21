CREATE TABLE [dbo].[JobsGroups] (
    [JobGroupID]        INT            IDENTITY (1, 1) NOT NULL,
    [JobGeneralGroupID] INT            NOT NULL,
    [JobGroupName]      NVARCHAR (200) NOT NULL,
    [CreatedDate]       DATETIME       CONSTRAINT [DF_JobsGroups_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]   DATETIME       NULL,
    [LastUpdatedBy]     INT            NULL,
    [CreatedBy]         INT            NULL,
    CONSTRAINT [PK_JobsGroups] PRIMARY KEY CLUSTERED ([JobGroupID] ASC),
    CONSTRAINT [FK_JobsGroups_JobsGeneralGroups] FOREIGN KEY ([JobGeneralGroupID]) REFERENCES [dbo].[JobsGeneralGroups] ([JobGeneralGroupID]) ON DELETE CASCADE
);

