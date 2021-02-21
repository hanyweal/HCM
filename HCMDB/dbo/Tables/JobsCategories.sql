CREATE TABLE [dbo].[JobsCategories] (
    [JobCategoryID]              INT            IDENTITY (1, 1) NOT NULL,
    [JobGroupID]                 INT            NOT NULL,
    [JobCategoryName]            NVARCHAR (200) NOT NULL,
    [CreatedDate]                DATETIME       CONSTRAINT [DF_JobsCategories_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]            DATETIME       NULL,
    [MinQualificationDegreeID]   INT            NULL,
    [MinQualificationID]         INT            NULL,
    [MinGeneralSpecializationID] INT            NULL,
    [CreatedBy]                  INT            NULL,
    [LastUpdatedBy]              INT            NULL,
    [JobCategoryNo]              NVARCHAR (5)   NULL,
    CONSTRAINT [PK_JobsCategories] PRIMARY KEY CLUSTERED ([JobCategoryID] ASC),
    CONSTRAINT [FK_JobsCategories_GeneralSpecializations] FOREIGN KEY ([MinGeneralSpecializationID]) REFERENCES [dbo].[GeneralSpecializations] ([GeneralSpecializationID]),
    CONSTRAINT [FK_JobsCategories_JobsGroups] FOREIGN KEY ([JobGroupID]) REFERENCES [dbo].[JobsGroups] ([JobGroupID]) ON DELETE CASCADE,
    CONSTRAINT [FK_JobsCategories_Qualifications] FOREIGN KEY ([MinQualificationID]) REFERENCES [dbo].[Qualifications] ([QualificationID]),
    CONSTRAINT [FK_JobsCategories_QualificationsDegrees] FOREIGN KEY ([MinQualificationDegreeID]) REFERENCES [dbo].[QualificationsDegrees] ([QualificationDegreeID])
);

