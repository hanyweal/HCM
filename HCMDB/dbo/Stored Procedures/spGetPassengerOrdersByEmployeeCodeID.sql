-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetPassengerOrdersByEmployeeCodeID]--21006
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
