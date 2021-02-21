USE [HCM]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempEmployeesBasicSalaries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeCareerHistoryID] [int] NOT NULL,
	EmployeeCodeID [int] NOT NULL,
	BasicSalary [decimal](18, 2) NOT NULL,
	NetSalary [decimal](18, 2) NOT NULL,
	CreateOn datetime not null default(getdate()),
 CONSTRAINT [PK_TempEmployeesBasicSalaries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


