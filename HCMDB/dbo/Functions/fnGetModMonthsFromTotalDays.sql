CREATE function [dbo].[fnGetModMonthsFromTotalDays]
(
	@TotalDays int = 1060
)
RETURNS INT
BEGIN
	DECLARE 
	@DaysCountInUmAlquraYear int = 354,
	@DaysCountInUmAlquraMonth decimal(18,2) = 29.5
	RETURN CONVERT(INT,(@TotalDays % @DaysCountInUmAlquraYear) / @DaysCountInUmAlquraMonth)
END
