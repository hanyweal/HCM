CREATE TABLE [dbo].[SMSLogs] (
    [SMSLogID]             INT            IDENTITY (1, 1) NOT NULL,
    [BusinssSubCategoryID] INT            NOT NULL,
    [DetailID]             INT            NOT NULL,
    [Message]              NVARCHAR (MAX) NULL,
    [MobileNo]             NVARCHAR (20)  NOT NULL,
    [CreatedDate]          DATETIME       NOT NULL,
    [CreatedBy]            INT            NOT NULL,
    CONSTRAINT [PK_SMSLogs] PRIMARY KEY CLUSTERED ([SMSLogID] ASC),
    CONSTRAINT [FK_SMSLogs_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
);

