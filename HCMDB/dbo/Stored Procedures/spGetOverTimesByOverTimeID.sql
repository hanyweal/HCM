CREATE PROC [dbo].[spGetOverTimesByOverTimeID]
(
	@OverTimeID INT
)
AS
BEGIN
	SELECT  OverTimes.OverTimeID, 
			OverTimes.OverTimeStartDate, 
			OverTimes.OverTimeEndDate, 
			mic_db.dbo.fn_GregToUmAlqura(OverTimes.OverTimeStartDate) AS OverTimeStartDateUmAlQura, 
			mic_db.dbo.fn_GregToUmAlqura(OverTimes.OverTimeEndDate) AS OverTimeEndDateUmAlQura, 
			OverTimes.WeekWorkHoursAvg, 
			OverTimes.SaturdayHoursAvg, 
			OverTimes.FridayHoursAvg, 
			OverTimes.Requester,
			OverTimes.Tasks, 
			OverTimes.CreatedDate,
			OverTimes.LastUpdatedDate, 
			dbo.fnGetPeriodBetweenTwoDates(OverTimes.OverTimeStartDate,OverTimes.OverTimeEndDate) AS OverTimePeriod
	FROM    OverTimes  
	WHERE   OverTimes.OverTimeID = @OverTimeID
END
