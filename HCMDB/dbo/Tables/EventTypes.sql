CREATE TABLE [dbo].[EventTypes] (
    [EventTypeID] INT            NOT NULL,
    [Type]        NVARCHAR (255) NULL,
    CONSTRAINT [PK_EventTypes] PRIMARY KEY CLUSTERED ([EventTypeID] ASC)
);

