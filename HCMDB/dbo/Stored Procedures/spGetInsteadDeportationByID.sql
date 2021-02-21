
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetInsteadDeportationByID]
(
	@InsteadDeportationID int
)
AS
BEGIN 

	SELECT InsteadDeportations.InsteadDeportationID, 
		InsteadDeportations.EmployeeCareerHistoryID, 
		vwEmployeesCareersHistory.EmployeeCodeID,
		InsteadDeportations.DeportationDate,
		mic_db.dbo.fn_GregToUmAlqura(cast(InsteadDeportations.DeportationDate as date)) AS DeportationDateUmAlQura,
		InsteadDeportations.Amount, 
		InsteadDeportations.Note
	FROM InsteadDeportations 
		INNER JOIN vwEmployeesCareersHistory ON InsteadDeportations.EmployeeCareerHistoryID = vwEmployeesCareersHistory.EmployeeCareerHistoryID
	WHERE InsteadDeportations.InsteadDeportationID = @InsteadDeportationID
 
END
