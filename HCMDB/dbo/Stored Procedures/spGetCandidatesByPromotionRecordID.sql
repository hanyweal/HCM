-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetCandidatesByPromotionRecordID]
(
	@PromotionRecordID int	= 284
)
AS
BEGIN
	SELECT  vwEmployeesCareersHistory.EmployeeCodeNo,
			vwEmployeesCareersHistory.EmployeeNameAr,
			vwEmployeesCareersHistory.JobNo AS CurrentJobNo,
			vwEmployeesCareersHistory.JobCode AS CurrentJobCode,
			vwEmployeesCareersHistory.JobName AS CurrentJobName,
			MIC_DB.dbo.fn_GregToUmAlqura(JoinDate) AS CurrentJoinDate,
			vwEmployeesCareersHistory.RankName AS CurrentRankName,
			vwOrganizationsJobs.JobNo AS NewJobNo,
			vwOrganizationsJobs.OrganizationName AS NewOrganizationName,
			vwOrganizationsJobs.JobName AS NewJobName,
			vwOrganizationsJobs.JobCode AS NewJobCode,
			vwOrganizationsJobs.RankName AS NewRankName,
			vwOrganizationsJobs.JobCategoryName AS NewJobCategoryName,
			QualificationsDegrees.QualificationDegreeName,
			Qualifications.QualificationName,
			GeneralSpecializations.GeneralSpecializationName,
			dbo.fnGetYearsFromTotalDays(PromotionsRecordsEmployees.OnGoingServiceDaysCount) OnGoingServiceYears,
			dbo.fnGetModMonthsFromTotalDays(PromotionsRecordsEmployees.OnGoingServiceDaysCount) OnGoingServiceMonths,
			dbo.fnGetModDaysFromTotalDays(PromotionsRecordsEmployees.OnGoingServiceDaysCount) OnGoingServiceDays,
			dbo.fnGetYearsFromTotalDays(PromotionsRecordsEmployees.PriorServiceDaysCount) PriorServiceYears,
			dbo.fnGetModMonthsFromTotalDays(PromotionsRecordsEmployees.PriorServiceDaysCount) PriorServiceMonths,
			dbo.fnGetModDaysFromTotalDays(PromotionsRecordsEmployees.PriorServiceDaysCount) PriorServiceDays,
			PromotionsRecordsEmployees.EducationPoints,
			PromotionsRecordsEmployees.EvaluationPoints,
			PromotionsRecordsEmployees.SeniorityPoints,
			PromotionsRecordsEmployees.IsRemovedAfterIncluding,
			PromotionsDecisions.PromotionDecisionName
	FROM PromotionsRecordsEmployees
	INNER JOIN vwEmployeesCareersHistory ON vwEmployeesCareersHistory.EmployeeCareerHistoryID = PromotionsRecordsEmployees.CurrentEmployeeCareerHistoryID
	LEFT JOIN PromotionsDecisions ON PromotionsRecordsEmployees.PromotionDecisionID = PromotionsDecisions.PromotionDecisionID
	LEFT JOIN PromotionsRecordsJobsVacancies ON PromotionsRecordsEmployees.PromotionRecordJobVacancyID = PromotionsRecordsJobsVacancies.PromotionRecordJobVacancyID
	LEFT JOIN vwOrganizationsJobs ON PromotionsRecordsJobsVacancies.OrganizationJobID = vwOrganizationsJobs.OrganizationJobID
	LEFT JOIN QualificationsDegrees ON PromotionsRecordsEmployees.LastQualificationDegreeID = QualificationsDegrees.QualificationDegreeID
	LEFT JOIN Qualifications ON PromotionsRecordsEmployees.LastQualificationID = Qualifications.QualificationID
	LEFT JOIN GeneralSpecializations ON PromotionsRecordsEmployees.LastGeneralSpecializationID = GeneralSpecializations.GeneralSpecializationID
	WHERE PromotionsRecordsEmployees.PromotionRecordID = @PromotionRecordID
	AND PromotionsRecordsEmployees.IsIncluded = 1
	AND ISNULL(PromotionsRecordsEmployees.IsRemovedAfterIncluding, 0) = 0
	ORDER BY ISNULL(PromotionsRecordsEmployees.PromotionDecisionID,500),
	(PromotionsRecordsEmployees.EducationPoints +
			PromotionsRecordsEmployees.EvaluationPoints +
			PromotionsRecordsEmployees.SeniorityPoints) DESC
	--AND PromotionsRecordsEmployees.IsRemovedAfterIncluding = 0
END