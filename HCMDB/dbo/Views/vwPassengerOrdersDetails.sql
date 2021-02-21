CREATE VIEW [dbo].[vwPassengerOrdersDetails]
AS
SELECT     ID, DetailID, EmployeeCareerHistoryID, StartDate, EndDate, Reason, Type
FROM         (SELECT     dbo.Delegations.DelegationID AS ID, dbo.DelegationsDetails.DelegationDetailID AS DetailID, dbo.DelegationsDetails.EmployeeCareerHistoryID, 
                                              dbo.Delegations.DelegationStartDate AS StartDate, dbo.Delegations.DelegationEndDate AS EndDate, dbo.Delegations.DelegationReason AS Reason, 
                                              1 AS Type
                        FROM         dbo.Delegations INNER JOIN
                                              dbo.DelegationsDetails ON dbo.Delegations.DelegationID = dbo.DelegationsDetails.DelegationID
                        UNION
                        SELECT     dbo.InternshipScholarships.InternshipScholarshipID AS ID, dbo.InternshipScholarshipsDetails.InternshipScholarshipDetailID AS DetailID, 
                                              dbo.InternshipScholarshipsDetails.EmployeeCareerHistoryID, dbo.InternshipScholarships.InternshipScholarshipStartDate AS StartDate, 
                                              dbo.InternshipScholarships.InternshipScholarshipEndDate AS EndDate, dbo.InternshipScholarships.InternshipScholarshipReason AS Reason, 
                                              2 AS Type
                        FROM         dbo.InternshipScholarships INNER JOIN
                                              dbo.InternshipScholarshipsDetails ON dbo.InternshipScholarships.InternshipScholarshipID = dbo.InternshipScholarshipsDetails.InternshipScholarshipID) 
                      AS v
