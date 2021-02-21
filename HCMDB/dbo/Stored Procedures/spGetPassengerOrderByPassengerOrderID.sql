-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetPassengerOrderByPassengerOrderID]
(
	@PassengerOrderID int
)
AS
BEGIN  

	SELECT PassengerOrders.PassengerOrderID, PassengerOrders.EmployeeCareerHistoryID, PassengerOrders.TravellingDate, PassengerOrders.RankID, 
	  PassengerOrders.TicketClassID, 
	  CASE WHEN PassengerOrders.Going = 1 THEN N'نعم' ELSE N'لا' END AS GoingText, 
	  CASE WHEN PassengerOrders.Returning = 1 THEN N'نعم' ELSE N'لا' END AS ReturningText,  
	  PassengerOrders.CreatedDate, PassengerOrders.LastUpdatedDate, 
	  PassengerOrders.Reason, vwEmployeesCodes.EmployeeCodeNo, Employees.EmployeeIDNo, 
	  vwEmployeesCodes.EmployeeNameAr, Ranks.RankName, TicketsClasses.TicketClassName
	FROM PassengerOrders 
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = PassengerOrders.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID=EmployeesCareersHistory.EmployeeCodeID
	INNER JOIN Employees ON vwEmployeesCodes.EmployeeID = Employees.EmployeeID 
	LEFT OUTER JOIN Ranks ON PassengerOrders.RankID = Ranks.RankID 
	LEFT OUTER JOIN TicketsClasses ON PassengerOrders.TicketClassID = TicketsClasses.TicketClassID
	WHERE PassengerOrders.PassengerOrderID = @PassengerOrderID
	
END
