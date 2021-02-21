CREATE TABLE [dbo].[PassengerOrdersItineraries] (
    [PassengerOrdersItineraryID] INT            IDENTITY (1, 1) NOT NULL,
    [PassengerOrderID]           INT            NOT NULL,
    [FromCity]                   NVARCHAR (100) NULL,
    [ToCity]                     NVARCHAR (100) NULL,
    [CreatedDate]                DATETIME       CONSTRAINT [DF_PassengerOrdersItineraries_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]            DATETIME       NULL,
    CONSTRAINT [PK_PassengerOrdersItineraries] PRIMARY KEY CLUSTERED ([PassengerOrdersItineraryID] ASC),
    CONSTRAINT [FK_PassengerOrdersItineraries_PassengerOrders] FOREIGN KEY ([PassengerOrderID]) REFERENCES [dbo].[PassengerOrders] ([PassengerOrderID]) ON DELETE CASCADE ON UPDATE CASCADE
);

