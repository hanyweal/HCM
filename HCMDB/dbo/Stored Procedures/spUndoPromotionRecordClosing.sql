
CREATE PROC [dbo].[spUndoPromotionRecordClosing] --303
(
		 @PromotionRecordID INT=287
)
AS
	DECLARE @PromotionRecordEmployeeID INT 
	DECLARE @Counter INT =0
	DECLARE TestCursor CURSOR LOCAL FAST_FORWARD FOR
 
	SELECT  PRE.PromotionRecordEmployeeID FROM PromotionsRecordsEmployees PRE WHERE PRE.PromotionRecordID=@PromotionRecordID AND PRE.PromotionRecordJobVacancyID IS NOT NULL
	OPEN TestCursor
	FETCH NEXT FROM TestCursor INTO @PromotionRecordEmployeeID 
	WHILE @@FETCH_STATUS = 0
		BEGIN
			--==================
			DECLARE @PromotionRecordJobVacancyID INT 
			DECLARE @NewEmployeeCareerHistoryID INT
			DECLARE @CurrentEmployeeCareerHistoryID INT
			DECLARE @CurrentOrganizationJobID INT
			DECLARE @JobVacancyOrganizationJobID INT
			SELECT @PromotionRecordJobVacancyID=PRE.PromotionRecordJobVacancyID,@NewEmployeeCareerHistoryID=PRE.NewEmployeeCareerHistoryID,@CurrentEmployeeCareerHistoryID=PRE.CurrentEmployeeCareerHistoryID FROM PromotionsRecordsEmployees PRE WHERE PRE.PromotionRecordEmployeeID=@PromotionRecordEmployeeID
			SELECT @CurrentOrganizationJobID=CURRENTEMPCARRER.OrganizationJobID FROM EmployeesCareersHistory CURRENTEMPCARRER WHERE CURRENTEMPCARRER.EmployeeCareerHistoryID=@CurrentEmployeeCareerHistoryID
			SELECT @JobVacancyOrganizationJobID=OJ.OrganizationJobID FROM PromotionsRecordsJobsVacancies OJ WHERE OJ.PromotionRecordJobVacancyID= @PromotionRecordJobVacancyID


			--SELECT  PRE.NewEmployeeCareerHistoryID FROM PromotionsRecordsEmployees PRE WHERE PRE.PromotionRecordEmployeeID=@PromotionRecordEmployeeID
			--SELECT  OJ.IsVacant ,OJ.IsReserved  FROM OrganizationsJobs OJ WHERE OJ.OrganizationJobID=@JobVacancyOrganizationJobID
			--SELECT  CURRENTEMPCARRER.IsActive  FROM EmployeesCareersHistory CURRENTEMPCARRER WHERE CURRENTEMPCARRER.EmployeeCareerHistoryID=@CurrentEmployeeCareerHistoryID
			--SELECT  OJ.IsVacant  FROM OrganizationsJobs OJ WHERE OJ.OrganizationJobID=@CurrentOrganizationJobID
			--SELECT * FROM EmployeesCareersHistory WHERE EmployeeCareerHistoryID=@NewEmployeeCareerHistoryID


			UPDATE PRE SET PRE.NewEmployeeCareerHistoryID=NULL FROM PromotionsRecordsEmployees PRE WHERE PRE.PromotionRecordEmployeeID=@PromotionRecordEmployeeID
			UPDATE OJ SET OJ.IsVacant=1,OJ.IsReserved=1 FROM OrganizationsJobs OJ WHERE OJ.OrganizationJobID=@JobVacancyOrganizationJobID
			UPDATE CURRENTEMPCARRER SET CURRENTEMPCARRER.IsActive=1 FROM EmployeesCareersHistory CURRENTEMPCARRER WHERE CURRENTEMPCARRER.EmployeeCareerHistoryID=@CurrentEmployeeCareerHistoryID
			UPDATE OJ SET OJ.IsVacant=0 FROM OrganizationsJobs OJ WHERE OJ.OrganizationJobID=@CurrentOrganizationJobID
			DELETE FROM EmployeesCareersHistory WHERE EmployeeCareerHistoryID=@NewEmployeeCareerHistoryID
			
			--==================
			FETCH NEXT FROM TestCursor INTO @PromotionRecordEmployeeID 
		END
	CLOSE TestCursor
	DEALLOCATE TestCursor
	UPDATE PR SET PR.PromotionRecordStatusID=3,PR.ClosingBy=NULL,PR.ClosingTime=NULL  FROM PromotionsRecords PR WHERE PR.PromotionRecordID=@PromotionRecordID
