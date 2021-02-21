CREATE PROCEDURE [dbo].[spGetEmployeeQualificationsByEmployeeCodeID]-- 22915
(
	@EmployeeCodeID int
)
AS
BEGIN	
	

	SELECT  EQ.EmployeeQualificationID,			
			QD.QualificationDegreeName,
			Q.QualificationName,
		    GS.GeneralSpecializationName,
		    ES.ExactSpecializationName,
			QT.QualificationTypeName,
			mic_db.dbo.fn_GregToUmAlqura(EQ.GraduationDate) AS GraduationDate,
			EQ.GraduationYear
			
	FROM EmployeesQualifications EQ
	INNER JOIN QualificationsDegrees QD ON  QD.QualificationDegreeID = EQ.QualificationDegreeID 
	INNER JOIN Qualifications Q ON Q.QualificationID = EQ.QualificationID	
	INNER JOIN GeneralSpecializations GS  ON GS.GeneralSpecializationID = EQ.GeneralSpecializationID
	LEFT JOIN ExactSpecializations  ES ON ES.ExactSpecializationID = EQ.ExactSpecializationID
	LEFT JOIN QualificationsTypes  QT ON QT.QualificationTypeID = EQ.QualificationTypeID
	WHERE EmployeeCodeID = @EmployeeCodeID
	ORDER BY EQ.EmployeeQualificationID
	
END
