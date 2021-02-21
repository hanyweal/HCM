CREATE TABLE [dbo].[Teachers] (
    [TeacherID]               INT      IDENTITY (1, 1) NOT NULL,
    [EmployeeCareerHistoryID] INT      NOT NULL,
    [StartDate]               DATE     NOT NULL,
    [EndDate]                 DATE     NOT NULL,
    [CreatedDate]             DATETIME NOT NULL,
    [CreatedBy]               INT      NOT NULL,
    [LastUpdatedDate]         DATETIME NULL,
    [LastUpdatedBy]           INT      NULL,
    CONSTRAINT [PK_Teachers] PRIMARY KEY CLUSTERED ([TeacherID] ASC),
    CONSTRAINT [FK_Teachers_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_Teachers_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_Teachers_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
);

