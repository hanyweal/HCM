Alter table [dbo].[PassengerOrders]
add TravelTo nvarchar(500) null
go
CREATE TABLE [dbo].[PassengerOrdersEscorts](
	[PassengerOrderEscortID] [int] IDENTITY(1,1) NOT NULL,
	[PassengerOrderID] [int] NOT NULL,
	[EscortName] [nvarchar](500) NOT NULL,
	[EscortIDNo] [nvarchar](11) NOT NULL,
	[EscortAge] [nvarchar](3) NULL,
	[EscortRelativeRelation] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_PassengerOrdersEscorts] PRIMARY KEY CLUSTERED 
(
	[PassengerOrderEscortID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PassengerOrdersEscorts]  WITH CHECK ADD  CONSTRAINT [FK_PassengerOrdersEscorts_PassengerOrders] FOREIGN KEY([PassengerOrderID])
REFERENCES [dbo].[PassengerOrders] ([PassengerOrderID])
GO

ALTER TABLE [dbo].[PassengerOrdersEscorts] CHECK CONSTRAINT [FK_PassengerOrdersEscorts_PassengerOrders]
GO
--USE [HCM]
--GO
--/****** Object:  UserDefinedFunction [dbo].[fnGetCurrentQualification]    Script Date: 12/05/42 08:31:10 م ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--SELECT DBO.[fnGetPassengerOrderTravelLine](87)
CREATE Function [dbo].[fnGetPassengerOrderTravelLine]			 
(
	@PassengerOrderID int
)
Returns nvarchar(MAX)
BEGIN

	DECLARE @TravelLine  nvarchar(MAX)=''
	DECLARE @Counter INT=0
	DECLARE @PassengerOrdersItineraryID INT
	DECLARE WrapperCussor CURSOR LOCAL FAST_FORWARD FOR
	 SELECT C.PassengerOrdersItineraryID FROM PassengerOrdersItineraries C
	 WHERE C.PassengerOrderID=@PassengerOrderID
	OPEN WrapperCussor
	FETCH NEXT FROM WrapperCussor INTO @PassengerOrdersItineraryID
	
	WHILE @@FETCH_STATUS = 0
	BEGIN

	SELECT  @TravelLine=@TravelLine+C.FromCity+' - '+C.ToCity 
	FROM PassengerOrdersItineraries C WHERE C.PassengerOrdersItineraryID= @PassengerOrdersItineraryID
	
 
	set @TravelLine=@TravelLine+' - '
 
	SET @Counter=@Counter+1

	FETCH NEXT FROM WrapperCussor INTO @PassengerOrdersItineraryID
 	END
	CLOSE WrapperCussor;
	DEALLOCATE WrapperCussor;
	RETURN @TravelLine  
END
GO

-- =============================================
-- Author:		<Author,,Name>
-- ALTER date: <ALTER Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spGetPassengerOrdersByEmployeeCodeID]--21006
(
	@EmployeeCodeID int
)
AS
BEGIN  

	SELECT PassengerOrders.*,
	  case when PassengerOrders.Going = 1 then N'نعم' else N'لا' end as GoingText, 
	  case when PassengerOrders.Returning = 1 then N'نعم' else N'لا' end as ReturningText,  
	  PassengerOrders.Reason, vwEmployeesCodes.EmployeeCodeNo, vwEmployeesCodes.EmployeeIDNo, 
	  vwEmployeesCodes.EmployeeNameAr, Ranks.RankName, TicketsClasses.TicketClassName
	FROM PassengerOrders	
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = PassengerOrders.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID=EmployeesCareersHistory.EmployeeCodeID
	LEFT OUTER JOIN Ranks ON PassengerOrders.RankID = Ranks.RankID 
	LEFT OUTER JOIN TicketsClasses ON PassengerOrders.TicketClassID = TicketsClasses.TicketClassID
	WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	
END
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetPassengerOrdersEscortsByPassengerOrderID]
(
	@PassengerOrderID int
)
AS
BEGIN
	
	SELECT * 
	FROM PassengerOrdersEscorts 
	WHERE PassengerOrdersEscorts.PassengerOrderID = @PassengerOrderID
	
END

 GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spGetPassengerOrdersItinerariesByPassengerOrderID]
(
	@PassengerOrderID int
)
AS
BEGIN
	
	SELECT * 
	FROM PassengerOrdersItineraries 
	WHERE PassengerOrdersItineraries.PassengerOrderID = @PassengerOrderID
	
END


