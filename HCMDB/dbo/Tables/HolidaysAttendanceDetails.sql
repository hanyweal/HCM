CREATE TABLE [dbo].[HolidaysAttendanceDetails] (
    [HolidayAttendanceDetailID] INT      IDENTITY (1, 1) NOT NULL,
    [EmployeeCareerHistoryID]   INT      NOT NULL,
    [HolidayAttendanceID]       INT      NOT NULL,
    [CreateDate]                DATETIME NOT NULL,
    [LastUpdatedDate]           DATETIME NULL,
    [CreatedBy]                 INT      NULL,
    [LastUpdatedBy]             INT      NULL,
    CONSTRAINT [PK_HolidaysAttendanceDetails_1] PRIMARY KEY CLUSTERED ([HolidayAttendanceDetailID] ASC),
    CONSTRAINT [FK_HolidaysAttendanceDetails_EmployeesCareersHistory] FOREIGN KEY ([EmployeeCareerHistoryID]) REFERENCES [dbo].[EmployeesCareersHistory] ([EmployeeCareerHistoryID]),
    CONSTRAINT [FK_HolidaysAttendanceDetails_HolidaysAttendance] FOREIGN KEY ([HolidayAttendanceID]) REFERENCES [dbo].[HolidaysAttendance] ([HolidayAttendanceID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Unique]
    ON [dbo].[HolidaysAttendanceDetails]([EmployeeCareerHistoryID] ASC, [HolidayAttendanceID] ASC);

