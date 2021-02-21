-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetPassengerOrdersByEmployeeCodeIDOld]--21006
(
	@EmployeeCodeID int
)
AS
BEGIN 
	
	DECLARE @tbl TABLE (ID int, DetailID int, EmployeeCareerHistoryID int, StartDate date, EndDate date, Reason nvarchar(500), PassengerOrderTypeID int)

	INSERT INTO @tbl SELECT Delegations.DelegationID, DelegationsDetails.DelegationDetailID, DelegationsDetails.EmployeeCareerHistoryID, Delegations.DelegationStartDate, Delegations.DelegationEndDate, 
							Delegations.DelegationReason, 1
						FROM Delegations INNER JOIN
							DelegationsDetails ON Delegations.DelegationID = DelegationsDetails.DelegationID 
							
	INSERT INTO @tbl SELECT InternshipScholarships.InternshipScholarshipID, InternshipScholarshipsDetails.InternshipScholarshipDetailID, 
						  InternshipScholarshipsDetails.EmployeeCareerHistoryID, InternshipScholarships.InternshipScholarshipStartDate, 
						  InternshipScholarships.InternshipScholarshipEndDate, InternshipScholarships.InternshipScholarshipReason, 2
						FROM InternshipScholarships INNER JOIN
						  InternshipScholarshipsDetails ON InternshipScholarships.InternshipScholarshipID = InternshipScholarshipsDetails.InternshipScholarshipID


	SELECT PassengerOrders.*,
	  case when PassengerOrders.Going = 1 then N'نعم' else N'لا' end as GoingText, 
	  case when PassengerOrders.Returning = 1 then N'نعم' else N'لا' end as ReturningText,  
	  Detail.StartDate, Detail.EndDate, Detail.Reason, vwEmployeesCodes.EmployeeCodeNo, vwEmployeesCodes.EmployeeIDNo, 
	  vwEmployeesCodes.EmployeeNameAr, Ranks.RankName, TicketsClasses.TicketClassName, 
	  PassengerOrdersTypes.PassengerOrderTypeName
	FROM PassengerOrdersOld as PassengerOrders
	INNER JOIN PassengerOrdersTypes on PassengerOrdersTypes.PassengerOrderTypeID = PassengerOrders.PassengerOrderTypeID
	INNER JOIN @tbl Detail ON PassengerOrders.DetailID = Detail.DetailID AND Detail.PassengerOrderTypeID = PassengerOrders.PassengerOrderTypeID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = Detail.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID=EmployeesCareersHistory.EmployeeCodeID
	LEFT OUTER JOIN Ranks ON PassengerOrders.RankID = Ranks.RankID 
	LEFT OUTER JOIN TicketsClasses ON PassengerOrders.TicketClassID = TicketsClasses.TicketClassID
	WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	
END
