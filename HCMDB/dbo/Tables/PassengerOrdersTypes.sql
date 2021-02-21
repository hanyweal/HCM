CREATE TABLE [dbo].[PassengerOrdersTypes] (
    [PassengerOrderTypeID]   INT           NOT NULL,
    [PassengerOrderTypeName] NVARCHAR (50) NULL,
    CONSTRAINT [PK_PassengerOrdersTypes] PRIMARY KEY CLUSTERED ([PassengerOrderTypeID] ASC)
);

