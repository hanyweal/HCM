ALTER TABLE EvaluationPoints ADD IsExcludedFromPromotion Bit not null default(0)
GO

UPDATE EvaluationPoints
SET IsExcludedFromPromotion = 1
WHERE EvaluationPointID in (4,5)
Go