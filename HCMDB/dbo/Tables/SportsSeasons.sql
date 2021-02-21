CREATE TABLE [dbo].[SportsSeasons] (
    [SportsSeasonID]          INT            IDENTITY (1, 1) NOT NULL,
    [MaturityYearID]          INT            NOT NULL,
    [SportsSeasonDescription] NVARCHAR (450) NOT NULL,
    [SportsSeasonStartDate]   DATE           NOT NULL,
    [SportsSeasonEndDate]     DATE           NOT NULL,
    [CreatedBy]               INT            NULL,
    [CreatedDate]             DATETIME       NULL,
    [Updatedby]               INT            NULL,
    [UpdatedDate]             DATETIME       NULL,
    CONSTRAINT [PK_SportsSeasons] PRIMARY KEY CLUSTERED ([SportsSeasonID] ASC),
    CONSTRAINT [FK_SportsSeasons_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_SportsSeasons_MaturityYearsBalances] FOREIGN KEY ([MaturityYearID]) REFERENCES [dbo].[MaturityYearsBalances] ([MaturityYearID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [PK_SportsSeasons_Unique]
    ON [dbo].[SportsSeasons]([MaturityYearID] ASC, [SportsSeasonDescription] ASC);

