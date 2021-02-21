CREATE TABLE [dbo].[ChangeDetailsLogs] (
    [ChangeDetailLogID] INT            IDENTITY (1, 1) NOT NULL,
    [ChangeLogID]       INT            NOT NULL,
    [PropertyName]      NVARCHAR (255) NULL,
    [PrimaryKeyValue]   INT            NULL,
    [OldValue]          NVARCHAR (MAX) NULL,
    [NewValue]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ChangeDetailLog] PRIMARY KEY CLUSTERED ([ChangeDetailLogID] ASC),
    CONSTRAINT [FK_ChangeDetailsLogs_ChangeLogs] FOREIGN KEY ([ChangeLogID]) REFERENCES [dbo].[ChangeLogs] ([ChangeLogID]) ON DELETE CASCADE ON UPDATE CASCADE
);

