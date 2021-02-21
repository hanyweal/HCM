USE [HCM]


Go
INSERT INTO BusinessCategories(BusinessCategoryID, BusinessCategoryAr, BusinessCategoryEn)
	VALUES (10, N'الموظفين', N'Employees')

Go
INSERT INTO BusinessSubCategories(BusinssSubCategoryID, BusinessSubCategoryAr, BusinessSubCategoryEn, BusinessCategoryID, EntityName)
	VALUES (32, N'خبرات الموظفين السابقة بالتفاصيل', N'Employee Experience Details', 10, 'HCMDAL.EmployeeExperiencesWithDetailsDAL')


Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spGetEmployeeExperiencesWithDetailsByEmployeeCodeID]-- 22915
(
	@EmployeeCodeID int
)
AS
BEGIN

	SELECT EmployeeExperiencesWithDetails.*,
		vwEmployeesCodes.EmployeeCodeNo,  
		vwEmployeesCodes.EmployeeIDNo, 
		vwEmployeesCodes.EmployeeNameAr, 
		SectorsTypes.SectorTypeName as SectorTypeNameLookup,
		dbo.fnGetPeriodBetweenTwoDates(EmployeeExperiencesWithDetails.FromDate, EmployeeExperiencesWithDetails.ToDate) AS EmployeeExperienceWithDetailPeriod
	FROM EmployeeExperiencesWithDetails 
	INNER JOIN SectorsTypes ON EmployeeExperiencesWithDetails.SectorTypeID = SectorsTypes.SectorTypeID
	INNER JOIN vwEmployeesCodes ON EmployeeExperiencesWithDetails.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
	WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	ORDER BY EmployeeExperiencesWithDetails.FromDate
	
END
Go