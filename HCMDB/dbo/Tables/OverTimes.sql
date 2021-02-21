CREATE TABLE [dbo].[OverTimes] (
    [OverTimeID]        INT             IDENTITY (1, 1) NOT NULL,
    [OverTimeStartDate] DATE            NOT NULL,
    [OverTimeEndDate]   DATE            NOT NULL,
    [WeekWorkHoursAvg]  FLOAT (53)      NOT NULL,
    [Tasks]             NVARCHAR (MAX)  NOT NULL,
    [CreatedDate]       DATETIME        NOT NULL,
    [LastUpdatedDate]   DATETIME        NULL,
    [CreatedBy]         INT             NULL,
    [LastUpdatedBy]     INT             NULL,
    [FridayHoursAvg]    FLOAT (53)      NULL,
    [SaturdayHoursAvg]  FLOAT (53)      NULL,
    [Requester]         NVARCHAR (1000) NULL,
    [IsApproved]        BIT             CONSTRAINT [DF__OverTimes__IsApp__3631FF56] DEFAULT ((0)) NOT NULL,
    [ApprovedBy]        INT             NULL,
    [ApprovedDate]      DATETIME        NULL,
    CONSTRAINT [PK_OverTimes] PRIMARY KEY CLUSTERED ([OverTimeID] ASC),
    CONSTRAINT [FK_OverTimes_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_OverTimes_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID])
);

