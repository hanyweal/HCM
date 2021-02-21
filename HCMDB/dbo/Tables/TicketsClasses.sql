CREATE TABLE [dbo].[TicketsClasses] (
    [TicketClassID]   INT            IDENTITY (1, 1) NOT NULL,
    [TicketClassName] NVARCHAR (100) NOT NULL,
    [CreatedDate]     DATETIME       CONSTRAINT [DF_TicketsClasses_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate] DATETIME       NULL,
    CONSTRAINT [PK_TicketsClasses] PRIMARY KEY CLUSTERED ([TicketClassID] ASC)
);

