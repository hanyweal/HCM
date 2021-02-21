
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetStopWorkByID]
(
	@StopWorkID int
)
AS
BEGIN 

	SELECT StopWorks.StopWorkID, 
		StopWorks.EmployeeCareerHistoryID, 
		vwEmployeesCareersHistory.EmployeeCodeID,
		StopWorks.StopWorkTypeID, 
		StopWorks.StopPoint, 
		StopWorks.StopWorkStartDate,
		StopWorks.StopWorkEndDate,
		mic_db.dbo.fn_GregToUmAlqura(cast(StopWorks.StopWorkStartDate as date)) AS StopWorkStartDateUmAlQura,
		mic_db.dbo.fn_GregToUmAlqura(cast(StopWorks.StopWorkEndDate as date)) AS StopWorkEndDateUmAlQura,
		StopWorks.IsConvicted, 
		StopWorks.CreatedDate, 
		StopWorks.LastUpdatedDate, 
		StopWorksTypes.StopWorkTypeName,  
		StopWorksCategories.StopWorkCategoryName,
		StopWorks.Note,
		dbo.fnGetPeriodBetweenTwoDates(StopWorks.StopWorkStartDate,StopWorks.StopWorkEndDate) AS Period
	FROM StopWorks 
		INNER JOIN StopWorksTypes ON StopWorks.StopWorkTypeID = StopWorksTypes.StopWorkTypeID 
		INNER JOIN vwEmployeesCareersHistory ON StopWorks.EmployeeCareerHistoryID = vwEmployeesCareersHistory.EmployeeCareerHistoryID
		INNER JOIN StopWorksCategories ON StopWorksTypes.StopWorkCategoryID = StopWorksCategories.StopWorkCategoryID	
	WHERE StopWorkID = @StopWorkID
 
END
