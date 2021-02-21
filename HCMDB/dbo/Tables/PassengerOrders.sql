CREATE TABLE [dbo].[PassengerOrders] (
    [PassengerOrderID]        INT            IDENTITY (1, 1) NOT NULL,
    [TravellingDate]          DATETIME       NOT NULL,
    [RankID]                  INT            NOT NULL,
    [TicketClassID]           INT            NOT NULL,
    [Going]                   BIT            NOT NULL,
    [Returning]               BIT            NULL,
    [CreatedDate]             DATETIME       CONSTRAINT [DF_PassengerOrders_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]         DATETIME       NULL,
    [CreatedBy]               INT            NULL,
    [LastUpdatedBy]           INT            NULL,
    [EmployeeCareerHistoryID] INT            NULL,
    [Reason]                  NVARCHAR (500) NULL,
    CONSTRAINT [PK_PassengerOrders] PRIMARY KEY CLUSTERED ([PassengerOrderID] ASC),
    CONSTRAINT [FK_PassengerOrders_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_PassengerOrders_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PassengerOrders_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PassengerOrders_Ranks] FOREIGN KEY ([RankID]) REFERENCES [dbo].[Ranks] ([RankID]),
    CONSTRAINT [FK_PassengerOrders_TicketsClasses] FOREIGN KEY ([TicketClassID]) REFERENCES [dbo].[TicketsClasses] ([TicketClassID])
);

