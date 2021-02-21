GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetOrphanAssignings]
AS
BEGIN

	DECLARE @tbl_TotalAssignings TABLE (EmployeeCodeID INT Primary KEY, TotalRecords INT NULL)
	DECLARE @tbl_TotaFinishedAssignings TABLE (EmployeeCodeID INT Primary KEY, TotalFinishedRecords INT NULL)
		
	-- part1 : get max end assigning date to get last assigning
	SELECT e.EmployeeCodeID, MAX(a.AssigningEndDate) as MaxAssigningEndDate
	INTO #MaxAssigningDate
	FROM Assignings a
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = a.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes e ON EmployeesCareersHistory.EmployeeCodeID = e.EmployeeCodeID
	WHERE a.IsFinished = 1
	GROUP BY e.EmployeeCodeID

	-- part2 : get last assigning
	SELECT a.*
	INTO #LastAssigning
	FROM Assignings a
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = a.EmployeeCareerHistoryID
	INNER JOIN #MaxAssigningDate mad ON mad.EmployeeCodeID = EmployeesCareersHistory.EmployeeCodeID AND mad.MaxAssigningEndDate = a.AssigningEndDate
 
	-- get total assigning of each active emp
	INSERT INTO @tbl_TotalAssignings 
	SELECT ech.EmployeeCodeID, COUNT(ech.EmployeeCodeID) as TotalRecords
	FROM Assignings a
	INNER JOIN EmployeesCareersHistory ech ON ech.EmployeeCareerHistoryID = a.EmployeeCareerHistoryID
	GROUP BY ech.EmployeeCodeID 

	-- get total finished assigning of each active emp	
	INSERT INTO @tbl_TotaFinishedAssignings
	SELECT ech.EmployeeCodeID, COUNT(ech.EmployeeCodeID) as TotalFinishedRecords
	FROM Assignings a
	INNER JOIN EmployeesCareersHistory ech ON ech.EmployeeCareerHistoryID = a.EmployeeCareerHistoryID
	WHERE a.IsFinished = 1
	GROUP BY ech.EmployeeCodeID 

	-- get employee assigning count (total & finished)
	SELECT e.EmployeeCodeID, ISNULL(ta.TotalRecords, 0) as TotalRecords, ISNULL(tfa.TotalFinishedRecords, 0) as TotalFinishedRecords 
	INTO #DistinctAssigningsWithTotals
	FROM vwEmployeesCodes e
	LEFT OUTER JOIN @tbl_TotalAssignings ta ON ta.EmployeeCodeID = e.EmployeeCodeID
	LEFT OUTER JOIN @tbl_TotaFinishedAssignings tfa ON tfa.EmployeeCodeID = e.EmployeeCodeID 

	-- final result query
	SELECT e.EmployeeCodeID, e.EmployeeCodeNo, e.EmployeeIDNo, e.EmployeeNameAr, a.AssigningID, a.AssigningStartDate, a.AssigningEndDate, 
		a.AssigningReasonID, ar.AssigningReasonName, a.AssigningReasonOther,
		a.EndAssigningReasonID, ear.AssigningReasonName as EndAssigningReasonNamer,
		AssigningsTypes.AssigningTypeName,
		Jobs.JobName,
		OrganizationsStructures.OrganizationName,
		KSACities.KSACityName
	FROM #DistinctAssigningsWithTotals da
	INNER JOIN [vwCurrentEmployeesCareer] as e ON da.EmployeeCodeID = e.EmployeeCodeID
	LEFT OUTER JOIN #LastAssigning a ON a.EmployeeCareerHistoryID = e.EmployeeCareerHistoryID 
	LEFT OUTER JOIN AssigningsReasons ar ON ar.AssigningReasonID = a.AssigningReasonID
	LEFT OUTER JOIN AssigningsReasons ear ON ear.AssigningReasonID = a.EndAssigningReasonID
	LEFT OUTER JOIN AssigningsTypes ON a.AssigningTypID = AssigningsTypes.AssigningTypID
	LEFT JOIN Jobs ON a.JobID = Jobs.JobID
	LEFT JOIN OrganizationsStructures ON OrganizationsStructures.OrganizationID = a.OrganizationID
	LEFT JOIN KSACities ON KSACities.KSACityID = a.ExternalKSACityID
	WHERE da.TotalRecords = da.TotalFinishedRecords 
	ORDER BY a.AssigningEndDate desc
	
END
Go