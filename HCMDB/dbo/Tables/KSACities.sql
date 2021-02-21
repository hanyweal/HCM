CREATE TABLE [dbo].[KSACities] (
    [KSACityID]       INT            IDENTITY (1, 1) NOT NULL,
    [KSARegionID]     INT            NOT NULL,
    [KSACityName]     NVARCHAR (100) NULL,
    [CreatedDate]     DATETIME       CONSTRAINT [DF_KSACities_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate] DATETIME       NULL,
    CONSTRAINT [PK_KSACities] PRIMARY KEY CLUSTERED ([KSACityID] ASC),
    CONSTRAINT [FK_KSACities_KSARegions] FOREIGN KEY ([KSARegionID]) REFERENCES [dbo].[KSARegions] ([KSARegionID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[KSACities]([KSACityName] ASC);

