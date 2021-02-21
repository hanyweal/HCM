CREATE PROCEDURE [dbo].[spGetQualificationsByPromotionRecordID]
(
	@PromotionRecordID int	
)
AS
BEGIN
	SELECT  QualificationsDegrees.QualificationDegreeName,
			Qualifications.QualificationName,
			GeneralSpecializations.GeneralSpecializationName,
			PromotionsRecordsQualificationsKinds.PromotionRecordQualificationKindName,
			PromotionsRecordsQualificationsPoints.Points
	FROM PromotionsRecordsQualificationsPoints
	LEFT JOIN QualificationsDegrees ON PromotionsRecordsQualificationsPoints.QualificationDegreeID = QualificationsDegrees.QualificationDegreeID
	LEFT JOIN Qualifications ON PromotionsRecordsQualificationsPoints.QualificationID = Qualifications.QualificationID
	LEFT JOIN GeneralSpecializations ON PromotionsRecordsQualificationsPoints.GeneralSpecializationID = GeneralSpecializations.GeneralSpecializationID
	LEFT JOIN PromotionsRecordsQualificationsKinds ON PromotionsRecordsQualificationsKinds.PromotionRecordQualificationKindID = PromotionsRecordsQualificationsPoints.PromotionRecordQualificationKindID
	WHERE PromotionRecordID = @PromotionRecordID
	ORDER BY QualificationsDegrees.[Weight] DESC,
			Qualifications.QualificationName
END
