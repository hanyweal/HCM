CREATE TABLE [dbo].[PromotionsRecordsJobsVacancies] (
    [PromotionRecordJobVacancyID] INT      IDENTITY (1, 1) NOT NULL,
    [OrganizationJobID]           INT      NOT NULL,
    [PromotionRecordID]           INT      NOT NULL,
    [CreatedBy]                   INT      NOT NULL,
    [CreatedDate]                 DATETIME CONSTRAINT [DF_PromotionsRecordsJobsVacancies_CreatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_PromotionsRecordsJobsVacancies] PRIMARY KEY CLUSTERED ([PromotionRecordJobVacancyID] ASC),
    CONSTRAINT [FK_PromotionsRecordsJobsVacancies_OrganizationsJobs] FOREIGN KEY ([OrganizationJobID]) REFERENCES [dbo].[OrganizationsJobs] ([OrganizationJobID]),
    CONSTRAINT [FK_PromotionsRecordsJobsVacancies_PromotionsRecords] FOREIGN KEY ([PromotionRecordID]) REFERENCES [dbo].[PromotionsRecords] ([PromotionRecordID]) ON DELETE CASCADE ON UPDATE CASCADE
);

