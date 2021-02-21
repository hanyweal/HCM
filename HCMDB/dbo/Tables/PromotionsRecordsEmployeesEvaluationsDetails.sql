CREATE TABLE [dbo].[PromotionsRecordsEmployeesEvaluationsDetails] (
    [PromotionRecordEmployeeEvaluationDetailID] INT            IDENTITY (1, 1) NOT NULL,
    [PromotionRecordEmployeeID]                 INT            NOT NULL,
    [EvaluationYear]                            INT            NOT NULL,
    [Evaluation]                                NVARCHAR (50)  NOT NULL,
    [Points]                                    DECIMAL (5, 2) NOT NULL,
    CONSTRAINT [PK_PromotionsRecordsEmployeesEvaluationsDetails] PRIMARY KEY CLUSTERED ([PromotionRecordEmployeeEvaluationDetailID] ASC),
    CONSTRAINT [FK_PromotionsRecordsEmployeesEvaluationsDetails_PromotionsRecordsEmployees] FOREIGN KEY ([PromotionRecordEmployeeID]) REFERENCES [dbo].[PromotionsRecordsEmployees] ([PromotionRecordEmployeeID]) ON DELETE CASCADE ON UPDATE CASCADE
);

