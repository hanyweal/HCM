
INSERT INTO [dbo].[BusinessCategories]
VALUES (8,N'طلبات الإجازة الإلكترونية','EVacationsRequests')
GO	

INSERT INTO [dbo].[BusinessCategories]
VALUES (9,N'الرواتب','Payrolls')
GO	

INSERT INTO [dbo].[BusinessSubCategories](BusinessSubCategoryID,BusinessSubCategoryAr,BusinessSubCategoryEn,BusinessCategoryID)
VALUES (31,N'قرار صاحب الصلاحية حيال طلب الإجازة','AuthorizedPersonDecision',8)
GO

INSERT INTO [dbo].[BusinessSubCategories](BusinessSubCategoryID,BusinessSubCategoryAr,BusinessSubCategoryEn,BusinessCategoryID)
VALUES (30,N'الراتب الأساسي للمتعاقدين','ContractorsBasicSalaries',9)
GO	
