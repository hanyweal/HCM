CREATE TABLE [dbo].[InternshipScholarships] (
    [InternshipScholarshipID]        INT            IDENTITY (1, 1) NOT NULL,
    [InternshipScholarshipTypeID]    INT            NOT NULL,
    [InternshipScholarshipStartDate] DATE           NOT NULL,
    [InternshipScholarshipEndDate]   DATE           NOT NULL,
    [InternshipScholarshipLocation]  NVARCHAR (100) NULL,
    [InternshipScholarshipReason]    NVARCHAR (500) NULL,
    [CountryID]                      INT            NULL,
    [KSACityID]                      INT            NULL,
    [IsActive]                       BIT            NOT NULL,
    [CreatedDate]                    DATETIME       CONSTRAINT [DF_InternshipScholarships_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate]                DATETIME       NULL,
    [CreatedBy]                      INT            NULL,
    [LastUpdatedBy]                  INT            NULL,
    CONSTRAINT [PK_InternshipScholarships] PRIMARY KEY CLUSTERED ([InternshipScholarshipID] ASC),
    CONSTRAINT [FK_InternshipScholarships_Countries] FOREIGN KEY ([CountryID]) REFERENCES [dbo].[Countries] ([CountryID]),
    CONSTRAINT [FK_InternshipScholarships_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_InternshipScholarships_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_InternshipScholarships_InternshipScholarshipsTypes] FOREIGN KEY ([InternshipScholarshipTypeID]) REFERENCES [dbo].[InternshipScholarshipsTypes] ([InternshipScholarshipTypeID]),
    CONSTRAINT [FK_InternshipScholarships_KSACities] FOREIGN KEY ([KSACityID]) REFERENCES [dbo].[KSACities] ([KSACityID])
);

