CREATE TABLE [dbo].[InternshipScholarshipsTypes] (
    [InternshipScholarshipTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [InternshipScholarshipTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_InternshipScholarshipsTypes] PRIMARY KEY CLUSTERED ([InternshipScholarshipTypeID] ASC)
);

