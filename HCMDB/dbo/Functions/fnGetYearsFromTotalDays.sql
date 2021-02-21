CREATE function [dbo].[fnGetYearsFromTotalDays]
(
	@TotalDays int = 1060
)
RETURNS INT
BEGIN
	DECLARE 
	@DaysCountInUmAlquraYear int = 354,
	@DaysCountInUmAlquraMonth decimal(18,2) = 29.5
	RETURN @TotalDays / @DaysCountInUmAlquraYear
END
