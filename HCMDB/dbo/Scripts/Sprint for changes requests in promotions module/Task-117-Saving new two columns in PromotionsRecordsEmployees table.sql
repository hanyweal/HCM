Alter table PromotionsRecordsEmployees
add CurrentJobJoinDate date

Alter table PromotionsRecordsEmployees
add PreviousJobJoinDate date

-- Update CurrentJobJoinDate
UPDATE pre1 
SET pre1.CurrentJobJoinDate = ech.JoinDate
FROM  PromotionsRecordsEmployees pre1
INNER JOIN EmployeesCareersHistory ech ON ech.EmployeeCareerHistoryID = pre1.CurrentEmployeeCareerHistoryID

-- Update PreviousJobJoinDate
UPDATE pre1
SET pre1.PreviousJobJoinDate = ech.JoinDate
FROM  PromotionsRecordsEmployees pre1
INNER JOIN EmployeesCareersHistory ech ON ech.EmployeeCareerHistoryID = pre1.PreviousEmployeeCareerHistoryID
WHERE pre1.PreviousEmployeeCareerHistoryID IS NOT NULL

