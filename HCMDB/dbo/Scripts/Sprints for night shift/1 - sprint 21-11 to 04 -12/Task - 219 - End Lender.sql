GO

ALTER TABLE Lenders ADD LenderEndReason nvarchar(MAX) NULL
GO
ALTER TABLE Lenders ADD EmployeeCareerHistoryID INT NULL
Go
UPDATE Lenders SET EmployeeCareerHistoryID = (SELECT EmployeeCareerHistoryID  FROM EmployeesCareersHistory ech WHERE ech.IsActive = 1 AND ech.EmployeeCodeID = Lenders.EmployeeCodeID)
Go
ALTER TABLE [dbo].[Lenders] DROP CONSTRAINT [FK_Lenders_EmployeesCodes]
GO
ALTER TABLE Lenders DROP COLUMN EmployeeCodeID
GO

ALTER TABLE [dbo].[Lenders]  WITH CHECK ADD  CONSTRAINT [FK_Lenders_EmployeesCareersHistory] FOREIGN KEY([EmployeeCareerHistoryID])
REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID])
GO

ALTER TABLE [dbo].Lenders CHECK CONSTRAINT [FK_Lenders_EmployeesCareersHistory]
GO

ALTER TABLE Lenders ALTER COLUMN EmployeeCareerHistoryID INT NOT NULL


GO
/****** Object:  StoredProcedure [dbo].[spGetLendersByEmployeeCodeID]    Script Date: 22/04/42 09:12:05 ص ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spGetLendersByEmployeeCodeID] --93
(
	@EmployeeCodeID int
)
AS
BEGIN

	SELECT Lenders.*,
			KSACities.KSACityName
	FROM Lenders
	INNER JOIN EmployeesCareersHistory On EmployeesCareersHistory.EmployeeCareerHistoryID = Lenders.EmployeeCareerHistoryID
	INNER JOIN vwEmployeesCodes ON EmployeesCareersHistory.EmployeeCodeID = vwEmployeesCodes.EmployeeCodeID
	LEFT JOIN KSACities ON Lenders.KSACityID = KSACities.KSACityID
	WHERE vwEmployeesCodes.EmployeeCodeID = @EmployeeCodeID
	
END

GO
/****** Object:  StoredProcedure [dbo].[spGetLenderByLenderID]    Script Date: 22/04/42 09:12:31 ص ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spGetLenderByLenderID]
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
		Lenders.EmployeeCareerHistoryID,
		Lenders.IsFinished,
		Lenders.CreatedDate,
		Lenders.CreatedBy,
		Lenders.LastUpdatedDate,
		Lenders.LastUpdatedBy, 
		KSACities.KSACityName,
		EmployeesCareersHistory.EmployeeCodeID
	FROM Lenders
	INNER JOIN KSACities ON Lenders.KSACityID = KSACities.KSACityID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory .EmployeeCareerHistoryID = Lenders.EmployeeCareerHistoryID
	WHERE LenderID = @LenderID
END

Go
