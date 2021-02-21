CREATE TABLE [dbo].[OverTimesDays] (
    [OverTimeDayID] INT  IDENTITY (1, 1) NOT NULL,
    [OverTimeID]    INT  NOT NULL,
    [OverTimeDay]   DATE NOT NULL,
    CONSTRAINT [PK_OverTimesDays] PRIMARY KEY CLUSTERED ([OverTimeDayID] ASC),
    CONSTRAINT [FK_OverTimesDays_OverTimes] FOREIGN KEY ([OverTimeID]) REFERENCES [dbo].[OverTimes] ([OverTimeID]) ON DELETE CASCADE
);

