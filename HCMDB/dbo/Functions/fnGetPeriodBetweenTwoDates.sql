-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetPeriodBetweenTwoDates]
(
	@StartDate DATE,
	@EndDate DATE
)
RETURNS INT
AS
BEGIN
	RETURN mic_db.dbo.fn_GetPeriodBetweenTwoUmAlquraDates(@StartDate,@EndDate) + 1
END
