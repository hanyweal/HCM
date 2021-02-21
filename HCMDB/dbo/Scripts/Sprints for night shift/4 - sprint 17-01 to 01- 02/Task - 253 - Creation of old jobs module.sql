GO

CREATE TABLE [dbo].[EmployeesOldJobs](
	[EmployeeOldJobID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeCodeID] [int] NOT NULL,
	[JobName] [nvarchar](500) NULL,
	[OrganizationName] [nvarchar](500) NULL,
	[RankName] [nvarchar](500) NULL,
	[CareerDegreeName] [nvarchar](500) NULL,
	[EmployeeOldJobStartDate] [date] NULL,
	[EmployeeOldJobEndDate] [date] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastUpdatedBy] [int] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_EmployeesOldJobs] PRIMARY KEY CLUSTERED 
(
	[EmployeeOldJobID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmployeesOldJobs] ADD  CONSTRAINT [DF_EmployeesOldJobs_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[EmployeesOldJobs]  WITH CHECK ADD  CONSTRAINT [FK_EmployeesOldJobs_EmployeesCodes] FOREIGN KEY([EmployeeCodeID])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EmployeesOldJobs] CHECK CONSTRAINT [FK_EmployeesOldJobs_EmployeesCodes]
GO

ALTER TABLE [dbo].[EmployeesOldJobs]  WITH CHECK ADD  CONSTRAINT [FK_EmployeesOldJobs_EmployeesCodes1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EmployeesOldJobs] CHECK CONSTRAINT [FK_EmployeesOldJobs_EmployeesCodes1]
GO

ALTER TABLE [dbo].[EmployeesOldJobs]  WITH CHECK ADD  CONSTRAINT [FK_EmployeesOldJobs_EmployeesCodes2] FOREIGN KEY([LastUpdatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EmployeesOldJobs] CHECK CONSTRAINT [FK_EmployeesOldJobs_EmployeesCodes2]
GO

