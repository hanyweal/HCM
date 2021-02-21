CREATE TABLE [dbo].[PromotionsRecordsEmployeesEducationsDetails] (
    [PromotionRecordEmployeeEducationDetailID] INT            IDENTITY (1, 1) NOT NULL,
    [PromotionRecordEmployeeID]                INT            NOT NULL,
    [QualificationDegreeID]                    INT            NOT NULL,
    [QualificationID]                          INT            NOT NULL,
    [GeneralSpecializationID]                  INT            NOT NULL,
    [Weight]                                   INT            NOT NULL,
    [Points]                                   DECIMAL (5, 2) NOT NULL,
    [IsIncluded]                               BIT            NOT NULL,
    CONSTRAINT [PK_PromotionsRecordsEmployeesEducationsDetails] PRIMARY KEY CLUSTERED ([PromotionRecordEmployeeEducationDetailID] ASC),
    CONSTRAINT [FK_PromotionsRecordsEmployeesEducationsDetails_GeneralSpecializations] FOREIGN KEY ([GeneralSpecializationID]) REFERENCES [dbo].[GeneralSpecializations] ([GeneralSpecializationID]),
    CONSTRAINT [FK_PromotionsRecordsEmployeesEducationsDetails_PromotionsRecordsEmployees] FOREIGN KEY ([PromotionRecordEmployeeID]) REFERENCES [dbo].[PromotionsRecordsEmployees] ([PromotionRecordEmployeeID]),
    CONSTRAINT [FK_PromotionsRecordsEmployeesEducationsDetails_Qualifications] FOREIGN KEY ([QualificationID]) REFERENCES [dbo].[Qualifications] ([QualificationID]),
    CONSTRAINT [FK_PromotionsRecordsEmployeesEducationsDetails_QualificationsDegrees] FOREIGN KEY ([QualificationDegreeID]) REFERENCES [dbo].[QualificationsDegrees] ([QualificationDegreeID])
);

