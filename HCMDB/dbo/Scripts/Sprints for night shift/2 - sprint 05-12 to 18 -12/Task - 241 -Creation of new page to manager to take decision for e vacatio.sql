
ALTER TABLE EVacationsRequests ADD ActualOrganizationID INT NULL
GO

ALTER TABLE EVacationsRequests ADD ActualJobID INT NULL
GO

ALTER TABLE [dbo].[EVacationsRequests]  WITH CHECK ADD  CONSTRAINT [FK_EVacationsRequests_Jobs] FOREIGN KEY([ActualJobID])
REFERENCES [dbo].[Jobs] ([JobID])
GO

ALTER TABLE [dbo].[EVacationsRequests] CHECK CONSTRAINT [FK_EVacationsRequests_Jobs]
GO

ALTER TABLE [dbo].[EVacationsRequests]  WITH CHECK ADD  CONSTRAINT [FK_EVacationsRequests_OrganizationsStructures] FOREIGN KEY([ActualOrganizationID])
REFERENCES [dbo].[OrganizationsStructures] ([OrganizationID])
GO

ALTER TABLE [dbo].[EVacationsRequests] CHECK CONSTRAINT [FK_EVacationsRequests_OrganizationsStructures]
GO

ALTER TABLE Vacations ADD EVacationRequestID INT NULL
GO

ALTER TABLE [dbo].[Vacations]  WITH CHECK ADD  CONSTRAINT [FK_Vacations_EVacationsRequests] FOREIGN KEY([EVacationRequestID])
REFERENCES [dbo].[EVacationsRequests] ([EVacationRequestID])
GO

ALTER TABLE [dbo].[Vacations] CHECK CONSTRAINT [FK_Vacations_EVacationsRequests]
GO

ALTER TABLE [dbo].[EVacationsRequestsStatus] ADD IsForAuthorizedPerson BIT NULL
GO

INSERT INTO [dbo].[EVacationRequestsStatus]
           ([EVacationRequestStatusID]
           ,[EVacationRequestStatusName])
     VALUES
           (5,
           N'ملغاة عن طريق الموارد البشرية')
GO

UPDATE  [dbo].[EVacationRequestsStatus]
SET  [EVacationRequestStatusName] = N'ملغاة عن طريق منشئ الطلب'
WHERE [EVacationRequestStatusID] = 4
GO



