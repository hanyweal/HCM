CREATE VIEW [dbo].[vwEmployeesCareers]
AS
SELECT     dbo.vwEmployeesCodes.EmployeeCodeNo, dbo.vwEmployeesCodes.EmployeeIDNo, dbo.vwEmployeesCodes.EmployeeNameAr, 
                      dbo.OrganizationsStructures.OrganizationCode, dbo.OrganizationsStructures.OrganizationName, dbo.Ranks.RankName, dbo.Jobs.JobName, 
                      dbo.OrganizationsJobs.JobNo, dbo.vwEmployeesCodes.EmployeeCodeID, dbo.EmployeesCareersHistory.JoinDate, 
                      dbo.CareersHistoryTypes.CareerHistoryTypeName, dbo.CareersDegrees.CareerDegreeName, dbo.EmployeesCareersHistory.IsActive
FROM         dbo.OrganizationsJobs INNER JOIN
                      dbo.EmployeesCareersHistory ON dbo.OrganizationsJobs.OrganizationJobID = dbo.EmployeesCareersHistory.OrganizationJobID INNER JOIN
                      dbo.OrganizationsStructures ON dbo.OrganizationsJobs.OrganizationID = dbo.OrganizationsStructures.OrganizationID INNER JOIN
                      dbo.Ranks ON dbo.OrganizationsJobs.RankID = dbo.Ranks.RankID INNER JOIN
                      dbo.vwEmployeesCodes ON dbo.EmployeesCareersHistory.EmployeeCodeID = dbo.vwEmployeesCodes.EmployeeCodeID INNER JOIN
                      dbo.Jobs ON dbo.OrganizationsJobs.JobID = dbo.Jobs.JobID INNER JOIN
                      dbo.CareersHistoryTypes ON dbo.EmployeesCareersHistory.CareerHistoryTypeID = dbo.CareersHistoryTypes.CareerHistoryTypeID INNER JOIN
                      dbo.CareersDegrees ON dbo.EmployeesCareersHistory.CareerDegreeID = dbo.CareersDegrees.CareerDegreeID

GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "OrganizationsJobs"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 216
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EmployeesCareersHistory"
            Begin Extent = 
               Top = 6
               Left = 254
               Bottom = 125
               Right = 467
            End
            DisplayFlags = 280
            TopColumn = 9
         End
         Begin Table = "OrganizationsStructures"
            Begin Extent = 
               Top = 6
               Left = 505
               Bottom = 125
               Right = 698
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Ranks"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "vwEmployeesCodes"
            Begin Extent = 
               Top = 126
               Left = 249
               Bottom = 245
               Right = 475
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Jobs"
            Begin Extent = 
               Top = 126
               Left = 513
               Bottom = 245
               Right = 686
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CareersHistoryTypes"
            Begin Extent = 
               Top = 6
               Left = 736
               Bottom = 95
               Right =', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwEmployeesCareers';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' 943
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CareersDegrees"
            Begin Extent = 
               Top = 96
               Left = 736
               Bottom = 185
               Right = 920
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwEmployeesCareers';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vwEmployeesCareers';

