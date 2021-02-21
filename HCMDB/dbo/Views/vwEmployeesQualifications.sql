CREATE VIEW [dbo].[vwEmployeesQualifications]
AS
SELECT     dbo.EmployeesQualifications.EmployeeCodeID, dbo.QualificationsDegrees.QualificationDegreeName, dbo.Qualifications.QualificationName, 
                      dbo.Qualifications.DirectPoints, dbo.Qualifications.IndirectPoints, dbo.EmployeesQualifications.QualificationDegreeID, dbo.EmployeesQualifications.QualificationID, 
                      dbo.EmployeesQualifications.GeneralSpecializationID, dbo.GeneralSpecializations.GeneralSpecializationName, dbo.QualificationsDegrees.Weight, 
                      dbo.EmployeesQualifications.GraduationYear, dbo.EmployeesQualifications.GraduationDate
FROM         dbo.EmployeesQualifications INNER JOIN
                      dbo.QualificationsDegrees ON dbo.EmployeesQualifications.QualificationDegreeID = dbo.QualificationsDegrees.QualificationDegreeID LEFT OUTER JOIN
                      dbo.GeneralSpecializations ON dbo.EmployeesQualifications.GeneralSpecializationID = dbo.GeneralSpecializations.GeneralSpecializationID FULL OUTER JOIN
                      dbo.Qualifications ON dbo.GeneralSpecializations.QualificationID = dbo.Qualifications.QualificationID AND 
                      dbo.EmployeesQualifications.QualificationID = dbo.Qualifications.QualificationID AND 
                      dbo.QualificationsDegrees.QualificationDegreeID = dbo.Qualifications.QualificationDegreeID
