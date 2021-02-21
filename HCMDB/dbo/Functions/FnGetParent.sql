CREATE FUNCTION [dbo].[FnGetParent](@OrganizationID int)
returns table as
RETURN (
		WITH EntityParent AS
		(
			SELECT OrganizationID , OrganizationParentID , OrganizationName
			FROM OrganizationsStructures 
			WHERE OrganizationID = @OrganizationID
			--ORDER BY ParentID
			
			UNION ALL
			
			SELECT e.OrganizationID , e.OrganizationParentID , e.OrganizationName
			FROM OrganizationsStructures e 
			INNER JOIN EntityParent e2 ON e.OrganizationID = e2.OrganizationParentID
			--ORDER BY E.ParentID
		)
	SELECT OrganizationID , OrganizationParentID , OrganizationName
	FROM EntityParent
);
