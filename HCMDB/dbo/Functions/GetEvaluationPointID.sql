-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetEvaluationPointID]
(
	@degree FLOAT
)
RETURNS int
AS
BEGIN
	DECLARE @Result int
	IF(@degree BETWEEN 90 AND 100)
	SET @Result = 1
	ELSE IF(@degree BETWEEN 80 AND 89.99)
	SET @Result = 2
	ELSE IF(@degree BETWEEN 70 AND 79.99)
	SET @Result = 3
	ELSE IF(@degree BETWEEN 60 AND 69.99)
	SET @Result = 4
	ELSE 
	SET @Result = 5
	
	RETURN @Result
END
