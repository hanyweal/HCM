CREATE PROCEDURE [dbo].[spGetEvaluationByEmployeeCodeID]
(
	@EmployeeCodeID int = 22057
)
AS
BEGIN
	SELECT TOP 2  EmployeesEvaluations.EvaluationDegree,
				  EvaluationPoints.Evaluation,
				  MaturityYearsBalances.MaturityYear
	FROM EmployeesEvaluations  
	INNER JOIN EvaluationPoints ON EmployeesEvaluations.EvaluationPointID = EvaluationPoints.EvaluationPointID
	INNER JOIN MaturityYearsBalances ON EmployeesEvaluations.MaturityYearID = MaturityYearsBalances.MaturityYearID
	WHERE EmployeeCodeID = @EmployeeCodeID 
	ORDER BY CONVERT(INT, MaturityYearsBalances.MaturityYear) DESC
END
