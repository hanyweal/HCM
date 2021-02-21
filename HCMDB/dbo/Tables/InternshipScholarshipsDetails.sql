CREATE TABLE [dbo].[InternshipScholarshipsDetails] (
    [InternshipScholarshipDetailID] INT      IDENTITY (1, 1) NOT NULL,
    [InternshipScholarshipID]       INT      NOT NULL,
    [EmployeeCareerHistoryID]       INT      NOT NULL,
    [CreatedDate]                   DATETIME CONSTRAINT [DF_InternshipScholarshipsDetails_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]               DATETIME NULL,
    [CreatedBy]                     INT      NULL,
    [LastUpdatedBy]                 INT      NULL,
    [IsPassengerOrderCompensation]  BIT      CONSTRAINT [DF_InternshipScholarshipsDetails_IsPassengerOrderCompensation] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_InternshipScholarshipsDetails] PRIMARY KEY CLUSTERED ([InternshipScholarshipDetailID] ASC),
    CONSTRAINT [FK_InternshipScholarshipsDetails_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_InternshipScholarshipsDetails_InternshipScholarships] FOREIGN KEY ([InternshipScholarshipID]) REFERENCES [dbo].[InternshipScholarships] ([InternshipScholarshipID]) ON DELETE CASCADE ON UPDATE CASCADE
);

