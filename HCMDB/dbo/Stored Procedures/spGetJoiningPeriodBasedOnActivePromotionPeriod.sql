﻿--SELECT * FROM OrganizationsJobs
--INNER JOIN Jobs ON Jobs.JobID = OrganizationsJobs.JobID
--WHERE RankID = 9
--ORDER BY OrganizationsJobs.CreatedDate

CREATE PROCEDURE [dbo].[spGetJoiningPeriodBasedOnActivePromotionPeriod]
(
	@EmployeeCodeID int = 22951
)
AS
BEGIN

	DECLARE @HiringDate date
	DECLARE @PromotionEndDate date
	DECLARE @DaysCount int

	SELECT @PromotionEndDate = PromotionEndDate 
	FROM PromotionsPeriods
	WHERE IsActive = 1

	SELECT @HiringDate = dbo.GetHiringDateByEmployeeCodeID(@EmployeeCodeID)

	SELECT @DaysCount = DATEDIFF(DAY,@HiringDate,@PromotionEndDate)

	SELECT @DaysCount AS TotalDays,
		dbo.fnGetYearsFromTotalDays(@DaysCount) Years,
		dbo.fnGetModMonthsFromTotalDays(@DaysCount) ModMonths,
		dbo.fnGetModDaysFromTotalDays(@DaysCount) ModDays
END
