CREATE TABLE [dbo].[Countries] (
    [CountryID]       INT            IDENTITY (1, 1) NOT NULL,
    [CountryName]     NVARCHAR (100) NULL,
    [CreatedDate]     DATETIME       CONSTRAINT [DF_Countries_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate] DATETIME       NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([CountryID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[Countries]([CountryName] ASC);

