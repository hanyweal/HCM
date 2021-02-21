CREATE TABLE [dbo].[MaritalStatus] (
    [MaritalStatusID]   INT           IDENTITY (1, 1) NOT NULL,
    [MaritalStatusName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_MaritalStatus] PRIMARY KEY CLUSTERED ([MaritalStatusID] ASC)
);

