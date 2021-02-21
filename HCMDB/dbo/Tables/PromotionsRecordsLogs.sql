CREATE TABLE [dbo].[PromotionsRecordsLogs] (
    [PromotionRecordLogID]        INT            IDENTITY (1, 1) NOT NULL,
    [PromotionRecordID]           INT            NOT NULL,
    [PromotionRecordNo]           NVARCHAR (50)  NOT NULL,
    [ActionDescription]           NVARCHAR (MAX) NOT NULL,
    [PromotionRecordActionTypeID] INT            NOT NULL,
    [ActionBy]                    INT            NOT NULL,
    [ActionDate]                  DATETIME       NOT NULL,
    CONSTRAINT [PK_PromotionsRecordsLogs] PRIMARY KEY CLUSTERED ([PromotionRecordLogID] ASC),
    CONSTRAINT [FK_PromotionsRecordsLogs_EmployeesCodes] FOREIGN KEY ([ActionBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_PromotionsRecordsLogs_PromotionsActionsTypes] FOREIGN KEY ([PromotionRecordActionTypeID]) REFERENCES [dbo].[PromotionsRecordsActionsTypes] ([PromotionActionTypeID])
);

