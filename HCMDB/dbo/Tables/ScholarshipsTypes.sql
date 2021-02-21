CREATE TABLE [dbo].[ScholarshipsTypes] (
    [ScholarshipTypeID] INT           NOT NULL,
    [ScholarshipType]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ScholarshipTypes] PRIMARY KEY CLUSTERED ([ScholarshipTypeID] ASC)
);

