/*
EndOfServices Reports
*/
CREATE PROC [dbo].[spGetEndOfServiceByEndOfServiceID] --1
( 
	@EndOfServiceID INT 
)
AS
SELECT  EOS.EndOfServiceID,
		mic_db.dbo.fn_GregToUmAlqura(EOS.EndOfServiceDate) AS EndOfServiceDate, -- [تاريخ نهاية الخدمة],
		EOS.EndOfServiceDecisionNo AS EndOfServiceDecisionNo,--[رقم القرار],
		mic_db.dbo.fn_GregToUmAlqura(EOS.EndOfServiceDecisionDate) AS EndOfServiceDecisionDate,-- [تاريخ القرار],
		EOSC.EndOfServiceCase +N' '+ EOSR.EndOfServiceReason  AS [Subject],-- [الموضوع],
		EOSC.EndOfServiceCase AS [SubjectCase],--[الموضوع حاله],
		EOSR.EndOfServiceReason  AS [SubjectReason],-- [ الموضوع سبب],
		E.FirstNameAr +' '	+ E.MiddleNameAr + ' '+ E.GrandFatherNameAr +' '+ E.LastNameAr AS [EmployeeNameAr],-- [الاسم],
		E.EmployeeIDNo AS EmployeeIDNo,--[السجل المدني],
		mic_db.dbo.fn_GregToUmAlqura(E.EmployeeBirthDate) AS EmployeeBirthDate,-- [تاريخ الميلاد],
		EC.EmployeeCodeNo AS EmployeeCodeNo,-- [رقم الكمبيوتر],
		OJ.JobNo AS JobNo,-- [رقم الوظيفة],
		J.JobName AS JobName,-- [الوظيفة],
		R.RankName AS RankName,-- [المرتبة],
		CD.CareerDegreeName AS CareerDegreeName,-- [الدرجة],
		BS.BasicSalary AS BasicSalary,-- [الراتب],
		O.OrganizationName AS OrganizationName-- [الجهة]
	FROM EndOfServices EOS 
		INNER JOIN EndOfServicesReasons EOSR  ON EOSR.EndOfServiceReasonID=EOS.EndOfServiceReasonID
		INNER JOIN EndOfServicesCases EOSC ON EOSC.EndOfServiceCaseID=EOSR.EndOfServiceCaseID
		INNER JOIN EmployeesCareersHistory ECH ON ECH.EmployeeCareerHistoryID=EOS.EmployeeCareerHistoryID
		INNER JOIN EmployeesCodes EC ON EC.EmployeeCodeID=ECH.EmployeeCodeID 
		INNER JOIN Employees E ON E.EmployeeID=EC.EmployeeID
		INNER JOIN OrganizationsJobs OJ ON OJ.OrganizationJobID=ECH.OrganizationJobID 
		INNER JOIN Jobs J ON J.JobID=OJ.JobID 
		INNER JOIN Ranks R ON R.RankID= OJ.RankID
		INNER JOIN CareersDegrees CD ON CD.CareerDegreeID=ECH.CareerDegreeID 
		INNER JOIN BasicSalaries BS ON BS.CareerDegreeID=CD.CareerDegreeID AND BS.RankID=R.RankID 
		INNER JOIN OrganizationsStructures O ON O.OrganizationID=OJ.OrganizationID 
	WHERE EOS.EndOfServiceID=@EndOfServiceID
