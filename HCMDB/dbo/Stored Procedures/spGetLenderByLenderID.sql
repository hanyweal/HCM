-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetLenderByLenderID]
(
	@LenderID int
)
AS
BEGIN
	SELECT Lenders.LenderID,
		Lenders.LenderStartDate,
		Lenders.LenderEndDate,
		mic_db.dbo.fn_GregToUmAlqura(Lenders.LenderStartDate) AS LenderStartDateUmAlQura,		
		mic_db.dbo.fn_GregToUmAlqura(Lenders.LenderEndDate) AS LenderEndDateUmAlQura,
		Lenders.KSACityID,
		Lenders.LenderOrganization,
		Lenders.EmployeeCodeID,
		Lenders.IsFinished,
		Lenders.CreatedDate,
		Lenders.CreatedBy,
		Lenders.LastUpdatedDate,
		Lenders.LastUpdatedBy, 
		KSACities.KSACityName
	FROM Lenders
	INNER JOIN KSACities ON Lenders.KSACityID = KSACities.KSACityID
	WHERE LenderID = @LenderID
END
