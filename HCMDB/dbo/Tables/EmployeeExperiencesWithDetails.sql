CREATE TABLE [dbo].[EmployeeExperiencesWithDetails](
	[EmployeeExperienceWithDetailID] [int] IDENTITY(1,1) NOT NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[SectorTypeID] [int] NOT NULL,
	[SectorName] [nvarchar](100) NULL,
	[JobName] [nvarchar](50) NULL,
	[EmployeeCodeID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[LastUpdatedBy] [int] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_EmployeeExperiencesWithDetails] PRIMARY KEY CLUSTERED 
(
	[EmployeeExperienceWithDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmployeeExperiencesWithDetails]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeExperiencesWithDetails_EmployeesCodes] FOREIGN KEY([EmployeeCodeID])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EmployeeExperiencesWithDetails] CHECK CONSTRAINT [FK_EmployeeExperiencesWithDetails_EmployeesCodes]
GO

ALTER TABLE [dbo].[EmployeeExperiencesWithDetails]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeExperiencesWithDetails_EmployeesCodes1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EmployeeExperiencesWithDetails] CHECK CONSTRAINT [FK_EmployeeExperiencesWithDetails_EmployeesCodes1]
GO

ALTER TABLE [dbo].[EmployeeExperiencesWithDetails]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeExperiencesWithDetails_SectorsTypes] FOREIGN KEY([SectorTypeID])
REFERENCES [dbo].[SectorsTypes] ([SectorTypeID])
GO

ALTER TABLE [dbo].[EmployeeExperiencesWithDetails] CHECK CONSTRAINT [FK_EmployeeExperiencesWithDetails_SectorsTypes]
GO

