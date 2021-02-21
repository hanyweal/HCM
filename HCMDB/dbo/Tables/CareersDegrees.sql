CREATE TABLE [dbo].[CareersDegrees] (
    [CareerDegreeID]   INT           IDENTITY (1, 1) NOT NULL,
    [CareerDegreeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CareersDegrees] PRIMARY KEY CLUSTERED ([CareerDegreeID] ASC)
);

