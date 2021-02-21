CREATE TABLE [dbo].[CareersHistoryTypes] (
    [CareerHistoryTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [CareerHistoryTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CareersHistoryTypes] PRIMARY KEY CLUSTERED ([CareerHistoryTypeID] ASC)
);

