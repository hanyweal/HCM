CREATE TABLE [dbo].[Jobs] (
    [JobID]           INT            IDENTITY (1, 1) NOT NULL,
    [JobCategoryID]   INT            NOT NULL,
    [JobCode]         NVARCHAR (20)  NULL,
    [JobName]         NVARCHAR (150) NOT NULL,
    [CreatedDate]     DATETIME       CONSTRAINT [DF_Jobs_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate] DATETIME       NULL,
    [LastUpdatedBy]   INT            NULL,
    [CreatedBy]       INT            NULL,
    CONSTRAINT [PK_Jobs] PRIMARY KEY CLUSTERED ([JobID] ASC),
    CONSTRAINT [FK_Jobs_JobsCategories] FOREIGN KEY ([JobCategoryID]) REFERENCES [dbo].[JobsCategories] ([JobCategoryID]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Jobs]
    ON [dbo].[Jobs]([JobCategoryID] ASC, [JobName] ASC);

