CREATE TABLE [dbo].[EndOfServicesReasons] (
    [EndOfServiceReasonID] INT           NOT NULL,
    [EndOfServiceCaseID]   INT           NOT NULL,
    [EndOfServiceReason]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EndOfServicesReasons] PRIMARY KEY CLUSTERED ([EndOfServiceReasonID] ASC),
    CONSTRAINT [FK_EndOfServicesReasons_EndOfServicesCases] FOREIGN KEY ([EndOfServiceCaseID]) REFERENCES [dbo].[EndOfServicesCases] ([EndOfServiceCaseID])
);

