
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetAssigningByAssigningID]
(
	@AssigningID int
)
AS
BEGIN

	DECLARE @OrganizationID int 

	SELECT @OrganizationID = Assignings.OrganizationID  FROM Assignings WHERE AssigningID = @AssigningID
	
	--SELECT * FROM dbo.FnGetParent(@OrganizationID) WHERE OrganizationID > 1

	DECLARE @combine nvarchar(max)
	SET @combine = ''

	SELECT @combine = OrganizationName + ' / ' + @combine FROM dbo.FnGetParent(@OrganizationID) WHERE OrganizationID > 1
	SET @combine = LTRIM(RTRIM(@combine))
	SET @combine = SUBSTRING(@combine, 0 , LEN(@combine)-1)

	SELECT	Assignings.AssigningID,
			Assignings.EmployeeCareerHistoryID,
			Assignings.AssigningTypID,
			Assignings.OrganizationID,
			Assignings.JobID,
			 Assignings.AssigningStartDate, 
			 mic_db.dbo.fn_GregToUmAlqura(Assignings.AssigningStartDate) AS AssigningStartDateUmAlQura,
			 Assignings.AssigningEndDate, 
			 mic_db.dbo.fn_GregToUmAlqura(Assignings.AssigningEndDate) AS AssigningEndDateUmAlQura,		 
--			Assignings.ExternalCity,
			Assignings.ExternalOrganization,
			Assignings.IsFinished,
			Assignings.CreatedDate,
			Assignings.CreatedBy,
			Assignings.LastUpdatedDate,
			Assignings.LastUpdatedBy, 
			AssigningsTypes.AssigningTypeName , 
			Jobs.JobName, 
			--OrganizationsStructures.OrganizationCode + ' - ' + OrganizationsStructures.OrganizationName as OrganizationName,
			OrganizationsStructures.OrganizationCode, 
			OrganizationsStructures.OrganizationName,
			@combine  as OrganizationFullName,
			EmployeesCareersHistory.EmployeeCodeID,
			mgr.EmployeeNameAr as ManagerNameAr
	FROM Assignings
	LEFT JOIN Jobs ON Assignings.JobID = Jobs.JobID
	LEFT JOIN OrganizationsStructures ON Assignings.OrganizationID = OrganizationsStructures.OrganizationID
	LEFT JOIN vwCurrentEmployeesCareer mgr ON mgr.EmployeeCodeID = Assignings.ManagerCodeID 
	INNER JOIN AssigningsTypes ON Assignings.AssigningTypID = AssigningsTypes.AssigningTypID
	INNER JOIN EmployeesCareersHistory ON EmployeesCareersHistory.EmployeeCareerHistoryID = Assignings.EmployeeCareerHistoryID
	WHERE AssigningID = @AssigningID
	
END
