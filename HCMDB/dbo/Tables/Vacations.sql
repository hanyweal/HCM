CREATE TABLE [dbo].[Vacations] (
    [VacationID]                INT      IDENTITY (1, 1) NOT NULL,
    [EmployeeCareerHistoryID]   INT      NOT NULL,
    [SickVacationTypeID]        INT      NULL,
    [VacationTypeID]            INT      NOT NULL,
    [VacationStartDate]         DATE     NOT NULL,
    [VacationEndDate]           DATE     NOT NULL,
    [StudiesVacationStartDate]  DATE     NULL,
    [IsCanceled]                BIT      NOT NULL,
    [OldBalanceConsumed]        INT      NULL,
    [InAdvConsumed]             INT      NULL,
    [HolidayAttendanceDetailID] INT      NULL,
    [CreatedDate]               DATETIME NOT NULL,
    [CreatedBy]                 INT      NOT NULL,
    [LastUpdatedDate]           DATETIME NULL,
    [LastUpdatedBy]             INT      NULL,
    [SportsSeasonID]            INT      NULL,
    [NormalVacationTypeID]      INT      NULL,
    [IsSickLeaveAmountPaid]     BIT      CONSTRAINT [DF_Vacations_IsSickLeaveAmountPaid] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Vacations] PRIMARY KEY CLUSTERED ([VacationID] ASC),
    CONSTRAINT [FK_Vacations_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_Vacations_EmployeesCodes] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_Vacations_EmployeesCodes1] FOREIGN KEY ([LastUpdatedBy]) REFERENCES [dbo].[EmployeesCodes] ([EmployeeCodeID]),
    CONSTRAINT [FK_Vacations_HolidaysAttendanceDetails] FOREIGN KEY ([HolidayAttendanceDetailID]) REFERENCES [dbo].[HolidaysAttendanceDetails] ([HolidayAttendanceDetailID]),
    CONSTRAINT [FK_Vacations_NormalVacationsTypes] FOREIGN KEY ([NormalVacationTypeID]) REFERENCES [dbo].[NormalVacationsTypes] ([NormalVacationTypeID]),
    CONSTRAINT [FK_Vacations_SickVacationsTypes] FOREIGN KEY ([SickVacationTypeID]) REFERENCES [dbo].[SickVacationsTypes] ([SickVacationTypeID]),
    CONSTRAINT [FK_Vacations_SportsSeasons] FOREIGN KEY ([SportsSeasonID]) REFERENCES [dbo].[SportsSeasons] ([SportsSeasonID]),
    CONSTRAINT [FK_Vacations_VacationsTypes] FOREIGN KEY ([VacationTypeID]) REFERENCES [dbo].[VacationsTypes] ([VacationTypeID]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [Employee]
    ON [dbo].[Vacations]([EmployeeCareerHistoryID] ASC);


GO
ALTER INDEX [Employee]
    ON [dbo].[Vacations] DISABLE;


GO
CREATE NONCLUSTERED INDEX [VacationStartDate]
    ON [dbo].[Vacations]([VacationStartDate] ASC);


GO
ALTER INDEX [VacationStartDate]
    ON [dbo].[Vacations] DISABLE;


GO
CREATE NONCLUSTERED INDEX [VacationType]
    ON [dbo].[Vacations]([VacationTypeID] ASC);


GO
ALTER INDEX [VacationType]
    ON [dbo].[Vacations] DISABLE;

