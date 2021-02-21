
ALTER TABLE PromotionsRecordsEmployees ADD ByExamResult decimal(5,2) null

INSERT INTO PromotionsRecordsActionsTypes (PromotionActionTypeID, PromotionActionTypeName) VALUES (19 , N'تعديل درجة الإختبار التحصيلي')
INSERT INTO PromotionsRecordsActionsTypes (PromotionActionTypeID, PromotionActionTypeName) VALUES (20 , N'حذف درجات الإختبار التحصيلي ')
