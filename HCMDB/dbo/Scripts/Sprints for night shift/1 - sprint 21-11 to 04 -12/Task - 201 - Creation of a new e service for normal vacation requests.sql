

CREATE TABLE [dbo].[EVacationRequestsStatus](
	[EVacationRequestStatusID] [int] NOT NULL,
	[EVacationRequestStatusName] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_EVacationRequestsStatus] PRIMARY KEY CLUSTERED 
(
	[EVacationRequestStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[EVacationsRequests](
	[EVacationRequestID] [int] IDENTITY(1,1) NOT NULL,
	[EVacationRequestNo] [int] NOT NULL,
	[EmployeeCareerHistoryID] [int] NOT NULL,
	[VacationStartDate] [date] NOT NULL,
	[VacationEndDate] [date] NOT NULL,
	[VacationTypeID] [int] NOT NULL,
	[OldBalanceConsumed] [int] NOT NULL,
	[CreatorNotes] [nvarchar](max) NULL,
	[EVacationRequestStatusID] [int] NULL,
	[ApprovalDateTime] [datetime] NULL,
	[ApprovedBy] [int] NULL,
	[ApproverNotes] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[LastUpdatedDate] [datetime] NULL,
	[LastUpdatedBy] [int] NULL,
 CONSTRAINT [PK_EVacationsRequests] PRIMARY KEY CLUSTERED 
(
	[EVacationRequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[EVacationsRequests]  WITH CHECK ADD  CONSTRAINT [FK_EVacationsRequests_EmployeesCareersHistory] FOREIGN KEY([EmployeeCareerHistoryID])
REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID])
GO

ALTER TABLE [dbo].[EVacationsRequests] CHECK CONSTRAINT [FK_EVacationsRequests_EmployeesCareersHistory]
GO

ALTER TABLE [dbo].[EVacationsRequests]  WITH CHECK ADD  CONSTRAINT [FK_EVacationsRequests_EmployeesCodes] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EVacationsRequests] CHECK CONSTRAINT [FK_EVacationsRequests_EmployeesCodes]
GO

ALTER TABLE [dbo].[EVacationsRequests]  WITH CHECK ADD  CONSTRAINT [FK_EVacationsRequests_EmployeesCodes1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EVacationsRequests] CHECK CONSTRAINT [FK_EVacationsRequests_EmployeesCodes1]
GO

ALTER TABLE [dbo].[EVacationsRequests]  WITH CHECK ADD  CONSTRAINT [FK_EVacationsRequests_EmployeesCodes2] FOREIGN KEY([LastUpdatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EVacationsRequests] CHECK CONSTRAINT [FK_EVacationsRequests_EmployeesCodes2]
GO

ALTER TABLE [dbo].[EVacationsRequests]  WITH CHECK ADD  CONSTRAINT [FK_EVacationsRequests_EVacationRequestsStatus] FOREIGN KEY([EVacationRequestStatusID])
REFERENCES [dbo].[EVacationRequestsStatus] ([EVacationRequestStatusID])
GO

ALTER TABLE [dbo].[EVacationsRequests] CHECK CONSTRAINT [FK_EVacationsRequests_EVacationRequestsStatus]
GO

ALTER TABLE [dbo].[EVacationsRequests]  WITH CHECK ADD CONSTRAINT [FK_EVacationsRequests_VacationsTypes] FOREIGN KEY([VacationTypeID])
REFERENCES [dbo].[VacationsTypes] ([VacationTypeID])
GO

ALTER TABLE [dbo].[EVacationsRequests] CHECK CONSTRAINT [FK_EVacationsRequests_VacationsTypes]
GO

INSERT INTO [dbo].[EVacationRequestsStatus]
           ([EVacationRequestStatusID]
           ,[EVacationRequestStatusName])
     VALUES
           (1,
           N'إنتظار موافقة صاحب الصلاحية')
GO

INSERT INTO [dbo].[EVacationRequestsStatus]
           ([EVacationRequestStatusID]
           ,[EVacationRequestStatusName])
     VALUES
           (2,
           N'موافقة')
GO

INSERT INTO [dbo].[EVacationRequestsStatus]
           ([EVacationRequestStatusID]
           ,[EVacationRequestStatusName])
     VALUES
           (3,
           N'رفض')
GO

INSERT INTO [dbo].[EVacationRequestsStatus]
           ([EVacationRequestStatusID]
           ,[EVacationRequestStatusName])
     VALUES
           (4,
           N'ملغاة')
GO

ALTER TABLE VacationsTypes ADD IsPossibleToBeCreatedFromEVacationRequest BIT NULL
GO

UPDATE VacationsTypes
SET IsPossibleToBeCreatedFromEVacationRequest = 1
WHERE VacationTypeID = 1
GO

ALTER TABLE OrganizationsStructures ADD AuthorizedToApproveOnEVacationsRequests INT NULL
GO

ALTER TABLE [dbo].OrganizationsStructures  WITH CHECK ADD CONSTRAINT [FK_[AuthorizedToApproveOnEVacationsRequests_OrganizationsStructures] FOREIGN KEY([AuthorizedToApproveOnEVacationsRequests])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

CREATE TABLE [dbo].[EServicesTypes](
	[EServiceTypeID] [int] NOT NULL,
	[EServiceTypeName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_EServicesTypes] PRIMARY KEY CLUSTERED 
(
	[EServiceTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [dbo].[EServicesTypes]
           ([EServiceTypeID]
           ,[EServiceTypeName])
     VALUES
           (1
           ,N'الإجازات')
GO

INSERT INTO [dbo].[EServicesTypes]
           ([EServiceTypeID]
           ,[EServiceTypeName])
     VALUES
           (1
           ,N'التقييم')
GO

CREATE TABLE [dbo].[EServicesAuthorizations](
	[EServiceAuthorizationID] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [int] NOT NULL,
	[EServiceTypeID] [int] NOT NULL,
	[AuthorizedPersonCodeID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastUpdatedBy] [int] NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_EServicesAuthorizations] PRIMARY KEY CLUSTERED 
(
	[EServiceAuthorizationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EServicesAuthorizations]  WITH CHECK ADD  CONSTRAINT [FK_EServicesAuthorizations_EmployeesCodes] FOREIGN KEY([AuthorizedPersonCodeID])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EServicesAuthorizations] CHECK CONSTRAINT [FK_EServicesAuthorizations_EmployeesCodes]
GO

ALTER TABLE [dbo].[EServicesAuthorizations]  WITH CHECK ADD  CONSTRAINT [FK_EServicesAuthorizations_EmployeesCodes1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EServicesAuthorizations] CHECK CONSTRAINT [FK_EServicesAuthorizations_EmployeesCodes1]
GO

ALTER TABLE [dbo].[EServicesAuthorizations]  WITH CHECK ADD  CONSTRAINT [FK_EServicesAuthorizations_EmployeesCodes2] FOREIGN KEY([LastUpdatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[EServicesAuthorizations] CHECK CONSTRAINT [FK_EServicesAuthorizations_EmployeesCodes2]
GO

ALTER TABLE [dbo].[EServicesAuthorizations]  WITH CHECK ADD  CONSTRAINT [FK_EServicesAuthorizations_EServicesTypes] FOREIGN KEY([EServiceTypeID])
REFERENCES [dbo].[EServicesTypes] ([EServiceTypeID])
GO

ALTER TABLE [dbo].[EServicesAuthorizations] CHECK CONSTRAINT [FK_EServicesAuthorizations_EServicesTypes]
GO

ALTER TABLE [dbo].[EServicesAuthorizations]  WITH CHECK ADD  CONSTRAINT [FK_EServicesAuthorizations_OrganizationsStructures] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[OrganizationsStructures] ([OrganizationID])
GO

ALTER TABLE [dbo].[EServicesAuthorizations] CHECK CONSTRAINT [FK_EServicesAuthorizations_OrganizationsStructures]
GO

CREATE UNIQUE NONCLUSTERED INDEX [Unique] ON [dbo].[EServicesAuthorizations]
(
	[OrganizationID] ASC,
	[EServiceTypeID] ASC,
	[AuthorizedPersonCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

INSERT INTO EServicesAuthorizations(OrganizationID,EServiceTypeID,AuthorizedPersonCodeID,CreatedBy,CreatedDate)
SELECT OrganizationID,1,19548,22915,GETDATE()
FROM OrganizationsStructures
GO