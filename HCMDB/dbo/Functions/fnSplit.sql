
CREATE Function [dbo].[fnSplit]			-- e.g.  select * from [dbo].[fn_Split]('1,23,123,232',',')
(
@IDs nvarchar(max),
@Delimiter varchar(1)
)
Returns @Tbl_IDs table (Seq int, value nvarchar(100)) as
BEGIN

	-- Append delimiter
	SET @IDs = @IDs + @Delimiter
	
	-- Indexes to keep the position of searching
	Declare @pos1 int
	Declare @pos2 int
	Declare @RowNum int
	Declare @Seq int = 0
	
	-- Start from first character
	Set @pos1 = CHARINDEX(@Delimiter, @IDs, 1)
	Set @pos2 = 1
	While @pos1 > 0
	Begin
		SET @Seq = @Seq + 1
		Insert into @Tbl_IDs(Seq, [value]) Values(@Seq, isnull(SUBSTRING(@IDs, @pos2, @pos1 - @pos2),''))
		
		-- goto next non comma character
		Set @pos2 = @pos1 + 1
		
		-- search for the next character
		Set @pos1 = CHARINDEX(@Delimiter, @IDs, @pos1+1)
			
	End

	RETURN
END
