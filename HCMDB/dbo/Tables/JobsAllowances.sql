CREATE TABLE [dbo].[JobsAllowances] (
    [JobAllowanceID]  INT      IDENTITY (1, 1) NOT NULL,
    [JobID]           INT      NOT NULL,
    [AllowanceID]     INT      NOT NULL,
    [IsActive]        BIT      NOT NULL,
    [CreatedDate]     DATETIME CONSTRAINT [DF_RanksAllowances_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdatedDate] DATETIME NULL,
    CONSTRAINT [PK_RanksAllowances] PRIMARY KEY CLUSTERED ([JobAllowanceID] ASC),
    CONSTRAINT [FK_JobsAllowances_Jobs] FOREIGN KEY ([JobID]) REFERENCES [dbo].[Jobs] ([JobID]),
    CONSTRAINT [FK_RanksAllowances_Allowances] FOREIGN KEY ([AllowanceID]) REFERENCES [dbo].[Allowances] ([AllowanceID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_JobsAllowances]
    ON [dbo].[JobsAllowances]([JobID] ASC, [AllowanceID] ASC);

