CREATE PROCEDURE [dbo].[spGetCurrentCareerByEmployeeCodeID]
(
	@EmployeeCodeID INT
)
AS
BEGIN
SELECT [EmployeeCodeNo]
      ,[EmployeeIDNo]
      ,[EmployeeNameAr]
      ,[OrganizationCode]
      ,[OrganizationName]
      ,[RankName]
      ,[JobName]
      ,[JobNo]
      ,[EmployeeCodeID]
      ,[EmployeeCareerHistoryID]
      ,mic_db.dbo.fn_GregToUmAlqura(JoinDate) AS JoinDate
      ,mic_db.dbo.fn_GregToUmAlqura(dbo.GetHiringDateByEmployeeCodeID(@EmployeeCodeID)) AS HiringDate
  FROM vwCurrentEmployeesCareer
  WHERE EmployeeCodeID = @EmployeeCodeID
END
