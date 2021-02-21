
CREATE FUNCTION [dbo].[FnGetChildren](@Parent INT)
RETURNS TABLE AS
RETURN (
		WITH EntityChildren AS
		(
			SELECT OrganizationsStructures.OrganizationID 
			FROM OrganizationsStructures
			WHERE OrganizationsStructures.OrganizationID = @Parent
			
			UNION ALL
			
			SELECT e.OrganizationID 
			FROM OrganizationsStructures e 
			INNER JOIN EntityChildren e2 ON e.OrganizationParentID = e2.OrganizationID
		)
	SELECT EntityChildren.OrganizationID 
	FROM EntityChildren
);

