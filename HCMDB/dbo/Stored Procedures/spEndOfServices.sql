CREATE PROCEDURE [dbo].[spEndOfServices]
AS
BEGIN
	/*	***************************
		SET IDENTITY_INSERT CareersHistoryTypes ON
		INSERT INTO CareersHistoryTypes (CareerHistoryTypeID, CareerHistoryTypeName) VALUES (3, N'EOS')
		SET IDENTITY_INSERT CareersHistoryTypes OFF	
		***************************		*/

	DECLARE @EndOfServiceID int, @EmployeeCareerHistoryID int, @OrganizationJobIDOLD int, @EndOfServiceDate date, @CreatedBy int	
	DECLARE @EmployeeCodeID int, @OrganizationJobID int
	DECLARE @CareerHistoryTypeID int = 3					-- EOS
	--DECLARE @IDs nvarchar(MAX) = '0'	
	
	DECLARE EOSCursor CURSOR LOCAL FAST_FORWARD FOR
	SELECT EOS.EndOfServiceID, EOS.EmployeeCareerHistoryID, EOS.EndOfServiceDate, ECH.EmployeeCodeID, EOS.CreatedBy, ECH.OrganizationJobID
	FROM EndOfServices EOS 
	INNER JOIN EmployeesCareersHistory ECH on ECH.EmployeeCareerHistoryID = EOS.EmployeeCareerHistoryID
	WHERE ISNULL(EOS.isProcessed, 0) = 0
	AND EOS.EndOfServiceDate <= GETDATE()
	OPEN EOSCursor
	FETCH NEXT FROM EOSCursor INTO @EndOfServiceID, @EmployeeCareerHistoryID, @EndOfServiceDate, @EmployeeCodeID, @CreatedBy, @OrganizationJobIDOLD
	WHILE @@FETCH_STATUS = 0
		BEGIN
		
			BEGIN TRY
				
				BEGIN TRANSACTION
				
					-- Create OrganizationsJobs record for EOS
					INSERT INTO OrganizationsJobs(JobNo, OrganizationID, JobID, RankID, JobKindID, IsVacant, IsReserved, CreatedBy) 
											SELECT 'EOS-'+ JobNo, OrganizationID, JobID, RankID, JobKindID, IsVacant, IsReserved, @CreatedBy 
												FROM OrganizationsJobs WHERE OrganizationJobID = @OrganizationJobIDOLD
					SET @OrganizationJobID = IDENT_CURRENT('OrganizationsJobs')
					--Print @OrganizationJobID 
					
					INSERT INTO EmployeesCareersHistory (EmployeeCodeID, CareerHistoryTypeID, OrganizationJobID, CareerDegreeID, JoinDate, TransactionStartDate, IsActive, CreatedBy)
											SELECT ECH1.EmployeeCodeID, @CareerHistoryTypeID, @OrganizationJobID, CareerDegreeID, @EndOfServiceDate, @EndOfServiceDate, 
												0 as IsActive, @CreatedBy 
												FROM EmployeesCareersHistory ECH1 WHERE EmployeeCareerHistoryID = @EmployeeCareerHistoryID

					UPDATE EmployeesCareersHistory SET IsActive = 0, LastUpdatedBy = @CreatedBy, LastUpdatedDate = GETDATE() WHERE EmployeeCareerHistoryID = @EmployeeCareerHistoryID
					UPDATE OrganizationsJobs SET IsVacant = 1, LastUpdatedBy = @CreatedBy, LastUpdatedDate = GETDATE() WHERE OrganizationJobID = @OrganizationJobIDOLD
					UPDATE EmployeesCodes SET IsActive = 0, LastUpdatedBy = @CreatedBy, LastUpdatedDate = GETDATE() WHERE EmployeeCodeID = @EmployeeCodeID
					
					--SET @IDs = @IDs + ',' + STR(@EndOfServiceID)
					UPDATE EndOfServices SET isProcessed = 1, LastUpdatedBy = @CreatedBy, LastUpdatedDate = GETDATE() WHERE EndOfServiceID = @EndOfServiceID
			
					--Print @EndOfServiceID
			
				COMMIT
				
			END TRY
			BEGIN CATCH
				ROLLBACK								
				PRINT 'error'
				RAISERROR('there is some error ...', 16, 1)
			END CATCH
			
			FETCH NEXT FROM EOSCursor INTO @EndOfServiceID, @EmployeeCareerHistoryID, @EndOfServiceDate, @EmployeeCodeID, @CreatedBy, @OrganizationJobIDOLD
		END
	CLOSE EOSCursor
	DEALLOCATE EOSCursor
	
	--EXECUTE('UPDATE EndOfServices SET isProcessed = 1 WHERE EndOfServiceID IN (' + @IDs + ')') 



END
