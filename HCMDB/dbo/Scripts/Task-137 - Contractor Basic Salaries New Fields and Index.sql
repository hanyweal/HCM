USE [HCM]
GO

/****** Object:  Table [dbo].[ContractorsBasicSalaries]    Script Date: 8/24/2020 02:26:34 م ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[ContractorsBasicSalaries]

GO

CREATE TABLE [dbo].[ContractorsBasicSalaries](
	[ContractorBasicSalaryID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeCodeID] [int] NOT NULL,
	[BasicSalary] [float] NOT NULL,
	[TransfareAllowance] [float] NULL,
	[CreatedDate] [datetime] NULL,
	[LastUpdatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
 CONSTRAINT [PK_ContractorsBasicSalaries] PRIMARY KEY CLUSTERED 
(
	[ContractorBasicSalaryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ContractorsBasicSalaries] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[ContractorsBasicSalaries]  WITH CHECK ADD  CONSTRAINT [FK_ContractorsBasicSalaries_EmployeesCodes] FOREIGN KEY([EmployeeCodeID])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[ContractorsBasicSalaries] CHECK CONSTRAINT [FK_ContractorsBasicSalaries_EmployeesCodes]
GO

ALTER TABLE [dbo].[ContractorsBasicSalaries]  WITH CHECK ADD  CONSTRAINT [FK_ContractorsBasicSalaries_EmployeesCodes1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[ContractorsBasicSalaries] CHECK CONSTRAINT [FK_ContractorsBasicSalaries_EmployeesCodes1]
GO

ALTER TABLE [dbo].[ContractorsBasicSalaries]  WITH CHECK ADD  CONSTRAINT [FK_ContractorsBasicSalaries_EmployeesCodes2] FOREIGN KEY([LastUpdatedBy])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[ContractorsBasicSalaries] CHECK CONSTRAINT [FK_ContractorsBasicSalaries_EmployeesCodes2]
GO



Go
UPDATE ContractorsBasicSalaries SET CreatedDate = getdate() WHERE CreatedDate IS NULL

