CREATE TABLE [dbo].[OrganizationsJobsStatus] (
    [OrganizationJobStatusID]   INT           IDENTITY (1, 1) NOT NULL,
    [OrganizationJobStatusName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_OrganizationsJobsStatus] PRIMARY KEY CLUSTERED ([OrganizationJobStatusID] ASC)
);

