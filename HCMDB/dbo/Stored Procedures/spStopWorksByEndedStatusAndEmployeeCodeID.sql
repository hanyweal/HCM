CREATE PROC [dbo].[spStopWorksByEndedStatusAndEmployeeCodeID] 
(
	 @StopWorkEnded INT,--0 =Mean it didn't end1= mean ended,=2 all of them.
	@EmployeeCodeID int=NULL
)
AS
SELECT  SW.StopWorkID,
		SW.StopWorkStartDate AS StopWorkStartDateGregorian,
		mic_db.dbo.fn_GregToUmAlqura(SW.StopWorkStartDate)StopWorkStartDateHijri,
		SW.StopWorkEndDate AS StopWorkEndDateGregorian,
		mic_db.dbo.fn_GregToUmAlqura(SW.StopWorkEndDate)StopWorkEndDateHijri,
		SW.StopPoint,
		CASE WHEN SW.IsConvicted = 1 THEN N'مدان' ELSE N'غير مدان' END AS IsConvicted, 
		SW.StartStopWorkDecisionNumber,
		SW.StartStopWorkDecisionDate AS StartStopWorkDecisionDateGregorian,
		mic_db.dbo.fn_GregToUmAlqura(CAST(SW.StartStopWorkDecisionDate AS DATE))StartStopWorkDecisionDateHijri,
		SW.EndStopWorkDecisionNumber,
		SW.EndStopWorkDecisionDate AS EndStopWorkDecisionDateGregorian,
		mic_db.dbo.fn_GregToUmAlqura(CAST(SW.EndStopWorkDecisionDate AS DATE))EndStopWorkDecisionDateHijri,
		ECH.EmployeeCareerHistoryID,
		EC.EmployeeCodeID,
		EC.EmployeeCodeNo,
		E.EmployeeID,
		E.EmployeeIDNo,
		E.FirstNameAr+' '+E.MiddleNameAr+' '+E.GrandFatherNameAr+' '+E.LastNameAr AS EmployeeNameAr,
		E.EmployeeMobileNo,
		SWT.StopWorkTypeID,
		SWT.StopWorkTypeName,
		SWC.StopWorkCategoryID,
		SWC.StopWorkCategoryName,
		CASE WHEN  SW.StopWorkEndDate > GETDATE() THEN N'ساري' ELSE N'منتهي' END as StopWorkIsEnd
	FROM StopWorks SW
		INNER JOIN EmployeesCareersHistory ECH ON ECH.EmployeeCareerHistoryID=SW.EmployeeCareerHistoryID
		INNER JOIN EmployeesCodes EC ON EC.EmployeeCodeID=ECH.EmployeeCodeID
		INNER JOIN Employees E ON E.EmployeeID=EC.EmployeeID
		INNER JOIN StopWorksTypes SWT ON SWT.StopWorkTypeID=SW.StopWorkTypeID
		INNER JOIN StopWorksCategories SWC ON SWC.StopWorkCategoryID=SWT.StopWorkCategoryID
	where 
		 @StopWorkEnded  = CASE WHEN @StopWorkEnded =2 
											THEN 2 
											ELSE (CASE WHEN  SW.StopWorkEndDate > GETDATE() THEN 0 ELSE 1 END) END
	 AND EC.EmployeeCodeID =ISNULL(@EmployeeCodeID,EC.EmployeeCodeID) 