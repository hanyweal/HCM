CREATE TABLE [dbo].[KSARegions] (
    [KSARegionID]     INT            IDENTITY (1, 1) NOT NULL,
    [KSARegionName]   NVARCHAR (100) NULL,
    [CreatedDate]     DATETIME       CONSTRAINT [DF_KSARegions_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate] DATETIME       NULL,
    CONSTRAINT [PK_KSARegions] PRIMARY KEY CLUSTERED ([KSARegionID] ASC)
);

