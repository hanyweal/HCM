CREATE TABLE [dbo].[BasicSalaries] (
    [BasicSalaryID]  INT IDENTITY (1, 1) NOT NULL,
    [CareerDegreeID] INT NOT NULL,
    [RankID]         INT NOT NULL,
    [BasicSalary]    INT NULL,
    CONSTRAINT [PK_BasicSalaries] PRIMARY KEY CLUSTERED ([BasicSalaryID] ASC),
    CONSTRAINT [FK__BasicSala__Caree__005FFE8A] FOREIGN KEY ([CareerDegreeID]) REFERENCES [dbo].[CareersDegrees] ([CareerDegreeID]),
    CONSTRAINT [FK__BasicSala__RankI__015422C3] FOREIGN KEY ([RankID]) REFERENCES [dbo].[Ranks] ([RankID]),
    CONSTRAINT [FK__BasicSalaries__CareesDegrees] FOREIGN KEY ([CareerDegreeID]) REFERENCES [dbo].[CareersDegrees] ([CareerDegreeID]) ON UPDATE CASCADE
);

