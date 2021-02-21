CREATE TABLE [dbo].[AssigningsReasons] (
    [AssigningReasonID]   INT            IDENTITY (1, 1) NOT NULL,
    [AssigningReasonName] NVARCHAR (100) NULL,
    CONSTRAINT [PK_AssigningReasons] PRIMARY KEY CLUSTERED ([AssigningReasonID] ASC)
);

