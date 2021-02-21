SET IDENTITY_INSERT CareersHistoryTypes ON
INSERT INTO CareersHistoryTypes(CareerHistoryTypeID, CareerHistoryTypeName) VALUES (6, N'نقل بوظيفة')
SET IDENTITY_INSERT CareersHistoryTypes OFF
GO

SET IDENTITY_INSERT CareersHistoryTypes ON
INSERT INTO CareersHistoryTypes(CareerHistoryTypeID, CareerHistoryTypeName) VALUES (7, N'نقل بدون وظيفة')
SET IDENTITY_INSERT CareersHistoryTypes OFF
GO

ALTER TABLE [dbo].[Transfers] ADD IsProcessed BIT NULL
GO


