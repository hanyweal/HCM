CREATE TABLE [dbo].[StopWorksTypes] (
    [StopWorkTypeID]     INT           IDENTITY (1, 1) NOT NULL,
    [StopWorkCategoryID] INT           NOT NULL,
    [StopWorkTypeName]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_StopWorksTypes] PRIMARY KEY CLUSTERED ([StopWorkTypeID] ASC),
    CONSTRAINT [FK_StopWorksTypes_StopWorksCategories] FOREIGN KEY ([StopWorkCategoryID]) REFERENCES [dbo].[StopWorksCategories] ([StopWorkCategoryID])
);

