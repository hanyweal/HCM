

CREATE PROC [dbo].[spGetTransfersByTransferID]--12
(
	 @TransferID INT
)
AS
SELECT     Transfers.TransferID, Transfers.EmployeeCareerHistoryID, Transfers.TransferTypeID, 
					Transfers.TransferDate, 
					mic_db.dbo.fn_GregToUmAlqura(convert(date,Transfers.TransferDate)) AS TransferDateUmAlQura,
					Transfers.Destination, Transfers.KSACityID, 
                      Transfers.JobName, 
                      EmployeesCareersHistory.EmployeeCodeID,
                      Transfers.RankName, 
                      Transfers.JobCode, 
                      Transfers.OrganizationName, 
                      Transfers.CareerDegreeName, 
                      TransfersTypes.TransferTypeName, 
                      KSACities.KSACityName
FROM         Transfers INNER JOIN
                      TransfersTypes ON Transfers.TransferTypeID = TransfersTypes.TransferTypeID INNER JOIN
                      EmployeesCareersHistory ON Transfers.EmployeeCareerHistoryID = EmployeesCareersHistory.EmployeeCareerHistoryID  INNER JOIN 
                      vwEmployeesCodes ON vwEmployeesCodes.EmployeeCodeID=EmployeesCareersHistory.EmployeeCodeID INNER JOIN
                      Employees ON vwEmployeesCodes.EmployeeID = Employees.EmployeeID INNER JOIN
                      KSACities ON Transfers.KSACityID = KSACities.KSACityID
WHERE     (Transfers.TransferID = @TransferID)
