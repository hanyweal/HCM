
CREATE TABLE [dbo].[OrganizationsManagers](
	[OrganizationManagerID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[ManagerCodeID] [int] NOT NULL,
	[FromDate] [date] NOT NULL,
	[ToDate] [date] NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastUpdatedBy] [int] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_OrganizationsManagers] PRIMARY KEY CLUSTERED 
(
	[OrganizationManagerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrganizationsManagers]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationsManagers_EmployeesCodes1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[OrganizationsManagers] CHECK CONSTRAINT [FK_OrganizationsManagers_EmployeesCodes1]
GO

ALTER TABLE [dbo].[OrganizationsManagers]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationsManagers_EmployeesCodes2] FOREIGN KEY([LastUpdatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[OrganizationsManagers] CHECK CONSTRAINT [FK_OrganizationsManagers_EmployeesCodes2]
GO
