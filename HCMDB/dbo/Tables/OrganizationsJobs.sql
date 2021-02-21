CREATE TABLE [dbo].[OrganizationsJobs] (
    [OrganizationJobID]       INT           IDENTITY (1, 1) NOT NULL,
    [JobNo]                   NVARCHAR (50) NOT NULL,
    [OrganizationID]          INT           NOT NULL,
    [JobID]                   INT           NOT NULL,
    [RankID]                  INT           NOT NULL,
    [IsVacant]                BIT           NOT NULL,
    [CreatedDate]             DATETIME      CONSTRAINT [DF_OrganizationsJobs_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]         DATETIME      NULL,
    [JobKindID]               INT           NULL,
    [IsReserved]              BIT           NULL,
    [CreatedBy]               INT           NULL,
    [LastUpdatedBy]           INT           NULL,
    [OrganizationJobStatusID] INT           NULL,
    CONSTRAINT [PK_OrganizationsJobs] PRIMARY KEY CLUSTERED ([OrganizationJobID] ASC),
    CONSTRAINT [FK_OrganizationsJobs_Jobs] FOREIGN KEY ([JobID]) REFERENCES [dbo].[Jobs] ([JobID]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrganizationsJobs_JobsKinds] FOREIGN KEY ([JobKindID]) REFERENCES [dbo].[JobsKinds] ([JobKindID]),
    CONSTRAINT [FK_OrganizationsJobs_OrganizationsJobsStatus] FOREIGN KEY ([OrganizationJobStatusID]) REFERENCES [dbo].[OrganizationsJobsStatus] ([OrganizationJobStatusID]),
    CONSTRAINT [FK_OrganizationsJobs_OrganizationsStructures] FOREIGN KEY ([OrganizationID]) REFERENCES [dbo].[OrganizationsStructures] ([OrganizationID]),
    CONSTRAINT [FK_OrganizationsJobs_Ranks] FOREIGN KEY ([RankID]) REFERENCES [dbo].[Ranks] ([RankID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-JobNo-RankID]
    ON [dbo].[OrganizationsJobs]([JobNo] ASC, [RankID] ASC);

