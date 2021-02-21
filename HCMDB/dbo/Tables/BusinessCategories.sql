CREATE TABLE [dbo].[BusinessCategories] (
    [BusinessCategoryID] INT            NOT NULL,
    [BusinessCategoryAr] NVARCHAR (500) NOT NULL,
    [BusinessCategoryEn] NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_BusinessCategories] PRIMARY KEY CLUSTERED ([BusinessCategoryID] ASC)
);

