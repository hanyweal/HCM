CREATE TABLE [dbo].[PromotionsRecordsEmployeesSeniorityDetails] (
    [PromotionRecordEmployeeSeniorityDetailID] INT            IDENTITY (1, 1) NOT NULL,
    [PromotionRecordEmployeeID]                INT            NOT NULL,
    [Months]                                   INT            NOT NULL,
    [Points]                                   DECIMAL (5, 2) NOT NULL,
    CONSTRAINT [PK_PromotionsRecordsEmployeesSeniorityDetails] PRIMARY KEY CLUSTERED ([PromotionRecordEmployeeSeniorityDetailID] ASC),
    CONSTRAINT [FK_PromotionsRecordsEmployeesSeniorityDetails_PromotionsRecordsEmployees] FOREIGN KEY ([PromotionRecordEmployeeID]) REFERENCES [dbo].[PromotionsRecordsEmployees] ([PromotionRecordEmployeeID]) ON DELETE CASCADE
);

