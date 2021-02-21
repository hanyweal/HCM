CREATE TABLE [dbo].[PassengerOrdersOld] (
    [PassengerOrderID]     INT      IDENTITY (1, 1) NOT NULL,
    [TravellingDate]       DATETIME NOT NULL,
    [RankID]               INT      NOT NULL,
    [TicketClassID]        INT      NOT NULL,
    [Going]                BIT      NOT NULL,
    [Returning]            BIT      NULL,
    [CreatedDate]          DATETIME NOT NULL,
    [LastUpdatedDate]      DATETIME NULL,
    [CreatedBy]            INT      NULL,
    [LastUpdatedBy]        INT      NULL,
    [PassengerOrderTypeID] INT      NULL,
    [DetailID]             INT      NOT NULL
);

