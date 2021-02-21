
CREATE Function [dbo].[fnGetCurrentQualification]			 
(
	@EmployeeCodeID int
)
Returns nvarchar(MAX)
BEGIN

	DECLARE @CurrentQualifications  nvarchar(MAX)

	SELECT @CurrentQualifications = ISNULL(QualificationsDegrees.QualificationDegreeName, '')  + ' - ' + ISNULL(Qualifications.QualificationName, '')
	FROM EmployeesQualifications AS eq LEFT OUTER JOIN
		Qualifications ON eq.QualificationID = Qualifications.QualificationID LEFT OUTER JOIN
		QualificationsDegrees ON eq.QualificationDegreeID = QualificationsDegrees.QualificationDegreeID
	WHERE eq.EmployeeCodeID = @EmployeeCodeID
	
	IF @CurrentQualifications is null
		SET @CurrentQualifications = ''
		
	RETURN @CurrentQualifications
END
