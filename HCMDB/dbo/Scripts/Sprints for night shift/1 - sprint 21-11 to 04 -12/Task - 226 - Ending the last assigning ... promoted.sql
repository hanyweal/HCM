GO

ALTER TABLE Assignings ADD EndAssigningReasonID int null 

GO

ALTER TABLE [dbo].[Assignings]  WITH CHECK ADD  CONSTRAINT [FK_Assignings_AssigningsReasons1] FOREIGN KEY([EndAssigningReasonID])
REFERENCES [dbo].[AssigningsReasons] ([AssigningReasonID])
GO

ALTER TABLE [dbo].[Assignings] CHECK CONSTRAINT [FK_Assignings_AssigningsReasons1]
GO

SET IDENTITY_INSERT AssigningsReasons ON
INSERT INTO AssigningsReasons (AssigningReasonID, AssigningReasonName) VALUES (4, N'Employee Promoted')
INSERT INTO AssigningsReasons (AssigningReasonID, AssigningReasonName) VALUES (5, N'StopWork Finished')
INSERT INTO AssigningsReasons (AssigningReasonID, AssigningReasonName) VALUES (6, N'Lender Finished')
INSERT INTO AssigningsReasons (AssigningReasonID, AssigningReasonName) VALUES (7, N'Created New Career History')
SET IDENTITY_INSERT AssigningsReasons OFF
