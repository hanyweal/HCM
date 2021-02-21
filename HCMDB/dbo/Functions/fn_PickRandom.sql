
CREATE FUNCTION [dbo].[fn_PickRandom] 
(
	 @max_length INT=3
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
DECLARE @result nvarchar(max) = NULL;
DECLARE @s nvarchar(max) = '';
WITH chars(n, c) AS (
   SELECT 1, N'1' UNION ALL 
   SELECT 2, N'2' UNION ALL 
   SELECT 3, N'3' UNION ALL 
   SELECT 4, N'4' UNION ALL 
   SELECT 5, N'5' UNION ALL
   SELECT 6, N'6' UNION ALL 
   SELECT 7, N'7' UNION ALL
   SELECT 8, N'8' UNION ALL 
   SELECT 9, N'9' UNION ALL 
   SELECT 10, N'0'
   -- add more characters here to include them in a random string
),
lines AS (
   SELECT 1 AS n UNION ALL
   SELECT 2 UNION ALL
   SELECT 3 UNION ALL
   SELECT 4 UNION ALL
   SELECT 5 UNION ALL
   SELECT 6 UNION ALL
   SELECT 7 UNION ALL
   SELECT 8 UNION ALL
   SELECT 9 UNION ALL
   SELECT 10
),
all_lines AS (
   SELECT ROW_NUMBER() OVER(ORDER BY l1.n) AS n,
          convert(int, DBO.RandFn() * (SELECT COUNT(n) FROM chars)) + 1 AS char_n
   FROM lines l1
        CROSS JOIN lines l2 -- 100
        CROSS JOIN lines l3 -- 1000
        -- add more CROSS JOINs here to get longer string
),
only_lines AS (
   SELECT * FROM all_lines
   WHERE n <= @max_length
)
SELECT @s = @s + c 
FROM only_lines l INNER JOIN chars c ON l.char_n = c.n
SELECT @result=RTRIM(LTRIM(@s))
  RETURN @result;
END
