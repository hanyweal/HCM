CREATE PROCEDURE [dbo].[spGetQualificationByEmployeeCodeID]
(
	@EmployeeCodeID int
)
AS
BEGIN
	SELECT EmployeesQualifications.EmployeeCodeID, 
			QualificationsDegrees.QualificationDegreeName,
			Qualifications.QualificationName,
			GeneralSpecializations.GeneralSpecializationName 
	FROM EmployeesQualifications  
	LEFT JOIN QualificationsDegrees ON EmployeesQualifications.QualificationDegreeID = QualificationsDegrees.QualificationDegreeID
	LEFT JOIN Qualifications ON EmployeesQualifications.QualificationID = Qualifications.QualificationID
	LEFT JOIN GeneralSpecializations ON EmployeesQualifications.GeneralSpecializationID = GeneralSpecializations.GeneralSpecializationID
	WHERE EmployeesQualifications.EmployeeCodeID = @EmployeeCodeID
END
