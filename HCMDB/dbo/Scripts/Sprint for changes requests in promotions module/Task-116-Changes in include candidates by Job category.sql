USE [HCM]
GO
/****** Object:  StoredProcedure [dbo].[spGetDeservedEmployeesToBeIncludedPromotion]    Script Date: 27/08/1441 01:02:50 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spGetDeservedEmployeesToBeIncludedPromotion]
(
	 @RankID int = 9,
	 @JobCategoryID int = 72,--78,
	 @PromotionPeriodID int = 1
)
AS
BEGIN

	--DECLARE @RankID int = 9
	--DECLARE @JobCategoryID int = 72--78
	--DECLARE @PromotionPeriodID int = 1
	
	
	DECLARE @PreviousRank int = 0
	DECLARE @TempTable TABLE(EmployeeCareerHistoryID int,
							EmployeeCodeID int, 
							EmployeeCodeNo nvarchar(15),
							EmployeeIDNo nvarchar(11), 
							EmployeeNameAr nvarchar(500), 
							JobCategoryID int,
							JobCategoryName nvarchar(100),
							JobCode nvarchar(100),
							JobName nvarchar(100),
							JobNo nvarchar(100),
							JoinDate date,
							OrganizationCode nvarchar(100),
							OrganizationName nvarchar(100),
							PromotionCandidateAddedWayID int,
							PromotionCandidateAddedWayName nvarchar(100),
							RankID int,
							RankName nvarchar(200))
							
	SELECT @PreviousRank = RankID 
	FROM Ranks 
	WHERE Ranks.NextRankID = @RankID
	
	-- By Current Job
	INSERT INTO @TempTable
	SELECT EmployeeCareerHistoryID ,
			EmployeeCodeID , 
			EmployeeCodeNo , 
			EmployeeIDNo,
			EmployeeNameAr , 
			JobCategoryID ,
			JobCategoryName ,
			JobCode ,
			JobName ,
			JobNo ,
			JoinDate ,
			OrganizationCode ,
			OrganizationName ,
			1 AS PromotionCandidateAddedWayID ,
			 (SELECT PromotionCandidateAddedWayName FROM PromotionsCandidatesAddedWays WHERE PromotionCandidateAddedWayID = 1) AS PromotionCandidateAddedWayName,
			RankID ,
			RankName 
	FROM vwEmployeesCareersHistory
	WHERE IsCurrentJob = 1
	AND RankID = @RankID
	AND JobCategoryID = @JobCategoryID
	
	-- By Previous Job
	INSERT INTO @TempTable
	SELECT EmployeeCareerHistoryID ,
			vwEmployeesCareersHistory.EmployeeCodeID , 
			EmployeeCodeNo , 
			EmployeeIDNo,
			EmployeeNameAr , 
			JobCategoryID ,
			JobCategoryName ,
			JobCode ,
			JobName ,
			JobNo ,
			JoinDate ,
			OrganizationCode ,
			OrganizationName ,
			2 AS PromotionCandidateAddedWayID ,
			 (SELECT PromotionCandidateAddedWayName FROM PromotionsCandidatesAddedWays WHERE PromotionCandidateAddedWayID = 2) AS PromotionCandidateAddedWayName,
			RankID ,
			RankName 
	FROM vwEmployeesCareersHistory
	INNER JOIN (SELECT EmployeeCodeID	
				FROM vwEmployeesCareersHistory 
				WHERE RankID = @PreviousRank
				AND JobCategoryID = @JobCategoryID
				AND EmployeeCodeID IN (SELECT EmployeeCodeID
									   FROM vwEmployeesCareersHistory
									   WHERE IsCurrentJob = 1
										AND RankID = @RankID)
			    AND EmployeeCodeID NOT IN (SELECT EmployeeCodeID 
										   FROM @TempTable)) AS PreviousJob ON PreviousJob.EmployeeCodeID = vwEmployeesCareersHistory.EmployeeCodeID
	WHERE IsCurrentJob = 1
	AND RankID = @RankID

	-- By Job Category
	INSERT INTO @TempTable
	SELECT DISTINCT EmployeeCareerHistoryID ,
			vwEmployeesCareersHistory.EmployeeCodeID , 
			EmployeeCodeNo , 
			EmployeeIDNo,
			EmployeeNameAr , 
			JobCategoryID ,
			JobCategoryName ,
			JobCode ,
			JobName ,
			JobNo ,
			JoinDate ,
			OrganizationCode ,
			OrganizationName ,
			3 AS PromotionCandidateAddedWayID ,
			 (SELECT PromotionCandidateAddedWayName FROM PromotionsCandidatesAddedWays WHERE PromotionCandidateAddedWayID = 3) AS PromotionCandidateAddedWayName,
			RankID ,
			RankName 
	FROM vwEmployeesCareersHistory 
	INNER JOIN vwEmployeesQualifications ON vwEmployeesCareersHistory.EmployeeCodeID = vwEmployeesQualifications.EmployeeCodeID
	WHERE IsCurrentJob = 1
	AND RankID = @RankID
	AND JobCategoryID IN (SELECT AssignedJobCategoryID 
						 FROM PromotionsJobsCategories 
						 WHERE JobCategoryID = @JobCategoryID)

	AND (vwEmployeesQualifications.QualificationDegreeID IN (SELECT QualificationDegreeID 
															 FROM JobsCategoriesQualifications 
															 WHERE JobsCategoriesQualifications.JobCategoryID = @JobCategoryID
															 AND PromotionPeriodID = @PromotionPeriodID) 
															 
															 AND 
	vwEmployeesQualifications.QualificationID IN (SELECT ISNULL(JobsCategoriesQualifications.QualificationID ,vwEmployeesQualifications.QualificationID)
												  FROM JobsCategoriesQualifications 
												  WHERE JobsCategoriesQualifications.JobCategoryID = @JobCategoryID
												  AND JobsCategoriesQualifications.QualificationDegreeID = vwEmployeesQualifications.QualificationDegreeID
												  AND PromotionPeriodID = @PromotionPeriodID) 
												  
												  AND
	vwEmployeesQualifications.GeneralSpecializationID IN (SELECT ISNULL(GeneralSpecializationID ,vwEmployeesQualifications.GeneralSpecializationID)
														  FROM JobsCategoriesQualifications 
														  WHERE JobsCategoriesQualifications.JobCategoryID = @JobCategoryID
														  AND JobsCategoriesQualifications.QualificationDegreeID = vwEmployeesQualifications.QualificationDegreeID
														  --AND JobsCategoriesQualifications.QualificationID = vwEmployeesQualifications.QualificationID
														  AND PromotionPeriodID = @PromotionPeriodID))	


   AND vwEmployeesCareersHistory.EmployeeCodeID NOT IN (SELECT EmployeeCodeID FROM @TempTable)
	
   IF(@RankID <= 6)
	BEGIN
		-- By Qualification
		INSERT INTO @TempTable
		SELECT DISTINCT EmployeeCareerHistoryID ,
			vwEmployeesCareersHistory.EmployeeCodeID , 
			EmployeeCodeNo , 
			EmployeeIDNo ,
			EmployeeNameAr , 
			JobCategoryID ,
			JobCategoryName ,
			JobCode ,
			JobName ,
			JobNo ,
			JoinDate ,
			OrganizationCode ,
			OrganizationName ,
			4 AS PromotionCandidateAddedWayID ,
			 (SELECT PromotionCandidateAddedWayName FROM PromotionsCandidatesAddedWays WHERE PromotionCandidateAddedWayID = 4) AS PromotionCandidateAddedWayName,
			RankID ,
			RankName 
		FROM vwEmployeesCareersHistory 
		INNER JOIN vwEmployeesQualifications ON vwEmployeesCareersHistory.EmployeeCodeID = vwEmployeesQualifications.EmployeeCodeID
		WHERE IsCurrentJob = 1
		AND RankID = @RankID
		AND vwEmployeesCareersHistory.EmployeeCodeID NOT IN (SELECT EmployeeCodeID FROM @TempTable)
	END
	
   SELECT * , 
		[dbo].[GetHiringDateByEmployeeCodeID](F.EmployeeCodeID) AS HiringDate
	FROM @TempTable AS F
	WHERE F.EmployeeCareerHistoryID NOT IN (SELECT CurrentEmployeeCareerHistoryID
										  FROM PromotionsRecords
										  INNER JOIN PromotionsRecordsEmployees ON PromotionsRecords.PromotionRecordID = PromotionsRecordsEmployees.PromotionRecordID
										  WHERE PromotionPeriodID = @PromotionPeriodID
										  AND PromotionsRecordsEmployees.PromotionRecordJobVacancyID IS NOT NULL)
    ORDER BY EmployeeCodeID
	

		------INSERT INTO @TempTable
	------SELECT (SELECT EmployeeCareerHistoryID FROM vwCurrentEmployeesCareer WHERE EmployeeCodeID = vwEmployeesCareersHistory.EmployeeCodeID),
	------		EmployeeCodeID , 
	------		EmployeeCodeNo , 
	------		EmployeeIDNo,
	------		EmployeeNameAr , 
	------		JobCategoryID ,
	------		JobCategoryName ,
	------		JobCode ,
	------		JobName ,
	------		JobNo ,
	------		JoinDate ,
	------		OrganizationCode ,
	------		OrganizationName ,
	------		2 AS PromotionCandidateAddedWayID ,
	------		 (SELECT PromotionCandidateAddedWayName FROM PromotionsCandidatesAddedWays WHERE PromotionCandidateAddedWayID = 2) AS PromotionCandidateAddedWayName,
	------		RankID ,
	------		RankName 
	------FROM vwEmployeesCareersHistory 
	------WHERE RankID = @PreviousRank
	------AND JobCategoryID = @JobCategoryID
	------AND EmployeeCodeID IN (SELECT EmployeeCodeID
	------						FROM vwEmployeesCareersHistory
	------						WHERE IsCurrentJob = 1
	------						AND RankID = @RankID)
	------  AND EmployeeCodeID NOT IN (SELECT EmployeeCodeID FROM @TempTable)
	
	
	-- it should be distinct because of may be some candidates will repeat if he has more than one qualification
	--SELECT DISTINCT vwEmployeesCareersHistory.*,
	--	[dbo].[GetHiringDateByEmployeeCodeID](vwEmployeesCareersHistory.EmployeeCodeID) AS HiringDate,
	--	'1' AS PromotionCandidateAddedWayID ,(select PromotionCandidateAddedWayName from PromotionsCandidatesAddedWays where PromotionCandidateAddedWayID = 1) as PromotionCandidateAddedWayName
	--FROM vwEmployeesCareersHistory
	--LEFT JOIN vwEmployeesQualifications ON vwEmployeesCareersHistory.EmployeeCodeID = vwEmployeesQualifications.EmployeeCodeID
	--WHERE vwEmployeesCareersHistory.RANKID = @RankID
	--AND vwEmployeesCareersHistory.IsActive = 1
	--AND vwEmployeesCareersHistory.IsCurrentJob = 1
	---- EXCLUDING EMPLOYEES THAT ALREADY RESERVED NEW JOB VACANCY IN THE SAME PROMOTION PERIOD
	--AND vwEmployeesCareersHistory.EmployeeCareerHistoryID NOT IN(SELECT CurrentEmployeeCareerHistoryID
	--															FROM PromotionsRecords
	--															INNER JOIN PromotionsRecordsEmployees ON PromotionsRecords.PromotionRecordID = PromotionsRecordsEmployees.PromotionRecordID
	--															WHERE PromotionPeriodID = @PromotionPeriodID
	--															AND PromotionsRecordsEmployees.PromotionRecordJobVacancyID IS NOT NULL) -- thats mean already the candidate reserved new job vancancy ... no need to fetch him again in other promotion record
	--AND (JOBCATEGORYID = @JobCategoryID OR JOBCATEGORYID IN (SELECT AssignedJobCategoryID 
	--														 FROM PromotionsJobsCategories 
	--														 WHERE JobCategoryID = @JobCategoryID))												  

	--UNION 
	
	--SELECT DISTINCT vwEmployeesCareersHistory.*,
	--	[dbo].[GetHiringDateByEmployeeCodeID](vwEmployeesCareersHistory.EmployeeCodeID) AS HiringDate,
	--	'2' AS PromotionCandidateAddedWayID ,(select PromotionCandidateAddedWayName from PromotionsCandidatesAddedWays where PromotionCandidateAddedWayID = 2) as PromotionCandidateAddedWayName
	--FROM vwEmployeesCareersHistory
	--LEFT JOIN vwEmployeesQualifications ON vwEmployeesCareersHistory.EmployeeCodeID = vwEmployeesQualifications.EmployeeCodeID
	----CROSS APPLY DBO.fnGetLastEmployeeQualificationByEmployeeCodeID(vwEmployeesCareersHistory.EmployeeCodeID) as LastEmployeeQualification
	--WHERE vwEmployeesCareersHistory.RANKID = @RankID
	--AND vwEmployeesCareersHistory.IsActive = 1
	--AND vwEmployeesCareersHistory.IsCurrentJob = 1
	---- EXCLUDING EMPLOYEES THAT ALREADY RESERVED NEW JOB VACANCY IN THE SAME PROMOTION PERIOD
	--AND vwEmployeesCareersHistory.EmployeeCareerHistoryID NOT IN(SELECT CurrentEmployeeCareerHistoryID
	--															FROM PromotionsRecords
	--															INNER JOIN PromotionsRecordsEmployees ON PromotionsRecords.PromotionRecordID = PromotionsRecordsEmployees.PromotionRecordID
	--															WHERE PromotionPeriodID = @PromotionPeriodID
	--															AND PromotionsRecordsEmployees.PromotionRecordJobVacancyID IS NOT NULL) -- thats mean already the candidate reserved new job vancancy ... no need to fetch him again in other promotion record
	--AND (vwEmployeesQualifications.QualificationDegreeID IN (SELECT QualificationDegreeID 
	--														 FROM JobsCategoriesQualifications 
	--														 WHERE JobsCategoriesQualifications.JobCategoryID = @JobCategoryID
	--														 AND PromotionPeriodID = @PromotionPeriodID) 
															 
	--														 and 
	--vwEmployeesQualifications.QualificationID IN (SELECT ISNULL(JobsCategoriesQualifications.QualificationID ,vwEmployeesQualifications.QualificationID)
	--											  FROM JobsCategoriesQualifications 
	--											  WHERE JobsCategoriesQualifications.JobCategoryID = @JobCategoryID
	--											  AND JobsCategoriesQualifications.QualificationDegreeID = vwEmployeesQualifications.QualificationDegreeID
	--											  AND PromotionPeriodID = @PromotionPeriodID) 
												  
	--											  and
	--vwEmployeesQualifications.GeneralSpecializationID IN (SELECT ISNULL(GeneralSpecializationID ,vwEmployeesQualifications.GeneralSpecializationID)
	--													  FROM JobsCategoriesQualifications 
	--													  WHERE JobsCategoriesQualifications.JobCategoryID = @JobCategoryID
	--													  AND JobsCategoriesQualifications.QualificationDegreeID = vwEmployeesQualifications.QualificationDegreeID
	--													  --AND JobsCategoriesQualifications.QualificationID = vwEmployeesQualifications.QualificationID
	--													  AND PromotionPeriodID = @PromotionPeriodID))	
														  
	--AND NOT EXISTS (SELECT D.EmployeeCodeID
	--														FROM vwEmployeesCareersHistory AS D
	--														LEFT JOIN vwEmployeesQualifications ON D.EmployeeCodeID = vwEmployeesQualifications.EmployeeCodeID
	--														WHERE D.RANKID = @RankID
	--														AND D.EmployeeCodeID = vwEmployeesCareersHistory.EmployeeCodeID 
	--														AND D.IsActive = 1
	--														AND D.IsCurrentJob = 1
	--														-- EXCLUDING EMPLOYEES THAT ALREADY RESERVED NEW JOB VACANCY IN THE SAME PROMOTION PERIOD
	--														AND D.EmployeeCareerHistoryID NOT IN(SELECT CurrentEmployeeCareerHistoryID
	--																													FROM PromotionsRecords
	--																													INNER JOIN PromotionsRecordsEmployees ON PromotionsRecords.PromotionRecordID = PromotionsRecordsEmployees.PromotionRecordID
	--																													WHERE PromotionPeriodID = @PromotionPeriodID
	--																													AND PromotionsRecordsEmployees.PromotionRecordJobVacancyID IS NOT NULL) -- thats mean already the candidate reserved new job vancancy ... no need to fetch him again in other promotion record
	--														AND (JOBCATEGORYID = @JobCategoryID OR JOBCATEGORYID IN (SELECT AssignedJobCategoryID 
	--																												 FROM PromotionsJobsCategories 
	--																												 WHERE JobCategoryID = @JobCategoryID))		)

														  
	--ORDER BY EmployeeCodeNo
--END

END