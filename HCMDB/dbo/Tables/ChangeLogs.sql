CREATE TABLE [dbo].[ChangeLogs] (
    [ChangeLogID]    INT            IDENTITY (1, 1) NOT NULL,
    [EntityName]     NVARCHAR (255) NULL,
    [DateChange]     DATETIME       NULL,
    [EventTypeID]    INT            NULL,
    [EmployeeCodeID] INT            NULL,
    CONSTRAINT [PK_ChangeLog] PRIMARY KEY CLUSTERED ([ChangeLogID] ASC),
    CONSTRAINT [FK_ChangeLogs_EventTypes] FOREIGN KEY ([EventTypeID]) REFERENCES [dbo].[EventTypes] ([EventTypeID])
);

