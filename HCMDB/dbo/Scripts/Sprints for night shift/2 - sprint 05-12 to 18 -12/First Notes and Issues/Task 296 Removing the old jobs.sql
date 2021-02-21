GO
CREATE PROCEDURE [dbo].[spGetPreviousCareerByEmployeeCodeID]
(
	@EmployeeCodeID INT
)
AS
BEGIN
SELECT  top 1 [EmployeeCodeNo]
      ,[EmployeeIDNo]
      ,[EmployeeNameAr]
      ,[OrganizationCode]
      ,[OrganizationName]
      ,[RankName]
      ,[JobName]
      ,[JobNo]
      ,[EmployeeCodeID]
	  ,IsCurrentJob
      ,[EmployeeCareerHistoryID]
      ,mic_db.dbo.fn_GregToUmAlqura(JoinDate) AS JoinDate
      ,mic_db.dbo.fn_GregToUmAlqura(dbo.GetHiringDateByEmployeeCodeID(@EmployeeCodeID)) AS HiringDate
  FROM vwEmployeesCareersHistory
  WHERE EmployeeCodeID = @EmployeeCodeID
	AND vwEmployeesCareersHistory.IsCurrentJob =0 
  ORDER BY vwEmployeesCareersHistory.JoinDate DESC
END
