CREATE TABLE [dbo].[JobsCategoriesQualifications] (
    [JobCategoryQualificationID] INT      IDENTITY (1, 1) NOT NULL,
    [JobCategoryID]              INT      NOT NULL,
    [QualificationDegreeID]      INT      NOT NULL,
    [QualificationID]            INT      NULL,
    [GeneralSpecializationID]    INT      NULL,
    [CreatedBy]                  INT      NULL,
    [CreatedDate]                DATETIME NOT NULL,
    [LastUpdatedBy]              INT      NULL,
    [LastUpdatedDate]            DATETIME NULL,
    [IsMinQualification]         BIT      NOT NULL,
    [PromotionPeriodID]          INT      NOT NULL,
    CONSTRAINT [PK_JobsCategoriesQualifications] PRIMARY KEY CLUSTERED ([JobCategoryQualificationID] ASC),
    CONSTRAINT [FK_JobsCategoriesQualifications_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE,
    CONSTRAINT [FK_JobsCategoriesQualifications_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_JobsCategoriesQualifications_GeneralSpecializations] FOREIGN KEY ([GeneralSpecializationID]) REFERENCES [dbo].[GeneralSpecializations] ([GeneralSpecializationID]),
    CONSTRAINT [FK_JobsCategoriesQualifications_JobsCategories] FOREIGN KEY ([JobCategoryID]) REFERENCES [dbo].[JobsCategories] ([JobCategoryID]),
    CONSTRAINT [FK_JobsCategoriesQualifications_PromotionsPeriods] FOREIGN KEY ([PromotionPeriodID]) REFERENCES [dbo].[PromotionsPeriods] ([PromotionPeriodID]),
    CONSTRAINT [FK_JobsCategoriesQualifications_Qualifications] FOREIGN KEY ([QualificationID]) REFERENCES [dbo].[Qualifications] ([QualificationID]),
    CONSTRAINT [FK_JobsCategoriesQualifications_QualificationsDegrees] FOREIGN KEY ([QualificationDegreeID]) REFERENCES [dbo].[QualificationsDegrees] ([QualificationDegreeID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-JobCategory-QualificationDegree-Qualification-GeneralSpecialization-PromotionPeriod]
    ON [dbo].[JobsCategoriesQualifications]([JobCategoryID] ASC, [QualificationDegreeID] ASC, [QualificationID] ASC, [GeneralSpecializationID] ASC, [PromotionPeriodID] ASC);

