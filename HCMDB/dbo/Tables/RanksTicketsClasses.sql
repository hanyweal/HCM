CREATE TABLE [dbo].[RanksTicketsClasses] (
    [RankTicketClassID] INT      IDENTITY (1, 1) NOT NULL,
    [RankID]            INT      NULL,
    [TicketClassID]     INT      NULL,
    [CreatedDate]       DATETIME CONSTRAINT [DF_RanksTicketsClasses_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]   DATETIME NULL,
    CONSTRAINT [PK_RanksTicketsClasses] PRIMARY KEY CLUSTERED ([RankTicketClassID] ASC),
    CONSTRAINT [FK_RanksTicketsClasses_Ranks] FOREIGN KEY ([RankID]) REFERENCES [dbo].[Ranks] ([RankID]),
    CONSTRAINT [FK_RanksTicketsClasses_TicketsClasses] FOREIGN KEY ([TicketClassID]) REFERENCES [dbo].[TicketsClasses] ([TicketClassID])
);

