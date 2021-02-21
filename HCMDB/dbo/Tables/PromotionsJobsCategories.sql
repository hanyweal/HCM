CREATE TABLE [dbo].[PromotionsJobsCategories] (
    [PromotionJobCategoryID] INT      IDENTITY (1, 1) NOT NULL,
    [PromotionPeriodID]      INT      NOT NULL,
    [JobCategoryID]          INT      NOT NULL,
    [AssignedJobCategoryID]  INT      NOT NULL,
    [CreatedDate]            DATETIME NOT NULL,
    [CreatedBy]              INT      NOT NULL,
    CONSTRAINT [PK_PromotionJobsCategories] PRIMARY KEY CLUSTERED ([PromotionJobCategoryID] ASC),
    CONSTRAINT [FK_PromotionJobsCategories_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionJobsCategories_JobsCategories] FOREIGN KEY ([JobCategoryID]) REFERENCES [dbo].[JobsCategories] ([JobCategoryID]),
    CONSTRAINT [FK_PromotionJobsCategories_JobsCategories1] FOREIGN KEY ([AssignedJobCategoryID]) REFERENCES [dbo].[JobsCategories] ([JobCategoryID]),
    CONSTRAINT [FK_PromotionJobsCategories_PromotionsPeriods] FOREIGN KEY ([PromotionPeriodID]) REFERENCES [dbo].[PromotionsPeriods] ([PromotionPeriodID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-JobCategory-AssignedJobCategory-PromotionPeriod]
    ON [dbo].[PromotionsJobsCategories]([PromotionPeriodID] ASC, [JobCategoryID] ASC, [AssignedJobCategoryID] ASC);

