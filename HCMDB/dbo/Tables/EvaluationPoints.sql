CREATE TABLE [dbo].[EvaluationPoints] (
    [EvaluationPointID]       INT           IDENTITY (1, 1) NOT NULL,
    [Evaluation]              NVARCHAR (50) NOT NULL,
    [EvaluationPoint]         FLOAT (53)    NOT NULL,
    [IsExcludedFromPromotion] BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EvaluationPoints] PRIMARY KEY CLUSTERED ([EvaluationPointID] ASC)
);


