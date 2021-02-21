
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

--exec [dbo].[spGetChangeLogs] '2019-01-01', '2020-10-10'

CREATE PROCEDURE [dbo].[spGetChangeLogs]
(
	@StartDate date,
	@EndDate date,
	@BusinssSubCategoryID int = -1,
	@EmployeeCodeID int = -1
)
AS
BEGIN

	Declare @StartDate_local date = @StartDate 
	Declare @EndDate_local date = @EndDate
	Declare @BusinssSubCategoryID_local int = @BusinssSubCategoryID
	Declare @EmployeeCodeID_local int = @EmployeeCodeID

	IF @BusinssSubCategoryID_local = -1
	BEGIN
		SET @BusinssSubCategoryID_local = null
	END
	
	IF @EmployeeCodeID_local = -1
	BEGIN
		SET @EmployeeCodeID_local = null
	END

	SELECT Count(*) total, BusinessSubCategories.BusinessSubCategoryAr, vwEmployeesCodes.EmployeeCodeNo, ChangeLogs.EmployeeCodeID, 
		EventTypes.Type, vwEmployeesCodes.EmployeeNameAr, ChangeLogs.EntityName
	FROM ChangeLogs
		INNER JOIN EventTypes ON ChangeLogs.EventTypeID = EventTypes.EventTypeID
		INNER JOIN vwEmployeesCodes ON ChangeLogs.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
		INNER JOIN BusinessSubCategories ON BusinessSubCategories.EntityName = ChangeLogs.EntityName 
	WHERE ChangeLogs.EmployeeCodeID is not null
		AND ChangeLogs.EntityName not like '%Details%'
		AND ChangeLogs.EntityName <> 'HCMDAL.SMSLogs'
		--AND EmployeeCodeNo <> '90012454'
		AND CONVERT(date, ChangeLogs.DateChange) BETWEEN @StartDate_local AND @EndDate_local 
		--AND CONVERT(date, ChangeLogs.DateChange) BETWEEN '2019-01-01' AND '2020-10-02'
		AND BusinessSubCategories.BusinssSubCategoryID = ISNULL(@BusinssSubCategoryID_local, BusinessSubCategories.BusinssSubCategoryID)
		AND vwEmployeesCodes.EmployeeCodeID = ISNULL(@EmployeeCodeID_local, vwEmployeesCodes.EmployeeCodeID)

	Group By BusinessSubCategories.BusinessSubCategoryAr, ChangeLogs.EmployeeCodeID, EventTypes.Type, vwEmployeesCodes.EmployeeNameAr, EmployeeCodeNo, ChangeLogs.EntityName
	Order By vwEmployeesCodes.EmployeeNameAr
	
	--SELECT DISTINCT EntityName FROM ChangeLogs
	
END

