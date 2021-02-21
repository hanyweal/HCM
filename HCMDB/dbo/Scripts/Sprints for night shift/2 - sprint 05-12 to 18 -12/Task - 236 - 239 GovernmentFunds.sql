GO
ALTER TABLE [dbo].GovernmentFunds
ADD GovernmentFundDeactiveReasonID INT NULL
ADD BankIPAN NVARCHAR(100) NULL
GO
CREATE TABLE [dbo].[GovernmentFundsDeactiveReasons](
	[GovernmentFundDeactiveReasonID] [int] IDENTITY(1,1) NOT NULL,
	[GovernmentFundDeactiveReasonName] [nvarchar](300) NOT NULL,
 CONSTRAINT [PK_GovernmentFundsDeactiveReasons] PRIMARY KEY CLUSTERED 
(
	[GovernmentFundDeactiveReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GovernmentFunds]  WITH CHECK ADD  CONSTRAINT [FK_GovernmentFunds_GovernmentFundsDeactiveReasons] FOREIGN KEY([GovernmentFundDeactiveReasonID])
REFERENCES [dbo].[GovernmentFundsDeactiveReasons] ([GovernmentFundDeactiveReasonID])
GO
INSERT INTO [dbo].[GovernmentFundsDeactiveReasons]
           ([GovernmentFundDeactiveReasonName])
     VALUES
           (N'تم سداد المبلغ')

GO
INSERT INTO [dbo].[GovernmentFundsDeactiveReasons]
           ([GovernmentFundDeactiveReasonName])
	VALUES
           (N'تم الإعفاء')
GO
 