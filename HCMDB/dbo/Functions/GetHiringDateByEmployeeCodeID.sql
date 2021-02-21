--ALTER FUNCTION [dbo].[GetHiringDateByEmployeeCodeID] (@EmployeeCodeID INT)
--RETURNS varchar(50)
--BEGIN
--	DECLARE @Result varchar(50)
--	SELECT @Result = mic_db.dbo.fn_GregToUmAlqura(JoinDate) 
--	FROM [HCM].[dbo].[EmployeesCareersHistory] 
--	WHERE CareerHistoryTypeID =1 and EmployeeCodeID = @EmployeeCodeID
--	RETURN @Result
--END 

CREATE FUNCTION [dbo].[GetHiringDateByEmployeeCodeID] 
(
	@EmployeeCodeID INT
)
RETURNS DATE
BEGIN
	DECLARE @Result DATE
	SELECT @Result = JoinDate
	FROM EmployeesCareersHistory
	WHERE CareerHistoryTypeID = 1 
	AND EmployeeCodeID = @EmployeeCodeID
	RETURN @Result
END
