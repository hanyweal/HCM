-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetPassengerOrdersItinerariesByPassengerOrderID]
(
	@PassengerOrderID int
)
AS
BEGIN
	
	SELECT * 
	FROM PassengerOrdersItineraries 
	WHERE PassengerOrdersItineraries.PassengerOrderID = @PassengerOrderID
	
END

 