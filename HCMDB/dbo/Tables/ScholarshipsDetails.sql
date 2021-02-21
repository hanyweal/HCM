CREATE TABLE [dbo].[ScholarshipsDetails] (
    [ScholarshipDetailID] INT            IDENTITY (1, 1) NOT NULL,
    [ScholarshipActionID] INT            NOT NULL,
    [ScholarshipID]       INT            NOT NULL,
    [FromDate]            DATE           NOT NULL,
    [ToDate]              DATE           NOT NULL,
    [Notes]               NVARCHAR (MAX) NULL,
    [CreatedBy]           INT            NOT NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    [LastUpdatedBy]       INT            NULL,
    [LastUpdatedDate]     DATETIME       NULL,
    CONSTRAINT [PK_ScholarshipsDetails] PRIMARY KEY CLUSTERED ([ScholarshipDetailID] ASC),
    CONSTRAINT [FK_ScholarshipsDetails_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_ScholarshipsDetails_EmployeesCodes2] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_ScholarshipsDetails_Scholarships] FOREIGN KEY ([ScholarshipID]) REFERENCES [dbo].[Scholarships] ([ScholarshipID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ScholarshipsDetails_ScholarshipsActions] FOREIGN KEY ([ScholarshipActionID]) REFERENCES [dbo].[ScholarshipsActions] ([ScholarshipActionID]) ON DELETE CASCADE ON UPDATE CASCADE
);

