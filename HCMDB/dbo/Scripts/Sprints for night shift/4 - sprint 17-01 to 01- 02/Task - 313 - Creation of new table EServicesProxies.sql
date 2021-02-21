GO
/****** Object:  Table [dbo].[EServicesProxies]    Script Date: 08/06/42 08:15:43 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EServicesProxies](
	[EServiceProxyID] [int] IDENTITY(1,1) NOT NULL,
	[EServiceTypeID] [int] NOT NULL,
	[FromEmployeeCodeID] [int] NOT NULL,
	[ToEmployeeCodeID] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NULL,
	[EServiceProxyStatusID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[LastUpdatedDate] [datetime] NULL,
	[LastUpdatedBy] [int] NULL,
 CONSTRAINT [PK_EServicesProxies] PRIMARY KEY CLUSTERED 
(
	[EServiceProxyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EServicesProxiesStatus]    Script Date: 08/06/42 08:15:44 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EServicesProxiesStatus](
	[EServiceProxyStatusID] [int] NOT NULL,
	[EServiceProxyStatus] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_EServicesProxiesStatus] PRIMARY KEY CLUSTERED 
(
	[EServiceProxyStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EServicesProxies] ADD  CONSTRAINT [DF_EServicesProxies_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[EServicesProxies]  WITH CHECK ADD  CONSTRAINT [FK_EServicesProxies_EmployeesCodes] FOREIGN KEY([FromEmployeeCodeID])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO
ALTER TABLE [dbo].[EServicesProxies] CHECK CONSTRAINT [FK_EServicesProxies_EmployeesCodes]
GO
ALTER TABLE [dbo].[EServicesProxies]  WITH CHECK ADD  CONSTRAINT [FK_EServicesProxies_EmployeesCodes1] FOREIGN KEY([ToEmployeeCodeID])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO
ALTER TABLE [dbo].[EServicesProxies] CHECK CONSTRAINT [FK_EServicesProxies_EmployeesCodes1]
GO
ALTER TABLE [dbo].[EServicesProxies]  WITH CHECK ADD  CONSTRAINT [FK_EServicesProxies_EmployeesCodes2] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO
ALTER TABLE [dbo].[EServicesProxies] CHECK CONSTRAINT [FK_EServicesProxies_EmployeesCodes2]
GO
ALTER TABLE [dbo].[EServicesProxies]  WITH CHECK ADD  CONSTRAINT [FK_EServicesProxies_EmployeesCodes3] FOREIGN KEY([LastUpdatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO
ALTER TABLE [dbo].[EServicesProxies] CHECK CONSTRAINT [FK_EServicesProxies_EmployeesCodes3]
GO
ALTER TABLE [dbo].[EServicesProxies]  WITH CHECK ADD  CONSTRAINT [FK_EServicesProxies_EServicesProxiesStatus] FOREIGN KEY([EServiceProxyStatusID])
REFERENCES [dbo].[EServicesProxiesStatus] ([EServiceProxyStatusID])
GO
ALTER TABLE [dbo].[EServicesProxies] CHECK CONSTRAINT [FK_EServicesProxies_EServicesProxiesStatus]
GO
ALTER TABLE [dbo].[EServicesProxies]  WITH CHECK ADD  CONSTRAINT [FK_EServicesProxies_EServicesTypes] FOREIGN KEY([EServiceTypeID])
REFERENCES [dbo].[EServicesTypes] ([EServiceTypeID])
GO
ALTER TABLE [dbo].[EServicesProxies] CHECK CONSTRAINT [FK_EServicesProxies_EServicesTypes]
GO


GO
INSERT [dbo].[EServicesProxiesStatus] ([EServiceProxyStatusID], [EServiceProxyStatus]) VALUES (1, N'Valid')
GO
INSERT [dbo].[EServicesProxiesStatus] ([EServiceProxyStatusID], [EServiceProxyStatus]) VALUES (2, N'Expired')
GO
INSERT [dbo].[EServicesProxiesStatus] ([EServiceProxyStatusID], [EServiceProxyStatus]) VALUES (3, N'Cancelled By System')
GO


INSERT INTO BusinessCategories(BusinessCategoryID, BusinessCategoryAr, BusinessCategoryEn) VALUES(11, N'EServices', N'EServices')
Go
INSERT INTO BusinessSubCategories(BusinssSubCategoryID, BusinessSubCategoryAr, BusinessSubCategoryEn, BusinessCategoryID, EntityName) 
	VALUES(33, N'EServices Proxies', N'EServices Proxies', 11, 'EServicesProxiesDAL')