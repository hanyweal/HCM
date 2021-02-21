CREATE TABLE [dbo].[PromotionsRecordsEmployees] (
    [PromotionRecordEmployeeID]       INT            IDENTITY (1, 1) NOT NULL,
    [PromotionRecordID]               INT            NOT NULL,
    [PromotionRecordJobVacancyID]     INT            NULL,
    [NewEmployeeCareerHistoryID]      INT            NULL,
    [CurrentEmployeeCareerHistoryID]  INT            NULL,
    [CurrentJobDaysCount]             INT            NULL,
    [PreviousEmployeeCareerHistoryID] INT            NULL,
    [PreviousJobDaysCount]            INT            NOT NULL,
    [LastQualificationDegreeID]       INT            NULL,
    [LastQualificationID]             INT            NULL,
    [LastGeneralSpecializationID]     INT            NULL,
    [OnGoingServiceDaysCount]         INT            NULL,
    [PriorServiceDaysCount]           INT            NULL,
    [EducationPoints]                 DECIMAL (5, 2) NULL,
    [SeniorityPoints]                 DECIMAL (5, 2) NULL,
    [EvaluationPoints]                DECIMAL (5, 2) NULL,
    [IsIncluded]                      BIT            NOT NULL,
    [PromotionCandidateAddedWayID]    INT            NOT NULL,
    [ManualAddedReason]               NVARCHAR (MAX) NULL,
    [IsDeserveExtraBonus]             BIT            NULL,
    [IsRemovedAfterIncluding]         BIT            NULL,
    [RemovingReason]                  NVARCHAR (MAX) NULL,
    [RemovingBy]                      INT            NULL,
    [PromotionDecisionID]             INT            NULL,
    [IsApproved]                      BIT            NULL,
    [ApprovedDate]                    DATETIME       NULL,
    [ApprovedBy]                      INT            NULL,
    [CreatedDate]                     DATETIME       NOT NULL,
    [CreatedBy]                       INT            NOT NULL,
    [LastUpdatedDate]                 DATETIME       NULL,
    [LastUpdatedBy]                   INT            NULL,
    CONSTRAINT [PK_PromotionsRecordsEmployees] PRIMARY KEY CLUSTERED ([PromotionRecordEmployeeID] ASC),
    CONSTRAINT [FK_PromotionsRecordsEmployees_EmployeesCareersHistory] FOREIGN KEY ([CurrentEmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_EmployeesCodes2] FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_EmployeesCodes3] FOREIGN KEY ([RemovingBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_GeneralSpecializations] FOREIGN KEY ([LastGeneralSpecializationID]) REFERENCES [dbo].[GeneralSpecializations] ([GeneralSpecializationID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_NewEmployeesCareersHistory] FOREIGN KEY ([NewEmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_PreviousEmployeesCareersHistory] FOREIGN KEY ([PreviousEmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_PromotionsCandidatesAddedWays] FOREIGN KEY ([PromotionCandidateAddedWayID]) REFERENCES [dbo].[PromotionsCandidatesAddedWays] ([PromotionCandidateAddedWayID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PromotionsRecordsEmployees_PromotionsDecisions] FOREIGN KEY ([PromotionDecisionID]) REFERENCES [dbo].[PromotionsDecisions] ([PromotionDecisionID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_PromotionsRecords] FOREIGN KEY ([PromotionRecordID]) REFERENCES [dbo].[PromotionsRecords] ([PromotionRecordID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PromotionsRecordsEmployees_PromotionsRecordsJobsVacancies] FOREIGN KEY ([PromotionRecordJobVacancyID]) REFERENCES [dbo].[PromotionsRecordsJobsVacancies] ([PromotionRecordJobVacancyID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_Qualifications] FOREIGN KEY ([LastQualificationID]) REFERENCES [dbo].[Qualifications] ([QualificationID]),
    CONSTRAINT [FK_PromotionsRecordsEmployees_QualificationsDegrees] FOREIGN KEY ([LastQualificationDegreeID]) REFERENCES [dbo].[QualificationsDegrees] ([QualificationDegreeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_PromotionRecordJobVacancyID]
    ON [dbo].[PromotionsRecordsEmployees]([PromotionRecordJobVacancyID] ASC) WHERE ([PromotionRecordJobVacancyID] IS NOT NULL);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[PromotionsRecordsEmployees]([PromotionRecordID] ASC, [CurrentEmployeeCareerHistoryID] ASC);

