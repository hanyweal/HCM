CREATE TABLE [dbo].[StopWorksCategories] (
    [StopWorkCategoryID]   INT           IDENTITY (1, 1) NOT NULL,
    [StopWorkCategoryName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_StopWorksCategories] PRIMARY KEY CLUSTERED ([StopWorkCategoryID] ASC)
);

