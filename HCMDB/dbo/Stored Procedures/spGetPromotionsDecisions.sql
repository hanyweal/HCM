--#####################################################################
/*
	PROMOTIONS DECISIONS 
*/

CREATE PROC [dbo].[spGetPromotionsDecisions]
(
	@PromotionRecordID INT,
	@CurrentEmployeeCareerHistoryID INT=NULL
)
AS
SELECT	PR.PromotionRecordID,
		PR.PromotionRecordNo,
		PR.PromotionRecordDate,
		mic_db.dbo.fn_GregToUmAlqura(PR.PromotionRecordDate) AS PromotionRecordDateUmAlQura, 
		ISNULL(PRE.IsDeserveExtraBonus,0) AS IsDeserveExtraBonus,
		EmpCurrent.FirstNameAr +' '	+ EmpCurrent.MiddleNameAr + ' '+ EmpCurrent.GrandFatherNameAr +' '+ EmpCurrent.LastNameAr AS EmployeeNameAr,
		EmpCurrent.EmployeeIDNo,
		EmpCodCurrent.EmployeeCodeNo,
		mic_db.dbo.fn_GregToUmAlqura(dbo.GetHiringDateByEmployeeCodeID(EmpCodCurrent.EmployeeCodeID)) AS HiringDateUmAlqura,	

		EmpCHCurrent.EmployeeCareerHistoryID AS CurrentEmployeeCareerHistoryID,
		JobCurrent.JobName AS CurrentJobName,
		RnkCurrent.RankName AS CurrentRankName,
		OrgJobCurrent.JobNo AS CurrentJobNo,
		OrgCurrent.OrganizationName AS CurrentOrganizationName,
		CDCurrent.CareerDegreeName AS CurrentCareerDegreeName ,
		(SELECT BS.BasicSalary FROM BasicSalaries BS WHERE BS.CareerDegreeID=EmpCHCurrent.CareerDegreeID AND BS.RankID=RnkCurrent.RankID)  AS CurrentBasicSalary,



		EmpCHNew.EmployeeCareerHistoryID AS NewEmployeeCareerHistoryID,
		JobNew.JobName AS NewJobName,
		RnkNew.RankName AS NewRankName,
		OrgJobNew.JobNo AS NewJobNo,
		OrgNew.OrganizationName AS NewOrganizationName,
		CDNew.CareerDegreeName AS NewCareerDegreeName,
		(SELECT BS.BasicSalary FROM BasicSalaries BS WHERE BS.CareerDegreeID=EmpCHNew.CareerDegreeID AND BS.RankID=RnkNew.RankID)  AS NewBasicSalary
	
	FROM PromotionsRecords PR
		INNER JOIN PromotionsRecordsEmployees PRE ON PR.PromotionRecordID=PRE.PromotionRecordID
		--======================   CURRENT JOB  =======================
		INNER JOIN EmployeesCareersHistory EmpCHCurrent ON EmpCHCurrent.EmployeeCareerHistoryID=PRE.CurrentEmployeeCareerHistoryID
		INNER JOIN EmployeesCodes EmpCodCurrent ON EmpCodCurrent.EmployeeCodeID=EmpCHCurrent.EmployeeCodeID
		INNER JOIN Employees EmpCurrent ON EmpCurrent.EmployeeID=EmpCodCurrent.EmployeeID
		INNER JOIN OrganizationsJobs OrgJobCurrent ON OrgJobCurrent.OrganizationJobID=EmpCHCurrent.OrganizationJobID
		INNER JOIN Jobs JobCurrent ON JobCurrent.JobID=OrgJobCurrent.JobID
		INNER JOIN OrganizationsStructures OrgCurrent ON OrgCurrent.OrganizationID=OrgJobCurrent.OrganizationID
		INNER JOIN Ranks RnkCurrent ON RnkCurrent.RankID=OrgJobCurrent.RankID
		INNER JOIN CareersDegrees CDCurrent on CDCurrent.CareerDegreeID=EmpCHCurrent.CareerDegreeID
		--======================   NEW JOB  =======================
		LEFT OUTER JOIN EmployeesCareersHistory EmpCHNew ON EmpCHNew.EmployeeCareerHistoryID=PRE.NewEmployeeCareerHistoryID
		LEFT OUTER JOIN EmployeesCodes EmpCodNew ON EmpCodNew.EmployeeCodeID=EmpCHNew.EmployeeCodeID
		LEFT OUTER JOIN OrganizationsJobs OrgJobNew ON OrgJobNew.OrganizationJobID=EmpCHNew.OrganizationJobID
		LEFT OUTER JOIN Jobs JobNew ON JobNew.JobID=OrgJobNew.JobID
		LEFT OUTER JOIN OrganizationsStructures OrgNew ON OrgNew.OrganizationID=OrgJobNew.OrganizationID
		LEFT OUTER JOIN Ranks RnkNew ON RnkNew.RankID=OrgJobNew.RankID
		LEFT OUTER JOIN CareersDegrees CDNew on CDNew.CareerDegreeID=EmpCHNew.CareerDegreeID
	WHERE PR.PromotionRecordID=@PromotionRecordID
		AND EmpCHCurrent.EmployeeCareerHistoryID=ISNULL(@CurrentEmployeeCareerHistoryID,EmpCHCurrent.EmployeeCareerHistoryID)
		AND PRE.IsIncluded=1
		AND (PRE.IsRemovedAfterIncluding=0 OR PRE.IsRemovedAfterIncluding IS NULL)
		AND PRE.NewEmployeeCareerHistoryID is not NULL
