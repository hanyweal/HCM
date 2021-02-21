CREATE TABLE [dbo].[BusinessSubCategories] (
    [BusinssSubCategoryID]  INT            NOT NULL,
    [BusinessSubCategoryAr] NVARCHAR (500) NOT NULL,
    [BusinessSubCategoryEn] NVARCHAR (500) NOT NULL,
    [BusinessCategoryID]    INT            NOT NULL,
    [EntityName]            NVARCHAR (100) NULL,
    CONSTRAINT [PK_BusinessSubCategories] PRIMARY KEY CLUSTERED ([BusinssSubCategoryID] ASC),
    CONSTRAINT [FK_BusinessSubCategories_BusinessCategories] FOREIGN KEY ([BusinessCategoryID]) REFERENCES [dbo].[BusinessCategories] ([BusinessCategoryID])
);

