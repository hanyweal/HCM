GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ContractorsBasicSalaries](
	[ContractorBasicSalaryID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeCodeID] [int] NOT NULL,
	[BasicSalary] [float] NOT NULL,
 CONSTRAINT [PK_ContractorsBasicSalaries] PRIMARY KEY CLUSTERED 
(
	[ContractorBasicSalaryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ContractorsBasicSalaries]  WITH CHECK ADD  CONSTRAINT [FK_ContractorsBasicSalaries_EmployeesCodes] FOREIGN KEY([EmployeeCodeID])
REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
GO

ALTER TABLE [dbo].[ContractorsBasicSalaries] CHECK CONSTRAINT [FK_ContractorsBasicSalaries_EmployeesCodes]
GO

/*
-- Data Migration
INSERT INTO ContractorsBasicSalaries(EmployeeCodeID, BasicSalary) SELECT e.EmployeeCodeID, cast(e.EmployeeIDNo as bigint) % 9999 FROM vwCurrentEmployeesCareer e WHERE e.RankCategoryID = 4
*/

Go
ALTER TABLE RanksCategories ADD RankCategoryNameEn nvarchar(50) null default('')
Go
ALTER TABLE RanksCategories ADD RetirementPercentage float null default(0)

Go
UPDATE RanksCategories SET RetirementPercentage = 9 WHERE RankCategoryID = 1
UPDATE RanksCategories SET RetirementPercentage = 9 WHERE RankCategoryID = 2
UPDATE RanksCategories SET RetirementPercentage = 10 WHERE RankCategoryID = 3
UPDATE RanksCategories SET RetirementPercentage = 9 WHERE RankCategoryID = 4
UPDATE RanksCategories SET RetirementPercentage = 0 WHERE RankCategoryID = 5
UPDATE RanksCategories SET RetirementPercentage = 0 WHERE RankCategoryID = 6

Go
ALTER TABLE ContractorsBasicSalaries ADD TransfareAllowance float		null 
ALTER TABLE ContractorsBasicSalaries ADD CreatedDate		datetime	null default(getdate())
ALTER TABLE ContractorsBasicSalaries ADD LastUpdatedDate	datetime	null
ALTER TABLE ContractorsBasicSalaries ADD CreatedBy			int			null
ALTER TABLE ContractorsBasicSalaries ADD LastUpdatedBy		int			null
Go
UPDATE ContractorsBasicSalaries SET CreatedDate = getdate() WHERE CreatedDate IS NULL

