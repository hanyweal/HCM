CREATE TABLE [dbo].[QualificationsTypes] (
    [QualificationTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [QualificationTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_QualificationsTypes] PRIMARY KEY CLUSTERED ([QualificationTypeID] ASC)
);

