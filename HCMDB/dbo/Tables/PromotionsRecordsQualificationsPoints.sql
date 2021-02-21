CREATE TABLE [dbo].[PromotionsRecordsQualificationsPoints] (
    [PromotionRecordQualificationPointID] INT            IDENTITY (1, 1) NOT NULL,
    [PromotionRecordID]                   INT            NOT NULL,
    [QualificationDegreeID]               INT            NULL,
    [QualificationID]                     INT            NULL,
    [GeneralSpecializationID]             INT            NULL,
    [PromotionRecordQualificationKindID]  INT            NULL,
    [Points]                              DECIMAL (5, 2) NULL,
    [CreatedDate]                         DATETIME       CONSTRAINT [DF_PromotionsRecordsQualificationsPoints_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]                           INT            NOT NULL,
    [LastUpdatedDate]                     DATETIME       NULL,
    [LastUpdatedBy]                       INT            NULL,
    CONSTRAINT [PK_PromotionsRecordsQualificationsPoints] PRIMARY KEY CLUSTERED ([PromotionRecordQualificationPointID] ASC),
    CONSTRAINT [FK_PromotionsRecordsQualificationsPoints_GeneralSpecializations] FOREIGN KEY ([GeneralSpecializationID]) REFERENCES [dbo].[GeneralSpecializations] ([GeneralSpecializationID]),
    CONSTRAINT [FK_PromotionsRecordsQualificationsPoints_PromotionsRecords] FOREIGN KEY ([PromotionRecordID]) REFERENCES [dbo].[PromotionsRecords] ([PromotionRecordID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PromotionsRecordsQualificationsPoints_PromotionsRecordsQualificationsKinds] FOREIGN KEY ([PromotionRecordQualificationKindID]) REFERENCES [dbo].[PromotionsRecordsQualificationsKinds] ([PromotionRecordQualificationKindID]),
    CONSTRAINT [FK_PromotionsRecordsQualificationsPoints_Qualifications] FOREIGN KEY ([QualificationID]) REFERENCES [dbo].[Qualifications] ([QualificationID]),
    CONSTRAINT [FK_PromotionsRecordsQualificationsPoints_QualificationsDegrees] FOREIGN KEY ([QualificationDegreeID]) REFERENCES [dbo].[QualificationsDegrees] ([QualificationDegreeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[PromotionsRecordsQualificationsPoints]([PromotionRecordID] ASC, [QualificationDegreeID] ASC, [QualificationID] ASC, [GeneralSpecializationID] ASC);

