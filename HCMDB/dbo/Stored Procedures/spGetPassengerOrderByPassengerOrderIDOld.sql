-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetPassengerOrderByPassengerOrderIDOld]
(
	@PassengerOrderID int
)
AS
BEGIN  

	DECLARE @PassengerOrderTypeID INT 
	SELECT @PassengerOrderTypeID = PassengerOrderTypeID FROM PassengerOrdersOld PassengerOrders WHERE PassengerOrders.PassengerOrderID = @PassengerOrderID
	
	DECLARE @tbl TABLE (ID int, DetailID int, EmployeeCareerHistoryID int, StartDate date, EndDate date, Reason nvarchar(500))
	
	IF @PassengerOrderTypeID = 1 
	BEGIN
		INSERT INTO @tbl SELECT Delegations.DelegationID, DelegationsDetails.DelegationDetailID, DelegationsDetails.EmployeeCareerHistoryID, Delegations.DelegationStartDate, Delegations.DelegationEndDate, 
								Delegations.DelegationReason
							FROM Delegations INNER JOIN
								DelegationsDetails ON Delegations.DelegationID = DelegationsDetails.DelegationID 
	END		
	ELSE
	BEGIN
		INSERT INTO @tbl SELECT InternshipScholarships.InternshipScholarshipID, InternshipScholarshipsDetails.InternshipScholarshipDetailID, 
							  InternshipScholarshipsDetails.EmployeeCareerHistoryID, InternshipScholarships.InternshipScholarshipStartDate, 
							  InternshipScholarships.InternshipScholarshipEndDate, InternshipScholarships.InternshipScholarshipReason
							FROM InternshipScholarships INNER JOIN
							  InternshipScholarshipsDetails ON InternshipScholarships.InternshipScholarshipID = InternshipScholarshipsDetails.InternshipScholarshipID
	END



	SELECT PassengerOrders.PassengerOrderID, PassengerOrders.DetailID, PassengerOrders.TravellingDate, PassengerOrders.RankID, 
	  PassengerOrders.TicketClassID, 
	  CASE WHEN PassengerOrders.Going = 1 THEN N'نعم' ELSE N'لا' END AS GoingText, 
	  CASE WHEN PassengerOrders.Returning = 1 THEN N'نعم' ELSE N'لا' END AS ReturningText,  
	  PassengerOrders.CreatedDate, PassengerOrders.LastUpdatedDate, 
	  detail.StartDate, detail.EndDate,  
	  mic_db.dbo.fn_GregToUmAlqura(detail.StartDate) AS StartDateUmAlQura, 
	  mic_db.dbo.fn_GregToUmAlqura(detail.EndDate) AS EndDateUmAlQura, 
	  detail.Reason, vwEmployeesCodes.EmployeeCodeNo, Employees.EmployeeIDNo, 
	  vwEmployeesCodes.EmployeeNameAr, Ranks.RankName, TicketsClasses.TicketClassName, PassengerOrders.PassengerOrderTypeID, 
	  PassengerOrdersTypes.PassengerOrderTypeName
	FROM PassengerOrdersOld PassengerOrders 
	INNER JOIN PassengerOrdersTypes on PassengerOrdersTypes.PassengerOrderTypeID = PassengerOrders.PassengerOrderTypeID
	INNER JOIN @tbl detail ON PassengerOrders.DetailID = detail.DetailID 
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = detail.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID=EmployeesCareersHistory.EmployeeCodeID
	INNER JOIN Employees ON vwEmployeesCodes.EmployeeID = Employees.EmployeeID 
	LEFT OUTER JOIN Ranks ON PassengerOrders.RankID = Ranks.RankID 
	LEFT OUTER JOIN TicketsClasses ON PassengerOrders.TicketClassID = TicketsClasses.TicketClassID
	WHERE PassengerOrders.PassengerOrderID = @PassengerOrderID
	
END
